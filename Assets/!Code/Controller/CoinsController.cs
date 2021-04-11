using System;
using System.Collections.Generic;
using DurkaSimRemastered.Interface;
using Model;
using UnityEngine;
using Object = UnityEngine.Object;


namespace DurkaSimRemastered
{
    public sealed class CoinsController : IExecute, IDisposable
    {
        private const float ANIMATIONS_SPEED = 10.0f;

        private readonly LevelObjectView _characterView;
        private readonly SpriteAnimator _spriteAnimator;
        private readonly AmmoModel _ammoModel;
        private readonly List<SyringeView> _coinViews;

        public CoinsController(LevelObjectView characterView, List<SyringeView> coinViews, 
            SpriteAnimatorConfig coinConfig, AmmoModel ammoModel)
        {
            _characterView = characterView;
            _spriteAnimator = new SpriteAnimator(coinConfig);
            _coinViews = coinViews;
            _ammoModel = ammoModel;
            _characterView.OnTriggerEnter += OnLevelObjectContact;

            foreach (var coinView in coinViews)
            {
                _spriteAnimator.StartAnimation(coinView.SpriteRenderer, AnimationState.Idle, true, ANIMATIONS_SPEED);
            }
        }

        public void Execute(float deltaTime)
        {
            _spriteAnimator.Execute(deltaTime);
        }

        private void OnLevelObjectContact(Collider2D collider2D)
        {
            var contactView = collider2D.gameObject.GetComponent<SyringeView>();

            if (_coinViews.Contains(contactView))
            {
                _spriteAnimator.StopAnimation(contactView.SpriteRenderer);
                _ammoModel.SetAmmoCount(_ammoModel.AmmoCount + 1);
                contactView.AudioSource.transform.SetParent(null);
                contactView.AudioSource.Play();
                contactView.gameObject.SetActive(false);
            }
        }

        public void Dispose()
        {
            _characterView.OnTriggerEnter -= OnLevelObjectContact;
        }
    }
}