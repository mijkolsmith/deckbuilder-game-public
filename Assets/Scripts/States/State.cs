using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
	protected StateManager stateManager;

	public State(StateManager stateManager)
	{
		this.stateManager = stateManager;
	}

	public virtual IEnumerator Start()
	{
		yield break;
	}

	public virtual IEnumerator CoroutineUpdate()
	{
		yield break;
	}

	public virtual IEnumerator Exit()
	{
		yield break;
	}
}