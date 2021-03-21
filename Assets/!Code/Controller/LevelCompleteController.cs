using System;
using System.Collections.Generic;
using UnityEngine;


namespace DurkaSimRemastered
{
    public sealed class LevelCompleteController : IDisposable
    {
        private readonly Vector3 _startPosition;
        private readonly LevelObjectView _characterView;
        private readonly List<LevelObjectView> _deathZones;
        private List<LevelObjectView> _winZones;

        public LevelCompleteController(LevelObjectView characterView, List<LevelObjectView> deathZones, 
            List<LevelObjectView> winZones)
        {
            _startPosition = characterView.transform.position;
            characterView.OnLevelObjectContact += OnLevelObjectContact;
            
            _characterView = characterView;
            _deathZones = deathZones;
            _winZones = winZones;
        }

        private void OnLevelObjectContact(Collider2D collider2D)
        {
            var contactView = collider2D.gameObject.GetComponent<LevelObjectView>();

            if (_deathZones.Contains(contactView))
            {
                _characterView.transform.position = _startPosition;
            }
        }
        
        public void Dispose()
        {
            _characterView.OnLevelObjectContact -= OnLevelObjectContact;
        }
    }
}