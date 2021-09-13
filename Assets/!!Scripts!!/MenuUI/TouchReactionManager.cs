using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchReactionManager : MonoBehaviour
{
    [SerializeField] TouchReaction[] touchReactions;
    int lastUsedObjectIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            DisplayTouchReaction(Input.GetTouch(0).position);
        }
        if (Input.touchCount > 1 && Input.GetTouch(1).phase == TouchPhase.Began)
        {
            DisplayTouchReaction(Input.GetTouch(0).position);
        }

    }

    void DisplayTouchReaction(Vector2 position)
    {
        TouchReaction reactionObject = GetComponentFromPool();
        reactionObject.Display(position);
    }

    TouchReaction GetComponentFromPool()
    {
        lastUsedObjectIndex = (lastUsedObjectIndex + 1) % touchReactions.Length;
        return touchReactions[lastUsedObjectIndex];
    }
}
