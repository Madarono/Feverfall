using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionSelection : MonoBehaviour
{
    public Image[] buttons;
    public SelectionButton[] buttonScripts;
    public TextMeshProUGUI[] visuals;
    public Sprite[] buttonStates;
    public Color[] textStates;
    public float amplifyHoverSound = 0.4f;

    [Header("Animation")]
    public float animationTime = 0.15f;
    public Vector3 scaleMax = new Vector3(1.1f, 1.1f, 1.1f);
    public Vector3 scaleMin = Vector3.one;
    private Coroutine[] buttonCoroutines;

    [Header("Continue Button")]
    public Sprite disabledSprite;
    public GameObject daysCounter;
    public TextMeshProUGUI daysVisual;

    void Awake()
    {
        if (buttons != null)
        {
            buttonCoroutines = new Coroutine[buttons.Length];
        }
    }

    void Start()
    {
        DeselectAllButtons();
        CheckContinueButton();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void SelectButton(int id, bool sound = true)
    {
        if (!IsValidIndex(id)) return;

        bool wasOn = buttons[id].sprite == buttonStates[1];

        buttons[id].sprite = buttonStates[1];
        if (visuals != null && visuals.Length > id && visuals[id] != null)
        {
            visuals[id].color = textStates[1];
        }

        if (buttonCoroutines[id] != null)
        {
            StopCoroutine(buttonCoroutines[id]);
        }

        buttonCoroutines[id] = StartCoroutine(AnimateButton(id, scaleMax));
        
        if (sound && !wasOn && AudioManager.instance != null)
        {
            AudioManager.instance.Play(AudioManager.instance.hoverButton, amplifyHoverSound);
        }
    }

    public void DeselectButton(int id)
    {
        if (!IsValidIndex(id)) return;
        if (buttonScripts != null && buttonScripts.Length > id && buttonScripts[id] != null && buttonScripts[id].stayOn) return;

        buttons[id].sprite = buttonStates[0];
        if (visuals != null && visuals.Length > id && visuals[id] != null)
        {
            visuals[id].color = textStates[0];
        }

        if (buttonCoroutines[id] != null)
        {
            StopCoroutine(buttonCoroutines[id]);
        }

        buttonCoroutines[id] = StartCoroutine(AnimateButton(id, scaleMin));
    }

    void DeselectAllButtons()
    {
        if (buttons == null) return;

        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] == null) continue;

            buttons[i].sprite = buttonStates[0];
            if (visuals != null && visuals.Length > i && visuals[i] != null)
            {
                visuals[i].color = textStates[0];
            }
            buttons[i].transform.localScale = scaleMin;
        }
    }

    public void CheckContinueButton()
    {
        if (buttons == null || buttons.Length == 0 || buttons[0] == null) return;
        if (MenuSavingSystem.instance == null) return;

        bool isNewGame = MenuSavingSystem.instance.newGame;

        buttons[0].transform.localScale = scaleMin;
        buttons[0].sprite = isNewGame ? disabledSprite : buttonStates[0];

        if (visuals != null && visuals.Length > 0 && visuals[0] != null)
        {
            visuals[0].color = isNewGame ? textStates[2] : textStates[0];
        }

        if (daysCounter != null)
        {
            daysCounter.SetActive(!isNewGame);
        }

        if (daysVisual != null)
        {
            daysVisual.text = $"Day {MenuSavingSystem.instance.totalDays}";
        }
    }

    IEnumerator AnimateButton(int id, Vector3 targetScale)
    {
        if (!IsValidIndex(id)) yield break;

        Transform btnTransform = buttons[id].transform;
        Vector3 startScale = btnTransform.localScale;

        float currentDistance = Vector3.Distance(startScale, targetScale);
        float maxDistance = Vector3.Distance(scaleMin, scaleMax);
        float adjustedDuration = animationTime * (maxDistance > 0 ? (currentDistance / maxDistance) : 1f);

        float t = 0;

        while (t < adjustedDuration)
        {
            if (btnTransform == null) yield break;

            t += Time.unscaledDeltaTime;
            btnTransform.localScale = Vector3.Lerp(startScale, targetScale, adjustedDuration > 0 ? t / adjustedDuration : 1f);
            yield return null;
        }

        if (btnTransform != null)
        {
            btnTransform.localScale = targetScale;
        }

        if (buttonCoroutines != null && id < buttonCoroutines.Length)
        {
            buttonCoroutines[id] = null;
        }
    }

    private bool IsValidIndex(int id)
    {
        return buttons != null && id >= 0 && id < buttons.Length && buttons[id] != null;
    }
}