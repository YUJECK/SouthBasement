﻿using UnityEngine;

namespace SouthBasement.Extensions
{
    public static class GameObjectExtensions
    {
        public static TComponent Get<TComponent>(this GameObject gameObject)
        {
            if (gameObject.TryGetComponent<TComponent>(out var component))
                return component;

            return gameObject.GetComponentInChildren<TComponent>();
        }
    }
}