using RimuruDev.Mechanics.Character;
using UnityEngine;
using UnityEngine.UI;

public class MelleWeaponSlot : MonoBehaviour
{
    public MelleRangeWeapon melleWeapon;   // Предмет который лежит в этом слоте
    public GameObject objectOfItem;    // Гейм обжект этого предмета
    [SerializeField] private Image slotIcon;   // Иконка предмета который лежит в этом слоте
    public bool isEmpty; // Используется ли этот слот сейчас
    public bool isActiveSlot; // Используется ли этот слот сейчас

    public void Add(MelleRangeWeapon newMelleWeapon, GameObject _objectOfItem) // Добавеление предмета
    {
        if(melleWeapon != null)
            Drop();

        melleWeapon = newMelleWeapon;
        objectOfItem = _objectOfItem;
        isEmpty = false;
        slotIcon.sprite = newMelleWeapon.spriteInInventory;
    }
    public void Drop() // Выброс предмета в игре
    {
        if(objectOfItem != null)
        {
            objectOfItem.SetActive(true);
            objectOfItem.transform.position = FindObjectOfType<RatCharacterData>().GetComponent<Transform>().position;
        }
        Remove();
    }
    public void Remove() // Удаление предмета из слота
    {
        melleWeapon = null;
        objectOfItem.GetComponent<DontDestroyOnLoadNextScene>().Disable();
        objectOfItem.GetComponent<ItemInfo>().SetActive(true); //Этот метод отключает спрайт и триггер прелмета
        objectOfItem = null;
        isEmpty = true;
        slotIcon.sprite = FindObjectOfType<GameManager>().hollowSprite;
    }
}