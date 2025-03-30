using UnityEngine;
using UnityEngine.Serialization;

namespace RPGGame
{
    [CreateAssetMenu(fileName = "Base Power Blueprint Template", menuName = "RPG Game SO/Power Template", order = 0)]
    public class PowerSelectionScript : ScriptableObject
    {
        [System.Serializable]
        public enum Powers
        {
            Fire,
            Water,
            Earth,
            Air
        }
        public enum PowerType
        {
            Projectile,
            Buff,
            AOE
        }
        public enum BuffAbility
        {
            Health,
            Damage,
            Speed
        }

        
        [System.Serializable]
        public struct PowerSelection
        {
            public int level;
            public string id;
            public string name;
            public PowerType powerType;
            public Powers power;
            public float coolDownTimer;
            public Sprite icon;
            public GameObject abilityPrefabs;
            public AnimationClip abilityAnimationClip;
            public float abilityPrice;
            public ProjectileType projectileType;
            public BuffType buffType;
            public AEOType aeoType;
        }
        
        [System.Serializable]
        public struct ProjectileType
        {
            public float range;
            public float speed;
            public float damage;
        }
        [System.Serializable]
        public struct BuffType
        {
            public float duration;
            public BuffAbility buffAbility;
            public float buffValue;
        }
        [System.Serializable]
        public struct AEOType
        {
            public float radius;
            public float abilityValue;
            public BuffAbility buffAbility;
            public float duration;
        }
        
        
        public PowerSelection powerData;
        
        private void OnValidate()
        {
#if UNITY_EDITOR
            if (!string.IsNullOrEmpty(powerData.name))
            {
                string assetPath = UnityEditor.AssetDatabase.GetAssetPath(this);
                if (!string.IsNullOrEmpty(assetPath))
                {
                    string newName = powerData.name.Replace(" ", "");
                    UnityEditor.AssetDatabase.RenameAsset(assetPath, newName);
                }
            }
#endif
        }
    }
}
