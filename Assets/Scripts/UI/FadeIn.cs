using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private float buffer;
    // Start is called before the first frame update
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        buffer = 1.15f;
        canvasGroup.alpha = buffer;
        if (canvasGroup.interactable)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        buffer -= 0.005f;
        canvasGroup.alpha = buffer;
        if (canvasGroup.alpha <= 0)
        {
            this.transform.parent.gameObject.SetActive(false);
        }

    }
}
