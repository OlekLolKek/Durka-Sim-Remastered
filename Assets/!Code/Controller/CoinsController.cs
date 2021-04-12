using System;
using System.Collections.Generic;
using DurkaSimRemastered.Interface;
using Model;
using UnityEngine;
using Object = UnityEngine.Object;


namespace DurkaSimRemastered
{
    public sealed class CoinsController : IExecute, ICleanup
    {
        private const float ANIMATIONS_SPEED = 10.0f;

        private readonly LevelObjectView _characterView;
        private readonly SpriteAnimator _spriteAnimator;
        private readonly AmmoModel _ammoModel;
        private readonly List<SyringeView> _syringeViews;

        public CoinsController(LevelObjectView characterView, List<SyringeView> syringeViews, 
            SpriteAnimatorConfig syringeAnimatorConfig, AmmoModel ammoModel)
        {
            _characterView = characterView;
            _spriteAnimator = new SpriteAnimator(syringeAnimatorConfig);
            _syringeViews = syringeViews;
            _ammoModel = ammoModel;
            _characterView.OnTriggerEnter += OnLevelObjectContact;

            foreach (var coinView in syringeViews)
            {
                _spriteAnimator.StartAnimation(coinView.SpriteRenderer, AnimationState.Idle, 
                    true, AnimationSpeeds.NORMAL_ANIMATION_SPEED);
            }
        }

        public void Execute(float deltaTime)
        {
            _spriteAnimator.Execute(deltaTime);
        }

        private void OnLevelObjectContact(Collider2D collider2D)
        {
            var contactView = collider2D.gameObject.GetComponent<SyringeView>();

            if (_syringeViews.Contains(contactView))
            {
                _spriteAnimator.StopAnimation(contactView.SpriteRenderer);
                _ammoModel.SetAmmoCount(_ammoModel.AmmoCount + 1);
                contactView.AudioSource.transform.SetParent(null);
                contactView.AudioSource.Play();
                contactView.gameObject.SetActive(false);
            }
        }

        public void Cleanup()
        {
            _characterView.OnTriggerEnter -= OnLevelObjectContact;
        }
    }
}