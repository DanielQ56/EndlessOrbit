using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    [SerializeField] List<GameObject> BackgroundObjects;
    [SerializeField] float LoopBound;
    [SerializeField] float DistanceToLoop;
    [SerializeField] float ShiftSpeed;

    // Update is called once per frame
    void Update()
    {
        LoopBackground();
    }

    void LoopBackground()
    {

        foreach (GameObject g in BackgroundObjects)
        {
            if (g.transform.localPosition.y < LoopBound)
            {
                g.transform.localPosition += (Vector3.up * DistanceToLoop * 2);
            }
            g.transform.Translate(Vector2.down * ShiftSpeed * Time.deltaTime);
        }
    }
}
