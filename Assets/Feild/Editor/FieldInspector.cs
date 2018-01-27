using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Field))]
public class FieldInspector : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();
		Field field = target as Field;
		if (!Application.isPlaying) {
			if (GUILayout.Button("Refresh")) {
				field.Refresh ();
			}
		}
	}
}
