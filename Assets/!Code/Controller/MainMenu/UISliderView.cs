using DurkaSimRemastered.Interface;
using UnityEngine;
using UnityEngine.UI;


namespace DurkaSimRemastered
{
    public class UISliderView : MonoBehaviour, IUIElement
    {
        [SerializeField] private Slider _slider;

        public Slider Slider => _slider;


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