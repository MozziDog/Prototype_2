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
    Vector3 originSpawnTransform;
    [SerializeField] GameObject _endPoint;
    [SerializeField] GameObject _groundStartPoint;
    [SerializeField] GameObject _airStartPoint;
    public bool pathAvailable;

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
        CurrentEnemyList = new List<GameObject>(); //���� �����Ǿ��ִ� �� ���� ����� ����Ʈ
        navMeshPath = new UnityEngine.AI.NavMeshPath();
        originSpawnTransform = _groundStartPoint.transform.position;
        
    }

    public void StartWave(Wave wave)
    {
        currentWave = wave;
        StartSpawn(); //currentWave�� ���̺� ������ ����ؼ� ���̺� ��ŸƮ
    }

    // Update is called once per frame
    void Update()
    {
 
    }

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
        
        line.positionCount = path.corners.Length;
        for (int i = 0; i < path.corners.Length; i++)
            line.SetPosition(i, path.corners[i]);

        if (line.positionCount == 0) return false;

        print("New path calculated");
        if (navMeshPath.status ==  NavMeshPathStatus.PathPartial) 
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
        
        switch (currentWave.enemyPrefabs[enemySpawnCount].tag )
        {
            case "FlyingEnemy":
                clone = Instantiate(currentWave.enemyPrefabs[enemySpawnCount], _airStartPoint.transform);
                SpawnedAirEnemyCount++;
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
