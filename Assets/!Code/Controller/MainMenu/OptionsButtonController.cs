using DurkaSimRemastered.Interface;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class OptionsButtonController : ICleanup
    {
        private readonly UIButtonView _buttonView;

        public OptionsButtonController(UIButtonView buttonView)
        {
            _buttonView = buttonView;
            _buttonView.Button.onClick.AddListener(OnOptionsButtonPressed);
        }

        private void OnOptionsButtonPressed()
        {
            _buttonView.AudioSource.Play();
        }

        public void Cleanup()
        {
            _buttonView.Button.onClick.RemoveAllListeners();
        }
    }
}