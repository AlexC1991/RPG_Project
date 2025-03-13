using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LayerSelectorAttribute : PropertyAttribute { }

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(LayerSelectorAttribute))]
public class LayerSelectorPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType == SerializedPropertyType.LayerMask)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Display a LayerMask field that lets the user select multiple layers
            property.intValue = EditorGUI.MaskField(position, label, property.intValue, GetLayerNames());

            EditorGUI.EndProperty();
        }
        else
        {
            EditorGUI.LabelField(position, label.text, "Use LayerSelector with LayerMask.");
        }
    }

    private string[] GetLayerNames()
    {
        var layers = new string[32];
        for (int i = 0; i < 32; i++)
        {
            layers[i] = LayerMask.LayerToName(i);
        }
        return layers;
    }
}
#endif