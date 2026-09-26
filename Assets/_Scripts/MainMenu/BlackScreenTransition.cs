using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BlackScreenTransition : MonoBehaviour
{
    public static BlackScreenTransition instance { get; private set; }

    public GameObject blackScreenIn;
    public GameObject blackScreenOut;
    public float inDuration = 0.5f;
    public float outDuration = 2f;

    private Coroutine inCoroutine;
    private Coroutine outCoroutine;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        TransitionIn();
    }

    public void TransitionIn()
    {
        Debug.Log("TransitionIn");
        SoundFade.instance.FadeIn();
        
        if (inCoroutine != null) StopCoroutine(inCoroutine);
        inCoroutine = StartCoroutine(Transition(blackScreenIn, inDuration, true));
    }

    public void TransitionOut()
    {
        SoundFade.instance.FadeOut();

        if (outCoroutine != null) StopCoroutine(outCoroutine);
        outCoroutine = StartCoroutine(Transition(blackScreenOut, outDuration, false));
    }

    IEnumerator Transition(GameObject obj, float duration, bool hideOnComplete)
    {
        obj.SetActive(false);
        obj.SetActive(true);
        yield return new WaitForSecondsRealtime(duration);
        if (hideOnComplete) obj.SetActive(false);
    }
}