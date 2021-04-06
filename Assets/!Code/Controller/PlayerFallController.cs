using DurkaSimRemastered.Interface;
using Model;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class PlayerFallController : IExecute
    {
        private readonly ContactPoller _contactPoller;
        private readonly InputModel _inputModel;
        
        private float _fallTimer;
        private float _vertical;
        
        private const float FALL_THRESHOLD = -0.1f;
        private const float FALL_TIME = 0.33f;

        public PlayerFallController(ContactPoller contactPoller, InputModel inputModel)
        {
            _contactPoller = contactPoller;
            _inputModel = inputModel;
        }
        
        public void Execute(float deltaTime)
        {
            _vertical = _inputModel.Vertical;
            
            Fall(deltaTime);
        }

        private void Fall(float deltaTime)
        {
            if (Physics2D.GetIgnoreLayerCollision(LayerID.PLAYER_LAYER, LayerID.ELEVATOR_LAYER)
                || Physics2D.GetIgnoreLayerCollision(LayerID.PLAYER_LAYER, LayerID.PLATFORM_LAYER))
            {
                _fallTimer -= deltaTime;
                if (_fallTimer <= 0.0f)
                {
                    Physics2D.IgnoreLayerCollision(LayerID.PLAYER_LAYER, LayerID.ELEVATOR_LAYER, false);
                    Physics2D.IgnoreLayerCollision(LayerID.PLAYER_LAYER, LayerID.PLATFORM_LAYER, false);
                }
            }
            if (_contactPoller.IsStandingOnPlatform)
            {
                if (_vertical < FALL_THRESHOLD)
                {
                    _fallTimer = FALL_TIME;
                    Physics2D.IgnoreLayerCollision(LayerID.PLAYER_LAYER, LayerID.PLATFORM_LAYER);
                }
            }

            if (_contactPoller.IsStandingOnElevator)
            {
                if (_vertical < FALL_THRESHOLD)
                {
                    _fallTimer = FALL_TIME;
                    Physics2D.IgnoreLayerCollision(LayerID.PLAYER_LAYER, LayerID.ELEVATOR_LAYER);
                }
            }
        }
    }
}