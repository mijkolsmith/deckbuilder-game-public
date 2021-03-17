using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Battle", menuName = "NodeEditor/BattleNode")]
public class BattleNode : BaseNode
{
	public override void Init()
	{
		nodeState = new BattleState(GameManager.Instance.gameObject.GetComponent<StateManager>());
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
		return CreateInstance<Battle>();
	}

	public override void Generate()
	{
		List<BaseNode> nodes = nodeEditor.windows.Where(x => x.GetType() == typeof(BattleNode)).ToList();

		foreach (BattleNode node in nodes)
		{
			Battle battle = CreateInstance<Battle>();

			//TODO: Implement setting the variables of Battles

			string fileName = node.windowTitle;

			if (!nodeEditor.foundObjects.Contains(battle))
			{
				AssetDatabase.CreateAsset(battle, "Assets/ScriptableObjects/LoadableObjects/" + fileName + ".asset");
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