using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StoryManager))]
public class StoryUpdaterEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		StoryManager myScript = (StoryManager)target;
		if (GUILayout.Button("Load Objects"))
		{
			myScript.LoadObjects();
		}
	}
}