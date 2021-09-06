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
    public float bulletSpeed;
    public float bulletDamage;
    public float attackRate;
    public float attackRange;

    [Header("variables for In-Game watch")]
    public Transform attackTarget = null;
    public GameObject SpawnPoint;
    public List<GameObject> enemyList;
    public List<GameObject> preTargetList;
    private Transform homeY;
    private bool isAiming;
    private bool isShooting;


    public void SetUp(TowerInfo towerinfo)
    {

        this.LV = towerinfo.LV;
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
        while (true)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange);
            if (colliders.Length == 0) continue;


            Collider temp = colliders[Random.Range(0, colliders.Length)];
            
            if (temp.gameObject.layer != LayerMask.NameToLayer("Enemy"))
                continue;
            if (BulletPrefab.tag == "BombBullet" && temp.tag == "FlyingEnemy")
                continue;

            attackTarget = temp.gameObject.transform;


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
        while (true)
        {
            if (attackTarget == null)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            //|| attackTarget.gameObject.layer == LayerMask.NameToLayer("Dead")



            float distance = Vector3.Distance(attackTarget.position, transform.position);


            if (distance > attackRange)
            {
                attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
                break;
            }
            yield return new WaitForSeconds(attackRate);


            SpawnBullet();
        }
    }


    private void SpawnBullet() //발사체 생성
    {
        float distance = Vector3.Distance(attackTarget.position, transform.position);
        /*
        if (!attackTarget || distance > attackRange)
        {
            attackTarget = null;
            ChangeState(WeaponState.SearchTarget);
            return;
        }
        */
        



        bulletinfo.attackTarget = this.attackTarget;
        GameObject clone = Instantiate(BulletPrefab, BulletSpawnPoint.position, Quaternion.identity);
        BulletInterFace bullet = clone.GetComponent<BulletInterFace>();
        bullet.SetUp(bulletinfo);

        attackTarget = null;
        ChangeState(WeaponState.SearchTarget);

    }


    void Start()
    {

        SpawnPoint = GameObject.Find("SpawnPointGroup");
        this.enemyList = SpawnPoint.GetComponent<EnemyManager>().CurrentEnemyList;
        
    }

    private void OnEnable()
    {
        this.preTargetList = new List<GameObject>();
        ChangeState(WeaponState.SearchTarget);
    }

    // Update is called once per frame
    void Update()
    {
        this.enemyList = SpawnPoint.GetComponent<EnemyManager>().CurrentEnemyList; //매 프레임마다 적 리스트 갱신


        if (!attackTarget || attackTarget.gameObject.GetComponent<EnemyInterFace>().CheckDead() == true)
        {
            
            ChangeState(WeaponState.SearchTarget);

        }

        if (attackTarget)
        {
            RotateToTarget();
        }
    }

}
