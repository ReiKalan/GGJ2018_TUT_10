using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class echoLight : MonoBehaviour {


    int initDistance;
    int maxDistance;

    int combo = 0;

    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void addCombo()
    {
        combo++;
    }

    public void comboReset()
    {
        combo = 0;
    }

    IEnumerator echo(float beat) {

        yield return new WaitForEndOfFrame();
    }
}
