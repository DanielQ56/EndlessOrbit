using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreLine : MonoBehaviour
{
    [SerializeField] ParticleSystem left;
    [SerializeField] ParticleSystem right;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject HighScoreImage;
    [SerializeField] float effectTimer;
    // Start is called before the first frame update
    bool passed = false;

    private void OnEnable()
    {
        text.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        text.gameObject.SetActive(false);
    }

    public void HighScore()
    {
        if(!passed)
            StartCoroutine(PassedHighScoreLine());  
    }

    IEnumerator PassedHighScoreLine()
    {
        passed = true;
        HighScoreImage.SetActive(false);
        text.gameObject.SetActive(false);
        ParticleSystem.MainModule lmain = left.main;
        lmain.startColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        left.Play();
        ParticleSystem.MainModule rmain = right.main;
        rmain.startColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        right.Play();
        yield return new WaitForSeconds(effectTimer);
        passed = false;
        this.gameObject.SetActive(false);
    }
}
