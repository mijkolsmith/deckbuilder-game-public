using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class RenameAttribute : PropertyAttribute
{
	public string newName { get; private set; }
	public RenameAttribute(string name)
	{
		newName = name;
	}
}

[CustomPropertyDrawer(typeof(RenameAttribute))]
public class RenameEditor<T> : PropertyDrawer
{
	List<T> list;
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.PropertyField(position, property, new GUIContent((attribute as RenameAttribute).newName));
	}
}