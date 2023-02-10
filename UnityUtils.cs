using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TagAttribute : PropertyAttribute { }
public class LayerAttribute : PropertyAttribute { }

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(TagAttribute))]
public class TagDrawer : PropertyDrawer
{

	public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
	{

		EditorGUI.BeginProperty(position, label, prop);

		prop.stringValue = EditorGUI.TagField(position, label, prop.stringValue);

		EditorGUI.EndProperty();
	}
}

[CustomPropertyDrawer(typeof(LayerAttribute))]
public class LayerDrawer : PropertyDrawer
{

	public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
	{

		EditorGUI.BeginProperty(position, label, prop);

		prop.intValue = EditorGUI.LayerField(position, label, prop.intValue);

		EditorGUI.EndProperty();
	}
}
#endif