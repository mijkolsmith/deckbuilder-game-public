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
	ADD_BATTLE_NODE,

	CHANGE_TITLE,
	ADD_CURVE,
	ADD_DIALOGUE,
	REMOVE_DIALOGUE,

	DELETE_NODE
}

public class NodeEditor : EditorWindow
{
	List<BaseNode> windows = new List<BaseNode>();
	List<BaseNode> foundNodes = new List<BaseNode>();
	List<LoadableObject> foundObjects = new List<LoadableObject>();
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
		string[] fileNames = AssetDatabase.FindAssets("t:" + typeof(BaseNode).ToString());

		foreach (var fileName in fileNames)
		{
			BaseNode foundObject = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(fileName), typeof(BaseNode)) as BaseNode;

			if (windows.Contains(foundObject))
			{
				foundNodes.Add(foundObject);
			}
			else
			{
				AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(fileName));
			}
		}

		SaveStoryNodes();
		SaveChoiceNodes();
		SaveBattleNodes();

		AssetDatabase.SaveAssets();

		windows.Clear();
		drawCurveOnMousePos = false;
	}

	private void OnEnable()
	{
		LoadStoryNodes();
		LoadChoiceNodes();
		LoadBattleNodes();
		foreach (BaseNode window in windows)
		{
			window.SetNodeEditor(this);
		}
	}

	/// <summary>
	/// Draws all the windows in the window list
	/// </summary>
	void DrawWindows()
	{
		BeginWindows();

		if (GUILayout.Button("Generate ScriptableObjects", GUILayout.Width(170)))
		{
			GenerateScriptableObjects();
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

	/// <summary>
	/// Draw a specific window
	/// </summary>
	/// <param name="id">Window id</param>
	void DrawNodeWindow(int id)
	{
		if (id >= windows.Count)
		{
			return;
		}

		windows[id].DrawWindow();
		GUI.DragWindow();
	}

	/// <summary>
	/// Draw a new curve to your mouse when making it
	/// </summary>
	void DrawCursorCurve()
	{
		StoryNode tempNode = CreateInstance<StoryNode>();
		tempNode.windowRect = new Rect(mousePos.x, mousePos.y, 0, 0);
		Curve tempCurve = new Curve(startNode, tempNode, Color.black);
		DrawNodeCurve(tempCurve);
		Repaint();
	}

	/// <summary>
	/// Check the user input
	/// </summary>
	void UserInput(Event e)
	{
		if (e.button == 1 && !makeTransition)
		{
			if (e.type == UnityEngine.EventType.MouseDown)
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

	/// <summary>
	/// When lmb is pressed
	/// </summary>
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

	/// <summary>
	/// When rmb is pressed
	/// </summary>
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

	/// <summary>
	/// Create rmb press menu for adding new nodes
	/// </summary>
	void AddNewNode(Event e)
	{
		GenericMenu menu = new GenericMenu();

		menu.AddItem(new GUIContent("Add Dialogue Node"), false, ContextCallback, UserActions.ADD_STORY_NODE);
		menu.AddItem(new GUIContent("Add Choice Node"), false, ContextCallback, UserActions.ADD_CHOICE_NODE);
		menu.AddItem(new GUIContent("Add Battle Node"), false, ContextCallback, UserActions.ADD_BATTLE_NODE);

		menu.ShowAsContext();
		e.Use();
	}

	/// <summary>
	/// Create rmb press menu for modifying existing nodes
	/// </summary>
	void ModifyNode(Event e)
	{
		GenericMenu menu = new GenericMenu();

		selectedNode.ContextMenu(menu, ContextCallback);

		menu.ShowAsContext();
		e.Use();
	}

	/// <summary>
	/// Check which UserAction has been selected in the contextmenu, and execute code accordingly
	/// </summary>
	void ContextCallback(object o)
	{
		UserActions a = (UserActions)o;
		switch (a)
		{
			//creation
			case UserActions.ADD_STORY_NODE:
				StoryNode storyNode = CreateInstance<StoryNode>();
				CreateNode(storyNode);
				break;

			case UserActions.ADD_CHOICE_NODE:
				ChoiceNode choiceNode = CreateInstance<ChoiceNode>();
				CreateNode(choiceNode);
				break;

			case UserActions.ADD_BATTLE_NODE:
				BattleNode battleNode = CreateInstance<BattleNode>();
				CreateNode(battleNode);
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
				if (selectedNode != null)
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
	private void SaveStoryNodes()
	{
		List<BaseNode> nodes = windows.Where(x => x.GetType() == typeof(StoryNode)).ToList();

		foreach (StoryNode node in nodes)
		{
			string fileName = node.windowTitle;

			if (!foundNodes.Contains(node))
			{
				AssetDatabase.CreateAsset(node, "Assets/ScriptableObjects/Nodes/" + fileName + ".asset");
			}
		}
	}

	private void LoadStoryNodes()
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
		/*string[] fileNames = AssetDatabase.FindAssets("t:" + typeof(Story).ToString());
		List<Story> foundObjects = new List<Story>();

		foreach (string fileName in fileNames)
		{
			Story foundObject = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(fileName), typeof(Story)) as Story;
			foundObjects.Add(foundObject);
			AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(fileName));
		}*/

		List<BaseNode> nodes = windows.Where(x => x.GetType() == typeof(StoryNode)).ToList();

		foreach (StoryNode node in nodes)
		{
			Story story = CreateInstance<Story>();

			for (int i = 0; i < node.textAreas.Count; i++)
			{
				story.dialogue.Add(node.textAreas[i]);
			}

			//TODO: storyNode can only have one state
			foreach (Curve curve in node.curves)
			{
				story.nextState = curve.endNode.nodeState;
			}

			string fileName = node.windowTitle;

			if (!foundObjects.Contains(story))
			{
				AssetDatabase.CreateAsset(story, "Assets/ScriptableObjects/Objects/" + fileName + ".asset");
				Debug.Log(AssetDatabase.GetAssetPath(story));
			}
		}
	}

	private void SaveChoiceNodes()
	{
		List<BaseNode> nodes = windows.Where(x => x.GetType() == typeof(ChoiceNode)).ToList();

		foreach (ChoiceNode node in nodes)
		{
			string fileName = node.windowTitle;

			if (!foundNodes.Contains(node))
			{
				AssetDatabase.CreateAsset(node, "Assets/ScriptableObjects/Nodes/" + fileName + ".asset");
			}
		}
	}

	private void LoadChoiceNodes()
	{
		string[] fileNames = AssetDatabase.FindAssets("t:" + typeof(ChoiceNode).ToString());

		foreach (var fileName in fileNames)
		{
			ChoiceNode foundObject = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(fileName), typeof(ChoiceNode)) as ChoiceNode;
			if (foundObject != null)
			{
				windows.Add(foundObject as ChoiceNode);
			}
		}
	}

	private void GenerateChoices()
	{
		/*string[] fileNames = AssetDatabase.FindAssets("t:" + typeof(Story).ToString());
		List<Choice> foundObjects = new List<Choice>();

		foreach (string fileName in fileNames)
		{
			Choice foundObject = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(fileName), typeof(Choice)) as Choice;
			foundObjects.Add(foundObject);
			AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(fileName));
		}*/

		List<BaseNode> nodes = windows.Where(x => x.GetType() == typeof(ChoiceNode)).ToList();

		foreach (ChoiceNode node in nodes)
		{
			Choice choice = CreateInstance<Choice>();

			//TODO: Implement setting the variables of Choices

			string fileName = node.windowTitle;

			if (!foundObjects.Contains(choice))
			{
				AssetDatabase.CreateAsset(choice, "Assets/ScriptableObjects/Objects/" + fileName + ".asset");
			}
		}
	}

	private void SaveBattleNodes()
	{
		List<BaseNode> nodes = windows.Where(x => x.GetType() == typeof(BattleNode)).ToList();

		foreach (BattleNode node in nodes)
		{
			string fileName = node.windowTitle;

			if (!foundNodes.Contains(node))
			{
				AssetDatabase.CreateAsset(node, "Assets/ScriptableObjects/Nodes/" + fileName + ".asset");
			}
		}
	}

	private void LoadBattleNodes()
	{
		string[] fileNames = AssetDatabase.FindAssets("t:" + typeof(BattleNode).ToString());

		foreach (var fileName in fileNames)
		{
			BattleNode foundObject = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(fileName), typeof(BattleNode)) as BattleNode;
			if (foundObject != null)
			{
				windows.Add(foundObject as BattleNode);
			}
		}
	}

	private void GenerateBattles()
	{
		/*string[] fileNames = AssetDatabase.FindAssets("t:" + typeof(Battle).ToString());
		List<Battle> foundObjects = new List<Battle>();

		foreach (string fileName in fileNames)
		{
			Battle foundObject = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(fileName), typeof(Battle)) as Battle;
			foundObjects.Add(foundObject);
			AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(fileName));
		}*/

		List<BaseNode> nodes = windows.Where(x => x.GetType() == typeof(BattleNode)).ToList();

		foreach (BattleNode node in nodes)
		{
			Battle battle = CreateInstance<Battle>();

			//TODO: Implement setting the variables of Battles

			string fileName = node.windowTitle;

			if (!foundObjects.Contains(battle))
			{
				AssetDatabase.CreateAsset(battle, "Assets/ScriptableObjects/Objects/" + fileName + ".asset");
			}
		}
	}

	private void GenerateScriptableObjects()
	{
		string[] fileNames = AssetDatabase.FindAssets("t:" + typeof(LoadableObject).ToString());

		foreach (string fileName in fileNames)
		{
			LoadableObject foundObject = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(fileName), typeof(LoadableObject)) as LoadableObject;
			foundObjects.Add(foundObject);
			AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(fileName));
		}
		
		GenerateStories();
		GenerateChoices();
		GenerateBattles();

		AssetDatabase.SaveAssets();
	}
	#endregion

	#region Helper Methods
	/// <summary>
	/// Draw a curve
	/// </summary>
	/// <param name="curve"></param>
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

	/// <summary>
	/// Save a Curve in their startNode
	/// </summary>
	/// <param name="curve"></param>
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

	/// <summary>
	/// Create a new node at mousePos with windowWidth and windowHeight
	/// </summary>
	/// <param name="node"></param>
	private void CreateNode(BaseNode node)
	{
		node.windowRect = new Rect(mousePos.x, mousePos.y, windowWidth, windowHeight);
		node.windowTitle = node.GetType().ToString();
		node.SetNodeEditor(this);
		node.Init();

		selectedNode = node;
		windows.Add(node);
	}

	/// <summary>
	/// Check the name of the selectedNode, if it's the same as another node add a 2 at the end
	/// </summary>
	public void CheckName()
	{
		if (selectedNode != null)
		{
			foreach (var window in windows)
			{
				if (selectedNode.windowTitle == window.windowTitle && selectedNode != window)
				{
					selectedNode.windowTitle += " 2";
					Repaint();
				}
			}
		}
	}
	#endregion
}