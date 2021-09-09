using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//k all



public class RandomTargetTower : MonoBehaviour, TowerInterFace
{
    private WeaponState weaponState = WeaponState.SearchTarget;
    BulletInfo bulletinfo = new BulletInfo();

    [Header("tower body points and bullet")]
    public GameObject BulletPrefab;
    public Transform BulletSpawnPoint;
    public Transform RotatingBody;
    [Header("tower info")]
    public float LV;
    public string type;
    public float bulletSpeed;
    public float bulletDamage;
    public float attackRate;
    public float attackRange;

    [Header("variables for In-Game watch")]
    public Transform attackTarget = null;
    public GameObject SpawnPoint;
    public WaveManager wavemanager;
    public List<GameObject> enemyList;
    private Transform homeY;
    private bool isAiming;
    private bool isShooting;

    private bool lockOn = false;
    GameObject temp;
    public void SetUp(TowerInfo towerinfo)
    {

        this.LV = towerinfo.LV;
        this.type = towerinfo.type;
        this.attackRate = towerinfo.attackRate;
        this.attackRange = towerinfo.attackRange;
        this.bulletSpeed = towerinfo.bulletSpeed;
        this.bulletDamage = towerinfo.bulletDamage;

        bulletinfo.bulletSpeed = towerinfo.bulletSpeed;
        bulletinfo.bulletDamage = towerinfo.bulletDamage;

        bulletinfo.bombRange = towerinfo.bombRange;

        bulletinfo.stunDuration = towerinfo.stunDuration;
        bulletinfo.mujeockTime = towerinfo.mujeockTime;



    }

    public void ChangeState(WeaponState newState) //적에 대한  탐색, 공격  모드의 코루틴 전환
    {
        StopCoroutine(weaponState.ToString());
        weaponState = newState;
        StartCoroutine(weaponState.ToString());
    }

    void OnDrawGizmos() //폭탄 범위 sphere 표시
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }


    private void RotateToTarget() //적을 바라봄
    {
        if (attackTarget)
        {

            Vector3 dir = attackTarget.transform.position - RotatingBody.transform.position;
            dir.y = 0;
            Quaternion rot = Quaternion.LookRotation(dir);
            RotatingBody.transform.rotation = Quaternion.Slerp(RotatingBody.transform.rotation, rot, 3f * Time.deltaTime);

        }
    }
    private void RotateToHome()
    {
        Quaternion home = Quaternion.LookRotation(SpawnPoint.transform.position);

        RotatingBody.transform.rotation = Quaternion.Slerp(RotatingBody.transform.rotation, home, 2f * Time.deltaTime);
    }

    private IEnumerator SearchTarget() //적 탐색
    {
        List<Transform> temp = new List<Transform>();
        while (true)
        {
            float closestDistSqr = Mathf.Infinity;
            for (int i = 0; i < enemyList.Count; ++i)
            {
                if (enemyList[i] == null)
                    continue;
                if (BulletPrefab.tag == "BombBullet" && enemyList[i].tag == "FlyingEnemy")
                    continue;
                if (enemyList[i].GetComponent<EnemyInterFace>().CheckDead())
                    continue;

                float distance = Vector3.Distance(enemyList[i].transform.position, transform.position);
                if (distance <= attackRange )
                {
                    
                  temp.Add(enemyList[i].transform);
                }
            }
            if(temp.Count>0)
            attackTarget = temp[Random.Range(0,temp.Count)];

            if (attackTarget != null)
            {

                ChangeState(WeaponState.AttackToTarget);
                break;
            }

            yield return null;
        }
    }

    private IEnumerator AttackToTarget() //적 공격
    {
        yield return new WaitForSeconds(1.25f);
        while (true)
        {
            

            //|| attackTarget.gameObject.layer == LayerMask.NameToLayer("Dead")



            float distance = Vector3.Distance(attackTarget.position, transform.position);


            if (distance > attackRange)
            {
                attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            SpawnBullet();
            yield return new WaitForSeconds(attackRate);
            attackTarget = null;
            ChangeState(WeaponState.SearchTarget);
        }
    }


    private void SpawnBullet() //발사체 생성
    {
       

        bulletinfo.attackTarget = this.attackTarget;
        GameObject clone = Instantiate(BulletPrefab, BulletSpawnPoint.position, Quaternion.identity);
        BulletInterFace bullet = clone.GetComponent<BulletInterFace>();
        bullet.SetUp(bulletinfo);

        

    }
    void CheckTarget()
    {

        if (!attackTarget || attackTarget.GetComponent<EnemyInterFace>().CheckDead())
        {
            attackTarget = null;
            ChangeState(WeaponState.SearchTarget);
            return;
        }

    }



    void Start()
    {

        SpawnPoint = GameObject.Find("SpawnPointGroup");
        this.enemyList = SpawnPoint.GetComponent<EnemyManager>().CurrentEnemyList;

    }

    private void OnEnable()
    {
        
        ChangeState(WeaponState.SearchTarget);
    }

    // Update is called once per frame
    void Update()
    {
        this.enemyList = SpawnPoint.GetComponent<EnemyManager>().CurrentEnemyList; //매 프레임마다 적 리스트 갱신


        CheckTarget();

        if (attackTarget)
        {
            RotateToTarget();
        }
    }

}
