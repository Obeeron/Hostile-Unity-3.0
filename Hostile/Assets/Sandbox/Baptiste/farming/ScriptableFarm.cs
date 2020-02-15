using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Loot", menuName = "Loot/farm", order = 1)]
public class ScriptableFarm : ScriptableObject
{
    public string prefabName;

    public int numberOfDrops;
    public Vector3[] spawnPoints;
}