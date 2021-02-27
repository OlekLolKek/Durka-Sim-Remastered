using System;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class MainController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private LevelObjectView _playerView;
        [SerializeField] private float _animationSpeed = 10.0f;

        private SpriteAnimator _playerAnimator;
        private MainHeroWalker _mainHeroWalker;
        
        private void Awake()
        {
            SpriteAnimatorConfig playerConfig = Resources.Load<SpriteAnimatorConfig>("PlayerAnimationConfig");
            _playerAnimator = new SpriteAnimator(playerConfig);

            _mainHeroWalker = new MainHeroWalker(_playerView, _playerAnimator);
        }
        
        private void Update()
        {
            _mainHeroWalker.Update();
            _playerAnimator.Update();
        }

        private void FixedUpdate()
        {
            
        }

        private void LateUpdate()
        {
            
        }
    }
}
