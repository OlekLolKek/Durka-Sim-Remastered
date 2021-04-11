using UnityEngine;


namespace DurkaSimRemastered
{
    public class SyringeView : LevelObjectView
    {
        [SerializeField] private AudioSource _audioSource;

        public AudioSource AudioSource => _audioSource;
    }
}