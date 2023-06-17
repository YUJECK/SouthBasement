﻿using System;
using TheRat.Weapons;
using UnityEngine;

namespace TheRat
{
    [Serializable]
    public sealed class CharacterStatsConfig
    {
        [field: SerializeField] public WeaponStats WeaponStats { get; set; } = new();
        [field: SerializeField] public float DefaultMoveSpeed { get; private set; } = 5f;
        [field: SerializeField] public int DefaultStamina { get; private set; } = 100;
        
        [field: SerializeField] public int DefaultMaximumHealth { get; private set; } = 60;
        [field: SerializeField] public int DefaultCurrentHealth { get; private set; } = 60;
    }
}