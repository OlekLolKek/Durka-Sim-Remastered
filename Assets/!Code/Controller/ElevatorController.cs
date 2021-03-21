using System.Collections.Generic;
using DurkaSimRemastered.Interface;


namespace DurkaSimRemastered
{
    public class ElevatorController : IExecute
    {
        private readonly List<Elevator> _elevators = new List<Elevator>();
        
        public ElevatorController(List<ElevatorView> elevatorViews)
        {
            foreach (var elevatorView in elevatorViews)
            {
                _elevators.Add(new Elevator(elevatorView));
            }
        }
        
        public void Execute(float deltaTime)
        {
            foreach (var elevator in _elevators)
            {
                elevator.Execute(deltaTime);
            }
        }
    }
}