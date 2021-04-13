using DurkaSimRemastered.Interface;
using UnityEngine;
using UnityEngine.UI;


namespace DurkaSimRemastered
{
    public class UIButtonView : MonoBehaviour, IUIElement
    {
        [SerializeField] private Button _button;
        [SerializeField] private AudioSource _audioSource;

        public Button Button => _button;
        public AudioSource AudioSource => _audioSource;
        
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}