using System;


namespace DurkaSimRemastered.Interface
{
    public interface IDamageable
    {
        Action<int> OnDamageReceived { get; set; }
        void Damage(int damage);
    }
}