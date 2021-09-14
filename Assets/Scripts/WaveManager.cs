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
    // Start is called before the first frame update

    public void StartWave()
    {
        if (enemySpawner.CurrentEnemyList.Count == 0 && currentWaveIndex < waves.Length - 1) //���̺� ����
        {
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
    }

    public void FinalReward()
    {
        allWaveClear = true; //��ü ���̺� �ϼ� �� ����ȭ�� �̵� , allWaveDone ��  GameManager �� ����� ����
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
                        StartWave();
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


}

