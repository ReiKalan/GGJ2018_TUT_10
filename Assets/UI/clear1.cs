using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class clear1 : MonoBehaviour {

    private int rand = 0;
    private void Awake()
    {
        rand = Random.Range(0, 99);
    }

	// Use this for initialization
	void Start () {
        switch (rand)
        {
            case 0:

                break;
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:

                break;
            default:

                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("Title");
        }
	}
}
