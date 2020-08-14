using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DailyLoginBonus : MonoBehaviour
{
    [SerializeField] RectTransform SpinWheel;
    [SerializeField] Button SpinButton;
    [SerializeField] TextMeshProUGUI SpinButtonText;
    [SerializeField] Button BackButton;
    [SerializeField] TextMeshProUGUI RewardText;

    [SerializeField] float spinTimer;
    [SerializeField] float MaxSpinAngle;
    [SerializeField] float MinSpinAngle;

    [SerializeField] int MinRewardAmount;
    [SerializeField] int MaxRewardAmount;
    [SerializeField] List<TextMeshProUGUI> PartsOfWheel;

    bool spinning = false;

    float startingAngle;

    private void Awake()
    {
        SpinButton.onClick.AddListener(Spin);
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        RewardText.text = "Spin for Rewards!";
        startingAngle = Random.Range(0, 360);
        SpinWheel.rotation = Quaternion.Euler(new Vector3(0, 0, startingAngle));
        RandomizeAndAssignRewardValues();
    }

    private void Update()
    {
        if(!PlayerManager.instance.BonusAvailable())
        {
            if(SpinButton.interactable)
            {
                SpinButton.interactable = false;
            }
            System.TimeSpan timeDiff = (PlayerManager.instance.GetNextBonus() - System.DateTime.Now);
            SpinButtonText.text = string.Format("{0:00}:{1:00}:{2:00}", timeDiff.Hours, timeDiff.Minutes, timeDiff.Seconds);
           
        }
        else if(!spinning)
        {
            if(!SpinButton.interactable)
            {
                SpinButton.interactable = true;
            }
            if(SpinButtonText.text != "Spin!")
            {
                SpinButtonText.text = "Spin!";
            }
        }
    }

    void RandomizeAndAssignRewardValues()
    {
        List<int> rewardValues = new List<int>();
        do
        {
            int val = Random.Range(MinRewardAmount, MaxRewardAmount);
            if(!rewardValues.Contains(val))
            {
                rewardValues.Add(val);
            }
        } while (rewardValues.Count < 8);

        for(int i = 0; i < 8; ++i)
        {
            PartsOfWheel[i].text = rewardValues[i].ToString();
        }

    }


    public void Spin()
    {
        SpinButton.interactable = false;
        StartCoroutine(SpinRoutine());
    }


    IEnumerator SpinRoutine()
    {
        float spinAngle = Random.Range(MinSpinAngle, MaxSpinAngle);
        spinning = true;
        BackButton.interactable = false;
        float timer = 0f;
        while(timer < spinTimer/3f)
        {
            SpinWheel.Rotate(Vector3.forward * Mathf.Lerp(0, spinAngle, timer / (spinTimer/3f)), Space.Self);
            yield return null;
            timer += Time.deltaTime;
        }
        timer = 0f;
        while (timer < (spinTimer/3f))
        {
            SpinWheel.Rotate(Vector3.forward * spinAngle, Space.Self);
            yield return null;
            timer += Time.deltaTime;
        }
        timer = 0f;
        while (timer < spinTimer / 3f)
        {
            SpinWheel.Rotate(Vector3.forward * Mathf.Lerp(spinAngle, 0, timer / (spinTimer / 3f)), Space.Self);
            yield return null;
            timer += Time.deltaTime;
        }
        Debug.Log(SpinWheel.eulerAngles.z);
        int index = Mathf.FloorToInt((Mathf.Abs(SpinWheel.eulerAngles.z) + (SpinWheel.eulerAngles.z < 0 ? 180f : 0f)) / 45f);
        int value = int.Parse(PartsOfWheel[index].text);
        Debug.Log("Index: " + index);
        RewardText.text = string.Format("Earned {0} stars!", value);
        PlayerManager.instance.AddStars(value);
        PlayerManager.instance.JustReceivedBonus();
        SpinButton.interactable = PlayerManager.instance.BonusAvailable();
        BackButton.interactable = true;
        spinning = false;
    }

    private void OnDestroy()
    {
        SpinButton.onClick.RemoveListener(Spin);
    }
}
