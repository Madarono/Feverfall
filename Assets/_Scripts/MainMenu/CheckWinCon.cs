using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CheckWinCon : MonoBehaviour
{
    public static CheckWinCon instance {get; private set;}

    [Header("Values")]
    public bool hasWon;
    public Mode mode;
    public Difficulty difficulty;

    [Header("Visuals")]
    public Image endlessButton;
    public TextMeshProUGUI endlessVisual;
    public Sprite[] buttonStates;
    public Color[] visualStates;

    [Header("For NewGameMenu")]
    public bool canChooseEndless;
    public string disabledString;
    public int endlessModeId = 1;

    void Awake()
    {
        instance = this;
    }

    public void Check()
    {
        if(!hasWon || canChooseEndless) return; //Has to win or has already unlocked Endless Mode
        if(mode != Mode.Normal) return; //Can't be endless

        if(difficulty != Difficulty.Easy) //Normal or higher
        {
            canChooseEndless = true;
        }
    }

    public void UpdateVisuals()
    {
        if(endlessButton != null) endlessButton.sprite = canChooseEndless ? buttonStates[1] : buttonStates[0];
        if(endlessVisual != null) endlessVisual.color = canChooseEndless ? visualStates[1] : visualStates[0];
    }
}