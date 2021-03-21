using System;
using System.Collections.Generic;
using System.Linq;
using DurkaSimRemastered.Interface;
using Pathfinding;
using UnityEngine;
using Object = UnityEngine.Object;


namespace DurkaSimRemastered
{
    public class EnemiesController : IExecute, IFixedExecute
    {
        private List<StalkerAI> _crawlers = new List<StalkerAI>();

        public EnemiesController(AIConfig aiConfig, Transform playerTransform)
        {
            var seekers = Object.FindObjectsOfType<Seeker>().ToList();
            
            foreach (var seeker in seekers)
            {
                if (!seeker.TryGetComponent(out LevelObjectView levelObjectView))
                {
                    throw new ArgumentNullException($"{seeker} doesn't have a {typeof(LevelObjectView)} component.");
                }
                
                _crawlers.Add(new StalkerAI(levelObjectView, aiConfig, seeker, playerTransform));
            }
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
    }
}