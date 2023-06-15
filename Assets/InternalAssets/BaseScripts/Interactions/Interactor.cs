﻿using System.Collections.Generic;
using TheRat.InputServices;
using UnityEngine;
using Zenject;

namespace TheRat.Interactions
{
    public sealed class Interactor : MonoBehaviour
    {
        private readonly List<IInteractive> _availableInteractions = new();

        [Inject]
        private void Construct(IInputService inputService)
        {
            inputService.OnInteracted += Interact;
        }
        
        private void Interact()
        {
            foreach (var interactive in _availableInteractions)
                interactive.Interact();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IInteractive interactive))
            {
                interactive.Detect();
                _availableInteractions.Add(interactive);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IInteractive interactive))
            {
                if (_availableInteractions.Contains(interactive))
                {
                    interactive.DetectionReleased();
                    _availableInteractions.Remove(interactive);
                }
            }
        }
    }
}