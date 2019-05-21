using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherTypeByTrigger : MonoBehaviour
{
    public WeatherType m_WeatherType;
    public string TriggerTag = "Player";

    void OnTriggerEnter(Collider C)
    {
        if (C.tag == TriggerTag)
        {
            UniStormSystem.Instance.ChangeWeather(m_WeatherType);
        }
    }
}