﻿using UnityEngine;
using System.Linq;
using System.Collections;

namespace Procedural
{
    public class TextureGenerator : MonoBehaviour
    {
        [Header("Textures' Index")]
        public int grass_textureIndex = 0;
        public int forestGround1_textureIndex = 1;
        public int forestGround2_textureIndex = 2;
        public int rock_textureIndex = 3;

        [Header("Hills")]
        [Range(0,90)] public float minHillSlope;

        [Header("Plains")]
        [Range(0,1)] public float plainDensity;
        [Range(0,1)] public float plainPersistance;
        [Min(1)] public float plainFrequency;

        [Header("Forest Soil")]
        [Range(0,1)] public float forestSoilDensity;
        [Range(0,1)] public float forestSoilPersistance;
        [Min(1)] public float forestSoilFrequency;


        // public TerrainLayer[] terrainLayers;
        // [Min(0)] public int defaultTextureIndex;

        public IEnumerator GenerateTextures(TerrainData terrainData, TerrainType[][] terrainTypeMap, float maxAltitude, System.Random rdm, TMPro.TextMeshProUGUI textSub)
        {
            int width = terrainData.alphamapWidth;
            int height = terrainData.alphamapHeight;
            int nbTextures = terrainData.alphamapLayers;
            
            textSub.text = "Creating Steepness Map..";
            yield return null;
            float[,] steepnessMap = GenerateSteepnessMap(terrainData, width, height);
            textSub.text = "Generating Grass Noise Map..";
            yield return null;
            float[,] grassMap = PerlinNoise.GenerateNoiseMap(width,height, width/plainFrequency, 9, plainPersistance, 2, rdm, Vector2.zero);
            textSub.text = "Creating Terrain Type Map..";
            yield return null;
            GenerateTerrainTypeMap(width, height, steepnessMap,grassMap,terrainTypeMap);
            float[,,] splatMap = new float[width,height,nbTextures];

            textSub.text = "Generating Forest Soil Noise Map..";
            yield return null;
            float[,] forestSoilMap = PerlinNoise.GenerateNoiseMap(width,height, width/forestSoilFrequency, 9, forestSoilPersistance, 2, rdm, Vector2.zero);

            textSub.text = "Applying Textures to Splat Map..";
            yield return null;
            for(int y=0; y<height; y++){
                for(int x=0; x<width; x++){
                    float[] textureWeightMap = new float[nbTextures];

                    switch(terrainTypeMap[y][x])
                    {
                        case TerrainType.Plain:
                            textureWeightMap[grass_textureIndex] = 1f;
                            break;
                        case TerrainType.Forest:
                            if(forestSoilMap[y,x] < forestSoilDensity)
                                textureWeightMap[forestGround2_textureIndex] = 1f;
                            else
                                textureWeightMap[forestGround1_textureIndex] = 1f;
                            break;
                        case TerrainType.Hill:
                            textureWeightMap[rock_textureIndex] = 1f;
                            break;
                    }

                    // Vector2 normPos = new Vector2((float)x/width,(float)y/height);

                    // float altitude = terrainData.GetHeight(Mathf.RoundToInt(normPos.x),Mathf.RoundToInt(normPos.y));
                    // float n_altitude = altitude/maxAltitude;

                    // //Weight list of textures for mixing them correctly, sum must be 1;
                    // float[] splatWeight = new float[nbTextures];

                    // //Register each texture's weight
                    // for(int i=0; i<nbTextures; i++){
                    //     splatMap[y,x,i] = 0f; 
                    //     splatWeight[i] = GetTextureValueAt(terrainLayers[i],n_altitude,steepness);
                    // }

                    float weightSum = textureWeightMap.Sum();

                    //Normalize the weight to make them add to 1
                    for(int i=0; i<nbTextures; i++){
                        splatMap[x,y,i] = textureWeightMap[i] / weightSum;
                    }
                }
            }

            terrainData.SetAlphamaps(0,0,splatMap);
        }

        private float[,] GenerateSteepnessMap(TerrainData terrainData, int width, int height)
        {
            float[,] steepnessMap = new float[height,width];
            for(int y=0; y<height; y++){
                for(int x=0; x<width; x++){
                    Vector2 normPos = new Vector2((float)x/width,(float)y/height);
                    steepnessMap[y,x] = terrainData.GetSteepness(normPos.y, normPos.x);
                }
            }
            return steepnessMap;
        }

        private void GenerateTerrainTypeMap(int width, int height, float[,] steepnessMap, float[,] grassMap,TerrainType[][] terrainTypeMap)
        {
            
            for(int y=0; y<height; y++){
                for(int x=0; x<width; x++){
                    float steepness = steepnessMap[y,x];

                    if (steepness > minHillSlope)
                        terrainTypeMap[y][x] = TerrainType.Hill;
                    else if (grassMap[y,x] < plainDensity)
                        terrainTypeMap[y][x] = TerrainType.Plain;
                    else
                        terrainTypeMap[y][x] = TerrainType.Forest;
                }
            }
        }

        public float GetTextureValueAt(TerrainLayer ground, float n_altitude, float steepness)
        {
            if (n_altitude < ground.minHeight || n_altitude > ground.maxHeight || steepness < ground.minSlope || steepness > ground.maxSlope)
                return 0;
            else
                return 1f;
        }



        [System.Serializable]
        public struct TerrainLayer
        {
            public string name;
            public int index;
            public TerrainType terrainType;

            [Header("Height Level")]
            [Range(0,1)] public float minHeight;
            [Range(0,1)] public float maxHeight;

            [Header("Slope Interval")]
            [Range(0,90)] public float minSlope;
            [Range(0,90)] public float maxSlope;
        }
    }
}