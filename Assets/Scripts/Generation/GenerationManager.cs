using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Generation
{
    [RequireComponent(typeof(RoomsLists))]
    public class GenerationManager : MonoBehaviour
    {
        [SerializeField] private string locationName = "Basement";
        [Header("Настройки комнат")]
        private List<Room> rooms = new List<Room>();
        [SerializeField] private int roomsCount = 10;
        [SerializeField] private int passagesCount = 2;
        
        [Header("Настройка НПС")]
        [SerializeField] private int npcRoomsCount = 1;
        [SerializeField] private int boxesOnLevel = 1;
        [SerializeField] private bool isTraderWillSpawn = true;

        [Header("События")]
        public UnityEvent afterSpawned = new UnityEvent();
        private bool isRoomsSpawned = false;

        //Другое
        public float roomSpawnOffset = 0.05f;
        private Rooms[] roomsMap;
        private RoomsLists roomsLists;

        //Геттеры, сеттеры
        public string LocationName => locationName;
        public Rooms[] RoomsMap => roomsMap;
        public RoomsLists RoomsLists => roomsLists;  
        public bool IsSpawned => isRoomsSpawned; 
        public void SetIsSpawned() { if (!isRoomsSpawned) { isRoomsSpawned = true; afterSpawned.Invoke(); Debug.Log("[Info]: Rooms have been spawned"); } }
        public int AllRoomsCount => roomsCount + npcRoomsCount + boxesOnLevel + 1; 
        public int NowSpawnedRoomsCount => rooms.Count; 
        public void AddRoomToList(Room newRoom) => rooms.Add(newRoom); 
        public void RemoveRoomFromList(Room removableRoom) => rooms.Remove(removableRoom); 

        //Другое
        private void GenerateRoomsMap()
        {
            roomsMap = new Rooms[AllRoomsCount];

            //Просто комнаты с НПС
            if (RoomsLists.GetRoomsList(Rooms.NPC).Count != 0) for (int i = 0; i < npcRoomsCount; i++)
                {
                    int tmp = Random.Range(0, roomsMap.Length - 1);

                    if (roomsMap[tmp] == Rooms.Default) roomsMap[tmp] = Rooms.NPC;
                    else
                    {
                        i--;
                        Utility.ChechNumber(ref i, 0, 0, Utility.CheckNumber.Less);
                    }
                }
            //Торговец
            if (isTraderWillSpawn && RoomsLists.GetRoomsList(Rooms.Trader).Count != 0) for (int i = 0; i < 1; i++)
            {
                int traderRoomIndex = Random.Range(0, roomsMap.Length - 1);
                if (roomsMap[traderRoomIndex] == Rooms.Default) roomsMap[traderRoomIndex] = Rooms.Trader;
                else i--;
            }
            //Коробка
            if (RoomsLists.GetRoomsList(Rooms.Box).Count != 0) for (int i = 0; i < boxesOnLevel; i++)
            {
                int boxRoomIndex = Random.Range(0, roomsMap.Length - 1);
                if (roomsMap[boxRoomIndex] == Rooms.Default) roomsMap[boxRoomIndex] = Rooms.Box;
                else i--;
            }
            //Обязательные комнаты 
            for (int i = 0; i < RoomsLists.GetRoomsList(Rooms.MustSpawn).Count; i++)
            {
                int tmp = Random.Range(0, roomsMap.Length - 1);

                if (roomsMap[tmp] == Rooms.Default) roomsMap[tmp] = Rooms.MustSpawn;
                else
                {
                    i--;
                    Utility.ChechNumber(ref i, 0, 0, Utility.CheckNumber.Less);
                }
            }
            //Комнаты коридоры
            for (int i = 0; i < passagesCount; i++)
            {
                int tmp = Random.Range(0, roomsMap.Length - 5);

                if (roomsMap[tmp] == Rooms.Default) roomsMap[tmp] = Rooms.Passage;
                else
                {
                    i--;
                    Utility.ChechNumber(ref i, 0, 0, Utility.CheckNumber.Less);
                }
            }
            //Выход
            roomsMap[roomsMap.Length - 1] = Rooms.Exit;
        }
        
        //Юнитивские методы
        private void Awake()
        {
            roomsLists = GetComponent<RoomsLists>();
            GenerateRoomsMap();
        }
    }
}