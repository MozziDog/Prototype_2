using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//k all
public class NormalBullet : Bullet_base, BulletInterFace
{
    public float LV;
    public float bulletSpeed;
    public float bulletDamage;
    private Transform target;
    public GameObject impactParticle;
    public Vector3 aimPosition;
    private AudioSource musicPlayer;
    public AudioClip shootSound;
    private bool isShooting = false;
    public void SetUp(BulletInfo bulletinfo)
    {
        musicPlayer=this.GetComponent<AudioSource>();
        MusicPlay();
        this.LV = bulletinfo.LV;
        this.bulletSpeed = bulletinfo.bulletSpeed;
        this.target = bulletinfo.attackTarget;
        this.bulletDamage = bulletinfo.bulletDamage;
        isShooting = true;
    }

    private void OnTriggerEnter(Collider other) //���� �浹�� ��ȣ�ۿ�
    {
        if (other.transform != target) return;
        if (other.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;
        if (other.gameObject.GetComponent<EnemyInterFace>() == null) return;
        other.GetComponent<EnemyInterFace>().GetDamage(bulletDamage);
        isShooting = false;

        //hit particle spawn
        GameObject clone = Instantiate(impactParticle, target.gameObject.GetComponent<EnemyInterFace>().GetBodyPos().position, Quaternion.identity) as GameObject;
        clone.transform.parent = target.transform;
        // Destroy(clone, 1f);
        Disable(clone, 1f);
        //destroy bullet prefab
        // Destroy(gameObject,0.22f);
        Disable(gameObject, 0.22f);

    }

    IEnumerator DestroyIfNoTarget()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject clone = Instantiate(impactParticle, this.gameObject.transform.position, Quaternion.identity) as GameObject;
        //Destroy(gameObject);
        Disable(gameObject);
        // Destroy(clone, 1f);
        Destroy(clone, 1f);
    }

    void MusicPlay()
    {
        musicPlayer.clip = shootSound;
        musicPlayer.time = 0;
        musicPlayer.volume = Global.soundVolume;
        musicPlayer.Play();
    }


    void Shoot()
    {
        //aim and shoot
        aimPosition = target.gameObject.GetComponent<EnemyInterFace>().GetBodyPos().position; //get the position to shoot.. it tracks enemy 
        transform.LookAt(aimPosition);
        this.transform.position = Vector3.MoveTowards(this.transform.position, aimPosition, bulletSpeed * Time.deltaTime);
    }

    // Start is called before the first frame update
    void OnEnable()
    {

        
        Disable(gameObject, 2.3f);
    }

    

    // Update is called once per frame
    void Update()
    {      if(target&&isShooting)
            Shoot();
        if (target != null && target.gameObject.GetComponent<EnemyInterFace>().CheckDead())
        {

            StartCoroutine(DestroyIfNoTarget());
        }

         if (target == null)
        {
            StartCoroutine(DestroyIfNoTarget());
        }
    }

}
