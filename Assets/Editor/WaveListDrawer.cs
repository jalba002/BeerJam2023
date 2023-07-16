using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(WaveDataEntry))]
public class WaveListDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        Rect contentPosition = EditorGUI.PrefixLabel(position, GUIContent.none);
        if (position.height > 16f)
        {
            position.height = 16f;
            EditorGUI.indentLevel += 1;
            contentPosition = EditorGUI.IndentedRect(position);
            contentPosition.y += 16f;
        }

        var verbIndx = property.FindPropertyRelative("verb").enumValueIndex;
        var verb = (Verbs)verbIndx;

        switch (verb)
        {
            case Verbs.ContainerRandom:
                contentPosition.width *= 0.33f;
                EditorGUI.indentLevel = 0;
                EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("verb"), GUIContent.none);
                contentPosition.x += contentPosition.width;
                EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("amount"), GUIContent.none);
                contentPosition.x += contentPosition.width;
                EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("side"), GUIContent.none);
                break;
            case Verbs.Container:
                contentPosition.width *= 0.325f;
                EditorGUI.indentLevel = 0;
                EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("verb"), GUIContent.none);
                contentPosition.x += contentPosition.width;
                EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("amount"), GUIContent.none);
                contentPosition.x += contentPosition.width;
                EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("doorStatus"), GUIContent.none);
                break;
            case Verbs.Spawn:
                contentPosition.width *= 0.325f;
                EditorGUI.indentLevel = 0;
                EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("verb"), GUIContent.none);
                contentPosition.x += contentPosition.width;
                EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("amount"), GUIContent.none);
                contentPosition.x += contentPosition.width;
                EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("side"), GUIContent.none);
                break;
            case Verbs.ContainerCloseAll:
                EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("verb"), GUIContent.none);
                break;
            case Verbs.Wait:
                contentPosition.width *= 0.5f;
                EditorGUI.indentLevel = 0;
                EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("verb"), GUIContent.none);
                contentPosition.x += contentPosition.width;
                EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("amount"), GUIContent.none);
                break;
            case Verbs.PlaySound:
                contentPosition.width *= 0.5f;
                EditorGUI.indentLevel = 0;
                EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("verb"), GUIContent.none);
                contentPosition.x += contentPosition.width;
                EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("information"), GUIContent.none);
                break;
            case Verbs.None:
                EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("verb"), GUIContent.none);
                break;
        }

        contentPosition.width /= 3f;
        EditorGUIUtility.labelWidth = 14f;
        EditorGUI.EndProperty();
    }

    //private void AddPropertyField(Rect pos, SerializedProperty prop, string text, string tooltip)
    //{
    //    EditorGUI.PropertyField(pos, prop, new GUIContent(text, tooltip));
    //}

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return Screen.width < 333 ? (16f + 18f) : 16f;
    }
}