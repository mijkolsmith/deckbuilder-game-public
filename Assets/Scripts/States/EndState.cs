using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndState : State
{
	public EndState(StateManager stateManager) : base(stateManager) { }

	public override IEnumerator Start()
	{
		stateManager.storyObject.SetActive(false);
		stateManager.choiceObject.SetActive(false);
		stateManager.battleObject.SetActive(false);
		yield break;
	}

	public override IEnumerator CoroutineUpdate()
	{
		yield break;
	}

	public override IEnumerator Exit()
	{
		yield break;
	}
}
