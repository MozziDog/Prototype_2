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
    public static int grade_meteor = 1;
    public static int grade_landsmash = 1;
    public static int grade_powerup = 1;
    public static int grade_earthquake = 1;
    public static int grade_heal = 1;
    public static int grade_sacrifice = 1;
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
    public int grade_meteor;
    public int grade_landsmash;
    public int grade_powerup;
    public int grade_earthquake;
    public int grade_heal;
    public int grade_sacrifice;
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
        grade_meteor = 1;
        grade_landsmash = 1;
        grade_powerup = 1;
        grade_earthquake = 1;
        grade_heal = 1;
        grade_sacrifice = 1;
    }
}
