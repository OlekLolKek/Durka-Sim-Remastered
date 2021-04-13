using DurkaSimRemastered.Interface;
using UnityEngine.SceneManagement;


namespace DurkaSimRemastered
{
    public class PlayButtonController : ICleanup
    {
        private readonly UIButtonView _buttonView;
        private const int LEVEL_ONE_SCENE_INDEX = 1;
        
        public PlayButtonController(UIButtonView buttonView)
        {
            _buttonView = buttonView;
            
            _buttonView.Button.onClick.AddListener(OnPlayButtonPressed);
        }

        private void OnPlayButtonPressed()
        {
            SceneManager.LoadScene(LEVEL_ONE_SCENE_INDEX);
            _buttonView.AudioSource.Play();
        }

        public void Cleanup()
        {
            _buttonView.Button.onClick.RemoveAllListeners();
        }
    }
}