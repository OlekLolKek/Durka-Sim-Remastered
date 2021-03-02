using DurkaSimRemastered.Interface;
using UnityEngine;


namespace DurkaSimRemastered
{
    public sealed class ContactPoller : IUpdate
    {
        #region Fields

        private const float COLLISION_THRESHOLD = 0.5f;

        private ContactPoint2D[] _contacts = new ContactPoint2D[10];
        private readonly Collider2D _collider2D;
        private int _contactsCount;

        #endregion

        #region Properties

        public bool IsGrounded { get; private set; }
        public bool HasLeftContacts { get; private set; }
        public bool HasRightContacts { get; private set; }

        #endregion

        public ContactPoller(Collider2D collider2D)
        {
            _collider2D = collider2D;
            Debug.Log(collider2D);
        }

        public void Update()
        {
            IsGrounded = false;
            HasLeftContacts = false;
            HasRightContacts = false;
            _contactsCount = _collider2D.GetContacts(_contacts);

            for (int i = 0; i < _contactsCount; i++)
            {
                var normal = _contacts[i].normal;
                var rigidbody = _contacts[i].rigidbody;

                if (normal.y > COLLISION_THRESHOLD)
                {
                    IsGrounded = true;
                }

                if (normal.x > COLLISION_THRESHOLD && rigidbody == null)
                {
                    HasLeftContacts = true;
                }

                if (normal.x < -COLLISION_THRESHOLD && rigidbody == null)
                {
                    HasRightContacts = true;
                }
            }
        }
    }
}