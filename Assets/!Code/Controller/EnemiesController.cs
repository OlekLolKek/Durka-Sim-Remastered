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
        private readonly KuvaldinBossAI _kuvaldinBoss;
        private readonly SpriteAnimatorConfig _robotConfig;

        private readonly bool _jamBossExists = true;
        private readonly bool _kuvaldinBossExists = true;

        public EnemiesController(AIConfig robotConfig, Transform playerTransform, 
            SpriteAnimatorConfig robotAnimationConfig,
            SpriteAnimatorConfig jamBossAnimationConfig, 
            SpriteAnimatorConfig kuvaldinBossAnimationConfig,
            AIConfig jamBossConfig, AIConfig kuvaldinAIConfig)
        {
            var seekers = Object.FindObjectsOfType<Seeker>().ToList();

            foreach (var seeker in seekers)
            {
                if (!seeker.TryGetComponent(out EnemyView levelObjectView))
                {
                    throw new ArgumentNullException($"{seeker} doesn't have a {typeof(LevelObjectView)} component.");
                }
                
                _crawlers.Add(new StalkerAI(levelObjectView, robotConfig, 
                    seeker, playerTransform, 
                    robotAnimationConfig));
            }

            var jamView = Object.FindObjectOfType<JamBossView>();

            if (jamView != null)
            {
                _jamBoss = new JamBossAI(jamView, jamBossConfig, 
                    jamBossAnimationConfig);
            }
            else
            {
                _jamBossExists = false;
            }

            var kuvaldinView = Object.FindObjectOfType<KuvaldinBossView>();

            if (kuvaldinView != null)
            {
                _kuvaldinBoss = new KuvaldinBossAI(kuvaldinView,kuvaldinBossAnimationConfig,
                    playerTransform, kuvaldinAIConfig);
            }
            else
            {
                _kuvaldinBossExists = false;
            }
        }

        public void Initialize()
        {
            if (_jamBossExists)
            {
                _jamBoss.Initialize();
            }
            if (_kuvaldinBossExists)
            {
                _kuvaldinBoss.Initialize();
            }
        }

        public void Execute(float deltaTime)
        {
            foreach (var crawler in _crawlers)
            {
                crawler.Execute(deltaTime);
            }

            if (_jamBossExists)
            {
                _jamBoss.Execute(deltaTime);
            }

            if (_kuvaldinBossExists)
            {
                _kuvaldinBoss.Execute(deltaTime);
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