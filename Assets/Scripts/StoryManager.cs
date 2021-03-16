using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
#if UNITY_EDITOR
	using UnityEditor;
#endif

public class StoryManager : MonoBehaviour
{
	public List<Story> stories = new List<Story>();
	int storyIndex = 0;
	int dialogueIndex = 0;

	public TextMeshProUGUI displayText;

	private StateManager stateManager;
	private void Start()
	{
		stateManager = GetComponent<StateManager>();
	}

	//this is called from the StoryState
	/*public void Update()
	{
		DisplayDialogue();
	}*/

	public void DisplayDialogue()
	{
		if (stories.ElementAtOrDefault(storyIndex) != null)
		{
			if(stories[storyIndex].dialogue.ElementAtOrDefault(dialogueIndex) != null)
			{
				displayText.text = stories[storyIndex].dialogue[dialogueIndex];
			}
			else
			{
				stateManager.SetState(stories[storyIndex].nextState);
				//TODO:
				//state = next state
				//this data should be saved in the Story object
			}
		}
		else
		{
			//TODO:
			//end of game state?
		}
	}

	public void NextDialogue()
	{
		dialogueIndex++;
	}

	//temporary helper function called from a button
	public void NextStory()
	{
		storyIndex++;
		dialogueIndex = 0;
	}

	public void LoadObjects()
	{
		//TODO: make loadableObjectsManager? or turn this into that?
#if UNITY_EDITOR
		stories.Clear();

		string[] fileNames = AssetDatabase.FindAssets("t:" + typeof(Story).ToString());

		foreach (var fileName in fileNames)
		{
			Story foundObject = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(fileName), typeof(Story)) as Story;
			if (foundObject != null)
			{
				stories.Add(foundObject);
			}
		}
#endif
	}
}