using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Procedural
{
    public static class PerlinNoise
    {
        public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale, int nbOctaves , float persistance, float lacunarity, int seed, Vector2 offset)
        {
            float[,] noiseMap = new float[mapWidth,mapHeight];

            System.Random rdm = new System.Random(seed);
            Vector2[] octaveOffsets = new Vector2[nbOctaves];
            for(int i=0; i<nbOctaves; i++)
            {
                float offsetX = rdm.Next(-100000, 100000) + offset.x;
                float offsetY = rdm.Next(-100000, 100000) + offset.y;
                octaveOffsets[i] = new Vector2(offsetX, offsetY);
            }

            if(scale <= 0) scale = 0.0001f;

            float maxNoiseHeight = float.MinValue;
            float minNoiseHeight = float.MaxValue;

            float halfWidth = mapWidth/2f;
            float halfHeight = mapHeight/2f;
            
            for(int y=0; y<mapHeight; y++){
                for(int x=0; x<mapWidth;x++){

                    float amplitude = 1f;
                    float frequency = 1f;
                    float noiseHeight = 0;

                    for(int i=0; i<nbOctaves; i++){
                        float perlinY = (halfHeight-(float)y)/scale * frequency+ octaveOffsets[i].y;
                        float perlinX = (halfWidth-(float)x)/scale * frequency+ octaveOffsets[i].x;
                    
                        float perlinValue = Mathf.PerlinNoise(perlinX,perlinY)*2f - 1f;
                        noiseHeight += perlinValue * amplitude;

                        amplitude *= persistance;
                        frequency *= lacunarity;
                    }
                    noiseMap[x,y] += noiseHeight;

                    if(noiseHeight>maxNoiseHeight) maxNoiseHeight = noiseHeight;
                    if(noiseHeight<minNoiseHeight) minNoiseHeight = noiseHeight;
                }
            }

            for(int y=0; y<mapHeight; y++){
                for(int x=0; x<mapWidth;x++){
                    noiseMap[x,y] = Mathf.InverseLerp(minNoiseHeight,maxNoiseHeight,noiseMap[x,y]);
                }
            }
            return noiseMap;
        }
    }
}