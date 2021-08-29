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

    private Wave currentWave;
    GameObject CurrentSpawnenemy;
    GameObject clone;
    public List<GameObject> CurrentEnemyList;

    [SerializeField]
    private int enemyMaxCount;
    private int enemySpawnCount = 0;
    public int SpawnedAirEnemyCount = 0;

    

    // Start is called before the first frame update
    void Start()
    {
        CurrentEnemyList = new List<GameObject>(); //현재 생성되어있는 적 정보 참고용 리스트
        navMeshPath = new UnityEngine.AI.NavMeshPath();
        
        
    }
    void Update()
    {
            AirRouteDraw();
    }

    public void StartWave(Wave wave)
    {
        currentWave = wave;
        StartSpawn(); //currentWave의 웨이브 정보에 기반해서 웨이브 스타트
    }

    // Update is called once per frame
   



    public void AirRouteDraw()
    {
        var lineForAir = GameObject.Find("GroundSpawnPoint").GetComponent< LineRenderer>();
        switch (SpawnedAirEnemyCount > 0)
        {
            case true:
                lineForAir.enabled = true;
            lineForAir.SetPosition(0, _groundStartPoint.transform.position);
            lineForAir.SetPosition(1, _endPoint.transform.position);
            break;

            case false:
                lineForAir.enabled = false;
                break;

        }

        
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
            enemySpawnCount++; //생존유무와 상관없이 생성된 적 카운트
            CurrentEnemyList.Add(CurrentSpawnenemy);
            yield return new WaitForSeconds(currentWave.spawnTime); //생성 빈도 
            
        if (enemySpawnCount < currentWave.maxEnemyCount) StartCoroutine(EnemySpawner());
    }


}
