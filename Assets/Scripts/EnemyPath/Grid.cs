using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public GameObject groundPrefab;
    GameObject parentGrid;          //groundPrefab의 부모

    public Vector3 gridWorldSize;   //노드의 크기

    Node[,] grid;


    public bool CreateGrid()
    {
        if (gridWorldSize.x < 2 || gridWorldSize.x > 101 || gridWorldSize.z < 2 || gridWorldSize.z > 51)
            return false;

        if (parentGrid != null)
            Destroy(parentGrid);
        parentGrid = new GameObject("parentGrid");

        grid = new Node[(int)gridWorldSize.x, (int)gridWorldSize.z];
        Vector3 worldBottomLeft = Vector3.zero - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.z / 2;

        for (int x = 0; x < (int)gridWorldSize.x; x++)
        {
            for (int z = 0; z < (int)gridWorldSize.z; z++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x + 0.5f) - Vector3.up * 0.5f + Vector3.forward * (z + 0.5f);
                GameObject obj = Instantiate(groundPrefab, worldPoint, Quaternion.Euler(90, 0, 0));
                obj.transform.parent = parentGrid.transform;
                grid[x, z] = new Node(obj, true, x, z);
            }
        }
        return true;
    }

    public void ResetGrid()
    {
        for (int x = 0; x < (int)gridWorldSize.x; x++)
        {
            for (int z = 0; z < (int)gridWorldSize.z; z++)
            {
                if (grid[x, z].walkable && !grid[x, z].start && !grid[x, z].end)
                {
                    grid[x, z].ChangeNode = true;
                }
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        int[,] temp = { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };
        bool[] walkableUDLR = new bool[4];
        for (int i = 0; i < 4; i++)
        {
            int checkX = node.gridX + temp[i, 0];
            int checkZ = node.gridZ + temp[i, 1];
            if (checkX >= 0 && checkX < (int)gridWorldSize.x && checkZ >= 0 && checkZ < (int)gridWorldSize.z)
            {
                if (grid[checkX, checkZ].walkable)
                    walkableUDLR[i] = true;
                neighbours.Add(grid[checkX, checkZ]);
            }
        }
        /*for (int i = 0; i < 4; i++)
        {
            if (walkableUDLR[i] || walkableUDLR[(i + 1) % 4])
            {
                int checkX = node.gridX + temp[i, 0] + temp[(i + 1) % 4, 0];
                int checkZ = node.gridZ + temp[i, 1] + temp[(i + 1) % 4, 1];
                if (checkX >= 0 && checkX < (int)gridWorldSize.x && checkZ >= 0 && checkZ < (int)gridWorldSize.z)
                {
                    neighbours.Add(grid[checkX, checkZ]);
                }
            }
        }*/

        return neighbours;
    }

    public Node NodePoint(Vector3 rayPosition)
    {
        int x = (int)(rayPosition.x + gridWorldSize.x / 2);
        int z = (int)(rayPosition.z + gridWorldSize.z / 2);

        return grid[x, z];
    }

    public Node StartNode
    {
        get
        {
            grid[0, 5].start = true;
            return grid[0, 5];
        }
    }

    public Node EndNode
    {
        get
        {
            grid[(int)gridWorldSize.x - 1, (int)gridWorldSize.z - 5].end = true;
            return grid[(int)gridWorldSize.x - 1, (int)gridWorldSize.z - 5];
        }
    }

}
