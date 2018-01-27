using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class FieldEditorWindow : EditorWindow
{
	[MenuItem("FieldEditorWindow/Open")]
	public static void Open()
	{
		EditorWindow.GetWindow<FieldEditorWindow>(false, "FieldEditorWindow");
	}

	int boxSize = 25;

	static Field field = null;

	GUIStyle _enableStyle;
	GUIStyleState enableStyleState;
	public GUIStyle enableStyle
	{
		get {
			if (_enableStyle == null) {
				_enableStyle = new GUIStyle(GUI.skin.box);
				enableStyleState = new GUIStyleState();
				//enableStyleState.background = _enableStyle.normal.background;
				enableStyleState.textColor = Color.white;
				_enableStyle.normal = enableStyleState;
			}
			return _enableStyle;
		}
	}


	GUIStyle _disableStyle;
	GUIStyleState disableStyleState;
	public GUIStyle disableStyle
	{
		get {
			if (_disableStyle == null) {
				_disableStyle = new GUIStyle(GUI.skin.box);
				disableStyleState = new GUIStyleState();
				//disableStyleState.background = _disableStyle.normal.background;
				disableStyleState.textColor = Color.white;
				_enableStyle.normal = disableStyleState;
			}
			return _enableStyle;
		}
	}

	private Vector2Int selectPos
	{
		get {
			return new Vector2Int(
				(int)Event.current.mousePosition.x / boxSize,
				(int)Event.current.mousePosition.y / boxSize);
		}
	}

	public void OnGUI()
	{
		GameObject obj = Selection.activeGameObject;

		if (obj != null) {
			Field temp = obj.GetComponent<Field>();
			if (temp != field && temp != null) {
				field = temp;
			}
		}
		if (field != null) {

			//選択処理
			var p = field.GetParts(selectPos.x, selectPos.y);
			if (p != null) {
				if (Event.current.button == 0 && Event.current.isMouse) {
					if (Event.current.type == EventType.MouseDown) {
						p.enable = true;
					}
				} else if (Event.current.button == 1 && Event.current.isMouse) {
					if (Event.current.type == EventType.MouseDown) {
						p.enable = false;
					}
				}
			}

			//四角形をガリガリ描きます
			for(int x = 0;x < field.width;++x) {
				for(int y = 0;y < field.height;++y) {
					PaintBox(field, x, y);
				}
			}


			GUI.BeginGroup(new Rect(0, field.height * boxSize, Screen.width, Screen.height));
			{
				int old_cell_size = boxSize;
				boxSize = EditorGUILayout.IntSlider(boxSize, 15, 50);
			}
			GUI.EndGroup();

			Repaint();

		}
	}

	void OnSelectionChange()
	{
		Repaint();
	}

	/*
		箱を書く
	*/
	void PaintBox(Field field, int x, int y)
	{
		GUIStyle style;

		var p = field.GetParts (x, y);
		var text = "";
		if (p.enable) {
			style = enableStyle;
			if (field.startPosition == new Vector2Int (x, y)) {
				text = "S";
			} else if (field.goalPosition == new Vector2Int(x, y)) {
				text = "G";
								
			} else {
				text = "■";
			}
		} else {
			if (field.startPosition == new Vector2Int (x, y)) {
				text = "☓";
			} else if (field.goalPosition == new Vector2Int(x, y)) {
				text = "☓";

			} else {
			}
			style = disableStyle;
		}

		GUI.Box(new Rect(x * boxSize, y * boxSize, boxSize, boxSize), text);
	}
}
