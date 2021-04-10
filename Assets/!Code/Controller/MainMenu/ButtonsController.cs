using DurkaSimRemastered.Interface;


namespace DurkaSimRemastered
{
    public class ButtonsController : ICleanup
    {
        private readonly Controllers _controllers;
        
        public ButtonsController(UIButtonView playButton, UIButtonView optionsButton,
            UIButtonView exitButton, FaderView faderView)
        {
            _controllers = new Controllers();
            
            _controllers.AddController(
                new PlayButtonController(playButton));
            
            _controllers.AddController(
                new OptionsButtonController(optionsButton));
            
            _controllers.AddController(
                new ExitButtonController(exitButton, faderView));
        }
        
        public void Cleanup()
        {
            _controllers.Cleanup();
        }
    }
}