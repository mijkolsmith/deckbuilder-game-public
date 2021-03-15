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
	//TODO: add statemanager (choice, battle, story)
	//replace with state check
	int storyIndex = 0;
	int dialogueIndex = 0;

	public TextMeshProUGUI tmp;

	public void Update()
	{
		Debug.Log("story: " + storyIndex);
		Debug.Log("dialogue: " + dialogueIndex);
		DisplayDialogue();
	}

	public void DisplayDialogue()
	{
		if (stories.ElementAtOrDefault(storyIndex) != null)
		{
			if(stories[storyIndex].dialogue.ElementAtOrDefault(dialogueIndex) != null)
			{
				tmp.text = stories[storyIndex].dialogue[dialogueIndex];
			}
			else
			{
				//go to next node
			}
		}
		else
		{
			//end of game
		}
	}

	public void NextDialogue()
	{
		dialogueIndex++;
	}

	//temporary helper function
	public void NextStory()
	{
		storyIndex++;
		dialogueIndex = 0;
	}

	public void LoadStories()
	{
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