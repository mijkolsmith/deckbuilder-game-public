using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : StateMachine
{
	public GameObject storyObject;
	public GameObject choiceObject;
	public GameObject battleObject;

	private void Start()
	{
		SetState(new StoryState(this));
	}
}
