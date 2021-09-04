using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//k all
public class NormalBombBullet : MonoBehaviour, BulletInterFace
{
    public string BulletName;
    public float bulletSpeed;
    private float bulletDamage;
    private Transform target;
    public GameObject impactParticle;
    public Vector3 aimPosition;
    public float BombRadius = 1.5f;
    RaycastHit hit;

    public void SetUp(BulletInfo bulletinfo)
    {
        this.bulletSpeed = bulletinfo.bulletSpeed;
        this.target = bulletinfo.attackTarget;
        this.bulletDamage = bulletinfo.bulletDamage;
    }

    private void OnTriggerEnter(Collider other) //적과 충돌시 상호작용
    {
        if (other.gameObject.layer != 12) return;
        if(other.transform != target) return;
        Explode();

       
         
        
    }
    void Explode()
    {
       
            
        //hit particle spawn
        GameObject BoomEffects = Instantiate(impactParticle, this.transform.position, Quaternion.identity) as GameObject;
       // BoomEffects.transform.parent = target.transform;
        Destroy(BoomEffects, 3);
        Collider[] colliders = Physics.OverlapSphere(transform.position, BombRadius);

        foreach (Collider searchedObject in colliders)
        {
            if (searchedObject != null && searchedObject.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                switch (searchedObject.tag)
                {
                    case "GroundEnemy":
                        searchedObject.GetComponent<GroundEnemy>().GetDamage(bulletDamage);
                        break;
                    case "FlyingEnemy":
                        searchedObject.GetComponent<FlyingEnemy>().GetDamage(bulletDamage);
                        break;
                }
        }

        Destroy(gameObject);
    }

    void Shoot()
    {
        //조준방향으로 발사..추적기능 on 
        aimPosition = new Vector3(target.position.x, target.position.y + 0.5f, target.position.z); 
        Physics.Raycast(transform.position, aimPosition, out hit);
        transform.LookAt(aimPosition);
        this.transform.position = Vector3.MoveTowards(this.transform.position, aimPosition, bulletSpeed * Time.deltaTime);
    }
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            
            Shoot();
            
        } 
        else
        {
            Destroy(gameObject); //적 소멸시 자체 파괴
        }
    }
}
