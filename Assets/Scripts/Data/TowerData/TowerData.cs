using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tower Data", menuName = "Scriptable Object/Tower Data", order = int.MaxValue)]
public class TowerData : ScriptableObject
{
    /*[SerializeField]
    private string towerName;
    public string TowerName { get { return towerName; } }*/

    [SerializeField]
    private int price;
    public int Price { get { return price; } }

    [SerializeField]
    private float bulletSpeed;
    public float BulletSpeed { get { return bulletSpeed; } }

    [SerializeField]
    private float bulletDamage;
    public float BulletDamage { get { return bulletDamage; } }

    [SerializeField]
    private float attackRate;
    public float AttackRate { get { return attackRate; } }

    [SerializeField]
    private float attackRange;
    public float AttackRange { get { return attackRange; } }
}
