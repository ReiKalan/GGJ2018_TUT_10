using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectScene : MonoBehaviour {

	static public GameObject fieldPrefab;

	public void StartScene(GameObject field) {
		fieldPrefab = field;
		SceneManager.LoadScene("GameScene");
	}
}
