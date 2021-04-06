using System;
using DurkaSimRemastered.Interface;


namespace DurkaSimRemastered
{
    public class PlayerView : LevelObjectView, IDamageable
    {
        public Action<int> OnDamageReceived { get; set; } = delegate(int i) {  };
        
        public void Damage(int damage)
        {
            OnDamageReceived.Invoke(damage);
        }
    }
}