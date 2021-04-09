using DurkaSimRemastered.Interface;


namespace DurkaSimRemastered
{
    public class JamBossAi : IInitialize, IExecute
    {
        private readonly SpriteAnimator _spriteAnimator;
        private readonly EnemyView _view;

        private float _currentHealth;
        private bool _isDead;
        
        public JamBossAi(EnemyView view, AIConfig config,
            SpriteAnimatorConfig animatorConfig)
        {
            _spriteAnimator = new SpriteAnimator(animatorConfig);
            _view = view;

            _view.OnDamageReceived += OnDamageReceived;

            _currentHealth = config.Health;
        }
        
        public void Initialize()
        {
            _spriteAnimator.StartAnimation(_view.SpriteRenderer, AnimationState.Idle, 
                true,AnimationSpeeds.NORMAL_ANIMATION_SPEED);
        }

        public void Execute(float deltaTime)
        {
            if (_isDead) return;
            
            _spriteAnimator.Execute(deltaTime);
        }

        private void OnDamageReceived(int damage)
        {
            _currentHealth -= damage;
            _view.DamageParticleSystem.Play();
            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            _view.OnDamageReceived -= OnDamageReceived;
            _view.DeathParticleSystem.transform.SetParent(null);
            _view.gameObject.SetActive(false);
            _view.DeathParticleSystem.Play();
            _isDead = true;
        }
    }
}