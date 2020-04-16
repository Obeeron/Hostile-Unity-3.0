using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildingSystem
{
    public class SnappingPoint : MonoBehaviour
    {
        public Building buildingParent;
        public List<BuildElement> buildElements;
        public int surroundingPillars = 0;
        public bool used = false;
    }
}