using DurkaSimRemastered.Interface;
using UnityEngine.Audio;


namespace DurkaSimRemastered
{
    public class MasterVolumeSliderController : ICleanup
    {
        private readonly UISliderView _masterVolumeSlider;
        private readonly AudioMixer _audioMixer;

        private const string MASTER_VOLUME_PARAMETER_NAME = "MasterVolume";

        public MasterVolumeSliderController(UISliderView masterVolumeSlider,
            AudioMixer audioMixer)
        {
            _masterVolumeSlider = masterVolumeSlider;
            _audioMixer = audioMixer;
            _masterVolumeSlider.Slider.onValueChanged.AddListener(OnSliderVolumeChanged);
            _audioMixer.GetFloat(MASTER_VOLUME_PARAMETER_NAME, out var volume);
            _masterVolumeSlider.Slider.value = volume;
        }

        private void OnSliderVolumeChanged(float value)
        {
            _audioMixer.SetFloat(MASTER_VOLUME_PARAMETER_NAME, value);
        }
        
        public void Cleanup()
        {
            _masterVolumeSlider.Slider.onValueChanged.RemoveAllListeners();
        }
    }
}