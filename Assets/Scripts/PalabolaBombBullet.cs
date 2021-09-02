using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//k all
public class PalabolaBombBullet : MonoBehaviour
{
    public string BulletName;
    public float bulletSpeed;
    private float bulletDamage;
    public Transform target;
    public GameObject impactParticle;
    public Vector3 aimPosition;
    public Transform Projectile;
    private Transform myTransform;
    public float firingAngle = 90.0f;
    public float gravity = 7.8f;
    public float BombRadius=1.5f;

    

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
            Projectile.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime * bulletSpeed, Vx * Time.deltaTime*bulletSpeed);

            elapse_time += Time.deltaTime*bulletSpeed;

            yield return null;
        }
      //  Explode();
    }
    //[출처] [Unity 5] 특정 지점으로 포물선 운동하는 물체 보내기|작성자 호이돌 + 개인 추가코드

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
        if (other.gameObject.layer == LayerMask.NameToLayer("Floor") || other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            Explode();
    }
    void Explode()
    {

        //hit particle spawn
       GameObject BoomEffects = Instantiate(impactParticle, Projectile.position, Quaternion.identity) as GameObject;
       // BoomEffects.transform.parent = target.transform;
        Destroy(BoomEffects, 3);
        Collider[] colliders = Physics.OverlapSphere(transform.position, BombRadius);

        foreach(Collider searchedObject in colliders)
        {
            if (searchedObject!=null &&searchedObject.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                searchedObject.GetComponent<GroundEnemy>().GetDamage(bulletDamage);
        }

        Destroy(gameObject);
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
        //if(target)
       // AimTarget();

    }
}
