using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Curve
{
    public BaseNode startNode;
	public BaseNode endNode;
	public LoadableObject startObject;
	public LoadableObject nextObject;

	public Color color;

	public Curve(BaseNode startNode, BaseNode endNode, LoadableObject startObject, LoadableObject nextObject, Color color)
	{
		this.startNode = startNode;
		this.endNode = endNode;
		this.startObject = startObject;
		this.nextObject = nextObject;
		this.color = color;
	}
}