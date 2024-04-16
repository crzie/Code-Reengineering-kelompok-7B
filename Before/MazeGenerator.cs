using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.AI;

enum Rotation : int
{
    Front = 0,
    Right = 90,
    Back = 180,
    Left = 270
}

public class MazeGenerator : MonoBehaviour
{
    private Node[,] firstFloor;
    private Node[,] secondFloor;
    
    public int MazeSize = 20;
    public float NodeSize = 4f;
    public static float SecondFloorOffset = 10f;

    [SerializeField] private Node nodeComponent;
    [SerializeField] private Room roomType1;
    [SerializeField] private Room roomType2;
    [SerializeField] private Room roomType3;
    [SerializeField] private Room spawnRoom;
    [SerializeField] private Room endRoom;
    [SerializeField] private Node teleporter;

    private List<Vector2> vertices1 = new List<Vector2>();
    private List<Vector2> vertices2 = new List<Vector2>();
    private List<Edge> edges1 = new List<Edge>();
    private List<Edge> edges2 = new List<Edge>();

    private Vector2 spawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        PlaceNodes();
        PlaceRooms();
        PlaceTeleporters();

        edges1 = Prim.GetMSTEdges(vertices1);
        edges2 = Prim.GetMSTEdges(vertices2);

        ConnectVertices();
        ClearUnusedNodes();

        Player.Instance.transform.position = new Vector3(spawnPosition.x, 1, spawnPosition.y);

