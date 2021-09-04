using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//k all
public enum WeaponState { SearchTarget, AttackToTarget }

public class ShootTower : MonoBehaviour,TowerInterFace
{
    private WeaponState weaponState = WeaponState.SearchTarget;
    BulletInfo bulletinfo = new BulletInfo();

    [Header("tower body points and bullet")]
    public GameObject BulletPrefab;
    public Transform BulletSpawnPoint;
    public Transform RotatingBody;
    [Header("tower info")]
    public float bulletSpeed;
    public float bulletDamage ;
    public float attackRate ;
    public float attackRange ;
    
    [Header("variables for In-Game watch")]
    public Transform attackTarget = null;
    public GameObject SpawnPoint;
    public List<GameObject> enemyList;
    public float homeY;

    public void SetUp(TowerInfo towerinfo)
    {

        bulletinfo.bulletSpeed = towerinfo.bulletSpeed;
        bulletinfo.bulletDamage = towerinfo.bulletDamage;
        this.attackRate =towerinfo.attackRate;
        this.attackRange= towerinfo.attackRange;

    }

    public void ChangeState(WeaponState newState) //적에 대한  탐색, 공격  모드의 코루틴 전환
    {
        StopCoroutine(weaponState.ToString());
        weaponState = newState;
        StartCoroutine(weaponState.ToString());
    }





    private void RotateToTarget() //적을 바라봄
    {
        if (attackTarget)
        {

            Vector3 dir = attackTarget.transform.position - RotatingBody.transform.position;
            dir.y = 0;
            Quaternion rot = Quaternion.LookRotation(dir);
            RotatingBody.transform.rotation = Quaternion.Slerp(RotatingBody.transform.rotation, rot, 5 * Time.deltaTime);

        }

        else
        {

            Quaternion home = new Quaternion(0, homeY, 0, 1);

            RotatingBody.transform.rotation = Quaternion.Slerp(RotatingBody.transform.rotation, home, Time.deltaTime);
        }

       

    }

    private IEnumerator SearchTarget() //적 탐색
    {
        while (true)
        {
            float closestDistSqr = Mathf.Infinity;
            for (int i = 0; i < enemyList.Count; ++i)
            {
                if (enemyList[i] == null)
                    continue;
                if (BulletPrefab.tag == "BulletBomb" && enemyList[i].tag == "FlyingEnemy")
                    continue;

                float distance = Vector3.Distance(enemyList[i].transform.position, transform.position);
                if (distance <= attackRange && distance <= closestDistSqr)
                {
                    closestDistSqr = distance;
                    attackTarget = enemyList[i].transform;
                }
            }
            if (attackTarget != null)
            {
                ChangeState(WeaponState.AttackToTarget);
            }
            yield return null;
        }
    }

    private IEnumerator AttackToTarget() //적 공격
    {
        while (true)
        {
            if (attackTarget == null )
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
        if (!attackTarget)
        {
            ChangeState(WeaponState.SearchTarget);
            return;
        }
            

        bulletinfo.attackTarget = this.attackTarget;
        GameObject clone = Instantiate(BulletPrefab, BulletSpawnPoint.position, Quaternion.identity);
        BulletInterFace bullet = clone.GetComponent<BulletInterFace>();
        bullet.SetUp(bulletinfo);
            
        
    }


    void Start()
    {
        homeY = RotatingBody.transform.localRotation.eulerAngles.y;
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
