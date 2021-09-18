using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class EnemyManager : MonoBehaviour
{


    public NavMeshSurface surface;
    public UnityEngine.AI.NavMeshPath navMeshPath;
    public UnityEngine.AI.NavMeshAgent agent;
    [SerializeField] GameObject _endPoint;
    [SerializeField] GameObject _groundStartPoint;
    [SerializeField] GameObject _airStartPoint;
    public bool pathAvailable;
    Transform spawnPos;

    private Wave currentWave;
    GameObject CurrentSpawnenemy;
    GameObject clone;
    public List<GameObject> CurrentEnemyList;
    public int enemySpawnCount = 0;
    public int SpawnedAirEnemyCount = 0;
    public int enemyKilledCount = 0;
    public WavePathDisplay wavePath_ground;
    public WavePathDisplay wavePath_air;

    // Start is called before the first frame update
    void Start()
    {
        CurrentEnemyList = new List<GameObject>(); //���� �����Ǿ��ִ� �� ���� ������ ����Ʈ
        navMeshPath = new UnityEngine.AI.NavMeshPath();
        BakeNav();


    }
    void Update()
    {

    }


    public void KillAllEnemy()
    {

        for (int i = 0; i < CurrentEnemyList.Count; i++)
        {
            if (CurrentEnemyList[i] == null)
                continue;

            switch (CurrentEnemyList[i].tag)
            {
                case "GroundEnemy":
                    CurrentEnemyList[i].GetComponent<GroundEnemy>().ReadyToDie();
                    break;
                case "FlyingEnemy":
                    CurrentEnemyList[i].GetComponent<FlyingEnemy>().ReadyToDie();
                    break;
            }

        }


    }



    public void AirRouteDraw()
    {
        Vector3[] lineForAir = { _airStartPoint.transform.position, _endPoint.transform.position };
        wavePath_air.DisplayPath(lineForAir);
    }

    public void StartWave(Wave wave)
    {
        enemySpawnCount = 0;
        currentWave = wave;
        StartSpawn(); //currentWave�� ���̺� ������ ����ؼ� ���̺� ��ŸƮ
    }




    public void BakeNav()
    {
        surface.BuildNavMesh();
    }

    public bool CalculateNewPath()
    {
        agent.CalculatePath(_endPoint.transform.position, navMeshPath);
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, _endPoint.transform.position, NavMesh.AllAreas, path);
        if (path.corners.Length == 0)
            return false;
        print("New path calculated");
        wavePath_ground.DisplayPath(path.corners);
        if (navMeshPath.status != NavMeshPathStatus.PathComplete)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void StartSpawn()
    {
        // BakeNav();
        this.GetComponent<NavMeshAgent>().enabled = false;
        enemySpawnCount = 0;
        StartCoroutine(EnemySpawner());
    }

    IEnumerator EnemySpawner()
    {

        switch (currentWave.enemyPrefabs[enemySpawnCount].tag)
        {
            case "FlyingEnemy":
                clone = Instantiate(currentWave.enemyPrefabs[enemySpawnCount], _airStartPoint.transform);
                SpawnedAirEnemyCount++;
                AirRouteDraw();
                break;
            case "GroundEnemy":
                clone = Instantiate(currentWave.enemyPrefabs[enemySpawnCount], _groundStartPoint.transform);
                break;

        }

        CurrentSpawnenemy = clone;
        enemySpawnCount++; //���������� ������� ������ �� ī��Ʈ
        CurrentEnemyList.Add(CurrentSpawnenemy);
        yield return new WaitForSeconds(currentWave.spawnTime); //���� �� 
        if (enemySpawnCount < currentWave.enemyPrefabs.Length) StartCoroutine(EnemySpawner());
    }

    public void isWaveEnd()
    {
        this.GetComponent<NavMeshAgent>().enabled = true;
    }


}
