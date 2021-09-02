using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//k all
public class BombBullet : MonoBehaviour
{
    public string BulletName;
    public float bulletSpeed;
    private float bulletDamage;
    private Transform target;
    public GameObject impactParticle;
    public Vector3 aimPosition;
    public Transform Projectile;
    private Transform myTransform;
    public float firingAngle = 60.0f;
    public float gravity = 9.8f;


    

    RaycastHit hit;
    public void Setup(Transform target, float bulletSpeed, float bulletDamage)
    {
        this.bulletSpeed = bulletSpeed;
        this.target = target;
        this.bulletDamage = bulletDamage;
    }


    IEnumerator SimulateProjectile()
    {
        // Short delay added before Projectile is thrown
        //yield return new WaitForSeconds(1.5f);

        // Move projectile to the position of throwing object + add some offset if needed.
        Projectile.position = this.transform.position + new Vector3(0, 0.0f, 0);

        // Calculate distance to target
        float target_Distance = Vector3.Distance(Projectile.position, target.position);

        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        // Extract the X  Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        // Calculate flight time.
        float flightDuration = target_Distance / Vx;

        // Rotate projectile to face the target.
        Projectile.rotation = Quaternion.LookRotation(target.position - Projectile.position);

        float elapse_time = 0;

        while (elapse_time < flightDuration)
        {
            Projectile.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime*bulletSpeed);

            elapse_time += Time.deltaTime;

            yield return null;
        }
    }
    //[출처] [Unity 5] 특정 지점으로 포물선 운동하는 물체 보내기|작성자 호이돌

    
    private void OnTriggerEnter(Collider other) //적 또는과 바닥과 충돌시 상호작용
    {
        if (1<<other.gameObject.layer != LayerMask.NameToLayer("Floor") || 1<<other.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;
        
        if(other.transform != target) return;

            Explode();
        


        //hit particle spawn
        impactParticle = Instantiate(impactParticle, target.transform.position + Vector3.up * 0.5f, Quaternion.FromToRotation(Vector3.forward, hit.normal)) as GameObject;
        impactParticle.transform.parent = target.transform;
        Destroy(impactParticle, 3);

        //destroy bullet prefab
        Destroy(gameObject, 0.6f);
         
        
    }

    void Explode()
    {
        Instantiate(impactParticle, transform.position, transform.rotation);


        Destroy(gameObject,0.6f);
    }

    
   /* 
    void Shoot()
    {
        //조준방향으로 발사..추적기능 on 
        this.transform.position = Vector3.MoveTowards(this.transform.position, aimPosition, bulletSpeed * Time.deltaTime);
    }
   */


    // Start is called before the first frame update
    void Start()
    {
        aimPosition = new Vector3(target.position.x, target.position.y + 0.5f, target.position.z);
        transform.LookAt(aimPosition);
        StartCoroutine(SimulateProjectile());
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            
          //Shoot();
            
        } 
        else
        {
            Destroy(gameObject); //적 소멸시 자체 파괴
        }
    }
}
