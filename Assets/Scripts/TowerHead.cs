using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//k all
public enum WeaponState { SearchTarget, AttackToTarget }

public class TowerHead : MonoBehaviour
{
    private WeaponState weaponState = WeaponState.SearchTarget;
    public GameObject BulletPrefab;
    public Transform BulletSpawnPoint;
    public float bulletSpeed;
    public float bulletDamage = 1;
    public float attackRate = 0.5f;
    public float attackRange = 3.5f;

    public Transform attackTarget = null;
    public GameObject SpawnPoint;
    public List<GameObject> enemyList;



    public void ChangeState(WeaponState newState) //���� ����  Ž��, ����  ����� �ڷ�ƾ ��ȯ
    {
        StopCoroutine(weaponState.ToString());
        weaponState = newState;
        StartCoroutine(weaponState.ToString());
    }





    private void RotateToTarget() //���� �ٶ�
    {

        transform.LookAt(new Vector3(attackTarget.position.x, transform.position.y, attackTarget.position.z));
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

    private IEnumerator AttackToTarget() //�� ����
    {
        while (true)
        {
            if (attackTarget == null)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }
            /*
            if (attackTarget.tag == "GroundEnemy" && attackTarget.Find("Enemy01").gameObject.GetComponent<GroundEnemy>().isDie)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }

           else if (attackTarget.tag == "FlyingEnemy" && attackTarget.Find("AirPlaneEnemy").gameObject.GetComponent<FlyingEnemy>().isDie)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }
            */


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


    private void SpawnBullet() //�߻�ü ����
    {

        GameObject clone = Instantiate(BulletPrefab, BulletSpawnPoint.position, Quaternion.identity);
        clone.GetComponent<Bullet>().Setup(attackTarget, bulletSpeed, bulletDamage);

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
        this.enemyList = SpawnPoint.GetComponent<EnemyManager>().CurrentEnemyList; //�� �����Ӹ��� �� ����Ʈ ����
        if (attackTarget != null)
        {
            RotateToTarget();
        }
    }

}
