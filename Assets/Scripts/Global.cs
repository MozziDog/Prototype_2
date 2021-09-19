using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    public static float soundVolume = 1;
    public static bool isDebug = false;


    public delegate void VolumeChangeEvent();
    public static VolumeChangeEvent OnVolumeChanged;
    public static void SetSoundVolume(float value)
    {
        soundVolume = Mathf.Clamp(value, 0, 1);
        //if (OnVolumeChanged != null)
        OnVolumeChanged();
    }
}


public struct UserProperty
{
    public int gold;
    public int ruby;
    public int stamina;
    public long nextStaminaRegenTime;
    public bool TutorialFinishFlag;
    public bool startStoryFinishFlag;
    public int LastReachedChapter;
    public int LastReachedStage;
    public int skill_1_level;
    public int skill_2_level;
    public int skill_3_level;
    public int skill_4_level;
    public int skill_5_level;
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
        this.TutorialFinishFlag = false;
        this.startStoryFinishFlag = false;
        this.LastReachedChapter = 1;
        this.LastReachedStage = 0;
        this.skill_1_level = 1;
        this.skill_2_level = 1;
        this.skill_3_level = 1;
        this.skill_4_level = 1;
        this.skill_5_level = 1;
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
