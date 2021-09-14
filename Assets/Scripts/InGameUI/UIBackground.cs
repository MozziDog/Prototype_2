using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBackground : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    TowerManager towerManager;
    // Start is called before the first frame update
    void Start()
    {
        towerManager = GameObject.Find("GameManager").GetComponent<TowerManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickBackground()
    {
        inventory.OnItemUnselected();
        towerManager.OnTowerUnselected();
    }
}
