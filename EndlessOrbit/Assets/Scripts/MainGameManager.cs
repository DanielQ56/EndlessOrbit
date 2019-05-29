using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviour
{
    [SerializeField] Camera mainCam;
    [SerializeField] Transform planetParent;
    [SerializeField] Transform startingPlanet;
    [SerializeField] List<GameObject> planets;
    [Range(1, 3)] public int maxNumOfPlanets;

    public static MainGameManager instance; 

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }

        updateCameraPosition(startingPlanet);
    }

    public void AttachedToNewPlanet(Transform newPlanet)
    {
        updateCameraPosition(newPlanet);
    }

    void GenerateNextPlanet(Transform newPlanet)
    {
        if (planetParent.childCount == 1)
        {
            int numPlanetsToSpawn = 1;
            float width = mainCam.orthographicSize * ((float)Screen.width / (float)Screen.height) - 3;
            float gap = ((width * 2) / numPlanetsToSpawn);
            for (int i = 0; i < numPlanetsToSpawn; ++i)
            {
                GameObject clone = Instantiate(planets[Random.Range(0, planets.Count)], planetParent);
                clone.transform.position = new Vector3(Random.Range(-width + gap * i, -width + gap * (i + 1)),
                    Random.Range(mainCam.transform.position.y, mainCam.transform.position.y + mainCam.orthographicSize -1), 0);

            }
        }
    }

    


    void updateCameraPosition(Transform newPlanet)
    {
        StartCoroutine(UpdateCamPos(newPlanet));
    }

    IEnumerator UpdateCamPos(Transform newPlanet)
    {
        Vector3 pos = new Vector3(0, newPlanet.position.y + mainCam.orthographicSize, -10);
        while (Mathf.Abs(mainCam.transform.position.y - pos.y) > 3)
        {
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, pos, 2 * Time.deltaTime);
            yield return null;
        }
        GenerateNextPlanet(newPlanet);

    }




}
