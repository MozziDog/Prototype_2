using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


// README
// 기존 설치된 타워만 Raycasting으로 검사하기 위해 "Tower_temp"라는 레이어를 사용합니다.
// 타워 프리팹의 레이어를 전부 "Tower_temp"로 바꿔주세요
// temp 레이어의 이름이나 번호는 바꿔도 상관없습니다.
// 
public enum TowerSpawnCheck
{
    OK,
    NotEnoughSpace,
    TooMuchTower,
    NoEnemyPath
}

public enum TowerManagerMode
{
    NotSpawnable, // 적 웨이브 진행중 등으로 타워 설치 불가능한 상태
    TowerSpawnable, // 필드에 타워를 설치할 수 있는 상태
    TowerSelected // 필드에 설치된 타워가 선택된 상태
}

public class TowerManager : MonoBehaviour
{
    [ReadOnly] [SerializeField] TowerManagerMode towerManagerStatus;
    [SerializeField] GameObject cannotBuildMessage;
    public NavMeshSurface surface;
    public static readonly int MAX_TOWER_ENTITY = 20;
    public EnemyManager enemyManager;
    public SelectManager selectManager;
    public Inventory _inven;
    public GameObject towerToSpawn;
    public GameObject temporarilyPlacedTower;
    public List<GameObject> towerSpawned = new List<GameObject>();
    public TowerSpawnUI spawnUI;
    public TowerSelectedUI selectedUI;


    void Start()
    {
        towerManagerStatus = TowerManagerMode.TowerSpawnable;
    }
    void Update()
    {

    }

    void OnTowerSelected(GameObject tower)
    {
        towerManagerStatus = TowerManagerMode.TowerSelected;
        selectedUI.SetUI(tower, true);
    }

    void OnTowerUnselected(GameObject tower)
    {
        if (false /*GameManager.isEnemyWaveStarted*/)
        {
            towerManagerStatus = TowerManagerMode.NotSpawnable;
        }
        else
        {
            towerManagerStatus = TowerManagerMode.TowerSpawnable;
        }
        selectedUI.SetUI(tower, false);
    }

    void OnSelectedTileChanged(Vector3 tilePosition)
    {
        if (towerManagerStatus == TowerManagerMode.TowerSpawnable)
        {
            if (temporarilyPlacedTower != null)
            {
                _inven.SetToggleInteractable(false);
                temporarilyPlacedTower.transform.position = tilePosition;
                if (!spawnUI.gameObject.activeSelf)
                    spawnUI.SetUI(temporarilyPlacedTower, true);
                spawnUI.SetUIPosition(temporarilyPlacedTower);
                CheckTowerSpawnableInDelay();
            }
        }
    }

    private Coroutine SpawnCheckObejct;
    private readonly float waitTime_towerCheck = 0.5f;
    public void CheckTowerSpawnableInDelay()
    {
        IEnumerator Check()
        {
            yield return new WaitForSeconds(waitTime_towerCheck);

            if (CheckTowerSpawnable() != TowerSpawnCheck.OK)
            {
                StartCoroutine(CannotBuildPopUp()); // 설치 불가능을 나타내는 효과
            }
            SpawnCheckObejct = null;
        }
        if (SpawnCheckObejct != null)
        {
            StopCoroutine(SpawnCheckObejct);
        }
        SpawnCheckObejct = StartCoroutine(Check());
    }

    public TowerSpawnCheck CheckTowerSpawnable()
    {
        if (CheckTowerSpace() == false)
        {
            return TowerSpawnCheck.NotEnoughSpace;
        }
        else if (CheckTowerEntity() == false)
            return TowerSpawnCheck.TooMuchTower;
        else if (CheckEnemyPath() == false)
            return TowerSpawnCheck.NoEnemyPath;
        else
            return TowerSpawnCheck.OK;
    }

    public bool CheckTowerSpace()
    {
        // BoxCollider의 위치를 활용해 각 블록의 위치마다 raycasting
        List<Transform> blockPosition = new List<Transform>();
        // 부모
        /*
        if (temporarilyPlacedTower.GetComponent<BoxCollider>() != null)
        {
            blockPosition.Add(towerToSpawn.transform);
        }
        */
        // 자식
        foreach (var item in temporarilyPlacedTower.GetComponentsInChildren<BoxCollider>())
        {
            blockPosition.Add(item.transform);
        };
        // 검출
        foreach (var item in blockPosition)
        {
            Ray ray = new Ray(item.position + new Vector3(0, 10f, 0), new Vector3(0, -1, 0));
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, 100f, 1 << LayerMask.NameToLayer("Floor")))
            {
                Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 1f);
                if (hit.transform.tag == "Tower")
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    public bool CheckTowerEntity()
    {
        if (towerSpawned.Count < MAX_TOWER_ENTITY)
            return true;
        else
            return false;
    }

