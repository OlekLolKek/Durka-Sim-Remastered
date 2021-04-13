using System.Collections.Generic;
using DurkaSimRemastered.Interface;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class ElevatorController : IExecute
    {
        private readonly List<Elevator> _elevators = new List<Elevator>();

        private readonly bool _isActive;
        
        public ElevatorController(List<ElevatorView> elevatorViews)
        {
            if (elevatorViews.Count == 0)
            {
                _isActive = false;
                return;
            }

            _isActive = true;
            
            foreach (var elevatorView in elevatorViews)
            {
                _elevators.Add(new Elevator(elevatorView));
            }
        }
        
        public void Execute(float deltaTime)
        {
            if (!_isActive)
            {
                return;
            }
            
            foreach (var elevator in _elevators)
            {
                elevator.Execute(deltaTime);
            }
        }
    }
}