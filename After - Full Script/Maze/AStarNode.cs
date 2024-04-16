using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode
{
    public Vector2Int position;
    public float cost = Mathf.Infinity;
    public bool isVisited = false;
    public AStarNode parent = null;

    public AStarNode()
    {

    }

    public AStarNode(Vector2Int position)
    {
        this.position = position;
    }

    public AStarNode(Vector2Int position, float cost, AStarNode parent)
    {
        this.position = position;
        this.cost = cost;
        isVisited = true;
        this.parent = parent;
    }
}
