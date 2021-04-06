using UnityEngine;


namespace DurkaSimRemastered
{
    [CreateAssetMenu(fileName = "BulletConfig", menuName = "Configs/Bullet", order = 0)]
    public class BulletConfig : ScriptableObject
    {
        [SerializeField] private float _throwForce;
        [SerializeField] private int _damage;

        public float ThrowForce => _throwForce;
        public int Damage => _damage;
    }
}