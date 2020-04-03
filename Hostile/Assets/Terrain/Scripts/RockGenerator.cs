using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Procedural
{
    public class RockGenerator : MonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject[] terrainRockPrefabs;
        public GameObject farmableRockPrefab;

        [Header("Parents")]
        public Transform terrainRocksParent;
        public Transform farmableRocksParent;

        [Header("Attributes")]
        public float terrainRockSpacing = 3f;
        public float farmableRockSpacing = 40f;
        public float maxScale = 2;
        public int maxClusterSize = 3;

        public IEnumerator GenerateRocks(TerrainData terrainData, TerrainType[][] terrainTypeMap, Vector2 mapSize, System.Random rdm, TMPro.TextMeshProUGUI textSub){
            textSub.text = "Sampling Terrain Rock Points..";
            yield return null;
            List<Vector2> terrainRocksSpawnPoints = PoissonDiscSampling.GenerateSpawnPoints(rdm,terrainRockSpacing,mapSize,5);
            textSub.text = "Sampling Farmable Rock Points..";
            yield return null;
            List<Vector2> farmableRocksSpawnPoints = PoissonDiscSampling.GenerateSpawnPoints(rdm,farmableRockSpacing,mapSize,5);

            int j=0;
            //TERRAIN ROCKS
            foreach(Vector2 point in terrainRocksSpawnPoints){
                int splatPosY = (int)(point.y*terrainData.alphamapHeight/mapSize.y);
                int splatPosX = (int)(point.x*terrainData.alphamapWidth/mapSize.x);

                if(terrainTypeMap[splatPosX][splatPosY] == TerrainType.Hill){
                    Vector3 spawnPoint = new Vector3(point.x, terrainData.GetHeight((int)point.x,(int)point.y),point.y);
                    RaycastHit hit;
                    if (Physics.Raycast(new Vector3(spawnPoint.x,spawnPoint.y+1,spawnPoint.z), Vector3.down, out hit)) {
                        j++;
                        int rockIndex = rdm.Next(terrainRockPrefabs.Length);
                        
                        GameObject go = Instantiate(terrainRockPrefabs[rockIndex],spawnPoint,Quaternion.identity,terrainRocksParent);
                        
                        go.transform.Rotate(GetSlopeRotation(hit.normal));
                        // Debug.DrawLine(spawnPoint,spawnPoint+4*hit.normal,Color.green,1000000);
                        // Debug.Log(direction.x+" "+direction.y+" "+yRotation);
                        go.transform.localScale =  Vector3.one*(1+(float)rdm.NextDouble()*maxScale);
                        if(j%5000==0){
                            textSub.text = string.Format("[{0}/{1}] Terrain Rocks Placed.",j,terrainRocksSpawnPoints.Count-j);
                            yield return null;
                        }
                    }
                }
            }

            j=0;
            //FARMABLE ROCKS
            foreach(Vector2 point in farmableRocksSpawnPoints){
                int splatPosY = (int)(point.y*terrainData.alphamapHeight/mapSize.y);
                int splatPosX = (int)(point.x*terrainData.alphamapWidth/mapSize.x);

                if(terrainTypeMap[splatPosX][splatPosY] != TerrainType.Hill){
                    Vector3 spawnPoint = new Vector3(point.x, terrainData.GetHeight((int)point.x,(int)point.y),point.y);
                    RaycastHit hit;
                    int layerMask = 1 << 11;
                    if (Physics.Raycast(new Vector3(spawnPoint.x,spawnPoint.y+1,spawnPoint.z), Vector3.down, out hit, layerMask)){
                        int clusterSize= rdm.Next(1,maxClusterSize);
                        for(int i=0; i<clusterSize; i++,j++){
                            Quaternion rotation = Quaternion.Euler(0,rdm.Next(360),0) * Quaternion.FromToRotation(Vector3.up, hit.normal);
                            spawnPoint = new Vector3(spawnPoint.x+1+(float)rdm.NextDouble()*2,  0,
                                                    spawnPoint.z+1+(float)rdm.NextDouble()*2);
                            spawnPoint = new Vector3(spawnPoint.x, terrainData.GetHeight((int)spawnPoint.x,(int)spawnPoint.z),spawnPoint.z);
                            GameObject go = Instantiate(farmableRockPrefab,spawnPoint,rotation,farmableRocksParent);
                            RockNetworkController.instance.AddToList(go.GetComponent<FarmingItem>());
                            if(j%100==0){
                                textSub.text = string.Format("[{0}/{1}] Farmable Rocks Placed.",j,terrainRocksSpawnPoints.Count-j);
                                yield return null;
                            }
                        }
                    }
                }
            }
        }

        Vector3 GetSlopeRotation(Vector3 normal){
            Vector2 direction = new Vector2(normal.x, normal.z);
            direction.Normalize();
            if(normal.z>0)
                return new Vector3(0,Mathf.Atan(direction.x/direction.y)*180/Mathf.PI-90,0);
            else if(normal.z<0)
                return new Vector3(0,Mathf.Atan(direction.x/direction.y)*180/Mathf.PI+90,0);
            else
                return new Vector3(0,direction.x*90,0);
        }
    }
}