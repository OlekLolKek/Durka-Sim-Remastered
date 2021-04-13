using System.Collections.Generic;
using UnityEngine;


namespace DurkaSimRemastered
{
    [CreateAssetMenu(fileName = "QuestItemsConfig", menuName = "Configs/QuestItemsConfig")]
    public class QuestItemsConfig : ScriptableObject
    {
        [SerializeField] private int _questId;
        [SerializeField] private List<int> _questItemIdCollection;

        public int QuestId => _questId;
        public List<int> QuestItemIdCollection => _questItemIdCollection;
    }
}