using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniStormManager : MonoBehaviour {

    public static UniStormManager Instance = null;

    void Start ()
    {
        Instance = this;
    }

    /// <summary>
    /// Sets UniStorm's Time
    /// </summary>
    public void SetTime (int Hour, int Minute)
    {
        UniStormSystem.Instance.m_TimeFloat = (float)Hour / 24 + (float)Minute / 1440;
    }

    /// <summary>
    /// Sets UniStorm's Date
    /// </summary>
    public void SetDate(int Year, int Month, int Day)
    {
        UniStormSystem.Instance.UniStormDate = new System.DateTime(Year, Month, Day);
    }

    /// <summary>
    /// Sets UniStorm's Date
    /// </summary>
    public System.DateTime GetDate()
    {
        return UniStormSystem.Instance.UniStormDate.Date;
    }

    /// <summary>
    /// Changes UniStorm's weather, regardless of conditions, with the transition speed to the weather type parameter. 
    /// </summary>
    public void ChangeWeatherWithTransition(WeatherType weatherType)
    {
        if (UniStormSystem.Instance.UniStormInitialized)
        {
            if (UniStormSystem.Instance.AllWeatherTypes.Contains(weatherType))
            {
                UniStormSystem.Instance.ChangeWeather(weatherType);
            }
            else
            {
                Debug.LogError("The weather type was not found in UniStorm Editor's All Weather Types list. The weather type must be added to the UniStorm Editor's All Weather Type list before it can be used.");
            }
        }
    }

    /// <summary>
    /// Changes UniStorm's weather instantly, regardless of conditions, to the weather type parameter. 
    /// </summary>
    public void ChangeWeatherInstantly(WeatherType weatherType)
    {
        if (UniStormSystem.Instance.UniStormInitialized)
        {
            if (UniStormSystem.Instance.AllWeatherTypes.Contains(weatherType))
            {
                UniStormSystem.Instance.CurrentWeatherType = weatherType;
                UniStormSystem.Instance.InitializeWeather(false);
            }
            else
            {
                Debug.LogError("The weather type was not found in UniStorm Editor's All Weather Types list. The weather type must be added to the UniStorm Editor's All Weather Type list before it can be used.");
            }
        }
    }

    /// <summary>
    /// Generates a random weather type, regardless of conditions, from UniStorm's All Weather Type list 
    /// </summary>
    public void RandomWeather()
    {
        if (UniStormSystem.Instance.UniStormInitialized)
        {
            int RandomWeatherType = Random.Range(0, UniStormSystem.Instance.AllWeatherTypes.Count);
            UniStormSystem.Instance.ChangeWeather(UniStormSystem.Instance.AllWeatherTypes[RandomWeatherType]);
        }
    }

    /// <summary>
    /// Regenerates UniStorm's forecast using the precipitation odds and weather type conditions, but does not change the weather. 
    /// If you are using the Daily weather generation type, a new forecast weather will be generated along with a new forecast hour.
    /// If you are using the Hourly weather generation type, a new forecast weather will be generated for all 24 hours.
    /// </summary>
    public void RegenerateForecast()
    {
        if (UniStormSystem.Instance.UniStormInitialized)
        {
            UniStormSystem.Instance.WeatherForecast.Clear();
            UniStormSystem.Instance.GenerateWeather();
        }
    }

    /// <summary>
    /// Disables or enables all UniStorm weather sounds depeding on the ActiveState bool, but does not affect their current volume.
    /// </summary>
    public void ChangeWeatherSoundsState(bool ActiveState)
    {
        if (UniStormSystem.Instance.UniStormInitialized)
        {
            UniStormSystem.Instance.m_SoundTransform.SetActive(ActiveState);
        }
    }

    /// <summary>
    /// Disables or enables all UniStorm particle effects depeding on the ActiveState bool, , but does not affect their emission amount.
    /// </summary>
    public void ChangeWeatherEffectsState(bool ActiveState)
    {
        if (UniStormSystem.Instance.UniStormInitialized)
        {
            UniStormSystem.Instance.m_EffectsTransform.SetActive(ActiveState);
        }
    }

    /// <summary>
    /// Change the player transform and player camera to UniStorm, if they need to be changed or updated.
    /// The Camera Source's Far Clipping Plane will be set to 16,000 so it can properly see all of UniStorm's components.
    /// UniStorm's weather type components will be moved to the new Player Transform's transform.
    /// </summary>
    public void ChangePlayerComponents(Transform PlayerTransform, Camera CameraSource)
    {
        if (UniStormSystem.Instance.UniStormInitialized)
        {
            UniStormSystem.Instance.PlayerTransform = PlayerTransform;
            UniStormSystem.Instance.m_EffectsTransform.transform.SetParent(PlayerTransform);
            UniStormSystem.Instance.m_EffectsTransform.transform.localPosition = Vector3.zero;
            UniStormSystem.Instance.m_SoundTransform.transform.SetParent(PlayerTransform);
            UniStormSystem.Instance.m_SoundTransform.transform.localPosition = Vector3.zero;
            UniStormSystem.Instance.PlayerCamera = CameraSource;
            UniStormSystem.Instance.PlayerCamera.farClipPlane = 16000;
        }
    }

    /// <summary>
    /// Gets the forecasted weather type's name
    /// </summary>
    public string GetWeatherForecastName()
    {
        return UniStormSystem.Instance.NextWeatherType.WeatherTypeName;
    }

    /// <summary>
    /// Gets the hour that the forecasted weather will change
    /// </summary>
    public int GetWeatherForecastHour()
    {
        return UniStormSystem.Instance.HourToChangeWeather;
    }

    /// <summary>
    /// Set UniStorm's Music volume using a value from 0 (Fully muted) to 1 (Full volume).
    /// </summary>
    public void SetMusicVolume(float Volume)
    {
        if (UniStormSystem.Instance.UniStormInitialized)
        {
            if (Volume <= 0)
            {
                Volume = 0.001f;
            }
            else if (Volume > 1)
            {
                Volume = 1;
            }
            UniStormSystem.Instance.UniStormAudioMixer.SetFloat("MusicVolume", Mathf.Log(Volume) * 20);
        }
    }

    /// <summary>
    /// Set UniStorm's Ambience volume using a value from 0 (Fully muted) to 1 (Full volume).
    /// </summary>
    public void SetAmbienceVolume(float Volume)
    {
        if (UniStormSystem.Instance.UniStormInitialized)
        {
            if (Volume <= 0)
            {
                Volume = 0.001f;
            }
            else if (Volume > 1)
            {
                Volume = 1;
            }
            UniStormSystem.Instance.UniStormAudioMixer.SetFloat("AmbienceVolume", Mathf.Log(Volume) * 20);
        }
    }

    /// <summary>
    /// Set UniStorm's Weather volume using a value from 0 (Fully muted) to 1 (Full volume).
    /// </summary>
    public void SetWeatherVolume(float Volume)
    {
        if (UniStormSystem.Instance.UniStormInitialized)
        {
            if (Volume <= 0)
            {
                Volume = 0.001f;
            }
            else if (Volume > 1)
            {
                Volume = 1;
            }
            UniStormSystem.Instance.UniStormAudioMixer.SetFloat("WeatherVolume", Mathf.Log(Volume) * 20);
        }
    }

    /// <summary>
    /// Sets the length, in minutes, of UniStorm's days
    /// </summary>
    public void SetDayLength(int MinuteLength)
    {
        UniStormSystem.Instance.DayLength = MinuteLength;
    }

    /// <summary>
    /// Sets the length, in minutes, of UniStorm's night
    /// </summary>
    public void SetNightLength(int MinuteLength)
    {
        UniStormSystem.Instance.NightLength = MinuteLength;
    }

    /// <summary>
    /// Changes UniStorm's moon phase color. The updated color will be applied at noon when UniStorm's moon is updated.
    /// </summary>
    public void ChangeMoonPhaseColor(Color MoonPhaseColor)
    {
        UniStormSystem.Instance.MoonPhaseColor = MoonPhaseColor;
    }

    /// <summary>
    /// Gets the current dominating precipitation type. This can only be Rain or Snow and is based off of a Weather Type's Shader Control Type.
    /// </summary>
    public string GetCurrentPrecipitationType()
    {
        if (Shader.GetGlobalFloat("_WetnessStrength") > Shader.GetGlobalFloat("_SnowStrength"))
        {
            return "Rain";
        }
        else if (Shader.GetGlobalFloat("_SnowStrength") > Shader.GetGlobalFloat("_WetnessStrength"))
        {
            return "Snow";
        }
        else
        {
            return "None";
        }
    }
}
