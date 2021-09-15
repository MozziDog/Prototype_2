using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//k all
public class PoisonBombBullet : MonoBehaviour, BulletInterFace
{
    [Header("Gameobject to add")]
    public GameObject BombAreaEffect;
    public GameObject impactParticle;
    public GameObject PoisonFx;
    GameObject ShootArea;


    [Header("BulletInfo")]
    public float LV;
    public string BulletName;
    public float bulletSpeed;
    public float bulletDamage;

    [Header("PoisonDebuff Info")]
    public float poisonDamage;
    public float poisonDuration;
    public float poisonRate;

    [Header("Palabora inspection")]
    public Transform target;
    public Transform Projectile;
    private Transform myTransform;
    public float firingAngle = 45.0f;
    public float gravity = 9.8f;
    public float BombRadius;


    private bool isExplode=false;

    RaycastHit hit;
    public void SetUp(BulletInfo bulletinfo)
    {
        this.LV = bulletinfo.LV;
        this.bulletSpeed = bulletinfo.bulletSpeed;
        this.target = bulletinfo.attackTarget;
        this.bulletDamage = bulletinfo.bulletDamage;
        this.poisonDamage = bulletinfo.poisonDamage;
        this.poisonDuration = bulletinfo.poisonDuration;
        this.poisonRate = bulletinfo.poisonRate;
        this.BombRadius =  bulletinfo.bombRange;
        Projectile= this.transform;
}


    IEnumerator SimulateProjectile() //PALABORA MOVEMENT
    {
       

        ShootArea = Instantiate(BombAreaEffect, new Vector3(target.position.x, 0f, target.position.z), BombAreaEffect.transform.rotation);
        Projectile.position = this.transform.position + new Vector3(0, 0.0f, 0);

        // Calculate distance to target
        float target_Distance = Vector3.Distance(Projectile.position, target.position);

        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
        Projectile.rotation = Quaternion.LookRotation(target.position - Projectile.position);
        float flightDuration = target_Distance / Vx;
        float elapse_time = 0;
        Destroy(ShootArea,flightDuration/bulletSpeed+0.3f);
        while (elapse_time < flightDuration)
        {
            Projectile.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime * bulletSpeed, Vx * Time.deltaTime*bulletSpeed);
            elapse_time += Time.deltaTime*bulletSpeed;

           
               
            yield return null;
        }
        Destroy(gameObject);
     
    }
    

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            isExplode = true;
            Explode();
        }
            
    }

    void Explode()
    {
        
        //hit particle spawn
        GameObject BoomEffects = Instantiate(impactParticle, Projectile.position, Quaternion.identity) as GameObject;
        Destroy(BoomEffects, 2f);
        Collider[] colliders = Physics.OverlapSphere(transform.position, BombRadius);
        foreach (Collider searchedObject in colliders)
        {
            
            if (searchedObject != null && searchedObject.gameObject.tag == "GroundEnemy"
                && !searchedObject.GetComponent<EnemyInterFace>().CheckDead())
            {
               
                
                if (!searchedObject.gameObject.GetComponent<PoisonDebuff>())
                {
                    
                    searchedObject.gameObject.AddComponent<PoisonDebuff>();
                    searchedObject.gameObject.GetComponent<PoisonDebuff>().SetUp(LV,poisonDamage,poisonDuration,poisonRate, PoisonFx);
                    searchedObject.gameObject.GetComponent<PoisonDebuff>().ExecuteDebuff();
                }
                else 
                {
                   
                    searchedObject.gameObject.GetComponent<PoisonDebuff>().RefreshDuration(poisonDuration, poisonDamage, poisonRate);
                }

                GameObject effect = Instantiate(PoisonFx, searchedObject.GetComponent<EnemyInterFace>().GetBodyPos().position, Quaternion.identity);
                effect.transform.SetParent(searchedObject.transform);
                Destroy(effect, 1f);
                searchedObject.GetComponent<EnemyInterFace>().GetDamage(bulletDamage);

            }
        }
       
       
        Destroy(gameObject);
    }
   

    void OnDrawGizmos() //폭탄 범위 sphere 표시
    {
        Gizmos.DrawWireSphere(transform.position, BombRadius);
    }




    // Start is called before the first frame update
    void Start()
    {
        
            StartCoroutine(SimulateProjectile());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
