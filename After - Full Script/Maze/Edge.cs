using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    public Vector2 Vertex1
    {
        get;
    }

    public Vector2 Vertex2
    {
        get;
    }

    public Edge(Vector2 vertex1, Vector2 vertex2)
    {
        Vertex1 = vertex1;
        Vertex2 = vertex2;
    }
}
