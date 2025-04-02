using UnityEngine;

namespace RPGGame
{
    [CreateAssetMenu(fileName = "QuestMaker", menuName = "RPG Game SO/Quest Maker", order = 4)]
    public class QuestMaker : ScriptableObject
    {
        [System.Serializable]
        public enum QuestType
        {
            MainQuest,
            SideQuest,
            DailyQuest
        }

        public enum QuestStatus
        {
            NotStarted,
            Started,
            Completed,
            Failed,
            InProgress
        }

        public enum QuestObjective
        {
            Kill,
            Collect,
            Talk,
            Explore,
            Defend,
            Deliver,
            Escort,
        }

        public enum QuestReward
        {
            None,
            CashReward,
            EXP,
            Item,
            Ability,
        }

        public enum QuestTimeLimit
        {
            None,
            Daily,
            Monthly,
            Quarterly,
            Yearly,
            Special,
        }

        public enum FailureCondition
        {
            None,
            ExceedTimeLimit,
            NPCDied,
            AreaAbandoned,
            PlayerDeath
        }

        public enum QuestCondition
        {
            None,
            MandatoryToComplete,
            Optional
        }

        public enum Faction
        {
            None,
            Nomads,
            Bandits,
            Humans,
            Robots,
        }

        public enum EnvironmentalCondition
        {
            None,
            NightTime,
            Raining,
            DuringFestival,
            FullMoon,
            DayTime,
            Snowing
        }

        [System.Serializable]
        public struct TheQuest
        {
            public GameObject[] questObjectives;
            public int questObjectiveAmount;
            public string[] prerequisiteQuestID;
            public string[] npcQuestDialogue;
            public string[] playerQuestDialogue;
            public string questName;
            public Transform questLocation;
            public string questDescription;
            public QuestType questType;
            public QuestStatus questStatus;
            public QuestObjective questObjective;
            public QuestReward questReward;
            public QuestReward secondQuestReward;
            public QuestTimeLimit questTimeLimit;
            public int questLevel;
            public string questID;
            public Sprite questRewardIcon;
            public Sprite questRewardIcon2;
            public int questRewardAmount;
            public int questRewardAmount2;
            public float questExpReward;
            public float rewardScalingFactor;
            public FailureCondition questFailureCondition;
            public QuestMandatoryConditions mandatoryConditions;
            public QuestOptionalConditions optionalConditions;
            public Faction questReputationImpactFaction;
            public int reputationImpactAmount;
            public int questFailedCount;
            public EnvironmentalCondition questEnvironmentRequirement;
        }

        [System.Serializable]
        public struct QuestMandatoryConditions
        {
            public string[] mandatoryQuestDescription;
            public QuestCondition questConditions;
        }

        [System.Serializable]
        public struct QuestOptionalConditions
        {
            public string[] optinalQuestDescription;
            public QuestCondition questConditions;
        }


        public TheQuest questData;

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (!string.IsNullOrEmpty(questData.questName))
            {
                string assetPath = UnityEditor.AssetDatabase.GetAssetPath(this);
                if (!string.IsNullOrEmpty(assetPath))
                {
                    string newName = questData.questName.Replace(" ", "");
                    UnityEditor.AssetDatabase.RenameAsset(assetPath, newName);
                }
            }
#endif
        }
    }
}
