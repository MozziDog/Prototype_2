using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//k all


public class MultipleShootTower : MonoBehaviour, TowerInterFace
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

    [Header("Multiple Shoot Info")]
    public float bulletAmmoCount;
    public float burstRate;



    [Header("variables for In-Game watch")]
    public Transform attackTarget = null;
    public GameObject SpawnPoint;
    public List<GameObject> enemyList;
    private Transform homeY;


    private bool lockOn = false;
    private bool isBulletCharged = true;
    private bool isBulletCharging = false;
    BulletObjectPull objectPool;


    public void SetUp(TowerInfo towerinfo)
    {

        this.LV = towerinfo.LV;
        this.type = towerinfo.type;
        this.attackRate = towerinfo.attackRate;
        this.attackRange = towerinfo.attackRange;
        this.bulletSpeed = towerinfo.bulletSpeed;
        this.bulletDamage = towerinfo.bulletDamage;
        this.bulletAmmoCount = towerinfo.bulletAmmoCount;
        this.burstRate = towerinfo.burstRate;


        bulletinfo.bulletSpeed = towerinfo.bulletSpeed;
        bulletinfo.bulletDamage = towerinfo.bulletDamage;

        bulletinfo.bombRange = towerinfo.bombRange;

        bulletinfo.maxChainCount = towerinfo.maxChainCount;
        bulletinfo.chainRadius = towerinfo.chainRadius;

        bulletinfo.poisonDamage = towerinfo.poisonDamage;
        bulletinfo.poisonDuration = towerinfo.poisonDuration;
        bulletinfo.poisonRate = towerinfo.poisonRate;

        bulletinfo.mujeockTime = towerinfo.mujeockTime;
        bulletinfo.stunDuration = towerinfo.stunDuration;




    }

    public void ChangeState(WeaponState newState) //���� ����  Ž��, ����  ����� �ڷ�ƾ ��ȯ
    {
        StopCoroutine(weaponState.ToString());
        weaponState = newState;
        StartCoroutine(weaponState.ToString());
    }





    private void RotateToTarget() //���� �ٶ�
    {
        if (attackTarget)
        {

            Vector3 dir = attackTarget.transform.position - RotatingBody.transform.position;
            dir.y = 0;
            Quaternion rot = Quaternion.LookRotation(dir);
            RotatingBody.transform.rotation = Quaternion.Slerp(RotatingBody.transform.rotation, rot, 3f * Time.deltaTime);
        }
    }


    private IEnumerator SearchTarget() //�� Ž��
    {
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
                if (distance <= attackRange && distance < closestDistSqr)
                {
                    closestDistSqr = distance;
                    attackTarget = enemyList[i].transform;
                }
            }

            if (attackTarget != null && !attackTarget.GetComponent<EnemyInterFace>().CheckDead())
            {
                lockOn = true;
                ChangeState(WeaponState.AttackToTarget);
            }

            yield return null;
        }
    }

    private IEnumerator AttackToTarget() //�� ����
    {
        yield return new WaitForSeconds(1.25f);
        while (true)
        {



            float distance = Vector3.Distance(attackTarget.position, transform.position);
            if (isBulletCharged)
            {
                for (int i = 0; i < bulletAmmoCount; i++)
            {
                if (distance > attackRange)
                {
                    lockOn = false;
                    isBulletCharged = false;
                    attackTarget = null;
                    ChangeState(WeaponState.SearchTarget);
                    break;
                }


                SpawnBullet();
                if (i == bulletAmmoCount - 1) break;
                yield return new WaitForSeconds(burstRate);
            }
                isBulletCharged = false;
               
            }

            yield return null;




        }
    }


    private void SpawnBullet() //�߻�ü ����
    {

        bulletinfo.attackTarget = this.attackTarget;
        GameObject clone = objectPool.GetObject(BulletPrefab);
        clone.transform.position = BulletSpawnPoint.position;
        BulletInterFace bullet = clone.GetComponent<BulletInterFace>();
        bullet.SetUp(bulletinfo);
    }

    void CheckTarget()
    {

        if (!attackTarget || attackTarget.GetComponent<EnemyInterFace>().CheckDead())
        {
            lockOn = false;
            attackTarget = null;
            ChangeState(WeaponState.SearchTarget);
            return;
        }

    }

    IEnumerator ChargeBullet()
    {
        yield return new WaitForSeconds(attackRate);
        isBulletCharged = true;
        isBulletCharging = false;
    }

    void Start()
    {

        SpawnPoint = GameObject.Find("SpawnPointGroup");
        this.enemyList = SpawnPoint.GetComponent<EnemyManager>().CurrentEnemyList;
        objectPool = GetComponent<BulletObjectPull>();
    }

    private void OnEnable()
    {
        ChangeState(WeaponState.SearchTarget);
    }



    // Update is called once per frame

    void Update()
    {
        CheckTarget();
        this.enemyList = SpawnPoint.GetComponent<EnemyManager>().CurrentEnemyList; //�� �����Ӹ��� �� ����Ʈ ����
        if (lockOn)
            RotateToTarget();
        if (!isBulletCharged && !isBulletCharging)
        {
            isBulletCharging = true;
            StartCoroutine(ChargeBullet());
        }
    }

}
