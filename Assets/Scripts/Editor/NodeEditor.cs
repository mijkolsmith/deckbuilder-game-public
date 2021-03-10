using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

public enum UserActions
{
	ADD_STORY_NODE,
	ADD_CHOICE_NODE,
	CHANGE_TITLE,
	ADD_DIALOGUE,
	REMOVE_DIALOGUE,
	DELETE_NODE
}

public class NodeEditor : EditorWindow
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
		NodeEditor window = (NodeEditor)EditorWindow.GetWindow(typeof(NodeEditor));
		window.minSize = new Vector2(1024, 768);
		window.Show();
	}

	private void OnGUI()
	{
		/* some basic GUI functions:
		GUILayout.Label("Base Settings", EditorStyles.boldLabel);
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
		SaveStoryNodes();
	}

	private void OnEnable()
	{
		LoadStoryNodes();
	}

	public void SaveStoryNodes()
	{
		string[] fileNames = AssetDatabase.FindAssets("t:" + typeof(StoryNode).ToString());
		//AssetDatabase.DeleteAssets(fileNames, new List<string>());
		List<StoryNode> foundObjects = new List<StoryNode>();
		int nameNumber = 1;

		foreach (var fileName in fileNames)
		{
			nameNumber++;
			foundObjects.Add(AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(fileName), typeof(StoryNode)) as StoryNode);
		}

		foreach (StoryNode node in windows)
		{
			string fileName = "StoryNode " + nameNumber;
			nameNumber++;

			if (!foundObjects.Contains(node))
			{
				AssetDatabase.CreateAsset(node, "Assets/StoryNodes/" + fileName + ".asset");
				AssetDatabase.SaveAssets();
			}
		}

		windows.Clear();
	}

	public void LoadStoryNodes()
	{
		string[] fileNames = AssetDatabase.FindAssets("t:" + typeof(StoryNode).ToString());

		foreach (var fileName in fileNames)
		{
			StoryNode foundObject = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(fileName), typeof(StoryNode)) as StoryNode;
			if (foundObject != null)
			{
				windows.Add(foundObject as StoryNode);
			}
		}
	}

	void DrawWindows()
	{
		BeginWindows();

		foreach (BaseNode n in windows)
		{
			n.DrawCurves();
		}

		for (int i = windows.Count - 1; i >= 0; i--)
		{
			if (windows[i] == null)
			{
				continue;
			}

			//change gui.window to own class
			windows[i].windowRect = GUI.Window(i, windows[i].windowRect, DrawNodeWindow, windows[i].windowTitle);
		}

		EndWindows();
	}

	void DrawNodeWindow(int id)
	{
		if (id >= windows.Count)
		{
			return;
		}

		windows[id].DrawWindow();
		GUI.DragWindow();
	}

	//Check the user input
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

		//Delete currently selected node
		if (e.keyCode == KeyCode.Delete && !makeTransition)
		{
			if (e.type == UnityEngine.EventType.KeyDown)
			{
				ContextCallback(UserActions.DELETE_NODE);
			}
		}
	}

	//When lmb is pressed
	void LeftClick(Event e)
	{
		selectedNode = null;
		for (int i = windows.Count - 1; i >= 0; i--)
		{
			if (windows[i].windowRect.Contains(e.mousePosition))
			{
				selectedNode = windows[i];
				break;
			}
		}
	}

	//When rmb is pressed
	void RightClick(Event e)
	{
		selectedNode = null;
		for (int i = 0; i < windows.Count; i++)
		{
			if (windows[i].windowRect.Contains(e.mousePosition))
			{
				clickedOnWindow = true;
				selectedNode = windows[i];
				e.button = 0;
				break;
			}
		}

		if (clickedOnWindow && selectedNode != null)
		{
			ModifyNode(e);
		}
		else
		{
			AddNewNode(e);
		}
	}

	//Create rmb press menu for adding new nodes
	void AddNewNode(Event e)
	{
		GenericMenu menu = new GenericMenu();

		//menu.AddSeparator("");
		menu.AddItem(new GUIContent("Add Dialogue"), false, ContextCallback, UserActions.ADD_STORY_NODE);
		menu.AddItem(new GUIContent("Add Choice"), false, ContextCallback, UserActions.ADD_CHOICE_NODE);

		menu.ShowAsContext();
		e.Use();
	}

	//Create rmb press menu for modifying existing nodes
	void ModifyNode(Event e)
	{
		GenericMenu menu = new GenericMenu();

		selectedNode.ContextMenu(menu, ContextCallback);

		menu.ShowAsContext();
		e.Use();
	}

	//Check which UserAction has been selected in the contextmenu, and execute code accordingly
	void ContextCallback(object o)
	{
		UserActions a = (UserActions)o;
		switch (a)
		{
			//creation
			case UserActions.ADD_STORY_NODE:
				StoryNode storyNode = CreateInstance<StoryNode>();
				storyNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 300);
				storyNode.windowTitle = "StoryNode";

				selectedNode = storyNode;
				storyNode.Init();
				windows.Add(storyNode);
				break;

			case UserActions.ADD_CHOICE_NODE:
				ChoiceNode choiceNode = CreateInstance<ChoiceNode>();
				choiceNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 300);
				choiceNode.windowTitle = "ChoiceNode";

				selectedNode = choiceNode;
				choiceNode.Init();
				windows.Add(choiceNode);
				break;

			//editing
			case UserActions.CHANGE_TITLE:
				selectedNode.changeTitle = false;
				break;

			case UserActions.ADD_DIALOGUE:
				selectedNode.textAreas.Add("new text area");
				break;

			case UserActions.REMOVE_DIALOGUE:
				selectedNode.textAreas.RemoveAt(selectedNode.textAreas.Count - 1);
				break;

			//deletion
			case UserActions.DELETE_NODE:
				if(selectedNode != null)
				{
					windows.Remove(selectedNode);
					//remove leaves an empty object behind and we don't want that
					windows.RemoveAll(item => item == null);
					selectedNode = null;
					Repaint();
				}
				break;

			default:
				break;
		}
	}

	public void GenerateStories()
	{
		int nameNumber = 1;
		foreach (StoryNode node in windows)
		{
			Story story = new Story();

			for(int i = 0; i < node.textAreas.Count; i++)
			{
				story.stories[i] = node.textAreas[i];
			}

			string fileName = "Story " + nameNumber;
			nameNumber++;

			AssetDatabase.CreateAsset(story, Application.dataPath + "/Stories/" + fileName + ".asset");
			AssetDatabase.SaveAssets();
		}
	}
}