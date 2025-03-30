using UnityEditor;
using UnityEngine;

namespace RPGGame
{
    [CustomEditor(typeof(PowerSelectionScript))]
    public class PowerSelectionEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Get the target object
            PowerSelectionScript script = (PowerSelectionScript)target;

            // Draw shared properties
            EditorGUILayout.LabelField("Shared Properties", EditorStyles.boldLabel);
            script.powerData.level = EditorGUILayout.IntField("Level", script.powerData.level);
            script.powerData.id = EditorGUILayout.TextField("ID", script.powerData.id);
            script.powerData.name = EditorGUILayout.TextField("Name", script.powerData.name);
            script.powerData.powerType =
                (PowerSelectionScript.PowerType)EditorGUILayout.EnumPopup("Power Type", script.powerData.powerType);
            script.powerData.power =
                (PowerSelectionScript.Powers)EditorGUILayout.EnumPopup("Power", script.powerData.power);
            script.powerData.coolDownTimer =
                EditorGUILayout.FloatField("Cooldown Timer", script.powerData.coolDownTimer);
            script.powerData.icon =
                (Sprite)EditorGUILayout.ObjectField("Icon", script.powerData.icon, typeof(Sprite), false);
            script.powerData.abilityPrefabs = (GameObject)EditorGUILayout.ObjectField("Ability Prefabs",
                script.powerData.abilityPrefabs, typeof(GameObject), false);
            script.powerData.abilityAnimationClip = (AnimationClip)EditorGUILayout.ObjectField("Ability Animation Clip",
                script.powerData.abilityAnimationClip, typeof(AnimationClip), false);
            script.powerData.abilityPrice = EditorGUILayout.FloatField("Ability Price", script.powerData.abilityPrice);

            // Expose only the struct associated with the selected PowerType
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Specific Properties", EditorStyles.boldLabel);
            switch (script.powerData.powerType)
            {
                case PowerSelectionScript.PowerType.Projectile:
                    DrawProjectileProperties(ref script.powerData.projectileType);
                    break;

                case PowerSelectionScript.PowerType.Buff:
                    DrawBuffProperties(ref script.powerData.buffType);
                    break;

                case PowerSelectionScript.PowerType.AOE:
                    DrawAOEProperties(ref script.powerData.aeoType);
                    break;
            }

            // Save changes
            if (GUI.changed)
            {
                EditorUtility.SetDirty(script);
            }
        }

        private void DrawProjectileProperties(ref PowerSelectionScript.ProjectileType projectile)
        {
            projectile.range = EditorGUILayout.FloatField("Range", projectile.range);
            projectile.speed = EditorGUILayout.FloatField("Speed", projectile.speed);
            projectile.damage = EditorGUILayout.FloatField("Damage", projectile.damage);
        }

        private void DrawBuffProperties(ref PowerSelectionScript.BuffType buff)
        {
            buff.duration = EditorGUILayout.FloatField("Duration", buff.duration);
            buff.buffAbility =
                (PowerSelectionScript.BuffAbility)EditorGUILayout.EnumPopup("Buff Ability", buff.buffAbility);
            buff.buffValue = EditorGUILayout.FloatField("Buff Value", buff.buffValue);
        }

        private void DrawAOEProperties(ref PowerSelectionScript.AEOType aoe)
        {
            aoe.radius = EditorGUILayout.FloatField("Radius", aoe.radius);
            aoe.duration = EditorGUILayout.FloatField("Duration", aoe.duration);
            aoe.abilityValue = EditorGUILayout.FloatField("Ability Value", aoe.abilityValue);
            aoe.buffAbility = (PowerSelectionScript.BuffAbility)EditorGUILayout.EnumPopup("Buff Ability", aoe.buffAbility);
        }
    }
}