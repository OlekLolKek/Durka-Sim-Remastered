using System;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class Lessons : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private LevelObjectView _playerView;
        [SerializeField] private float _animationSpeed = 10.0f;

        private SpriteAnimator _playerAnimator;
        
        private void Awake()
        {
            SpriteAnimatorConfig playerConfig = Resources.Load<SpriteAnimatorConfig>("PlayerAnimationConfig");
            _playerAnimator = new SpriteAnimator(playerConfig);
            _playerAnimator.StartAnimation(_playerView.SpriteRenderer, AnimationState.Run, true, _animationSpeed);
        }
        
        private void Update()
        {
            _playerAnimator.Update();
        }

        private void FixedUpdate()
        {
            
        }
    }
}
