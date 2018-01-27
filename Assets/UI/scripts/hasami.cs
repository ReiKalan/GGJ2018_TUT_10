using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class hasami : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        int rand = Random.Range(0, 99);
        Debug.Log(rand);
        switch (rand)
        {
            case 0:
                SceneManager.LoadScene("clear3");
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
                SceneManager.LoadScene("clear2");
                break;
            default:
                SceneManager.LoadScene("clear1");
                break;
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
