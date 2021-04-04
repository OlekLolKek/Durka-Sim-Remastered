using DurkaSimRemastered.Interface;
using UnityEngine;


namespace DurkaSimRemastered
{
    public sealed class ContactPoller : IExecute
    {
        #region Fields

        private const float COLLISION_THRESHOLD = 0.5f;

        private readonly ContactPoint2D[] _contacts = new ContactPoint2D[10];
        private readonly Collider2D _collider2D;
        private int _contactsCount;

        #endregion

        
        #region Properties

        public Vector2 GroundVelocity { get; private set; }
        public bool IsGrounded { get; private set; }
        public bool IsStandingOnElevator { get; private set; }
        public bool IsStandingOnPlatform { get; private set; }
        public bool HasLeftContacts { get; private set; }
        public bool HasRightContacts { get; private set; }

        #endregion

        public ContactPoller(Collider2D collider2D)
        {
            _collider2D = collider2D;
        }

        public void Execute(float deltaTime)
        {
            IsGrounded = false;
            IsStandingOnElevator = false;
            IsStandingOnPlatform = false;
            HasLeftContacts = false;
            HasRightContacts = false;
            _contactsCount = _collider2D.GetContacts(_contacts);

            for (int i = 0; i < _contactsCount; i++)
            {
                var normal = _contacts[i].normal;
                var rigidbody = _contacts[i].rigidbody;
                
                bool hasRigidbody = rigidbody != null;
                
                if (normal.y > COLLISION_THRESHOLD)
                {
                    IsGrounded = true;
                    
                    if (_contacts[i].collider.gameObject.TryGetComponent(out ElevatorView _))
                    {
                        IsStandingOnElevator = true;
                    }

                    if (_contacts[i].collider.gameObject.TryGetComponent(out PlatformView _))
                    {
                        IsStandingOnPlatform = true;
                    }
                }

                if (hasRigidbody)
                {
                    GroundVelocity = rigidbody.velocity;
                }

                if (normal.x > COLLISION_THRESHOLD && !hasRigidbody)
                {
                    HasLeftContacts = true;
                }

                if (normal.x < -COLLISION_THRESHOLD && !hasRigidbody)
                {
                    HasRightContacts = true;
                }
            }
        }
    }
}