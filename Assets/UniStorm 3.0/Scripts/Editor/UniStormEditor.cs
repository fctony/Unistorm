using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using UnityEditorInternal;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(UniStormSystem))]
[System.Serializable]
public class UniStormEditor : Editor {

    //Audio Mixer
    SerializedProperty TimeOfDaySoundsVolumeProp;
    SerializedProperty TimeOfDayMusicVolumeProp;
    SerializedProperty WeatherSoundsVolumeProp;

    SerializedProperty TabNumberProp;
    SerializedProperty PlayerTransformProp;
    SerializedProperty PlayerCameraProp;
    SerializedProperty CameraFoldoutProp;
    SerializedProperty SettingsFoldoutProp;
    SerializedProperty UniStormProfileProp;

    //Camera & Player
    SerializedProperty UniStormFollowsPlayerProp;
    SerializedProperty GetPlayerAtRuntimeProp;
    SerializedProperty GetPlayerMethodProp;
    SerializedProperty UseRuntimeDelayProp;
    SerializedProperty PlayerTagProp;
    SerializedProperty CameraTagProp;
    SerializedProperty PlayerNameProp;
    SerializedProperty CameraNameProp;
    SerializedProperty UseUniStormMenuProp;
    SerializedProperty UniStormMenuKeyProp;

    //Time
    SerializedProperty HourProp;
	SerializedProperty MinuteProp;
	SerializedProperty TimeFlowProp;
	SerializedProperty RealWorldTimeProp;
    SerializedProperty DayLengthProp;
    SerializedProperty NightLengthProp;
    SerializedProperty TimeOfDaySoundsSecondsMinProp;
    SerializedProperty TimeOfDaySoundsSecondsMaxProp;
    SerializedProperty TimeFoldoutProp;
    SerializedProperty DateFoldoutProp;
    SerializedProperty TimeSoundsFoldoutProp;
    SerializedProperty TimeOfDayMusicDelayProp;
    SerializedProperty TimeMusicFoldoutProp;
    SerializedProperty TimeOfDaySoundsDuringPrecipitationWeatherProp;

    //Date
    SerializedProperty DayProp;
	SerializedProperty MonthProp;
	SerializedProperty YearProp;

	//Weather
	SerializedProperty PrecipitationGraphProp;
	SerializedProperty TemperatureCurveProp;
	SerializedProperty TemperatureFluctuationProp;
	SerializedProperty FogColorProp;
	SerializedProperty FogStormyColorProp;
	SerializedProperty CloudLightColorProp;
	SerializedProperty CloudBaseColorProp;
	SerializedProperty CloudStormyBaseColorProp;
	SerializedProperty TransitionSpeedProp;
	SerializedProperty WeatherFoldoutProp;
	SerializedProperty LightningFoldoutProp;
    SerializedProperty LightningStrikeOddsProp;
    SerializedProperty LightningStrikeEffectProp;
    SerializedProperty LightningStrikeFireProp;
    SerializedProperty LightningSecondsMinProp;
    SerializedProperty LightningSecondsMaxProp;
    SerializedProperty LightningLightIntensityMinProp;
    SerializedProperty LightningLightIntensityMaxProp;
    SerializedProperty LightningGenerationDistanceProp;
    SerializedProperty LightningDetectionDistanceProp;
    SerializedProperty CloudFormationSpeedProp;
    SerializedProperty CloudSpeedProp;
    SerializedProperty WeatherGenerationMethodProp;
    SerializedProperty CurrentWeatherTypeProp;
    SerializedProperty DetectionLayerMaskProp;
    SerializedProperty TemperatureTypeProp;

    //Celestial
    SerializedProperty SunColorProp;
    SerializedProperty StormySunColorProp;
    SerializedProperty MoonColorProp;
	SerializedProperty SkyTintColorProp;
    SerializedProperty SkyColorProp;
    SerializedProperty AmbientSkyLightColorProp;
    SerializedProperty StormyAmbientSkyLightColorProp;
    SerializedProperty AmbientEquatorLightColorProp;
    SerializedProperty StormyAmbientEquatorLightColorProp;
    SerializedProperty AmbientGroundLightColorProp;
    SerializedProperty StormyAmbientGroundLightColorProp;
    SerializedProperty StarLightColorProp;
	SerializedProperty SunRevolutionProp;
	SerializedProperty SunIntensityCurveProp;
	SerializedProperty SunSizeProp;
	SerializedProperty SunFoldoutProp;
	SerializedProperty MoonIntensityCurveProp;
	SerializedProperty MoonSizeProp;
    SerializedProperty MoonPhaseColorProp;
    SerializedProperty MoonFoldoutProp;
	SerializedProperty MoonPhaseIndexProp;
	SerializedProperty MoonBrightnessProp;
	SerializedProperty AtmosphereFoldoutProp;
    SerializedProperty StarSpeedProp;
    SerializedProperty SunAngleProp;
    SerializedProperty MoonAngleProp;
    SerializedProperty HemisphereProp;
    SerializedProperty AtmosphereThicknessProp;


    Texture TimeIcon;
	Texture WeatherIcon;
	Texture CelestialIcon;
    Texture CameraIcon;
    Texture HelpIcon;
    Texture SettingsIcon;

    //Reorderable lists
    ReorderableList AllWeatherTypesList;
    ReorderableList MoonPhaseList;
    ReorderableList LightningFlashPatternsList;
	ReorderableList ThunderSoundsList;
    ReorderableList MorningSoundsList;
    ReorderableList DaySoundsList;
    ReorderableList EveningSoundsList;
    ReorderableList NightSoundsList;
    ReorderableList MorningMusicList;
    ReorderableList DayMusicList;
    ReorderableList EveningMusicList;
    ReorderableList NightMusicList;
    ReorderableList LightningFireTagsList;

    public float secs = 2f;
    public float startVal = 0f;
    public float progress = 0f;
    public bool Importing;

    void OnDisable() { EditorApplication.update -= Update; }

    void UpdateColorProperties ()
    {
        SunColorProp = serializedObject.FindProperty("SunColor");
        StormySunColorProp = serializedObject.FindProperty("StormySunColor");
        MoonColorProp = serializedObject.FindProperty("MoonColor");
        SkyTintColorProp = serializedObject.FindProperty("SkyTintColor");
        SkyColorProp = serializedObject.FindProperty("SkyColor");
        AmbientSkyLightColorProp = serializedObject.FindProperty("AmbientSkyLightColor");
        StormyAmbientSkyLightColorProp = serializedObject.FindProperty("StormyAmbientSkyLightColor");
        AmbientEquatorLightColorProp = serializedObject.FindProperty("AmbientEquatorLightColor");
        StormyAmbientEquatorLightColorProp = serializedObject.FindProperty("StormyAmbientEquatorLightColor");
        AmbientGroundLightColorProp = serializedObject.FindProperty("AmbientGroundLightColor");
        StormyAmbientGroundLightColorProp = serializedObject.FindProperty("StormyAmbientGroundLightColor");
        StarLightColorProp = serializedObject.FindProperty("StarLightColor");
        FogColorProp = serializedObject.FindProperty("FogColor");
        FogStormyColorProp = serializedObject.FindProperty("FogStormyColor");
        CloudLightColorProp = serializedObject.FindProperty("CloudLightColor");
        CloudBaseColorProp = serializedObject.FindProperty("CloudBaseColor");
        CloudStormyBaseColorProp = serializedObject.FindProperty("CloudStormyBaseColor");
    }

