using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroundEnemy : MonoBehaviour
{
    [SerializeField]
    public float maxHP;
    [SerializeField]
    public float moveSpeed;
    public Animator anim;
    public GameObject enemyManager;
    public float hitDamage;
    public float currentHP;
    public bool isDie = false;
    public bool isHit = false;
    private bool isWalking = true;
    GameObject target;
    GameObject Player;
    public NavMeshAgent agent;

    public EnemyData enemyData;
    




    void Start()
    {
        maxHP = enemyData.MaxHP;
        moveSpeed = enemyData.MoveSpeed;
        hitDamage = enemyData.HitDamage;


        currentHP = maxHP;
        anim = this.GetComponent<Animator>();
        enemyManager = GameObject.Find("SpawnPointGroup");
        target = GameObject.Find("EndPoint");
        Player = GameObject.Find("Player1");
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(target.transform.position);
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
            StartCoroutine(HitPlayer());


        }

    }

    IEnumerator HitPlayer()
    {
        
            yield return null;
        isHit = true;
        anim.SetBool("ContactPlayer", true);
        yield return new WaitForSeconds(0.35f);
        Player.GetComponent<Player>().StartGetHit(hitDamage);
        yield return new WaitForSeconds(0.62f);
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
        agent.speed = 0;
        yield return new WaitForSeconds(1.2f);
        RemoveObject();
    }






}
