using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Procedural{
    public class WorldGenerator : MonoBehaviour
    {
        private TerrainGenerator terrainGenerator;
        private TreeGenerator treeGenerator;
        private VegetationGenerator vegetationGenerator;
        private TextureGenerator textureGenerator;
        private RockGenerator rockGenerator;

        public TextMeshProUGUI textMain;  
        public TextMeshProUGUI textSub;
        public Terrain terrain;
        List<Vector2> tree_spawnPoints;
        TerrainType[][] terrainTypeMap;

        System.Random rdm;

        void Start()
        {
            terrainGenerator = GetComponent<TerrainGenerator>();
            treeGenerator = GetComponent<TreeGenerator>();
            vegetationGenerator = GetComponent<VegetationGenerator>();
            textureGenerator = GetComponent<TextureGenerator>();
            rockGenerator = GetComponent<RockGenerator>();
        }

        // Start is called before the first frame update
        public IEnumerator GenerateWorld(int seed)
        {
            rdm = new System.Random(seed);

            textMain.text = "Generating Terrain..";
            yield return StartCoroutine(GenerateTerrain());
            textMain.text = "Generating Textures..";
            yield return StartCoroutine(GenerateTextures());
            textMain.text = "Generating Rocks..";
            yield return StartCoroutine(GenerateRock());
            textMain.text = "Generating Trees..";
            yield return StartCoroutine(SpawnTrees());
            textMain.text = "Generating Grass..";
            yield return StartCoroutine(SpawnGrass());
            textMain.text = "Generating Bushes..";
            yield return StartCoroutine(SpawnBush());
            GameObject.FindObjectOfType<GameSetupManager>().AfterWorldGeneration();
        }

        public IEnumerator GenerateTerrain()
        {
            terrainTypeMap = new TerrainType[terrain.terrainData.alphamapHeight][];
            for(int i=0; i<terrain.terrainData.alphamapHeight; i++)
                terrainTypeMap[i] = new TerrainType[terrain.terrainData.alphamapWidth];
            yield return StartCoroutine(terrainGenerator.GenerateTerrain(rdm,textSub));
        }

        public IEnumerator GenerateTextures()
        {
            yield return StartCoroutine(textureGenerator.GenerateTextures(terrain.terrainData, terrainTypeMap, terrainGenerator.maxAltitude, rdm,textSub));
        }
        
        private IEnumerator GenerateRock()
        {
            yield return StartCoroutine(rockGenerator.GenerateRocks(terrain.terrainData,terrainTypeMap,new Vector2(terrainGenerator.height,terrainGenerator.width),rdm,textSub));
        }

        IEnumerator SpawnTrees()
        {
            yield return StartCoroutine(treeGenerator.GenerateTrees(terrain.terrainData,new Vector2(terrainGenerator.height,terrainGenerator.width), terrainTypeMap, rdm,textSub));
        }

        public IEnumerator SpawnGrass()
        {
            yield return StartCoroutine(vegetationGenerator.GenerateGrass(terrain.terrainData,new Vector2(terrainGenerator.height,terrainGenerator.width), terrainTypeMap, rdm,textSub));
        }

        public IEnumerator SpawnBush()
        {
            yield return StartCoroutine(vegetationGenerator.GenerateBush(terrain.terrainData,new Vector2(terrainGenerator.height,terrainGenerator.width), terrainTypeMap, rdm,textSub));
        }

        public void GenerateViaEditor(){
            rdm = new System.Random();
            terrainTypeMap = new TerrainType[terrain.terrainData.alphamapHeight][];
            for(int i=0; i<terrain.terrainData.alphamapHeight; i++)
                terrainTypeMap[i] = new TerrainType[terrain.terrainData.alphamapWidth];
            terrainGenerator.GenerateTerrain(rdm,textSub);
            textureGenerator.GenerateTextures(terrain.terrainData, terrainTypeMap, terrainGenerator.maxAltitude, rdm,textSub);
            vegetationGenerator.GenerateGrass(terrain.terrainData,new Vector2(terrainGenerator.height,terrainGenerator.width), terrainTypeMap, rdm,textSub);
            vegetationGenerator.GenerateBush(terrain.terrainData,new Vector2(terrainGenerator.height,terrainGenerator.width), terrainTypeMap, rdm,textSub);
        }
    }

    public enum TerrainType{
        Forest,
        Plain,
        Hill
    };
}
