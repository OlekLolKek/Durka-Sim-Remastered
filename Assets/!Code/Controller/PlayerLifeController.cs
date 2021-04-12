using DurkaSimRemastered.Interface;
using Model;


namespace DurkaSimRemastered
{
    public class PlayerLifeController : ICleanup
    {
        private readonly PlayerLifeModel _playerLifeModel;
        private readonly PlayerView _playerView;
        
        public PlayerLifeController(PlayerView playerView, PlayerLifeModel playerLifeModel)
        {
            _playerView = playerView;
            _playerView.OnDamageReceived += OnDamageReceived;
            _playerLifeModel = playerLifeModel;
        }

        private void OnDamageReceived(int damage)
        {
            _playerLifeModel.SetHealth(_playerLifeModel.CurrentHealth - damage);
            _playerView.AudioSource.Play();
            _playerView.DamageParticleSystem.Play();
        }

        public void Cleanup()
        {
            _playerView.OnDamageReceived -= OnDamageReceived;
        }
    }
}