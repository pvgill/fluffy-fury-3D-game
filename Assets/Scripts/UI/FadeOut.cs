using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private float buffer;
    public bool startFade = false;
    // Start is called before the first frame update
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        buffer = 0f;
        canvasGroup.alpha = buffer;
        if (canvasGroup.interactable)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        } 
    }

    // Update is called once per frame
    void Update()
    {
        if (startFade && buffer < 1)
        {
            buffer += 0.01f;
            canvasGroup.alpha = buffer;
        } 

    }
    public void SelfTerminate()
    {
        buffer = 0f;
        canvasGroup.alpha = 0f;
        startFade = false;
    }
}
