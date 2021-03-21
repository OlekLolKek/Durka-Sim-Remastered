using System;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class LevelObjectView : MonoBehaviour
    {
        public SpriteRenderer SpriteRenderer;
        public Rigidbody2D Rigidbody2D;
        public Collider2D Collider2D;
        
        public Action<Collider2D> OnLevelObjectContact { get; set; }

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnLevelObjectContact?.Invoke(other);
        }
    }
}