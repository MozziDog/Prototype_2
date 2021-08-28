using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlyingEnemy : MonoBehaviour
{
    [SerializeField]
    private float maxHP;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Animator anim;
    
    private float currentHP;
    private bool isDie = false;
    private bool isWalking = true;
    public float hitDamage;
    GameObject target;
    GameObject Player;
    Vector3 targetPositionForAir;
    




    public GameObject GameManagerObject;
   

    

    void Start()
    {
        currentHP = maxHP;
        GameManagerObject = GameObject.Find("SpawnPoint"); 
        target = GameObject.Find("EndPoint");
        Player = GameObject.Find("Player1");
        transform.position = GameManagerObject.transform.position;
        targetPositionForAir = new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z);
    }

    



    private void Update()
    {
        // AgentStuckAvoid();
        if(isWalking)
            AirMove();

    }
    /*
    public void AgentStuckAvoid()
    {
        if (isWalking && !agent.hasPath && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.speed > 0.3)
        {
            Debug.LogWarning("enemy Repathing!!");
            agent.enabled = false;
            agent.enabled = true;
            agent.SetDestination(target.transform.position);
            agent.speed = moveSpeed;
        }
    }
    */
    public void AirMove()
    {
        transform.LookAt(new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z));
        transform.position = Vector3.MoveTowards(this.transform.position, targetPositionForAir, moveSpeed*Time.deltaTime);
    }

    public void GetDamage(float Damage) //k
    {
        currentHP -= Damage;
        if (currentHP <= 0)
        {
            isDie = true;
            GameManagerObject.GetComponent<EnemyManager>().CurrentEnemyList.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            isWalking = false;
            StartCoroutine(HitPlayer());
            
           
        }
        
    }

    IEnumerator HitPlayer()
    {
        
        anim.SetBool("ContactPlayer", true);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Player.GetComponent<Player>().GetHitCoroutine(hitDamage));
        GameManagerObject.GetComponent<EnemyManager>().CurrentEnemyList.Remove(gameObject);
        yield return new WaitForSeconds(0.75f);
        Destroy(gameObject);
    }



    


}
