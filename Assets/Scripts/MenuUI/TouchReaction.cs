using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchReaction : MonoBehaviour
{
    [SerializeField] float animTime = 1.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Display(Vector2 position)
    {
        gameObject.transform.position = position;
        gameObject.SetActive(true);
        if (DisplayCoroutine != null)
            StopCoroutine(DisplayCoroutine);
        DisplayCoroutine = StartCoroutine(DisplayTouchReaction());

    }

    Coroutine DisplayCoroutine;
    IEnumerator DisplayTouchReaction()
    {
        yield return new WaitForSeconds(animTime);
        gameObject.SetActive(false);
    }
}
