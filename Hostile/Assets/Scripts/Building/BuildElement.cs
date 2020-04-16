using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildingSystem
{
    public class BuildElement : NetworkObject
    {
        public BuildType buildType;
        [HideInInspector] public List<SnappingPoint> snappingPoints = new List<SnappingPoint>();
        [HideInInspector] public Building buildingParent;

        void Awake()
        {
            foreach(SnappingPoint sp in GetComponentsInChildren<SnappingPoint>(true)){
                snappingPoints.Add(sp);
            }
        }
    }

    public enum BuildType{
        Foundation,
        Pillar,
        Wall,
        Ceiling,
        Doorway,
        Window,
        Stair
    }
}