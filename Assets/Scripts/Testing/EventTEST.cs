using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTEST : MonoBehaviour
{
	[SerializeField] Material material;
	[SerializeField] EventType eventType;

	private void Start()
    {
		EventManager.AddListener(eventType, ApplyColor);
    }

	public void CallEvent()
	{
		EventManager.RaiseEvent(eventType);
	}

	private void ApplyColor()
    {
		GetComponent<MeshRenderer>().material = material;
    }
}
