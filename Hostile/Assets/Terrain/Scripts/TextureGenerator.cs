using UnityEngine;
using System.Linq;

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

        public void GenerateTextures(TerrainData terrainData,ref TerrainType[,] terrainTypeMap, float maxAltitude, System.Random rdm)
        {
            int width = terrainData.alphamapWidth;
            int height = terrainData.alphamapHeight;
            int nbTextures = terrainData.alphamapLayers;
            
            float[,] steepnessMap = GenerateSteepnessMap(terrainData, width, height);
            float[,] grassMap = PerlinNoise.GenerateNoiseMap(width,height, width/plainFrequency, 9, plainPersistance, 2, rdm, new Vector2(0,0));
            terrainTypeMap = GenerateTerrainTypeMap(width, height, steepnessMap,grassMap);
            float[,,] splatMap = new float[width,height,nbTextures];

            float[,] forestSoilMap = PerlinNoise.GenerateNoiseMap(width,height, width/forestSoilFrequency, 9, forestSoilPersistance, 2, rdm, new Vector2(0,0));

            for(int y=0; y<height; y++){
                for(int x=0; x<width; x++){
                    float[] textureWeightMap = new float[nbTextures];

                    switch(terrainTypeMap[y,x])
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

                    //Apply default texture if there is no texture
                    // if(Mathf.Approximately(weightSum,0f)){
                    //     textureWeightMap[defaultTextureIndex] = 1f;
                    //     Debug.Log(x+" "+y);
                    // }
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

        private TerrainType[,] GenerateTerrainTypeMap(int width, int height, float[,] steepnessMap, float[,] grassMap)
        {
            TerrainType[,] terrainTypeMap = new TerrainType[height,width];
            
            for(int y=0; y<height; y++){
                for(int x=0; x<width; x++){
                    float steepness = steepnessMap[y,x];

                    if (steepness > minHillSlope)
                        terrainTypeMap[y,x] = TerrainType.Hill;
                    else if (grassMap[y,x] < plainDensity)
                        terrainTypeMap[y,x] = TerrainType.Plain;
                    else
                        terrainTypeMap[y,x] = TerrainType.Forest;
                }
            }
            return terrainTypeMap;
        }

        //Ajusts Height & Slope values
        // void AjustParameters(){
        //     //Switch minHeight-maxHeight minSlope-MaxSlope
        //     for (int i = 0; i < terrainLayers.Length; i++) {
        //         if (terrainLayers[i].minHeight > terrainLayers[i].maxHeight)
        //             (terrainLayers [i].minHeight, terrainLayers [i].maxHeight) = (terrainLayers [i].maxHeight, terrainLayers[i].minHeight);

        //         if (terrainLayers[i].minSlope > terrainLayers [i].maxSlope)
        //             (terrainLayers [i].minSlope, terrainLayers [i].maxSlope) = (terrainLayers [i].maxSlope, terrainLayers [i].minSlope);
        //     }

        //     //if defaultTextureIndex points on not existing layer
        //     if(defaultTextureIndex >= terrainLayers.Length)
        //         defaultTextureIndex = 0;
        // }

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