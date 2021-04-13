using UnityEngine;


namespace DurkaSimRemastered
{
    public class DoorView : LevelObjectView
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private DoorView _pairDoor;

        public AudioSource AudioSource => _audioSource;
        public DoorView PairDoor => _pairDoor;
    }
}