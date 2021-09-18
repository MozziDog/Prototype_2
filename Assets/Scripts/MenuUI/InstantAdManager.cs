using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class InstantAdManager : MonoBehaviour
{
    public GameObject chargeCompleteWindow;
    public GameObject staminaBuyWindow;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnButtonClick()
    {
        TryShowAd(() =>
        {
            staminaBuyWindow.SetActive(false);
            GameObject.Find("StaminaManager").GetComponent<StaminaManager>().AddStamina(10);
            chargeCompleteWindow.SetActive(true);
        });
    }

    public void TryShowAd(Action onFinish = null)
    {
        ShowAd();
        if (onFinish != null)
        {
            onFinish();
        }
    }

    public void ShowAd()
    {
        SceneManager.LoadScene("Ad", LoadSceneMode.Additive);
    }

    public void OnClickButton_ChargeCompleteWindow()
    {
        chargeCompleteWindow.SetActive(false);
    }
}
