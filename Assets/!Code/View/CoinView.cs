using System;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class CoinView : LevelObjectView
    {
        public Action<Collider2D> OnLevelObjectContact { get; set; }

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnLevelObjectContact?.Invoke(other);
        }
    }
}