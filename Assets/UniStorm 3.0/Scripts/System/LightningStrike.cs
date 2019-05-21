using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStrike : MonoBehaviour {
    [HideInInspector]
	public GameObject LightningStrikeFire;
    [HideInInspector]
    public GameObject LightningStrikeEffect;
    [HideInInspector]
    public Vector3 HitPosition;
    [HideInInspector]
    public bool PlayerDetected = false;
    [HideInInspector]
    public int GroundStrikeOdds = 50;
    int RaycastDistance = 75;
    [HideInInspector]
    public bool LightningGenerated = false;
    [HideInInspector]
    public LayerMask DetectionLayerMask;
    [HideInInspector]
    public bool ObjectDetected = false;
    [HideInInspector]
    public string FireTag = "Finish";
    [HideInInspector]
    public List<string> LightningFireTags = new List<string>();
    [HideInInspector]
    public GameObject HitObject;

    void Start ()
	{
		UniStormSystem UniStormSystemTemp = GameObject.Find("UniStorm System").GetComponent<UniStormSystem>();
		UniStormSystemTemp.m_LightningStrikeSystem = GetComponent<LightningStrike>();
		GroundStrikeOdds = UniStormSystemTemp.LightningGroundStrikeOdds;
		LightningStrikeEffect = UniStormSystemTemp.LightningStrikeEffect;
		LightningStrikeFire = UniStormSystemTemp.LightningStrikeFire;
        DetectionLayerMask = UniStormSystemTemp.DetectionLayerMask;
        LightningFireTags = UniStormSystemTemp.LightningFireTags;
        GetComponent<SphereCollider>().radius = UniStormSystemTemp.LightningDetectionDistance;

        HitPosition = Vector3.zero+new Vector3(0,1000,0);
	}

    void OnTriggerEnter(Collider C)
    {
        if (C.gameObject.layer != 2 && C.GetComponent<Terrain>() == null && (DetectionLayerMask.value & 1 << C.gameObject.layer) != 0)
        {
            ObjectDetected = true;
            HitPosition = C.transform.position;
            HitObject = C.gameObject;

            if (C.tag == "Player")
            {
                HitPosition = C.transform.position;
                PlayerDetected = true;
            }
        }
    }

    public void CreateLightningStrike ()
	{
        RaycastHit hit;

        int Roll = Random.Range(1, 101);

        if (Roll <= GroundStrikeOdds)
        {
            RaycastDistance = 150;
        }
        else
        {
            RaycastDistance = 1;
        }

        if (!ObjectDetected)
        {
            HitPosition = transform.position;
        }

        if (Physics.Raycast(new Vector3(HitPosition.x, HitPosition.y+40, HitPosition.z), -transform.up, out hit, RaycastDistance, DetectionLayerMask))
        {
            Vector3 pos = hit.point;
            LightningGenerated = true;

            if (hit.collider.GetComponent<Terrain>() != null && !ObjectDetected)
            {
                HitPosition = new Vector3(pos.x, hit.collider.GetComponent<Terrain>().SampleHeight(hit.point) + 0.5f, pos.z);
            }
            else
            {
                HitPosition = new Vector3(HitPosition.x, pos.y + 0.5f, HitPosition.z);
            }

            if (!PlayerDetected)
            {
                //If our hit object contains a LightningFireTag, start a fire.
                if (LightningFireTags.Contains(hit.collider.tag))
                {
                    GameObject HitEffect = UniStormPool.Spawn(LightningStrikeFire, HitPosition, Quaternion.identity);
                    HitEffect.transform.SetParent(hit.collider.transform);
                }

                UniStormPool.Spawn(LightningStrikeEffect, HitPosition, Quaternion.identity);
            }
            else if (PlayerDetected)
            {
                if (LightningFireTags.Contains(hit.collider.tag))
                {
                    UniStormPool.Spawn(LightningStrikeFire, HitPosition, Quaternion.identity);
                }

                UniStormPool.Spawn(LightningStrikeEffect, HitPosition, Quaternion.identity);
            }

            LightningGenerated = false;
            ObjectDetected = false;
            PlayerDetected = false;
            HitObject = null;
        }
    }
}
