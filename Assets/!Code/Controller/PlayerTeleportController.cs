using System;
using System.Collections;
using DurkaSimRemastered.Interface;
using Model;
using UniRx;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class PlayerTeleportController : ICleanup
    {
        private IDisposable _teleportCoroutine;
        
        private readonly DoorUseModel _doorUseModel;
        private readonly Transform _playerTransform;

        public PlayerTeleportController(DoorUseModel doorUseModel, PlayerView playerView)
        {
            _doorUseModel = doorUseModel;
            _doorUseModel.OnDoorActivated += Teleport;

            _playerTransform = playerView.transform;
        }

        private void Teleport(DoorView pairDoorView)
        {
            _teleportCoroutine = StartTeleport(pairDoorView).ToObservable().Subscribe();
        }

        private IEnumerator StartTeleport(DoorView pairDoorView)
        {
            yield return new WaitForSeconds(TeleportTimings.FADE_IN_DURATION);
            _playerTransform.position = pairDoorView.transform.position;
        }

        public void Cleanup()
        {
            _doorUseModel.OnDoorActivated -= Teleport;
            _teleportCoroutine?.Dispose();
        }
    }
}