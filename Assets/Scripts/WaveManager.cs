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
    public bool allWaveClear = false;

    public WaveData[] waveData;
    // Start is called before the first frame update

    public void StartWave()
    {
        if (enemySpawner.CurrentEnemyList.Count == 0 && currentWaveIndex < waves.Length - 1) //웨이브 진행
        {
            currentWaveIndex++;
            obstacleManager.WayObstacleActiveSwitch();
            enemySpawner.StartWave(waves[currentWaveIndex]);
        }
    }

    public  bool isWaveClear()
    {
        if (enemySpawner.enemySpawnCount == waves[currentWaveIndex].maxEnemyCount
            && player.currentHP > 0
            && enemySpawner.enemyKilledCount >= waves[currentWaveIndex].maxEnemyCount
            && enemySpawner.CurrentEnemyList.Count==0)
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
        Debug.LogWarning("WaveDone");//한 웨이브 완수 시 보상
    }

    public void FinalReward()
    {
        allWaveClear = true; //전체 웨이브 완수 시 보상화면 이동 , allWaveDone 은  GameManager 이 사용할 것임
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
        if (Input.GetKeyDown(KeyCode.S)) StartWave(); //웨이브 스타트
        
        if (currentWaveIndex != -1)
        if (isWaveClear())
            switch(currentWaveIndex == waves.Length -1)
            {
                case false:
                    MidTermReward();
                    break;
                case true:
                    FinalReward();
                    break;
            }

    }


}

