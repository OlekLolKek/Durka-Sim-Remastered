using UnityEngine;


namespace DurkaSimRemastered
{
    public class BulletParticleSystemView : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        
        public void Play()
        {
            _particleSystem.Play();
        }
    }
}