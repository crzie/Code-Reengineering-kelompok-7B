using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Node : MonoBehaviour
{
    public GameObject leftWall;
    public GameObject rightWall;
    public GameObject frontWall;
    public GameObject backWall;

    public bool isBuildable = true;
    public bool isPath = false;

    public void OpenPath(string wall)
    {
        switch (wall)
        {
            case "left":
                leftWall.SetActive(false);
                break;
            case "right":
                rightWall.SetActive(false);
                break;
            case "front":
                frontWall.SetActive(false);
                break;
            case "back":
                backWall.SetActive(false);
                break;
        }
    }
}
