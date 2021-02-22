using System;
using System.Collections.Generic;
using UnityEngine;


namespace DurkaSimRemastered
{
    [CreateAssetMenu(fileName = "SpriteAnimatorConfig", menuName = "Configs/SpriteAnimator", order = 1)]
    public class SpriteAnimatorConfig : ScriptableObject
    {
        [Serializable]
        public sealed class SpriteSequence
        {
            public AnimationState Track;
            public List<Sprite> Sprites = new List<Sprite>();
        }

        public List<SpriteSequence> Sequences = new List<SpriteSequence>();
    }
}