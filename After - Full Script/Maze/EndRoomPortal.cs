using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRoomPortal : MonoBehaviour
{
    [SerializeField] private GameObject areaWarning;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            areaWarning.GetComponent<AreaWarning>().ShowWarning("Teresa's Childhood Village", 1, 10, "MainScene");
        }
    }
}
