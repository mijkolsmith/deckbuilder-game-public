using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class ReflectionTest : MonoBehaviour
{
	[SerializeField] private bool testing;

    private void Update()
    {
		if (testing)
		{
			MethodInfo[] methods = typeof(ExampleClass).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
			foreach (var method in methods)
			{
				if (method.GetParameters().Length == 0)
				{
					Debug.Log(method.Name);
				}
				else
				{
					Debug.Log("method has parameter: " + method.Name);
				}
			}

			ExampleClass myInstance = new ExampleClass();

			MethodInfo getSecret = typeof(ExampleClass).GetMethod("GetSecret", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
			Debug.Log(getSecret.Invoke(myInstance, null));

			MethodInfo setSecret = typeof(ExampleClass).GetMethod("SetSecret", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
			setSecret.Invoke(myInstance, new object[] { "burg" });
			Debug.Log(getSecret.Invoke(myInstance, null));
		}
	}
}