        //BuildNavigation();
    }

    void PlaceNodes()
    {
        firstFloor = new Node[MazeSize, MazeSize];
        secondFloor = new Node[MazeSize, MazeSize];

        for (int i = 0; i < MazeSize; i++)
        {
            for (int j = 0; j < MazeSize; j++)
            {
                Node node = Instantiate(nodeComponent, new Vector3(j * NodeSize, 0, i * NodeSize), Quaternion.identity);

                firstFloor[j, i] = node;
                //node.name = j + ", " + i;

                Node node2 = Instantiate(nodeComponent, new Vector3(j * NodeSize, 0 + SecondFloorOffset, i * NodeSize), Quaternion.identity);

                secondFloor[j, i] = node2;
            }
        }
    }

    void PlaceRooms()
    {
        Rotation spawnRotation = GetRandomRotation();

        CalculateAndPlaceRoom(spawnRoom, spawnRotation, 1);

        //PlaceRoom(spawnRoom, 0, 0, Rotation.Front, 1);

        // 1st floor
        int room1Count = 2;
        int room2Count = 3;
        int room3Count = 2;

        for (int i = 0; i < room1Count; i++)
        {
            Rotation rotation = GetRandomRotation();

            CalculateAndPlaceRoom(roomType1, rotation, 1);
        }
        for (int i = 0; i < room2Count; i++)
        {
            Rotation rotation = GetRandomRotation();

            CalculateAndPlaceRoom(roomType2, rotation, 1);
        }
        for (int i = 0; i < room3Count; i++)
        {
            Rotation rotation = GetRandomRotation();

            CalculateAndPlaceRoom(roomType3, rotation, 1);
        }

        // 2nd floor
        Rotation endRotation = GetRandomRotation();

        CalculateAndPlaceRoom(endRoom, endRotation, 2);

        room1Count = 3;
        room2Count = 4;
        room3Count = 2;

        for (int i = 0; i < room1Count; i++)
        {
            Rotation rotation = GetRandomRotation();

            CalculateAndPlaceRoom(roomType1, rotation, 2);
        }
        for (int i = 0; i < room2Count; i++)
        {
            Rotation rotation = GetRandomRotation();

            CalculateAndPlaceRoom(roomType2, rotation, 2);
        }
        for (int i = 0; i < room3Count; i++)
        {
            Rotation rotation = GetRandomRotation();

            CalculateAndPlaceRoom(roomType3, rotation, 2);
        }
    }

    void PlaceTeleporters()
    {
        int x = 0, y = 0;
        bool isLegit;
        float teleporterDistance = MazeSize / 2;
        int teleporterCount = 2;
        Vector2 lastTeleporterPos = Vector2.zero;

        for(int i = 0; i < teleporterCount; i++)
        {
            do
            {
                x = GetRandomNumber(1, MazeSize - 2);
                y = GetRandomNumber(1, MazeSize - 2);

                // position legit for 2 floors
                isLegit = IsLegitPosition(x-1, y-1, 3, 3, 1) && IsLegitPosition(x-1, y-1, 3, 3, 2);

                if(isLegit && i == 0)
                {
                    break;
                }
            } while (!isLegit || Vector2.Distance(lastTeleporterPos, new Vector2(x, y)) < teleporterDistance);

            //ClearNodes(x, y, 1, 1, 1);
            //ClearNodes(x, y, 1, 1, 2);
            firstFloor[x, y].gameObject.SetActive(false);
            secondFloor[x, y].gameObject.SetActive(false);
            Node tp1 = Instantiate(teleporter, new Vector3(x * NodeSize, 0, y * NodeSize), Quaternion.identity);
            Node tp2 = Instantiate(teleporter, new Vector3(x * NodeSize, 0 + SecondFloorOffset, y * NodeSize), Quaternion.identity);
            tp1.GetComponentInChildren<Teleporter>().floor = 1;
            tp2.GetComponentInChildren<Teleporter>().floor = 2;

            vertices1.Add(new Vector2(x, y));
            vertices2.Add(new Vector2(x, y));

            lastTeleporterPos = new Vector2(x, y);

            firstFloor[x, y] = tp1;
            secondFloor[x, y] = tp2;
        }
    }

    void ConnectVertices()
    {
        foreach(Edge edge in edges1)
        {
            AStarConnect(edge, 1);
        }
        foreach(Edge edge in edges2)
        {
            AStarConnect(edge, 2);
        }
    }
    
    void AStarConnect(Edge edge, int floor)
    {
        Vector2Int source = new((int)edge.Vertex1.x, (int)edge.Vertex1.y);
        Vector2Int dest = new((int)edge.Vertex2.x, (int)edge.Vertex2.y);
        AStarNode currNode;

        AStarNode[,] pathMap = new AStarNode[MazeSize, MazeSize];

        PriorityQueue<AStarNode> queue = new PriorityQueue<AStarNode>();

        for (int i = 0; i < MazeSize; i++)
        {
            for (int j = 0; j < MazeSize; j++)
            {
                pathMap[j, i] = new AStarNode(new Vector2Int(j, i));
                if (floor == 1 && !firstFloor[j, i].isBuildable) pathMap[j, i].isVisited = true; // if the node has room
                else if(floor == 2 && !secondFloor[j, i].isBuildable) pathMap[j, i].isVisited = true;
            }
        }

        // initialize source
        AStarNode pathSource = pathMap[source.x, source.y];

        pathSource.isVisited = true;
        pathSource.cost = 0;
        currNode = pathSource;

        while (currNode.position.x != dest.x || currNode.position.y != dest.y)
        {
            // define possible paths
            if (currNode.position.y + 1 < MazeSize && !pathMap[currNode.position.x, currNode.position.y + 1].isVisited)
            {
                AStarNode neighborNode = pathMap[currNode.position.x, currNode.position.y + 1];
                float cost = currNode.cost + Vector2.Distance(currNode.position, neighborNode.position);
                queue.Enqueue(new(new(currNode.position.x, currNode.position.y + 1), cost, currNode), cost + Vector2.Distance(neighborNode.position, dest));
            }
            if (currNode.position.x + 1 < MazeSize && !pathMap[currNode.position.x + 1, currNode.position.y].isVisited)
            {
                AStarNode neighborNode = pathMap[currNode.position.x + 1, currNode.position.y];
                float cost = currNode.cost + Vector2.Distance(currNode.position, neighborNode.position);
                queue.Enqueue(new(new(currNode.position.x + 1, currNode.position.y), cost, currNode), cost + Vector2.Distance(neighborNode.position, dest));
            }
            if (currNode.position.y - 1 >= 0 && !pathMap[currNode.position.x, currNode.position.y - 1].isVisited)
            {
                AStarNode neighborNode = pathMap[currNode.position.x, currNode.position.y - 1];
                float cost = currNode.cost + Vector2.Distance(currNode.position, neighborNode.position);
                queue.Enqueue(new(new(currNode.position.x, currNode.position.y - 1), cost, currNode), cost + Vector2.Distance(neighborNode.position, dest));
            }
            if (currNode.position.x - 1 >= 0 && !pathMap[currNode.position.x - 1, currNode.position.y].isVisited)
            {
                AStarNode neighborNode = pathMap[currNode.position.x - 1, currNode.position.y];
                float cost = currNode.cost + Vector2.Distance(currNode.position, neighborNode.position);
                queue.Enqueue(new(new(currNode.position.x - 1, currNode.position.y), cost, currNode), cost + Vector2.Distance(neighborNode.position, dest));
            }

            // pick closest way
            //Debug.Log(queue.Count);
            //Debug.Log(source);
            //Debug.Log(dest);
            AStarNode nextNode = queue.GetFirstAndDequeue();
            pathMap[nextNode.position.x, nextNode.position.y].isVisited = true;
            pathMap[nextNode.position.x, nextNode.position.y].cost = nextNode.cost;
            pathMap[nextNode.position.x, nextNode.position.y].parent = nextNode.parent;

            //aStarPath.Add(new Edge(currNode.position, pathMap[nextNode.position.x, nextNode.position.y].position));

            currNode = pathMap[nextNode.position.x, nextNode.position.y];
        }

        AStarNode pathDest = pathMap[dest.x, dest.y];
        currNode = pathMap[pathDest.position.x, pathDest.position.y];

        while (currNode.parent != null)
        {
            // clear the way
            if (currNode.position.x - 1 == currNode.parent.position.x) // go to left
            {
                if(floor == 1)
                {
                    firstFloor[currNode.position.x, currNode.position.y].OpenPath("left");
                    firstFloor[currNode.parent.position.x, currNode.parent.position.y].OpenPath("right");
                }
                else if(floor == 2)
                {
                    secondFloor[currNode.position.x, currNode.position.y].OpenPath("left");
                    secondFloor[currNode.parent.position.x, currNode.parent.position.y].OpenPath("right");
                }
            }
            else if (currNode.position.x + 1 == currNode.parent.position.x) // go right
            {
                if(floor == 1)
                {
                    firstFloor[currNode.position.x, currNode.position.y].OpenPath("right");
                    firstFloor[currNode.parent.position.x, currNode.parent.position.y].OpenPath("left");
                }
                else if(floor == 2)
                {
                    secondFloor[currNode.position.x, currNode.position.y].OpenPath("right");
                    secondFloor[currNode.parent.position.x, currNode.parent.position.y].OpenPath("left");
                }
            }
            else if (currNode.position.y + 1 == currNode.parent.position.y) // go front
            {
                if(floor == 1)
                {
                    firstFloor[currNode.position.x, currNode.position.y].OpenPath("front");
                    firstFloor[currNode.parent.position.x, currNode.parent.position.y].OpenPath("back");
                }
                else if(floor == 2)
                {
                    secondFloor[currNode.position.x, currNode.position.y].OpenPath("front");
                    secondFloor[currNode.parent.position.x, currNode.parent.position.y].OpenPath("back");
                }
            }
            else if (currNode.position.y - 1 == currNode.parent.position.y) // go back
            {
                if (floor == 1)
                {
                    firstFloor[currNode.position.x, currNode.position.y].OpenPath("back");
                    firstFloor[currNode.parent.position.x, currNode.parent.position.y].OpenPath("front");
                }
                else if (floor == 2)
                {
                    secondFloor[currNode.position.x, currNode.position.y].OpenPath("back");
                    secondFloor[currNode.parent.position.x, currNode.parent.position.y].OpenPath("front");
                }
            }

            if (floor == 1) firstFloor[currNode.position.x, currNode.position.y].isPath = true;
            else if(floor == 2) secondFloor[currNode.position.x, currNode.position.y].isPath = true;
            currNode = currNode.parent;
        }

        if(floor == 1) firstFloor[currNode.position.x, currNode.position.y].isPath = true;
        if(floor == 2) secondFloor[currNode.position.x, currNode.position.y].isPath = true;
    }

    void ClearUnusedNodes()
    {
        for (int i = 0; i < MazeSize; i++)
        {
            for (int j = 0; j < MazeSize; j++)
            {
                if (firstFloor[j, i].isBuildable && !firstFloor[j, i].isPath)
                {
                    Destroy(firstFloor[j, i].gameObject);
                    firstFloor[j, i] = null;
                }
                if (secondFloor[j, i].isBuildable && !secondFloor[j, i].isPath)
                {
                    Destroy(secondFloor[j, i].gameObject);
                    secondFloor[j, i] = null;
                }
            }
        }
    }

    void BuildNavigation()
    {
        for (int i = 0; i < MazeSize; i++)
        {
            for(int j = 0; j < MazeSize; j++)
            {
                if (firstFloor[j, i] != null) firstFloor[j, i].GetComponentInChildren<NavMeshSurface>()?.BuildNavMesh();
                if (secondFloor[j, i] != null) secondFloor[j, i].GetComponentInChildren<NavMeshSurface>()?.BuildNavMesh();
            }
        }
    }

    void CalculateAndPlaceRoom(Room room, Rotation rotationType, int floor)
    {
        int x = 0, y = 0;
        bool isLegit;

        if (rotationType == Rotation.Front)
        {
            do
            {
                x = GetRandomNumber(1, MazeSize - room.length - 1); // 1 - 15
                y = GetRandomNumber(1, MazeSize - room.width - 1); // 1 - 16

                isLegit = IsLegitPosition(x-1, y-1, room.length+2, room.width+2, floor); // +2 untuk cek depan pintu dan belakang room
            } while (!isLegit);

            if (room == spawnRoom) spawnPosition = new Vector2((x + 0.5f) * NodeSize, (y + 0.5f) * NodeSize);
        }
        else if (rotationType == Rotation.Back)
        {
            do
            {
                x = GetRandomNumber(room.length, MazeSize - 2); // 4 - 18
                y = GetRandomNumber(room.width, MazeSize - 2); // 3 - 18

                int xChecking = x - room.length;
                int yChecking = y - room.width;

                isLegit = IsLegitPosition(xChecking, yChecking, room.length+2, room.width+2, floor);
            } while (!isLegit);

            if (room == spawnRoom) spawnPosition = new Vector2((x - 0.5f) * NodeSize, (y - 0.5f) * NodeSize);
        }
        else if (rotationType == Rotation.Left)
        {
            do
            {
                x = GetRandomNumber(room.width, MazeSize - 2); // 3 - 18
                y = GetRandomNumber(1, MazeSize - room.length - 1); // 1 - 15

                int xChecking = x - room.width;

                isLegit = IsLegitPosition(xChecking, y-1, room.width+2, room.length+2, floor);
            } while (!isLegit);

            if (room == spawnRoom) spawnPosition = new Vector2((x - 0.5f) * NodeSize, (y + 0.5f) * NodeSize);
        }
        else if (rotationType == Rotation.Right)
        {
            do
            {
                x = GetRandomNumber(1, MazeSize - room.width - 1); // 1 - 16
                y = GetRandomNumber(room.length, MazeSize - 2); // 4 - 18

                int yChecking = y - room.length;

                isLegit = IsLegitPosition(x-1, yChecking, room.width+2, room.length+2, floor);
            } while (!isLegit);

            if (room == spawnRoom) spawnPosition = new Vector2((x + 0.5f) * NodeSize, (y - 0.5f) * NodeSize);
        }

        PlaceRoom(room, x, y, rotationType, floor);
    }

    void PlaceRoom(Room room, int x, int y, Rotation rotationType, int floor)
    {   
        if(rotationType == Rotation.Front)
        {
            ClearNodes(x, y, room.length, room.width, floor);

            if (floor == 1)
            {
                vertices1.Add(new Vector2(x, y) + room.doorPosition);
                firstFloor[x + (int)room.doorPosition.x, y + (int)room.doorPosition.y].OpenPath("back");
            }
            else if (floor == 2)
            {
                vertices2.Add(new Vector2(x, y) + room.doorPosition);
                secondFloor[x + (int)room.doorPosition.x, y + (int)room.doorPosition.y].OpenPath("back");
            }
        }
        else if(rotationType == Rotation.Back)
        {
            int xClearing = x - room.length + 1;
            int yClearing = y - room.width + 1;

            ClearNodes(xClearing, yClearing, room.length, room.width, floor);

            if (floor == 1)
            {
                vertices1.Add(new Vector2(x, y) - room.doorPosition);
                firstFloor[x - (int)room.doorPosition.x, y - (int)room.doorPosition.y].OpenPath("front");
            }
            else if (floor == 2)
            {
                vertices2.Add(new Vector2(x, y) - room.doorPosition);
                secondFloor[x - (int)room.doorPosition.x, y - (int)room.doorPosition.y].OpenPath("front");
            }
        }
        else if(rotationType == Rotation.Left)
        {
            int xClearing = x - room.width + 1;

            // ke samping, jd len = width, width = len
            ClearNodes(xClearing, y, room.width, room.length, floor);

            if (floor == 1)
            {
                vertices1.Add(new Vector2(x - room.doorPosition.y, y + room.doorPosition.x));
                firstFloor[x - (int)room.doorPosition.y, y + (int)room.doorPosition.x].OpenPath("right");
            }
            else if (floor == 2)
            {
                vertices2.Add(new Vector2(x - room.doorPosition.y, y + room.doorPosition.x));
                secondFloor[x - (int)room.doorPosition.y, y + (int)room.doorPosition.x].OpenPath("right");
            }
        }
        else if(rotationType == Rotation.Right) 
        {
            int yClearing = y - room.length + 1;

            ClearNodes(x, yClearing, room.width, room.length, floor);

            if (floor == 1)
            {
                vertices1.Add(new Vector2(x + room.doorPosition.y, y - room.doorPosition.x));
                firstFloor[x + (int)room.doorPosition.y, y - (int)room.doorPosition.x].OpenPath("left");
            }
            else if (floor == 2)
            {
                vertices2.Add(new Vector2(x + room.doorPosition.y, y - room.doorPosition.x));
                secondFloor[x + (int)room.doorPosition.y, y - (int)room.doorPosition.x].OpenPath("left");
            }
        }

        Quaternion rotation = Quaternion.Euler(0, (float) rotationType, 0);
        Room roomCreated = room;

        if (floor == 1) roomCreated = Instantiate(room, new Vector3(x * NodeSize, 0, y * NodeSize), rotation);
        else if (floor == 2) roomCreated = Instantiate(room, new Vector3(x * NodeSize, 0 + SecondFloorOffset, y * NodeSize), rotation);

        if (room == endRoom)
        {
            var loadingScreen = roomCreated.GetComponentInChildren<LoadingScreen>();
            loadingScreen.gameObject.SetActive(false);
            loadingScreen.playerUI = GameObject.FindGameObjectWithTag("PlayerUI");
        }
    }

    void ClearNodes(int x, int y, int xSize, int ySize, int floor)
    {
        for(int i = 0; i < ySize;i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                if(floor == 1)
                { 
                    firstFloor[j + x, i + y].gameObject.SetActive(false);
                    firstFloor[j + x, i + y].isBuildable = false;
                }
                else if (floor == 2)
                {
                    secondFloor[j + x, i + y].gameObject.SetActive(false);
                    secondFloor[j + x, i + y].isBuildable = false;
                }
            }
        }
    }

    bool IsLegitPosition(int x, int y, int xSize, int ySize, int floor)
    {
        for (int i = 0; i < ySize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                if (floor == 1 && !firstFloor[j + x, i + y].isBuildable) return false;
                if (floor == 2 && !secondFloor[j + x, i + y].isBuildable) return false;
            }
        }

        return true;
    }

    Rotation GetRandomRotation()
    {
        Rotation[] values = (Rotation[]) Enum.GetValues(typeof(Rotation));

        int randomizer = GetRandomNumber(values.Length-1);

        return values[randomizer];
    }

    int GetRandomNumber(int limit)
    {
        return UnityEngine.Random.Range(0, int.MaxValue) % (limit+1);
    }

    int GetRandomNumber(int start, int limit)
    {
        return UnityEngine.Random.Range(0, int.MaxValue) % (limit-start + 1) + start;
    }
}
