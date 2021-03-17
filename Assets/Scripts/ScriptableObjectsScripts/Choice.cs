using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Choice", menuName = "NodeEditor/Choice")]
public class Choice : LoadableObject
{
	public List<LoadableObject> loadableObjects;
	public List<State> nextStates;

	public List<string> choices = new List<string>() { };
	//TODO: requirements for choices (if you need a certain item to make a choice)
}