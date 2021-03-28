using System.Collections.Generic;
using DurkaSimRemastered.Interface;


namespace DurkaSimRemastered
{
    public class Controllers : IInitialize, IExecute, IFixedExecute, ILateExecute, ICleanup
    {
        private readonly List<IInitialize> _initializes = new List<IInitialize>();
        private readonly List<IExecute> _executes = new List<IExecute>();
        private readonly List<IFixedExecute> _fixedExecutes = new List<IFixedExecute>();
        private readonly List<ILateExecute> _lateExecutes = new List<ILateExecute>();
        private readonly List<ICleanup> _cleanups = new List<ICleanup>();

        public void AddController(IController controller)
        {
            if (controller is IInitialize initialize)
            {
                _initializes.Add(initialize);
            }

            if (controller is IExecute execute)
            {
                _executes.Add(execute);
            }

            if (controller is IFixedExecute fixedExecute)
            {
                _fixedExecutes.Add(fixedExecute);
            }

            if (controller is ILateExecute lateExecute)
            {
                _lateExecutes.Add(lateExecute);
            }

            if (controller is ICleanup cleanup)
            {
                _cleanups.Add(cleanup);
            }
        }
        
            
        public void Initialize()
        {
            foreach (var initialize in _initializes)
            {
                initialize.Initialize();
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
            foreach (var fixedExecute in _fixedExecutes)
            {
                fixedExecute.FixedExecute();
            }
        }

        public void LateExecute()
        {
            foreach (var lateExecute in _lateExecutes)
            {
                lateExecute.LateExecute();
            }
        }

        public void Cleanup()
        {
            foreach (var cleanup in _cleanups)
            {
                cleanup.Cleanup();
            }
        }
    }
}