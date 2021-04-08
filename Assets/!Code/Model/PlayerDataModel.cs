using UnityEngine;


namespace Model
{
    public class PlayerDataModel
    {
        public bool IsPlayerFacingRight { get; set; } = true;
        public bool IsPlayerShooting { get; set; }
        public bool IsGrounded { get; set; }
        public bool IsStandingOnElevator { get; set; }
        public bool IsStandingOnPlatform { get; set; }
        public bool HasLeftContacts { get; set; }
        public bool HasRightContacts { get; set; }
        public bool PlayerIntersects { get; set; }
    }
}