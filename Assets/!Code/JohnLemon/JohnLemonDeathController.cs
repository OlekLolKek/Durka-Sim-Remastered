using System;
using System.Collections;
using DurkaSimRemastered.Interface;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;


namespace DurkaSimRemastered
{
    public class JohnLemonDeathController : ICleanup
    {
        private readonly JohnLemonLifeModel _johnLemonLifeModel;
        private readonly JohnLemonView _johnLemonView;
        private readonly GameObject _johnLemonHead;

        private IDisposable _coroutine;

        public JohnLemonDeathController(JohnLemonLifeModel johnLemonLifeModel)
        {
            _johnLemonLifeModel = johnLemonLifeModel;
            _johnLemonView  = Object.FindObjectOfType<JohnLemonView>();;
            _johnLemonHead = _johnLemonView.Head;
            _johnLemonLifeModel.OnLemonDied += StartDeath;
        }

        private void StartDeath()
        {
            _johnLemonLifeModel.OnLemonDied -= StartDeath;
            _coroutine = Die().ToObservable().Subscribe();
        }

        private IEnumerator Die()
        {
            yield return new WaitForSeconds(JohnLemonTimings.DEATH_DELAY);
            _johnLemonHead.SetActive(true);
            _johnLemonHead.transform.SetParent(null);
            _johnLemonView.gameObject.SetActive(false);
        }

        public void Cleanup()
        {
            _johnLemonLifeModel.OnLemonDied -= StartDeath;
            _coroutine?.Dispose();
        }
    }
}