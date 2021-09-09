using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroundEnemy : MonoBehaviour, EnemyInterFace
{
    [Header("Enemy Info")]
    public float maxHP;
    public float currentHP;
    public float moveSpeed;
    public float hitDamage;
    public Transform headPos;

    [Header("Enemy State")]
    public bool isDie = false;
    public bool isHitting = false;
    private bool isWalking = true;


    [Header("Animator and EnemyManager")]
    public Animator anim;
    public GameObject enemyManager;

    GameObject target;
    GameObject Player;
    NavMeshAgent agent;


    public void SetUp() { }

    public Transform GetHeadPos()
    {
        return headPos;
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

    void Start()
    {
        currentHP = maxHP;
        anim = this.GetComponent<Animator>();
        enemyManager = GameObject.Find("SpawnPointGroup");
        target = GameObject.Find("EndPoint");
        Player = GameObject.Find("Player1");
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(target.transform.position);
        agent.speed = moveSpeed;



    }
    private void Update()
    {

    }

    public float GetSpeed()
    {
        return moveSpeed;
    }

    public void SetSpeed(float ApplySpeed)
    {
        moveSpeed = ApplySpeed;
        agent.speed = moveSpeed;
    }





    public void GetDamage(float Damage) //k
    {
        currentHP -= Damage;
        if (currentHP <= 0)
        {
            ReadyToDie();
        }
    }

    public void RemoveObject()
    {
        enemyManager.GetComponent<EnemyManager>().CurrentEnemyList.Remove(gameObject);
        enemyManager.GetComponent<EnemyManager>().enemyKilledCount++;
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDie)
            return;

        if (other.gameObject.CompareTag("Player"))
        {
            agent.speed = 0;
            isWalking = false;
            isDie = true;
            StartCoroutine(HitPlayer());


        }

    }

    IEnumerator HitPlayer()
    {

        yield return null;
        isHitting = true;
        anim.SetBool("ContactPlayer", true);
        yield return new WaitForSeconds(0.35f);
        Player.GetComponent<Player>().StartGetHit(hitDamage);
        yield return new WaitForSeconds(0.62f);
        RemoveObject();
    }

    public void ReadyToDie()
    {
        if (isHitting)
            return;
        isDie = true;
        this.gameObject.layer = LayerMask.NameToLayer("Dead");
        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine()
    {
        anim.SetBool("isDead", true);
        agent.speed = 0;
        yield return new WaitForSeconds(1.2f);
        RemoveObject();
    }






}
