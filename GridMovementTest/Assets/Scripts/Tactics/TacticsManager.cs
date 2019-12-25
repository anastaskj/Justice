using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TacticsManager : MonoBehaviour
{
    public Tactics[] availableTactics;

    public void SetTactic(Unit u, Tactics t)
    {
        if (u.res.flexibility >= t.flexibilityRequirement)
        {
            u.tactic = t;
        }
    }
}
