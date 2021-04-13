using System;


namespace Model
{
    public class PlayerLifeModel
    {
        public Action<int> OnPlayerHealthChanged { get; set; } = delegate(int i) {  };
        public Action OnPlayerWon { get; set; } = delegate {  };
        public Action OnPlayerDied { get; set; } = delegate {  };
        public int MaxHealth { get; set; }
        public int CurrentHealth { get; private set; }
        public bool IsDead { get; set; }

        public PlayerLifeModel(int health)
        {
            MaxHealth = health;
            CurrentHealth = MaxHealth;
        }

        public void SetHealth(int newHealth)
        {
            CurrentHealth = newHealth;
            
            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                Die();
            }
            
            OnPlayerHealthChanged.Invoke(CurrentHealth);
        }

        public void Win()
        {
            OnPlayerWon.Invoke();
        }

        private void Die()
        {
            IsDead = true;
            OnPlayerDied.Invoke();
        }
    }
}