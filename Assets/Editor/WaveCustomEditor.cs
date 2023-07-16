using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WaveData)), CanEditMultipleObjects]
public class WaveDataInspector : Editor
{
	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		EditorList.Show(serializedObject.FindProperty("waveInstructions"), EditorList.EditorListOption.Buttons | EditorList.EditorListOption.ElementLabels);
		//EditorGUILayout.PropertyField(serializedObject.FindProperty("vectors"));
		//EditorGUILayout.PropertyField(serializedObject.FindProperty("colorPoints"));
		//EditorGUILayout.PropertyField(serializedObject.FindProperty("objects"));
		serializedObject.ApplyModifiedProperties();
	}
}