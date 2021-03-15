using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
	protected State state;

	public void SetState(State state)
	{
		StartCoroutine(state.Exit());
		this.state = state;
		StartCoroutine(state.Start());
	}

	private void Update()
	{
		StartCoroutine(state.CoroutineUpdate());
	}
}
