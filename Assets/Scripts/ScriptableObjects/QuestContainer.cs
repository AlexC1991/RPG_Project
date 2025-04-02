using System.Collections.Generic;
using UnityEngine;

namespace RPGGame
{
    [CreateAssetMenu(fileName = "QuestContainer", menuName = "RPG Game SO/Quest Container", order = 3)]
    public class QuestContainer : ScriptableObject
    {
        public List<QuestMaker> LocationTheNewbieCity = new List<QuestMaker>();

        public List<QuestMaker> TheWhisperingForest = new List<QuestMaker>();

        public List<QuestMaker> TheDryLands = new List<QuestMaker>();

        public List<QuestMaker> TheFrozenPassage = new List<QuestMaker>();

        public List<QuestMaker> TheDarkUnderworld = new List<QuestMaker>();

        public List<QuestMaker> GoldCity = new List<QuestMaker>();

        public List<QuestMaker> TheLostIslands = new List<QuestMaker>();

        public List<QuestMaker> Robotica = new List<QuestMaker>();

        public List<QuestMaker> CurrentQuestListActive = new List<QuestMaker>();

        public List<QuestMaker> CurrentQuestListCompleted = new List<QuestMaker>();

        public List<QuestMaker> CurrentQuestListFailed = new List<QuestMaker>();
    }
}