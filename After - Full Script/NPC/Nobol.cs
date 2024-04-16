using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nobol : NPC
{
    public override void Interact()
    {
        UI.GetComponent<AreaWarning>().ShowWarning("Phoebus's Chamber", 60, 80, "BossScene");
    }
}
