using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave Data", menuName = "Scriptable Object/Wave Data", order = int.MaxValue)]
public class WaveData : ScriptableObject
{
    /*[SerializeField]
    private string towerName;
    public string TowerName { get { return towerName; } }*/

    [SerializeField]
    private float spawnTime;
    public float SpawnTime { get { return spawnTime; } }

    [SerializeField]
    private int maxEnemyCount;
    public int MaxEnemyCount { get { return maxEnemyCount; } }

    [SerializeField]
    private GameObject[] enemyPrefabs;
    public GameObject[] EnemyPrefabs { get { return enemyPrefabs; } }
}
