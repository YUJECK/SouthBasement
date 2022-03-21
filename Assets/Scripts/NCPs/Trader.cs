using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trader : MonoBehaviour
{
    private GameManager gameManager;
    bool isOnTrigger;
    public DialogueManager manager;
    
    [Header("ItemSlots")]
    public int boxesForItems = 3;
    public Transform item1Transform;
    public Transform item2Transform;
    public Transform item3Transform;

    [Header("Sentences")]
    public bool isTraderTalking = false;
    [TextArea(1,3)]
    public string SentenceNotEnoghtMoney;
    [TextArea(1, 3)]
    public string FirstMeetingTraderSentence;
    public List<GameObject> itemsForTrade;
    
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        GameObject item_1 = gameManager.items[Random.Range(0,gameManager.items.Count)];
        gameManager.items.Remove(item_1);
        SetItemForTrade(item_1);
        Instantiate(item_1,item1Transform.position,Quaternion.identity,item1Transform);

        GameObject item_2 = gameManager.items[Random.Range(0,gameManager.items.Count)];
        gameManager.items.Remove(item_2);
        SetItemForTrade(item_2);
        Instantiate(item_2,item2Transform.position,Quaternion.identity,item2Transform);

        GameObject item_3 = gameManager.items[Random.Range(0,gameManager.items.Count)];
        gameManager.items.Remove(item_3);
        SetItemForTrade(item_3);
        Instantiate(item_3,item3Transform.position,Quaternion.identity,item3Transform);
    }
    
    public void DisplayFrase(string frase)
    {
        manager.DisplayText(frase);
    }

    private void SetItemForTrade(GameObject item)
    {
        if(item.TryGetComponent(out FoodPickUp food))
        {
            food.isForTrade = true;
            food.trader = GetComponent<Trader>();
            return;
        }
        if(item.TryGetComponent(out ActiveItemPickUp activeItem))
        {
            activeItem.isForTrade = true;
            activeItem.trader = GetComponent<Trader>();
            return;
        }
        if(item.TryGetComponent(out FoodPickUp melleRange))
        {
            melleRange.isForTrade = true;
            melleRange.trader = GetComponent<Trader>();
            return;
        }
    }

    private IEnumerator NotEnoghtMoney()
    {
        isTraderTalking = true;
        DisplayFrase(SentenceNotEnoghtMoney);
        yield return new WaitForSeconds(3f);
        isTraderTalking = false;
    }
}