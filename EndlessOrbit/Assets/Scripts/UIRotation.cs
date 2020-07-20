using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRotation : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector3 direction;

    [SerializeField]
    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        direction = new Vector3(0, 0, speed);
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.Rotate(direction);
    }
}
