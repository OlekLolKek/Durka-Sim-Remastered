using DurkaSimRemastered.Interface;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class JamBossAI : IInitialize, IExecute, ICleanup
    {
        private readonly SpriteAnimator _spriteAnimator;
        private readonly EnemyView _view;

        private float _currentHealth;
        
        public JamBossAI(EnemyView view, AIConfig config,
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
                true, AnimationSpeeds.NORMAL_ANIMATION_SPEED);
        }

        public void Execute(float deltaTime)
        {
            _spriteAnimator.Execute(deltaTime);
        }

        private void OnDamageReceived(int damage)
        {
            _currentHealth -= damage;
            _view.DamageParticleSystem.Play();
            _view.AudioSource.Play();
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
            _spriteAnimator.StopAnimation(_view.SpriteRenderer);
        }

        public void Cleanup()
        {
            _view.OnDamageReceived -= OnDamageReceived;
        }
    }
}