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
   

    private IEnumerator SearchTarget() //�� Ž��
    {
        while (true)
        {
            /*
             if (enemyList.Count==0&&wavemanager.isWaveClear())
            {
                StopCoroutine(SearchTarget());
            }
            */
               if (enemyList.Count > 0)
            {
                temp = enemyList[Random.Range(0, enemyList.Count)];

                if (temp == null)
                    continue;
                if (BulletPrefab.tag == "BombBullet" && temp.tag == "FlyingEnemy")
                    continue;
                if (temp.GetComponent<EnemyInterFace>().CheckDead())
                    continue;

                float distance = Vector3.Distance(temp.transform.position, transform.position);
                if (distance <= attackRange)
                {
                    attackTarget = temp.transform;
                }
            }
           
            
            /*
            Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange);
            //if (colliders.Length == 0) continue;


            Collider temp = colliders[Random.Range(0, colliders.Length)];
            
            if (temp.gameObject.layer != LayerMask.NameToLayer("Enemy"))
                continue;
            if (BulletPrefab.tag == "BombBullet" && temp.tag == "FlyingEnemy")
                continue;
            if (temp.GetComponent<EnemyInterFace>().CheckDead())
                continue;


            float distance = Vector3.Distance(temp.transform.position, transform.position);
            if (distance <= attackRange)
            {
                attackTarget = temp.gameObject.transform;
            }
            */
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
        
    

            float distance = Vector3.Distance(attackTarget.position, transform.position);
            if (distance > attackRange)
            {
                lockOn = false;
                attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
                
            }
            SpawnBullet();
           // attackTarget = null;
            lockOn = false;
             yield return new WaitForSeconds(attackRate);
            ChangeState(WeaponState.SearchTarget);
       
    }


    private void SpawnBullet() //�߻�ü ����
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
            lockOn = false;
            attackTarget = null;
            ChangeState(WeaponState.SearchTarget);
            return;
        }

    }


    void Start()
    {

        SpawnPoint = GameObject.Find("SpawnPointGroup");
        wavemanager = GameObject.Find("GameManager").GetComponent<WaveManager>();
        this.enemyList = SpawnPoint.GetComponent<EnemyManager>().CurrentEnemyList;
        
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
        
    }

}
