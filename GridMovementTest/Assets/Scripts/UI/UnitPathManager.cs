using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPathManager : MonoBehaviour
{
    public GameObject pathPrefabs;

    public List<UnitPath> allPaths;

    private void Awake()
    {
        allPaths = new List<UnitPath>();
    }

    public void CreatePath(IsometricDirection d)
    {

    }
}
