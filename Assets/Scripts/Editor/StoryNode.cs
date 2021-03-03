using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StoryNode : BaseNode
{
	public override void DrawWindow()
	{
		if (!changeTitle)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Title: ", GUILayout.Width(50));
			windowTitle = EditorGUILayout.TextField(windowTitle, GUILayout.Width(100));
			changeTitle = EditorGUILayout.Toggle(changeTitle);
			EditorGUILayout.EndHorizontal();
		}
		
		base.DrawWindow();
	}

	public override void DrawCurves()
	{
		base.DrawCurves();
	}

#if UNITY_EDITOR
	public override void ContextMenu(GenericMenu menu, GenericMenu.MenuFunction2 contextCallback)
	{
		base.ContextMenu(menu, contextCallback);
	}
#endif
}
