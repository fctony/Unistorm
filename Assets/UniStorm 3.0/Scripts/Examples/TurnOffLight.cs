using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffLight : MonoBehaviour {

    Light LightSource;

	void Start () {
        LightSource = GetComponent<Light>();

        if (UniStormSystem.Instance.CurrentTimeOfDay == UniStormSystem.CurrentTimeOfDayEnum.Night)
        {
            LightSource.enabled = true;
        }
        else
        {
            LightSource.enabled = false;
        }
    }

	void Update ()
    {
        if (UniStormSystem.Instance.CurrentTimeOfDay == UniStormSystem.CurrentTimeOfDayEnum.Night)
        {
            LightSource.enabled = true;
        }
        else
        {
            LightSource.enabled = false;
        }
    }
}
