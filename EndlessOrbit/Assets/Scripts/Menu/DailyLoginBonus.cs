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
    [SerializeField] TextMeshProUGUI StarsAmount;
    [SerializeField] RectTransform StarImage;

    [SerializeField] float spinTimer;
    [SerializeField] float MaxSpinAngle;
    [SerializeField] float MinSpinAngle;

    [SerializeField] int MinRewardAmount;
    [SerializeField] int MaxRewardAmount;
    [SerializeField] List<TextMeshProUGUI> PartsOfWheel;

    [SerializeField] List<GameObject> PooledStars;

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
        if(PlayerManager.instance.DailyLoginValuesEmpty())
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

        PlayerManager.instance.SetLoginValues(rewardValues);

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
        this.GetComponent<BackGestureComponent>().CanUseBackGesture(false);
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
        StartCoroutine(TallyStars(value));
        PlayerManager.instance.AddStars(value);
        PlayerManager.instance.JustReceivedBonus();
        SpinButton.interactable = PlayerManager.instance.BonusAvailable();
        BackButton.interactable = true;
        this.GetComponent<BackGestureComponent>().CanUseBackGesture(true);
        spinning = false;
    }

    IEnumerator TallyStars(int collectedStars)
    {
        int currStars = PlayerManager.instance.GetSilverStars();
        int newAmount = currStars + collectedStars;
        int count = 0;
        Vector2 startingPosition = new Vector2(PooledStars[0].GetComponent<RectTransform>().position.x, PooledStars[0].GetComponent<RectTransform>().position.y);
        RectTransform imageTransform = PooledStars[0].GetComponent<RectTransform>();
        Vector2 velocity = new Vector2(0, 0);
        StarsAmount.text = currStars.ToString();
        while (currStars < newAmount)
        {        
            float timer = 0f;
            while (timer < .1f)
            {
                PooledStars[count].SetActive(true);
                Debug.Log("test: " + count);
                PooledStars[count].GetComponent<RectTransform>().position = Vector2.Lerp(PooledStars[count].GetComponent<RectTransform>().position, StarImage.position, timer / .1f);
                yield return null;
                timer += Time.deltaTime;
            }
            PooledStars[count].SetActive(false);
            PooledStars[count++].GetComponent<RectTransform>().position = startingPosition;
            StarsAmount.text = (++currStars).ToString();
            yield return null;
        }
        StarsAmount.text = newAmount.ToString();
    }

    private void OnDestroy()
    {
        SpinButton.onClick.RemoveListener(Spin);
    }
}
