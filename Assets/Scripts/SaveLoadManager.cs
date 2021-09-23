using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.IO;

public class SaveLoadManager : MonoBehaviour
{
    public string path;

    // 타이틀 씬에서 Load하고 이후 save를 위해 DontDestroyOnLoad. 
    // 그러므로 스테이지 선택 씬이나 인게임 씬에서 테스트 돌릴 시 정상 작동 안될 수 있음
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        path = Path.Combine(Application.persistentDataPath, "save.json");
        Load();
    }

    // Update is called once per frame
    void Update()
    {

    }
    /*
    public void Save()
    {
        UserProperty data = new UserProperty();
        data.gold = Global.userProperty.gold;
        data.ruby = Global.userProperty.ruby;
        // data.stamina = Global.userProperty.stamina;
        data.nextStaminaRegenTime = Global.userProperty.nextStaminaRegenTime;

        string jsonDataString = JsonUtility.ToJson(data, true);

        File.WriteAllText(path, jsonDataString);

        Debug.Log("Saved");
    }
    public void Load()
    {
        if (File.Exists(path))
        {
            string loadedJsonDataString = File.ReadAllText(path);

            UserProperty data = JsonUtility.FromJson<UserProperty>(loadedJsonDataString);
            Debug.Log("gold: " + data.gold.ToString() + " | ruby: " + data.ruby.ToString());

            Global.userProperty.gold = data.gold;
            Global.userProperty.ruby = data.ruby;
            // Global.userProperty.stamina = data.stamina;
            Global.userProperty.nextStaminaRegenTime = data.nextStaminaRegenTime;

            GameObject selectStageManager = GameObject.Find("StageSelectManager");
            if (selectStageManager != null)
            {
                selectStageManager.GetComponent<StageSelectSceneManager>().RefreshUserPropertyData();
            }
        }
    }
    */

    public void Save()
    {
        PlayerPrefs.SetInt("gold", Global.userProperty.gold);
        PlayerPrefs.SetInt("ruby", Global.userProperty.ruby);
        PlayerPrefs.SetInt("TutorialFinishFlag", Global.userProperty.TutorialFinishFlag ? 1 : 0);
        PlayerPrefs.SetInt("StartStoryFinishFlag", Global.userProperty.startStoryFinishFlag ? 1 : 0);
        PlayerPrefs.SetInt("lastReachedChapter", Global.userProperty.LastReachedChapter);
        PlayerPrefs.SetInt("lastReachedStage", Global.userProperty.LastReachedStage);
        PlayerPrefs.Save();
        Debug.Log("Saved");
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("gold")) // 세이브 되어있는지 확인
        {
            Global.userProperty.gold = PlayerPrefs.GetInt("gold");
            Global.userProperty.ruby = PlayerPrefs.GetInt("ruby");
            Global.userProperty.TutorialFinishFlag = PlayerPrefs.GetInt("TutorialFinishFlag") == 1 ? true : false;
            Global.userProperty.startStoryFinishFlag = PlayerPrefs.GetInt("StartStoryFinishFlag") == 1 ? true : false;
            Global.userProperty.LastReachedChapter = PlayerPrefs.GetInt("lastReachedChapter");
            Global.userProperty.LastReachedStage = PlayerPrefs.GetInt("lastReachedStage");
            Debug.Log($"gold: {Global.userProperty.gold}, ruby: {Global.userProperty.ruby}");
            Debug.Log($"LastReached: {Global.userProperty.LastReachedChapter} - {Global.userProperty.LastReachedStage}");
        }
        else
        {
            // 초기 소지값
            Global.userProperty.gold = 1000;
            Global.userProperty.ruby = 0;
            Global.userProperty.TutorialFinishFlag = false;
            Global.userProperty.LastReachedChapter = 1;
            Global.userProperty.LastReachedStage = 0;
        }
    }
}
