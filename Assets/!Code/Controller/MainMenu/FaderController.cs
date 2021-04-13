using System;
using System.Collections;
using DurkaSimRemastered.Interface;
using UniRx;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class FaderController : IInitialize, ICleanup
    {
        private readonly FaderView _faderView;

        private IDisposable _faderCoroutine;

        private const float FADER_DELAY = 0.25f;

        public FaderController(FaderView faderView)
        {
            _faderView = faderView;
        }
        
        public void Initialize()
        {
            _faderCoroutine = ShowFaderAfterDelay().ToObservable().Subscribe();
        }

        private IEnumerator ShowFaderAfterDelay()
        {
            _faderView.ShowInstant();
            yield return new WaitForSeconds(FADER_DELAY);
            _faderView.Hide();
        }

        public void Cleanup()
        {
            _faderCoroutine?.Dispose();
        }
    }
}