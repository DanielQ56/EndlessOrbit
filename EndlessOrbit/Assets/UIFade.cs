using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{
    [SerializeField] GameObject cover;
    private CanvasGroup canvas;

    void Awake()
    {
        canvas = this.GetComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        if (cover != null)
        {
            cover.SetActive(true);
        }
        canvas.alpha = 0.0f;
        StartCoroutine(FadeTo(1.0f, 0.25f));
    }

    //Fade to alpha value v in t time
    IEnumerator FadeTo(float v, float t)
    {
        float alpha = canvas.alpha;
        float timer = 0.0f;
        while(timer <= t)
        {
            canvas.alpha = Mathf.Lerp(alpha, v, timer / t);
            yield return null;
            timer += Time.deltaTime;
        }
        canvas.alpha = v;
    }

    void OnDisable()
    {
        if (cover != null)
        {
            cover.SetActive(false);
        }
    }
}
