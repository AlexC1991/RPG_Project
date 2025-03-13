using UnityEngine;

namespace RPGGame
{
    [CreateAssetMenu(fileName = "PowerSelectionScript", menuName = "ScriptableObjects/PowerSelectionScript", order = 0)]
    public class PowerSelectionScript : ScriptableObject
    {
        public enum Powers
        {
            Fire,
            Water,
            Earth,
            Air
        }
        public struct PowerSelection
        {
            public int level;
            public string id;
            public string name;
            public Powers power;
            public float damage;
            public float range;
            public float cooldown;
            public Sprite icon;
        }
    }
}
