using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Battle", menuName = "NodeEditor/Battle")]
public class Battle : ScriptableObject
{
	public State winState;
	//TODO: requirements for win (if you need a certain item to win a battle)
	public State loseState;
}