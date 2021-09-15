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
    public static int grade_Meteor = 1;
    public static int grade_LandSmash = 1;
    public static int grade_PowerUp = 1;
    public static int grade_Earthquake = 1;
    public static int grade_Heal = 1;
    public static int grade_Sacrifice = 1;
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
    public int grade_Meteor;
    public int grade_LandSmash;
    public int grade_PowerUp;
    public int grade_Earthquake;
    public int grade_Heal;
    public int grade_Sacrifice;
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
        grade_Meteor = 1;
        grade_LandSmash = 1;
        grade_PowerUp = 1;
        grade_Earthquake = 1;
        grade_Heal = 1;
        grade_Sacrifice = 1;
    }
}
