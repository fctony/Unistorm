using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UniStormSystem : MonoBehaviour {

    public static UniStormSystem Instance = null;

    //Events
    public UnityEvent OnHourChangeEvent;
    public UnityEvent OnDayChangeEvent;
    public UnityEvent OnMonthChangeEvent;
    public UnityEvent OnYearChangeEvent;
    public UnityEvent OnWeatherChangeEvent;

    //Audio Mixer Volumes
    public float WeatherSoundsVolume = 1;
    public float AmbienceVolume = 1;
    public float MusicVolume = 1;

    //UI
    public Slider TimeSlider;
    public GameObject WeatherButtonGameObject;
    public GameObject TimeSliderGameObject;
    public Dropdown WeatherDropdown;
    public EnableFeature UseUniStormMenu = EnableFeature.Enabled;
    public KeyCode UniStormMenuKey = KeyCode.Escape;
    public GameObject UniStormCanvas;
    public bool m_MenuToggle = true;

    //Editor
    public int TabNumber = 0;
    public int TimeTabNumbers = 0;
    public int WeatherTabNumbers = 0;
    public int CelestialTabNumbers = 0;
    public bool TimeFoldout = true, DateFoldout = true, TimeSoundsFoldout = true, TimeMusicFoldout = true,
        SunFoldout = true, MoonFoldout = true, AtmosphereFoldout = true,
        WeatherFoldout = true, LightningFoldout = true, CameraFoldout = true, SettingsFoldout = true;
    public UniStormProfile m_UniStormProfile;
    public string FilePath = "";
    public UniStormProfileTypeEnum UniStormProfileType;
    public enum UniStormProfileTypeEnum
    {
        Import,
        Export
    }

    //Camera & Player
    public Transform PlayerTransform;
    public Camera PlayerCamera;
    public bool m_PlayerFound = false;
    public EnableFeature UniStormFollowsPlayer = EnableFeature.Disabled;
    public EnableFeature GetPlayerAtRuntime = EnableFeature.Disabled;
    public EnableFeature UseRuntimeDelay = EnableFeature.Disabled;
    public GetPlayerMethodEnum GetPlayerMethod = GetPlayerMethodEnum.ByTag;
    public enum GetPlayerMethodEnum {ByTag, ByName};
    public string PlayerTag = "Player";
    public string PlayerName = "Player";
    public string CameraTag = "MainCamera";
    public string CameraName = "MainCamera";

    //Time
    public System.DateTime UniStormDate;
    public int StartingMinute = 0;
    public int StartingHour = 0;
    public int Minute = 1;
    public int Hour = 0;
    public int Day = 0;
    public int Month = 0;
    public int Year = 0;
    public int DayLength = 10;
    public int NightLength = 10;
    public float m_TimeFloat;
    public EnableFeature TimeFlow = EnableFeature.Enabled;
    public EnableFeature RealWorldTime = EnableFeature.Disabled;
    float m_roundingCorrection;
    float m_PreciseCurveTime;
    public bool m_HourUpdate = false;
    float m_TimeOfDaySoundsTimer = 0;
    int m_TimeOfDaySoundsSeconds = 10;
    public int TimeOfDaySoundsSecondsMin = 10;
    public int TimeOfDaySoundsSecondsMax = 30;
    public List<AudioClip> MorningSounds = new List<AudioClip>();
    public List<AudioClip> DaySounds = new List<AudioClip>();
    public List<AudioClip> EveningSounds = new List<AudioClip>();
    public List<AudioClip> NightSounds = new List<AudioClip>();
    public AudioSource TimeOfDayAudioSource;
    public List<AudioClip> MorningMusic = new List<AudioClip>();
    public List<AudioClip> DayMusic = new List<AudioClip>();
    public List<AudioClip> EveningMusic = new List<AudioClip>();
    public List<AudioClip> NightMusic = new List<AudioClip>();
    public AudioSource TimeOfDayMusicAudioSource;
    public int TimeOfDayMusicDelay = 1;
    float m_CurrentMusicClipLength = 0;
    float m_TimeOfDayMusicTimer = 0;
    public EnableFeature TimeOfDaySoundsDuringPrecipitationWeather = EnableFeature.Disabled;
    float m_CurrentClipLength = 0;
    int m_LastHour;

    public CurrentTimeOfDayEnum CurrentTimeOfDay;
    public enum CurrentTimeOfDayEnum
    {
        Morning = 0, Day, Evening, Night
    }

    public WeatherGenerationMethodEnum WeatherGenerationMethod = WeatherGenerationMethodEnum.Daily;
    public List<WeatherType> WeatherForecast = new List<WeatherType>();
    public enum WeatherGenerationMethodEnum
    {
        Hourly = 0, Daily = 1
    }

    //General Enums
    public enum EnableFeature
    {
        Enabled = 0, Disabled = 1
    }

    //Weather
    public AnimationCurve PrecipitationGraph = AnimationCurve.Linear(1, 0, 13, 100);
    public List<WeatherType> NonPrecipiationWeatherTypes = new List<WeatherType>();
    public List<WeatherType> PrecipiationWeatherTypes = new List<WeatherType>();
    public List<WeatherType> AllWeatherTypes = new List<WeatherType>();
    public WeatherType CurrentWeatherType;
    public WeatherType NextWeatherType;
    public int m_PrecipitationOdds = 50;
    float m_CurrentPrecipitationAmountFloat = 1;
    int m_CurrentPrecipitationAmountInt = 1;
    public static bool m_IsFading;
    public int TransitionSpeed = 45;
    public int HourToChangeWeather;
    int m_GeneratedOdds;
    bool m_WeatherGenerated = false;
    Coroutine CloudCoroutine, FogCoroutine, WeatherEffectCoroutine, AdditionalWeatherEffectCoroutine, ParticleFadeCoroutine;
    Coroutine AdditionalParticleFadeCoroutine, SunCoroutine, MoonCoroutine, WindCoroutine, SoundInCoroutine, SoundOutCoroutine;
    Coroutine LightningCloudsCoroutine, ColorCoroutine, CloudHeightCoroutine, RainShaderCoroutine, SnowShaderCoroutine;
    public WindZone UniStormWindZone;
    public GameObject m_SoundTransform;
    public GameObject m_EffectsTransform;
    Light m_LightningLight;
    LightningSystem m_UniStormLightningSystem;
    public LightningStrike m_LightningStrikeSystem;
    public int LightningSecondsMin = 5;
    public int LightningSecondsMax = 10;
    int m_LightningSeconds;
    float m_LightningTimer;
    public List<AnimationCurve> LightningFlashPatterns = new List<AnimationCurve>();
    public List<AudioClip> ThunderSounds = new List<AudioClip>();
    public int LightningGroundStrikeOdds = 50;
    public GameObject LightningStrikeEffect;
    public GameObject LightningStrikeFire;
    public EnableFeature LightningOnClouds = EnableFeature.Enabled;
    Material m_LightningFlashMaterial;
    public int CloudSpeed = 8;
    public LayerMask DetectionLayerMask;
    public List<string> LightningFireTags = new List<string>();
    public float LightningLightIntensityMin = 1;
    public float LightningLightIntensityMax = 3;
    public float CurrentFogAmount;
    public int LightningGenerationDistance = 100;
    public int LightningDetectionDistance = 20;
    public int m_CloudSeed;
    public Color CurrentFogColor;
    public float SnowAmount = 0;
    public float CurrentWindIntensity = 0;
    WeatherType TempWeatherType;
    public CurrentSeasonEnum CurrentSeason;
    public enum CurrentSeasonEnum
    {
        Spring = 1, Summer = 2, Fall = 3, Winter = 4
    }

    //Temperature
    public TemperatureTypeEnum TemperatureType = TemperatureTypeEnum.Fahrenheit;
    public enum TemperatureTypeEnum
    {
        Fahrenheit, Celsius
    }
    public AnimationCurve TemperatureCurve = AnimationCurve.Linear(1, -100, 13, 125);
    public AnimationCurve TemperatureFluctuation = AnimationCurve.Linear(0, -25, 24, 25);
    public int Temperature;
    int m_FreezingTemperature;

    //Celestial
    Renderer m_CloudDomeRenderer;
    Renderer m_CloudDomeLightningRenderer;
    Material m_CloudDomeMaterial;
    Material m_SkyBoxMaterial;
    Renderer m_StarsRenderer;
    Material m_StarsMaterial;
    Light m_SunLight;
    Transform m_CelestialAxisTransform;
    public int SunRevolution = -90;
    public float SunIntensity = 1;
    public float PrecipitationSunIntensity = 0.25f;
    public AnimationCurve SunIntensityCurve = AnimationCurve.Linear(0, 0, 24, 5);
    public AnimationCurve SunSize = AnimationCurve.Linear(0, 1, 24, 10);
    Light m_MoonLight;
    public int MoonPhaseIndex = 5;
    public float MoonBrightness = 0.7f;
    public Material m_MoonPhaseMaterial;
    Renderer m_MoonRenderer;
    Transform m_MoonTransform;
    public float MoonIntensity = 1;
    public float MoonPhaseIntensity = 1;
    public AnimationCurve MoonIntensityCurve = AnimationCurve.Linear(0, 0, 24, 5);
    public AnimationCurve MoonSize = AnimationCurve.Linear(0, 1, 24, 10);
    Vector3 m_MoonStartingSize;
    GameObject m_MoonParent;
    public AnimationCurve AtmosphereThickness = AnimationCurve.Linear(0, 1, 24, 3);
    public float StarSpeed = 0.75f;
    public int SunAngle = 10;
    public int MoonAngle = -10;
    public HemisphereEnum Hemisphere = HemisphereEnum.Northern;
    public enum HemisphereEnum
    {
        Northern = 0, Southern
    }
    [System.Serializable]
    public class MoonPhaseClass
    {
        public Texture MoonPhaseTexture = null;
        public float MoonPhaseIntensity = 1;
    }
    public List<MoonPhaseClass> MoonPhaseList = new List<MoonPhaseClass>();

    //Colors
    public Gradient SunColor;
    public Gradient StormySunColor;
    public Gradient MoonColor;
    public Gradient SkyColor;
    public Gradient AmbientSkyLightColor;
    public Gradient StormyAmbientSkyLightColor;
    public Gradient AmbientEquatorLightColor;
    public Gradient StormyAmbientEquatorLightColor;
    public Gradient AmbientGroundLightColor;
    public Gradient StormyAmbientGroundLightColor;
    public Gradient StarLightColor;
    public Gradient FogColor;
    public Gradient FogStormyColor;
    public Gradient CloudLightColor;
    public Gradient CloudBaseColor;
    public Gradient CloudStormyBaseColor;
    public Gradient SkyTintColor;
    public Color MoonPhaseColor = Color.white;

    float m_FadeValue;
    float m_ReceivedCloudValue;

    public Gradient DefaultCloudBaseColor;
    GradientColorKey[] CloudColorKeySwitcher;

    public Gradient DefaultFogBaseColor;
    GradientColorKey[] FogColorKeySwitcher;

    public Gradient DefaultAmbientSkyLightBaseColor;
    GradientColorKey[] AmbientSkyLightColorKeySwitcher;

    public Gradient DefaultAmbientEquatorLightBaseColor;
    GradientColorKey[] AmbientEquatorLightColorKeySwitcher;

    public Gradient DefaultAmbientGroundLightBaseColor;
    GradientColorKey[] AmbientGroundLightColorKeySwitcher;

    public Gradient DefaultSunLightBaseColor;
    GradientColorKey[] SunLightColorKeySwitcher;

    public List<ParticleSystem> ParticleSystemList = new List<ParticleSystem>();
    public List<ParticleSystem> WeatherEffectsList = new List<ParticleSystem>();
    public List<ParticleSystem> AdditionalParticleSystemList = new List<ParticleSystem>();
    public List<ParticleSystem> AdditionalWeatherEffectsList = new List<ParticleSystem>();
    public List<AudioSource> WeatherSoundsList = new List<AudioSource>();
    public ParticleSystem CurrentParticleSystem;
    public ParticleSystem AdditionalCurrentParticleSystem;

    public bool UniStormInitialized = false;

    //AQUAS support comming soon
    /*
    #if AQUAS_PRESENT
    public float m_AQUAS_CurrentFogValue;
    public bool m_AquasPresent = false;
    AQUAS_LensEffects m_Aquas;
    #endif
    */

    public UnityEngine.Audio.AudioMixer UniStormAudioMixer;

    void Awake ()
    {
        GameObject m_UniStormManager = new GameObject();
        m_UniStormManager.transform.SetParent(this.transform);
        m_UniStormManager.AddComponent<UniStormManager>();
        m_UniStormManager.name = "UniStorm Manager";
        Instance = this;
    }

    void Start()
    {
        if (GetPlayerAtRuntime == EnableFeature.Enabled)
        {
            //Make sure our PlayerTransform is null because we will be looking it up via Unity tag or by name.
            PlayerTransform = null;

            //If our player is being received at runtime, wait to intilialize UniStorm until the player has been found.
            if (UseRuntimeDelay == EnableFeature.Enabled)
            {
                StartCoroutine(InitializeDelay());
            }
            //If our player is being received at runtime and UseRuntimeDelay is disabled, get our player immediately by tag.
            else if (UseRuntimeDelay == EnableFeature.Disabled)
            {
                if (GetPlayerMethod == GetPlayerMethodEnum.ByTag)
                {
                    PlayerTransform = GameObject.FindWithTag(PlayerTag).transform;
                    PlayerCamera = GameObject.FindWithTag(CameraTag).GetComponent<Camera>();
                }
                else if (GetPlayerMethod == GetPlayerMethodEnum.ByName)
                {
                    PlayerTransform = GameObject.Find(PlayerName).transform;
                    PlayerCamera = GameObject.Find(CameraName).GetComponent<Camera>();
                }
                InitializeUniStorm();
            }
        }
        //If our player is not being received at runtime, initialize UniStorm immediately.
        else if (GetPlayerAtRuntime == EnableFeature.Disabled)
        {
            InitializeUniStorm();
        }
    }

    //Wait to intilialize UniStorm until the player has been found.
    IEnumerator InitializeDelay ()
    {
        yield return new WaitWhile(() => PlayerTransform == null);
        if (GetPlayerMethod == GetPlayerMethodEnum.ByTag)
        {
            PlayerTransform = GameObject.FindWithTag(PlayerTag).transform;
            PlayerCamera = GameObject.FindWithTag(CameraTag).GetComponent<Camera>();
        }
        else if (GetPlayerMethod == GetPlayerMethodEnum.ByName)
        {
            PlayerTransform = GameObject.Find(PlayerName).transform;
            PlayerCamera = GameObject.Find(CameraName).GetComponent<Camera>();
        }
        InitializeUniStorm();
    }

    //Intilialize UniStorm
    void InitializeUniStorm()
    {
        StopCoroutine(InitializeDelay());

        if (PlayerTransform == null || PlayerCamera == null)
        {
            Debug.LogWarning("(UniStorm has been disabled) - No player/camera has been assigned on the Player Transform/Player Camera slot." +
                "Please go to the Player & Camera tab and assign one.");
            GetComponent<UniStormSystem>().enabled = false;
        }
        else if (!PlayerTransform.gameObject.activeSelf || !PlayerCamera.gameObject.activeSelf)
        {
            Debug.LogWarning("(UniStorm has been disabled) - The player/camera game object is disabled on the Player Transform/Player Camera slot is disabled. " +
                "Please go to the Player & Camera tab and ensure your player/camera is enabled.");
            GetComponent<UniStormSystem>().enabled = false;
        }

        //If our current weather type is not apart of the available weather type lists, assign it to the proper category.
        if (!AllWeatherTypes.Contains(CurrentWeatherType))
        {
            AllWeatherTypes.Add(CurrentWeatherType);
        }

        if (MusicVolume == 0)
        {
            MusicVolume = 0.001f;
        }
        if (AmbienceVolume == 0)
        {
            AmbienceVolume = 0.001f;
        }
        if (WeatherSoundsVolume == 0)
        {
            WeatherSoundsVolume = 0.001f;
        }
        
        UniStormAudioMixer = Resources.Load("UniStorm Audio Mixer") as UnityEngine.Audio.AudioMixer;
        UniStormAudioMixer.SetFloat("MusicVolume", Mathf.Log(MusicVolume) * 20);
        UniStormAudioMixer.SetFloat("AmbienceVolume", Mathf.Log(AmbienceVolume) * 20);
        UniStormAudioMixer.SetFloat("WeatherVolume", Mathf.Log(WeatherSoundsVolume) * 20);

        //Setup the proper settings for our camera
        PlayerCamera.farClipPlane = 16000;

        //Setup our sound holder
        m_SoundTransform = new GameObject();
        m_SoundTransform.name = "UniStorm Sounds";
        m_SoundTransform.transform.SetParent(PlayerTransform);
        m_SoundTransform.transform.localPosition = Vector3.zero;

        //Setup our particle effects holder
        m_EffectsTransform = new GameObject();
        m_EffectsTransform.name = "UniStorm Effects";
        m_EffectsTransform.transform.SetParent(PlayerTransform);
        m_EffectsTransform.transform.localPosition = Vector3.zero;

        for (int i = 0; i < AllWeatherTypes.Count; i++)
        {
            if (AllWeatherTypes[i].PrecipitationWeatherType == WeatherType.Yes_No.Yes && !PrecipiationWeatherTypes.Contains(AllWeatherTypes[i]))
            {
                PrecipiationWeatherTypes.Add(AllWeatherTypes[i]);
            }
            else if (AllWeatherTypes[i].PrecipitationWeatherType == WeatherType.Yes_No.No && !NonPrecipiationWeatherTypes.Contains(AllWeatherTypes[i]))
            {
                NonPrecipiationWeatherTypes.Add(AllWeatherTypes[i]);
            }
        }

        //Sets up and checks all of our weather types
        for (int i = 0; i < AllWeatherTypes.Count; i++)
        {
            //If our weather types have certain features enabled, but there are none detected, disable the feature.
            if (AllWeatherTypes[i].UseWeatherSound == WeatherType.Yes_No.Yes && AllWeatherTypes[i].WeatherSound == null)
            {
                AllWeatherTypes[i].UseWeatherSound = WeatherType.Yes_No.No;
            }

            if (AllWeatherTypes[i].UseWeatherEffect == WeatherType.Yes_No.Yes && AllWeatherTypes[i].WeatherEffect == null)
            {
                AllWeatherTypes[i].UseWeatherEffect = WeatherType.Yes_No.No;
            }

            if (AllWeatherTypes[i].UseAdditionalWeatherEffect == WeatherType.Yes_No.Yes && AllWeatherTypes[i].AdditionalWeatherEffect == null)
            {
                AllWeatherTypes[i].UseAdditionalWeatherEffect = WeatherType.Yes_No.No;
            }

            //Add all of our weather effects to a list to be controlled when needed.
            if (!ParticleSystemList.Contains(AllWeatherTypes[i].WeatherEffect) && AllWeatherTypes[i].WeatherEffect != null)
            {
                AllWeatherTypes[i].CreateWeatherEffect();
                ParticleSystemList.Add(AllWeatherTypes[i].WeatherEffect);
            }

            //Add all of our additional weather effects to a list to be controlled when needed.
            if (!AdditionalParticleSystemList.Contains(AllWeatherTypes[i].AdditionalWeatherEffect))
            {
                if (AllWeatherTypes[i].UseAdditionalWeatherEffect == WeatherType.Yes_No.Yes)
                {
                    AllWeatherTypes[i].CreateAdditionalWeatherEffect();
                    AdditionalParticleSystemList.Add(AllWeatherTypes[i].AdditionalWeatherEffect);
                }
            }

            //Create a weather sound for each weather type that has one.
            if (AllWeatherTypes[i].UseWeatherSound == WeatherType.Yes_No.Yes && AllWeatherTypes[i].WeatherSound != null)
            {
                AllWeatherTypes[i].CreateWeatherSound();
            }
        }

        //Initialize our color switching keys. This allows gradient colors to be switched between stormy and regular.
        CloudColorKeySwitcher = new GradientColorKey[8];
        CloudColorKeySwitcher = CloudBaseColor.colorKeys;
        DefaultCloudBaseColor.colorKeys = new GradientColorKey[8];
        DefaultCloudBaseColor.colorKeys = CloudBaseColor.colorKeys;

        FogColorKeySwitcher = new GradientColorKey[8];
        FogColorKeySwitcher = FogColor.colorKeys;
        DefaultFogBaseColor.colorKeys = new GradientColorKey[8];
        DefaultFogBaseColor.colorKeys = FogColor.colorKeys;

        AmbientSkyLightColorKeySwitcher = new GradientColorKey[8];
        AmbientSkyLightColorKeySwitcher = AmbientSkyLightColor.colorKeys;
        DefaultAmbientSkyLightBaseColor.colorKeys = new GradientColorKey[8];
        DefaultAmbientSkyLightBaseColor.colorKeys = AmbientSkyLightColor.colorKeys;

        AmbientEquatorLightColorKeySwitcher = new GradientColorKey[8];
        AmbientEquatorLightColorKeySwitcher = AmbientEquatorLightColor.colorKeys;
        DefaultAmbientEquatorLightBaseColor.colorKeys = new GradientColorKey[8];
        DefaultAmbientEquatorLightBaseColor.colorKeys = AmbientEquatorLightColor.colorKeys;

        AmbientGroundLightColorKeySwitcher = new GradientColorKey[8];
        AmbientGroundLightColorKeySwitcher = AmbientGroundLightColor.colorKeys;
        DefaultAmbientGroundLightBaseColor.colorKeys = new GradientColorKey[8];
        DefaultAmbientGroundLightBaseColor.colorKeys = AmbientGroundLightColor.colorKeys;

        SunLightColorKeySwitcher = new GradientColorKey[6];
        SunLightColorKeySwitcher = SunColor.colorKeys;
        DefaultSunLightBaseColor.colorKeys = new GradientColorKey[6];
        DefaultSunLightBaseColor.colorKeys = SunColor.colorKeys;

        CalculatePrecipiation();
        CreateSun();
        CreateMoon();

        //Intialize our other components and set the proper settings from within the editor
        GameObject TempAudioSource = new GameObject("UniStorm Time of Day Sounds");
        TempAudioSource.transform.SetParent(this.transform);
        TempAudioSource.transform.localPosition = Vector3.zero;
        TempAudioSource.AddComponent<AudioSource>();
        TimeOfDayAudioSource = TempAudioSource.GetComponent<AudioSource>();
        TimeOfDayAudioSource.outputAudioMixerGroup = UniStormAudioMixer.FindMatchingGroups("Master/Ambience")[0];
        m_TimeOfDaySoundsSeconds = Random.Range(TimeOfDaySoundsSecondsMin, TimeOfDaySoundsSecondsMax+1);

        GameObject TempAudioSourceMusic = new GameObject("UniStorm Time of Day Music");
        TempAudioSourceMusic.transform.SetParent(this.transform);
        TempAudioSourceMusic.transform.localPosition = Vector3.zero;
        TempAudioSourceMusic.AddComponent<AudioSource>();
        TimeOfDayMusicAudioSource = TempAudioSourceMusic.GetComponent<AudioSource>();
        TimeOfDayMusicAudioSource.outputAudioMixerGroup = UniStormAudioMixer.FindMatchingGroups("Master/Music")[0];

        UniStormWindZone = GameObject.Find("UniStorm Windzone").GetComponent<WindZone>();

        m_StarsRenderer = GameObject.Find("UniStorm Stars").GetComponent<Renderer>();
        m_StarsMaterial = m_StarsRenderer.material;
        m_StarsMaterial.SetFloat("_StarSpeed", StarSpeed);

        m_CloudDomeRenderer = GameObject.Find("UniStorm Clouds").GetComponent<Renderer>();
        m_CloudDomeMaterial = m_CloudDomeRenderer.material;
        m_CloudDomeMaterial.SetVector("_CloudSpeed", new Vector4(CloudSpeed*0.0001f, 0, 0, 0));

        m_CloudDomeLightningRenderer = GameObject.Find("UniStorm Clouds (Lightning)").GetComponent<Renderer>();
        m_LightningFlashMaterial = m_CloudDomeLightningRenderer.material;

        //Calculates our start time based off the user's input
        float StartingMinuteFloat = (int)Minute;
        if (RealWorldTime == UniStormSystem.EnableFeature.Disabled)
        {
            m_TimeFloat = (float)Hour / 24 + StartingMinuteFloat / 1440;
        }
        else if (RealWorldTime == UniStormSystem.EnableFeature.Enabled)
        {
            m_TimeFloat = (float)System.DateTime.Now.Hour / 24 + (float)System.DateTime.Now.Minute / 1440;
        }

        m_LastHour = Hour;
        m_SunLight.intensity = SunIntensityCurve.Evaluate((float)Hour) * SunIntensity;
        m_MoonLight.intensity = MoonIntensityCurve.Evaluate((float)Hour) * MoonIntensity * MoonPhaseIntensity;

        m_SkyBoxMaterial = (Material)Resources.Load("UniStorm Skybox") as Material;
        RenderSettings.skybox = m_SkyBoxMaterial;
        m_SkyBoxMaterial.SetFloat("_AtmosphereThickness", AtmosphereThickness.Evaluate((float)Hour));
        m_SkyBoxMaterial.SetColor("_NightSkyTint", SkyTintColor.Evaluate((float)Hour));

        Temperature = (int)TemperatureCurve.Evaluate(m_PreciseCurveTime) + (int)TemperatureFluctuation.Evaluate((float)StartingHour);

        if (TemperatureType == TemperatureTypeEnum.Fahrenheit)
        {
            m_FreezingTemperature = 32;
        }
        else if (TemperatureType == TemperatureTypeEnum.Celsius)
        {
            m_FreezingTemperature = 0;
        }

        transform.position = new Vector3(PlayerTransform.position.x, transform.position.y, PlayerTransform.position.z);

        GenerateWeather();
        CreateLightning();
        UpdateColors();
        CalculateClouds();
        CalculateMoonPhase();
        InitializeWeather(true);
        CalculateTimeOfDay();
        CalculateSeason();

        if (WeatherGenerationMethod == WeatherGenerationMethodEnum.Hourly)
        {
            WeatherForecast[Hour] = CurrentWeatherType;
        }

        //Only create our UniStorm UI if it is enabled
        if (UseUniStormMenu == UniStormSystem.EnableFeature.Enabled)
        {
            CreateUniStormMenu();
        }

        UniStormInitialized = true;

        //AQUAS support coming soon
        /*
        //Look for AQUAS so it can be used elsewhere
        #if AQUAS_PRESENT
        m_Aquas = FindObjectOfType<AQUAS_LensEffects>();
        if (m_Aquas != null)
        {
            m_AquasPresent = true;
        }
        #endif
        */
}

    //Initialize our starting weather so it fades in instantly on start
    public void InitializeWeather(bool UseWeatherConditions)
    {
        //If our starting weather type's conditions are not met, keep rerolling weather until an appropriate one is found.
        TempWeatherType = CurrentWeatherType;

        if (UseWeatherConditions)
        {
            while (TempWeatherType.TemperatureType == WeatherType.TemperatureTypeEnum.AboveFreezing && Temperature <= m_FreezingTemperature
                || TempWeatherType.Season != WeatherType.SeasonEnum.All && (int)TempWeatherType.Season != (int)CurrentSeason
            || TempWeatherType.TemperatureType == WeatherType.TemperatureTypeEnum.BelowFreezing && Temperature > m_FreezingTemperature)
            {
                if (TempWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.No)
                {
                    TempWeatherType = NonPrecipiationWeatherTypes[Random.Range(0, NonPrecipiationWeatherTypes.Count)];
                }
                else if (TempWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.Yes)
                {
                    TempWeatherType = PrecipiationWeatherTypes[Random.Range(0, PrecipiationWeatherTypes.Count)];
                }
                else
                {
                    break;
                }
            }
        }

        CurrentWeatherType = TempWeatherType;

        m_ReceivedCloudValue = GetCloudLevel(m_ReceivedCloudValue);
        m_CloudDomeMaterial.SetFloat("_CloudCover", m_ReceivedCloudValue);
        RenderSettings.fogDensity = CurrentWeatherType.FogDensity;
        CurrentFogAmount = RenderSettings.fogDensity;
        UniStormWindZone.windMain = CurrentWeatherType.WindIntensity;
        CurrentWindIntensity = CurrentWeatherType.WindIntensity;
        SunIntensity = CurrentWeatherType.SunIntensity;
        MoonIntensity = CurrentWeatherType.MoonIntensity;

        if (CurrentWeatherType.ShaderControl == WeatherType.ShaderControlEnum.Rain)
        {
            Shader.SetGlobalFloat("_WetnessStrength", 1);
            Shader.SetGlobalFloat("_SnowStrength", 0);
        }
        else if (CurrentWeatherType.ShaderControl == WeatherType.ShaderControlEnum.Snow)
        {
            Shader.SetGlobalFloat("_SnowStrength", 1);
            Shader.SetGlobalFloat("_WetnessStrength", 0);
        }
        else
        {
            Shader.SetGlobalFloat("_WetnessStrength", 0);
            Shader.SetGlobalFloat("_SnowStrength", 0);
        }

        if (CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.Yes)
        {
            m_CloudDomeMaterial.SetFloat("_WorldPosGlobal", 2500);
        }

        for (int i = 0; i < WeatherEffectsList.Count; i++)
        {
            ParticleSystem.EmissionModule CurrentEmission = WeatherEffectsList[i].emission;
            CurrentEmission.rateOverTime = new ParticleSystem.MinMaxCurve(0);
        }

        for (int i = 0; i < AdditionalWeatherEffectsList.Count; i++)
        {
            ParticleSystem.EmissionModule CurrentEmission = AdditionalWeatherEffectsList[i].emission;
            CurrentEmission.rateOverTime = new ParticleSystem.MinMaxCurve(0);
        }

        //Initialize our weather type's particle effetcs
        if (CurrentWeatherType.UseWeatherEffect == WeatherType.Yes_No.Yes)
        {
            for (int i = 0; i < WeatherEffectsList.Count; i++)
            {
                if (WeatherEffectsList[i].name == CurrentWeatherType.WeatherEffect.name + " (UniStorm)")
                {
                    CurrentParticleSystem = WeatherEffectsList[i];
                    ParticleSystem.EmissionModule CurrentEmission = CurrentParticleSystem.emission;
                    CurrentEmission.rateOverTime = new ParticleSystem.MinMaxCurve((float)CurrentWeatherType.ParticleEffectAmount);
                }
            }

            CurrentParticleSystem.transform.localPosition = CurrentWeatherType.ParticleEffectVector;
        }

        //Initialize our weather type's additional particle effetcs
        if (CurrentWeatherType.UseAdditionalWeatherEffect == WeatherType.Yes_No.Yes)
        {
            for (int i = 0; i < AdditionalWeatherEffectsList.Count; i++)
            {
                if (AdditionalWeatherEffectsList[i].name == CurrentWeatherType.AdditionalWeatherEffect.name + " (UniStorm)")
                {
                    AdditionalCurrentParticleSystem = AdditionalWeatherEffectsList[i];
                    ParticleSystem.EmissionModule CurrentEmission = AdditionalCurrentParticleSystem.emission;
                    CurrentEmission.rateOverTime = new ParticleSystem.MinMaxCurve((float)CurrentWeatherType.AdditionalParticleEffectAmount);
                }
            }

            AdditionalCurrentParticleSystem.transform.localPosition = CurrentWeatherType.AdditionalParticleEffectVector;
        }

        if (CurrentWeatherType.UseLightning == WeatherType.Yes_No.Yes && CurrentWeatherType.UseWeatherEffect == WeatherType.Yes_No.Yes)
        {
            Color C = m_LightningFlashMaterial.color;
            C.a = 0.5f;
            m_LightningFlashMaterial.color = C;
        }
        
        //Instantly change all of our gradients to the stormy gradients
        if (CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.Yes)
        {
            for (int i = 0; i < CloudBaseColor.colorKeys.Length; i++)
            {
                CloudColorKeySwitcher[i].color = Color.Lerp(CloudColorKeySwitcher[i].color, CloudStormyBaseColor.colorKeys[i].color, 1);
            }

            for (int i = 0; i < FogColor.colorKeys.Length; i++)
            {
                FogColorKeySwitcher[i].color = Color.Lerp(FogColorKeySwitcher[i].color, FogStormyColor.colorKeys[i].color, 1);
            }

            for (int i = 0; i < AmbientSkyLightColor.colorKeys.Length; i++)
            {
                AmbientSkyLightColorKeySwitcher[i].color = Color.Lerp(AmbientSkyLightColorKeySwitcher[i].color, StormyAmbientSkyLightColor.colorKeys[i].color, 1);
            }

            for (int i = 0; i < AmbientEquatorLightColor.colorKeys.Length; i++)
            {
                AmbientEquatorLightColorKeySwitcher[i].color = Color.Lerp(AmbientEquatorLightColorKeySwitcher[i].color, StormyAmbientEquatorLightColor.colorKeys[i].color, 1);
            }

            for (int i = 0; i < AmbientGroundLightColor.colorKeys.Length; i++)
            {
                AmbientGroundLightColorKeySwitcher[i].color = Color.Lerp(AmbientGroundLightColorKeySwitcher[i].color, StormyAmbientGroundLightColor.colorKeys[i].color, 1);
            }

            for (int i = 0; i < SunColor.colorKeys.Length; i++)
            {
                SunLightColorKeySwitcher[i].color = Color.Lerp(SunLightColorKeySwitcher[i].color, StormySunColor.colorKeys[i].color, 1);
            }

            FogColor.SetKeys(FogColorKeySwitcher, FogColor.alphaKeys);
            CloudBaseColor.SetKeys(CloudColorKeySwitcher, CloudBaseColor.alphaKeys);
            AmbientSkyLightColor.SetKeys(AmbientSkyLightColorKeySwitcher, AmbientSkyLightColor.alphaKeys);
            AmbientEquatorLightColor.SetKeys(AmbientEquatorLightColorKeySwitcher, AmbientEquatorLightColor.alphaKeys);
            AmbientGroundLightColor.SetKeys(AmbientGroundLightColorKeySwitcher, AmbientGroundLightColor.alphaKeys);
            SunColor.SetKeys(SunLightColorKeySwitcher, SunColor.alphaKeys);
        }
        //Instantly change all of our gradients to the regular gradients
        else if (CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.No)
        {
            for (int i = 0; i < CloudBaseColor.colorKeys.Length; i++)
            {
                CloudColorKeySwitcher[i].color = Color.Lerp(CloudColorKeySwitcher[i].color, DefaultCloudBaseColor.colorKeys[i].color, 1);
            }

            for (int i = 0; i < FogColor.colorKeys.Length; i++)
            {
                FogColorKeySwitcher[i].color = Color.Lerp(FogColorKeySwitcher[i].color, DefaultFogBaseColor.colorKeys[i].color, 1);
            }

            for (int i = 0; i < AmbientSkyLightColor.colorKeys.Length; i++)
            {
                AmbientSkyLightColorKeySwitcher[i].color = Color.Lerp(AmbientSkyLightColorKeySwitcher[i].color, DefaultAmbientSkyLightBaseColor.colorKeys[i].color, 1);
            }

            for (int i = 0; i < AmbientEquatorLightColor.colorKeys.Length; i++)
            {
                AmbientEquatorLightColorKeySwitcher[i].color = Color.Lerp(AmbientEquatorLightColorKeySwitcher[i].color, DefaultAmbientEquatorLightBaseColor.colorKeys[i].color, 1);
            }

            for (int i = 0; i < AmbientGroundLightColor.colorKeys.Length; i++)
            {
                AmbientGroundLightColorKeySwitcher[i].color = Color.Lerp(AmbientGroundLightColorKeySwitcher[i].color, DefaultAmbientGroundLightBaseColor.colorKeys[i].color, 1);
            }

            for (int i = 0; i < SunColor.colorKeys.Length; i++)
            {
                SunLightColorKeySwitcher[i].color = Color.Lerp(SunLightColorKeySwitcher[i].color, DefaultSunLightBaseColor.colorKeys[i].color, 1);
            }

            FogColor.SetKeys(FogColorKeySwitcher, FogColor.alphaKeys);
            CloudBaseColor.SetKeys(CloudColorKeySwitcher, CloudBaseColor.alphaKeys);
            AmbientSkyLightColor.SetKeys(AmbientSkyLightColorKeySwitcher, AmbientSkyLightColor.alphaKeys);
            AmbientEquatorLightColor.SetKeys(AmbientEquatorLightColorKeySwitcher, AmbientEquatorLightColor.alphaKeys);
            AmbientGroundLightColor.SetKeys(AmbientGroundLightColorKeySwitcher, AmbientGroundLightColor.alphaKeys);
            SunColor.SetKeys(SunLightColorKeySwitcher, SunColor.alphaKeys);
            m_CloudDomeMaterial.SetFloat("_WorldPosGlobal", 0);
        }

        foreach (AudioSource A in WeatherSoundsList)
        {
            A.volume = 0;
        }

        if (CurrentWeatherType.UseWeatherSound == WeatherType.Yes_No.Yes)
        {
            foreach (AudioSource A in WeatherSoundsList)
            {
                if (A.gameObject.name == CurrentWeatherType.WeatherTypeName + " (UniStorm)")
                {
                    A.Play();
                    A.volume = CurrentWeatherType.WeatherVolume;
                }
            }
        }

        if (CurrentWeatherType.CloudLevel == WeatherType.CloudLevelEnum.MostlyCloudy)
        { 
            m_CloudDomeMaterial.SetFloat("_WorldPosGlobal", 500);
        }
        else if (CurrentWeatherType.CloudLevel == WeatherType.CloudLevelEnum.Cloudy)
        {
            m_CloudDomeMaterial.SetFloat("_WorldPosGlobal", 2500);
        }
        else
        {
            m_CloudDomeMaterial.SetFloat("_WorldPosGlobal", 0);
        }
    }

    //If follow player is enabled, adjust the distant UniStorm components to the player's position
    void FollowPlayer ()
    {
        m_CloudDomeRenderer.transform.position = new Vector3(PlayerTransform.position.x, PlayerTransform.position.y - 550, PlayerTransform.position.z);
        m_MoonLight.transform.position = PlayerTransform.position;
        m_StarsRenderer.transform.position = PlayerTransform.position;
    }

    //Calculate our precipitation odds based on the UniStorm date
    void CalculatePrecipiation()
    {
        CalculateMonths(); //Calculate our months
        GetDate(); //Calculate our UniStorm date

        //This algorithm uses an Animation curve to calculate the precipitation odds given the date from the Animation Curve
        m_roundingCorrection = UniStormDate.DayOfYear * 0.00273972602f;
        m_PreciseCurveTime = ((UniStormDate.DayOfYear / 28.07692307692308f)) + 1 - m_roundingCorrection;
        m_PreciseCurveTime = Mathf.Round(m_PreciseCurveTime * 10f) / 10f;

        m_CurrentPrecipitationAmountFloat = PrecipitationGraph.Evaluate(m_PreciseCurveTime);
        m_CurrentPrecipitationAmountInt = (int)Mathf.Round(m_CurrentPrecipitationAmountFloat);
        m_PrecipitationOdds = m_CurrentPrecipitationAmountInt;
    }

    //Create and positioned UniStorm's moon
    void CreateMoon()
    {
        m_MoonLight = GameObject.Find("UniStorm Moon").GetComponent<Light>();
        m_MoonLight.transform.localEulerAngles = new Vector3(-180, MoonAngle, 0);
        GameObject m_CreatedMoon = Instantiate((GameObject)Resources.Load("UniStorm Moon Object") as GameObject, transform.position, Quaternion.identity);
        m_CreatedMoon.name = "UniStorm Moon Object";
        m_MoonRenderer = GameObject.Find("UniStorm Moon Object").GetComponent<Renderer>();
        m_MoonTransform = m_MoonRenderer.transform;
        m_MoonStartingSize = m_MoonTransform.localScale;
        m_MoonPhaseMaterial = m_MoonRenderer.material;
        m_MoonPhaseMaterial.SetColor("_MoonColor", MoonPhaseColor);
        m_MoonTransform.parent = m_MoonLight.transform;
        m_MoonTransform.localPosition = new Vector3(0, 0, -11000);
        m_MoonTransform.localEulerAngles = new Vector3(270, 0, 0);
        m_MoonTransform.localScale = new Vector3(m_MoonTransform.localScale.x, m_MoonTransform.localScale.y, m_MoonTransform.localScale.z);
    }

    //Sets up UniStorm's sun
    void CreateSun ()
    {
        m_SunLight = GameObject.Find("UniStorm Sun").GetComponent<Light>();
        m_SunLight.transform.localEulerAngles = new Vector3(0, SunAngle, 0);
        m_CelestialAxisTransform = GameObject.Find("Celestial Axis").transform;
        RenderSettings.sun = m_SunLight;
        m_SkyBoxMaterial = RenderSettings.skybox;
    }

    //Create, setup, and assign all needed lightning components
    void CreateLightning()
    {
        GameObject CreatedLightningSystem = new GameObject("UniStorm Lightning System");
        CreatedLightningSystem.AddComponent<LightningSystem>();
        m_UniStormLightningSystem = CreatedLightningSystem.GetComponent<LightningSystem>();
        m_UniStormLightningSystem.transform.SetParent(this.transform);

        for (int i = 0; i < ThunderSounds.Count; i++)
        {
            m_UniStormLightningSystem.ThunderSounds.Add(ThunderSounds[i]);
        }

        GameObject CreatedLightningLight = new GameObject("UniStorm Lightning Light");
        CreatedLightningLight.AddComponent<Light>();
        m_LightningLight = CreatedLightningLight.GetComponent<Light>();
        m_LightningLight.type = LightType.Directional;
        m_LightningLight.transform.SetParent(this.transform);
        m_LightningLight.transform.localPosition = Vector3.zero;
        m_LightningLight.intensity = 0;
        m_LightningLight.shadows = LightShadows.Hard;
        m_UniStormLightningSystem.LightningLightSource = m_LightningLight;
        m_UniStormLightningSystem.PlayerTransform = PlayerTransform;
        m_UniStormLightningSystem.LightningGenerationDistance = LightningGenerationDistance;
        m_LightningSeconds = Random.Range(LightningSecondsMin, LightningSecondsMax);
        m_UniStormLightningSystem.LightningLightIntensityMin = LightningLightIntensityMin;
        m_UniStormLightningSystem.LightningLightIntensityMax = LightningLightIntensityMax;
    }

    //A public function for UniStorm's UI Menu to change the weather with a dropdown
    public void ChangeWeatherUI ()
    {
        CurrentWeatherType = AllWeatherTypes[WeatherDropdown.value];
        TransitionWeather();
    }

    //If enabled, create our UniStorm UI and Canvas.
    void CreateUniStormMenu()
    {
        //Resource load UI here
        UniStormCanvas = Instantiate((GameObject)Resources.Load("UniStorm Canvas") as GameObject, transform.position, Quaternion.identity);
        UniStormCanvas.name = "UniStorm Canvas";

        TimeSlider = GameObject.Find("Time Slider").GetComponent<Slider>();
        TimeSliderGameObject = TimeSlider.gameObject;
        TimeSlider.onValueChanged.AddListener(delegate { CalculateTimeSlider(); }); //Create an event to control UniStorm's time with a slider
        TimeSlider.maxValue = 0.995f;

        WeatherButtonGameObject = GameObject.Find("Change Weather Button");

        WeatherDropdown = GameObject.Find("Weather Dropdown").GetComponent<Dropdown>();
        GameObject.Find("Change Weather Button").GetComponent<Button>().onClick.AddListener(delegate { ChangeWeatherUI(); });

        List<string> m_DropOptions = new List<string> { };

        for (int i = 0; i < AllWeatherTypes.Count; i++)
        {
            m_DropOptions.Add(AllWeatherTypes[i].WeatherTypeName);
        }

        WeatherDropdown.AddOptions(m_DropOptions);
        TimeSlider.value = m_TimeFloat;

        WeatherDropdown.value = AllWeatherTypes.IndexOf(CurrentWeatherType);
        
        if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject m_EventSystem = new GameObject();
            m_EventSystem.name = "EventSystem";
            m_EventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            m_EventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }

        m_MenuToggle = false;
        ToggleUniStormMenu();
    }

    //Gets a custom DateTime using UniStorm's current date
    public System.DateTime GetDate()
    {
        if (RealWorldTime == UniStormSystem.EnableFeature.Disabled)
        {
            UniStormDate = new System.DateTime(Year, Month, Day, Hour, Minute, 0);
        }
        else if (RealWorldTime == UniStormSystem.EnableFeature.Enabled)
        {
            UniStormDate = new System.DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, Hour, Minute, 0);
            Year = UniStormDate.Year;
            Month = UniStormDate.Month;
            Day = UniStormDate.Day;
        }

        return UniStormDate;
    }

    //Move our sun according to the time of day
    public void MoveSun()
    {
        m_CelestialAxisTransform.eulerAngles = new Vector3(m_TimeFloat * 360 - 100, SunRevolution, 180);
    }

    public void ToggleUniStormMenu ()
    {
        WeatherButtonGameObject.SetActive(m_MenuToggle);
        TimeSliderGameObject.SetActive(m_MenuToggle);
        m_MenuToggle = !m_MenuToggle;

        int m_AdjustedMenuHeight = 0;

        if (m_MenuToggle)
        {
            m_AdjustedMenuHeight = 300;
        }
        else
        {
            m_AdjustedMenuHeight = -300;
        }

        RectTransform U_Dropdown = GameObject.Find("Weather Dropdown").GetComponent<RectTransform>();
        Vector3 V3 = U_Dropdown.position;
        U_Dropdown.position = new Vector3(V3.x, V3.y + m_AdjustedMenuHeight, V3.z);
    }

    void Update()
    {
        //Only run UniStorm if it has been initialized.
        if (UniStormInitialized)
        {
            if (UseUniStormMenu == EnableFeature.Enabled)
            {
                //Some versions of Unity cannot have the Canvas disabled without causing issues with dropdown menus.
                //So, disable the button and slider gameobjects then move the dropdown menu up 300 units so it is no longer visible. 
                //Revese everything when the menu is enabled again.
                if (Input.GetKeyDown(UniStormMenuKey))
                {
                    ToggleUniStormMenu();
                }
            }

            //Only calculate our time if TimeFlow is enabled
            if (TimeFlow == UniStormSystem.EnableFeature.Enabled)
            {
                if (RealWorldTime == UniStormSystem.EnableFeature.Disabled)
                {
                    if (Hour > 6 && Hour <= 18)
                    {
                        m_TimeFloat = m_TimeFloat + Time.deltaTime / DayLength / 120;
                    }

                    if (Hour > 18 || Hour <= 6)
                    {
                        m_TimeFloat = m_TimeFloat + Time.deltaTime / NightLength / 120;
                    }
                }
                else if (RealWorldTime == UniStormSystem.EnableFeature.Enabled)
                {
                    m_TimeFloat = (float)System.DateTime.Now.Hour / 24 + (float)System.DateTime.Now.Minute / 1440;
                }

                if (m_TimeFloat >= 1.0f)
                {
                    m_TimeFloat = 0;
                    CalculateDays();
                }
            }

            //Calculate our time
            float m_HourFloat = m_TimeFloat * 24;
            Hour = (int)m_HourFloat;
            float m_MinuteFloat = m_HourFloat * 60;
            Minute = (int)m_MinuteFloat % 60;

            if (UseUniStormMenu == EnableFeature.Enabled && !m_MenuToggle)
            {
                if (UniStormCanvas != null && UniStormCanvas.activeSelf)
                {
                    TimeSlider.value = m_TimeFloat;
                }
            }

            //Update all hourly related settings
            if (m_LastHour != Hour)
            {
                m_LastHour = Hour;
                HourlyUpdate();
            }

            MoveSun();
            UpdateColors();
            PlayTimeOfDaySound();
            PlayTimeOfDayMusic();
            CalculateTimeOfDay();

            //Generate our lightning, if the randomized lightning seconds have been met
            if (CurrentWeatherType.UseLightning == WeatherType.Yes_No.Yes && CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.Yes)
            {
                m_LightningTimer += Time.deltaTime;

                //Only create a lightning strike if the clouds have fully faded in
                if (m_LightningTimer >= m_LightningSeconds && m_CloudDomeMaterial.GetFloat("_CloudCover") >= 1)
                {
                    m_UniStormLightningSystem.LightningCurve = LightningFlashPatterns[Random.Range(0, LightningFlashPatterns.Count)];
                    m_UniStormLightningSystem.GenerateLightning();
                    m_LightningSeconds = Random.Range(LightningSecondsMin, LightningSecondsMax);
                    m_LightningTimer = 0;
                }
            }

            //Update our FollowPlayer function
            if (UniStormFollowsPlayer == UniStormSystem.EnableFeature.Enabled)
            {
                FollowPlayer();
            }

            //AQUAS support coming soon
            /*
            //Update AQUAS' current fog value, but only if AQUAS was detected on UniStorm's initialization
            #if AQUAS_PRESENT
            if (m_AquasPresent)
            {
                if (!m_Aquas.underWater && RenderSettings.fogDensity != m_Aquas.underWaterParameters.fogDensity)
                {
                    m_AQUAS_CurrentFogValue = RenderSettings.fogDensity;
                }
            }
            #endif
            */
        }
        else if (GetPlayerAtRuntime == UniStormSystem.EnableFeature.Enabled && !UniStormInitialized)
        {
            //Continue to look for our player until it's found. Once it is, UniStorm can be initialized.
            try
            {
                PlayerTransform = GameObject.FindWithTag(PlayerTag).transform;
                m_PlayerFound = true;
            }
            catch
            {
                m_PlayerFound = false;
            }

        }
    }

    //Generate and return a random cloud intensity based on the current weather type cloud level
    float GetCloudLevel(float MaxValue)
    {
        if (CurrentWeatherType.CloudLevel == WeatherType.CloudLevelEnum.Clear)
        {
            MaxValue = 0.5f;
        }
        else if (CurrentWeatherType.CloudLevel == WeatherType.CloudLevelEnum.MostlyClear)
        {
            MaxValue = Random.Range(0.66f, 0.72f);
        }
        else if (CurrentWeatherType.CloudLevel == WeatherType.CloudLevelEnum.PartyCloudy)
        {
            MaxValue = Random.Range(0.78f, 0.86f);
        }
        else if (CurrentWeatherType.CloudLevel == WeatherType.CloudLevelEnum.MostlyCloudy)
        {
            MaxValue = Random.Range(0.9f, 0.96f);
        }
        else if (CurrentWeatherType.CloudLevel == WeatherType.CloudLevelEnum.Cloudy)
        {
            MaxValue = 1.2f;
        }
        else if (CurrentWeatherType.CloudLevel == WeatherType.CloudLevelEnum.DontChange)
        {
            MaxValue = m_CloudDomeMaterial.GetFloat("_CloudCover");
        }

        return MaxValue;
    }

    //Generate a cloud seed for our clouds
    void CalculateClouds()
    {
        m_CloudSeed = Random.Range(-9999, 10000);
        m_CloudDomeMaterial.SetFloat("_Seed", m_CloudSeed);
    }

    //Used for controlling UniStorm's time slider
    public void CalculateTimeSlider ()
    {
        m_TimeFloat = TimeSlider.value;
    }

    //Calculate all of our hourly updates
    void HourlyUpdate ()
    {
        OnHourChangeEvent.Invoke();

        Temperature = (int)TemperatureCurve.Evaluate(m_PreciseCurveTime) + (int)TemperatureFluctuation.Evaluate((float)Hour);

        if (WeatherGenerationMethod == WeatherGenerationMethodEnum.Hourly)
        {
            if (Hour < 23)
            {
                CurrentWeatherType = WeatherForecast[Hour];
                NextWeatherType = WeatherForecast[Hour + 1];
            }
            else
            {
                CurrentWeatherType = WeatherForecast[Hour];
                NextWeatherType = WeatherForecast[0];
            }
        }

        CheckWeather();

        //If the hour is equal to 12, update our moon phase.
        if (Hour == 12)
        {
            MoonPhaseIndex++;
            CalculateMoonPhase();
        }
    }

    void CalculateTimeOfDay ()
    {
        if (Hour >= 6 && Hour <= 7)
        {
            CurrentTimeOfDay = CurrentTimeOfDayEnum.Morning;
        }
        else if (Hour >= 8 && Hour <= 16)
        {
            CurrentTimeOfDay = CurrentTimeOfDayEnum.Day;
        }
        else if (Hour >= 17 && Hour <= 18)
        {
            CurrentTimeOfDay = CurrentTimeOfDayEnum.Evening;
        }
        else if (Hour >= 19 && Hour <= 23 || Hour >= 0 && Hour <= 5)
        {
            CurrentTimeOfDay = CurrentTimeOfDayEnum.Night;
        }
    }

    //Calculate our seasons based on either the Norhtern or Southern Hemisphere
    public void CalculateSeason ()
    {
        if (Month == 3 && Day >= 20 || Month == 4 || Month == 5 || Month == 6 && Day <= 20)
        {
            if (Hemisphere == HemisphereEnum.Northern)
            {
                CurrentSeason = CurrentSeasonEnum.Spring;
            }
            else if (Hemisphere == HemisphereEnum.Southern)
            {
                CurrentSeason = CurrentSeasonEnum.Fall;
            }
        }
        else if (Month == 6 && Day >= 21 || Month == 7 || Month == 8 || Month == 9 && Day <= 21)
        {
            if (Hemisphere == HemisphereEnum.Northern)
            {
                CurrentSeason = CurrentSeasonEnum.Summer;
            }
            else if (Hemisphere == HemisphereEnum.Southern)
            {
                CurrentSeason = CurrentSeasonEnum.Winter;
            }
        }
        else if (Month == 9 && Day >= 22 || Month == 10 || Month == 11 || Month == 12 && Day <= 20)
        {
            if (Hemisphere == HemisphereEnum.Northern)
            {
                CurrentSeason = CurrentSeasonEnum.Fall;
            }
            else if (Hemisphere == HemisphereEnum.Southern)
            {
                CurrentSeason = CurrentSeasonEnum.Spring;
            }
        }
        else if (Month == 12 && Day >= 21 || Month == 1 || Month == 2 || Month == 3 && Day <= 19)
        {
            if (Hemisphere == HemisphereEnum.Northern)
            {
                CurrentSeason = CurrentSeasonEnum.Winter;
            }
            else if (Hemisphere == HemisphereEnum.Southern)
            {
                CurrentSeason = CurrentSeasonEnum.Summer;
            }
        }
    }

    //Calculates our time of day sounds according to the hour and randomized seconds set by the user.
    void PlayTimeOfDaySound ()
    {
        m_TimeOfDaySoundsTimer += Time.deltaTime;

        if (m_TimeOfDaySoundsTimer >= m_TimeOfDaySoundsSeconds+m_CurrentClipLength)
        {
            if (CurrentWeatherType != null && CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.Yes && 
                TimeOfDaySoundsDuringPrecipitationWeather == UniStormSystem.EnableFeature.Enabled ||
                CurrentWeatherType != null && CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.No && 
                TimeOfDaySoundsDuringPrecipitationWeather == UniStormSystem.EnableFeature.Disabled)
            {
                if (CurrentTimeOfDay == CurrentTimeOfDayEnum.Morning)
                {
                    //Morning Sounds
                    if (MorningSounds.Count != 0)
                    {
                        TimeOfDayAudioSource.clip = MorningSounds[Random.Range(0, MorningSounds.Count)];
                        TimeOfDayAudioSource.Play();
                        m_CurrentClipLength = TimeOfDayAudioSource.clip.length;
                    }
                }
                else if (CurrentTimeOfDay == CurrentTimeOfDayEnum.Day)
                {
                    //Day Sounds
                    if (DaySounds.Count != 0)
                    {
                        TimeOfDayAudioSource.clip = DaySounds[Random.Range(0, DaySounds.Count)];
                        TimeOfDayAudioSource.Play();
                        m_CurrentClipLength = TimeOfDayAudioSource.clip.length;
                    }
                }
                else if (CurrentTimeOfDay == CurrentTimeOfDayEnum.Evening)
                {
                    //Evening Sounds
                    if (EveningSounds.Count != 0)
                    {
                        TimeOfDayAudioSource.clip = EveningSounds[Random.Range(0, EveningSounds.Count)];
                        TimeOfDayAudioSource.Play();
                        m_CurrentClipLength = TimeOfDayAudioSource.clip.length;
                    }
                }
                else if (CurrentTimeOfDay == CurrentTimeOfDayEnum.Night)
                {
                    //Night Sounds
                    if (NightSounds.Count != 0)
                    {
                        TimeOfDayAudioSource.clip = NightSounds[Random.Range(0, NightSounds.Count)];
                        TimeOfDayAudioSource.Play();
                        m_CurrentClipLength = TimeOfDayAudioSource.clip.length;
                    }
                }

                m_TimeOfDaySoundsTimer = 0;
            }
        }
    }

    //Calculates our time of day sounds according to the hour and randomized seconds set by the user.
    void PlayTimeOfDayMusic()
    {
        m_TimeOfDayMusicTimer += Time.deltaTime;

        if (m_TimeOfDayMusicTimer >= m_CurrentMusicClipLength + TimeOfDayMusicDelay)
        {
            if (CurrentTimeOfDay == CurrentTimeOfDayEnum.Morning)
            {
                //Morning Music
                if (MorningMusic.Count != 0)
                {
                    TimeOfDayMusicAudioSource.clip = MorningMusic[Random.Range(0, MorningMusic.Count)];
                    TimeOfDayMusicAudioSource.Play();
                    m_CurrentMusicClipLength = TimeOfDayMusicAudioSource.clip.length;
                }
            }
            else if (CurrentTimeOfDay == CurrentTimeOfDayEnum.Day)
            {
                //Day Music
                if (DayMusic.Count != 0)
                {
                    TimeOfDayMusicAudioSource.clip = DayMusic[Random.Range(0, DayMusic.Count)];
                    TimeOfDayMusicAudioSource.Play();
                    m_CurrentMusicClipLength = TimeOfDayMusicAudioSource.clip.length;
                }
            }
            else if (CurrentTimeOfDay == CurrentTimeOfDayEnum.Evening)
            {
                //Evening Music
                if (EveningMusic.Count != 0)
                {
                    TimeOfDayMusicAudioSource.clip = EveningMusic[Random.Range(0, EveningMusic.Count)];
                    TimeOfDayMusicAudioSource.Play();
                    m_CurrentMusicClipLength = TimeOfDayMusicAudioSource.clip.length;
                }
            }
            else if (CurrentTimeOfDay == CurrentTimeOfDayEnum.Night)
            {
                //Night Music
                if (NightMusic.Count != 0)
                {
                    TimeOfDayMusicAudioSource.clip = NightMusic[Random.Range(0, NightMusic.Count)];
                    TimeOfDayMusicAudioSource.Play();
                    m_CurrentMusicClipLength = TimeOfDayMusicAudioSource.clip.length;
                }
            }

            m_TimeOfDayMusicTimer = 0;
        }
    }

    //Check our generated weather to see if it's time to update the weather.
    //If it is, slowly transition the weather according to the current weather type scriptable object
    void CheckWeather()
    {
        if (m_WeatherGenerated)
        {
            if (Hour == HourToChangeWeather || WeatherGenerationMethod == WeatherGenerationMethodEnum.Hourly)
            {
                if (CurrentWeatherType != NextWeatherType)
                {
                    if (WeatherGenerationMethod == WeatherGenerationMethodEnum.Daily)
                    {
                        CurrentWeatherType = NextWeatherType;
                    }

                    TransitionWeather();
                }

                if (WeatherGenerationMethod == WeatherGenerationMethodEnum.Daily)
                {
                    GenerateWeather();
                }
            }
        }
    }

    /// <summary>
    /// Changes UniStorm's weather according to the Weather parameter.
    /// </summary>
    public void ChangeWeather (WeatherType Weather)
    {
        CurrentWeatherType = Weather;
        TransitionWeather();
    }

    void TransitionWeather()
    {
        OnWeatherChangeEvent.Invoke(); //Invoke our weather change event
        if (CloudCoroutine != null) { StopCoroutine(CloudCoroutine); }
        if (FogCoroutine != null) { StopCoroutine(FogCoroutine); }
        if (WeatherEffectCoroutine != null) { StopCoroutine(WeatherEffectCoroutine); }
        if (AdditionalWeatherEffectCoroutine != null) { StopCoroutine(AdditionalWeatherEffectCoroutine); }
        if (ParticleFadeCoroutine != null) { StopCoroutine(ParticleFadeCoroutine); }
        if (AdditionalParticleFadeCoroutine != null) { StopCoroutine(AdditionalParticleFadeCoroutine); }
        if (SunCoroutine != null) { StopCoroutine(SunCoroutine); }
        if (MoonCoroutine != null) { StopCoroutine(MoonCoroutine); }
        if (WindCoroutine != null) { StopCoroutine(WindCoroutine); }
        if (SoundInCoroutine != null) { StopCoroutine(SoundInCoroutine); }
        if (SoundOutCoroutine != null) { StopCoroutine(SoundOutCoroutine); }
        if (LightningCloudsCoroutine != null) { StopCoroutine(LightningCloudsCoroutine); }
        if (ColorCoroutine != null) { StopCoroutine(ColorCoroutine); }
        if (CloudHeightCoroutine != null) { StopCoroutine(CloudHeightCoroutine); }
        if (RainShaderCoroutine != null) { StopCoroutine(RainShaderCoroutine); }
        if (SnowShaderCoroutine != null) { StopCoroutine(SnowShaderCoroutine); }

        //Reset our time of day sounds timer so it doesn't play right after a weather change
        m_TimeOfDaySoundsTimer = 0;

        //Get randomized cloud amount based on cloud level from weather type.
        if (CurrentWeatherType.CloudLevel != WeatherType.CloudLevelEnum.DontChange)
        {
            m_ReceivedCloudValue = GetCloudLevel(m_ReceivedCloudValue);
        }

        //Clouds
        if (m_CloudDomeMaterial.GetFloat("_CloudCover") < m_ReceivedCloudValue)
        {
            CloudCoroutine = StartCoroutine(CloudFadeSequence(TransitionSpeed, m_ReceivedCloudValue));
        }
        else
        {
            CloudCoroutine = StartCoroutine(CloudFadeSequence(-TransitionSpeed, m_ReceivedCloudValue));
        }
       
        //Wind
        if (CurrentWindIntensity < CurrentWeatherType.WindIntensity)
        {
            WindCoroutine = StartCoroutine(WindFadeSequence(TransitionSpeed / 8, 0, CurrentWeatherType.WindIntensity));
        }
        else
        {
            WindCoroutine = StartCoroutine(WindFadeSequence(-TransitionSpeed / 8, CurrentWeatherType.WindIntensity, 1));
        }

        //Fog
        if (RenderSettings.fogDensity < CurrentWeatherType.FogDensity)
        {
            FogCoroutine = StartCoroutine(FogFadeSequence(TransitionSpeed * 350 / CurrentWeatherType.FogSpeedMultiplier, CurrentWeatherType.FogDensity, CurrentWeatherType.FogDensity));
        }
        else
        {
            FogCoroutine = StartCoroutine(FogFadeSequence(-TransitionSpeed * 10, CurrentWeatherType.FogDensity, CurrentWeatherType.FogDensity));
        }


        if (CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.Yes)
        {
            SunCoroutine = StartCoroutine(SunFadeSequence(TransitionSpeed, CurrentWeatherType.SunIntensity));
            MoonCoroutine = StartCoroutine(MoonFadeSequence(TransitionSpeed, CurrentWeatherType.MoonIntensity));
            ColorCoroutine = StartCoroutine(ColorFadeSequence(TransitionSpeed * 20, 0, 1));
            CloudHeightCoroutine = StartCoroutine(CloudHeightFadeSequence(TransitionSpeed / 2, 0, 2500));

            if (CurrentWeatherType.ShaderControl == WeatherType.ShaderControlEnum.Rain)
            {
                RainShaderCoroutine = StartCoroutine(RainShaderFadeInSequence(TransitionSpeed / 2, 1));
                SnowShaderCoroutine = StartCoroutine(SnowShaderFadeOutSequence(-TransitionSpeed, 0));
            }
            else if (CurrentWeatherType.ShaderControl == WeatherType.ShaderControlEnum.Snow)
            {
                SnowShaderCoroutine = StartCoroutine(SnowShaderFadeInSequence(TransitionSpeed * 2, 1f));
                RainShaderCoroutine = StartCoroutine(RainShaderFadeOutSequence(-TransitionSpeed / 2, 0));
            }
            else if (CurrentWeatherType.ShaderControl == WeatherType.ShaderControlEnum.None)
            {
                SnowShaderCoroutine = StartCoroutine(SnowShaderFadeOutSequence(-TransitionSpeed, 0));
                RainShaderCoroutine = StartCoroutine(RainShaderFadeOutSequence(-TransitionSpeed / 2, 0));
            }
        }
        else if (CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.No)
        {
            SunCoroutine = StartCoroutine(SunFadeSequence(TransitionSpeed/1.75f, CurrentWeatherType.SunIntensity));
            MoonCoroutine = StartCoroutine(MoonFadeSequence(TransitionSpeed, CurrentWeatherType.MoonIntensity));
            ColorCoroutine = StartCoroutine(ColorFadeSequence(TransitionSpeed * 100, 0, 1));

            if (CurrentWeatherType.ShaderControl == WeatherType.ShaderControlEnum.None)
            {
                SnowShaderCoroutine = StartCoroutine(SnowShaderFadeOutSequence(-TransitionSpeed, 0));
                RainShaderCoroutine = StartCoroutine(RainShaderFadeOutSequence(-TransitionSpeed / 2, 0));
            }

            if (CurrentWeatherType.CloudLevel != WeatherType.CloudLevelEnum.Cloudy && CurrentWeatherType.CloudLevel != WeatherType.CloudLevelEnum.MostlyCloudy 
                && CurrentWeatherType.CloudLevel != WeatherType.CloudLevelEnum.DontChange)
            {
                if (m_CloudDomeMaterial.GetFloat("_WorldPosGlobal") > 500)
                {
                    CloudHeightCoroutine = StartCoroutine(CloudHeightFadeSequence(-TransitionSpeed / 3.5f, 0, 2500));
                }
                else if (m_CloudDomeMaterial.GetFloat("_WorldPosGlobal") <= 500)
                {
                    CloudHeightCoroutine = StartCoroutine(CloudHeightFadeSequence(-TransitionSpeed * 2, 0, 2500));
                }
            }
            else
            {
                if (m_CloudDomeMaterial.GetFloat("_WorldPosGlobal") > 500)
                {
                    CloudHeightCoroutine = StartCoroutine(CloudHeightFadeSequence(-TransitionSpeed / 3.5f, 500, 500));
                }
                else if (m_CloudDomeMaterial.GetFloat("_WorldPosGlobal") < 500)
                {
                    CloudHeightCoroutine = StartCoroutine(CloudHeightFadeSequence(TransitionSpeed / 2, 0, 500));
                }
            }
        }

        if (CurrentWeatherType.UseLightning == WeatherType.Yes_No.Yes && CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.Yes)
        {
            LightningCloudsCoroutine = StartCoroutine(LightningCloudsFadeSequence(TransitionSpeed, 0, 0.5f));
        }
        else if (CurrentWeatherType.UseLightning == WeatherType.Yes_No.No)
        {
            LightningCloudsCoroutine = StartCoroutine(LightningCloudsFadeSequence(-TransitionSpeed / 8, 0, 0.5f));
        }

        if (CurrentWeatherType.UseWeatherEffect == WeatherType.Yes_No.Yes)
        {
            for (int i = 0; i < WeatherEffectsList.Count; i++)
            {
                if (WeatherEffectsList[i].name == CurrentWeatherType.WeatherEffect.name + " (UniStorm)")
                {
                    CurrentParticleSystem = WeatherEffectsList[i];
                    CurrentParticleSystem.transform.localPosition = CurrentWeatherType.ParticleEffectVector;
                }
            }

            if (CurrentParticleSystem.emission.rateOverTime.constant < CurrentWeatherType.ParticleEffectAmount)
            {
                WeatherEffectCoroutine = StartCoroutine(ParticleFadeSequence(TransitionSpeed, 0, CurrentWeatherType.ParticleEffectAmount));
            }
            else
            {
                ParticleFadeCoroutine = StartCoroutine(ParticleFadeOutSequence(-TransitionSpeed / 6, CurrentWeatherType.ParticleEffectAmount, CurrentParticleSystem));
            }
        }

        if (CurrentWeatherType.UseAdditionalWeatherEffect == WeatherType.Yes_No.Yes)
        {
            for (int i = 0; i < AdditionalWeatherEffectsList.Count; i++)
            {
                if (AdditionalWeatherEffectsList[i].name == CurrentWeatherType.AdditionalWeatherEffect.name + " (UniStorm)")
                {
                    AdditionalCurrentParticleSystem = AdditionalWeatherEffectsList[i];
                    AdditionalCurrentParticleSystem.transform.localPosition = CurrentWeatherType.AdditionalParticleEffectVector;
                }
            }

            if (AdditionalCurrentParticleSystem.emission.rateOverTime.constant < CurrentWeatherType.AdditionalParticleEffectAmount)
            {
                AdditionalWeatherEffectCoroutine = StartCoroutine(AdditionalParticleFadeSequence(TransitionSpeed * 10, 0, CurrentWeatherType.AdditionalParticleEffectAmount));
            }
            else
            {
                AdditionalParticleFadeCoroutine = StartCoroutine(AdditionalParticleFadeOutSequence(-TransitionSpeed * 2.5f, CurrentWeatherType.AdditionalParticleEffectAmount, AdditionalCurrentParticleSystem));
            }
        }

        if (CurrentWeatherType.UseWeatherSound == WeatherType.Yes_No.Yes)
        {
            foreach (AudioSource A in WeatherSoundsList)
            {
                if (A.gameObject.name == CurrentWeatherType.WeatherTypeName + " (UniStorm)")
                {
                    A.Play();
                    SoundInCoroutine = StartCoroutine(SoundFadeInSequence(TransitionSpeed / 4, CurrentWeatherType.WeatherVolume, A));
                }
            }
        }

        if (CurrentWeatherType.UseWeatherEffect == WeatherType.Yes_No.No)
        {
            CurrentParticleSystem = null;

            if (CurrentWeatherType.UseAdditionalWeatherEffect == WeatherType.Yes_No.No)
            {
                AdditionalCurrentParticleSystem = null;
            }
        }

        foreach (ParticleSystem P in WeatherEffectsList)
        {
            if (P != CurrentParticleSystem && P.emission.rateOverTime.constant > 0 ||
                CurrentWeatherType.UseWeatherEffect == WeatherType.Yes_No.No && P.emission.rateOverTime.constant > 0)
            {
                ParticleFadeCoroutine = StartCoroutine(ParticleFadeOutSequence(-TransitionSpeed / 6, 0, P));
            }
        }

        foreach (ParticleSystem P in AdditionalWeatherEffectsList)
        {
            if (P != AdditionalCurrentParticleSystem && P.emission.rateOverTime.constant > 0 ||
                CurrentWeatherType.UseAdditionalWeatherEffect == WeatherType.Yes_No.No && P.emission.rateOverTime.constant > 0)
            {
                AdditionalParticleFadeCoroutine = StartCoroutine(AdditionalParticleFadeOutSequence(-TransitionSpeed * 2.5f, 0, P));
            }
        }

        foreach (AudioSource A in WeatherSoundsList)
        {
            if (A.gameObject.name != CurrentWeatherType.WeatherTypeName + " (UniStorm)" && A.volume > 0 || CurrentWeatherType.UseWeatherSound == WeatherType.Yes_No.No && A.volume > 0)
            {
                SoundOutCoroutine = StartCoroutine(SoundFadeOutSequence(-TransitionSpeed / 8, 0, A));
            }
        }
    }

    //Calculates our moon phases. This is updated daily at exactly 12:00.
    void CalculateMoonPhase()
    {
        if (MoonPhaseList.Count > 0)
        {
            if (MoonPhaseIndex == MoonPhaseList.Count) {
                MoonPhaseIndex = 0;
            }
            m_MoonPhaseMaterial.SetTexture("_MainTex", MoonPhaseList[MoonPhaseIndex].MoonPhaseTexture);
            m_MoonRenderer.material = m_MoonPhaseMaterial;
            m_MoonPhaseMaterial.SetFloat("_MoonBrightness", MoonBrightness);
            MoonPhaseIntensity = MoonPhaseList[MoonPhaseIndex].MoonPhaseIntensity;
            m_MoonPhaseMaterial.SetColor("_MoonColor", MoonPhaseColor);
        }
    }

    //Continuously update our colors based on the time of day
    void UpdateColors()
    {
        m_SunLight.color = SunColor.Evaluate(m_TimeFloat);
        m_MoonLight.color = MoonColor.Evaluate(m_TimeFloat);
        m_StarsMaterial.color = StarLightColor.Evaluate(m_TimeFloat);
        m_SkyBoxMaterial.SetColor("_SkyTint", SkyColor.Evaluate(m_TimeFloat));
        m_SkyBoxMaterial.SetFloat("_AtmosphereThickness", AtmosphereThickness.Evaluate(m_TimeFloat*24));
        m_SkyBoxMaterial.SetColor("_NightSkyTint", SkyTintColor.Evaluate(m_TimeFloat));
        m_CloudDomeMaterial.SetColor("_LightColor", CloudLightColor.Evaluate(m_TimeFloat));
        m_CloudDomeMaterial.SetColor("_BaseColor", CloudBaseColor.Evaluate(m_TimeFloat));
        m_CloudDomeMaterial.SetFloat("_Attenuation", m_SunLight.intensity + m_MoonLight.intensity);
        RenderSettings.ambientSkyColor = AmbientSkyLightColor.Evaluate(m_TimeFloat);
        RenderSettings.ambientEquatorColor = AmbientEquatorLightColor.Evaluate(m_TimeFloat);
        RenderSettings.ambientGroundColor = AmbientGroundLightColor.Evaluate(m_TimeFloat);
        RenderSettings.fogColor = FogColor.Evaluate(m_TimeFloat);
        CurrentFogColor = FogColor.Evaluate(m_TimeFloat);
        m_SunLight.intensity = SunIntensityCurve.Evaluate(m_TimeFloat * 24) * SunIntensity;
        m_SkyBoxMaterial.SetFloat("_SunSize", SunSize.Evaluate(m_TimeFloat * 24) * 0.01f);
        m_MoonLight.intensity = MoonIntensityCurve.Evaluate(m_TimeFloat * 24) * MoonIntensity * MoonPhaseIntensity;
        m_MoonTransform.localScale = MoonSize.Evaluate(m_TimeFloat * 24) * m_MoonStartingSize;
    }

    //Calculates our days and updates our Animation curves.
    void CalculateDays()
    {
        CalculatePrecipiation();
        TemperatureCurve.Evaluate(m_PreciseCurveTime);

        Day++; //Add a day to our Day variable
        CalculateMonths(); //Calculate our months
        CalculateSeason(); //Calculate our seasons
        OnDayChangeEvent.Invoke(); //Invoke our day events
        GetDate(); //Calculate the DateTime

        //Clears our hourly forecast and generates a new one for the current day
        if (WeatherGenerationMethod == UniStormSystem.WeatherGenerationMethodEnum.Hourly)
        {
            WeatherForecast.Clear();
            GenerateWeather();
        }
    }

    //Calculates our months for an accurate calendar that also calculates leap year
    void CalculateMonths()
    {
        //Calculates our days into months
        if (Day >= 32 && Month == 1 || Day >= 32 && Month == 3 || Day >= 32 && Month == 5 || Day >= 32 && Month == 7
            || Day >= 32 && Month == 8 || Day >= 32 && Month == 10 || Day >= 32 && Month == 12) {
            Day = Day % 32;
            Day += 1;
            Month += 1;
            OnMonthChangeEvent.Invoke(); //Invoke our Month events
        }

        if (Day >= 31 && Month == 4 || Day >= 31 && Month == 6 || Day >= 31 && Month == 9 || Day >= 31 && Month == 11) {
            Day = Day % 31;
            Day += 1;
            Month += 1;
            OnMonthChangeEvent.Invoke(); //Invoke our Month events
        }

        //Calculates Leap Year
        if (Day >= 30 && Month == 2 && (Year % 4 == 0 && Year % 100 != 0) || (Year % 400 == 0)) {
            Day = Day % 30;
            Day += 1;
            Month += 1;
            OnMonthChangeEvent.Invoke(); //Invoke our Month events
        }

        else if (Day >= 29 && Month == 2 && Year % 4 != 0) {
            Day = Day % 29;
            Day += 1;
            Month += 1;
            OnMonthChangeEvent.Invoke(); //Invoke our Month events
        }

        //Calculates our months into years
        if (Month > 12) {
            Month = Month % 13;
            Year += 1;
            Month += 1;
            OnYearChangeEvent.Invoke(); //Invoke our Year events

            //Reset our m_roundingCorrection variable to 0
            m_roundingCorrection = 0;
        }
    }

    //Generate our weather according to the precipitation odds for the current time of year.
    //Check the weather type's conditions, if they are not met, reroll weather within the same category.
    public void GenerateWeather()
    {
        if (WeatherGenerationMethod == WeatherGenerationMethodEnum.Daily)
        {
            m_GeneratedOdds = UnityEngine.Random.Range(1, 101);
            HourToChangeWeather = UnityEngine.Random.Range(0, 23);

            if (HourToChangeWeather == Hour)
            {
                HourToChangeWeather = Hour - 1;
            }

            CheckGeneratedWeather();
        }
        else if (WeatherGenerationMethod == WeatherGenerationMethodEnum.Hourly)
        {
            for (int i = 0; i < 24; i++)
            {
                m_GeneratedOdds = UnityEngine.Random.Range(1, 101);
                CheckGeneratedWeather();
            }
        }
    }

    //Check our generated weather for seasonal and temperature conditions. 
    //Reroll the weather if they are not met until an appropriate weather type in the same category is found.
    void CheckGeneratedWeather ()
    {
        if (m_GeneratedOdds <= m_PrecipitationOdds && PrecipiationWeatherTypes.Count != 0)
        {
            TempWeatherType = PrecipiationWeatherTypes[Random.Range(0, PrecipiationWeatherTypes.Count)];
        }
        else if (m_GeneratedOdds > m_PrecipitationOdds && NonPrecipiationWeatherTypes.Count != 0)
        {
            TempWeatherType = NonPrecipiationWeatherTypes[Random.Range(0, NonPrecipiationWeatherTypes.Count)];
        }

        while (TempWeatherType.TemperatureType == WeatherType.TemperatureTypeEnum.AboveFreezing && Temperature <= m_FreezingTemperature
            || TempWeatherType.Season != WeatherType.SeasonEnum.All && (int)TempWeatherType.Season != (int)CurrentSeason
        || TempWeatherType.TemperatureType == WeatherType.TemperatureTypeEnum.BelowFreezing && Temperature > m_FreezingTemperature
        || TempWeatherType.SpecialWeatherType == WeatherType.Yes_No.Yes)
        {
            if (TempWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.No)
            {
                TempWeatherType = NonPrecipiationWeatherTypes[Random.Range(0, NonPrecipiationWeatherTypes.Count)];
            }
            else if (TempWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.Yes)
            {
                TempWeatherType = PrecipiationWeatherTypes[Random.Range(0, PrecipiationWeatherTypes.Count)];
            }
            else
            {
                break;
            }
        }

        if(WeatherGenerationMethod == WeatherGenerationMethodEnum.Daily)
        {
            NextWeatherType = TempWeatherType;
        }
        else if (WeatherGenerationMethod == WeatherGenerationMethodEnum.Hourly)
        {
            WeatherForecast.Add(TempWeatherType);
        }
        m_WeatherGenerated = true;
    }

    IEnumerator LightningCloudsFadeSequence(float TransitionTime, float MinValue, float MaxValue)
    {
        bool FadingOut = (TransitionTime < 0.0f);
        float m_LocalTransitionSpeed = 1.0f / TransitionTime;
        float CurrentValue = m_LightningFlashMaterial.color.a;

        while ((CurrentValue >= 0f && FadingOut) || (CurrentValue <= 0.5f && !FadingOut))
        {
            CurrentValue += Time.deltaTime * m_LocalTransitionSpeed;
            Color C = m_LightningFlashMaterial.color;
            C.a = CurrentValue;
            m_LightningFlashMaterial.color = C;

            yield return null;
        }
    }

    IEnumerator ColorFadeSequence(float TransitionTime, float MinValue, float MaxValue)
    {
        yield return new WaitUntil(() => m_CloudDomeMaterial.GetFloat("_CloudCover") > 0.9f);

        bool FadingOut = (TransitionTime < 0.0f);
        float m_LocalTransitionSpeed = 1.0f / TransitionTime;
        float CurrentValue = 0;

        while ((CurrentValue >= 0f && FadingOut) || (CurrentValue <= 0.01f && !FadingOut))
        {
            CurrentValue += Time.deltaTime * m_LocalTransitionSpeed;

            if (CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.Yes)
            {
                for (int i = 0; i < CloudBaseColor.colorKeys.Length; i++)
                {
                    CloudColorKeySwitcher[i].color = Color.Lerp(CloudColorKeySwitcher[i].color, CloudStormyBaseColor.colorKeys[i].color, CurrentValue);
                }

                for (int i = 0; i < FogColor.colorKeys.Length; i++)
                {
                    FogColorKeySwitcher[i].color = Color.Lerp(FogColorKeySwitcher[i].color, FogStormyColor.colorKeys[i].color, CurrentValue);
                }

                for (int i = 0; i < AmbientSkyLightColor.colorKeys.Length; i++)
                {
                    AmbientSkyLightColorKeySwitcher[i].color = Color.Lerp(AmbientSkyLightColorKeySwitcher[i].color, StormyAmbientSkyLightColor.colorKeys[i].color, CurrentValue);
                }

                for (int i = 0; i < AmbientEquatorLightColor.colorKeys.Length; i++)
                {
                    AmbientEquatorLightColorKeySwitcher[i].color = Color.Lerp(AmbientEquatorLightColorKeySwitcher[i].color, StormyAmbientEquatorLightColor.colorKeys[i].color, CurrentValue);
                }

                for (int i = 0; i < AmbientGroundLightColor.colorKeys.Length; i++)
                {
                    AmbientGroundLightColorKeySwitcher[i].color = Color.Lerp(AmbientGroundLightColorKeySwitcher[i].color, StormyAmbientGroundLightColor.colorKeys[i].color, CurrentValue);
                }

                for (int i = 0; i < SunColor.colorKeys.Length; i++)
                {
                    SunLightColorKeySwitcher[i].color = Color.Lerp(SunLightColorKeySwitcher[i].color, StormySunColor.colorKeys[i].color, CurrentValue);
                }

                FogColor.SetKeys(FogColorKeySwitcher, FogColor.alphaKeys);
                CloudBaseColor.SetKeys(CloudColorKeySwitcher, CloudBaseColor.alphaKeys);
                AmbientSkyLightColor.SetKeys(AmbientSkyLightColorKeySwitcher, AmbientSkyLightColor.alphaKeys);
                AmbientEquatorLightColor.SetKeys(AmbientEquatorLightColorKeySwitcher, AmbientEquatorLightColor.alphaKeys);
                AmbientGroundLightColor.SetKeys(AmbientGroundLightColorKeySwitcher, AmbientGroundLightColor.alphaKeys);
                SunColor.SetKeys(SunLightColorKeySwitcher, SunColor.alphaKeys);
            }
            else if (CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.No)
            {
                for (int i = 0; i < CloudBaseColor.colorKeys.Length; i++)
                {
                    CloudColorKeySwitcher[i].color = Color.Lerp(CloudColorKeySwitcher[i].color, DefaultCloudBaseColor.colorKeys[i].color, CurrentValue);
                }

                for (int i = 0; i < FogColor.colorKeys.Length; i++)
                {
                    FogColorKeySwitcher[i].color = Color.Lerp(FogColorKeySwitcher[i].color, DefaultFogBaseColor.colorKeys[i].color, CurrentValue);
                }

                for (int i = 0; i < AmbientSkyLightColor.colorKeys.Length; i++)
                {
                    AmbientSkyLightColorKeySwitcher[i].color = Color.Lerp(AmbientSkyLightColorKeySwitcher[i].color, DefaultAmbientSkyLightBaseColor.colorKeys[i].color, CurrentValue);
                }

                for (int i = 0; i < AmbientEquatorLightColor.colorKeys.Length; i++)
                {
                    AmbientEquatorLightColorKeySwitcher[i].color = Color.Lerp(AmbientEquatorLightColorKeySwitcher[i].color, DefaultAmbientEquatorLightBaseColor.colorKeys[i].color, CurrentValue);
                }

                for (int i = 0; i < AmbientGroundLightColor.colorKeys.Length; i++)
                {
                    AmbientGroundLightColorKeySwitcher[i].color = Color.Lerp(AmbientGroundLightColorKeySwitcher[i].color, DefaultAmbientGroundLightBaseColor.colorKeys[i].color, CurrentValue);
                }

                for (int i = 0; i < SunColor.colorKeys.Length; i++)
                {
                    SunLightColorKeySwitcher[i].color = Color.Lerp(SunLightColorKeySwitcher[i].color, DefaultSunLightBaseColor.colorKeys[i].color, CurrentValue);
                }

                FogColor.SetKeys(FogColorKeySwitcher, FogColor.alphaKeys);
                CloudBaseColor.SetKeys(CloudColorKeySwitcher, CloudBaseColor.alphaKeys);
                AmbientSkyLightColor.SetKeys(AmbientSkyLightColorKeySwitcher, AmbientSkyLightColor.alphaKeys);
                AmbientEquatorLightColor.SetKeys(AmbientEquatorLightColorKeySwitcher, AmbientEquatorLightColor.alphaKeys);
                AmbientGroundLightColor.SetKeys(AmbientGroundLightColorKeySwitcher, AmbientGroundLightColor.alphaKeys);
                SunColor.SetKeys(SunLightColorKeySwitcher, SunColor.alphaKeys);
            }

            yield return null;
        }
    }

    IEnumerator CloudFadeSequence (float TransitionTime, float MaxValue)
	{
		bool FadingOut = (TransitionTime < 0.0f);
		float m_LocalTransitionSpeed = 1.0f / TransitionTime; 
		float CurrentValue = m_CloudDomeMaterial.GetFloat("_CloudCover");

		while ((CurrentValue >= MaxValue && FadingOut) || (CurrentValue <= MaxValue && !FadingOut)) 
		{
			CurrentValue += Time.deltaTime * m_LocalTransitionSpeed;
			m_CloudDomeMaterial.SetFloat("_CloudCover", CurrentValue);

			yield return null;
		}
	}

	IEnumerator FogFadeSequence (float TransitionTime, float MinValue, float MaxValue)
	{
        if (CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.Yes)
        {
            yield return new WaitUntil(() => m_CloudDomeMaterial.GetFloat("_CloudCover") > 0.9f);
        }

        bool FadingOut = (TransitionTime < 0.0f);
		float m_LocalTransitionSpeed = 1.0f / TransitionTime; 
		float CurrentValue = RenderSettings.fogDensity;
        CurrentFogAmount = RenderSettings.fogDensity;

        //AQUAS support comming soon
        /*
        #if AQUAS_PRESENT
        if (m_AquasPresent && m_Aquas.underWater)
        {
            CurrentValue = m_AQUAS_CurrentFogValue;
            CurrentFogAmount = m_AQUAS_CurrentFogValue;
        }
        #endif
        */

        while ((CurrentValue >= MinValue && FadingOut) || (CurrentValue <= MaxValue && !FadingOut)) 
		{
			CurrentValue += Time.deltaTime * m_LocalTransitionSpeed;

            RenderSettings.fogDensity = CurrentValue;
            CurrentFogAmount = RenderSettings.fogDensity;
            //AQUAS support comming soon
            /*
            #if AQUAS_PRESENT
            if (m_AquasPresent)
            {
                if (!m_Aquas.underWater)
                {
                    RenderSettings.fogDensity = CurrentValue;
                    CurrentFogAmount = RenderSettings.fogDensity;
                }
            }
            else
            RenderSettings.fogDensity = CurrentValue;
            CurrentFogAmount = RenderSettings.fogDensity;
            #else
            RenderSettings.fogDensity = CurrentValue;
            CurrentFogAmount = RenderSettings.fogDensity;
            #endif
            */

            yield return null;
		}

        CurrentFogAmount = CurrentValue;
    }

	IEnumerator WindFadeSequence (float TransitionTime, float MinValue, float MaxValue)
	{
		bool FadingOut = (TransitionTime < 0.0f);
		float m_LocalTransitionSpeed = 1.0f / TransitionTime; 
		float CurrentValue = UniStormWindZone.windMain;
        CurrentWindIntensity = CurrentValue;


        while ((CurrentValue >= MinValue && FadingOut) || (CurrentValue <= MaxValue && !FadingOut)) 
		{
			CurrentValue += Time.deltaTime * m_LocalTransitionSpeed;
			UniStormWindZone.windMain = CurrentValue;
            CurrentWindIntensity = CurrentValue;

            yield return null;
		}
	}

	IEnumerator SunFadeSequence (float TransitionTime, float MaxValue)
	{
        if (CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.Yes)
        {
            yield return new WaitUntil(() => m_CloudDomeMaterial.GetFloat("_CloudCover") > 0.8f);
            TransitionTime = TransitionTime / 3;
        }

        if (SunIntensity > CurrentWeatherType.SunIntensity)
        {
            TransitionTime = TransitionTime * -1;
        }
        
        if (CurrentWeatherType.CloudLevel == WeatherType.CloudLevelEnum.DontChange)
        {
            yield break;
        }

        bool FadingOut = (TransitionTime < 0.0f);
		float m_LocalTransitionSpeed = 1.0f / TransitionTime; 
		float CurrentValue = SunIntensity;

		while ((CurrentValue >= MaxValue && FadingOut) || (CurrentValue <= MaxValue && !FadingOut)) 
		{
			CurrentValue += Time.deltaTime * m_LocalTransitionSpeed;
			SunIntensity = CurrentValue;

			yield return null;
		}
	}

    IEnumerator MoonFadeSequence(float TransitionTime, float MaxValue)
    {
        if (CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.Yes)
        {
            yield return new WaitUntil(() => m_CloudDomeMaterial.GetFloat("_CloudCover") > 0.8f);
        }

        if (MoonIntensity > CurrentWeatherType.MoonIntensity)
        {
            TransitionTime = TransitionTime * -1;
        }

        if (CurrentWeatherType.CloudLevel == WeatherType.CloudLevelEnum.DontChange)
        {
            yield break;
        }

        bool FadingOut = (TransitionTime < 0.0f);
        float m_LocalTransitionSpeed = 1.0f / TransitionTime;
        float CurrentValue = MoonIntensity;

        while ((CurrentValue >= MaxValue && FadingOut) || (CurrentValue <= MaxValue && !FadingOut))
        {
            CurrentValue += Time.deltaTime * m_LocalTransitionSpeed;
            MoonIntensity = CurrentValue;

            yield return null;
        }
    }

    IEnumerator ParticleFadeSequence (float TransitionTime, float MinValue, float MaxValue)
	{
        if (CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.Yes)
        {
            yield return new WaitUntil(() => m_CloudDomeMaterial.GetFloat("_CloudCover") > 0.9f);
        }

		bool FadingOut = (TransitionTime < 0.0f);
		float m_LocalTransitionSpeed = 1.0f / TransitionTime; 
		float CurrentValue = CurrentParticleSystem.emission.rateOverTime.constant;

		while ((CurrentValue >= MinValue && FadingOut) || (CurrentValue <= MaxValue && !FadingOut)) 
		{
			CurrentValue += Time.deltaTime * m_LocalTransitionSpeed * 2000;
			ParticleSystem.EmissionModule CurrentEmission = CurrentParticleSystem.emission;
			CurrentEmission.rateOverTime = new ParticleSystem.MinMaxCurve(CurrentValue);

			yield return null;
		}
	}

	IEnumerator ParticleFadeOutSequence (float TransitionTime, float MinValue, ParticleSystem EffectToFade)
	{
		bool FadingOut = (TransitionTime < 0.0f);
		float m_LocalTransitionSpeed = 1.0f / TransitionTime; 
		float CurrentValue = EffectToFade.emission.rateOverTime.constant;

		while ((CurrentValue >= MinValue && FadingOut)) 
		{
			CurrentValue += Time.deltaTime * m_LocalTransitionSpeed * 2000;
			ParticleSystem.EmissionModule CurrentEmission = EffectToFade.emission;
			CurrentEmission.rateOverTime = new ParticleSystem.MinMaxCurve(CurrentValue);

			yield return null;
		}
	}

    IEnumerator AdditionalParticleFadeSequence(float TransitionTime, float MinValue, float MaxValue)
    {
        if (CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.Yes)
        {
            yield return new WaitUntil(() => m_CloudDomeMaterial.GetFloat("_CloudCover") > 0.9f);
        }

        bool FadingOut = (TransitionTime < 0.0f);
        float m_LocalTransitionSpeed = 1.0f / TransitionTime;
        float CurrentValue = AdditionalCurrentParticleSystem.emission.rateOverTime.constant;

        while ((CurrentValue >= MinValue && FadingOut) || (CurrentValue <= MaxValue && !FadingOut))
        {
            CurrentValue += Time.deltaTime * m_LocalTransitionSpeed * 2000;
            ParticleSystem.EmissionModule CurrentEmission = AdditionalCurrentParticleSystem.emission;
            CurrentEmission.rateOverTime = new ParticleSystem.MinMaxCurve(CurrentValue);

            yield return null;
        }
    }

    IEnumerator AdditionalParticleFadeOutSequence(float TransitionTime, float MinValue, ParticleSystem EffectToFade)
    {
        bool FadingOut = (TransitionTime < 0.0f);
        float m_LocalTransitionSpeed = 1.0f / TransitionTime;
        float CurrentValue = EffectToFade.emission.rateOverTime.constant;

        while ((CurrentValue >= MinValue && FadingOut))
        {
            CurrentValue += Time.deltaTime * m_LocalTransitionSpeed * 2000;
            ParticleSystem.EmissionModule CurrentEmission = EffectToFade.emission;
            CurrentEmission.rateOverTime = new ParticleSystem.MinMaxCurve(CurrentValue);

            yield return null;
        }
    }

    IEnumerator SoundFadeInSequence (float TransitionTime, float MaxValue, AudioSource SourceToFade)
	{
        if (CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.Yes)
        {
            yield return new WaitUntil(() => m_CloudDomeMaterial.GetFloat("_CloudCover") > 0.9f);
        }

		bool FadingOut = (TransitionTime < 0.0f);
		float m_LocalTransitionSpeed = 1.0f / TransitionTime; 
		float CurrentValue = SourceToFade.volume;

		while ((CurrentValue <= MaxValue && !FadingOut)) 
		{
			CurrentValue += Time.deltaTime * m_LocalTransitionSpeed;
			SourceToFade.volume = CurrentValue;

			yield return null;
		}
	}

	IEnumerator SoundFadeOutSequence (float TransitionTime, float MinValue, AudioSource SourceToFade)
	{
		bool FadingOut = (TransitionTime < 0.0f);
		float m_LocalTransitionSpeed = 1.0f / TransitionTime; 
		float CurrentValue = SourceToFade.volume;

		while ((CurrentValue >= MinValue && FadingOut)) 
		{
			CurrentValue += Time.deltaTime * m_LocalTransitionSpeed;
			SourceToFade.volume = CurrentValue;

			yield return null;
		}
	}

    IEnumerator CloudHeightFadeSequence(float TransitionTime, int MinValue, int MaxValue)
    {
        yield return new WaitUntil(() => m_CloudDomeMaterial.GetFloat("_CloudCover") > 0.8f);

        bool FadingOut = (TransitionTime < 0.0f);
        float m_LocalTransitionSpeed = 1.0f / TransitionTime;
        float CurrentValue = m_CloudDomeMaterial.GetFloat("_WorldPosGlobal");

        while ((CurrentValue >= MinValue && FadingOut) || (CurrentValue <= MaxValue && !FadingOut))
        {
            CurrentValue += Time.deltaTime * m_LocalTransitionSpeed * 2000;
            m_CloudDomeMaterial.SetFloat("_WorldPosGlobal", CurrentValue);

            yield return null;
        }
    }

    IEnumerator RainShaderFadeInSequence(float TransitionTime, float MaxValue)
    {
        yield return new WaitUntil(() => m_CloudDomeMaterial.GetFloat("_CloudCover") > 1.0f);
        bool FadingOut = (TransitionTime < 0.0f);
        float m_LocalTransitionSpeed = 1.0f / TransitionTime;
        float CurrentValue = Shader.GetGlobalFloat("_WetnessStrength");

        while ((CurrentValue < MaxValue && !FadingOut))
        {
            CurrentValue += Time.deltaTime * m_LocalTransitionSpeed;
            Shader.SetGlobalFloat("_WetnessStrength", CurrentValue);

            yield return null;
        }
    }

    IEnumerator RainShaderFadeOutSequence(float TransitionTime, float MinValue)
    {
        bool FadingOut = (TransitionTime < 0.0f);
        float m_LocalTransitionSpeed = 1.0f / TransitionTime;
        float CurrentValue = Shader.GetGlobalFloat("_WetnessStrength");

        while ((CurrentValue >= 0 && FadingOut))
        {
            CurrentValue += Time.deltaTime * m_LocalTransitionSpeed;
            Shader.SetGlobalFloat("_WetnessStrength", CurrentValue);

            yield return null;
        }
    }

    IEnumerator SnowShaderFadeInSequence(float TransitionTime, float MaxValue)
    {
        yield return new WaitUntil(() => m_CloudDomeMaterial.GetFloat("_CloudCover") > 1.0f);
        bool FadingOut = (TransitionTime < 0.0f);
        float m_LocalTransitionSpeed = 1.0f / TransitionTime;
        float CurrentValue = Shader.GetGlobalFloat("_SnowStrength");

        while ((CurrentValue < MaxValue && !FadingOut))
        {
            CurrentValue += Time.deltaTime * m_LocalTransitionSpeed;
            Shader.SetGlobalFloat("_SnowStrength", CurrentValue);

            yield return null;
        }
    }

    IEnumerator SnowShaderFadeOutSequence(float TransitionTime, float MinValue)
    {
        bool FadingOut = (TransitionTime < 0.0f);
        float m_LocalTransitionSpeed = 1.0f / TransitionTime;
        float CurrentValue = Shader.GetGlobalFloat("_SnowStrength");

        while ((CurrentValue >= 0 && FadingOut))
        {
            CurrentValue += Time.deltaTime * m_LocalTransitionSpeed;
            Shader.SetGlobalFloat("_SnowStrength", CurrentValue);

            yield return null;
        }
    }

    void OnApplicationQuit()
    {
        //Reset our weather shader when the scene is stopped so the shader values remain unchanged in the editor.
        Shader.SetGlobalFloat("_WetnessStrength", 0);
        Shader.SetGlobalFloat("_SnowStrength", 0);
    }
}