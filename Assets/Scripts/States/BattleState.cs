using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : State
{
	public BattleState(StateManager stateManager) : base(stateManager) { }

	public override IEnumerator Start()
	{
		stateManager.battleObject.SetActive(true);
		yield break;
	}

	public override IEnumerator CoroutineUpdate()
	{
		yield break;
	}

	public override IEnumerator Exit()
	{
		stateManager.battleObject.SetActive(false);
		yield break;
	}
}