using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObjectPull : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _pullingBullets;
    public static BulletObjectPull bulletObjectPull;
    Dictionary<string,Queue<GameObject>> BulletDictionary;
    //Queue<GameObject> poolingObjectQueue = new Queue<GameObject>();


    private void SetUpDictionary(int Ammo)
    {
      foreach(GameObject Bullet in _pullingBullets)
        {
            bulletObjectPull.BulletDictionary.Add(Bullet.name, new Queue<GameObject>());
            for (int i = 0; i < Ammo; i++)
                bulletObjectPull.BulletDictionary[Bullet.name].Enqueue(CreateNewObject(Bullet));
            

        } 
    }

    private GameObject CreateNewObject(GameObject Bullet)
    {
        GameObject newBullet = Instantiate(Bullet,transform.position,Quaternion.identity);
        newBullet.SetActive(false);
        newBullet.transform.SetParent(this.transform);
        return newBullet;
    }

    public static GameObject GetObject(GameObject Bullet,Vector3 SpawnPosition)
    {
       Queue<GameObject> tempQueue = bulletObjectPull.BulletDictionary[Bullet.name]; 
        if (tempQueue.Count > 0)
        {
            GameObject obj = tempQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.transform.position = SpawnPosition;
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            GameObject newObj = bulletObjectPull.CreateNewObject(Bullet);
            newObj.transform.SetParent(null);
            newObj.transform.position = SpawnPosition;
            newObj.gameObject.SetActive(true);
            return newObj;
        }
       
    }

    public static void ReturnObject(GameObject Bullet)
    {
        string bulletName = GetRightBulletName(Bullet.name);
        Bullet.gameObject.SetActive(false);
        Bullet.transform.SetParent(bulletObjectPull.transform);
        bulletObjectPull.BulletDictionary[bulletName].Enqueue(Bullet);
    }

    private static string GetRightBulletName(string BulletName)
    {
        string str1 = BulletName;
        string str2 = "(Clone)";
        return str1.Replace(str2, "");
    }

    void Awake()
    {
        bulletObjectPull = this;
        BulletDictionary = new Dictionary<string, Queue<GameObject>>();
        SetUpDictionary(10);



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
