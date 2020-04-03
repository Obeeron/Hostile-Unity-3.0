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

        public IEnumerator GenerateTrees(TerrainData terrainData, Vector2 mapSize, TerrainType[][] terrainTypeMap, System.Random rdm, TMPro.TextMeshProUGUI textSub)
        {
            if(treePrefabs.Length==0) yield break;
            textSub.text = "Sampling Tree Points..";
            yield return null;
            List<Vector2> tree_spawnPoints = PoissonDiscSampling.GenerateSpawnPoints(rdm, spacing, mapSize, nbSpawnTriesMax);
            Debug.Log(tree_spawnPoints.Count+" tree points generated");

            int j=0;
            foreach(Vector2 point in tree_spawnPoints)
            {
                int splatPosY = (int)(point.y*terrainData.alphamapHeight/mapSize.y);
                int splatPosX = (int)(point.x*terrainData.alphamapWidth/mapSize.x);

                if(terrainTypeMap[splatPosX][splatPosY] == TerrainType.Forest)
                {
                    RaycastHit hit;
                    int layerMask = 1 << 11;
                    Vector3 spawnPoint = new Vector3(point.x, terrainData.GetHeight((int)point.x,(int)point.y) - 0.5f,point.y);
                    if (Physics.Raycast(new Vector3(spawnPoint.x,spawnPoint.y+5,spawnPoint.z), Vector3.down, out hit, layerMask)){
                        j++;
                        Quaternion rotation = Quaternion.Euler(rdm.Next(3),rdm.Next(360),rdm.Next(3));
                        GameObject tree = Instantiate(treePrefabs[rdm.Next(treePrefabs.Length)], spawnPoint, rotation,parent.transform);
                        tree.transform.localScale *=  (float)(minScale + rdm.NextDouble()*(maxScale-minScale));
                        TreeNetworkController.instance.AddToList(tree.GetComponent<FarmingItem>());
                        if(j%5000==0){
                            textSub.text = string.Format("[{0}/{1}] Trees Placed.",j,tree_spawnPoints.Count-j);
                            yield return null;
                        }
                    }
                }
            }
        }
    }
}