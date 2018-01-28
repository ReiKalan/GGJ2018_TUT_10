using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class MangaTutorial : MonoBehaviour {

	public PlayableDirector[] directors;
	private PlayableDirector playDirector;

	int nowIndex = 0;

	// Use this for initialization
	void Start () {
		//directors [0].Play ();

	}
	
	// Update is called once per frame
	void Update () {
		if (directors.Length <= nowIndex) {
			if (Input.GetMouseButtonDown (0)) {
				SceneManager.LoadScene ("StageSelect");
			}
			
		}
		if (playDirector != null && playDirector.state == PlayState.Playing) {
			return;
		}
		if (Input.GetMouseButtonDown (0)) {
			Active (nowIndex++);
		}
	}

	void Active(int index) {
		Debug.Log (index);
		if (0 <= index && index < directors.Length) {
			for (int i = 0; i < directors.Length; i++) {
				playDirector = directors [i];
				playDirector.Play ();
				directors[i].gameObject.SetActive (index == i);
			}
		} else {
			SceneManager.LoadScene("StageSelect");
		}
	}
}
