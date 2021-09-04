using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class TowerBase : MonoBehaviour
{
    TowerInfo towerinfo= new TowerInfo(); 
    public Vector3[] _myposition;

    [Header("Info about this Tower")]
    [SerializeField] 
    private int price = 30;
    public float bulletSpeed;
    public float bulletDamage;
    public float attackRate;
    public float attackRange;

    [Header("Inspections that should be managed per Tower 유형")]
    [SerializeField]
    private float numberOfBlocks;
    [SerializeField]
    public GameObject[] Towers;

    [Header("is this Tower Built-Temporary Mode ?")]
    public bool isTemp = true;

    
    // Start is called before the first frame update
    public void ConfirmTowerPosition()
    {
        isTemp = false;
        gameObject.layer = LayerMask.NameToLayer("Floor");
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Floor");
        }
    }
    public void SetUp()
    {
        
        towerinfo.bulletSpeed = this.bulletSpeed;
        towerinfo.bulletDamage = this.bulletDamage;
        towerinfo.attackRate = this.attackRate;
        towerinfo.attackRange = this.attackRange;
        
        foreach (GameObject Tower in Towers)
        {
            TowerInterFace tower = Tower.GetComponent<TowerInterFace>();
            tower.SetUp(towerinfo);

        }

     }
    

    void Start()
    {
        if (Towers.Length > 0) //���ݼ� Ÿ�� �����Ҷ� setup ����
            SetUp();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetPrice()
    {
        return price;
    }
}
