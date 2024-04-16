using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalToMaze : MonoBehaviour
{
    [SerializeField] private GameObject areaWarning;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            areaWarning.GetComponent<AreaWarning>().ShowWarning("Hendi the Rubick's Maze", 50, 60, "MazeScene");
        }
    }
}
