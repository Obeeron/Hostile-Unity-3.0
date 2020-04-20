using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Procedural
{
    public class VegetationGenerator : MonoBehaviour
    {
        [Header("Grass scattering properties")]
        public float spacing = 1;
        public int nbSpawnTriesMax = 3;
        public float minScale = 1;
        public float maxScale = 1.2f;
        int nbGrassPrefabs = 1;

        [Header("Bush scattering properties")]
        public Transform bushParent;
        public GameObject[] bushPrefabs;
        public float bush_minScale = 1;
        public float bush_maxScale = 1.2f;
        [Range(0f,1f)] public float bushDensity = 0.005f;

        public IEnumerator GenerateGrass(TerrainData terrainData, Vector2 mapSize, TerrainType[][] terrainTypeMap, System.Random rdm, TMPro.TextMeshProUGUI textSub)
        {
            if(nbGrassPrefabs == 0) yield break;

            textSub.text = "Sampling Grass Points..";
            yield return null;
            List<Vector2> grass_spawnPoints = PoissonDiscSampling.GenerateSpawnPoints(rdm, spacing, mapSize, nbSpawnTriesMax);
            Debug.Log(grass_spawnPoints.Count+" grass point generated");

            textSub.text = "Initializing Detail Map..";
            yield return null;
            List<int[,]> detailMapData = new List<int[,]>();
            for(int i =0; i<nbGrassPrefabs; i++) 
                detailMapData.Add(new int[terrainData.detailHeight, terrainData.detailWidth]);
            
            int j=0,k=0;
            foreach(Vector2 point in grass_spawnPoints)
            {
                int splatPosY = (int)(point.y*terrainData.alphamapHeight/mapSize.y);
                int splatPosX = (int)(point.x*terrainData.alphamapWidth/mapSize.x);
                int detailPosY = (int)(point.y*terrainData.detailHeight/mapSize.y);
                int detailPosX = (int)(point.x*terrainData.detailWidth/mapSize.x);

                if(terrainTypeMap[splatPosX][splatPosY] != TerrainType.Hill)
                {
                    j++;
                    int grassPrefabIndex = UnityEngine.Random.Range(0,nbGrassPrefabs);
                    detailMapData[grassPrefabIndex][detailPosY,detailPosX] = 1;
                    if(j%100000==0){
                        textSub.text = string.Format("[{0}/{1}] Grass Placed.",j,grass_spawnPoints.Count-k);
                        yield return null;
                    }
                }
                else{
                    k++;
                }
            }


            // write all detail data to terrain data:
            for (int i = 0; i < detailMapData.Count; i++)
            {
                terrainData.SetDetailLayer(0, 0, i, detailMapData[i]);
            }
        }

        public IEnumerator GenerateBush(Terrain terrain, Vector2 mapSize, TerrainType[][] terrainTypeMap, System.Random rdm, TMPro.TextMeshProUGUI textSub)
        {
            if(bushPrefabs.Length == 0) yield break;
            
            Vector2 point;
            int splatPosX;
            int splatPosY;
            Vector3 spawnPoint;
            Quaternion rotation;
            GameObject tree;

            int nbMaxBush = (int)(mapSize.x*mapSize.y*bushDensity);
            int i=0;
            while(i<nbMaxBush){
                point = new Vector2((float)(rdm.NextDouble()*mapSize.x), (float)(rdm.NextDouble()*mapSize.y));

                splatPosY = (int)(point.y*terrain.terrainData.alphamapHeight/mapSize.y);
                splatPosX = (int)(point.x*terrain.terrainData.alphamapWidth/mapSize.x);

                if(terrainTypeMap[splatPosX][splatPosY] == TerrainType.Forest){
                    i++;

                    spawnPoint = new Vector3(point.x, terrain.terrainData.GetHeight((int)point.x,(int)point.y) - 0.5f,point.y);
                    rotation = Quaternion.Euler(0,rdm.Next(360),0);
                    tree = Instantiate(bushPrefabs[rdm.Next(bushPrefabs.Length)], spawnPoint, rotation,bushParent);
                    tree.transform.localScale *=  (float)(bush_minScale + rdm.NextDouble()*(bush_maxScale-bush_minScale));

                    if(i%500==0){
                        textSub.text = string.Format("[{0}/{1}] Bush Placed.",i,nbMaxBush);
                        yield return null;
                    }
               }
               else{
                    nbMaxBush--;
                }
            }

            terrain.Flush();
        }
    }
}