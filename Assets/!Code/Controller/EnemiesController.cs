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
        private readonly List<BubAI> _bubs = new List<BubAI>();
        private readonly JamBossAI _jamBoss;
        private readonly KuvaldinBossAI _kuvaldinBoss;

        private readonly bool _jamBossExists = true;
        private readonly bool _kuvaldinBossExists = true;

        public EnemiesController(AIConfig robotConfig, Transform playerTransform, 
            SpriteAnimatorConfig robotAnimationConfig,
            SpriteAnimatorConfig shizikAnimationConfig,
            SpriteAnimatorConfig jamBossAnimationConfig, 
            SpriteAnimatorConfig kuvaldinBossAnimationConfig,
            SpriteAnimatorConfig bubAnimationConfig,
            AIConfig jamBossConfig, AIConfig kuvaldinAIConfig,
            AIConfig bubAIConfig)
        {
            var seekers = Object.FindObjectsOfType<Seeker>().ToList();

            foreach (var seeker in seekers)
            {
                if (seeker.TryGetComponent(out CrawlerView crawlerView))
                {
                    _crawlers.Add(new StalkerAI(crawlerView, robotConfig, 
                        seeker, playerTransform, 
                        robotAnimationConfig));
                }
                else if (seeker.TryGetComponent(out ShizikView shizikView))
                {
                    _crawlers.Add(new StalkerAI(shizikView, robotConfig,
                        seeker, playerTransform, shizikAnimationConfig
                        ));
                }
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
                _kuvaldinBoss = new KuvaldinBossAI(kuvaldinView, kuvaldinBossAnimationConfig,
                    playerTransform, kuvaldinAIConfig);
            }
            else
            {
                _kuvaldinBossExists = false;
            }

            var bubs = Object.FindObjectsOfType<BubView>();

            foreach (var bub in bubs)
            {
                _bubs.Add(new BubAI(bub, bubAnimationConfig,
                    playerTransform, bubAIConfig));
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

            foreach (var bub in _bubs)
            {
                bub.Execute(deltaTime);
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

            foreach (var bub in _bubs)
            {
                bub.Cleanup();
            }

            if (_kuvaldinBossExists)
            {
                _kuvaldinBoss.Cleanup();
            }

            if (_jamBossExists)
            {
                _jamBoss.Cleanup();
            }
        }
    }
}