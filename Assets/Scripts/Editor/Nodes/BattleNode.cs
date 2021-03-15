using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

#if UNITY_EDITOR
	public override void ContextMenu(GenericMenu menu, GenericMenu.MenuFunction2 contextCallback)
	{
		base.ContextMenu(menu, contextCallback);
	}
#endif
}