    void OnEnable () {

        EditorApplication.update += Update;
        UniStormSystem self = (UniStormSystem)target;

        //Audio Mixer
        TimeOfDaySoundsVolumeProp = serializedObject.FindProperty("AmbienceVolume");
        TimeOfDayMusicVolumeProp = serializedObject.FindProperty("MusicVolume");
        WeatherSoundsVolumeProp = serializedObject.FindProperty("WeatherSoundsVolume");

        //Editor
        TabNumberProp = serializedObject.FindProperty ("TabNumber");
        PlayerTransformProp = serializedObject.FindProperty("PlayerTransform");
        PlayerCameraProp = serializedObject.FindProperty("PlayerCamera");
        CameraFoldoutProp = serializedObject.FindProperty("CameraFoldout");
        SettingsFoldoutProp = serializedObject.FindProperty("SettingsFoldout");
        UniStormProfileProp = serializedObject.FindProperty("m_UniStormProfile");
        if (TimeIcon == null) TimeIcon = Resources.Load("TimeIcon") as Texture;
		if (WeatherIcon == null) WeatherIcon = Resources.Load("WeatherIcon") as Texture;
		if (CelestialIcon == null) CelestialIcon = Resources.Load("CelestialIcon") as Texture;
        if (CameraIcon == null) CameraIcon = Resources.Load("CameraIcon") as Texture;
        if (HelpIcon == null) HelpIcon = Resources.Load("HelpIcon") as Texture;
        if (SettingsIcon == null) SettingsIcon = Resources.Load("SettingsIcon") as Texture;

        //Camera & Player
        UniStormFollowsPlayerProp = serializedObject.FindProperty("UniStormFollowsPlayer");
        GetPlayerAtRuntimeProp = serializedObject.FindProperty("GetPlayerAtRuntime");
        GetPlayerMethodProp = serializedObject.FindProperty("GetPlayerMethod");
        UseRuntimeDelayProp = serializedObject.FindProperty("UseRuntimeDelay");
        PlayerTagProp = serializedObject.FindProperty("PlayerTag");
        CameraTagProp = serializedObject.FindProperty("CameraTag");
        PlayerNameProp = serializedObject.FindProperty("PlayerName");
        CameraNameProp = serializedObject.FindProperty("CameraName");
        UseUniStormMenuProp = serializedObject.FindProperty("UseUniStormMenu");
        UniStormMenuKeyProp = serializedObject.FindProperty("UniStormMenuKey");

        //Time
        HourProp = serializedObject.FindProperty ("Hour");
		MinuteProp = serializedObject.FindProperty ("Minute");
		TimeFlowProp = serializedObject.FindProperty ("TimeFlow");
		RealWorldTimeProp = serializedObject.FindProperty ("RealWorldTime");
        TimeOfDaySoundsSecondsMinProp = serializedObject.FindProperty("TimeOfDaySoundsSecondsMin");
        TimeOfDaySoundsSecondsMaxProp = serializedObject.FindProperty("TimeOfDaySoundsSecondsMax");
        TimeOfDayMusicDelayProp = serializedObject.FindProperty("TimeOfDayMusicDelay");
        TimeFoldoutProp = serializedObject.FindProperty("TimeFoldout");
        DateFoldoutProp = serializedObject.FindProperty("DateFoldout");
        TimeSoundsFoldoutProp = serializedObject.FindProperty("TimeSoundsFoldout");
        TimeMusicFoldoutProp = serializedObject.FindProperty("TimeMusicFoldout");
        TimeOfDaySoundsDuringPrecipitationWeatherProp = serializedObject.FindProperty("TimeOfDaySoundsDuringPrecipitationWeather");

        //Date
        DayProp = serializedObject.FindProperty ("Day");
		MonthProp = serializedObject.FindProperty ("Month");
		YearProp = serializedObject.FindProperty ("Year");
        DayLengthProp = serializedObject.FindProperty("DayLength");
        NightLengthProp = serializedObject.FindProperty("NightLength");

        //Weather
        PrecipitationGraphProp = serializedObject.FindProperty ("PrecipitationGraph");
		TemperatureCurveProp = serializedObject.FindProperty ("TemperatureCurve");
		TemperatureFluctuationProp = serializedObject.FindProperty ("TemperatureFluctuation");
		FogColorProp = serializedObject.FindProperty ("FogColor");
		FogStormyColorProp = serializedObject.FindProperty ("FogStormyColor");
		CloudLightColorProp = serializedObject.FindProperty ("CloudLightColor");
		CloudBaseColorProp = serializedObject.FindProperty ("CloudBaseColor");
		CloudStormyBaseColorProp = serializedObject.FindProperty ("CloudStormyBaseColor");
		TransitionSpeedProp = serializedObject.FindProperty ("TransitionSpeed");
		WeatherFoldoutProp = serializedObject.FindProperty ("WeatherFoldout");
		LightningFoldoutProp = serializedObject.FindProperty ("LightningFoldout");
        LightningStrikeOddsProp = serializedObject.FindProperty("LightningGroundStrikeOdds");
        LightningStrikeEffectProp = serializedObject.FindProperty("LightningStrikeEffect");
        LightningStrikeFireProp = serializedObject.FindProperty("LightningStrikeFire");
        LightningSecondsMinProp = serializedObject.FindProperty("LightningSecondsMin");
        LightningSecondsMaxProp = serializedObject.FindProperty("LightningSecondsMax");
        LightningLightIntensityMinProp = serializedObject.FindProperty("LightningLightIntensityMin");
        LightningLightIntensityMaxProp = serializedObject.FindProperty("LightningLightIntensityMax");
        LightningGenerationDistanceProp = serializedObject.FindProperty("LightningGenerationDistance");
        LightningDetectionDistanceProp = serializedObject.FindProperty("LightningDetectionDistance");
        CloudSpeedProp = serializedObject.FindProperty("CloudSpeed");
        WeatherGenerationMethodProp = serializedObject.FindProperty("WeatherGenerationMethod");
        CurrentWeatherTypeProp = serializedObject.FindProperty("CurrentWeatherType");
        DetectionLayerMaskProp = serializedObject.FindProperty("DetectionLayerMask");
        TemperatureTypeProp = serializedObject.FindProperty("TemperatureType");

        //Celestial
        SunColorProp = serializedObject.FindProperty ("SunColor");
        StormySunColorProp = serializedObject.FindProperty("StormySunColor");
        MoonColorProp = serializedObject.FindProperty ("MoonColor");
		SkyTintColorProp = serializedObject.FindProperty ("SkyTintColor");
        SkyColorProp = serializedObject.FindProperty("SkyColor");
        AmbientSkyLightColorProp = serializedObject.FindProperty ("AmbientSkyLightColor");
        StormyAmbientSkyLightColorProp = serializedObject.FindProperty("StormyAmbientSkyLightColor");
        AmbientEquatorLightColorProp = serializedObject.FindProperty ("AmbientEquatorLightColor");
        StormyAmbientEquatorLightColorProp = serializedObject.FindProperty("StormyAmbientEquatorLightColor");
        AmbientGroundLightColorProp = serializedObject.FindProperty ("AmbientGroundLightColor");
        StormyAmbientGroundLightColorProp = serializedObject.FindProperty("StormyAmbientGroundLightColor");
        StarLightColorProp = serializedObject.FindProperty ("StarLightColor");
		SunRevolutionProp = serializedObject.FindProperty ("SunRevolution");
		SunIntensityCurveProp = serializedObject.FindProperty ("SunIntensityCurve");
		SunSizeProp = serializedObject.FindProperty ("SunSize");
		SunFoldoutProp = serializedObject.FindProperty ("SunFoldout");
		MoonIntensityCurveProp = serializedObject.FindProperty ("MoonIntensityCurve");
		MoonSizeProp = serializedObject.FindProperty ("MoonSize");
        MoonPhaseColorProp = serializedObject.FindProperty("MoonPhaseColor");
        MoonFoldoutProp = serializedObject.FindProperty ("MoonFoldout");
		MoonPhaseIndexProp = serializedObject.FindProperty ("MoonPhaseIndex");
		MoonBrightnessProp = serializedObject.FindProperty ("MoonBrightness");
		AtmosphereFoldoutProp = serializedObject.FindProperty ("AtmosphereFoldout");
        StarSpeedProp = serializedObject.FindProperty("StarSpeed");
        SunAngleProp = serializedObject.FindProperty("SunAngle");
        MoonAngleProp = serializedObject.FindProperty("MoonAngle");
        HemisphereProp = serializedObject.FindProperty("Hemisphere");
        AtmosphereThicknessProp = serializedObject.FindProperty("AtmosphereThickness");

        //Reorderable lists
        //Put our lists into reorderable lists because Unity allows these lists to be serialized
        //All weather types
        AllWeatherTypesList = new ReorderableList(serializedObject, serializedObject.FindProperty("AllWeatherTypes"), true, true, true, true);
        AllWeatherTypesList.drawHeaderCallback = rect => {
            EditorGUI.LabelField(rect, "All Weather Types", EditorStyles.boldLabel);
        };
        AllWeatherTypesList.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) => {
                var element = AllWeatherTypesList.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
            };

