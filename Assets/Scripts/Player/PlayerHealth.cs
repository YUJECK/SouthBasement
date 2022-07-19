using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health
{
    [Header("Другое")]
    private bool invisibleCadrs = false;
    public Animator healthBar;
    [HideInInspector] public GameManager gameManager;
    [HideInInspector] public AudioManager audioManager;
    
    //Методы управления
    public override void Heal(int heal)
    {
        health += heal;

        if (health > maxHealth)
            health = maxHealth;
        onHealthChange.Invoke(health, maxHealth);
    }
    public override void SetHealth(int newMaxHealth, int newHealth)
    {
        maxHealth = newMaxHealth;
        if (health >= maxHealth) health = maxHealth;

        newHealth -= health;
        if (newHealth < 0)
            TakeHit(-newHealth);
        else if (newHealth > 0)
            Heal(newHealth);
    }
    public override void TakeHit(int damage, float stunDuration = 0)
    {
        if (!invisibleCadrs)
        {
            health -= damage;
            audioManager.PlayClip("RatHurt");
            StartCoroutine(TakeHitVizualization());

            invisibleCadrs = true;
            healthBar.SetBool("InvisibleCadrs", true);
            StartCoroutine(InvisibleCadrs());

            onHealthChange.Invoke(health, maxHealth);

            if (health <= 0)
            {
                Destroy(gameObject);
                SceneManager.LoadScene("RestartMenu");
            }
        }
    }
    
    //Юнитивские методы
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        effectManager = FindObjectOfType<EffectsInfo>();
        audioManager = FindObjectOfType<AudioManager>();
        onHealthChange.Invoke(health, maxHealth);
        GetEffect(10f, new EffectStats(3, 4), EffectsList.Poison);
    }
    public void Update() { if(effects.GetPersistentEventCount() != 0) effects.Invoke(); }

    public IEnumerator InvisibleCadrs()
    {
        yield return new WaitForSeconds(1f);
        invisibleCadrs = false;
        healthBar.SetBool("InvisibleCadrs", false);
    }
}