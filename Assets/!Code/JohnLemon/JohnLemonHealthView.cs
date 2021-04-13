using DurkaSimRemastered.Interface;
using UnityEngine;
using UnityEngine.UI;


namespace DurkaSimRemastered
{
    public sealed class JohnLemonHealthView : MonoBehaviour, IUIElement, ICleanup
    {
        [SerializeField] private Image _healthBar;
        [SerializeField] private Text _healthText;

        private JohnLemonLifeModel _johnLemonLifeModel;

        public void Initialize(JohnLemonLifeModel johnLemonLifeModel)
        {
            _johnLemonLifeModel = johnLemonLifeModel;
            _johnLemonLifeModel.OnHealthChanged += ChangeHealthAmount;
            ChangeHealthAmount(_johnLemonLifeModel.MaxHealth);
            _johnLemonLifeModel.OnLemonDied += Hide;
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void ChangeHealthAmount(int health)
        {
            _healthText.text = health.ToString();
            _healthBar.fillAmount = (float) health / _johnLemonLifeModel.MaxHealth;
        }

        public void Cleanup()
        {
            _johnLemonLifeModel.OnHealthChanged -= ChangeHealthAmount;
        }
    }
}