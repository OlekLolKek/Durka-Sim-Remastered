using System;
using System.Collections.Generic;
using System.Linq;
using DurkaSimRemastered.Interface;
using Pathfinding;
using UnityEngine;
using Object = UnityEngine.Object;


namespace DurkaSimRemastered
{
    public class EnemiesController : IInitialize, IExecute, IFixedExecute, ICleanup
    {
        private readonly List<StalkerAI> _crawlers = new List<StalkerAI>();
        private readonly JamBossAI _jamBoss;
        private readonly SpriteAnimatorConfig _robotConfig;

        public EnemiesController(AIConfig robotConfig, Transform playerTransform, SpriteAnimatorConfig robotAnimationConfig,
            SpriteAnimatorConfig jamBossAnimationConfig, AIConfig jamBossConfig)
        {
            var seekers = Object.FindObjectsOfType<Seeker>().ToList();

            foreach (var seeker in seekers)
            {
                if (!seeker.TryGetComponent(out EnemyView levelObjectView))
                {
                    throw new ArgumentNullException($"{seeker} doesn't have a {typeof(LevelObjectView)} component.");
                }
                
                _crawlers.Add(new StalkerAI(levelObjectView, robotConfig, seeker, playerTransform, robotAnimationConfig));
            }

            var jamView = Object.FindObjectOfType<JamBossView>();

            _jamBoss = new JamBossAI(jamView, jamBossConfig, jamBossAnimationConfig);
        }

        public void Initialize()
        {
            _jamBoss.Initialize();
        }

        public void Execute(float deltaTime)
        {
            foreach (var crawler in _crawlers)
            {
                crawler.Execute(deltaTime);
            }
        }

        public void FixedExecute()
        {
            foreach (var crawler in _crawlers)
            {
                crawler.FixedExecute();
            }
        }

        public void Cleanup()
        {
            foreach (var crawler in _crawlers)
            {
                crawler.Cleanup();
            }
        }
    }
}