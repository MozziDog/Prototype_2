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
    Transform spawnPos;
    [SerializeField] GameObject _endpoint;
    [SerializeField] GameObject _enemypoint;
    public bool pathAvailable;

    private Wave currentWave;
    GameObject CurrentSpawnenemy;
    public List<GameObject> CurrentEnemyList;

    private int enemySpawnCount = 0;
    [SerializeField]
    private int enemyMaxCount;

    // Start is called before the first frame update
    void Start()
    {
        CurrentEnemyList = new List<GameObject>(); //현재 생성되어있는 적 정보 참고용 리스트
        navMeshPath = new UnityEngine.AI.NavMeshPath();
    }

    public void StartWave(Wave wave)
    {
        currentWave = wave;
        StartSpawn(); //currentWave의 웨이브 정보에 기반해서 웨이브 스타트
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
        agent.CalculatePath(_endpoint.transform.position, navMeshPath);
        NavMeshPath path = new NavMeshPath();
        var line = this.GetComponent<LineRenderer>();
        NavMesh.CalculatePath(transform.position, _endpoint.transform.position, NavMesh.AllAreas, path);
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
        BakeNav();
        _enemypoint.GetComponent<NavMeshAgent>().enabled = false;
        StartCoroutine(EnemySpawner());
    }

    IEnumerator EnemySpawner()
    {

        switch (currentWave.enemyPrefabs[enemySpawnCount].tag )
        {
            case "FlyingEnemy":
                spawnPos.position = new Vector3(_enemypoint.transform.position.x, 3, _enemypoint.transform.position.z);
                break;
            case "GroundEnemy":
                spawnPos = _enemypoint.transform;
                break;

        }
            


            GameObject clone = Instantiate(currentWave.enemyPrefabs[enemySpawnCount],spawnPos);
            CurrentSpawnenemy = clone;
            enemySpawnCount++; //생존유무와 상관없이 생성된 적 카운트
            CurrentEnemyList.Add(CurrentSpawnenemy);
            yield return new WaitForSeconds(currentWave.spawnTime); //생성 빈도 
        if (enemySpawnCount < currentWave.maxEnemyCount) StartCoroutine(EnemySpawner());
    }


}
