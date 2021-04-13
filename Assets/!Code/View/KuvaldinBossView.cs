using System;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class KuvaldinBossView : EnemyView
    {
        public event Action<Collision2D> OnKuvaldinCollision = delegate(Collision2D collision2D) {  }; 
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            OnKuvaldinCollision.Invoke(other);
        }
    }
}