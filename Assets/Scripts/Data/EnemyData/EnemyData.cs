using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data", menuName = "Scriptable Object/Enemy Data", order = int.MaxValue)]
public class EnemyData : ScriptableObject
{
    /*[SerializeField]
    private string towerName;
    public string TowerName { get { return towerName; } }*/

    [SerializeField]
    private float maxHP;
    public float MaxHP { get { return maxHP; } }

    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed { get { return moveSpeed; } }

    [SerializeField]
    private float hitDamage;
    public float HitDamage { get { return hitDamage; } }
}
