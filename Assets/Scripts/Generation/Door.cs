using EnemysAI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject doors;
    private bool isClosed = false;
    private int enemysCount = 0;
    public UnityEvent onClose = new UnityEvent();
    public UnityEvent onOpen = new UnityEvent();

    public void IncreaseEnemysCount() => enemysCount++;
    public void ReduceEnemysCount()
    {
        enemysCount--;
        if (enemysCount <= 0)
        {
            OpenDoors();
            Destroy(gameObject);
        }
    }
    public bool IsClosed => isClosed;
    public void CloseDoors()
    {
        if(enemysCount > 0)
        {
            doors.SetActive(true); isClosed = true;
            for (int i = 0; i < enemysCount; i++)
                transform.GetChild(i).GetComponent<EnemyAI>().WakeUp();
        }
    }
    public void OpenDoors() { doors.SetActive(false); isClosed = false; }

    private void Start()
    {
        enemysCount = transform.childCount;
        for (int i = 0; i < enemysCount; i++)
            transform.GetChild(i).GetComponent<EnemyHealth>().onDie.AddListener(ReduceEnemysCount);
    }
}