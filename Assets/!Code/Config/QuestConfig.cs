using UnityEngine;


namespace DurkaSimRemastered
{
    [CreateAssetMenu(fileName = "QuestConfig", menuName = "Configs/QuestConfig")]
    public class QuestConfig : ScriptableObject
    {
        [SerializeField] private int _id;
        [SerializeField] private QuestType _questType;
        
        public int ID => _id;
        public QuestType QuestType => _questType;
    }
}