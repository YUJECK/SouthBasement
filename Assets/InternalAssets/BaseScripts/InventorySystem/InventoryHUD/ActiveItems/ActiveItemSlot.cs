﻿using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SouthBasement.InventorySystem
{
    [RequireComponent(typeof(Image))]
    [AddComponentMenu("HUD/Inventory/ActiveItemSlot")]
    public sealed class ActiveItemSlot : InventorySlot<ActiveItem>
    {
        private ActiveItemUsage _activeItemUsage;
        [SerializeField] private GameObject isCurrent;

        [Inject]
        private void Construct(ActiveItemUsage activeItemUsage)
        {
            _activeItemUsage = activeItemUsage;
            _activeItemUsage.OnSelected += CheckCurrent;
        }

        private void CheckCurrent(ActiveItem item)
        {
            if (CurrentItem == null || item == null)
            {
                isCurrent.SetActive(false);
                return;
            }

            if (item.ItemID == CurrentItem.ItemID)
                isCurrent.SetActive(true);
            else
                isCurrent.SetActive(false);
        }

        private void Awake()
        {
            ItemImage = GetComponent<Image>();
            GetComponentInParent<Button>().onClick.AddListener(SetCurrent);
            
            OnSetted += CheckCurrent;
            SetItem(null);
        }

        private void SetCurrent()
        {
            if(CurrentItem == null)
                return;
            
            _activeItemUsage.SetCurrent(CurrentItem);
        }
    }
}