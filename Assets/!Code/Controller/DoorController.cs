using System.Collections.Generic;
using DurkaSimRemastered.Interface;
using Model;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class DoorController : IExecute, ICleanup
    {
        private readonly List<Door> _doors;
        private readonly DoorUseModel _doorUseModel;

        public DoorController(PlayerDataModel playerDataModel,
            InputModel inputModel, DoorUseModel doorUseModel)
        {
            var doorViews = Object.FindObjectsOfType<DoorView>();
            _doors = new List<Door>();

            for (int i = 0; i < doorViews.Length; i++)
            {
                _doors.Add(new Door(doorViews[i], playerDataModel, inputModel));
                _doors[i].OnDoorActivated += OnDoorActivated;
            }
            
            _doorUseModel = doorUseModel;
        }
        
        public void Execute(float deltaTime)
        {
            foreach (var door in _doors)
            {
                door.Execute(deltaTime);
            }
        }

        private void OnDoorActivated(DoorView pairDoorView)
        {
            _doorUseModel.ActivateDoor(pairDoorView);
        }

        public void Cleanup()
        {
            foreach (var door in _doors)
            {
                door.OnDoorActivated -= OnDoorActivated;
            }
        }
    }
}