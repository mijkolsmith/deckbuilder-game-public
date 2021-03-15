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
	ADD_CURVE,
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
	bool drawCurveOnMousePos;
	BaseNode startNode;
	BaseNode selectedNode;

	int windowWidth = 200;
	int windowHeight = 300;

	[MenuItem("NodeEditor/Node Editor")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		NodeEditor window = (NodeEditor)EditorWindow.GetWindow(typeof(NodeEditor));
		window.minSize = new Vector2(1024, 768);
		window.Show();
	}

	#region GUI Methods
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
		if (drawCurveOnMousePos)
		{
			DrawCursorCurve();
		}
	}

	private void OnDisable()
	{
		SaveStoryNodes();
		drawCurveOnMousePos = false;
	}

	private void OnEnable()
	{
		LoadStoryNodes();
		foreach (BaseNode window in windows)
		{
			window.SetNodeEditor(this);
		}
	}

	void DrawWindows()
	{
		BeginWindows();
		
		if (GUILayout.Button("Generate Stories", GUILayout.Width(120)))
		{
			GenerateStories();
		}

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

			//TODO: change gui.window to own class
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

	//Draw the curve when making a new one
	void DrawCursorCurve()
	{
		StoryNode tempNode = CreateInstance<StoryNode>();
		tempNode.windowRect = new Rect(mousePos.x, mousePos.y, 0, 0);
		Curve tempCurve = new Curve(startNode, tempNode, Color.black);
		DrawNodeCurve(tempCurve);
		Repaint();
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
				if (drawCurveOnMousePos)
				{
					Curve curve = new Curve(startNode, selectedNode, Color.black);
					SaveConnection(curve);
				}
				break;
			}
		}
		drawCurveOnMousePos = false;
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
				storyNode.windowRect = new Rect(mousePos.x, mousePos.y, windowWidth, windowHeight);
				storyNode.windowTitle = "StoryNode";
				storyNode.SetNodeEditor(this);

				selectedNode = storyNode;
				storyNode.Init();
				windows.Add(storyNode);
				break;

			case UserActions.ADD_CHOICE_NODE:
				ChoiceNode choiceNode = CreateInstance<ChoiceNode>();
				choiceNode.windowRect = new Rect(mousePos.x, mousePos.y, windowWidth, windowHeight);
				choiceNode.windowTitle = "ChoiceNode";
				choiceNode.SetNodeEditor(this);

				selectedNode = choiceNode;
				choiceNode.Init();
				windows.Add(choiceNode);
				break;

			//editing
			case UserActions.CHANGE_TITLE:
				selectedNode.changeTitle = false;
				break;

			case UserActions.ADD_CURVE:
				drawCurveOnMousePos = true;
				startNode = selectedNode;
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
	#endregion

	#region Saving and Loading
	public void SaveStoryNodes()
	{
		string[] fileNames = AssetDatabase.FindAssets("t:" + typeof(StoryNode).ToString());
		List<StoryNode> foundObjects = new List<StoryNode>();

		foreach (var fileName in fileNames)
		{
			StoryNode foundObject = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(fileName), typeof(StoryNode)) as StoryNode;

			if (windows.Contains(foundObject))
			{
				foundObjects.Add(foundObject);
			}
			else
			{
				AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(fileName));
			}
		}

		foreach (StoryNode node in windows)
		{
			string fileName = node.windowTitle;

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

	private void GenerateStories()
	{
		string[] fileNames = AssetDatabase.FindAssets("t:" + typeof(Story).ToString());
		List<Story> foundObjects = new List<Story>();

		foreach (string fileName in fileNames)
		{
			Story foundObject = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(fileName), typeof(Story)) as Story;
			foundObjects.Add(foundObject);
			AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(fileName));
		}

		foreach (StoryNode node in windows)
		{
			Story story = CreateInstance<Story>();

			for(int i = 0; i < node.textAreas.Count; i++)
			{
				story.dialogue.Add(node.textAreas[i]);
			}

			string fileName = node.windowTitle;

			if (!foundObjects.Contains(story))
			{
				AssetDatabase.CreateAsset(story, "Assets/Stories/" + fileName + ".asset");
				AssetDatabase.SaveAssets();
			}
		}
	}

	public void CheckName()
	{
		if (selectedNode != null)
		{
			foreach (var window in windows)
			{
				if (selectedNode.windowTitle == window.windowTitle && selectedNode != window)
				{
					selectedNode.windowTitle += " 2";
				}
			}
		}
	}
	#endregion

	#region Helper Methods
	//Draw the curves for each saved curve
	public static void DrawNodeCurve(Curve curve)
	{
		Rect start = curve.startNode.windowRect;
		Rect end = curve.endNode.windowRect;
		Color curveColor = curve.color;

		if (end == start)
		{
			return;
		}

		Vector3 startPos = new Vector3(
			start.x + start.width,
			start.y + start.height * .5f,
			0);

		Vector3 endPos = new Vector3(
			end.x,
			end.y + end.height * .5f,
			0);

		Vector3 startTan = startPos + Vector3.right * 100;
		Vector3 endTan = endPos + Vector3.left * 100;

		Color shadow = new Color(0, 0, 0, 0.1f);

		for (int i = 0; i < 3; i++)
		{
			Handles.DrawBezier(startPos, endPos, startTan, endTan, shadow, null, (i + 5) * .5f);
		}

		Handles.DrawBezier(startPos, endPos, startTan, endTan, curveColor, null, 5);
	}

	public void SaveConnection(Curve curve)
	{
		/*foreach (Curve tempCurve in startNode.curves)
		{
			if (tempCurve.endNode == curve.endNode)
			{
				return;
			}
		}*/

		if (!startNode.curves.Contains(startNode.curves.First(x => x.endNode == curve.endNode)))
		{
			startNode.curves.Add(curve);
		}

		startNode = null;
	}
	#endregion
}