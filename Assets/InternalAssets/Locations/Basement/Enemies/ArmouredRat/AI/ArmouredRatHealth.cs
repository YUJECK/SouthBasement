﻿using System.Linq;
using SouthBasement.AI;
using UnityEngine;

namespace SouthBasement.Basement.Enemies.ArmouredRat.AI
{
    public sealed class ArmouredRatHealth : DefaultEnemyHealth
    {
        [SerializeField] private AudioSource _shieldHitSound;
        
        private bool _currentDefends;

        public bool CurrentDefends
        {
            get => _currentDefends;
            set
            {
                _currentDefends = value;
                EffectsHandler.Blocked = value;
            }
        }

        public override void Damage(int damage, string[] args)
        {
            if(!CurrentDefends || args.Contains("effect")) base.Damage(damage, args);
            else _shieldHitSound.Play();
        }
    }
}