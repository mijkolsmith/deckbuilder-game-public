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
	public bool changeTextAreas;
	public bool changeCurveConditions;

	public List<string> textAreas;
	public List<Curve> curves;
	public State nodeState;

	Vector2 scrollPosition;
	//TODO: add conditions
	string testCondition = "Curve Condition";

	//TODO: add background

	protected NodeEditor nodeEditor;

	public virtual void Init()
	{
		textAreas = new List<string>();
		curves = new List<Curve>();
	}

	#region Draw Functionality
	public virtual void DrawWindow()
	{
		scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
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

		if (!changeTextAreas)
		{
			DrawTextAreas();
		}

		if (!changeCurveConditions)
		{
			DrawCurveConditions();
		}

		EditorGUILayout.EndScrollView();
	}

	public virtual void DrawTextAreas()
	{
		for (int i = 0; i < textAreas.Count; i++)
		{
			textAreas[i] = EditorGUILayout.TextArea(textAreas[i], GUILayout.MinHeight(36));
		}
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

	public virtual void DrawCurveConditions()
	{
		foreach (Curve curve in curves)
		{
			//Curve Condition
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(curve.endNode.ToString(), GUILayout.Width(50));
			testCondition = EditorGUILayout.TextArea(testCondition, GUILayout.Width(100));
			EditorGUILayout.EndHorizontal();
		}
	}
	#endregion

	public virtual LoadableObject CreateObject()
	{
		return null;
	}

	public virtual void Generate()
	{ }

	public virtual void SetNodeEditor(NodeEditor nodeEditor)
	{
		this.nodeEditor = nodeEditor;
	}

#if UNITY_EDITOR
	public virtual void ContextMenu(GenericMenu menu, GenericMenu.MenuFunction2 contextCallback)
	{
		menu.AddItem(new GUIContent("Add TextArea"), false, contextCallback, UserActions.ADD_DIALOGUE);
		menu.AddItem(new GUIContent("Remove Last TextArea"), false, contextCallback, UserActions.REMOVE_DIALOGUE);
		menu.AddItem(new GUIContent("Add Curve"), false, contextCallback, UserActions.ADD_CURVE);
		menu.AddSeparator("");
		string changeTitleString = changeTitle ? "Show Change Title" : "Hide Change Title";
		menu.AddItem(new GUIContent("Hide or Show Items/" + changeTitleString), false, contextCallback, UserActions.CHANGE_TITLE);
		string changeTextAreaString = changeTextAreas ? "Show TextAreas" : "Hide TextAreas";
		menu.AddItem(new GUIContent("Hide or Show Items/" + changeTextAreaString), false, contextCallback, UserActions.CHANGE_TEXTAREAS);
		string changeRequirementString = changeCurveConditions ? "Show Requirements" : "Hide Requirements";
		menu.AddItem(new GUIContent("Hide or Show Items/" + changeRequirementString), false, contextCallback, UserActions.CHANGE_REQUIREMENTS);

		menu.AddSeparator("");
		menu.AddItem(new GUIContent("Delete Node"), false, contextCallback, UserActions.DELETE_NODE);
	}
#endif
}