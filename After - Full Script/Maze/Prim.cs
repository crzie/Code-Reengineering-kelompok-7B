using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prim
{
    public static List<Edge> GetMSTEdges(List<Vector2> vertices)
    {
        List<Edge> edgeList = new List<Edge>();
        PriorityQueue<Edge> queue = new PriorityQueue<Edge>();

        List<Vector2> unvisited = new List<Vector2>(vertices);
        List<Vector2> visited = new List<Vector2>
        {
            vertices[0]
        };
        unvisited.Remove(vertices[0]);

        Vector2 lastVisited = vertices[0];

        while(unvisited.Count > 0)
        {
            foreach (Vector2 vertex in unvisited)
            {
                queue.Enqueue(new Edge(lastVisited, vertex), Vector2.Distance(lastVisited, vertex));
            }

            bool tookEdge = false;
            while (!tookEdge)
            {
                Edge takenEdge = queue.GetFirstAndDequeue();
                if (!visited.Contains(takenEdge.Vertex1))
                {
                    lastVisited = takenEdge.Vertex1;
                    visited.Add(lastVisited);
                    edgeList.Add(takenEdge);
                    tookEdge = true;
                }
                else if (!visited.Contains(takenEdge.Vertex2))
                {
                    lastVisited = takenEdge.Vertex2;
                    visited.Add(lastVisited);
                    edgeList.Add(takenEdge);
                    tookEdge = true;
                }
            }

            unvisited.Remove(lastVisited);
        }

        return edgeList;
    }
}
