using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    private bool switchBool = true;
    public List<GameObject> wayPointObstacle;
    // Start is called before the first frame update
    
    public void WayObstacleActiveSwitch()
    {
        switchBool = !switchBool;
        for (int i = 0; i < wayPointObstacle.Count; i++)
        {
            wayPointObstacle[i].SetActive(switchBool);
        }
    }
    



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
