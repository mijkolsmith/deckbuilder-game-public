using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryState : State
{
	public Story story;
	public StoryManager storyManager;

	public StoryState(StateManager stateManager) : base(stateManager) { }

	public override IEnumerator Start()
	{
		storyManager = GameManager.Instance.gameObject.GetComponent<StoryManager>();
		Debug.Log(storyManager);
		stateManager.storyObject.SetActive(true);
		yield break;
	}

	public override IEnumerator CoroutineUpdate()
	{
		storyManager.DisplayDialogue();
		yield break;
	}

	public override IEnumerator Exit()
	{
		stateManager.storyObject.SetActive(false);
		yield break;
	}
}