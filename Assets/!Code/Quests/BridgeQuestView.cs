using DG.Tweening;
using DurkaSimRemastered;
using DurkaSimRemastered.Interface;
using UnityEngine;
using AnimationState = DurkaSimRemastered.AnimationState;


namespace Quests
{
    public class BridgeQuestView : QuestObjectView, IInjectSpriteAnimator
    {
        [SerializeField] private AudioSource _thisAudioSource;
        [SerializeField] private Transform _bridge;
        [SerializeField] private Vector3 _finalPosition;
        [SerializeField] private float _tweenTime;

        private SpriteAnimator _spriteAnimator;

        private const float ANIMATION_SPEED = 10.0f;
        
        public override void ProcessComplete()
        {
            _bridge.DOMove(_finalPosition, _tweenTime);
            _thisAudioSource.Play();
            _spriteAnimator.StartAnimation(_spriteRenderer, AnimationState.Attack, false, ANIMATION_SPEED);
        }

        public void InjectSpriteAnimator(SpriteAnimator spriteAnimator)
        {
            _spriteAnimator = spriteAnimator;
        }
    }
}