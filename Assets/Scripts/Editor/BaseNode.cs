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

	public virtual void DrawWindow() { }
	public virtual void DrawCurves() { }
#if UNITY_EDITOR
	public virtual void ContextMenu(GenericMenu menu, GenericMenu.MenuFunction2 contextCallback)
	{
		menu.AddItem(new GUIContent("Change Title"), false, contextCallback, UserActions.CHANGE_TITLE);
		menu.AddItem(new GUIContent("Add Text"), false, contextCallback, UserActions.ADD_DIALOGUE);
		menu.AddSeparator("");
		menu.AddItem(new GUIContent("Delete Node"), false, contextCallback, UserActions.DELETE_NODE);
	}
#endif
}