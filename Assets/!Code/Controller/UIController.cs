using DurkaSimRemastered.Interface;
using Model;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class UIController : IInitialize, ICleanup
    {
        private readonly PlayerHealthView _playerHealthView;
        private readonly AmmoCounterView _ammoCounterView;
        private readonly FaderView _faderView;

        private readonly PlayerLifeModel _playerLifeModel;
        private readonly AmmoModel _ammoModel;
        private readonly DoorUseModel _doorUseModel;

        public UIController(PlayerLifeModel playerLifeModel, AmmoModel ammoModel,
            DoorUseModel doorUseModel)
        {
            _playerLifeModel = playerLifeModel;
            _ammoModel = ammoModel;
            _doorUseModel = doorUseModel;

            _playerHealthView = Object.FindObjectOfType<PlayerHealthView>();
            _ammoCounterView = Object.FindObjectOfType<AmmoCounterView>();
            _faderView = Object.FindObjectOfType<FaderView>();
        }
        
        public void Initialize()
        {
            _playerHealthView.Initialize(_playerLifeModel);
            _ammoCounterView.Initialize(_ammoModel);
            _faderView.Initialize(_doorUseModel, _playerLifeModel);
        }

        public void Cleanup()
        {
            _playerHealthView.Cleanup();
            _ammoCounterView.Cleanup();
            _faderView.Cleanup();
        }
    }
}