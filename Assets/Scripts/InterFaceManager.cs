using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface TowerInterFace
{
    public void SetUp(TowerInfo towerinfo);
}

public interface BulletInterFace
{
    public void SetUp(BulletInfo bullet);
}

public interface EnemyInterFace
{
    public void SetUp();

    public bool CheckDead();

    public void GetDamage(float Damage);

    public float GetSpeed();

    public void SetSpeed(float ApplySpeed);

    public Transform GetHeadPos();

    public Transform GetBodyPos();
}

public struct TowerInfo
{
    public float LV;
    public string type;
    public float bulletSpeed;
    public float bulletDamage;
    public float attackRate;
    public float attackRange;


    //if bomb-type
    public float bombRange;

    //if multiple-shoot
    public float bulletAmmoCount;
    public float burstRate;


    //chain
    public int maxChainCount;
    public float chainRadius;

    //stun
    public float stunDuration;
    public float mujeockTime;
    //poison
    public float poisonDamage;
    public float poisonDuration;
    public float poisonRate;
    //slow
    public float slowIntensity;
    public float slowRange;


}

public struct BulletInfo
{
    public float LV;
    public string type;
    public float bulletSpeed;
    public float bulletDamage;
    public Transform attackTarget;

    //if bomb-type
    public float bombRange;

    //if multiple-shoot
    public float bulletAmmoCount;
    public float burstRate;

    //chain
    public int maxChainCount;
    public float chainRadius;

    //stun
    public float stunDuration;
    public float mujeockTime;
    //poison
    public float poisonDamage;
    public float poisonDuration;
    public float poisonRate;
}

public class InterFaceManager : MonoBehaviour
{


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
