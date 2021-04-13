using System;


namespace DurkaSimRemastered
{
    public class CreditsMovementModel
    {
        public Action OnMovementFinished = delegate {  };
        
        public void FinishMovement()
        {
            OnMovementFinished.Invoke();
        }
    }
}