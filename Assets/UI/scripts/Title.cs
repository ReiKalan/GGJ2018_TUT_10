﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SceneManager.LoadScene ("BGM", LoadSceneMode.Additive);
	}
	
	// Update is called once per frame
	void Update () {
		/*if(Input.touchCount>0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                SceneManager.LoadScene("game");
            }
        }*/
        if(Input.GetMouseButtonDown(0))
        {
			SceneManager.UnloadSceneAsync ("Title");
			SceneManager.LoadScene("MangaTutorialScene", LoadSceneMode.Additive);
        }
	}
}
