using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableWeatherByTrigger : MonoBehaviour {

    public string TriggerTag = "Player";
    public ControlEffectsEnum ControlEffects;
    public enum ControlEffectsEnum
    {
        Disable,
        Enable
    }
    public ControlSoundsEnum ControlSounds = ControlSoundsEnum.Yes;
    public enum ControlSoundsEnum
    {
        Yes,
        No
    }
	
	void OnTriggerEnter (Collider C)
    {
        if (C.tag == TriggerTag && ControlEffects == ControlEffectsEnum.Disable)
        {
            UniStormManager.Instance.ChangeWeatherEffectsState(false);
            if (ControlSounds == ControlSoundsEnum.Yes)
            {
                UniStormManager.Instance.ChangeWeatherSoundsState(false);
            }
        }
        else if (C.tag == TriggerTag && ControlEffects == ControlEffectsEnum.Enable)
        {
            UniStormManager.Instance.ChangeWeatherEffectsState(true);
            if (ControlSounds == ControlSoundsEnum.Yes)
            {
                UniStormManager.Instance.ChangeWeatherSoundsState(true);
            }
        }
    }
}
