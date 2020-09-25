using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidManager : MonoBehaviour
{
    public static AndroidManager instance = null;
    // Start is called before the first frame update

    List<BackGestureComponent> components = new List<BackGestureComponent>();
    
    void Awake()
    {
        instance = this;
    }

    private void Update()
    {
#if UNITY_ANDROID
        if (Input.GetKey(KeyCode.Escape))
        {
            TriggerBackGesture();
        }
#endif
    }

    public void AddComponentToList(BackGestureComponent c)
    {
        components.Add(c);
    }

    void TriggerBackGesture()
    {
        BackGestureComponent c = components[components.Count - 1];
        c.GoBack();
    }

    public void RemoveComponentFromList(BackGestureComponent c)
    {
        components.Remove(c);
    }


}
