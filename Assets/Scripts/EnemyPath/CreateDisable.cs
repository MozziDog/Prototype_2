using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDisable : MonoBehaviour
{
    Grid grid;
    Main main;

    private void Awake()
    {
        main = GetComponent<Main>();
        grid = GetComponent<Grid>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Node node = Raycast();
            if (node != null)
            {
                if (node.start || node.end)
                    StartCoroutine("SwitchStartEnd", node);
                else
                    StartCoroutine("ChangeWalkable", node);
            }
        }
    }

    IEnumerator SwitchStartEnd(Node node)
    {
        bool start = node.start;
        Node nodeOld = node;
        while (Input.GetMouseButton(0))
        {
            node = Raycast();
            if(node != null && node != nodeOld)
            {
                if(start && !node.end)
                {
                    node.ChangeStart = true;
                    main.start = node;
                    nodeOld.ChangeStart = false;
                    nodeOld = node;
                }
                else if(!start && !node.start)
                {
                    node.ChangeEnd = true;
                    main.end = node;
                    nodeOld.ChangeEnd = false;
                    nodeOld = node;
                }
            }
            yield return null;
        }
    }

    IEnumerator ChangeWalkable(Node node)
    {
        bool walkable = !node.walkable;

        while (Input.GetMouseButton(0))
        {
            node = Raycast();
            if (node != null) node.ChangeNode = walkable;

            yield return null;
        }
    }

    public Node Raycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100f, 1 << LayerMask.NameToLayer("Grid")))
        {
            GameObject ground = hit.collider.gameObject;
            Debug.Log(ground.transform.gameObject);
            return grid.NodePoint(ground.transform.position);
        }
        return null;
    }
}
