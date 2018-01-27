using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;

public class echoLight : MonoBehaviour {
    [SerializeField]
    AnimationCurve curve;
    [SerializeField] Light light;
    [SerializeField] int bpm;
    [SerializeField] int lightAngle =20;

    int combo = 0;

    // Use this for initialization
    void Start () {
        light.spotAngle = 20;
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

    public void Play()
    {
        float time = 1 / (bpm / 60);
        int max = 40 * (1 + (int)( 0.2f *(1 + combo) ) );
        DOTween.To(() => light.spotAngle, (n) => light.spotAngle = n, max, time / 4);
        DOVirtual.DelayedCall(time / 4, () => DOTween.To(() => light.spotAngle, (n) => light.spotAngle = n, 20, time - (time / 4)));
        

    }

}
