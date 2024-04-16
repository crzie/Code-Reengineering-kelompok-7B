using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutUI : MonoBehaviour
{
    [SerializeField] private Image icon1;
    [SerializeField] private Image icon2;
    [SerializeField] private Image icon3;
    [SerializeField] private Image icon4;

    private PlayerData player;

    private static LoadoutUI instance;

    public static LoadoutUI Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LoadoutUI>();
            }
            return instance;
        }
    }

    private void Start()
    {
        player = PlayerData.Instance;
        Refresh();
    }

    public void Refresh()
    {
        Color opaque = new Color(1, 1, 1, 1);

        icon1.sprite = player.GetFromLoadout(0)?.icon;
        icon2.sprite = player.GetFromLoadout(1)?.icon;
        icon3.sprite = player.GetFromLoadout(2)?.icon;
        icon4.sprite = player.GetFromLoadout(3)?.icon;

        icon1.color = (icon1.sprite != null) ? opaque : icon1.color;
        icon2.color = (icon2.sprite != null) ? opaque : icon2.color;
        icon3.color = (icon3.sprite != null) ? opaque : icon3.color;
        icon4.color = (icon4.sprite != null) ? opaque : icon4.color;
    }
}
