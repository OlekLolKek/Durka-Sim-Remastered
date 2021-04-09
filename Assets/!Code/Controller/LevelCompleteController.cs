using System;
using System.Collections.Generic;
using DurkaSimRemastered.Interface;
using Model;
using UnityEngine;


namespace DurkaSimRemastered
{
    public sealed class LevelCompleteController : ICleanup
    {
        private readonly Vector3 _startPosition;
        private readonly LevelObjectView _characterView;
        private readonly List<LevelObjectView> _deathZones;
        private List<LevelObjectView> _winZones;
        private readonly PlayerLifeModel _playerLifeModel;

        private const int DEATH_PIT_DAMAGE = 9999;

        public LevelCompleteController(LevelObjectView characterView, List<LevelObjectView> deathZones, 
            List<LevelObjectView> winZones, PlayerLifeModel playerLifeModel)
        {
            _startPosition = characterView.transform.position;
            
            _characterView = characterView;
            _deathZones = deathZones;
            _winZones = winZones;
            _playerLifeModel = playerLifeModel;
            _playerLifeModel.OnPlayerDied += OnPlayerDied;
            
            foreach (var deathZone in _deathZones)
            {
                deathZone.OnTriggerEnter += OnTriggerEnter;
            }
        }

        private void OnTriggerEnter(Collider2D other)
        {
            if (other.TryGetComponent(out PlayerView playerView))
            {
                playerView.Damage(DEATH_PIT_DAMAGE);
            }
        }

        private void OnPlayerDied()
        {
            
        }
        
        public void Cleanup()
        {
            foreach (var deathZone in _deathZones)
            {
                deathZone.OnTriggerEnter -= OnTriggerEnter;
            }
            _playerLifeModel.OnPlayerDied -= OnPlayerDied;
            
        }
    }
}