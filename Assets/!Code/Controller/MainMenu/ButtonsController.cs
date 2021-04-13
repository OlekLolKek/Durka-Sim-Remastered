using DurkaSimRemastered.Interface;
using UnityEngine;
using UnityEngine.Audio;


namespace DurkaSimRemastered
{
    public class ButtonsController : ICleanup
    {
        private readonly Controllers _controllers;
        
        public ButtonsController(UIButtonView playButton, UIButtonView optionsButton,
            UIButtonView exitButton, UIButtonView backFromOptionsButton,
            FaderView faderView, RectTransform mainLayout, 
            RectTransform optionsLayout, UISliderView masterVolumeSlider,
            AudioMixer mainAudioMixer)
        {
            _controllers = new Controllers();
            
            _controllers.AddController(
                new PlayButtonController(playButton));
            
            _controllers.AddController(
                new OptionsButtonController(optionsButton, mainLayout,
                    optionsLayout));

            _controllers.AddController(
                new ExitButtonController(exitButton, faderView));
            
            _controllers.AddController(
                new BackFromOptionsButtonController(backFromOptionsButton, 
                    mainLayout, optionsLayout));
            
            _controllers.AddController(
                new MasterVolumeSliderController(masterVolumeSlider, mainAudioMixer));
        }
        
        public void Cleanup()
        {
            _controllers.Cleanup();
        }
    }
}