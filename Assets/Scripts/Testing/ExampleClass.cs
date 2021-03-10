using UnityEngine;

public class ExampleClass : MonoBehaviour, IExampleClass
{
	[ContextMenu("Generate Interface")]
	public void GenerateInterface()
	{
		InterfaceGenerator.Generate(typeof(ExampleClass));
	}

	public string someData = "DATA";
	protected string testname = "ExampleClass";
	private string secret = "I love snacks";

	public string GetName()
	{
		return testname;
	}

	public void SetName(string newName) 
	{
		testname = newName;
	}

	public void Prepare()
	{
		Debug.Log("Preparing...");
	}

	public void DoSomething()
	{
		Debug.Log("DO SOMETHING!");
	}

	private void SetSecret(string newSecret)
	{
		secret = newSecret;
	}

	private string GetSecret()
	{
		return secret;
	}
}
