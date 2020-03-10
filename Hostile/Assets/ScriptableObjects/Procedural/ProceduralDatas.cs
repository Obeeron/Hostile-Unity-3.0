using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProceduralDatas", menuName = "Procedural/ProceduralDatas", order = 0)]
public class ProceduralDatas : ScriptableObject
{
    [Header("Terrain")]
    [Min(0)] public int width = 100;
    [Min(0)] public int height = 100;
    [Min(0)] public int depth = 20;
    
    [Header("Trees")]
    public float tree_spacing = 9;
    public int tree_nbSpawnTriesMax = 30;

    //[Header("Rocks")]    
}
