using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Procedural
{
    public class GrassGenerator : MonoBehaviour
    {
        [Header("Grass scattering properties")]
        public float spacing = 1;
        public int nbSpawnTriesMax = 2;
        public float minScale = 1;
        public float maxScale = 1;

        [Header("Grass gameobject")]
        public GameObject parent;

        public IEnumerator GenerateGrass(TerrainData terrainData, Vector2 mapSize, TerrainType[][] terrainTypeMap, System.Random rdm, TMPro.TextMeshProUGUI textSub)
        {
            int nbGrassPrefabs = terrainData.detailPrototypes.Length;
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

                if(terrainTypeMap[splatPosX][splatPosY] == TerrainType.Plain)
                {
                    j++;
                    int grassPrefabIndex = UnityEngine.Random.Range(0,nbGrassPrefabs);
                    detailMapData[grassPrefabIndex][detailPosY,detailPosX] = 1;
                    if(j%10000==0){
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
    }
}