    // TODO : CheckEnemyPath 완성
    public bool CheckEnemyPath()
    {
        enemyManager.BakeNav();
        if (enemyManager.CalculateNewPath())
        {

            temporarilyPlacedTower.SetActive(false);
            enemyManager.BakeNav();
            temporarilyPlacedTower.SetActive(true);
            return true;
        }
        else
        {
            temporarilyPlacedTower.SetActive(false);
            enemyManager.BakeNav();
            temporarilyPlacedTower.SetActive(true);
            return false;
        }
    }

    void OnInventoryItemSelected()
    {
        if (towerManagerStatus != TowerManagerMode.NotSpawnable)
        {
            PlaceTempTower();
        }
    }

    public void PlaceTempTower()
    {
        if (temporarilyPlacedTower != null)
        {
            Destroy(temporarilyPlacedTower);
            temporarilyPlacedTower = null;
        }
        Vector3 towerSpawnPosition = new Vector3(0, 20, 0); // 카메라에 안보이는 어딘가
        towerToSpawn = _inven.GetSelectedTower();
        if (towerToSpawn != null)
        {
            temporarilyPlacedTower = Instantiate(towerToSpawn, towerSpawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Inventory에서 GetSelectedTower 오류!");
        }
    }

    public void TrySpawnTower()
    {
        if (temporarilyPlacedTower != null)
        {
            TowerSpawnCheck checkResult = CheckTowerSpawnable();
            if (checkResult != TowerSpawnCheck.OK)
            {
                //오류 표시
                Debug.LogWarning("여기에는 설치할 수 없습니다");
                return;
            }
            else
            {
                for (int i = 0; i < towerToSpawn.GetComponent<TowerInfo>()._myposition.Length; i++)
                {
                    if (Physics.CheckSphere(towerToSpawn.GetComponent<TowerInfo>()._myposition[i], 0.4f))
                    {

                    }
                }
                temporarilyPlacedTower.GetComponent<TowerInfo>().ConfirmTowerPosition();
                enemyManager.BakeNav();
                towerSpawned.Add(temporarilyPlacedTower);
                temporarilyPlacedTower = null;

                _inven.DeleteSelectedItem();
                _inven.SetToggleInteractable(true);

                spawnUI.SetUI(null, false);
                return;
            }
        }
    }

    public void RotateTempTower()
    {
        if (temporarilyPlacedTower != null)
            temporarilyPlacedTower.transform.Rotate(new Vector3(0, 90, 0));
        CheckTowerSpawnableInDelay();
    }

    public void invertTempTower()
    {
        if (temporarilyPlacedTower != null)
        {
            if ((temporarilyPlacedTower.transform.rotation.y % 90) == 0)
            {
                temporarilyPlacedTower.transform.localScale = new Vector3(
                    temporarilyPlacedTower.transform.localScale.x * -1,
                    1,
                    temporarilyPlacedTower.transform.localScale.z);
                // BoxCollider 꼬임 방지를 위해 Children의 globalScale 값을 양수로 해줌.
                for (int i = 0; i < temporarilyPlacedTower.transform.childCount; i++)
                {
                    Transform childTransform = temporarilyPlacedTower.transform.GetChild(i).transform;
                    childTransform.localScale = new Vector3(
                        childTransform.localScale.x * -1,
                        1,
                        childTransform.localScale.z);
                }
            }
            else
            {
                temporarilyPlacedTower.transform.localScale = new Vector3(
                    temporarilyPlacedTower.transform.localScale.x,
                    1,
                    temporarilyPlacedTower.transform.localScale.z * -1);
                // BoxCollider 꼬임 방지를 위해 Children의 globalScale 값을 양수로 해줌.
                for (int i = 0; i < temporarilyPlacedTower.transform.childCount; i++)
                {
                    Transform childTransform = temporarilyPlacedTower.transform.GetChild(i).transform;
                    childTransform.localScale = new Vector3(
                        childTransform.localScale.x,
                        1,
                        childTransform.localScale.z * -1);
                }
            }
        }
        CheckTowerSpawnableInDelay();
    }

    public void destroyTempTower()
    {
        if (temporarilyPlacedTower != null)
        {
            GameObject.Destroy(temporarilyPlacedTower);
            temporarilyPlacedTower = null;

            _inven.SetToggleInteractable(true);
        }
        CheckTowerSpawnableInDelay();
    }

    IEnumerator CannotBuildPopUp() //k
    {
        cannotBuildMessage.SetActive(true);
        yield return new WaitForSeconds(1.3f);
        cannotBuildMessage.SetActive(false);
    }

}
