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
	public List<Curve> curves;

	private NodeEditor nodeEditor;

	public virtual void Init()
	{
		textAreas = new List<string>();
		curves = new List<Curve>();
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
			/*else
			{
				Debug.Log(windowTitle + " node editor is null");
			}*/
		}
		DrawTextAreas();
		DrawCurves();
	}

	public virtual void DrawCurves()
	{
		foreach (Curve curve in curves)
		{
			if (curve.endNode == null)
			{
				curves.Remove(curve);
				break;
			}
			NodeEditor.DrawNodeCurve(curve);
		}
	}

	public virtual void DrawTextAreas()
	{
		for (int i = 0; i < textAreas.Count; i++)
		{
			textAreas[i] = EditorGUILayout.TextArea(textAreas[i]);
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
		menu.AddItem(new GUIContent("Add Curve"), false, contextCallback, UserActions.ADD_CURVE);
		menu.AddSeparator("");
		menu.AddItem(new GUIContent("Delete Node"), false, contextCallback, UserActions.DELETE_NODE);
	}
#endif
}