using DurkaSimRemastered.Interface;
using Model;
using UnityEngine;


namespace DurkaSimRemastered
{
    public sealed class ContactPoller : IExecute
    {
        #region Fields

        private const float COLLISION_THRESHOLD = 0.5f;

        private readonly PlayerDataModel _playerDataModel;
        private readonly ContactPoint2D[] _contacts = new ContactPoint2D[10];
        private readonly Collider2D _collider2D;
        private int _contactsCount;

        #endregion

        public ContactPoller(Collider2D collider2D, PlayerDataModel playerDataModel)
        {
            _collider2D = collider2D;
            _playerDataModel = playerDataModel;
        }

        public void Execute(float deltaTime)
        {
            _playerDataModel.IsGrounded = false;
            _playerDataModel.IsStandingOnElevator = false;
            _playerDataModel.IsStandingOnPlatform = false;
            _playerDataModel.HasLeftContacts = false;
            _playerDataModel.HasRightContacts = false;
            _contactsCount = _collider2D.GetContacts(_contacts);

            for (int i = 0; i < _contactsCount; i++)
            {
                var normal = _contacts[i].normal;
                var rigidbody = _contacts[i].rigidbody;
                
                bool hasRigidbody = rigidbody != null;
                
                if (normal.y > COLLISION_THRESHOLD)
                {
                    _playerDataModel.IsGrounded = true;
                    
                    if (_contacts[i].collider.gameObject.TryGetComponent(out ElevatorView _))
                    {
                        _playerDataModel.IsStandingOnElevator = true;
                    }

                    if (_contacts[i].collider.gameObject.TryGetComponent(out PlatformView _))
                    {
                        _playerDataModel.IsStandingOnPlatform = true;
                    }
                }

                if (normal.x > COLLISION_THRESHOLD && !hasRigidbody)
                {
                    _playerDataModel.HasLeftContacts = true;
                }

                if (normal.x < -COLLISION_THRESHOLD && !hasRigidbody)
                {
                    _playerDataModel.HasRightContacts = true;
                }
            }
        }
    }
}