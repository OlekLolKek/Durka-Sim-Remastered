using DurkaSimRemastered.Interface;
using Model;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class UIController : IInitialize, ICleanup
    {
        private readonly PlayerHealthView _playerHealthView;
        private readonly AmmoCounterView _ammoCounterView;
            
        private readonly PlayerLifeModel _playerLifeModel;
        private readonly AmmoModel _ammoModel;

        public UIController(PlayerLifeModel playerLifeModel, AmmoModel ammoModel)
        {
            _playerLifeModel = playerLifeModel;
            _ammoModel = ammoModel;

            _playerHealthView = Object.FindObjectOfType<PlayerHealthView>();
            _ammoCounterView = Object.FindObjectOfType<AmmoCounterView>();
        }
        
        public void Initialize()
        {
            _playerHealthView.Initialize(_playerLifeModel);
            _ammoCounterView.Initialize(_ammoModel);
        }

        public void Cleanup()
        {
            _playerHealthView.Cleanup();
            _ammoCounterView.Cleanup();
        }
    }
}