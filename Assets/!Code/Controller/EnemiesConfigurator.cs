using System;
using Model;
using Pathfinding;
using UnityEngine;


namespace DurkaSimRemastered
{
    //TODO: Remove MonoBehaviour
    public class EnemiesConfigurator : MonoBehaviour
    {
        #region Fields
        
        [Header("Stalker AI")] 
        [SerializeField] private AIConfig _stalkerAIConfig;
        [SerializeField] private LevelObjectView _stalkerAIView;
        [SerializeField] private Seeker _stalkerAISeeker;
        [SerializeField] private Transform _stalkerAITarget;
        
        private StalkerAI _stalkerAI;

        #endregion


        #region Methods

        private void Start()
        {
            _stalkerAI = new StalkerAI(_stalkerAIView, new StalkerAIModel(_stalkerAIConfig), _stalkerAISeeker,
                _stalkerAITarget);
            //TODO: Replace InvokeRepeating with something else
            //TODO: Replace magic numbers with fields
            InvokeRepeating(nameof(RecalculatePath), 0.0f, 1.0f);
        }

        private void FixedUpdate()
        {
            _stalkerAI?.FixedExecute();
        }
        
        private void RecalculatePath()
        {
            _stalkerAI.RecalculatePath();
        }

        #endregion
    }
}