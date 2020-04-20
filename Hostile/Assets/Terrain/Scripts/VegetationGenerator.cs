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
        public int nbBushPrefabs = 4;
        public float bush_minScale = 1;
        public float bush_maxScale = 1.2f;
        public float nbMaxBush = 5000;

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

        public IEnumerator GenerateBush(TerrainData terrainData, Vector2 mapSize, TerrainType[][] terrainTypeMap, System.Random rdm, TMPro.TextMeshProUGUI textSub)
        {
            if(nbBushPrefabs == 0) yield break;

            textSub.text = "Initializing Detail Map..";
            yield return null;
            List<int[,]> detailMapData = new List<int[,]>();
            int i;
            for(i=0; i<nbBushPrefabs; i++) 
                detailMapData.Add(new int[terrainData.detailHeight, terrainData.detailWidth]);
            
            i=0;
            while(i<nbMaxBush){
                Vector2 point = new Vector2((float)(rdm.NextDouble()*mapSize.x), (float)(rdm.NextDouble()*mapSize.y));

                int splatPosY = (int)(point.y*terrainData.alphamapHeight/mapSize.y);
                int splatPosX = (int)(point.x*terrainData.alphamapWidth/mapSize.x);
                int detailPosY = (int)(point.y*terrainData.detailHeight/mapSize.y);
                int detailPosX = (int)(point.x*terrainData.detailWidth/mapSize.x);

                switch(terrainTypeMap[splatPosX][splatPosY]){
                    case TerrainType.Forest:
                    case TerrainType.Plain:
                        i++;
                        int bushPrefabIndex = rdm.Next(nbBushPrefabs);
                        detailMapData[bushPrefabIndex][detailPosY,detailPosX] = 1;
                        if(i%500==0){
                            textSub.text = string.Format("[{0}/{1}] Bush Placed.",i,nbMaxBush);
                            yield return null;
                        }
                        break;
                    default:
                        nbMaxBush--;
                        break;
                }
            }

            // write all detail data to terrain data:
            for (i = 0; i < nbBushPrefabs; i++)
            {
                terrainData.SetDetailLayer(0, 0, nbGrassPrefabs+i, detailMapData[i]);
            }
        }
    }
}