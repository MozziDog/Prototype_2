using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private Wave[] waves;
    [SerializeField]
    private EnemyManager enemySpawner;
    [SerializeField]
    private ObstacleManager obstacleManager;
    public Player player;
    private int currentWaveIndex = -1;
    private static bool isWaveProceeding = false;
    public bool allWaveClear = false;

    public WaveData[] waveData;
    // Start is called before the first frame update

    public void StartWave()
    {
        if (enemySpawner.CurrentEnemyList.Count == 0 && currentWaveIndex < waves.Length - 1) //���̺� ����
        {
            currentWaveIndex++;
            obstacleManager.WayObstacleActiveSwitch();
            enemySpawner.StartWave(waves[currentWaveIndex]);
            isWaveProceeding = true;
        }
    }

    public bool isWaveClear()
    {

        if (enemySpawner.enemySpawnCount == waves[currentWaveIndex].maxEnemyCount
            && player.currentHP > 0
            && enemySpawner.enemyKilledCount >= waves[currentWaveIndex].maxEnemyCount
            && enemySpawner.CurrentEnemyList.Count == 0)
        {
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
        isWaveProceeding = false;
    }

    public void FinalReward()
    {
        allWaveClear = true; //��ü ���̺� �ϼ� �� ����ȭ�� �̵� , allWaveDone ��  GameManager �� ����� ����
    }

    private void Start()
    {
        Array.Resize(ref waves, waveData.Length);
        for(int i = 0; i < waves.Length; i++)
        {
            waves[i].spawnTime = waveData[i].SpawnTime;
            waves[i].maxEnemyCount = waveData[i].MaxEnemyCount;
            waves[i].enemyPrefabs = waveData[i].EnemyPrefabs;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) StartWave(); //���̺� ��ŸƮ

        if (currentWaveIndex != -1)
            if (isWaveClear())
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
        return isWaveProceeding;
    }
}

