using System.Collections.Generic;
using UnityEngine;

namespace Procedural
{
    public class PoissonDiscSampling
    {
        public static List<Vector2> GenerateSpawnPoints(float spacing, Vector2 regionSize, int nbSpawnTriesMax = 30)
        {
            float cellSize = spacing/Mathf.Sqrt(2);         //Length grid cells
            
            int[,] grid = new int[Mathf.CeilToInt(regionSize.x/cellSize),Mathf.CeilToInt(regionSize.y/cellSize)];  //Grid for all the terrain. One cell contain 1 spawn point
            List<Vector2> spawnPoints = new List<Vector2>();//List of spawn points coordinates
            List<Vector2> frontier = new List<Vector2>();   //List of the external spawn points from wich we are trying to spawn other points

            frontier.Add(regionSize/2); //First spawn point set at the center of the terrain

            while(frontier.Count>0){
                int frontierIndex = Random.Range(0,frontier.Count); //Geting a random index for choosing a point from the frontier
                Vector2 spawnCenter = frontier[frontierIndex];      //Getting the frontier's spawnpoint coordinates
                bool candidateAccepted = false;            

                //Trying to find space for a new spawnpoint around the randomly choosen frontier point 
                for(int i=0; i<nbSpawnTriesMax; i++){
                    float angle = Random.value * Mathf.PI * 2f;                                 //random direction
                    Vector2 direction = new Vector2(Mathf.Sin(angle),Mathf.Cos(angle));         //normalized direction given an angle
                    Vector2 candidate = spawnCenter+direction*Random.Range(spacing,2*spacing);  //random new spawn point

                    if(IsValid(candidate,spacing, regionSize, cellSize, spawnPoints, grid)){         //If the new spawn point has a minimum spacing betwen the points around him
                        candidateAccepted = true;
                        frontier.Add(candidate);    //make the new spawn point a frontier point
                        spawnPoints.Add(candidate); //add it to the spawn points list
                        grid[(int)(candidate.x/cellSize),(int)(candidate.y/cellSize)] = spawnPoints.Count; //mark it in the grid with its index(+1) in the spawn points list
                        break;
                    }
                }
                
                if(!candidateAccepted){ //if unable to spawn a point arount the choosen frontier point
                    frontier.RemoveAt(frontierIndex); //remove this point from the frontier
                }
            }

            return spawnPoints;
        }

        static bool IsValid(Vector2 candidate, float spacing, Vector2 regionSize, float cellSize, List<Vector2> points, int[,] grid)
        {
            if(candidate.x >= 0 && candidate.x < regionSize.x && candidate.y >=  0 && candidate.y < regionSize.y){
                int cellX = (int)(candidate.x/cellSize);
                int cellY = (int)(candidate.y/cellSize);

                int searchStartX = Mathf.Max(0,cellX-2);
                int searchEndX = Mathf.Min(cellX+2,grid.GetLength(0)-1);
                int searchStartY = Mathf.Max(0,cellY-2);
                int searchEndY = Mathf.Min(cellY+2,grid.GetLength(1)-1);

                for(int x = searchStartX; x <= searchEndX; x++){
                    for(int y = searchStartY; y <= searchEndY; y++){
                        int pointIndex = grid[x,y] - 1;
                        if(pointIndex != -1){
                            float squaredDistance = (candidate - points[pointIndex]).sqrMagnitude;
                            if(squaredDistance < spacing*spacing){
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            return false;
        }
    }
}