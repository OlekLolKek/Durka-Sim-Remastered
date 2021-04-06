using System;
using DurkaSimRemastered.Interface;
using Model;
using UnityEngine;
using UnityEngine.UI;


namespace DurkaSimRemastered
{
    public class PlayerHealthView : MonoBehaviour, IUIElement, ICleanup
    {
        [SerializeField] private Image _healthSlider;
        [SerializeField] private Text _healthText;
        private PlayerLifeModel _playerLifeModel;

        public void Initialize(PlayerLifeModel playerLifeModel)
        {
            _playerLifeModel = playerLifeModel;
            _playerLifeModel.OnPlayerHealthChanged += ChangeHealthAmount;
            ChangeHealthAmount(_playerLifeModel.MaxHealth);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Cleanup()
        {
            _playerLifeModel.OnPlayerHealthChanged -= ChangeHealthAmount;
        }

        private void ChangeHealthAmount(int health)
        {
            _healthText.text = health.ToString();
            _healthSlider.fillAmount = (float)health / _playerLifeModel.MaxHealth;
        }
    }
}