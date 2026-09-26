using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class MainMenuModes
{
    public Mode mode;
    public Image button;
    public string visual;
}

[System.Serializable]
public class MainMenuDifficulty
{
    public Difficulty difficulty;
    public Image button;
}

public class NewGameMenu : MonoBehaviour
{
    public static NewGameMenu instance {get; private set;}

    [Header("Visuals")]
    public GameObject window;
    public bool isOpen;

    [Header("Values")]
    public Mode currentMode;
    public int modeId;
    public Difficulty currentDifficulty;
    public int difficultyId;

    [Header("Mode")]
    public MainMenuModes[] mainMenuModes;
    public TextMeshProUGUI modeVisual;
    public Sprite[] buttonStates;
    public GameObject[] mods;

    [Header("Difficulty")]
    public MainMenuDifficulty[] mainMenuDifficulty;
    public DifficultyInfo difficultyInfo;
    public GameObject returnButton;
    public GameObject reportReturnButton;
    public TextMeshProUGUI difficultyVisual;

    [Header("Modes")]
    public GameObject modificationReturnButton;

    [Header("For the OptionSelection")]
    public OptionSelection optionSelection;
    public int newGameId = 1;
    public int settingsId = 2;

    [Header("Making Sure")]
    public GameObject confirmWindow;
    
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        window.SetActive(false);
        confirmWindow.SetActive(false);
    }
    
    public void BothWindow()
    {
        isOpen = !isOpen;

        if(isOpen) OpenWindow();
        else CloseWindow();
    }

    public void OpenWindow()
    {
        isOpen = true;
        window.SetActive(true);
        Settings.instance.CloseWindow();
        EndlessModifications.instance.CloseWindow();
        optionSelection.buttonScripts[newGameId].StayOn();
        optionSelection.buttonScripts[settingsId].StayOff();
        returnButton.SetActive(true);
        reportReturnButton.SetActive(false);
        modificationReturnButton.SetActive(false);
        CloseConfirmWindow();
        UpdateVisuals();
    }

    public void CloseWindow()
    {
        isOpen = false;
        window.SetActive(false);
        EndlessModifications.instance.CloseWindow();
        optionSelection.buttonScripts[newGameId].StayOff();
        CloseConfirmWindow();
    }

    void UpdateVisuals()
    {
        ResetAll();

        mainMenuModes[modeId].button.sprite = buttonStates[1];
        modeVisual.text = mainMenuModes[modeId].visual;
        currentMode = (Mode)modeId;

        mainMenuDifficulty[difficultyId].button.sprite = buttonStates[1];
        currentDifficulty = (Difficulty)difficultyId;

        mods[modeId].SetActive(true);
    }

    public void ResetAll()
    {
        foreach(var mode in mainMenuModes)
        {
            mode.button.sprite = buttonStates[0];
        }

        foreach(var difficulty in mainMenuDifficulty)
        {
            difficulty.button.sprite = buttonStates[0];
        }

        foreach(var mod in mods)
        {
            mod.SetActive(false);
        }

        CheckWinCon.instance.UpdateVisuals();
    }

    public void ChangeMode(int id)
    {
        if(id == CheckWinCon.instance.endlessModeId && !CheckWinCon.instance.canChooseEndless)
        {
            PopupText.instance.Popup(CheckWinCon.instance.disabledString);
            return;
        }

        modeId = id;
        UpdateVisuals();
    }

    public void ChangeDifficulty(int id)
    {
        difficultyId = id;
        UpdateVisuals();
    }

    public void OpenReport()
    {   
        difficultyInfo.OpenWindow();
        difficultyInfo.UpdateVisuals(difficultyId);
        difficultyVisual.text = $"- {currentDifficulty.ToString()} -";
        reportReturnButton.SetActive(true);
        returnButton.SetActive(false);
    }

    public void AttemptStartGame()
    {
        if(MenuSavingSystem.instance.newGame)
        {
            StartGame();
        }
        else
        {
            OpenConfirmWindow();
        }
    }

    public void OpenConfirmWindow()
    {
        confirmWindow.SetActive(true);
    }

    public void CloseConfirmWindow()
    {
        confirmWindow.SetActive(false);
    }

    public void StartGame()
    {
        StartCoroutine(GoToGame());
    }

    IEnumerator GoToGame()
    {
        BlackScreenTransition.instance.TransitionOut();
        MenuSavingSystem.instance.newGame = true;
        DataPersistenceManager.instance.NewGame();
        yield return new WaitForSecondsRealtime(BlackScreenTransition.instance.outDuration);
        yield return new WaitForEndOfFrame();
        SceneManager.LoadScene("Game");
    }
}