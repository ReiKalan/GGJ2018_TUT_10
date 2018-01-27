using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gamecon : MonoBehaviour {
    public float remainingTime = 30f;
    public Text remainingTimeLabel;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        remainingTime -= Time.deltaTime;

        remainingTimeLabel.text = remainingTime.ToString("00");

        if(remainingTime<=0)
        {

        }
    }
}