        //Moon Phases New
        MoonPhaseList = new ReorderableList(serializedObject, serializedObject.FindProperty("MoonPhaseList"), true, true, true, true);
        MoonPhaseList.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) => {
                rect.y += 1;
                var element = MoonPhaseList.serializedProperty.GetArrayElementAtIndex(index);

                //Give our MoonPhaseIntensity an initialized value of 1.
                if (element.FindPropertyRelative("MoonPhaseIntensity").floatValue == 0)
                {
                    element.FindPropertyRelative("MoonPhaseIntensity").floatValue = 1;
                }

                EditorGUI.PropertyField(new Rect(rect.x + 200, rect.y, rect.width - 200, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("MoonPhaseIntensity"), GUIContent.none);
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, 200, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("MoonPhaseTexture"), GUIContent.none);
            };

        MoonPhaseList.drawHeaderCallback = rect => {
            EditorGUI.LabelField(rect, "   Moon Phase Texture  " + "               Light Intensity", EditorStyles.boldLabel);
        };

		//Lightning patterns
		LightningFlashPatternsList = new ReorderableList(serializedObject, serializedObject.FindProperty("LightningFlashPatterns"), true, true, true, true);
		LightningFlashPatternsList.drawHeaderCallback = rect => {
			EditorGUI.LabelField (rect, "Lightning Flash Patterns", EditorStyles.boldLabel);
		};
		LightningFlashPatternsList.drawElementCallback = 
			(Rect rect, int index, bool isActive, bool isFocused) => {
			var element = LightningFlashPatternsList.serializedProperty.GetArrayElementAtIndex (index);
			EditorGUI.PropertyField (new Rect (rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
		};

		//Thunder Sounds
		ThunderSoundsList = new ReorderableList(serializedObject, serializedObject.FindProperty("ThunderSounds"), true, true, true, true);
		ThunderSoundsList.drawHeaderCallback = rect => {
			EditorGUI.LabelField (rect, "Thunder Sounds", EditorStyles.boldLabel);
		};
		ThunderSoundsList.drawElementCallback = 
			(Rect rect, int index, bool isActive, bool isFocused) => {
			var element = ThunderSoundsList.serializedProperty.GetArrayElementAtIndex (index);
			EditorGUI.PropertyField (new Rect (rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
		};

        //Morning Sounds
        MorningSoundsList = new ReorderableList(serializedObject, serializedObject.FindProperty("MorningSounds"), true, true, true, true);
        MorningSoundsList.drawHeaderCallback = rect => {
            EditorGUI.LabelField(rect, "Morning Sounds", EditorStyles.boldLabel);
        };
        MorningSoundsList.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) => {
                var element = MorningSoundsList.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
            };

        //Day Sounds
        DaySoundsList = new ReorderableList(serializedObject, serializedObject.FindProperty("DaySounds"), true, true, true, true);
        DaySoundsList.drawHeaderCallback = rect => {
            EditorGUI.LabelField(rect, "Day Sounds", EditorStyles.boldLabel);
        };
        DaySoundsList.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) => {
                var element = DaySoundsList.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
            };

        //Evening Sounds
        EveningSoundsList = new ReorderableList(serializedObject, serializedObject.FindProperty("EveningSounds"), true, true, true, true);
        EveningSoundsList.drawHeaderCallback = rect => {
            EditorGUI.LabelField(rect, "Evening Sounds", EditorStyles.boldLabel);
        };
        EveningSoundsList.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) => {
                var element = EveningSoundsList.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
            };

        //Night Sounds
        NightSoundsList = new ReorderableList(serializedObject, serializedObject.FindProperty("NightSounds"), true, true, true, true);
        NightSoundsList.drawHeaderCallback = rect => {
            EditorGUI.LabelField(rect, "Night Sounds", EditorStyles.boldLabel);
        };
        NightSoundsList.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) => {
                var element = NightSoundsList.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
            };

        //Morning Music
        MorningMusicList = new ReorderableList(serializedObject, serializedObject.FindProperty("MorningMusic"), true, true, true, true);
        MorningMusicList.drawHeaderCallback = rect => {
            EditorGUI.LabelField(rect, "Morning Music", EditorStyles.boldLabel);
        };
        MorningMusicList.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) => {
                var element = MorningMusicList.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
            };

        //Day Music
        DayMusicList = new ReorderableList(serializedObject, serializedObject.FindProperty("DayMusic"), true, true, true, true);
        DayMusicList.drawHeaderCallback = rect => {
            EditorGUI.LabelField(rect, "Day Music", EditorStyles.boldLabel);
        };
        DayMusicList.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) => {
                var element = DayMusicList.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
            };

        //Evening Music
        EveningMusicList = new ReorderableList(serializedObject, serializedObject.FindProperty("EveningMusic"), true, true, true, true);
        EveningMusicList.drawHeaderCallback = rect => {
            EditorGUI.LabelField(rect, "Evening Music", EditorStyles.boldLabel);
        };
        EveningMusicList.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) => {
                var element = EveningMusicList.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
            };

        //Night Music
        NightMusicList = new ReorderableList(serializedObject, serializedObject.FindProperty("NightMusic"), true, true, true, true);
        NightMusicList.drawHeaderCallback = rect => {
            EditorGUI.LabelField(rect, "Night Music", EditorStyles.boldLabel);
        };
        NightMusicList.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) => {
                var element = NightMusicList.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
            };

        //Lightning Fire Tags
        LightningFireTagsList = new ReorderableList(serializedObject, serializedObject.FindProperty("LightningFireTags"), true, true, true, true);
        LightningFireTagsList.drawHeaderCallback = rect => {
            EditorGUI.LabelField(rect, "Lightning Fire Tags", EditorStyles.boldLabel);
        };
        LightningFireTagsList.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) => {
                var element = LightningFireTagsList.serializedProperty.GetArrayElementAtIndex(index);

                if (element.stringValue == "")
                {
                    element.stringValue = "Untagged";
                }

                element.stringValue = EditorGUI.TagField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element.stringValue);
            };
    }

	public override void OnInspectorGUI () {
		UniStormSystem self = (UniStormSystem)target;

		serializedObject.Update ();

		EditorGUILayout.Space();
		EditorGUILayout.Space();

        GUIStyle myFoldoutStyle = new GUIStyle(EditorStyles.foldout);
		myFoldoutStyle.fontStyle = FontStyle.Bold;
		myFoldoutStyle.fontSize = 12;
		myFoldoutStyle.active.textColor = Color.black;
		myFoldoutStyle.focused.textColor = Color.white;
		myFoldoutStyle.onHover.textColor = Color.black;
		myFoldoutStyle.normal.textColor = Color.white;
		myFoldoutStyle.onNormal.textColor = Color.black;
		myFoldoutStyle.onActive.textColor = Color.black;
		myFoldoutStyle.onFocused.textColor = Color.black;
		Color myStyleColor = Color.black;

        var HelpStyle = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.UpperRight };

        GUIContent[] TabButtons = new GUIContent[5] {new GUIContent(" Player & \n Camera", CameraIcon), new GUIContent(" Time", TimeIcon), new GUIContent(" Weather", WeatherIcon),
            new GUIContent(" Celestial", CelestialIcon), new GUIContent(" Settings", SettingsIcon)};

        EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		TabNumberProp.intValue = GUILayout.SelectionGrid (TabNumberProp.intValue, TabButtons, 5, GUILayout.Height(28), GUILayout.Width(90 * Screen.width/100));
        GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

        //Camera Settings
        if (TabNumberProp.intValue == 0)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));

            if (GUILayout.Button(new GUIContent(HelpIcon), HelpStyle, GUILayout.ExpandWidth(true), GUILayout.Height(22.5f)))
            {
                Application.OpenURL("https://docs.google.com/document/d/1uL_oMqHC_EduRGEnOihifwcpkQmWX9rubGw8qjkZ4b4/edit#heading=h.z5gprdso537b");
            }

            var style = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
            EditorGUILayout.LabelField(new GUIContent(CameraIcon), style, GUILayout.ExpandWidth(true), GUILayout.Height(32));
            EditorGUILayout.LabelField("Camera & Player Settings", style, GUILayout.ExpandWidth(true));

            GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f, 1);
            EditorGUILayout.LabelField("The Camera & Player Settings allow you to control what objects UniStorm uses for its player and camera.", EditorStyles.helpBox);

            GUI.backgroundColor = new Color(1f, 1, 0.5f, 0.5f);
            EditorGUILayout.LabelField("If you need help, you can hover over each field with your mouse for a tooltip description. " +
                "Documentation for this section can be found by pressing the ? in the upper right hand corner.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;

            GUILayout.Space(5);

            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            //Camera Settings
            if (!CameraFoldoutProp.boolValue)
            {
                GUI.backgroundColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
            }

            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical();

            CameraFoldoutProp.boolValue = Foldout(CameraFoldoutProp.boolValue, "Camera & Player Settings", true, myFoldoutStyle);
            GUI.backgroundColor = Color.white;

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            if (CameraFoldoutProp.boolValue)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(15);
                EditorGUILayout.BeginVertical();
                GUILayout.Space(10);

                EditorGUILayout.PropertyField(GetPlayerAtRuntimeProp,
                        new GUIContent("Get Player at Runtime", "Controls whether or not UniStorm will get your player at runtime. " +
                        "This is useful if your player is instantiated or created at runtime."));
                EditorGUILayout.Space();

                if (self.GetPlayerAtRuntime == UniStormSystem.EnableFeature.Enabled)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(15);
                    EditorGUILayout.BeginVertical();

                    EditorGUILayout.PropertyField(GetPlayerMethodProp,
                        new GUIContent("Get Player Method", "Controls whether your player and camera are found by tag or by name."));
                    EditorGUILayout.Space();

                    if (self.GetPlayerMethod == UniStormSystem.GetPlayerMethodEnum.ByTag)
                    {
                        CustomTagField(new Rect(), new GUIContent("Player Tag", "The tag of your player."), PlayerTagProp, "Player Tag");
                        CustomTagField(new Rect(), new GUIContent("Camera Tag", "The tag of your player's camera."), CameraTagProp, "Camera Tag");
                        EditorGUILayout.Space();
                    }
                    else if (self.GetPlayerMethod == UniStormSystem.GetPlayerMethodEnum.ByName)
                    {
                        EditorGUILayout.PropertyField(PlayerNameProp, new GUIContent("Player Name", "The name of your player."));
                        EditorGUILayout.PropertyField(CameraNameProp, new GUIContent("Camera Name", "The name of your player's camera."));
                        EditorGUILayout.Space();
                    }

                    EditorGUILayout.PropertyField(UseRuntimeDelayProp,
                        new GUIContent("Use Runtime Delay", "Controls whether or not UniStorm will wait to initialize until the runtime player has been created and found. " +
                        "Note: There will be a brief flash upon initialization for about 0.2 seconds."));
                    EditorGUILayout.Space();

                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();
                }

                if (self.GetPlayerAtRuntime == UniStormSystem.EnableFeature.Disabled)
                {
                    CustomObjectField(new Rect(), new GUIContent(), PlayerTransformProp,
                    new GUIContent("Player Transform", "The parent transform your player uses."), typeof(Transform), true);
                    CustomObjectField(new Rect(), new GUIContent(), PlayerCameraProp,
                    new GUIContent("Player Camera", "The main camera your player uses."), typeof(Camera), true);
                    EditorGUILayout.Space();
                }

                EditorGUILayout.PropertyField(UniStormFollowsPlayerProp,
                        new GUIContent("UniStorm Follows Player", "Controls whether or not UniStorm will follow your player when using large or infinite terrains."));
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(UseUniStormMenuProp,
                    new GUIContent("Use UniStorm Menu", "Controls whether UniStorm's UI menu is usable during runtime. This menu allows the ability to control UniStorm's " +
                    "time and weather via a slider and drop down UI."));
                EditorGUILayout.Space();

                if (self.UseUniStormMenu == UniStormSystem.EnableFeature.Enabled)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(15);
                    EditorGUILayout.BeginVertical();

                    EditorGUILayout.PropertyField(UniStormMenuKeyProp,
                    new GUIContent("UniStorm Menu Key", "Controls which key will enable UniStorm's menu."));
                    EditorGUILayout.Space();

                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();
                }


                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
        }

        //Time Settings
        if (TabNumberProp.intValue == 1)
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			EditorGUILayout.BeginVertical("Box",GUILayout.Width(90 * Screen.width/100));

            if (GUILayout.Button(new GUIContent(HelpIcon), HelpStyle, GUILayout.ExpandWidth(true), GUILayout.Height(22.5f)))
            {
                Application.OpenURL("https://docs.google.com/document/d/1uL_oMqHC_EduRGEnOihifwcpkQmWX9rubGw8qjkZ4b4/edit#heading=h.3fm44ev48fcm");
            }

            var style = new GUIStyle(EditorStyles.boldLabel) {alignment = TextAnchor.MiddleCenter};
			EditorGUILayout.LabelField(new GUIContent(TimeIcon), style, GUILayout.ExpandWidth(true), GUILayout.Height(32));
			EditorGUILayout.LabelField("Time Management", style, GUILayout.ExpandWidth(true));

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (self.Minute < 10)
            {
                EditorGUILayout.LabelField("Time: " + self.Hour + ":" + 0 + self.Minute + "  Date: " + self.Month + "/" + self.Day + "/" + self.Year + 
                    "  Day of the Week: " + self.UniStormDate.DayOfWeek.ToString(), EditorStyles.helpBox);
            }
            else
            {
                EditorGUILayout.LabelField("Time: " + self.Hour + ":" + self.Minute + "  Date: " + self.Month + "/" + self.Day + "/" + self.Year + 
                    "  Day of the Week: " + self.UniStormDate.DayOfWeek.ToString(), EditorStyles.helpBox);
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                self.CalculateSeason();
            }
