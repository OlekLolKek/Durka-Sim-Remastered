using System;
using System.Collections.Generic;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class SpriteAnimator : IDisposable
    {
        private sealed class Animation
        {
            public AnimationState Track;
            public List<Sprite> Sprites;
            public bool Loop = true;
            public float Speed = 10.0f;
            public float Counter = 0.0f;
            public bool Sleeps;

            public void Update()
            {
                Debug.Log("animation update started");
                if (Sleeps) return;
                Debug.Log("not sleeps");

                Counter += Time.deltaTime * Speed;
                Debug.Log(Counter);

                if (Loop)
                {
                    Debug.Log("Loop");
                    while (Counter > Sprites.Count)
                    {
                        Debug.Log("counter > sprites.count");
                        Counter -= Sprites.Count;
                        Debug.Log($"teper counter = {Counter}");
                    }
                }
                else if (Counter > Sprites.Count)
                {
                    Debug.Log("not loop");
                    Counter = Sprites.Count;
                    Debug.Log($"Counter = {Counter}");
                    Sleeps = true;
                }
            }
        }

        private readonly SpriteAnimatorConfig _config;
        private readonly Dictionary<SpriteRenderer, Animation> _activeAnimations = new Dictionary<SpriteRenderer, Animation>();

        public SpriteAnimator(SpriteAnimatorConfig config)
        {
            _config = config;
        }

        public void StartAnimation(SpriteRenderer spriteRenderer, AnimationState track, bool loop, float speed)
        {
            if (_activeAnimations.TryGetValue(spriteRenderer, out var animation))
            {
                animation.Loop = loop;
                animation.Speed = speed;
                animation.Sleeps = false;
                if (animation.Track != track)
                {
                    animation.Track = track;
                    animation.Sprites = _config.Sequences.Find(sequence => sequence.Track == track).Sprites;
                    animation.Counter = 0.0f;
                }
            }
            else
            {
                _activeAnimations.Add(spriteRenderer, new Animation()
                {
                    Track = track,
                    Sprites = _config.Sequences.Find(sequence => sequence.Track == track).Sprites,
                    Loop = loop,
                    Speed = speed
                });
            }
        }
        
        public void StopAnimation(SpriteRenderer spriteRenderer)
        {
            if (_activeAnimations.ContainsKey(spriteRenderer))
            {
                _activeAnimations.Remove(spriteRenderer);
            }
        }

        public void Update()
        {
            Debug.Log("update started :roflanPominki:");
            
            foreach (var animation in _activeAnimations)
            {
                Debug.Log(animation);
                animation.Value.Update();
                if (animation.Value.Counter < animation.Value.Sprites.Count)
                {
                    animation.Key.sprite = animation.Value.Sprites[(int) animation.Value.Counter];
                }
            }
        }
        
        public void Dispose()
        {
            _activeAnimations.Clear();
        }
    }
}