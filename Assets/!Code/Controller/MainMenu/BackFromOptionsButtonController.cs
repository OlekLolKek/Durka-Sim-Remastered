using DurkaSimRemastered.Interface;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class BackFromOptionsButtonController : ICleanup
    {
        private readonly UIButtonView _backButtonView;
        private readonly RectTransform _mainLayout;
        private readonly RectTransform _optionsLayout;

        public BackFromOptionsButtonController(UIButtonView backButtonView,
            RectTransform mainLayout, RectTransform optionsLayout)
        {
            _backButtonView = backButtonView;
            _mainLayout = mainLayout;
            _optionsLayout = optionsLayout;
            _backButtonView.Button.onClick.AddListener(OnBackButtonPressed);
        }

        private void OnBackButtonPressed()
        {
            _backButtonView.AudioSource.Play();
            _mainLayout.gameObject.SetActive(true);
            _optionsLayout.gameObject.SetActive(false);
        }

        public void Cleanup()
        {
            _backButtonView.Button.onClick.RemoveAllListeners();
        }
    }
}