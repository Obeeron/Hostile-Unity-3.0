using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Procedural
{
    public class TreeGenerator : MonoBehaviour
    {
        public float spacing = 9;
        public int nbSpawnTriesMax = 30;
        public float minScale = 1;
        public float maxScale = 1;

        public GameObject parent;
        public GameObject[] treePrefabs;
        public void GenerateTrees(TerrainData terrainData, Vector2 mapSize, TerrainType[,] terrainTypeMap, System.Random rdm)
        {
            if(treePrefabs.Length==0) return;
            List<Vector2> tree_spawnPoints = PoissonDiscSampling.GenerateSpawnPoints(rdm, spacing, mapSize, nbSpawnTriesMax);
            Debug.Log(tree_spawnPoints.Count+" tree generated");

            foreach(Vector2 point in tree_spawnPoints)
            {
                int splatPosX = (int)(point.y*terrainData.alphamapHeight/mapSize.y);
                int splatPosY = (int)(point.x*terrainData.alphamapWidth/mapSize.x);

                if(terrainTypeMap[splatPosY,splatPosX] == TerrainType.Forest)
                {

                    Vector3 spawnPoint = new Vector3(point.x, terrainData.GetHeight((int)point.x,(int)point.y) - 0.5f,point.y);
                    Quaternion rotation = Quaternion.Euler(rdm.Next(3),rdm.Next(360),rdm.Next(3));
                    GameObject tree = Instantiate(treePrefabs[rdm.Next(treePrefabs.Length)], spawnPoint, rotation,parent.transform);
                    tree.transform.localScale *=  (float)(minScale + rdm.NextDouble()*(maxScale-minScale));
                    //tree.transform.localScale = Vector3.one * rdm.(minScaleMultiplier,maxScaleMultiplier);
                }
            }
        }
    }
}