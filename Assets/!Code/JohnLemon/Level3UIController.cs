using System;
using DurkaSimRemastered.Interface;
using Model;
using UnityEngine;
using Object = UnityEngine.Object;


namespace DurkaSimRemastered
{
    public class Level3UIController : IInitialize, ICleanup
    {
        private readonly JohnLemonHealthView _johnLemonHealthView;
        private readonly PlayerHealthView _playerHealthView;
        private readonly AmmoCounterView _ammoCounterView;
        private readonly LemonFaderView _faderView;

        private readonly PlayerLifeModel _playerLifeModel;
        private readonly JohnLemonLifeModel _johnLemonLifeModel;
        private readonly AmmoModel _ammoModel;

        public Level3UIController(PlayerLifeModel playerLifeModel,
            AmmoModel ammoModel, JohnLemonLifeModel johnLemonLifeModel)
        {
            _playerLifeModel = playerLifeModel;
            _ammoModel = ammoModel;
            _johnLemonLifeModel = johnLemonLifeModel;

            _johnLemonHealthView = Object.FindObjectOfType<JohnLemonHealthView>();
            _playerHealthView = Object.FindObjectOfType<PlayerHealthView>();
            _ammoCounterView = Object.FindObjectOfType<AmmoCounterView>();
            _faderView = Object.FindObjectOfType<LemonFaderView>();
        }

        public void Initialize()
        {
            _johnLemonHealthView.Initialize(_johnLemonLifeModel);
            _playerHealthView.Initialize(_playerLifeModel);
            _ammoCounterView.Initialize(_ammoModel);
            _faderView.Initialize(_playerLifeModel);
        }

        public void Cleanup()
        {
            _johnLemonHealthView.Cleanup();
            _playerHealthView.Cleanup();
            _ammoCounterView.Cleanup();
            _faderView.Cleanup();
        }
    }
}