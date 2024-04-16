using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CheatInstance
{
    public string Keyword
    {
        get;
    }

    public delegate void Effect();
    public Effect CheatEffect
    {
        get;
    }

    public CheatInstance(string keyword, Effect effect)
    {
        Keyword = keyword;
        CheatEffect = effect;
    }
}
