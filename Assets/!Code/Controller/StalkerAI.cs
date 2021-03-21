using System;
using DurkaSimRemastered.Interface;
using Pathfinding;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class StalkerAI : IInitialize, IExecute, IFixedExecute
    {
        private readonly LevelObjectView _view;
        private readonly StalkerAIModel _model;
        private readonly AIConfig _config;
        private readonly Seeker _seeker;
        private readonly Transform _target;

        private readonly float _rotationSpeed;
        private const float RECALCULATE_PATH_FREQUENCY = 0.5f;
        private const float SPRITE_ROTATION_OFFSET = -90.0f;
        private float _recalculatePathTimer;

        public StalkerAI(LevelObjectView view, AIConfig config, 
            Seeker seeker, Transform target)
        {
            _view = view != null ? view : throw new ArgumentNullException(nameof(view));
            _config = config != null ? config : throw new ArgumentNullException(nameof(config));
            _seeker = seeker != null ? seeker : throw new ArgumentNullException(nameof(seeker));
            _target = target != null ? target : throw new ArgumentNullException(nameof(target));

            _model = new StalkerAIModel(_config);
            _rotationSpeed = _config.RotationSpeed;
        }

        #region Methods

        public void Initialize()
        {
            RecalculatePath();
        }

        public void Execute(float deltaTime)
        {
            _recalculatePathTimer += deltaTime;
            if (_recalculatePathTimer >= RECALCULATE_PATH_FREQUENCY)
            {
                _recalculatePathTimer = 0.0f;
                RecalculatePath();
            }
        }

        public void FixedExecute()
        {
            var position = _view.transform.position;
            
            var newVelocity = _model.CalculateVelocity(position) * Time.fixedDeltaTime;
            _view.Rigidbody2D.velocity = newVelocity;
            
            Rotate(position);
        }

        private void Rotate(Vector3 position)
        {
            Debug.Log(_model);
            Debug.Log(_model.GetPath());
            Debug.Log(_model.GetPath().vectorPath);
            Debug.Log(_model.GetPath().vectorPath[1]);
            var target = _model.GetPath().vectorPath[1];
            var direction = target - position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, angle + SPRITE_ROTATION_OFFSET));
            _view.transform.rotation = Quaternion.Slerp(_view.transform.rotation, rotation, _rotationSpeed * Time.fixedDeltaTime);
        }

        private void RecalculatePath()
        {
            if (_seeker.IsDone())
            {
                _seeker.StartPath(_view.Rigidbody2D.position, _target.position, OnPathComplete);
            }
        }

        private void OnPathComplete(Path path)
        {
            if (path.error) return;
            _model.UpdatePath(path);
        }

        #endregion
    }
}