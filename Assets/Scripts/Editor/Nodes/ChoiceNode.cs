using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Choice", menuName = "NodeEditor/ChoiceNode")]
public class ChoiceNode : BaseNode
{
	public override void Init()
	{
		nodeState = new ChoiceState(GameManager.Instance.gameObject.GetComponent<StateManager>());
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
		return CreateInstance<Choice>();
	}

	public override void Generate()
	{
		List<BaseNode> nodes = nodeEditor.windows.Where(x => x.GetType() == typeof(ChoiceNode)).ToList();

		foreach (ChoiceNode node in nodes)
		{
			Choice choice = CreateInstance<Choice>();

			//TODO: Implement setting the variables of Choices

			string fileName = node.windowTitle;

			if (!nodeEditor.foundObjects.Contains(choice))
			{
				AssetDatabase.CreateAsset(choice, "Assets/ScriptableObjects/LoadableObjects/" + fileName + ".asset");
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