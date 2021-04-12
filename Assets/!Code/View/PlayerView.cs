using System;
using DurkaSimRemastered.Interface;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class PlayerView : LevelObjectView, IDamageable
    {
        public Action<int> OnDamageReceived { get; set; } = delegate(int i) {  };

        [SerializeField] private ParticleSystem _damageParticleSystem;
        [SerializeField] private AudioSource _audioSource;

        public ParticleSystem DamageParticleSystem => _damageParticleSystem;
        public AudioSource AudioSource => _audioSource;
        
        public void Damage(int damage)
        {
            OnDamageReceived.Invoke(damage);
        }
    }
}