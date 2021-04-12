using System;
using System.Collections.Generic;
using DG.Tweening;
using DurkaSimRemastered.Interface;
using Model;
using UnityEngine;


namespace DurkaSimRemastered
{
    public sealed class Level3CoinController : IExecute, ICleanup
    {        
        private const float TIME_BETWEEN_DROPS = 10.0f;
        private const float DROP_TWEEN_TIME = 5.0f;
        
        private readonly PlayerView _playerView;
        private readonly SpriteAnimator _spriteAnimator;
        private readonly AmmoModel _ammoModel;
        private readonly List<AmmoDropperView> _droppers;
        private readonly List<SyringeView> _syringeViews;

        private AmmoDropperView _activeDropper;
        private int _currentDropperIndex;
        private float _dropTimer;
        private bool _isReadyToDrop;

        public Level3CoinController(PlayerView playerView, 
            List<SyringeView> syringeViews, 
            SpriteAnimatorConfig syringeAnimatorConfig,
            AmmoModel ammoModel, List<AmmoDropperView> droppers)
        {
            _playerView = playerView;
            _spriteAnimator = new SpriteAnimator(syringeAnimatorConfig);
            _syringeViews = syringeViews;
            _ammoModel = ammoModel;
            _droppers = droppers;
            _playerView.OnTriggerEnter += OnTriggerEnter;

            foreach (var syringeView in syringeViews)
            {
                _spriteAnimator.StartAnimation(syringeView.SpriteRenderer, 
                    AnimationState.Idle, true, 
                    AnimationSpeeds.NORMAL_ANIMATION_SPEED);
            }

            _currentDropperIndex = 0;
            _activeDropper = _droppers[_currentDropperIndex];
        }
        
        public void Execute(float deltaTime)
        {
            _spriteAnimator.Execute(deltaTime);
            CheckForDrops(deltaTime);
        }

        private void CheckForDrops(float deltaTime)
        {
            if (!_isReadyToDrop)
            {
                if (_dropTimer <= TIME_BETWEEN_DROPS)
                {
                    _dropTimer += deltaTime;
                }
                else
                {
                    _isReadyToDrop = true;
                    _dropTimer = 0.0f;
                }
            }
            else
            {
                Drop();
            }
        }

        private void Drop()
        {
            foreach (var syringeView in _syringeViews)
            {
                if (!syringeView.gameObject.activeSelf)
                {
                    _isReadyToDrop = false;
                    _spriteAnimator.StartAnimation(syringeView.SpriteRenderer, 
                        AnimationState.Idle, true, 
                        AnimationSpeeds.NORMAL_ANIMATION_SPEED);
                    
                    var audioSourceTransform = syringeView.AudioSource.transform;
                    audioSourceTransform.SetParent(syringeView.transform);
                    audioSourceTransform.localPosition = Vector3.zero;
                    syringeView.gameObject.SetActive(true);
                    syringeView.transform.position = _activeDropper.transform.position;
                    syringeView.transform.DOMove(_activeDropper.EndPosition, DROP_TWEEN_TIME);
                    SwitchActiveDropper();

                    break;
                }
            }
        }

        private void SwitchActiveDropper()
        {
            _currentDropperIndex++;
            if (_currentDropperIndex == _droppers.Count)
            {
                _currentDropperIndex = 0;
            }

            _activeDropper = _droppers[_currentDropperIndex];
        }

        private void OnTriggerEnter(Collider2D other)
        {
            var contactView = other.gameObject.GetComponent<SyringeView>();

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
            _playerView.OnTriggerEnter -= OnTriggerEnter;
        }
    }
}