using UnityEngine;


namespace DurkaSimRemastered
{
    public class JohnLemonView : EnemyView
    {
        [SerializeField] private Transform _bulletSource;

        public Transform BulletSource => _bulletSource;
    }
}