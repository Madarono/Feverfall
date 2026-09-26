using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class MenuSavingSystem : MonoBehaviour, IDataPersistence
{
    public static MenuSavingSystem instance { get; private set; }

    [Header("Settings")]
    public float sfxValue;
    public float musicValue;
    public bool muteSfx;
    public bool muteMusic;
    public int graphicsIndex;
    public bool canScreenShake;
    public int fpsIndex;

    [Header("For Options")]
    public bool newGame;
    public int totalDays;

    public Mode mode;
    public Difficulty difficulty;

    [Header("For CheckWinCon")]
    public bool hasWon;
    public bool hasLost;
    public bool fromEnvelope;
    public Difficulty gameDifficulty;
    public Mode gameMode;
    public bool canChooseEndless;

    [Header("Endless Modifications")]
    public float o_VirusPower;
    public float o_ShopMultiplier;
    public float o_PenaltyMultiplier;
    public int o_StartingCash;
    public float o_ScaleRate;

    [Header("Main Menu Village")]
    public List<Vector2> roadPos = new List<Vector2>();
    public List<Vector3> buildingPos = new List<Vector3>();
    public List<int> buildingId = new List<int>();

    public List<int> motelTypeId = new List<int>();
    public List<Vector3> motelPos = new List<Vector3>();

    public List<int> workplaceTypeId = new List<int>();
    public List<Vector3> workplacePos = new List<Vector3>();

    public List<Vector3> villagerPos = new List<Vector3>();

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void SaveData(GameData data)
    {
        SaveInfo();

        data.sfxValue = this.sfxValue;
        data.musicValue = this.musicValue;
        data.muteSfx = this.muteSfx;
        data.muteMusic = this.muteMusic;
        data.graphicsIndex = this.graphicsIndex;
        data.canScreenShake = this.canScreenShake;
        data.fpsIndex = this.fpsIndex;
        data.newGame = this.newGame;
        data.mainMenuDifficulty = this.difficulty;
        data.mainMenuMode = this.mode;
        data.canChooseEndless = this.canChooseEndless;
        data.fromEnvelope = this.fromEnvelope;
        data.o_VirusPower = this.o_VirusPower;
        data.o_ShopMultiplier = this.o_ShopMultiplier;
        data.o_PenaltyMultiplier = this.o_PenaltyMultiplier;
        data.o_StartingCash = this.o_StartingCash;
        data.o_ScaleRate = this.o_ScaleRate;
    }

    public void LoadData(GameData data)
    {
        this.sfxValue = data.sfxValue;
        this.musicValue = data.musicValue;
        this.muteSfx = data.muteSfx;
        this.muteMusic = data.muteMusic;
        this.graphicsIndex = data.graphicsIndex;
        this.canScreenShake = data.canScreenShake;
        this.fpsIndex = data.fpsIndex;
        this.newGame = data.newGame;
        this.totalDays = data.totalDays;
        this.hasWon = data.hasWon;
        this.gameMode = data.mode;
        this.gameDifficulty = data.difficulty;
        this.canChooseEndless = data.canChooseEndless;
        this.fromEnvelope = data.fromEnvelope;
        this.hasLost = data.hasLost;

        this.workplaceTypeId = data.workplaceTypeId;
        this.workplacePos = data.workplacePos;
        this.motelTypeId = data.motelTypeId;
        this.motelPos = data.motelPos;
        this.roadPos = data.roadPos;
        this.villagerPos = data.villagerPos;

        LoadInfo();
    }

    public void SaveInfo()
    {
        if (Settings.instance == null) return;

        // Gathering the Settings Info
        sfxValue = Settings.instance.sfxValue;
        musicValue = Settings.instance.musicValue;
        muteSfx = Settings.instance.muteSfx;
        muteMusic = Settings.instance.muteMusic;
        graphicsIndex = Settings.instance.graphicsIndex;
        canScreenShake = Settings.instance.canScreenShake;
        fpsIndex = Settings.instance.fpsIndex;

        difficulty = NewGameMenu.instance.currentDifficulty;
        mode = NewGameMenu.instance.currentMode;

        canChooseEndless = CheckWinCon.instance.canChooseEndless;

        o_VirusPower = EndlessModifications.instance.o_VirusPower;
        o_ShopMultiplier = EndlessModifications.instance.o_ShopMultiplier;
        o_PenaltyMultiplier = EndlessModifications.instance.o_PenaltyMultiplier;
        o_StartingCash = EndlessModifications.instance.o_StartingCash;
        o_ScaleRate = EndlessModifications.instance.o_ScaleRate;
    }

    public void LoadInfo()
    {
        Settings.instance.sfxValue = sfxValue;
        Settings.instance.musicValue = musicValue;
        Settings.instance.muteSfx = muteSfx;
        Settings.instance.muteMusic = muteMusic;
        Settings.instance.graphicsIndex = graphicsIndex;
        Settings.instance.canScreenShake = canScreenShake;
        Settings.instance.fpsIndex = fpsIndex;

        Settings.instance.musicSlider.value = musicValue * Settings.instance.audioDivider;
        Settings.instance.sfxSlider.value = sfxValue * Settings.instance.audioDivider;

        Settings.instance.UpdateValues();
        Settings.instance.SetFPS();
        Settings.instance.ApplyChanges();

        CheckWinCon.instance.hasWon = hasWon;
        CheckWinCon.instance.mode = gameMode;
        CheckWinCon.instance.difficulty = gameDifficulty;
        CheckWinCon.instance.canChooseEndless = canChooseEndless;
        CheckWinCon.instance.Check();
        CheckWinCon.instance.UpdateVisuals();

        if (AudioManager.instance != null)
        {
            AudioManager.instance.UpdateVolume();
        }

        OptionSelection currentOptionSelection = FindFirstObjectByType<OptionSelection>();
        if (currentOptionSelection != null)
        {
            currentOptionSelection.CheckContinueButton();
        }

        Time.timeScale = 1f;

        //MainMenuVillage.cs
        foreach(var motel in motelTypeId)
        {
            buildingId.Add(motel);
        }
        foreach(var workplace in workplaceTypeId)
        {
            buildingId.Add(workplace);
        }

        foreach(var motelPos in motelPos)
        {
            buildingPos.Add(motelPos);
        }
        foreach(var workplacePos in workplacePos)
        {
            buildingPos.Add(workplacePos);
        }

        MainMenuVillage.instance.buildingPos = buildingPos;
        MainMenuVillage.instance.buildingId = buildingId;
        MainMenuVillage.instance.roadPos = roadPos;
        MainMenuVillage.instance.villagerPos = villagerPos;
        MainMenuVillage.instance.BuildVillage();

        CheckWin();
        CheckLoss();
    }

    public void CheckWin() //If Won from envelope, then make NewGames
    {
        if(hasWon && fromEnvelope)
        {
            DataPersistenceManager.instance.NewGame();
            fromEnvelope = false;
            hasWon = false;
        }
    }

    public void CheckLoss()
    {
        if(hasLost)
        {
            DataPersistenceManager.instance.NewGame();
            fromEnvelope = false;
            hasWon = false;
            hasLost = false;
        }
    }
}