using System;
using DurkaSimRemastered;


namespace Model
{
    public class DoorUseModel
    {
        public Action<DoorView> OnDoorActivated = delegate(DoorView view) {  };

        public void ActivateDoor(DoorView pairDoor)
        {
            OnDoorActivated.Invoke(pairDoor);
        }
    }
}