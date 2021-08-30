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
    public WavePathDisplay _pathDisplay_Ground;
    public WavePathDisplay _pathDisplay_Air;
    public bool pathAvailable;
    Transform spawnPos;

    private Wave currentWave;
    GameObject CurrentSpawnenemy;
    GameObject clone;
    public List<GameObject> CurrentEnemyList;

    private int enemySpawnCount = 0;
    public int SpawnedAirEnemyCount = 0;

    [SerializeField]
    private int enemyMaxCount;

    // Start is called before the first frame update
    void Start()
    {
        CurrentEnemyList = new List<GameObject>(); //���� �����Ǿ��ִ� �� ���� ������ ����Ʈ
        navMeshPath = new UnityEngine.AI.NavMeshPath();


    }
    void Update()
    {
    }


    public void KillAllEnemy()
    {
        if (CurrentEnemyList.Count>0)
        for (int i = 0; i < CurrentEnemyList.Count; i++)
        {
            CurrentEnemyList.RemoveAt(i);
            Destroy(CurrentEnemyList[i]);
        }
        
        
    }

    public void AirRouteDraw()
    {
        Vector3[] airPath = new Vector3[2];
        airPath[0] = _groundStartPoint.transform.position;
        airPath[1] = _endPoint.transform.position;
        _pathDisplay_Air.DisplayPath(airPath);
    }

    public void StartWave(Wave wave)
    {
        currentWave = wave;
        StartSpawn(); //currentWave�� ���̺� ������ ����ؼ� ���̺� ��ŸƮ
    }

    // Update is called once per frame


    public void BakeNav()
    {
        surface.BuildNavMesh();
    }

    public bool CalculateNewPath()
    {
        agent.CalculatePath(_endPoint.transform.position, navMeshPath);
        NavMeshPath path = new NavMeshPath();
        var line = this.GetComponent<LineRenderer>();
        NavMesh.CalculatePath(transform.position, _endPoint.transform.position, NavMesh.AllAreas, path);

        _pathDisplay_Ground.DisplayPath(path.corners);

        if (path.corners.Length == 0)
        {
            return false;
        }
        print("New path calculated");
        if (navMeshPath.status == NavMeshPathStatus.PathPartial)
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
        StartCoroutine(EnemySpawner());
    }

    IEnumerator EnemySpawner()
    {

        switch (currentWave.enemyPrefabs[enemySpawnCount].tag)
        {
            case "FlyingEnemy":
                clone = Instantiate(currentWave.enemyPrefabs[enemySpawnCount], _airStartPoint.transform);
                SpawnedAirEnemyCount++;
                // 적 경로 표시
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
        if (enemySpawnCount < currentWave.maxEnemyCount) StartCoroutine(EnemySpawner());
    }


}
