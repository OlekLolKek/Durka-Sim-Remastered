using System;
using UnityEngine;


namespace Random_Generation
{
    public class LevelGeneratorStarter : MonoBehaviour
    {
        [SerializeField] private GenerateLevelView _generateLevelView;
        private LevelGeneratorController _levelGeneratorController;

        private void Awake()
        {
            _levelGeneratorController = new LevelGeneratorController(_generateLevelView);
            
            _levelGeneratorController.Awake();
        }
    }
}