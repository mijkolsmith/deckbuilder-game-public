using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "End", menuName = "NodeEditor/EndNode")]
public class EndNode : BaseNode
{
	public override void Init()
	{
		nodeState = new EndState(GameManager.Instance.gameObject.GetComponent<StateManager>());
		base.Init();
	}

	public override LoadableObject CreateObject()
	{
		return CreateInstance<End>();
	}

	public override void Generate()
	{
		List<BaseNode> nodes = nodeEditor.windows.Where(x => x.GetType() == typeof(EndNode)).ToList();

		foreach (EndNode node in nodes)
		{
			End end = CreateInstance<End>();

			//TODO: Implement setting the variables of EndNode if there will be any

			string fileName = node.windowTitle;

			if (!nodeEditor.foundObjects.Contains(end))
			{
				AssetDatabase.CreateAsset(end, "Assets/ScriptableObjects/LoadableObjects/" + fileName + ".asset");
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