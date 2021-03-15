using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceState : State
{
	public ChoiceState(StateManager stateManager) : base(stateManager) { }

	public override IEnumerator Start()
	{
		stateManager.choiceObject.SetActive(true);
		yield break;
	}

	public override IEnumerator CoroutineUpdate()
	{
		yield break;
	}

	public override IEnumerator Exit()
	{
		stateManager.choiceObject.SetActive(false);
		yield break;
	}
}