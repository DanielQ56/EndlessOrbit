using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HowToPlay : MonoBehaviour, IEndDragHandler, IDragHandler, IBeginDragHandler
{
    [SerializeField] GameObject[] Panels;
    [SerializeField] float DragDistance;

    int index = 0;

    Vector2 StartPos;

    // Start is called before the first frame update
    void OnEnable()
    {
        index = 0;
        foreach(GameObject g in Panels)
        {
            g.SetActive(false);
        }

        Panels[index].SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        StartPos = eventData.position;
        Debug.Log("Start position is: " + StartPos);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End position is: " + eventData.position);
        if (eventData.position.x - StartPos.x > DragDistance)
        {
            PreviousPanel();
        }
        else if(StartPos.x - eventData.position.x > DragDistance)
        {
            NextPanel();
        }
    }

    void NextPanel()
    {
        if(index < Panels.Length - 1)
        {
            Panels[index++].SetActive(false);
            Panels[index].SetActive(true);
        }

    }

    void PreviousPanel()
    {
        if (index > 0)
        {
            Panels[index--].SetActive(false);
            Panels[index].SetActive(true);
        }
    }
}
