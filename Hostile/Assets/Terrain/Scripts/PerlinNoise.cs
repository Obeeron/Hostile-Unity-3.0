using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Procedural
{
    public static class PerlinNoise
    {
        public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale, int nbOctaves , float persistance, float lacunarity, System.Random rdm, Vector2 offset)
        {
            float[,] noiseMap = new float[mapWidth,mapHeight];

            Vector2[] octaveOffsets = new Vector2[nbOctaves];
            for(int i=0; i<nbOctaves; i++)
            {
                float offsetX = rdm.Next(0, 100000) + offset.x;
                float offsetY = rdm.Next(0, 100000) + offset.y;
                octaveOffsets[i] = new Vector2(offsetX, offsetY);
            }

            if(scale <= 0) scale = 0.0001f;

            float maxNoiseHeight = float.MinValue;
            float minNoiseHeight = float.MaxValue;

            float halfWidth = mapWidth/2f;
            float halfHeight = mapHeight/2f;
            
            for(int y=0; y<mapHeight; y++){
                for(int x=0; x<mapWidth;x++){

                    float amplitude = 1;
                    float frequency = 1;
                    float noiseHeight = 0;

                    for(int i=0; i<nbOctaves; i++){
                        float perlinY = (y-halfHeight + octaveOffsets[i].y)/ scale * frequency;
                        float perlinX = (x-halfWidth + octaveOffsets[i].x)/ scale * frequency;

                        float perlinValue = Mathf.PerlinNoise(perlinX,perlinY)*2 - 1;
                        noiseHeight += perlinValue * amplitude;

                        amplitude *= persistance;
                        frequency *= lacunarity;
                    }
                    noiseMap[x,y] = noiseHeight;

                    if(noiseHeight>maxNoiseHeight) maxNoiseHeight = (float)noiseHeight;
                    if(noiseHeight<minNoiseHeight) minNoiseHeight = (float)noiseHeight;
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