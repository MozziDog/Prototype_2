using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotPrepared : MonoBehaviour
{
    public GameObject notPreparedUI;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenNotPreparedWindow()
    {
        notPreparedUI.SetActive(true);
    }

    public void CloseNotPreparedWindow()
    {
        notPreparedUI.SetActive(false);
    }
}
