using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data", menuName = "Scriptable Object/Enemy Data", order = int.MaxValue)]
public class SkillData : ScriptableObject
{
    /*[SerializeField]
    private string towerName;
    public string TowerName { get { return towerName; } }*/

    [SerializeField]
    private float coolDown;
    public float CoolDown { get { return coolDown; } }
}
