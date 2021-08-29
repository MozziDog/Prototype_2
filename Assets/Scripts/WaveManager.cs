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
    private Wave[] waves;
    [SerializeField]
    private EnemyManager enemySpawner;
    [SerializeField]
    private ObstacleManager obstacleManager;
    private int currentWaveIndex = -1;

    private int _waveMode = 0;
    [SerializeField] Button StartBtn;
    [SerializeField] Sprite[] BtnImages;
    // Start is called before the first frame update

    public void StartWave()
    {
        if(enemySpawner.CurrentEnemyList.Count ==0 && currentWaveIndex < waves.Length-1) //웨이브 진행
        {
            currentWaveIndex++;
            obstacleManager.WayObstacleActiveSwitch();
            enemySpawner.StartWave(waves[currentWaveIndex]);
        }
    }

    public void ChangeWaveSpeed()
    {
        if(_waveMode == 0)
        {
            StartBtn.GetComponent<Image>().sprite = BtnImages[_waveMode];
            Time.timeScale = 1;
            _waveMode = 1;
        }
        else if(_waveMode == 1)
        {
            StartBtn.GetComponent<Image>().sprite = BtnImages[_waveMode];
            Time.timeScale = 2;
            _waveMode = 0;
        }
            
    }

   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) StartWave(); //웨이브 스타트


    }
}
