using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaManager : MonoBehaviour
{
    StageSelectSceneManager selectSceneManager;
    readonly int MAX_STAMINA = 10;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DisplayStamina());
    }

    IEnumerator DisplayStamina()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            RefreshStamina();
        }
    }

    void RefreshStamina()
    {
        selectSceneManager.RefreshUserPropertyData();
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void useStamina(int value)
    {
        Global.userProperty.stamina -= value;
        if (Global.userProperty.stamina < MAX_STAMINA)
        {
            System.DateTime staminaRegenAt = System.DateTime.Now.Add(new System.TimeSpan(0, 10, 0));
            Global.nextStaminaRegenTime = staminaRegenAt.ToBinary();
        }
    }

    public void calcStamina()
    {
        System.DateTime nowTime = System.DateTime.Now;
        System.DateTime staminaRegenTime = System.DateTime.FromBinary(Global.nextStaminaRegenTime);
        while (nowTime.CompareTo(staminaRegenTime) > 0) // nowTime > staminaRegenTime
        {
            staminaRegenTime.Add(new System.TimeSpan(0, 10, 0));
            Global.userProperty.stamina += 1;
        }
    }
}
