using System;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class BubView : EnemyView
    {
        public event Action<Collision2D> OnBubCollision = delegate(Collision2D collision2D) {  }; 
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            OnBubCollision.Invoke(other);
        }
    }
}