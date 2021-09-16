using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlyingEnemy : MonoBehaviour, EnemyInterFace
{

    [Header("Enemy Info")]
    public float maxHP;
    public float currentHP;
    public float moveSpeed;
    public float hitDamage;

    public Transform headPos;
    public Transform bodyPos;
    public Transform shootPos;
    public GameObject fireball;
    [Header("Enemy State")]
    public bool isDie;
    public bool isHit;
    private bool isWalking;


    [Header("Animator and EnemyManager")]
    public Animator anim;
    public GameObject enemyManager;


   
    GameObject target;
    GameObject Player;
    Vector3 targetPositionForAir;
  

    public void SetUp() { }


    public Transform GetHeadPos()
    {
        return headPos;
    }

    public Transform GetBodyPos()
    {
        return bodyPos;
    }


    public bool CheckDead()
    {
        if (isDie)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public float GetSpeed()
    {
        return moveSpeed;
    }

    public void SetSpeed(float ApplySpeed)
    {
        moveSpeed = ApplySpeed;
    }

    




    void Start()
    {
        isDie = false;
        isHit = false;
        isWalking = true;

        currentHP = maxHP;
        target = GameObject.Find("EndPoint");
        Player = GameObject.Find("Player1");
        enemyManager = GameObject.Find("SpawnPointGroup");
        targetPositionForAir = new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z);
    }





    private void Update()
    {
        

        if (isWalking)
            AirMove();

    }

  
    public void AirMove()
    {
        transform.LookAt(new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z));
        transform.position = Vector3.MoveTowards(this.transform.position, targetPositionForAir, moveSpeed * Time.deltaTime);

    }



    public void GetDamage(float Damage)
    {
        currentHP -= Damage;
        if (currentHP <= 0)
        {
            ReadyToDie();
        }

    }

    public void RemoveObject()
    {
        isDie = true;
        enemyManager.GetComponent<EnemyManager>().CurrentEnemyList.Remove(gameObject);
        enemyManager.GetComponent<EnemyManager>().SpawnedAirEnemyCount--;
        enemyManager.GetComponent<EnemyManager>().enemyKilledCount++;
        Destroy(gameObject);

    }

    

    private void OnTriggerEnter(Collider other)
    {

        if (isDie)
            return;
            if (other.gameObject.CompareTag("Player"))
        {

            isWalking = false;
            isDie = true;
            StartCoroutine(HitPlayer(other));
        }

    }

    IEnumerator HitPlayer(Collider other)
    {
        

        anim.SetBool("ContactPlayer", true);
        yield return new WaitForSeconds(0.45f);
        GameObject shootElement = Instantiate(fireball, shootPos.position, Quaternion.identity);
        Vector3 targetPos = new Vector3(other.transform.position.x, other.transform.position.y + 0.5f, other.transform.position.z);
        shootElement.GetComponent<DragonBullet>().SetUp(targetPos);
        while (Vector3.Distance(shootElement.transform.position, targetPos) >0.2f)
        {
            yield return null;
        }
        Player.GetComponent<Player>().StartGetHit(hitDamage);
        Destroy(shootElement);
        yield return new WaitForSeconds(0.55f);
        RemoveObject();
    }

  

    public void ReadyToDie()
    {
        if (isHit)
            return;
        isDie = true;
        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine()
    {
        anim.SetBool("isDead", true);
        isWalking = false;
        yield return new WaitForSeconds(1.07f);
        RemoveObject();
    }






}
