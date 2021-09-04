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
}

public struct TowerInfo
{
    public float bulletSpeed;
    public float bulletDamage;
    public float attackRate;
    public float attackRange;
}

public struct BulletInfo
{
    public float bulletSpeed;
    public float bulletDamage;
    public Transform attackTarget;
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
