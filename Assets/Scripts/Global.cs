using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Global variables
public static class Global
{
    public static UserProperty userProperty = new UserProperty(1000, 0, 10);
    public static int _chapter = 1;
    public static int _stage = 1;
    public static long nextStaminaRegenTime;
    public static string selectedSkill1;
    public static string selectedSkill2;
    public static string selectedSkill3;
    public static string selectedSkill4;
}


public struct UserProperty
{
    public int gold;
    public int ruby;
    public int stamina;
    public long nextStaminaRegenTime;
    public string selectedSkill1;
    public string selectedSkill2;
    public string selectedSkill3;
    public string selectedSkill4;
    public UserProperty(int gold, int ruby, int stamina)
    {
        this.gold = gold;
        this.ruby = ruby;
        this.stamina = stamina;
        this.nextStaminaRegenTime = 0;
        selectedSkill1 = "";
        selectedSkill2 = "";
        selectedSkill3 = "";
        selectedSkill4 = "";
    }
}
