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

    private bool lockOn = false;
    private bool isBulletCharged = true;
    private bool isBulletCharging = false;
    GameObject temp;
    BulletObjectPull objectPool;
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

    public void ChangeState(WeaponState newState) //���� ����  Ž��, ����  ����� �ڷ�ƾ ��ȯ
    {
        StopCoroutine(weaponState.ToString());
        weaponState = newState;
        StartCoroutine(weaponState.ToString());
    }

    void OnDrawGizmos() //��ź ���� sphere ǥ��
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
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
    /*
    private void RotateToHome()
    {
        Quaternion home = Quaternion.LookRotation(SpawnPoint.transform.position);

        RotatingBody.transform.rotation = Quaternion.Slerp(RotatingBody.transform.rotation, home, 2f * Time.deltaTime);
    }
    */

    private IEnumerator SearchTarget() //�� Ž��
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
                if (distance <= attackRange)
                {

                    temp.Add(enemyList[i].transform);
                }
            }
            if (temp.Count > 0)
            {
                attackTarget = temp[Random.Range(0, temp.Count)];
                temp.Clear();
            }

            if (attackTarget != null && !attackTarget.GetComponent<EnemyInterFace>().CheckDead())
            {
                lockOn = true;
                ChangeState(WeaponState.AttackToTarget);
                break;
            }

            yield return null;
        }
    }

    private IEnumerator AttackToTarget() //�� ����
    {
        yield return new WaitForSeconds(1.15f);
        while (true)
        {

            float distance = Vector3.Distance(attackTarget.position, transform.position);
            if (distance > attackRange)
            {
                lockOn = false;
                attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            if (isBulletCharged)
            {
                isBulletCharged = false;
                SpawnBullet();
                //yield return new WaitForSeconds(attackRate);
                attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
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
        clone.transform.LookAt(attackTarget);


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
