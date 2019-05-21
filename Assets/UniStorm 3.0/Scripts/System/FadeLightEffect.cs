using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeLightEffect : MonoBehaviour {

	public AnimationCurve LightCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
	public float FadeMultipler = 1;
	Light LightSource;
	float Timer;

    void OnEnable()
    {
        Timer = 0;
    }

    void Start () 
	{
		if (GetComponent<Light>() != null)
		{
			LightSource = GetComponent<Light>();
		}
		else
		{
			GetComponent<FadeLightEffect>().enabled = false;
		}
	}

	void Update () 
	{
		Timer += Time.deltaTime;
		LightSource.intensity = LightCurve.Evaluate(Timer*FadeMultipler);
	}
}
