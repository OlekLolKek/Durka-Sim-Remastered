using System.Collections;
using DG.Tweening;
using DurkaSimRemastered.Interface;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class CreditsRollController : IExecute
    {
        private readonly GameObject _creditsCanvas;
        private readonly CreditsMovementModel _creditsMovementModel;
        private const float MAX_HEIGHT = 50.0f;
        private const float SPEED = 3.0f;
        private bool _movementFinished;

        public CreditsRollController(GameObject creditsCanvas,
            CreditsMovementModel creditsMovementModel)
        {
            _creditsCanvas = creditsCanvas;
            _creditsMovementModel = creditsMovementModel;
        }

        public void Execute(float deltaTime)
        {
            if (_creditsCanvas.transform.position.y < MAX_HEIGHT)
            {
                _creditsCanvas.transform.Translate(Vector3.up * (deltaTime * SPEED));
            }
            else if (!_movementFinished)
            {
                _creditsMovementModel.FinishMovement();
                _movementFinished = true;
            }
        }
    }
}