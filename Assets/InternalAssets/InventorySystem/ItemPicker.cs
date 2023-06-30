using System.Collections;
using SouthBasement.Interactions;
using SouthBasement.Helpers;
using UnityEngine;
using Zenject;

namespace SouthBasement.InventorySystem
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class ItemPicker : MonoBehaviour, IInteractive
    {
        [SerializeField] private Item item;

        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rigidbody2D;
        
        private Inventory _inventory;
        private DiContainer _diContainer;
        private MaterialHelper _materialHelper;

        [Inject]
        private void Construct(Inventory inventory, MaterialHelper materialHelper, DiContainer diContainer)
        {
            _diContainer = diContainer;
            _inventory = inventory;
            _materialHelper = materialHelper;
        }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rigidbody2D = GetComponent<Rigidbody2D>();

            if(item != null)
                SetItem(item);
        }

        public void PlayMove(Vector2 move, float speed)
        {
            StartCoroutine(Move(move, speed));
        }

        private IEnumerator Move(Vector2 move, float speed)
        {
            Vector3 endPosition = transform.position + new Vector3(move.x, move.y, 0f);
            
            while (transform.position != endPosition)
            {
                Vector2 currentMove = Vector2.MoveTowards(transform.position, endPosition, Time.deltaTime * speed); 
                _rigidbody2D.MovePosition(currentMove);
                
                yield return new WaitForFixedUpdate();
            }
        }

        public virtual void SetItem(Item item)
        {
            this.item = item;
            
            gameObject.name = item.ItemID;
            _spriteRenderer.sprite = this.item.ItemSprite;
            _diContainer.Inject(item);
        }

        public void Detect()
        {
            _spriteRenderer.material = _materialHelper.OutlineMaterial;
        }

        public virtual void Interact()
        {
            if(_inventory.TryAddItem(item))            
                Destroy(gameObject);
        }

        public virtual void DetectionReleased()
        {
            _spriteRenderer.material = _materialHelper.DefaultMaterial;
        }
    }
}