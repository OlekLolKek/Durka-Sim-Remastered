using DurkaSimRemastered.Interface;
using Model;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class UIController : IInitialize
    {
        private readonly AmmoCounterView _ammoCounterView;
            
        private readonly AmmoModel _ammoModel;

        public UIController(AmmoModel ammoModel)
        {
            _ammoModel = ammoModel;

            _ammoCounterView = Object.FindObjectOfType<AmmoCounterView>();
        }
        
        public void Initialize()
        {
            _ammoCounterView.Initialize(_ammoModel);
        }
    }
}