using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//k all
public class NormalBullet : MonoBehaviour, BulletInterFace
{
    public float LV;
    public float bulletSpeed;
    public float bulletDamage;
    private Transform target;
    public GameObject impactParticle;
    public Vector3 aimPosition;
    private AudioSource musicPlayer;
    public AudioClip shootSound;
    
    public void SetUp(BulletInfo bulletinfo)
    {
        this.LV = bulletinfo.LV;
        this.bulletSpeed = bulletinfo.bulletSpeed;
        this.target = bulletinfo.attackTarget;
        this.bulletDamage = bulletinfo.bulletDamage;
    }

    private void OnTriggerEnter(Collider other) //적과 충돌시 상호작용
    {
        if(other.transform != target) return;
        if ( other.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;
        if (other.gameObject.GetComponent<EnemyInterFace>() == null) return;
        other.GetComponent<EnemyInterFace>().GetDamage(bulletDamage);
        

        //hit particle spawn
        GameObject clone = Instantiate(impactParticle, target.gameObject.GetComponent<EnemyInterFace>().GetBodyPos().position, Quaternion.identity) as GameObject;
        clone.transform.parent = target.transform;
        Destroy(clone, 1f);

        //destroy bullet prefab
        Destroy(gameObject,0.22f);
      
    }

    IEnumerator DestroyIfNoTarget()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject clone = Instantiate(impactParticle, this.gameObject.transform.position, Quaternion.identity) as GameObject;
        Destroy(gameObject);
        Destroy(clone, 1f);
    }

    void Shoot()
    {
        //aim and shoot
        aimPosition = target.gameObject.GetComponent<EnemyInterFace>().GetBodyPos().position; //get the position to shoot.. it tracks enemy 
        transform.LookAt(aimPosition);
        this.transform.position = Vector3.MoveTowards(this.transform.position, aimPosition, bulletSpeed * Time.deltaTime);
    }
   
    // Start is called before the first frame update
    void Start()
    {
        musicPlayer = GetComponent<AudioSource>();
        musicPlayer.clip = shootSound;
        musicPlayer.time = 0;
        musicPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
        if (target!= null && target.gameObject.GetComponent<EnemyInterFace>().CheckDead())
        {
          
            StartCoroutine(DestroyIfNoTarget());
        }
     
        else if (target == null)
        {
            StartCoroutine(DestroyIfNoTarget());
        }
    }


}
