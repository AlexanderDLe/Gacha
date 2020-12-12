using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FX
{
    Default,
    Wait,
    Damage
}

namespace RPG.Combat
{
    [System.Serializable]
    public class Effect
    {
        FX fxEnum;
        float val;
    }
}