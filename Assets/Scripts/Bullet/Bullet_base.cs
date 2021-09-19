using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_base : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected void Disable(GameObject obj, float delay = 0)
    {
        IEnumerator doDisable(float t)
        {
            yield return new WaitForSeconds(t);
            obj.SetActive(false);
            StopAllCoroutines();
        }
        if (delay == 0)
        {
            obj.SetActive(false);
            StopAllCoroutines();
            return;
        }
        else
        {
            if (obj.activeInHierarchy == true)
                StartCoroutine(doDisable(delay));
        }
    }
}
