using System;
using DurkaSimRemastered.Interface;
using Pathfinding;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class StalkerAI : IFixedExecute
    {
        private readonly LevelObjectView _view;
        private readonly StalkerAIModel _model;
        private readonly Seeker _seeker;
        private readonly Transform _target;

        public StalkerAI(LevelObjectView view, StalkerAIModel model, Seeker seeker, Transform target)
        {
            _view = view != null ? view : throw new ArgumentNullException(nameof(view));
            _model = model != null ? model : throw new ArgumentNullException(nameof(model));
            _seeker = seeker != null ? seeker : throw new ArgumentNullException(nameof(seeker));
            _target = target != null ? target : throw new ArgumentNullException(nameof(target));
        }

        #region Methods

        public void FixedExecute()
        {
            var newVelocity = _model.CalculateVelocity(_view.transform.position) * Time.fixedDeltaTime;
            _view.Rigidbody2D.velocity = newVelocity;
            //TODO: refactor this garbage
            var target = _model.GetPath().vectorPath[1];
            var direction = target - _view.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var rotation = Quaternion.Euler(new Vector3(0, 0, angle + -90));
            _view.transform.rotation = Quaternion.Slerp(_view.transform.rotation, rotation, 5 * Time.fixedDeltaTime);
        }

        public void RecalculatePath()
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