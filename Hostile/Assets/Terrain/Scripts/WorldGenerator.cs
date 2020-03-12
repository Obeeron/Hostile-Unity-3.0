using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Procedural{
    public class WorldGenerator : MonoBehaviour
    {
        public TerrainGenerator terrainGenerator;
        public TreeGenerator treeGenerator;
        public GrassGenerator grassGenerator;
        public TextureGenerator textureGenerator;

        public Terrain terrain;
        List<Vector2> tree_spawnPoints;
        TerrainType[,] terrainTypeMap;

        System.Random rdm;

        // Start is called before the first frame update
        public void GenerateWorld(int seed)
        {
            rdm = new System.Random(seed);
            GenerateTerrain();
            GenerateTextures();
            SpawnTrees();
            SpawnGrass();
        }

        public void GenerateTerrain()
        {
            terrainTypeMap = new TerrainType[terrain.terrainData.alphamapHeight,terrain.terrainData.alphamapWidth]; 
            terrainGenerator.GenerateTerrain(rdm);
        }

        public void GenerateTextures()
        {
            textureGenerator.GenerateTextures(terrain.terrainData, ref terrainTypeMap, terrainGenerator.maxAltitude, rdm);
        }

        void SpawnTrees()
        {
            treeGenerator.GenerateTrees(terrain.terrainData,new Vector2(terrainGenerator.height,terrainGenerator.width), terrainTypeMap, rdm);
            //GenerateRocks
        }

        public void SpawnGrass(){
            grassGenerator.GenerateGrass(terrain.terrainData,new Vector2(terrainGenerator.height,terrainGenerator.width), terrainTypeMap, rdm);
        }
    }

    public enum TerrainType{
        Forest,
        Plain,
        Hill
    };
}
