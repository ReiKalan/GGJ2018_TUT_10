using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hit : MonoBehaviour {
    [SerializeField] Graphic gra;
    [SerializeField] float ing = 1.0f;
    [SerializeField] float dei = 0.333f;
    Coroutine cor;


    private Text text;

    private float nextTime;
    public float interval;

    void Reset()
    {
        gra = GetComponent<Graphic>();
    }
    void Awake()
    {
        StartFlash();
    }


    // Use this for initialization
    IEnumerator Flash()
    {
        float time = 0.0f;
        text = GetComponent<Text>();

        while (true)
        {
            time += ing * dei;

            var color = gra.color;
            color.a = Mathf.Abs(Mathf.Sin(time));
            gra.color = color;
            yield return new WaitForSeconds(dei);
        }




    }

    public void StartFlash()
    {
        cor = StartCoroutine(Flash());
    }

    public void StopFlash()
    {
        StopCoroutine(cor);
    }


    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextTime)
        {

        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	
}
