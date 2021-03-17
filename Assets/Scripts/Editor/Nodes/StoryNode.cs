using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Story", menuName = "NodeEditor/StoryNode")]
public class StoryNode : BaseNode
{
	public override void Init()
	{
		nodeState = new StoryState(GameManager.Instance.gameObject.GetComponent<StateManager>());
		base.Init();
	}

	public override void DrawWindow()
	{
		base.DrawWindow();
	}

	public override void DrawTextAreas()
	{
		base.DrawTextAreas();
	}

	public override void DrawCurves()
	{
		base.DrawCurves();
	}

	public override LoadableObject CreateObject()
	{
		return CreateInstance<Story>();
	}

	public override void Generate()
	{
		List<BaseNode> nodes = nodeEditor.windows.Where(x => x.GetType() == typeof(StoryNode)).ToList();

		foreach (StoryNode node in nodes)
		{
			Story story = null;

			//TODO: fix this
			/*
			foreach (Curve curve in node.curves)
			{
				story = (Story) curve.startObject;

				if (curve.nextObject != null)
				{
					story.nextObject = curve.nextObject;
				}

				Debug.Log(node + " startObject: " + curve.startObject);
				Debug.Log(node + " nextObject: " + curve.nextObject);

				story.nextState = curve.endNode.nodeState;
			}*/
			
			if (node.curves.Count == 0)
			{//testing
				story = (Story) CreateObject();
			}

			//delete this after it works
			story = (Story)CreateObject();

			for (int i = 0; i < node.textAreas.Count; i++)
			{
				story.dialogue.Add(node.textAreas[i]);
			}

			string fileName = node.windowTitle;

			if (!nodeEditor.foundObjects.Contains(story))
			{
				AssetDatabase.CreateAsset(story, "Assets/ScriptableObjects/LoadableObjects/" + fileName + ".asset");
			}
		}
	}

#if UNITY_EDITOR
	public override void ContextMenu(GenericMenu menu, GenericMenu.MenuFunction2 contextCallback)
	{
		base.ContextMenu(menu, contextCallback);
	}
#endif
}
