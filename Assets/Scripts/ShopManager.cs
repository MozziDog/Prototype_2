using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject notPreparedWindow;
    public GameObject confirmWindow;
    public GameObject notEnoughMoneyWindow;
    public StaminaManager staminaManager;

    int gemToBuy = 0;
    int goldToBuy = 0;
    float price_RealMoney = 0;
    int price_gem = 0;

    private Dictionary<int, float> gemPrice = new Dictionary<int, float>{
        {100, 1200},
        {500, 5500},
        {1000, 11000},
        {3000, 33000},
        {5000, 55000},
        {10000, 110000}
    };

    private Dictionary<int, int> goldPrice = new Dictionary<int, int>{
        {3000, 100},
        {7500, 250},
        {15000, 500},
        {30000, 1000},
        {90000, 3000},
        {150000, 5000}
    };

    public void NotPrepared()
    {
        notPreparedWindow.SetActive(true);
    }

    public void OnClickButton_notPrepared()
    {
        notPreparedWindow.SetActive(false);
    }

    public void OnClickButton_PurchaseGem(int value)
    {
        gemToBuy = value;
        goldToBuy = 0;
        price_RealMoney = gemPrice[value];
        price_gem = 0;
        confirmWindow.SetActive(true);
    }

    public void OnClickButton_PurchaseGold(int value)
    {
        price_gem = goldPrice[value];
        if (price_gem > Global.userProperty.ruby)
        {
            notEnoughMoneyWindow.SetActive(true);
            return;
        }
        gemToBuy = 0;
        goldToBuy = value;
        price_RealMoney = 0;
        confirmWindow.SetActive(true);
    }

    public void ConfirmPurchase()
    {
        if (price_RealMoney > 0)
        {
            // 실제라면 여기에 결제 코드 넣기
            Debug.Log(price_RealMoney + "원 결제됨");
        }
        if (price_gem > 0)
        {
            // 결제되는 연출 들어갈 부분
            Global.userProperty.ruby -= price_gem;
        }
        if (gemToBuy > 0)
        {
            // 추가되는 연출 들어갈 부분
            Global.userProperty.ruby += gemToBuy;
        }
        if (goldToBuy > 0)
        {
            // 추가되는 연출 들어갈 부분
            Global.userProperty.gold += goldToBuy;
        }
        GameObject.Find("StageSelectManager").GetComponent<StageSelectSceneManager>().RefreshUserPropertyData();
        GameObject.Find("SaveLoadManager").GetComponent<SaveLoadManager>().Save();
        confirmWindow.SetActive(false);
    }

    public void CancelPurchase()
    {
        price_gem = 0;
        price_RealMoney = 0;
        gemToBuy = 0;
        goldToBuy = 0;
        confirmWindow.SetActive(false);
    }


    public void OnClickButton_notEnoughMoney()
    {
        CancelPurchase();
        notEnoughMoneyWindow.SetActive(false);
    }

    public GameObject staminaChargeUI;
    public void OpenStaminaChargeUI()
    {
        staminaChargeUI.SetActive(true);
    }

    public void CloseStaminaChargeUI()
    {
        staminaChargeUI.SetActive(false);
    }
    public void OnClickButton_BuyStamina()
    {
        if (Global.userProperty.ruby < 50)
        {
            notEnoughMoneyWindow.SetActive(true);
        }
        else
        {
            staminaManager.AddStamina(100);
            Global.userProperty.ruby -= 50;
            GameObject.Find("StageSelectManager").GetComponent<StageSelectSceneManager>().RefreshUserPropertyData();
            GameObject.Find("SaveLoadManager").GetComponent<SaveLoadManager>().Save();
            staminaChargeUI.SetActive(false);
            buyCompleteWindow.SetActive(true);
        }
    }

    public GameObject buyCompleteWindow;

    public void OnClickButton_buyComplete()
    {
        buyCompleteWindow.SetActive(false);
    }
}
