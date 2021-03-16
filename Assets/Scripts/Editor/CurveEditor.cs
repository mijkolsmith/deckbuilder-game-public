using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CurveEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		BaseNode baseNode = (BaseNode)target;

		int height = 0;
		/*foreach (Curve curve in baseNode.curves)
		{
			height += 100;
			/*if (EditorGUI.LabelField(new Rect(0, height, 100, EditorGUIUtility.singleLineHeight), "name: "))
			{

			}
		}*/
	}
}

[CustomEditor(typeof(StoryNode))]
public class StoryEditor : CurveEditor { }


[CustomEditor(typeof(ChoiceNode))]
public class ChoiceEditor : CurveEditor { }