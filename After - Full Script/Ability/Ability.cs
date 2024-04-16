using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Ability
{
    [SerializeField] private string name;
    [SerializeField] private string description;
    [SerializeField] private long price;
    public Sprite icon;

    public Ability(string name, string description, long price)
    {
        this.name = name;
        this.description = description;
        this.price = price;
    }

    public string Name{ get { return name; } }
    public string Description { get { return description; } }
    public long Price { get { return price; } }
}
