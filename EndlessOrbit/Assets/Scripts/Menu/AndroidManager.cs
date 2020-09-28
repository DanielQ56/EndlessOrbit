using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidManager : MonoBehaviour
{
    public static AndroidManager instance = null;
    // Start is called before the first frame update


    List<BackGestureComponent> components;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            components = new List<BackGestureComponent>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
#if UNITY_ANDROID || UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TriggerBackGesture();
        }
#endif
    }

    public void AddComponentToList(BackGestureComponent c)
    {

        //Debug.Log("ADDING: There are " + components.Count + " components in the list before adding");
        components.Add(c);
        //Debug.Log("ADDING: There are " + components.Count + " components in the list after adding");
    }

    void TriggerBackGesture()
    {
        //Debug.Log("THERE ARE CURRENTLY " + components.Count + " COMPONENTS IN THE LIST");
        BackGestureComponent c = components[components.Count - 1];
        c.GoBack();
    }

    public void RemoveComponentFromList(BackGestureComponent c)
    {
        //Debug.Log("REMOVING: There are " + components.Count + " components in the list before removing");
        components.Remove(c);
        //Debug.Log("REMOVING: There are " + components.Count + " components in the list after removing");
    }


}
