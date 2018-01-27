using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

	Quaternion targetEuler;

	float cameraMoveDuretion = 2f;
	float cameraMoveTIme = 0f;

	bool animateStart = false;

	// Use this for initialization
	void Start () {
		RunGame.instance.OnBeats += OnBeats;
		RunGame.instance.OnChangeRotate += OnChangeRotate;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = RunGame.instance.mainCharacter.transform.position;

		if (animateStart) {
			cameraMoveTIme += Time.deltaTime;

			if (cameraMoveTIme / cameraMoveDuretion > 1f) {
				transform.rotation = targetEuler;

				animateStart = false;
			} else {
				var fromEuler = transform.rotation;
				transform.rotation = Quaternion.Lerp (
					fromEuler, targetEuler, cameraMoveTIme / cameraMoveDuretion);
			}
		}

	}

	void OnChangeRotate() {
		targetEuler = RunGame.instance.mainCharacter.transform.rotation;

		//このコメントを外すと
		//animateStart = false;
	}

	void OnBeats() {
		cameraMoveTIme = 0f;
		animateStart = true;
	}
}
