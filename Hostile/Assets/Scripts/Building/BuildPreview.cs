using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildingSystem
{
    public class BuildPreview : MonoBehaviour
    {
    #region Variables
        [Header("Building parent")]
        public Building building;

        [Header("Prefab")]
        public BuildElement prefab;
        public Transform referencePoint;
        private Vector3 groundOffset = Vector3.zero;
        public Vector3 GroundOffset => groundOffset;

        [Header("Overview attributes")]
        public Material validMaterial;
        public Material invalidMaterial;
        
        private MeshRenderer meshRenderer;

        private int nbBlockingObjects = 0;
        private bool lowEnough = true;
        public int nbSurroundingPillars = 0;

        [Header("Other Attributes")]
        public bool isSnapped = false;
        public bool placable = false;

        public List<string> canSnapToTags = new List<string>();
    #endregion

        void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            groundOffset = referencePoint.localPosition*transform.localScale.y;
            ChangeColor();
        }

        public void UpdateRotation()
        {
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + BuildCore.instance.RotationOffset,0);
        }

        public void ChangeColor()
        {
            if(placable){
                meshRenderer.material = validMaterial;
            }
            else{
                meshRenderer.material = invalidMaterial;
            }
        }

        public void TryPlace(){
            switch(prefab.buildType){
                case BuildType.Foundation:
                    Vector3 raycastOrigin = transform.position + Vector3.up*transform.localScale.y/2;
                    lowEnough = Physics.Raycast(raycastOrigin,Vector3.down,BuildCore.instance.maxFoundationHeight,BuildCore.instance.groundMask); 
                    placable = nbBlockingObjects==0 && lowEnough;
                    break;

                case BuildType.Doorway:
                case BuildType.Window:
                case BuildType.Wall:
                    placable = isSnapped && nbBlockingObjects==0 && nbSurroundingPillars == 2;
                    break;

                case BuildType.Ceiling:
                    placable = isSnapped && nbBlockingObjects==0 && nbSurroundingPillars == 4;
                    break;

                default:
                    placable = isSnapped && nbBlockingObjects==0;
                    break;
            }
            ChangeColor();
        }

        public int GetNbSurroundPillars(SnappingPoint sp){
            int sum = 0;
            foreach(BuildElement be in sp.buildElements){
                if(be.buildType == BuildType.Pillar)
                    sum++;
            }
            return sum;
        }

        public bool IsCorrectSnappingTag(string tag)
        {
            foreach(string ITag in canSnapToTags){
                if(tag == ITag){
                    return true;
                }
            }
            return false;
        }

        public void SnapTo(SnappingPoint snapPoint)
        {
            isSnapped = true;
            transform.position = snapPoint.transform.transform.position;
            transform.rotation = snapPoint.transform.rotation;
            nbSurroundingPillars = GetNbSurroundPillars(BuildCore.instance.CurrentSnapPoint);
        }

        public void UnSnap()
        {
            isSnapped = false;
            nbSurroundingPillars = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other);
            if (IsBlockLayer(other.gameObject)){
                nbBlockingObjects++;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (IsBlockLayer(other.gameObject)){
                nbBlockingObjects--;
            }
        }

        private bool IsBlockLayer(GameObject go){
            return (go.tag != "Buidling" && BuildCore.instance.blockingLayers == (BuildCore.instance.blockingLayers | (1 << go.layer)));
        }
    }
}