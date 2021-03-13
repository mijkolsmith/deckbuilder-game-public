using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class BaseNode : ScriptableObject
{
	public Rect windowRect;
	public string windowTitle;

	public bool changeTitle;
	public List<string> textAreas;

	private NodeEditor nodeEditor;

	public virtual void Init()
	{
		textAreas = new List<string>();
	}

	public virtual void DrawWindow()
	{
		if (!changeTitle)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Title: ", GUILayout.Width(50));
			windowTitle = EditorGUILayout.TextField(windowTitle, GUILayout.Width(100));
			changeTitle = EditorGUILayout.Toggle(changeTitle);
			EditorGUILayout.EndHorizontal();
			
			if (nodeEditor != null)
			{
				nodeEditor.CheckName();
			}
			else
			{
				Debug.Log(windowTitle + " bug");
			}
		}
		DrawTextAreas();
	}

	public virtual void DrawCurves() { }
	public virtual void DrawTextAreas()
	{
		foreach (string s in textAreas)
		{
			GUILayout.TextArea(s, 200);
		}
	}

	public virtual void SetNodeEditor(NodeEditor nodeEditor)
	{
		this.nodeEditor = nodeEditor;
	}

#if UNITY_EDITOR
	public virtual void ContextMenu(GenericMenu menu, GenericMenu.MenuFunction2 contextCallback)
	{
		menu.AddItem(new GUIContent("Change Title"), false, contextCallback, UserActions.CHANGE_TITLE);
		menu.AddItem(new GUIContent("Add Text"), false, contextCallback, UserActions.ADD_DIALOGUE);
		menu.AddItem(new GUIContent("Remove Last Text"), false, contextCallback, UserActions.REMOVE_DIALOGUE);
		menu.AddSeparator("");
		menu.AddItem(new GUIContent("Delete Node"), false, contextCallback, UserActions.DELETE_NODE);
	}
#endif
}