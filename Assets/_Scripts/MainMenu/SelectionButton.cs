using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SelectionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public OptionSelection option;
    public int id;
    public bool stayOn;

    [Header("Special")]
    public bool isContinue = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isContinue && MenuSavingSystem.instance.newGame)
        {
            return;
        }

        option.SelectButton(id);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(isContinue && MenuSavingSystem.instance.newGame)
        {
            return;
        }

        option.DeselectButton(id);
    }

    public void StayBoth()
    {
        if(isContinue && MenuSavingSystem.instance.newGame)
        {
            return;
        }


        stayOn = !stayOn;

        if(stayOn) option.SelectButton(id, false);
        else option.DeselectButton(id);
    }

    public void StayOn()
    {
        if(isContinue && MenuSavingSystem.instance.newGame)
        {
            return;
        }


        stayOn = true;
        option.SelectButton(id, false);
    }

    public void StayOff()
    {
        if(isContinue && MenuSavingSystem.instance.newGame)
        {
            return;
        }

        stayOn = false;
        option.DeselectButton(id);
    }

    public void Continue()
    {
        if(isContinue && MenuSavingSystem.instance.newGame)
        {
            PopupText.instance.Popup("You have no existing save file");
            return;
        }

        StartCoroutine(ContinueGame());
    }

    public void Quit()
    {
        DataPersistenceManager.instance.SaveGame();
        Application.Quit();
    }

    IEnumerator ContinueGame()
    {
        BlackScreenTransition.instance.TransitionOut();
        yield return new WaitForSecondsRealtime(BlackScreenTransition.instance.outDuration);
        MenuSavingSystem.instance.newGame = false;
        SceneManager.LoadScene("Game");
    }
}