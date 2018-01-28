using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;

public class echoLight : MonoBehaviour {
    [SerializeField]
    AnimationCurve curve;
    [SerializeField] Light light;
	public float periodTime;
    [SerializeField] float expandRadiusTime = 0.5f;
    [SerializeField] float contractRadiusTime = 0.5f;
    [SerializeField] int lightAngle = 20;

    public int combo = 0;

    // Use this for initialization
    void Start () {
        light.spotAngle = lightAngle;
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
		float time = periodTime;
        int max = 40 * (1 + (int)( 0.2f *(1 + combo) ) );
        DOTween.To(() => light.spotAngle, (n) => light.spotAngle = n, max, time * expandRadiusTime);
        DOVirtual.DelayedCall(time * expandRadiusTime, () => DOTween.To(() => light.spotAngle, (n) => light.spotAngle = n, 20, time * contractRadiusTime));
        

    }

}
