using UnityEngine;
using System.Collections.Generic;
using RimuruDev.Mechanics.Character;

public class RatAttack : MonoBehaviour
{
    public MelleRangeWeapon melleWeapon;
    public PointRotation pointRotation;
    public SpriteRenderer weaponSprite;
    public Animator playerAnim;
    public Animator animRange;
    public Transform AttackPoint;
    private float nextTime;

    [Header("Attack settings")]
    public LayerMask EnemyLayers;
    public float AttackRange;
    public float attackRate = 2f;
    public int damage = 2;
    public int damageBoost = 2;
    public bool is_Attack;
    private Vector3 posWhenAttack;

    private RatCharacterData player;

    private void Start()
    {
        pointRotation = FindObjectOfType<PointRotation>();
        player = FindObjectOfType<RatCharacterData>();
    }

    void Update()
    {
        if(Time.time >= nextTime) // Атакак крысы
        {
            if (Input.GetMouseButtonDown(0) & !FindObjectOfType<RatCharacterData>().isSprinting)
            {
                Attack();
                // cursor.CursorClick();
                nextTime = Time.time + 1f / attackRate;
            }
        }
        if(Time.time >= nextTime)
        {
            is_Attack = false;  
            pointRotation.stopRotating = false;
        }
    }
    void Attack()
    {
        is_Attack = true;
        pointRotation.stopRotating = true;
        playerAnim.SetTrigger("IsAttack");
        animRange.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position,AttackRange,EnemyLayers);
        
        foreach (Collider2D enemy in hitEnemies)
        {
            if(enemy.tag == "Enemy")
            {
                if (!enemy.isTrigger)
                {
                    if(melleWeapon != null)
                    {
                        if(melleWeapon.effect == MelleRangeWeapon.Effect.Poisoned)
                            enemy.GetComponent<HealthEnemy>().GetPoisoned(melleWeapon.effectTime);
                        if(melleWeapon.effect == MelleRangeWeapon.Effect.Bleed)
                            enemy.GetComponent<HealthEnemy>().GetBleed(melleWeapon.effectTime);
                        if(melleWeapon.effect == MelleRangeWeapon.Effect.Burn)
                            enemy.GetComponent<HealthEnemy>().GetBurn(melleWeapon.effectTime);
                    }
                    enemy.GetComponent<HealthEnemy>().TakeHit(damage+damageBoost);
                    Debug.Log("Enemy health: " + enemy.GetComponent<HealthEnemy>().health);
                }
            }
        }
    }
    public void SetMelleWeapon(MelleRangeWeapon weapon)
    {
        melleWeapon = weapon;
        AttackRange = weapon.attackRange;
        attackRate = weapon.attackRate;
        damage = weapon.damage;
        weaponSprite.sprite = weapon.sprite;
        AttackPoint.localScale = new Vector3(10*weapon.attackRange/2, 10*weapon.attackRange/2, 1);
    }

    public void HideMelleweaponIcon(bool hiding) // Включние, выключение спрайта оружия
    {   
        weaponSprite.gameObject.SetActive(!hiding);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }
}
