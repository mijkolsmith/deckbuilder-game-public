using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
	using UnityEditor;
#endif

public class StoryManager : MonoBehaviour
{
	public List<Story> stories = new List<Story>();

	public Story currentStory;

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