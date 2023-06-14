using System;
using UnityEngine;

namespace TheRat.Characters
{
    public interface IMovable
    {
        public event Action<Vector2> OnMoved;
        public event Action OnMoveReleased;

        public bool CanMove { get; set; }
        public Vector2 Movement { get; }
    }
}