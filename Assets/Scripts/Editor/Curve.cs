using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Curve
{
    public BaseNode startNode;
	public BaseNode endNode;
	public Color color;

	public Curve(BaseNode startNode, BaseNode endNode, Color color)
	{
		this.startNode = startNode;
		this.endNode = endNode;
		this.color = color;
	}
}