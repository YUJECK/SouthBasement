using NaughtyAttributes;
using SouthBasement.Generation;
using UnityEngine;

namespace SouthBasement
{
    [RequireComponent(typeof(PassageHandler))]
    public class RoomInspectorController : MonoBehaviour
    {
        [SerializeField] private bool upPassage;
        [SerializeField] private bool downPassage;
        [SerializeField] private bool leftPassage;
        [SerializeField] private bool rightPassage;
        
        private PassageHandler _passageHandler;

        private void OnValidate()
        {
            if (_passageHandler.Contains(Direction.Up))
            {
                if(upPassage) _passageHandler.GetPassage(Direction.Up).EnablePassage();
                else _passageHandler.GetPassage(Direction.Up).DisablePassage();
            }

            if (_passageHandler.Contains(Direction.Down))
            {
                if (downPassage) _passageHandler.GetPassage(Direction.Down).EnablePassage();
                else _passageHandler.GetPassage(Direction.Down).DisablePassage();
            }

            if (_passageHandler.Contains(Direction.Left))
            {
                if (leftPassage) _passageHandler.GetPassage(Direction.Left).EnablePassage();
                else _passageHandler.GetPassage(Direction.Left).DisablePassage();
            }

            if (_passageHandler.Contains(Direction.Right))
            {
                if (rightPassage) _passageHandler.GetPassage(Direction.Right).EnablePassage();
                else _passageHandler.GetPassage(Direction.Right).DisablePassage();
            }
        }
        private void Reset()
            => _passageHandler = GetComponentInChildren<PassageHandler>();

        [Button()]
        private void EnableDoors() 
            => _passageHandler.CloseAllDoors();
        
        [Button()]
        private void DisableDoors() 
            => _passageHandler.OpenAllDoors();
    }
}
