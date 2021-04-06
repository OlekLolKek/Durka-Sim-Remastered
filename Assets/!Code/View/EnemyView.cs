using System;
using DurkaSimRemastered.Interface;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class EnemyView : LevelObjectView, IDamageable
    {
        [SerializeField] private ParticleSystem _deathParticleSystem;
        [SerializeField] private ParticleSystem _damageParticleSystem;

        public ParticleSystem DeathParticleSystem => _deathParticleSystem;
        public ParticleSystem DamageParticleSystem => _damageParticleSystem;
        public Action<int> OnDamageReceived { get; set; } = delegate(int i) {  };

        public void Damage(int damage)
        {
            OnDamageReceived.Invoke(damage);
        }
    }
}