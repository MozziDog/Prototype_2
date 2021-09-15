using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Global variables
public static class Global
{
    public static UserProperty userProperty = new UserProperty(1000, 0, 10);
    public static int _chapter = 1;
    public static int _stage = 1;
}


public struct UserProperty
{
    public int gold;
    public int ruby;
    public int stamina;
    public long nextStaminaRegenTime;
    public bool TutorialFinishFlag;
    public int LastReachedChapter;
    public int LastReachedStage;
    public int skill_1_level;
    public int skill_2_level;
    public int skill_3_level;
    public int skill_4_level;
    public int skill_5_level;
    public UserProperty(int gold, int ruby, int stamina)
    {
        this.gold = gold;
        this.ruby = ruby;
        this.stamina = stamina;
        this.nextStaminaRegenTime = 0;
        this.TutorialFinishFlag = false;
        this.LastReachedChapter = 1;
        this.LastReachedStage = 0;
        this.skill_1_level = 1;
        this.skill_2_level = 1;
        this.skill_3_level = 1;
        this.skill_4_level = 1;
        this.skill_5_level = 1;
    }
}
