using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectScene : MonoBehaviour {

	static public GameObject fieldPrefab;

	void Start() {
		if (SceneManager.sceneCount == 1) {
			SceneManager.LoadScene("BGM", LoadSceneMode.Additive);
		}
	}

	public void StartScene(GameObject field) {
		fieldPrefab = field;
		SceneManager.LoadScene("GameScene");
	}
}
