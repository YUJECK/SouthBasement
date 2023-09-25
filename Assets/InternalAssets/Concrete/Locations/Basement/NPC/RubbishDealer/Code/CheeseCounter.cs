using System;
using System.Linq;
using SouthBasement.InventorySystem;
using SouthBasement.InventorySystem.ItemBase;
using TMPro;
using UnityEngine;
using Zenject;

namespace SouthBasement
{
    public class CheeseCounter : MonoBehaviour
    {
        private Inventory _inventory;
        private TMP_Text _text;

        [Inject]
        private void Construct(Inventory inventory)
        {
            _inventory = inventory;
        }

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
        }

        private void OnEnable()
        {
            var container = _inventory.ItemsContainer.GetAllInContainer<RubbishItem>();

            var cheeseCount 
                = container.Sum(rubbish => rubbish.ItemPrice);

            _text.text = cheeseCount.ToString();
        }
    }
}