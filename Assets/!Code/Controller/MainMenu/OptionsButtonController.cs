using DurkaSimRemastered.Interface;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class OptionsButtonController : ICleanup
    {
        private readonly UIButtonView _optionsButtonView;
        private readonly RectTransform _mainLayout;
        private readonly RectTransform _optionsLayout;

        public OptionsButtonController(UIButtonView optionsButtonView,
            RectTransform mainLayout, RectTransform optionsLayout)
        {
            _optionsButtonView = optionsButtonView;
            _mainLayout = mainLayout;
            _optionsLayout = optionsLayout;
            _optionsButtonView.Button.onClick.AddListener(OnOptionsButtonPressed);
        }

        private void OnOptionsButtonPressed()
        {
            _optionsButtonView.AudioSource.Play();
            _mainLayout.gameObject.SetActive(false);
            _optionsLayout.gameObject.SetActive(true);
        }

        public void Cleanup()
        {
            _optionsButtonView.Button.onClick.RemoveAllListeners();
        }
    }
}