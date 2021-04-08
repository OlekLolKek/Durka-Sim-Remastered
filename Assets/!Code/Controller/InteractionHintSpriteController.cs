using DurkaSimRemastered.Interface;
using Model;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class InteractionHintSpriteController : IExecute
    {
        private readonly PlayerDataModel _playerDataModel;
        private readonly SpriteAnimator _spriteAnimator;
        private readonly LevelObjectView _view;
        private readonly Transform _player;
        private readonly Vector3 _offset = new Vector3(0.0f, 1.5f, 0.0f);

        public InteractionHintSpriteController(PlayerDataModel playerDataModel, 
            SpriteAnimatorConfig interactionHintSpriteAnimatorConfig,
            LevelObjectView view, LevelObjectView playerView)
        {
            _playerDataModel = playerDataModel;
            _view = view;

            _spriteAnimator = new SpriteAnimator(interactionHintSpriteAnimatorConfig);
            _spriteAnimator.StartAnimation(
                _view.SpriteRenderer, 
                AnimationState.Idle, 
                true, 
                AnimationSpeeds.HALF_ANIMATION_SPEED);

            _player = playerView.transform;
        }
        
        public void Execute(float deltaTime)
        {
            SpriteVisibility(deltaTime);
            MoveSprite();
        }

        private void SpriteVisibility(float deltaTime)
        {
            if (_playerDataModel.PlayerIntersects)
            {
                _view.gameObject.SetActive(true);
                _spriteAnimator.Execute(deltaTime);
            }
            else
            {
                _view.gameObject.SetActive(false);
            }
        }

        private void MoveSprite()
        {
            _view.transform.position = _player.position + _offset;
        }
    }
}