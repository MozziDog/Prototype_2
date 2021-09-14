using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenuManager : MonoBehaviour
{
    public GameObject notPreparedWindow;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NotPrepared()
    {
        notPreparedWindow.SetActive(true);
    }

    public void OnClickButton_notPrepared()
    {
        notPreparedWindow.SetActive(false);
    }
}
