using System.Collections.Generic;
using DurkaSimRemastered.Interface;


namespace DurkaSimRemastered
{
    public class Controllers : IInitialize, IExecute, IFixedExecute, ILateExecute
    {
        private readonly List<IInitialize> _starts = new List<IInitialize>();
        private readonly List<IExecute> _executes = new List<IExecute>();
        private readonly List<IFixedExecute> _fixedUpdates = new List<IFixedExecute>();
        private readonly List<ILateExecute> _lateUpdates = new List<ILateExecute>();

        public void AddController(IController controller)
        {
            if (controller is IInitialize start)
            {
                _starts.Add(start);
            }

            if (controller is IExecute execute)
            {
                _executes.Add(execute);
            }

            if (controller is IFixedExecute fixedUpdate)
            {
                _fixedUpdates.Add(fixedUpdate);
            }

            if (controller is ILateExecute lateUpdate)
            {
                _lateUpdates.Add(lateUpdate);
            }
        }
        
            
        public void Initialize()
        {
            foreach (var start in _starts)
            {
                start.Initialize();
            }
        }

        public void Execute(float deltaTime)
        {
            foreach (var execute in _executes)
            {
                execute.Execute(deltaTime);
            }
        }

        public void FixedExecute()
        {
            foreach (var fixedUpdate in _fixedUpdates)
            {
                fixedUpdate.FixedExecute();
            }
        }

        public void LateExecute()
        {
            foreach (var lateUpdate in _lateUpdates)
            {
                lateUpdate.LateExecute();
            }
        }
    }
}