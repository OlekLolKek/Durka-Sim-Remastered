using DG.Tweening;
using UnityEngine;


namespace Quests
{
    public class BridgeQuestView : QuestObjectView
    {
        [SerializeField] private Transform _bridge;
        [SerializeField] private Vector3 _finalPosition;
        [SerializeField] private float _tweenTime;
        
        public override void ProcessComplete()
        {
            _bridge.DOMove(_finalPosition, _tweenTime);
        }
    }
}