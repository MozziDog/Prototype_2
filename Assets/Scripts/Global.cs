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
    public UserProperty(int gold, int ruby, int stamina)
    {
        this.gold = gold;
        this.ruby = ruby;
        this.stamina = stamina;
        this.nextStaminaRegenTime = 0;
        this.TutorialFinishFlag = false;
        this.LastReachedChapter = 0;
        this.LastReachedStage = 0;
    }
}
