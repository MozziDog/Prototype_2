using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//k all
public class NormalBombBullet : MonoBehaviour, BulletInterFace
{
    public GameObject BombAreaEffect;
    public GameObject impactParticle;
    GameObject ShootArea;

    public string BulletName;
    public float bulletSpeed;
    public float bulletDamage;
    
    public Transform target;
    //public Vector3 aimPosition;
    public Transform Projectile;
    private Transform myTransform;


    public float firingAngle = 55.0f;
    public float gravity = 9.8f;
    public float BombRadius;
   
    

    RaycastHit hit;
    public void SetUp(BulletInfo bulletinfo)
    {
        this.bulletSpeed = bulletinfo.bulletSpeed;
        this.target = bulletinfo.attackTarget;
        this.bulletDamage = bulletinfo.bulletDamage;
        this.BombRadius = bulletinfo.bombRange;
        Projectile = this.transform;
    }


    IEnumerator SimulateProjectile()
    {

        ShootArea = Instantiate(BombAreaEffect, new Vector3(target.position.x, 0.1f, target.position.z), BombAreaEffect.transform.rotation);
        Projectile.position = this.transform.position + new Vector3(0, 0.0f, 0);

        // Calculate distance to target
        float target_Distance = Vector3.Distance(Projectile.position, target.position);

        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
        float flightDuration = target_Distance / Vx;
        Projectile.rotation = Quaternion.LookRotation(target.position - Projectile.position);
        float elapse_time = 0;

        while (elapse_time < flightDuration)
        {
            Projectile.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime * bulletSpeed, Vx * Time.deltaTime*bulletSpeed);
            elapse_time += Time.deltaTime*bulletSpeed;

            yield return null;
        }
     
    }
    

    /*
     private void OnCollisionEnter(Collision collision)
     {
         if (collision.gameObject.layer == 10) { 
             Debug.LogWarning("Bomb Exploding!!");
         Explode(collision);
             }

     }
    */

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Floor")) //|| other.gameObject.layer == LayerMask.NameToLayer("Enemy")
            Explode();
    }

    void Explode()
    {
        
        //hit particle spawn
       GameObject BoomEffects = Instantiate(impactParticle, Projectile.position, Quaternion.identity) as GameObject;
       // BoomEffects.transform.parent = target.transform;
        Destroy(BoomEffects, 3);
        Collider[] colliders = Physics.OverlapSphere(transform.position, BombRadius);


        foreach (Collider searchedObject in colliders)
        {
            Debug.Log("Bomb Area Searching");
            if (searchedObject != null && searchedObject.gameObject.tag == "GroundEnemy")
            {
                
                searchedObject.gameObject.GetComponent<EnemyInterFace>().GetDamage(bulletDamage);
            }
        }
       
        Destroy(ShootArea,0.3f);
        Destroy(gameObject);
    }

    void OnDrawGizmos() //폭탄 범위 sphere 표시
    {
        Gizmos.DrawWireSphere(transform.position, BombRadius);
    }

    /*
    void AimTarget() //자동추격 기능 
    {
        aimPosition = new Vector3(target.position.x, target.position.y, target.position.z);
        transform.LookAt(aimPosition);
        Physics.Raycast(transform.position, aimPosition, out hit);
        Debug.DrawLine(transform.position, aimPosition);
    }
  */


    // Start is called before the first frame update
    void Start()
    {
        if (target)
            StartCoroutine(SimulateProjectile());
    }

    

    // Update is called once per frame
    void Update()
    {
      

    }
}
