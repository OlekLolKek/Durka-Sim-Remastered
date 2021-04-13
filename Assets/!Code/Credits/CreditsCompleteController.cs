using System;
using System.Collections;
using DG.Tweening;
using DurkaSimRemastered.Interface;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace DurkaSimRemastered
{
    public class CreditsCompleteController :  ICleanup
    {
        private readonly Image _fader;
        private readonly AudioSource _audioSource;
        private readonly CreditsMovementModel _creditsMovementModel;

        private IDisposable _completeCoroutine;

        public CreditsCompleteController(Image fader, AudioSource audioSource,
            CreditsMovementModel creditsMovementModel)
        {
            _fader = fader;
            _audioSource = audioSource;
            _creditsMovementModel = creditsMovementModel;
            _creditsMovementModel.OnMovementFinished += StartComplete;
        }

        private void StartComplete()
        {
            _completeCoroutine = Complete().ToObservable().Subscribe();
        }

        private IEnumerator Complete()
        {
            yield return new WaitForSeconds(CreditsTimings.PAUSE_AFTER_ROLL_TIME);
            _fader.DOColor(Color.black, CreditsTimings.FADE_TIME);
            _audioSource.DOFade(0.0f, CreditsTimings.FADE_TIME);
            yield return new WaitForSeconds(CreditsTimings.FADE_TIME);
            SceneManager.LoadScene(0);
        }

        public void Cleanup()
        {
            _completeCoroutine?.Dispose();
            _creditsMovementModel.OnMovementFinished -= StartComplete;
        }
    }
}