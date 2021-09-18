using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataTransfer : MonoBehaviour
{
    [SerializeField] int chapter;
    [SerializeField] int stage;

    [SerializeField] int chapterIndexInCSV;
    [SerializeField] int stageIndexInCSV;
    [SerializeField] int spawnTimeIndexInCSV;
    [SerializeField] int maxEnemyCountIndexInCSV;
    [SerializeField] int enemyDataStartIndexInCSV;
    WaveManager waveManager;
    // Start is called before the first frame update
    void Start()
    {
        waveManager = GameObject.Find("GameManager").GetComponent<WaveManager>();
        chapter = Global._chapter;
        stage = Global._stage;
        SetStageData();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetStageData()
    {
        string[] rawData = ReadStageData(chapter, stage);
        Debug.Log("CSV 파일 읽어옴.");
        List<Wave> waves = new List<Wave>();
        for (int i = 0; i < rawData.Length; i++)
        {
            Wave wave = ParseLines(rawData[i]);
            waves.Add(wave);
        }
        waveManager.setWaves(waves.ToArray());
        Debug.Log("CSV 파일 적용 완료");
    }

    string[] ReadStageData(int chapter, int stage)
    {
        var stringAll = Resources.Load("waveDesign") as TextAsset;
        string[] lines = stringAll.text.Split('\n');
        List<string> stageDataStrings = new List<string>();
        for (int i = 1 /*범례 무시*/; i < lines.Length; i++)
        {
            if (lines[i].Length > 1) //빈줄 무시
            {
                string[] commaSplited = lines[i].Split(',');
                if (int.Parse(commaSplited[chapterIndexInCSV]) == chapter
                        && int.Parse(commaSplited[stageIndexInCSV]) == stage)
                {
                    stageDataStrings.Add(lines[i]);
                }
            }
        }
        return stageDataStrings.ToArray();
    }

    Wave ParseLines(string dataString)
    {
        string[] splitted = dataString.Split(',');
        Wave newWave = new Wave();
        newWave.spawnTime = float.Parse(splitted[spawnTimeIndexInCSV]);
        //newWave.maxEnemyCount = splitted[maxEnemyCountIndex];

        List<GameObject> enemyList = new List<GameObject>();
        // csv 데이터 형식이 달라졌으면 여기서 변경
        for (int i = enemyDataStartIndexInCSV; i < splitted.Length; i += 2)
        {
            if (splitted[i].Length > 0) // 빈 항목 무시
            {
                GameObject enemyPrefab = Resources.Load<GameObject>("EnemyPrefabs/" + splitted[i]);
                int enemyCount = int.Parse(splitted[i + 1]);
                newWave.enemyPrefabs = new GameObject[enemyCount];
                for (int j = 0; j < enemyCount; j++)
                {
                    enemyList.Add(enemyPrefab);
                }
            }
        }

        newWave.enemyPrefabs = enemyList.ToArray();
        newWave.maxEnemyCount = newWave.enemyPrefabs.Length;

        return newWave;
    }



}
