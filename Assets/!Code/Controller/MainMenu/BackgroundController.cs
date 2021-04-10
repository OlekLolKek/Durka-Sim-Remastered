using DurkaSimRemastered.Interface;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class BackgroundController : IExecute
    {
        private readonly Transform[] _backgroundTransforms;

        private readonly float _startPositionX = -24.0f;
        private const float MAX_POSITION_X = 24.0f;
        private const float SPEED = 1.5f;

        public BackgroundController(Transform[] backgroundTransforms)
        {
            _backgroundTransforms = backgroundTransforms;
        }


        public void Execute(float deltaTime)
        {
            foreach (var backgroundTransform in _backgroundTransforms)
            {
                Move(backgroundTransform, deltaTime);
            }
        }

        private void Move(Transform transform, float deltaTime)
        {
            if (transform.position.x > MAX_POSITION_X)
            {
                var position = transform.position;
                position = position.Change(x: position.x - 48.0f);
                transform.position = position;
                transform.Translate(Vector3.right * (deltaTime * SPEED));
            }
            else
            {
                transform.Translate(Vector3.right * (deltaTime * SPEED));
            }
        }
    }
}