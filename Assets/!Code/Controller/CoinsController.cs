using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;


namespace DurkaSimRemastered
{
    public sealed class CoinsController : IDisposable
    {
        private const float ANIMATIONS_SPEED = 10.0f;

        private readonly LevelObjectView _characterView;
        private readonly SpriteAnimator _spriteAnimator;
        private readonly List<LevelObjectView> _coinViews;

        public CoinsController(LevelObjectView characterView, List<LevelObjectView> coinViews, 
            SpriteAnimator spriteAnimator)
        {
            _characterView = characterView;
            _spriteAnimator = spriteAnimator;
            _coinViews = coinViews;
            _characterView.OnLevelObjectContact += OnLevelObjectContact;

            foreach (var coinView in coinViews)
            {
                _spriteAnimator.StartAnimation(coinView.SpriteRenderer, AnimationState.Idle, true, ANIMATIONS_SPEED);
            }
        }

        private void OnLevelObjectContact(Collider2D collider2D)
        {
            var contactView = collider2D.gameObject.GetComponent<LevelObjectView>();

            if (_coinViews.Contains(contactView))
            {
                _spriteAnimator.StopAnimation(contactView.SpriteRenderer);
                Object.Destroy(contactView.gameObject);
            }
        }

        public void Dispose()
        {
            _characterView.OnLevelObjectContact -= OnLevelObjectContact;
        }
    }
}