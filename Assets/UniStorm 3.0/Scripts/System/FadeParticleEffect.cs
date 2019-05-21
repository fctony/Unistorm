using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeParticleEffect : MonoBehaviour {

	public AnimationCurve ParticleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
	public float FadeMultipler = 1;
	ParticleSystem ParticleSource;
    float EmissionAmount;
	float Timer;

    void OnEnable ()
    {
        if (ParticleSource != null)
        {
            ParticleSystem.EmissionModule CurrentEmission = ParticleSource.emission;
            CurrentEmission.rateOverTime = new ParticleSystem.MinMaxCurve(EmissionAmount);
            Timer = 0;
        }
    }

	void Start () 
	{
		if (GetComponent<ParticleSystem>() != null)
		{
			ParticleSource = GetComponent<ParticleSystem>();
            EmissionAmount = ParticleSource.emission.rateOverTime.constant;
        }
		else
		{
			GetComponent<FadeParticleEffect>().enabled = false;
		}
	}

	void Update () 
	{
		Timer += Time.deltaTime;
		ParticleSystem.EmissionModule CurrentEmission = ParticleSource.emission;
		CurrentEmission.rateOverTime = new ParticleSystem.MinMaxCurve(ParticleCurve.Evaluate(Timer*FadeMultipler));
	}
}
