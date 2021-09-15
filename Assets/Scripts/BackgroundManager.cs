using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public GameObject[] backgroundPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (backgroundPrefab.Length >= Global._chapter)
        {
            Instantiate(backgroundPrefab[Global._chapter - 1], Vector3.zero, Quaternion.identity);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
