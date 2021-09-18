using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class InGameAdManager : MonoBehaviour
{
    bool Adready = true;
    [SerializeField] Button button;

    // Start is called before the first frame update
    void Start()
    {
        Adready = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnButtonClick()
    {
        TryShowAd(() =>
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().GiveAdBonusReward();
            button.interactable = false;
        });
    }

    public void TryShowAd(Action onFinish = null)
    {
        if (Adready == false)
        {
            return;
        }
        Adready = false;
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
}
