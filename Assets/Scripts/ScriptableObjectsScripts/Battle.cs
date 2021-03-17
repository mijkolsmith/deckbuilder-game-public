using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Battle", menuName = "NodeEditor/Battle")]
public class Battle : LoadableObject
{
	public LoadableObject winObject;
	public LoadableObject loseObject;
	public State winState;
	//TODO: requirements for win (if you need a certain item to win a battle)
	public State loseState;
}