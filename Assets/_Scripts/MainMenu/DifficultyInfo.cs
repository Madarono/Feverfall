using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

[System.Serializable]
public class DifficultyReport
{
    public Difficulty difficulty;

    [Header("Virus Power")]
    public string virusPower;

    [Header("Market")]
    public string marketDemand;

    [Header("Shop Discount")]
    public string shopDiscount;

    [Header("Starting Cash")]
    public string startingCash;

    [Header("Penalty Discount")]
    public string penaltyDiscount;
}

public class DifficultyInfo : MonoBehaviour
{
    public GameObject window;
    public bool isOpen;

    public DifficultyReport[] reports;

    public TextMeshProUGUI virusVisual;    
    public TextMeshProUGUI marketVisual;    
    public TextMeshProUGUI shopVisual;    
    public TextMeshProUGUI startingVisual;    
    public TextMeshProUGUI penaltyVisual;

    void Start()
    {
        CloseWindow();
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
    }

    public void CloseWindow()
    {
        isOpen = false;
        window.SetActive(false);
        NewGameMenu.instance.reportReturnButton.SetActive(false);
        NewGameMenu.instance.returnButton.SetActive(true);
    }

    public void UpdateVisuals(int id)
    {
        virusVisual.text = $"Virus Power: {reports[id].virusPower}";
        startingVisual.text = $"Starting Cash: {reports[id].startingCash}";
        marketVisual.text = $"Market Demand: {reports[id].marketDemand}";
        shopVisual.text = $"Shop Multiplier: {reports[id].shopDiscount}";
        penaltyVisual.text = $"Penalty Multiplier: {reports[id].startingCash}";
    }    
}