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
        
        [System.Serializable]
        public struct PowerSelection
        {
            public int level;
            public string id;
            public string name;
            public Powers power;
            public float damage;
            public float range;
            public float velocity; 
            public float coolDownTimer;
            public Sprite icon;
            public GameObject abilityPrefabs;
            public AnimationClip abilityAnimationClip;
            public float abilityPrice;
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
