using UnityEngine;


namespace DurkaSimRemastered
{
    public class JohnLemonView : EnemyView
    {
        [SerializeField] private Transform _bulletSource;
        [SerializeField] private GameObject _head;

        public Transform BulletSource => _bulletSource;
        public GameObject Head => _head;
    }
}