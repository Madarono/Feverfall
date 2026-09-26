using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class MenuNewGame : MonoBehaviour
{
    public static MenuNewGame instance {get; private set;}
    public OptionSelection optionSelection;

    void Awake()
    {
        instance = this;
    }

    public void InitializeNewGame()
    {
        Settings.instance.sfxValue = 100f;
        Settings.instance.musicValue = 100f;
        Settings.instance.muteSfx = false;
        Settings.instance.muteMusic = false;
        Settings.instance.graphicsIndex = 2;
        Settings.instance.canScreenShake = true;
        Settings.instance.fpsIndex = 1;
        Settings.instance.musicSlider.value = 100f * Settings.instance.audioDivider;
        Settings.instance.sfxSlider.value = 100f * Settings.instance.audioDivider;
        Settings.instance.UpdateValues();
        Settings.instance.SetFPS();
        Settings.instance.ApplyChanges();
        MenuSavingSystem.instance.newGame = true;
        AudioManager.instance.UpdateVolume();
        optionSelection.CheckContinueButton();
        MainMenuVillage.instance.BuildVillage();
        Time.timeScale = 1f;
    }
}