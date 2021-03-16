using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Story", menuName = "NodeEditor/Story")]
public class Story : LoadableObject
{
	public List<string> dialogue = new List<string>() { };
	public State nextState;
}