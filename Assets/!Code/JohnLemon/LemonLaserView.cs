using System;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class LemonLaserView : LevelObjectView
    {
        public event Action<Collision2D> OnLaserCollision 
            = delegate(Collision2D collision2D) {  };

        public void SetVisible(bool visible)
        {
            SpriteRenderer.enabled = visible;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            OnLaserCollision.Invoke(other);
        }
    }
}