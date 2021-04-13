using System;
using DurkaSimRemastered.Interface;


namespace DurkaSimRemastered
{
    public class JohnLemonLifeModel
    {
        public int CurrentHealth { get; set; }
        public int MaxHealth { get; private set; }
        
        public Action<int> OnHealthChanged = delegate(int i) {  };
        
        public Action OnLemonDied = delegate {  };
        public bool IsDead { get; private set; }

        public JohnLemonLifeModel(int health)
        {
            MaxHealth = health;
            SetHealth(MaxHealth);
        }
        
        public void SetHealth(int newHealth)
        {
            CurrentHealth = newHealth;
            OnHealthChanged.Invoke(CurrentHealth);
            if (CurrentHealth <= 0)
            {
                IsDead = true;
                OnLemonDied.Invoke();
            }
        }
    }
}