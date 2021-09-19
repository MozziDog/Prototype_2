using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Wave
{

    public float spawnTime;
    public int maxEnemyCount;
    public GameObject[] enemyPrefabs;
}

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    public Wave[] waves;
    [SerializeField]
    private EnemyManager enemySpawner;
    [SerializeField]
    private ObstacleManager obstacleManager;
    public Player player;
    private int currentWaveIndex = -1;
    public static bool isInGame = false;
    public bool allWaveClear = false;

    public WaveData[] waveData;

    [SerializeField] GameObject ShopUI;
    [SerializeField] GameObject ClearUI;
    [SerializeField] TowerShop _towerShop;
    [SerializeField] MoneyManager moneyManager;
    [SerializeField] GameObject _waveText;
    [SerializeField] GameObject _Heal;

    [SerializeField] GameObject _invenUI;

    public int rewardPerWave = 100;
    // Start is called before the first frame update

    public void StartWave()
    {
        _invenUI.SetActive(false);
        if (enemySpawner.CurrentEnemyList.Count == 0 && currentWaveIndex < waves.Length - 1) //���̺� ����
        {
            isInGame = true;
            currentWaveIndex++;
            obstacleManager.WayObstacleActiveSwitch();
            enemySpawner.StartWave(waves[currentWaveIndex]);
        }
    }

    public bool isWaveClear()
    {

        if (enemySpawner.enemySpawnCount == waves[currentWaveIndex].maxEnemyCount
            && player.currentHP > 0
            && enemySpawner.enemyKilledCount >= waves[currentWaveIndex].maxEnemyCount
            && enemySpawner.CurrentEnemyList.Count == 0)
        {
            enemySpawner.isWaveEnd();
            isInGame = false;
            return true;
        }

        else
        {
            return false;
        }

    }

    public void MidTermReward()
    {
        Debug.LogWarning("WaveDone");//�� ���̺� �ϼ� �� ����
        isInGame = false;
        _towerShop.MakeShoppingList();
        moneyManager.GiveWaveClearReward();
        if (!ShopUI.activeInHierarchy)
            ShopUI.SetActive(true);
        _invenUI.SetActive(true);
    }

    public void FinalReward()
    {
        /*
        if (_Heal != null)
            _Heal.GetComponent<Heal>()._usage = 1;
       */
        allWaveClear = true; //��ü ���̺� �ϼ� �� ����ȭ�� �̵� , allWaveclear ��  GameManager �� ����� ����
        GameObject.Find("GameManager").GetComponent<GameManager>().OnStageClear();
    }

    private void Start()
    {
        _invenUI = GameObject.Find("Inventory Window");
        if (GameObject.Find("TutorialUI"))
            ShopUI.SetActive(false);
        isInGame = false;
        if (GameObject.Find("Heal"))
            _Heal = GameObject.Find("Heal");
        Array.Resize(ref waves, waveData.Length);
        for (int i = 0; i < waves.Length; i++)
        {
            waves[i].spawnTime = waveData[i].SpawnTime;
            waves[i].maxEnemyCount = waveData[i].MaxEnemyCount;
            waves[i].enemyPrefabs = waveData[i].EnemyPrefabs;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _waveText.GetComponent<Text>().text = (currentWaveIndex + 1).ToString() + "/" + waves.Length;
        if (Input.GetKeyDown(KeyCode.S)) StartWave(); //���̺� ��ŸƮ

        if (currentWaveIndex != -1)
            if (isInGame && isWaveClear())
                switch (currentWaveIndex == waves.Length - 1)
                {
                    case false:
                        MidTermReward();
                        // StartWave();
                        // 바로 다음 웨이브 시작 x
                        break;
                    case true:
                        FinalReward();
                        break;
                }

    }

    public void setWaves(Wave[] waveData)
    {
        this.waves = waveData;
    }

    public static bool isWaveOn()
    {
        return isInGame;
    }
}

