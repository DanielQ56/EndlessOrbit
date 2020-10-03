using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;

public class NotificationSystem : MonoBehaviour
{
    public static NotificationSystem instance = null;

    const string LOGIN_ID = "login_rewards";

    bool SetupNotification = false;

    AndroidNotificationChannel channel;

    int identifier;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    private void Start()
    {
        channel = new AndroidNotificationChannel()
        {
            Id = LOGIN_ID,
            Name = "Daily Login Rewards",
            Importance = Importance.Default,
            Description = "Spin for stars!",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
        AndroidNotificationCenter.CancelAllNotifications();
    }

    void SetNewNotification()
    {
        AndroidNotification newNotif = new AndroidNotification();
        newNotif.Title = "You can spin for stars!";
        newNotif.Text = "Your daily login spin is ready to be collected!";
        newNotif.FireTime = System.DateTime.Now.AddSeconds(30);//GetFireTime();
        identifier = AndroidNotificationCenter.SendNotification(newNotif, channel.Id);
        
        AndroidNotificationCenter.NotificationReceivedCallback receivedNotificationHandler = delegate (AndroidNotificationIntentData data)
        {
            var msg = "Notification received: " + data.Id + "\n";
            Debug.Log(msg);
        };


        AndroidNotificationCenter.OnNotificationReceived += receivedNotificationHandler;

        var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();

        if(notificationIntentData != null)
        {
            Debug.Log("App was opened with notification!");
            AndroidNotificationCenter.CancelAllDisplayedNotifications();
        }
    }

    public System.DateTime GetFireTime()
    {
        if (PlayerManager.instance.BonusAvailable())
        {
            return System.DateTime.Now.AddHours(2);
        }
        else
        {
            System.TimeSpan span = PlayerManager.instance.GetNextBonus() - System.DateTime.Now;
            System.DateTime current = System.DateTime.Now;
            current.AddHours(span.Hours);
            current.AddMinutes(span.Minutes);
            current.AddSeconds(span.Seconds);
            return current;
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            SetNewNotification();
    }



}
