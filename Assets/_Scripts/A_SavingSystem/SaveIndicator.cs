using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SaveIndicator : MonoBehaviour
{
    public GameObject indicator;
    public float minimumDisplayTime = 0.4f;

    private Coroutine hideCoroutine;

    private void Start()
    {
        indicator.SetActive(false);

        if (DataPersistenceManager.instance != null)
        {
            DataPersistenceManager.instance.SaveStarted += Show;
            DataPersistenceManager.instance.SaveFinished += Hide;
        }
    }

    private void OnDestroy()
    {
        if (DataPersistenceManager.instance != null)
        {
            DataPersistenceManager.instance.SaveStarted -= Show;
            DataPersistenceManager.instance.SaveFinished -= Hide;
        }
    }

    private void Show()
    {
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }

        indicator.SetActive(true);
    }

    private void Hide()
    {
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        hideCoroutine = StartCoroutine(HideAfterMinimumTime());
    }

    private IEnumerator HideAfterMinimumTime()
    {
        yield return new WaitForSeconds(minimumDisplayTime);

        indicator.SetActive(false);
        hideCoroutine = null;
    }
}