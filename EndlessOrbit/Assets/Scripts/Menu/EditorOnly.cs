using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorOnly : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        this.gameObject.SetActive(true);

#else
        this.gameObject.SetActive(false);

#endif
    }

    public void DeletePath()
    {
        ScoreManager.instance.DeleteAllData();
        ScoreManager.instance.DeletePath();
    }
}
