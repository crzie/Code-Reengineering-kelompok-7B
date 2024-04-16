using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public int floor;
    private float lastUsed = -Mathf.Infinity;
    private const float TeleporterCooldown = 5f;

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("tp in floor" + floor);
        if(Input.GetKeyDown(KeyCode.E) && Time.time - lastUsed > TeleporterCooldown)
        {
            if(floor == 1)
            {
                Player.Instance.transform.position += Vector3.up * MazeGenerator.SecondFloorOffset;
            }
            else if (floor == 2)
            {
                Player.Instance.transform.position += Vector3.down * MazeGenerator.SecondFloorOffset;
            }

            lastUsed = Time.time;
        }
    }
}
