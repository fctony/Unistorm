using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSystem : MonoBehaviour {

    [HideInInspector]
    public int LightningGenerationDistance = 100;
    [HideInInspector]
    public LineRenderer LightningBolt;
    [HideInInspector]
    List<Vector3> LightningPoints = new List<Vector3>();
    [HideInInspector]
    public bool AnimateLight;
    [HideInInspector]
    public float LightningLightIntensityMin = 1;
    [HideInInspector]
    public float LightningLightIntensityMax = 1;
    [HideInInspector]
    public float LightningLightIntensity;
    [HideInInspector]
    public Light LightningLightSource;
    [HideInInspector]
    public float LightningCurveMultipler = 1.45f;
    [HideInInspector]
    public AnimationCurve LightningCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [HideInInspector]
    public float m_FlashSeconds = 0.5f;
    [HideInInspector]
    public Transform StartingPoint;
    [HideInInspector]
    public Transform EndingPoint;
    [HideInInspector]
    public int m_Segments = 45;
    [HideInInspector]
    public float Speed = 0.032f;
    [HideInInspector]
    public int Scale = 2;
    [HideInInspector]
    public Transform PlayerTransform;
    [HideInInspector]
    public List<AudioClip> ThunderSounds = new List<AudioClip>();

    float m_FlashTimer;
    float PointIndex;

    float TimeX_1;
    float TimeY_1;
    float TimeZ_1;
    float TimeX_2;
	float TimeY_2;
	float TimeZ_2;

	bool Generated = false;
	float LightningTime;

	AudioSource AS;
	Coroutine LightningCoroutine;

    void Start () {
		GameObject TempBolt = Resources.Load("Lightning Renderer") as GameObject;
		LightningBolt = Instantiate(TempBolt, Vector3.zero, Quaternion.identity).GetComponent<LineRenderer>();
        LightningBolt.transform.SetParent(GameObject.Find("UniStorm System").transform);
		LightningBolt.name = "Lightning Renderer";

		GameObject TempEndPoint = Resources.Load("Lightning End Point") as GameObject;
		EndingPoint = Instantiate(TempEndPoint, Vector3.zero, Quaternion.identity).transform;
        EndingPoint.transform.SetParent(GameObject.Find("UniStorm System").transform);
        EndingPoint.name = "Lightning End Point";

		StartingPoint = new GameObject().transform;
		StartingPoint.transform.position = Vector3.zero;
        StartingPoint.SetParent(GameObject.Find("UniStorm System").transform);
        StartingPoint.name = "Lightning Start Point";

		LightningBolt.positionCount = m_Segments;
		PointIndex = 1f / (float)m_Segments;

		for (int i = 0; i < LightningBolt.positionCount; i++)
		{
			LightningPoints.Add(transform.position);
		}

		m_FlashTimer = m_FlashSeconds;
		gameObject.AddComponent<AudioSource>();
		AS = GetComponent<AudioSource>();
        AS.outputAudioMixerGroup = FindObjectOfType<UniStormSystem>().UniStormAudioMixer.FindMatchingGroups("Master/Weather")[0];
        LightningBolt.enabled = false;
		GeneratePoints();

		Vector3 GeneratedPosition = new Vector3(PlayerTransform.position.x, PlayerTransform.position.y, PlayerTransform.position.z)  + new Vector3(Random.insideUnitSphere.x, 0,Random.insideUnitSphere.z) * LightningGenerationDistance;
		StartingPoint.position = GeneratedPosition+new Vector3(0,80,0);
		EndingPoint.position = GeneratedPosition;

        LightningLightSource.transform.rotation = Quaternion.Euler(UnityEngine.Random.Range(35, 85), UnityEngine.Random.Range(0, 360), 0);
        LightningLightIntensity = Random.Range(LightningLightIntensityMin, LightningLightIntensityMax);

    }

	void SetupLightningLight ()
	{
		LightningLightSource.transform.rotation = Quaternion.Euler (UnityEngine.Random.Range(10,40), UnityEngine.Random.Range(0,360), 0);
	}
	
	void GeneratePoints () 
	{
		TimeX_1 += Time.deltaTime * Speed * 5f;
		TimeY_1 += Time.deltaTime * Speed * 0.1f;
		TimeZ_1 += Time.deltaTime * Speed * 5f;

		TimeX_2 += Time.deltaTime * Speed * 5f;
		TimeY_2 += Time.deltaTime * Speed * 25f;
		TimeZ_2 += Time.deltaTime * Speed * 5f;

		for (int i = 0; i < LightningBolt.positionCount; i++)
		{
			Vector3 position = Vector3.Lerp(StartingPoint.position, EndingPoint.position, (float)i * PointIndex);
			Vector3 position2 = Vector3.Lerp(StartingPoint.position, EndingPoint.position, (float)i * PointIndex);
			Vector3 offsetAll = new Vector3(Mathf.PerlinNoise(TimeX_1 + position.x/2, TimeX_1 + position.y/2),Mathf.PerlinNoise(TimeX_1 + position.x, TimeY_1 + position.y),Mathf.PerlinNoise(TimeX_1 + position.x/2, TimeZ_1 + position.z/2));
			Vector3 offsety = new Vector3(Mathf.PerlinNoise(TimeX_2 + position2.x/12, TimeX_2 + position2.y/12),Mathf.PerlinNoise(TimeX_2 + position2.x/12, TimeY_2 + position2.y/12),Mathf.PerlinNoise(TimeX_2 + position2.x/12, TimeZ_2 + position2.z/12));
            position += (offsetAll + offsety * Scale);

            if (i <= LightningBolt.positionCount - 5)
            {
                LightningPoints[i] = position;
            }
            else if (i != LightningBolt.positionCount - 1)
            {
                LightningPoints[i] = Vector3.Lerp(position, position2, (float)i * PointIndex);
            }
            else
            {
                LightningPoints[i] = new Vector3(EndingPoint.position.x, EndingPoint.position.y-0.5f, EndingPoint.position.z);
            }

			LightningBolt.SetPosition(i,LightningPoints[i]);
		}
	}

	void Update ()
	{
		if (AnimateLight)
		{
			LightningTime += Time.deltaTime*LightningCurveMultipler;
			var LightIntensity = LightningCurve.Evaluate(LightningTime);
			LightningLightSource.intensity = LightIntensity*LightningLightIntensity;

			if (LightningTime >= 1)
			{
				LightningTime = 0;
				AnimateLight = false;
				LightningLightSource.transform.rotation = Quaternion.Euler (UnityEngine.Random.Range(35,85), UnityEngine.Random.Range(0,360), 0);
			}
		}
	}

	public void GenerateLightning ()
	{
		Generated = true;
        //Randomize our scale and speed to create additional randomness to each lightning bolt
        Speed = Random.Range(0.05f, 0.08f);
        Scale = Random.Range(5, 12);
        LightningLightIntensity = Random.Range(LightningLightIntensityMin, LightningLightIntensityMax);
        if (LightningCoroutine != null)
		{
			StopCoroutine(LightningCoroutine);
		}
		LightningCoroutine = StartCoroutine(DrawLightning());
	}

	IEnumerator DrawLightning ()
	{
		AnimateLight = true;
		LightningBolt.enabled = true;
        StartCoroutine(ThunderSoundDelay());
		LightningBolt.widthMultiplier = 0;

        EndingPoint.GetComponent<LightningStrike>().CreateLightningStrike();
		if (EndingPoint.GetComponent<LightningStrike>().HitPosition != Vector3.zero)
		{
			Vector3 OffSet = new Vector3(Random.Range(-20,20),0,Random.Range(-20,20));
			EndingPoint.position = EndingPoint.GetComponent<LightningStrike>().HitPosition;
			StartingPoint.position = new Vector3(EndingPoint.position.x, StartingPoint.position.y, EndingPoint.position.z)+OffSet;
		}

		while (Generated)
		{
			m_FlashTimer += Time.deltaTime;

            GeneratePoints();

			LightningBolt.widthMultiplier = Mathf.PingPong(Time.time*6f,1.25f);

			if (m_FlashTimer >= m_FlashSeconds && LightningBolt.widthMultiplier <= 0.06f)
			{
				Generated = false;
			}

			yield return null;
		}
		m_FlashTimer = 0;
		LightningBolt.widthMultiplier = 0;
		LightningBolt.enabled = false;

		EndingPoint.GetComponent<LightningStrike>().HitPosition = Vector3.zero;
		Vector3 GeneratedPosition = new Vector3(PlayerTransform.position.x, PlayerTransform.position.y, PlayerTransform.position.z)  + new Vector3(Random.insideUnitSphere.x, 0,Random.insideUnitSphere.z) * LightningGenerationDistance;
		Vector3 RandomOffSet = new Vector3(0,Random.Range(0,20),0);
		StartingPoint.position = GeneratedPosition+new Vector3(0,90,0)+RandomOffSet;
		EndingPoint.position = GeneratedPosition+RandomOffSet;
    }

    //When a lightning strike is generated, get the distance between the player and the lightning position.
    //Create a delay based on the distance to simulate the sound having to travel.
    IEnumerator ThunderSoundDelay ()
    {
        float DistanceDelay = Vector3.Distance(EndingPoint.position, PlayerTransform.position)/50;
        yield return new WaitForSeconds(DistanceDelay);
        AS.pitch = Random.Range(0.7f, 1.3f);
        AS.PlayOneShot(ThunderSounds[Random.Range(0, ThunderSounds.Count)]);
    }
}
