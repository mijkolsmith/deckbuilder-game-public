using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Choice", menuName = "NodeEditor/Choice")]
public class Choice : ScriptableObject
{
	public List<string> choices = new List<string>() { };
	//TODO: requirements for choices (if you need a certain item to make a choice)
	public List<State> nextStates;
}
