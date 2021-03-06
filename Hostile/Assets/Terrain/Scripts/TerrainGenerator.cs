﻿using System.Collections;
using UnityEngine;

namespace Procedural
{
    public class TerrainGenerator : MonoBehaviour
    {
        public Terrain terrain;
        // public enum Drawmode {HeightMap, ColorMap, Mesh}
        // public Drawmode drawmode = Drawmode.Mesh;
        public int width = 128;
        public int height = 128;
        public float maxAltitude = 20;

    #region PerlinValues
        public AnimationCurve heightCurve;
        [Min(0)] public float noiseScale = 100;
        [Min(0)] public int nbOctaves = 8;
        [Range(0f,1f)] public float persistance = 0.5f;
        [Min(1)] public float lacunarity = 2f;
        public Vector2 offset = Vector2.zero;
    #endregion

        public bool autoUpdate;
        public TerrainType[] grounds;
        
        public IEnumerator GenerateTerrain(System.Random rdm,TMPro.TextMeshProUGUI textSub)
        {
            TerrainData terrainData = terrain.terrainData;
            terrainData.heightmapResolution = height+1;
            terrainData.size = new Vector3(height,maxAltitude,width);

            textSub.text = "Generating Procedural Noise Map..";
            yield return null;
            float[,] noiseMap = PerlinNoise.GenerateNoiseMap (width,height, noiseScale, nbOctaves, persistance, lacunarity, rdm, offset);
            textSub.text = "Creating Height Map..";
            yield return null;
            float[,] heightMap = GenerateHeightMap(noiseMap);

            terrainData.SetHeights(0,0,heightMap);
        }

        public float[,] GenerateHeightMap(float[,] noiseMap)
        {
            float[,] heightMap = new float[width,height];

            for(int y=0; y<height; y++){
                for(int x=0; x<width; x++){
                    heightMap[x,y] = heightCurve.Evaluate(noiseMap[x,y]);
                }
            }

            return heightMap;
        }
    }
}