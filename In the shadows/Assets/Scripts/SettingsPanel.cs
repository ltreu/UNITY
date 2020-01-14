using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
	public Toggle fullscreenToggle;
	public Slider soundVolSlider;
	public Slider musicVolSlider;
	public ProgressController controller;

	void OnEnable()
	{
		if (controller)
		{
			fullscreenToggle.isOn = controller.records.appSettings.fullscreen;
			musicVolSlider.value = controller.records.appSettings.musicVolume;
            soundVolSlider.value = controller.records.appSettings.soundVolume;
        }
	}
}
