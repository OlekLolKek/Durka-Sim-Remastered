using System;
using DG.Tweening;
using DurkaSimRemastered.Interface;
using Model;
using UnityEngine;
using UnityEngine.UI;


namespace DurkaSimRemastered
{
    public class AmmoCounterView : MonoBehaviour, IUIElement, ICleanup
    {
        [SerializeField] private Text _text;
        [SerializeField] private string _baseText;

        private AmmoModel _ammoModel;

        public void Initialize(AmmoModel ammoModel)
        {
            _ammoModel = ammoModel;
            _ammoModel.OnAmmoCountChanged += SetAmmoCount;
            SetAmmoCount(_ammoModel.AmmoCount);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void SetAmmoCount(int newCount)
        {
            _text.text = $"{_baseText}{newCount}";
        }

        public void Cleanup()
        {
            _ammoModel.OnAmmoCountChanged -= SetAmmoCount;
        }
    }
}