using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObjectPull : MonoBehaviour
{
    List<GameObject> pool;
    int lastIndex;
    int index;



    private void Start()
    {
        pool = new List<GameObject>();
        index = 0;
        lastIndex = 0;
    }

    public GameObject GetObject(GameObject prefab)
    {
        if (pool.Count < 1)
        {
            GameObject newObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            // newObject.transform.SetParent(gameObject.transform);
            pool.Add(newObject);
            index = pool.Count - 1;
            lastIndex = index;
            Debug.Log("getObject : " + index);
            newObject.SetActive(true);
            return newObject;
        }
        while (true)
        {
            index = (index + 1) % pool.Count;
            if (pool[index].activeInHierarchy == false)
            {
                lastIndex = index;

                pool[index].SetActive(true);
                return pool[index];
            }
            if (lastIndex == index)
            {
                GameObject newObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                // newObject.transform.SetParent(gameObject.transform);
                pool.Add(newObject);
                index = pool.Count - 1;
                lastIndex = index;

                newObject.SetActive(true);
                return newObject;
            }
        }

    }

    public static void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}
