using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRotation : MonoBehaviour
{
    private RectTransform rectTransform;

    [SerializeField] float speed;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.Rotate(0, 0, speed * Time.deltaTime);
    }
}
