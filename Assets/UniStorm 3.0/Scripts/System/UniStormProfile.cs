using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniStormProfile : ScriptableObject
{
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
    public AnimationCurve SunIntensityCurve = AnimationCurve.Linear(0, 0, 24, 5);
    public AnimationCurve MoonIntensityCurve = AnimationCurve.Linear(0, 0, 24, 5);
    public AnimationCurve AtmosphereThickness = AnimationCurve.Linear(0, 0, 24, 5);
}