#endif
            EditorGUILayout.LabelField("Current Season: " + self.CurrentSeason, EditorStyles.helpBox);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f, 1);
            EditorGUILayout.LabelField("The Time Management section allow you to control various time related settings such as starting time, starting date, and time flow.", EditorStyles.helpBox);

            GUI.backgroundColor = new Color(1f, 1, 0.5f, 0.5f);
            EditorGUILayout.LabelField("If you need help, you can hover over each field with your mouse for a tooltip description. " +
                "Documentation for this section can be found by pressing the ? in the upper right hand corner.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;

            GUILayout.Space(5);

			EditorGUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

            //Time Settings
			if (!TimeFoldoutProp.boolValue)
			{
				GUI.backgroundColor = new Color(0.5f,0.5f,0.5f,1.0f);
			}

			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(10);
			EditorGUILayout.BeginVertical();

			TimeFoldoutProp.boolValue = Foldout(TimeFoldoutProp.boolValue, "Time Settings", true, myFoldoutStyle);
			GUI.backgroundColor = Color.white;

			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();

            if (TimeFoldoutProp.boolValue)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(15);
                EditorGUILayout.BeginVertical();
                GUILayout.Space(10);

                if (self.RealWorldTime == UniStormSystem.EnableFeature.Disabled)
                {
                    CustomIntSlider(new Rect(), new GUIContent(), HourProp,
                    new GUIContent("Hour", "The starting hour UniStorm will start with."), 0, 23);
                    EditorGUILayout.Space();

                    CustomIntSlider(new Rect(), new GUIContent(), MinuteProp,
                        new GUIContent("Minute", "The starting minute UniStorm will start with."), 0, 59);
                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(TimeFlowProp,
                        new GUIContent("Time Flow", "Controls whether UniStorm's time is currently flowing."));
                    EditorGUILayout.Space();
                }
                else if (self.RealWorldTime == UniStormSystem.EnableFeature.Enabled)
                {
                    GUI.backgroundColor = new Color(1f, 1, 0.5f, 0.5f);
                    EditorGUILayout.LabelField("The time settings cannot be set when using the Real-world Time setting.", EditorStyles.helpBox);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                }

                EditorGUILayout.PropertyField(RealWorldTimeProp,
                    new GUIContent("Use Real-world Time", "Controls whether UniStorm's time will use real-world time. " +
                    "Note: This will overwrite your starting time on start and your UniStorm Date will use the real-world date."));
                EditorGUILayout.Space();

                if (self.RealWorldTime == UniStormSystem.EnableFeature.Disabled)
                {
                    CustomIntSlider(new Rect(), new GUIContent(), DayLengthProp,
                        new GUIContent("Day Length", "Controls how long, in minutes, UniStorm's Days are."), 1, 500);
                    EditorGUILayout.Space();

                    CustomIntSlider(new Rect(), new GUIContent(), NightLengthProp,
                        new GUIContent("Night Length", "Controls how long, in minutes, UniStorm's Nights are."), 1, 500);
                    EditorGUILayout.Space();
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();

            }
            EditorGUILayout.EndVertical();

            //Date Settings
            if (!DateFoldoutProp.boolValue)
            {
                GUI.backgroundColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
            }

            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical();

            DateFoldoutProp.boolValue = Foldout(DateFoldoutProp.boolValue, "Date Settings", true, myFoldoutStyle);
            GUI.backgroundColor = Color.white;

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            if (DateFoldoutProp.boolValue)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(15);
                EditorGUILayout.BeginVertical();
                GUILayout.Space(10);

                if (self.RealWorldTime == UniStormSystem.EnableFeature.Disabled)
                {
                    CustomIntSlider(new Rect(), new GUIContent(), MonthProp,
                    new GUIContent("Month", "The starting month UniStorm will start with."), 1, 12);
                    EditorGUILayout.Space();

                    CustomIntSlider(new Rect(), new GUIContent(), DayProp,
                        new GUIContent("Day", "The starting day UniStorm will start with."), 1, 31);
                    EditorGUILayout.Space();

                    CustomIntSlider(new Rect(), new GUIContent(), YearProp,
                        new GUIContent("Year", "The starting year UniStorm will start with."), -2000, 3000);
                    EditorGUILayout.Space();
                }
                else if (self.RealWorldTime == UniStormSystem.EnableFeature.Enabled)
                {
                    GUI.backgroundColor = new Color(1f, 1, 0.5f, 0.5f);
                    EditorGUILayout.LabelField("The date settings cannot be set when using the Real-world Time setting.", EditorStyles.helpBox);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();


            //Time Sounds Settings
            if (!TimeSoundsFoldoutProp.boolValue)
            {
                GUI.backgroundColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
            }

            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical();

            TimeSoundsFoldoutProp.boolValue = Foldout(TimeSoundsFoldoutProp.boolValue, "Time of Day Sounds Settings", true, myFoldoutStyle);
            GUI.backgroundColor = Color.white;

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            if (TimeSoundsFoldoutProp.boolValue)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(15);
                EditorGUILayout.BeginVertical();
                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TimeOfDaySoundsDuringPrecipitationWeatherProp,
                    new GUIContent("Play During Precipitation", "Controls whether UniStorm's Time of Day sounds will play during precipitation weather types."));
                EditorGUILayout.Space();

                CustomIntSlider(new Rect(), new GUIContent(), TimeOfDaySoundsSecondsMinProp,
                    new GUIContent("Time of Day Sounds Min", "The minimum seconds needed for a time of day sound to trigger."), 0, self.TimeOfDaySoundsSecondsMax);
                EditorGUILayout.Space();

                CustomIntSlider(new Rect(), new GUIContent(), TimeOfDaySoundsSecondsMaxProp,
                    new GUIContent("Time of Day Sounds Max", "The maximum seconds needed for a time of day sound to trigger."), self.TimeOfDaySoundsSecondsMin, 60);
                EditorGUILayout.Space();

                CustomFloatSlider(new Rect(), new GUIContent(), TimeOfDaySoundsVolumeProp,
                    new GUIContent("Time of Day Sounds Volume", "The volume of the time of day sounds. This value alters UniStorm's Ambience Audio Mixer volume."), 0f, 1f);
                EditorGUILayout.Space();

                EditorGUILayout.Space();
                EditorGUILayout.Space();
                GUILayout.Box(new GUIContent("What's This?", "A list of possible sounds that UniStorm will play during the Morning."),
                    EditorStyles.toolbarButton, GUILayout.ExpandWidth(false));
                MorningSoundsList.DoLayoutList();

                EditorGUILayout.Space();
                EditorGUILayout.Space();
                GUILayout.Box(new GUIContent("What's This?", "A list of possible sounds that UniStorm will play during the Day."),
                    EditorStyles.toolbarButton, GUILayout.ExpandWidth(false));
                DaySoundsList.DoLayoutList();

                EditorGUILayout.Space();
                EditorGUILayout.Space();
                GUILayout.Box(new GUIContent("What's This?", "A list of possible sounds that UniStorm will play during the Evening."),
                    EditorStyles.toolbarButton, GUILayout.ExpandWidth(false));
                EveningSoundsList.DoLayoutList();

                EditorGUILayout.Space();
                EditorGUILayout.Space();
                GUILayout.Box(new GUIContent("What's This?", "A list of possible sounds that UniStorm will play during the Night."),
                    EditorStyles.toolbarButton, GUILayout.ExpandWidth(false));
                NightSoundsList.DoLayoutList();

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();

            //Time Music Settings
            if (!TimeMusicFoldoutProp.boolValue)
            {
                GUI.backgroundColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
            }

            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical();

            TimeMusicFoldoutProp.boolValue = Foldout(TimeMusicFoldoutProp.boolValue, "Time of Day Music Settings", true, myFoldoutStyle);
            GUI.backgroundColor = Color.white;

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            if (TimeMusicFoldoutProp.boolValue)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(15);
                EditorGUILayout.BeginVertical();
                GUILayout.Space(10);

                CustomIntSlider(new Rect(), new GUIContent(), TimeOfDayMusicDelayProp,
                    new GUIContent("Time of Day Music Delay", "The delay, in seconds, between each time of day music clip."), 0, 30);
                EditorGUILayout.Space();

                CustomFloatSlider(new Rect(), new GUIContent(), TimeOfDayMusicVolumeProp,
                    new GUIContent("Time of Day Music Volume", "The volume of the time of day music. This value alters UniStorm's Music Audio Mixer volume."), 0f, 1f);
                EditorGUILayout.Space();

                EditorGUILayout.Space();
                GUILayout.Box(new GUIContent("What's This?", "A list of possible music clips that UniStorm will play during the Morning."),
                    EditorStyles.toolbarButton, GUILayout.ExpandWidth(false));
                MorningMusicList.DoLayoutList();

                EditorGUILayout.Space();
                EditorGUILayout.Space();
                GUILayout.Box(new GUIContent("What's This?", "A list of possible music clips that UniStorm will play during the Day."),
                    EditorStyles.toolbarButton, GUILayout.ExpandWidth(false));
                DayMusicList.DoLayoutList();

                EditorGUILayout.Space();
                EditorGUILayout.Space();
                GUILayout.Box(new GUIContent("What's This?", "A list of possible music clips that UniStorm will play during the Evening."),
                    EditorStyles.toolbarButton, GUILayout.ExpandWidth(false));
                EveningMusicList.DoLayoutList();

                EditorGUILayout.Space();
                EditorGUILayout.Space();
                GUILayout.Box(new GUIContent("What's This?", "A list of possible music clips that UniStorm will play during the Night."),
                    EditorStyles.toolbarButton, GUILayout.ExpandWidth(false));
                NightMusicList.DoLayoutList();

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();

        }

		//Weather Settings
		if (TabNumberProp.intValue == 2)
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			EditorGUILayout.BeginVertical("Box",GUILayout.Width(90 * Screen.width/100));

            if (GUILayout.Button(new GUIContent(HelpIcon), HelpStyle, GUILayout.ExpandWidth(true), GUILayout.Height(22.5f)))
            {
                Application.OpenURL("https://docs.google.com/document/d/1uL_oMqHC_EduRGEnOihifwcpkQmWX9rubGw8qjkZ4b4/edit#heading=h.8rkomidbhb1x");
            }

            var style = new GUIStyle(EditorStyles.boldLabel) {alignment = TextAnchor.MiddleCenter};
			EditorGUILayout.LabelField(new GUIContent(WeatherIcon), style, GUILayout.ExpandWidth(true), GUILayout.Height(32));
			EditorGUILayout.LabelField("Weather Management", style, GUILayout.ExpandWidth(true));

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField("Temperature: " + self.Temperature, EditorStyles.helpBox);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
            if (self.CurrentWeatherType != null)
            {
                EditorGUILayout.LabelField("Current Weather: " + self.CurrentWeatherType.WeatherTypeName, EditorStyles.helpBox);
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            if (self.NextWeatherType != null)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (self.WeatherGenerationMethod == UniStormSystem.WeatherGenerationMethodEnum.Daily)
                {
                    EditorGUILayout.LabelField("Forecast: " + self.NextWeatherType.WeatherTypeName + " at " + self.HourToChangeWeather + ":00", EditorStyles.helpBox);
                }
                else  if (self.WeatherGenerationMethod == UniStormSystem.WeatherGenerationMethodEnum.Hourly && self.Hour < 23)
                {
                    EditorGUILayout.LabelField("Forecast: " + self.NextWeatherType.WeatherTypeName + " at " + (self.Hour+1)+":00", EditorStyles.helpBox);
                }
                else if (self.WeatherGenerationMethod == UniStormSystem.WeatherGenerationMethodEnum.Hourly && self.Hour == 23)
                {
                    EditorGUILayout.LabelField("Forecast: " + "?", EditorStyles.helpBox);
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }

            GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f, 1);
            EditorGUILayout.LabelField("The Weather Management section allow you to adjust various settings related to weather. These include the Weather Types that UniStorm can use, " +
                "fog and cloud colors, and lightning settings.", EditorStyles.helpBox);

            GUI.backgroundColor = new Color(1f, 1, 0.5f, 0.5f);
            EditorGUILayout.LabelField("If you need help, you can hover over each field with your mouse for a tooltip description. " +
                "Documentation for this section can be found by pressing the ? in the upper right hand corner.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;

            GUILayout.Space(5);

			EditorGUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			//Weather Settings
			if (!WeatherFoldoutProp.boolValue)
			{
				GUI.backgroundColor = new Color(0.5f,0.5f,0.5f,1.0f);
			}

			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(10);
			EditorGUILayout.BeginVertical();

			WeatherFoldoutProp.boolValue = Foldout(WeatherFoldoutProp.boolValue, "Weather Settings", true, myFoldoutStyle);
			GUI.backgroundColor = Color.white;

			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();

			if (WeatherFoldoutProp.boolValue)
			{
				EditorGUILayout.BeginHorizontal();
				GUILayout.Space(15);
				EditorGUILayout.BeginVertical();
				GUILayout.Space(10);

                EditorGUILayout.PropertyField(WeatherGenerationMethodProp,
                    new GUIContent("Weather Generation Type", "Controls whether UniStorm's weather is generated per hour or per day. When using Daily weather generation, a generated hour " +
                    "for the weather change is also generated."));
                EditorGUILayout.Space();

                CustomFloatSlider(new Rect(), new GUIContent(), WeatherSoundsVolumeProp,
                    new GUIContent("Weather Sounds Volume", "The global volume for UniStorm's weather sounds. Each weather effect will still have its own volume. " +
                    "This settings simply controls the global volume of all weather sounds and thunder. This value alters UniStorm's Weather Audio Mixer volume."), 0f, 1f);
                EditorGUILayout.Space();

                CustomIntSlider(new Rect(), new GUIContent(), TransitionSpeedProp, 
                    new GUIContent("Transition Speed", "Controls how fast UniStorm will transition Weather Types."), 8, 250);
				EditorGUILayout.Space();

                CustomIntSlider(new Rect(), new GUIContent(), CloudSpeedProp,
                    new GUIContent("Cloud Speed", "Controls how fast UniStorm's procedural clouds move across the sky."), 1, 25);
                EditorGUILayout.Space();

                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(PrecipitationGraphProp,
                    new GUIContent("Precipiation Odds", "Controls the odds of UniStorm's precipitation throughout the year. X represents the calendar month and Y represents the odds."));
                EditorGUILayout.Space();

                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(TemperatureTypeProp,
                    new GUIContent("Temperature Type", "Controls whether UniStorm uses the Fahrenheit or Celsius temperature type. Fahrenheit requires a temperature of 32 degrees or colder for snow" +
                    " and Celsius requires a temperature of 0 degrees or colder for snow."));
                EditorGUILayout.Space();

                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(TemperatureCurveProp,
                    new GUIContent("Temperature Curve", "Controls UniStorm's temperature throughout the year. X represents the calendar month and Y represents the temperature."));
                EditorGUILayout.Space();

				EditorGUILayout.Space();
                EditorGUILayout.PropertyField(TemperatureFluctuationProp,
                    new GUIContent("Temperature Fluctuation", "Controls UniStorm's temperature fluctuation throughout the day. X represents the hour and Y represents the temperature fluctuation."));
                EditorGUILayout.Space();

                EditorGUILayout.Space();
                CustomObjectField(new Rect(), new GUIContent(), CurrentWeatherTypeProp,
                    new GUIContent("Starting Weather Type", "The starting weather type UniStorm will start with."), typeof(WeatherType), true);

                EditorGUILayout.Space();
				EditorGUILayout.Space();

                GUILayout.Box(new GUIContent("What's This?", "A list of all possible Weather Types that UniStorm will use when generating weather. " +
                    "To create a new Weather Type, right click in the project tab and go to Create>UniStorm>New Weather Type."),
                    EditorStyles.toolbarButton, GUILayout.ExpandWidth(false));
                AllWeatherTypesList.DoLayoutList();
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.Space();
				EditorGUILayout.PropertyField(FogColorProp,
                    new GUIContent("Fog Color", "A gradient that controls the fog color during non-precipitation Weather Types. Each element is a transition into the next time of day."));
				EditorGUILayout.Space();

				EditorGUILayout.Space();
				EditorGUILayout.PropertyField(FogStormyColorProp,
                    new GUIContent("Fog Color (Stormy)", "A gradient that controls the fog color during precipitation Weather Types. Each element is a transition into the next time of day."));
				EditorGUILayout.Space();

				EditorGUILayout.Space();
				EditorGUILayout.PropertyField(CloudBaseColorProp,
                    new GUIContent("Cloud Color", "A gradient that controls the base color of UniStorm's clouds during non-precipitation Weather Types. " +
                    "Each element is a transition into the next time of day."));
				EditorGUILayout.Space();

                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(CloudStormyBaseColorProp,
                    new GUIContent("Cloud Color (Stormy)", "A gradient that controls the base color of UniStorm's clouds during precipitation Weather Types. " +
                    "Each element is a transition into the next time of day."));
                EditorGUILayout.Space();

                EditorGUILayout.Space();
				EditorGUILayout.PropertyField(CloudLightColorProp,
                    new GUIContent("Cloud Light Color", "A gradient that controls the light color of UniStorm's clouds when the clouds are receiving light. " +
                    "Each element is a transition into the next time of day."));
				EditorGUILayout.Space();

				EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.EndVertical();
			GUI.backgroundColor = Color.white;

			//Lightning Settings
			if (!LightningFoldoutProp.boolValue)
			{
				GUI.backgroundColor = new Color(0.5f,0.5f,0.5f,1.0f);
			}

			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(10);
			EditorGUILayout.BeginVertical();

			LightningFoldoutProp.boolValue = Foldout(LightningFoldoutProp.boolValue, "Lightning Settings", true, myFoldoutStyle);
			GUI.backgroundColor = Color.white;

			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();

			if (LightningFoldoutProp.boolValue)
			{
				EditorGUILayout.BeginHorizontal();
				GUILayout.Space(15);
				EditorGUILayout.BeginVertical();

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                CustomIntSlider(new Rect(), new GUIContent(), LightningGenerationDistanceProp,
                new GUIContent("Generation Distance", "Controls the maximum distance lightning can be generated around the player."), 50, 300);
                EditorGUILayout.Space();

                CustomIntSlider(new Rect(), new GUIContent(), LightningSecondsMinProp,
                new GUIContent("Min Lightning Seconds", "Controls the minimum seconds for lightning to be generated."), 2, self.LightningSecondsMax);
                
                CustomIntSlider(new Rect(), new GUIContent(), LightningSecondsMaxProp,
                new GUIContent("Max Lightning Seconds", "Controls the maximum seconds for lightning to be generated."), self.LightningSecondsMin+1, 60);

                EditorGUILayout.Space();
                CustomFloatSlider(new Rect(), new GUIContent(), LightningLightIntensityMinProp,
                new GUIContent("Min Lightning Intensity", "Controls the minimum light intensity for the lightning to be generated."), 1, self.LightningLightIntensityMax);

                CustomFloatSlider(new Rect(), new GUIContent(), LightningLightIntensityMaxProp,
                new GUIContent("Max Lightning Intensity", "Controls the maximum light intensity for the lightning to be generated."), 1, 4);
                EditorGUILayout.Space();

                CustomIntSlider(new Rect(), new GUIContent(), LightningDetectionDistanceProp,
                new GUIContent("Detection Distance", "Controls the distance of UniStorm's lightning strike collider. The larger the radius, the more likely lightning will strike " +
                "objects instead of the ground."), 5, 60);
                EditorGUILayout.Space();

                EditorGUILayout.Space();
                EditorGUI.BeginChangeCheck();
                var layersSelection = EditorGUILayout.MaskField(new GUIContent("Lightning Strike Layers", "Controls what layers UniStorm's procedural lightning can strike."), LayerMaskToField(DetectionLayerMaskProp.intValue), InternalEditorUtility.layers);
                EditorGUILayout.Space();
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(self, "Layers changed");
                    DetectionLayerMaskProp.intValue = FieldToLayerMask(layersSelection);
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();
                GUILayout.Box(new GUIContent("What's This?", "A list of tags that will create a fire particle effect when struck by lightning."),
                    EditorStyles.toolbarButton, GUILayout.ExpandWidth(false));
                LightningFireTagsList.DoLayoutList();
                EditorGUILayout.Space();

                CustomIntSlider(new Rect(), new GUIContent(), LightningStrikeOddsProp, 
                    new GUIContent("Ground Strike Odds", "Controls the odds in which UniStorm's lightning will strike the ground or other objects of the appropriate tag."), 0, 100);

                EditorGUILayout.Space();
                CustomObjectField(new Rect(), new GUIContent(), LightningStrikeEffectProp, 
                    new GUIContent("Lightning Strike Effect","The effect that plays when lightning strikes the ground."), typeof(GameObject), true);

                EditorGUILayout.Space();
                CustomObjectField(new Rect(), new GUIContent(), LightningStrikeFireProp, 
                    new GUIContent("Lightning Strike Fire", "The fire effect that plays when lightning strikes an object of the appropriate tag."), typeof(GameObject), true);

                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                GUILayout.Box(new GUIContent("What's This?", "A list of possible lightning flash patterns that UniStorm will use during lightning Weather Types."),
                    EditorStyles.toolbarButton, GUILayout.ExpandWidth(false));
                LightningFlashPatternsList.DoLayoutList();

				EditorGUILayout.Space();
				EditorGUILayout.Space();
                GUILayout.Box(new GUIContent("What's This?", "A list of possible thunder sounds that UniStorm will play during lightning Weather Types."),
                    EditorStyles.toolbarButton, GUILayout.ExpandWidth(false));
                ThunderSoundsList.DoLayoutList();

				EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.EndVertical();
			GUI.backgroundColor = Color.white;
		}

		//Celestial Settings
		if (TabNumberProp.intValue == 3)
		{
            EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			EditorGUILayout.BeginVertical("Box",GUILayout.Width(90 * Screen.width/100));

            if (GUILayout.Button(new GUIContent(HelpIcon), HelpStyle, GUILayout.ExpandWidth(true), GUILayout.Height(22.5f)))
            {
                Application.OpenURL("https://docs.google.com/document/d/1uL_oMqHC_EduRGEnOihifwcpkQmWX9rubGw8qjkZ4b4/edit#heading=h.g75asyk1ag9n");
            }

            var style = new GUIStyle(EditorStyles.boldLabel) {alignment = TextAnchor.MiddleCenter};
			EditorGUILayout.LabelField(new GUIContent(CelestialIcon), style, GUILayout.ExpandWidth(true), GUILayout.Height(32));
			EditorGUILayout.LabelField("Celestial Settings", style, GUILayout.ExpandWidth(true));

            GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f, 1);
            EditorGUILayout.LabelField("The Celestial Settings allow you to control various celestial settings and colors for UniStorm's sun, moon, stars, and atmosphere.", EditorStyles.helpBox);

            GUI.backgroundColor = new Color(1f, 1, 0.5f, 0.5f);
            EditorGUILayout.LabelField("If you need help, you can hover over each field with your mouse for a tooltip description. " +
                "Documentation for this section can be found by pressing the ? in the upper right hand corner.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;

            GUILayout.Space(5);

			EditorGUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			//Sun Settings
			if (!SunFoldoutProp.boolValue)
			{
				GUI.backgroundColor = new Color(0.5f,0.5f,0.5f,1.0f);
			}

			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(10);
			EditorGUILayout.BeginVertical();

			SunFoldoutProp.boolValue = Foldout(SunFoldoutProp.boolValue, "Sun Settings", true, myFoldoutStyle);
			GUI.backgroundColor = Color.white;

			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();

			if (SunFoldoutProp.boolValue)
			{
				EditorGUILayout.BeginHorizontal();
				GUILayout.Space(15);
				EditorGUILayout.BeginVertical();

				EditorGUILayout.Space();
				EditorGUILayout.PropertyField(SunColorProp, 
                    new GUIContent("Sun Color", "A gradient that controls UniStorm's sun color during non-precipitation weather types. Each element is a transition into the next time of day."));
                EditorGUILayout.PropertyField(StormySunColorProp,
                    new GUIContent("Stormy Sun Color", "A gradient that controls UniStorm's sun color during precipitation weather types. Each element is a transition into the next time of day."));

                EditorGUILayout.Space();
                CustomIntSlider(new Rect(), new GUIContent(), SunAngleProp,
                        new GUIContent("Sun Tilt Angle", "Controls the tilt angle of the Sun."), -45, 45);

                EditorGUILayout.Space();
				CustomIntSlider(new Rect(), new GUIContent(), SunRevolutionProp, 
                    new GUIContent("Sun Revolution", "Controls the direction in which UniStorm's sun sets and rise."), -180, 180);
				EditorGUILayout.Space();

                EditorGUILayout.PropertyField(SunIntensityCurveProp,
                    new GUIContent("Sun Intensity Curve", "Controls the intensity of UniStorm's sun. X represents the hour and Y represents the intensity."));
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(SunSizeProp,
                    new GUIContent("Sun Size Curve", "Controls the size of UniStorm's sun. X represents the hour and Y represents the size."));
                EditorGUILayout.Space();

				EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.EndVertical();
			GUI.backgroundColor = Color.white;

			//Moon Settings
			if (!MoonFoldoutProp.boolValue)
			{
				GUI.backgroundColor = new Color(0.5f,0.5f,0.5f,1.0f);
			}
			else
			{
				GUI.backgroundColor = Color.white;
			}

			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(10);
			EditorGUILayout.BeginVertical();

			MoonFoldoutProp.boolValue = Foldout(MoonFoldoutProp.boolValue, "Moon Settings", true, myFoldoutStyle);
			GUI.backgroundColor = Color.white;

			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();

			if (MoonFoldoutProp.boolValue)
			{
				EditorGUILayout.BeginHorizontal();
				GUILayout.Space(15);
				EditorGUILayout.BeginVertical();

				EditorGUILayout.Space();
				EditorGUILayout.PropertyField(MoonColorProp, 
                    new GUIContent("Moon Light Color", "A gradient that controls UniStorm's moon light color."));

                EditorGUILayout.Space();
                CustomIntSlider(new Rect(), new GUIContent(), MoonAngleProp,
                        new GUIContent("Moon Tilt Angle", "Controls the tilt angle of the Moon."), -45, 45);
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(MoonIntensityCurveProp,
                    new GUIContent("Moon Intensity Curve", "Controls the intensity of UniStorm's moon. X represents the hour and Y represents the intensity."));
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(MoonSizeProp,
                    new GUIContent("Moon Size Curve", "Controls the size of UniStorm's moon. X represents the hour and Y represents the size."));
                EditorGUILayout.Space();

                EditorGUILayout.Space();
				EditorGUILayout.Space();
                GUILayout.Box(new GUIContent("What's This?", "A list of moon phase textures that UniStorm will use when creating UniStorm's moon. " +
                    "Each texture applied to the list will be used as a moon phase and be applied in order of the current moon phase. Each moon phase " +
                    "has an individual light intensity to allow each moon phase to give off different amounts of light."),
                   EditorStyles.toolbarButton, GUILayout.ExpandWidth(false));
                MoonPhaseList.DoLayoutList();
				EditorGUILayout.Space();

				GUIStyle TitleStyle = new GUIStyle();
				TitleStyle.fontStyle = FontStyle.Bold;
				TitleStyle.fontSize = 14;
				TitleStyle.alignment = TextAnchor.MiddleCenter;


                if (MoonPhaseIndexProp.intValue == self.MoonPhaseList.Count && self.MoonPhaseList.Count > 0)
                {
                    MoonPhaseIndexProp.intValue = self.MoonPhaseList.Count - 1;
                }

                if (self.MoonPhaseList.Count > 0)
                {
                    EditorGUILayout.LabelField(new GUIContent("Current Moon Phase", "Displays all moon phases by adjusting the slider. " +
                    "The Current Moon Phase also controls the moon phase UniStorm will start with."), TitleStyle);
                    GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1.0f);
                    EditorGUILayout.BeginVertical("Box");
                    GUI.backgroundColor = Color.white;
                    GUILayout.Space(5);
                    EditorGUILayout.LabelField(new GUIContent(self.MoonPhaseList[MoonPhaseIndexProp.intValue].MoonPhaseTexture), style, GUILayout.Height(50));
                    GUILayout.Space(5);
                    EditorGUILayout.EndVertical();
                }

                if (self.MoonPhaseList.Count-1 != 0 && self.MoonPhaseList.Count > 0)
                {
                    EditorGUILayout.Space();
                    CustomIntSliderNoTooltipTest(new Rect(), new GUIContent(), MoonPhaseIndexProp, "", 0, self.MoonPhaseList.Count - 1);
                }

                if (self.MoonPhaseList.Count == 0)
                {
                    GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                    EditorGUILayout.HelpBox("There are currently no moon phases. To creat one, " +
                        "press the + sign on the list above and assign a texture to the newly created slot.", MessageType.Warning, true);
                    GUI.backgroundColor = Color.white;
                }

                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(MoonPhaseColorProp,
                    new GUIContent("Moon Phase Color", "A gradient that controls the color of UniStorm's moon phases."));  

                EditorGUILayout.Space();
                CustomFloatSlider(new Rect(), new GUIContent(), MoonBrightnessProp, 
                    new GUIContent("Moon Phase Brightness", "Controls the brightness of all moon phase textures."), 0.25f, 1.0f);

                EditorGUILayout.Space();
                EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.EndVertical();
			GUI.backgroundColor = Color.white;

			//Atmosphere Settings
			if (!AtmosphereFoldoutProp.boolValue)
			{
				GUI.backgroundColor = new Color(0.5f,0.5f,0.5f,1.0f);
			}

			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(10);
			EditorGUILayout.BeginVertical();

			AtmosphereFoldoutProp.boolValue = Foldout(AtmosphereFoldoutProp.boolValue, "Atmosphere Settings", true, myFoldoutStyle);
			GUI.backgroundColor = Color.white;

			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();

			if (AtmosphereFoldoutProp.boolValue)
			{
				EditorGUILayout.BeginHorizontal();
				GUILayout.Space(15);
				EditorGUILayout.BeginVertical();

                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(HemisphereProp,
                    new GUIContent("Hemisphere", "Controls whether UniStorm's seasons are calculated in either the Northern or Southern Hemisphere."));

                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(AtmosphereThicknessProp,
                    new GUIContent("Atmosphere Thickness", "Controls the thickness of UniStorm's atmosphere for each time of day. This is mainly used for sunrises and sunsets. " +
                    "The higher the value, the darker and more red the sunrises and sunsets will be."));       

                EditorGUILayout.Space();
				EditorGUILayout.PropertyField(AmbientSkyLightColorProp, 
                    new GUIContent("Ambient Sky Color", "A gradient that controls the Ambient Sky Color during non-precipitation weather types. Each element is a transition into the next time of day."));
                EditorGUILayout.PropertyField(StormyAmbientSkyLightColorProp,
                    new GUIContent("Stormy Ambient Sky Color", "A gradient that controls the Ambient Sky Color during precipitation weather types. Each element is a transition into the next time of day."));

                EditorGUILayout.Space();
				EditorGUILayout.PropertyField(AmbientEquatorLightColorProp,
                    new GUIContent("Ambient Equator Color", "A gradient that controls the Ambient Equator Color during non-precipitation weather types. Each element is a transition into the next time of day."));
                EditorGUILayout.PropertyField(StormyAmbientEquatorLightColorProp,
                    new GUIContent("Stormy Ambient Equator Color", "A gradient that controls the Ambient Equator Color during precipitation weather types. Each element is a transition into the next time of day."));

                EditorGUILayout.Space();
				EditorGUILayout.PropertyField(AmbientGroundLightColorProp,
                    new GUIContent("Ambient Ground Color", "A gradient that controls the Ambient Ground Color during non-precipitation weather types. Each element is a transition into the next time of day."));
                EditorGUILayout.PropertyField(StormyAmbientGroundLightColorProp,
                    new GUIContent("Stormy Ambient Ground Color", "A gradient that controls the Ambient Ground Color during precipitation weather types. Each element is a transition into the next time of day."));

                EditorGUILayout.Space();
				EditorGUILayout.PropertyField(SkyColorProp,
                    new GUIContent("Sky Color", "A gradient that controls the overall sky color of UniStorm's Skybox. " +
                    "Each element is a transition into the next time of day."));

                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(SkyTintColorProp,
                    new GUIContent("Sky Tint Color", "A gradient that controls the tint color of UniStorm's Skybox. " +
                    "Each element is a transition into the next time of day."));

                EditorGUILayout.Space();
				EditorGUILayout.PropertyField(StarLightColorProp, 
                    new GUIContent("Starlight Color", "A gradient that controls the color and transparency of UniStorm's stars. Each element is a transition into the next time of day."));

                EditorGUILayout.Space();
                CustomFloatSlider(new Rect(), new GUIContent(), StarSpeedProp,
                    new GUIContent("Star Speed", "Controls how fast the stars will move in the sky at night."), 0, 3.0f);

                EditorGUILayout.Space();
                EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.EndVertical();
			GUI.backgroundColor = Color.white;
		}


        //Settings
        if (TabNumberProp.intValue == 4)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical("Box", GUILayout.Width(90 * Screen.width / 100));

            if (GUILayout.Button(new GUIContent(HelpIcon), HelpStyle, GUILayout.ExpandWidth(true), GUILayout.Height(22.5f)))
            {
                Application.OpenURL("https://docs.google.com/document/d/1uL_oMqHC_EduRGEnOihifwcpkQmWX9rubGw8qjkZ4b4/edit#heading=h.abpb6eb0nbfc");
            }

            var style = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
            EditorGUILayout.LabelField(new GUIContent(SettingsIcon), style, GUILayout.ExpandWidth(true), GUILayout.Height(32));
            EditorGUILayout.LabelField("Settings", style, GUILayout.ExpandWidth(true));

            GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f, 1);
            EditorGUILayout.LabelField("The Settings allow you to control various system related settings. Currently, the UniStorm Profile Manager is the only available setting, " +
                "which is used to import or export UniStorm profiles to overwrite or export your current settings.", EditorStyles.helpBox);

            GUI.backgroundColor = new Color(1f, 1, 0.5f, 0.5f);
            EditorGUILayout.LabelField("If you need help, you can hover over each field with your mouse for a tooltip description. " +
                "Documentation for this section can be found by pressing the ? in the upper right hand corner.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;

            GUILayout.Space(5);

            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            //Settings
            if (!SettingsFoldoutProp.boolValue)
            {
                GUI.backgroundColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
            }

            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical();

            SettingsFoldoutProp.boolValue = Foldout(SettingsFoldoutProp.boolValue, "Profile Management", true, myFoldoutStyle);
            GUI.backgroundColor = Color.white;

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            if (SettingsFoldoutProp.boolValue)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(15);
                EditorGUILayout.BeginVertical();
                GUILayout.Space(10);

                CustomObjectProfileField(new Rect(), new GUIContent(), UniStormProfileProp, new GUIContent("UniStorm Profile", "Allows the importing of UniStorm settings from a profile object."), typeof(UniStormProfile), true);

                GUILayout.Space(3);
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(Application.isPlaying);
                if (GUILayout.Button("Import Profile"))
                {
                    if (self.m_UniStormProfile != null)
                    {
                        if (EditorUtility.DisplayDialog("UniStorm Profile Importer", "Are you sure you'd like to import settings from the UniStorm Profile " + "''" +
                            self.m_UniStormProfile.name + "''" + "? This process cannot be undone.", "Yes", "Cancel"))
                        {

                            UniStormProfile Temp_UniStormProfile = self.m_UniStormProfile;

                            self.SunColor = UpdateGradient(Temp_UniStormProfile.SunColor, self.SunColor);
                            SunColorProp = serializedObject.FindProperty("SunColor").Copy();

                            self.StormySunColor = UpdateGradient(Temp_UniStormProfile.StormySunColor, self.StormySunColor);
                            StormySunColorProp = serializedObject.FindProperty("StormySunColor").Copy();

                            self.MoonColor = UpdateGradient(Temp_UniStormProfile.MoonColor, self.MoonColor);
                            MoonColorProp = serializedObject.FindProperty("MoonColor").Copy();

                            self.SkyTintColor = UpdateGradient(Temp_UniStormProfile.SkyTintColor, self.SkyTintColor);
                            SkyTintColorProp = serializedObject.FindProperty("SkyTintColor").Copy();

                            self.SkyColor = UpdateGradient(Temp_UniStormProfile.SkyColor, self.SkyColor);
                            SkyColorProp = serializedObject.FindProperty("SkyColor").Copy();

                            self.AmbientSkyLightColor = UpdateGradient(Temp_UniStormProfile.AmbientSkyLightColor, self.AmbientSkyLightColor);
                            AmbientSkyLightColorProp = serializedObject.FindProperty("AmbientSkyLightColor").Copy();

                            self.StormyAmbientSkyLightColor = UpdateGradient(Temp_UniStormProfile.StormyAmbientSkyLightColor, self.StormyAmbientSkyLightColor);
                            StormyAmbientSkyLightColorProp = serializedObject.FindProperty("StormyAmbientSkyLightColor").Copy();

                            self.AmbientEquatorLightColor = UpdateGradient(Temp_UniStormProfile.AmbientEquatorLightColor, self.AmbientEquatorLightColor);
                            AmbientEquatorLightColorProp = serializedObject.FindProperty("AmbientEquatorLightColor").Copy();

                            self.StormyAmbientEquatorLightColor = UpdateGradient(Temp_UniStormProfile.StormyAmbientEquatorLightColor, self.StormyAmbientEquatorLightColor);
                            StormyAmbientEquatorLightColorProp = serializedObject.FindProperty("StormyAmbientEquatorLightColor").Copy();

                            self.AmbientGroundLightColor = UpdateGradient(Temp_UniStormProfile.AmbientGroundLightColor, self.AmbientGroundLightColor);
                            AmbientGroundLightColorProp = serializedObject.FindProperty("AmbientGroundLightColor").Copy();

                            self.StormyAmbientGroundLightColor = UpdateGradient(Temp_UniStormProfile.StormyAmbientGroundLightColor, self.StormyAmbientGroundLightColor);
                            StormyAmbientGroundLightColorProp = serializedObject.FindProperty("StormyAmbientGroundLightColor").Copy();

                            self.StarLightColor = UpdateGradient(Temp_UniStormProfile.StarLightColor, self.StarLightColor);
                            StarLightColorProp = serializedObject.FindProperty("StarLightColor").Copy();

                            self.FogColor = UpdateGradient(Temp_UniStormProfile.FogColor, self.FogColor);
                            FogColorProp = serializedObject.FindProperty("FogColor").Copy();

                            self.FogStormyColor = UpdateGradient(Temp_UniStormProfile.FogStormyColor, self.FogStormyColor);
                            FogStormyColorProp = serializedObject.FindProperty("FogStormyColor").Copy();

                            self.CloudLightColor = UpdateGradient(Temp_UniStormProfile.CloudLightColor, self.CloudLightColor);
                            CloudLightColorProp = serializedObject.FindProperty("CloudLightColor").Copy();

                            self.CloudBaseColor = UpdateGradient(Temp_UniStormProfile.CloudBaseColor, self.CloudBaseColor);
                            CloudBaseColorProp = serializedObject.FindProperty("CloudBaseColor").Copy();

                            self.CloudStormyBaseColor = UpdateGradient(Temp_UniStormProfile.CloudStormyBaseColor, self.CloudStormyBaseColor);
                            CloudStormyBaseColorProp = serializedObject.FindProperty("CloudStormyBaseColor").Copy();

                            self.SunIntensityCurve = new AnimationCurve(Temp_UniStormProfile.SunIntensityCurve.keys);
                            SunIntensityCurveProp = serializedObject.FindProperty("SunIntensityCurve").Copy();

                            self.MoonIntensityCurve = new AnimationCurve(Temp_UniStormProfile.MoonIntensityCurve.keys);
                            MoonIntensityCurveProp = serializedObject.FindProperty("MoonIntensityCurve").Copy();

                            self.AtmosphereThickness = new AnimationCurve(Temp_UniStormProfile.AtmosphereThickness.keys);
                            AtmosphereThicknessProp = serializedObject.FindProperty("AtmosphereThickness").Copy();

                            serializedObject.ApplyModifiedProperties();
                            Importing = true;
                            startVal = (float)EditorApplication.timeSinceStartup;
                        }
                    }
                    else
                    {
                        if (EditorUtility.DisplayDialog("UniStorm Profile Importer - Error", "There was no UniStorm Profile found in the UniStorm Profile slot. Please apply one and try again.", "Okay"))
                        {

                        }
                    }
                }
                EditorGUI.EndDisabledGroup();

                if (GUILayout.Button("Export Profile"))
                {
                    self.FilePath = EditorUtility.SaveFilePanelInProject("Save as UniStorm Profile", "New UniStorm Profile", "asset", "Please enter a file name to save the file to");

                    if (self.FilePath != string.Empty)
                    {
                        UniStormProfile Temp_UniStormProfile = ScriptableObject.CreateInstance<UniStormProfile>();
                        UniStormSystem Temp_UniStormSystem = self;

                        Temp_UniStormProfile.SkyTintColor = UpdateGradient(self.SkyTintColor, Temp_UniStormProfile.SkyTintColor);
                        Temp_UniStormProfile.SkyColor = UpdateGradient(self.SkyColor, Temp_UniStormProfile.SkyColor);
                        Temp_UniStormProfile.SunColor = UpdateGradient(self.SunColor, Temp_UniStormProfile.SunColor);
                        Temp_UniStormProfile.StormySunColor = UpdateGradient(self.StormySunColor, Temp_UniStormProfile.StormySunColor);
                        Temp_UniStormProfile.MoonColor = UpdateGradient(self.MoonColor, Temp_UniStormProfile.MoonColor);
                        Temp_UniStormProfile.AmbientSkyLightColor = UpdateGradient(self.AmbientSkyLightColor, Temp_UniStormProfile.AmbientSkyLightColor);
                        Temp_UniStormProfile.StormyAmbientSkyLightColor = UpdateGradient(self.StormyAmbientSkyLightColor, Temp_UniStormProfile.StormyAmbientSkyLightColor);
                        Temp_UniStormProfile.AmbientEquatorLightColor = UpdateGradient(self.AmbientEquatorLightColor, Temp_UniStormProfile.AmbientEquatorLightColor);
                        Temp_UniStormProfile.StormyAmbientEquatorLightColor = UpdateGradient(self.StormyAmbientEquatorLightColor, Temp_UniStormProfile.StormyAmbientEquatorLightColor);
                        Temp_UniStormProfile.AmbientGroundLightColor = UpdateGradient(self.AmbientGroundLightColor, Temp_UniStormProfile.AmbientGroundLightColor);
                        Temp_UniStormProfile.StormyAmbientGroundLightColor = UpdateGradient(self.StormyAmbientGroundLightColor, Temp_UniStormProfile.StormyAmbientGroundLightColor);
                        Temp_UniStormProfile.StarLightColor = UpdateGradient(self.StarLightColor, Temp_UniStormProfile.StarLightColor);
                        Temp_UniStormProfile.FogColor = UpdateGradient(self.FogColor, Temp_UniStormProfile.FogColor);
                        Temp_UniStormProfile.FogStormyColor = UpdateGradient(self.FogStormyColor, Temp_UniStormProfile.FogStormyColor);
                        Temp_UniStormProfile.CloudLightColor = UpdateGradient(self.CloudLightColor, Temp_UniStormProfile.CloudLightColor);
                        Temp_UniStormProfile.CloudBaseColor = UpdateGradient(self.CloudBaseColor, Temp_UniStormProfile.CloudBaseColor);
                        Temp_UniStormProfile.CloudStormyBaseColor = UpdateGradient(self.CloudStormyBaseColor, Temp_UniStormProfile.CloudStormyBaseColor);
                        Temp_UniStormProfile.SunIntensityCurve = new AnimationCurve(Temp_UniStormSystem.SunIntensityCurve.keys);
                        Temp_UniStormProfile.MoonIntensityCurve = new AnimationCurve(Temp_UniStormSystem.MoonIntensityCurve.keys);
                        Temp_UniStormProfile.AtmosphereThickness = new AnimationCurve(Temp_UniStormSystem.AtmosphereThickness.keys);

                        AssetDatabase.CreateAsset(Temp_UniStormProfile, self.FilePath);
                    }
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
        }

#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
        }
#endif

        serializedObject.ApplyModifiedProperties();
	}

    void Update ()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            if (progress < secs && Importing)
            {
                EditorUtility.DisplayProgressBar("UniStorm Profile Importer", "Importing UniStorm Profile settings...", progress / secs);

                if (Importing && progress / secs > 0.99f)
                {
                    if (EditorUtility.DisplayDialog("UniStorm Profile Importer", "The UniStorm Profile settings have been successfully imported.", "Okay"))
                    {
                        Importing = false;
                        EditorUtility.ClearProgressBar();
                        Selection.activeObject = null;
                    }
                    else
                    {
                        Importing = false;
                        EditorUtility.ClearProgressBar();
                        Selection.activeObject = null;
                    }
                }
            }
            else if (Importing && progress >= secs)
            {
                EditorUtility.ClearProgressBar();
            }

            progress = (float)(EditorApplication.timeSinceStartup - startVal);
        }
#endif
    }

    Gradient UpdateGradient (Gradient Reference, Gradient GradientToUpdate)
    {
        GradientToUpdate = new Gradient();
        GradientColorKey[] m_ColorKey;
        GradientAlphaKey[] m_AlphaKey;
        m_ColorKey = new GradientColorKey[Reference.colorKeys.Length];
        m_ColorKey = Reference.colorKeys;
        m_AlphaKey = new GradientAlphaKey[Reference.alphaKeys.Length];
        m_AlphaKey = Reference.alphaKeys;
        GradientToUpdate.SetKeys(m_ColorKey, m_AlphaKey);
        return GradientToUpdate;
    }

    //Custom functions for handling serialized properties.
    void CustomIntSlider (Rect position, GUIContent label, SerializedProperty property, GUIContent NameAndTip, int MinValue, int MaxValue) {
		label = EditorGUI.BeginProperty (position, label, property);
		EditorGUI.BeginChangeCheck ();
		var newValue = EditorGUILayout.IntSlider (NameAndTip, property.intValue, MinValue, MaxValue);
		if (EditorGUI.EndChangeCheck ())
			property.intValue = newValue;

		EditorGUI.EndProperty ();
	}

    void CustomIntSliderNoTooltip(Rect position, GUIContent label, SerializedProperty property, string Name, int MinValue, int MaxValue)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        EditorGUI.BeginChangeCheck();
        var newValue = EditorGUILayout.IntSlider(Name, property.intValue, MinValue, MaxValue);
        if (EditorGUI.EndChangeCheck())
            property.intValue = newValue;

        EditorGUI.EndProperty();
    }

    void CustomIntSliderNoTooltipTest(Rect position, GUIContent label, SerializedProperty property, string Name, int MinValue, int MaxValue)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        EditorGUI.BeginChangeCheck();
        var newValue = EditorGUILayout.IntSlider(Name, property.intValue, MinValue, MaxValue);
        if (EditorGUI.EndChangeCheck())
            property.intValue = newValue;

        EditorGUI.EndProperty();
    }

    void CustomFloatSlider (Rect position, GUIContent label, SerializedProperty property, GUIContent NameAndTip, float MinValue, float MaxValue) {
		label = EditorGUI.BeginProperty (position, label, property);
		EditorGUI.BeginChangeCheck ();
		var newValue = EditorGUILayout.Slider (NameAndTip, property.floatValue, MinValue, MaxValue);
		if (EditorGUI.EndChangeCheck ())
			property.floatValue = newValue;

		EditorGUI.EndProperty ();
	}

	public static bool Foldout(bool foldout, GUIContent content, bool toggleOnLabelClick, GUIStyle style){
		Rect position = GUILayoutUtility.GetRect(40f, 40f, 16f, 16f, style);
		return EditorGUI.Foldout(position, foldout, content, toggleOnLabelClick, style);
	}

	public static bool Foldout(bool foldout, string content, bool toggleOnLabelClick, GUIStyle style){
		return Foldout(foldout, new GUIContent(content), toggleOnLabelClick, style);
	}

    void CustomObjectField(Rect position, GUIContent label, SerializedProperty property, GUIContent NameAndTip, Type typeOfObject, bool IsEssential)
    {
        if (IsEssential && property.objectReferenceValue == null)
        {
            GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
            EditorGUILayout.LabelField("This field cannot be left blank", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
        }

        label = EditorGUI.BeginProperty(position, label, property);
        EditorGUI.BeginChangeCheck();
        var newValue = EditorGUILayout.ObjectField(NameAndTip, property.objectReferenceValue, typeOfObject, true);

        if (EditorGUI.EndChangeCheck())
            property.objectReferenceValue = newValue;

        EditorGUI.EndProperty();
    }

    void CustomObjectProfileField(Rect position, GUIContent label, SerializedProperty property, GUIContent NameAndTip, Type typeOfObject, bool IsEssential)
    {
        if (IsEssential && property.objectReferenceValue == null)
        {
            GUI.backgroundColor = new Color(1f, 1, 0.5f, 0.5f);
            EditorGUILayout.LabelField("If you are importing a UniStorm Profile, this field cannot be left unassigned.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
        }

        label = EditorGUI.BeginProperty(position, label, property);
        EditorGUI.BeginChangeCheck();
        var newValue = EditorGUILayout.ObjectField(NameAndTip, property.objectReferenceValue, typeOfObject, true);

        if (EditorGUI.EndChangeCheck())
            property.objectReferenceValue = newValue;

        EditorGUI.EndProperty();
    }

    private LayerMask FieldToLayerMask(int field)
    {
        LayerMask mask = 0;
        var layers = InternalEditorUtility.layers;
        for (int c = 0; c < layers.Length; c++)
        {
            if ((field & (1 << c)) != 0)
            {
                mask |= 1 << LayerMask.NameToLayer(layers[c]);
            }
        }
        return mask;
    }

    private int LayerMaskToField(LayerMask mask)
    {
        int field = 0;
        var layers = InternalEditorUtility.layers;
        for (int c = 0; c < layers.Length; c++)
        {
            if ((mask & (1 << LayerMask.NameToLayer(layers[c]))) != 0)
            {
                field |= 1 << c;
            }
        }
        return field;
    }

    void CustomTagField(Rect position, GUIContent label, SerializedProperty property, string Name)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        EditorGUI.BeginChangeCheck();
        var newValue = EditorGUILayout.TagField(label, property.stringValue);
        if (EditorGUI.EndChangeCheck())
            property.stringValue = newValue;

        EditorGUI.EndProperty();
    }
}
