using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndlessModifications : MonoBehaviour
{
    public static EndlessModifications instance {get; private set;}

    public GameObject window;
    public bool isOpen;

    [Header("Visuals")]
    public Slider virusSlider;
    public Slider shopSlider;
    public Slider penaltySlider;
    public Slider startingSlider;
    public Slider scaleSlider;

    public TextMeshProUGUI virusVisual;
    public TextMeshProUGUI shopVisual;
    public TextMeshProUGUI penaltyVisual;
    public TextMeshProUGUI startingVisual;
    public TextMeshProUGUI scaleVisual;

    [Header("Values")]
    public float o_VirusPower;
    public float o_ShopMultiplier;
    public float o_PenaltyMultiplier;
    public int o_StartingCash;
    public float o_ScaleRate;

    public bool hasAppliedValues = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateSliders();
    }

    public void OpenWindow()
    {
        isOpen = true;
        window.SetActive(true);
        NewGameMenu.instance.modificationReturnButton.SetActive(true);
        NewGameMenu.instance.reportReturnButton.SetActive(false);
        NewGameMenu.instance.returnButton.SetActive(false);
        UpdateVisuals();
    }

    public void CloseWindow()
    {
        isOpen = false;
        window.SetActive(false);
        NewGameMenu.instance.modificationReturnButton.SetActive(false);
        NewGameMenu.instance.returnButton.SetActive(true);
    }

    public void UpdateSliders()
    {
        virusSlider.value = o_VirusPower;
        shopSlider.value = o_ShopMultiplier;
        penaltySlider.value = o_PenaltyMultiplier;
        startingSlider.value = o_StartingCash;
        scaleSlider.value = o_ScaleRate / 100f;

        hasAppliedValues = true;
    }

    public void UpdateVisuals()
    {
        if(!hasAppliedValues)
        {
            UpdateSliders();
            return;
        }

        o_VirusPower = virusSlider.value;
        o_ShopMultiplier = shopSlider.value;
        o_PenaltyMultiplier = penaltySlider.value;
        o_StartingCash = Mathf.RoundToInt(startingSlider.value);
        o_ScaleRate = scaleSlider.value / 100f;

        virusVisual.text = $"{virusSlider.value:F1}x";
        shopVisual.text = $"{shopSlider.value:F1}x";
        penaltyVisual.text = $"{penaltySlider.value:F1}x";
        startingVisual.text = $"${startingSlider.value}";
        scaleVisual.text = $"{scaleSlider.value:F1}%";
    }

    public void ChangeValue() //For Sliders
    {
        UpdateVisuals();
    }
}