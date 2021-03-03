using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum UserActions
{
	ADD_STORY_NODE,
	ADD_CHOICE_NODE,
	CHANGE_TITLE,
	ADD_DIALOGUE,
	DELETE_NODE
}

public class NodeWindow : EditorWindow
{
	static List<BaseNode> windows = new List<BaseNode>();
	Vector3 mousePos;
	bool makeTransition;
	bool clickedOnWindow;
	BaseNode selectedNode;

	[MenuItem("NodeEditor/Node Editor")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		NodeWindow window = (NodeWindow)EditorWindow.GetWindow(typeof(NodeWindow));
		window.minSize = new Vector2(1024, 768);
		window.Show();
	}

	private void OnGUI()
	{
		/*GUILayout.Label("Base Settings", EditorStyles.boldLabel);
		myString = EditorGUILayout.TextField("Text Field", myString);

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);*/

		Event e = Event.current;
		mousePos = e.mousePosition;
		UserInput(e);
		DrawWindows();
	}

	private void OnDisable()
	{
		//TODO: save the list to a file somewhere
	}

	private void OnEnable()
	{
		windows.Clear();
	}

	void DrawWindows()
	{
		BeginWindows();

		foreach (BaseNode n in windows)
		{
			n.DrawCurves();
		}

		for (int i = 0; i < windows.Count; i++)
		{
			windows[i].windowRect = GUI.Window(i, windows[i].windowRect, DrawNodeWindow, windows[i].windowTitle);
		}

		EndWindows();
	}

	void DrawNodeWindow(int id)
	{
		windows[id].DrawWindow();
		GUI.DragWindow();
	}

	void UserInput(Event e)
	{
		if(e.button == 1 && !makeTransition)
		{
			if(e.type == UnityEngine.EventType.MouseDown)
			{
				RightClick(e);
			}
		}

		if (e.button == 0 && !makeTransition)
		{
			if (e.type == UnityEngine.EventType.MouseDown)
			{
				LeftClick(e);
			}
		}
	}

	void LeftClick(Event e)
	{
		
	}

	void RightClick(Event e)
	{
		selectedNode = null;
		for (int i = 0; i < windows.Count; i++)
		{
			if (windows[i].windowRect.Contains(e.mousePosition))
			{
				clickedOnWindow = true;
				selectedNode = windows[i];
				break;
			}
		}

		if (!clickedOnWindow)
		{
			AddNewNode(e);
		}
		else
		{
			ModifyNode(e);
		}
	}

	void AddNewNode(Event e)
	{
		GenericMenu menu = new GenericMenu();

		//menu.AddSeparator("");
		menu.AddItem(new GUIContent("Add Dialogue"), false, ContextCallback, UserActions.ADD_STORY_NODE);
		menu.AddItem(new GUIContent("Add Choice"), false, ContextCallback, UserActions.ADD_CHOICE_NODE);

		menu.ShowAsContext();
		e.Use();
	}

	void ModifyNode(Event e)
	{
		GenericMenu menu = new GenericMenu();

		selectedNode.ContextMenu(menu, ContextCallback);

		menu.ShowAsContext();
		e.Use();
	}

	void ContextCallback(object o)
	{
		UserActions a = (UserActions)o;
		switch (a)
		{
			//creation
			case UserActions.ADD_STORY_NODE:
				StoryNode storyNode = new StoryNode
				{
					windowRect = new Rect(mousePos.x, mousePos.y, 200, 300),
					windowTitle = "StoryNode"
				};
				windows.Add(storyNode);

				break;
			case UserActions.ADD_CHOICE_NODE:
				ChoiceNode choiceNode = new ChoiceNode
				{
					windowRect = new Rect(mousePos.x, mousePos.y, 200, 300),
					windowTitle = "ChoiceNode"
				};
				windows.Add(choiceNode);

				break;

			//editing
			case UserActions.CHANGE_TITLE:
				selectedNode.changeTitle = false;
				break;
			case UserActions.ADD_DIALOGUE:
				//EditorGUILayout.TextField("Text Field", selectedNode.windowTitle);
				break;


			//deletion
			case UserActions.DELETE_NODE:
				if(selectedNode != null)
				{
					windows.Remove(selectedNode);
				}
				break;
			default:
				break;
		}
	}
}