﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//k all
public class NormalBombBullet : Bullet_base, BulletInterFace
{
    [Header("Gameobject to add")]
    public GameObject BombAreaEffect;
    public GameObject impactParticle;
    GameObject ShootArea;

    [Header("BulletInfo")]
    public float LV;
    public string BulletName;
    public float bulletSpeed;
    public float bulletDamage;

    [Header("Palabora inspection")]
    public Transform target;
    public Transform Projectile;
    private Transform myTransform;
    public float firingAngle = 45.0f;
    public float gravity = 9.8f;
    public float BombRadius;
    private bool isShooting = false;


    private AudioSource musicPlayer;
    public AudioClip shootSound;



    RaycastHit hit;
    public void SetUp(BulletInfo bulletinfo)
    {
        isShooting = true;
        this.LV = bulletinfo.LV;
        this.bulletSpeed = bulletinfo.bulletSpeed;
        this.target = bulletinfo.attackTarget;
        this.bulletDamage = bulletinfo.bulletDamage;
        this.BombRadius = bulletinfo.bombRange;
        Projectile = this.transform;
    }


    IEnumerator SimulateProjectile()
    {

        ShootArea = Instantiate(BombAreaEffect, new Vector3(target.position.x, 0f, target.position.z), BombAreaEffect.transform.rotation);
        ChangeAreaScale(ShootArea);
        Projectile.position = this.transform.position + new Vector3(0, 0.0f, 0);

        // Calculate distance to target
        float target_Distance = Vector3.Distance(Projectile.position, target.position);

        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
        float flightDuration = target_Distance / Vx;
        Projectile.rotation = Quaternion.LookRotation(target.position - Projectile.position);
        float elapse_time = 0;
        Destroy(ShootArea, flightDuration / bulletSpeed + 0.22f);
        //Disable(ShootArea, flightDuration / bulletSpeed + 0.22f);
        while (elapse_time < flightDuration)
        {
            Projectile.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime * bulletSpeed, Vx * Time.deltaTime * bulletSpeed);
            elapse_time += Time.deltaTime * bulletSpeed;
            yield return null;
        }
        BulletObjectPull.ReturnObject(this.gameObject);
        //Destroy(gameObject);

    }

    private void ChangeAreaScale(GameObject ShootArea)
    {
        switch (BombRadius)
        {
            case 1.5f:
                ShootArea.transform.localScale = new Vector3(0.371638f, 0.371638f, 0.371638f);
                break;
            case 2.5f:
                ShootArea.transform.localScale = new Vector3(0.6041434f, 0.6041434f, 0.6041434f);
                break;
            case 3.5f:
                ShootArea.transform.localScale = new Vector3(0.84248f, 0.84248f, 0.84248f);
                break;
            case 2f:
                ShootArea.transform.localScale = new Vector3(0.4783712f, 0.4783712f, 0.4783712f);
                break;
            case 3f:
                ShootArea.transform.localScale = new Vector3(0.728966f, 0.728966f, 0.728966f);
                break;
        }


    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Floor")) //|| other.gameObject.layer == LayerMask.NameToLayer("Enemy")
            Explode();
    }

    void Explode()
    {

        //hit particle spawn
        GameObject BoomEffects = Instantiate(impactParticle, Projectile.position, Quaternion.identity) as GameObject;
        Destroy(BoomEffects, 2.2f);
        //Disable(BoomEffects, 2.2f)
        Collider[] colliders = Physics.OverlapSphere(transform.position, BombRadius);


        foreach (Collider searchedObject in colliders)
        {

            if (searchedObject != null && searchedObject.gameObject.tag == "GroundEnemy")
            {

                searchedObject.gameObject.GetComponent<EnemyInterFace>().GetDamage(bulletDamage);
            }
        }
        BulletObjectPull.ReturnObject(this.gameObject);
        //Destroy(gameObject);
    }

    IEnumerator DestroyAfterTime(GameObject Bullet, float Time)
    {
        yield return new WaitForSeconds(Time);
        BulletObjectPull.ReturnObject(this.gameObject);
    }


    void OnDrawGizmos() //폭탄 범위 sphere 표시
    {
        Gizmos.DrawWireSphere(transform.position, BombRadius);
    }

    void MusicPlay()
    {
        musicPlayer = GetComponent<AudioSource>();
        musicPlayer.clip = shootSound;
        musicPlayer.time = 0;
        musicPlayer.volume = Global.soundVolume;
        musicPlayer.Play();
    }


    void OnEnable()
    {

        //DestroyAfterTime(this.gameObject, 3f);
        //Destroy(gameObject, 3f);
        Disable(this.gameObject, 3f);
    }


    // Start is called before the first frame update
    void Start()
    {

    }




    // Update is called once per frame
    void Update()
    {
        if (target && isShooting)
        {
            isShooting = false;
            MusicPlay();
            StartCoroutine(SimulateProjectile());

        }
    }

}
