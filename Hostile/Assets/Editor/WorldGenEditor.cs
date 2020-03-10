using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Procedural.WorldGenerator))]
public class WorldGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Procedural.WorldGenerator worldGen = (Procedural.WorldGenerator)target;

        DrawDefaultInspector(); 

        if(GUILayout.Button("Generate Terrain")){
            worldGen.GenerateTerrain();
            worldGen.GenerateTextures();
            worldGen.SpawnGrass();
        }

        if(GUILayout.Button("Generate Textures")){
            worldGen.GenerateTextures();
        }
    }
}
