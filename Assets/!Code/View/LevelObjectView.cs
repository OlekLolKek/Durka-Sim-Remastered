using System;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class LevelObjectView : MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer _spriteRenderer;
        [SerializeField] protected Rigidbody2D _rigidbody2D;
        [SerializeField] protected  Collider2D _collider2D;

        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        public Collider2D Collider2D => _collider2D;
        
        public Action<Collider2D> OnLevelObjectContact { get; set; }

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnLevelObjectContact?.Invoke(other);
        }
    }
}