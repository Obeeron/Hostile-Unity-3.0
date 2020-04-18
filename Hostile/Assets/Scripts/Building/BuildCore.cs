using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildingSystem
{
    public class BuildCore : NetworkObjectManager
    {
        #region Singleton
        public static BuildCore instance;
        protected override void Awake()
        {
            if (instance != null)
            {
                Debug.LogWarning("warning singleton, il y a deja une instance de BuildCore");
                return;
            }
            instance = this;
        }
        #endregion

        #region Variables
        [Header("References")]
        public Camera playerCamera;
        public void SetCamera (Camera playerCamera){
            this.playerCamera = playerCamera;
        }
        public Transform editor_buildingsParent;
        public GameObject buildingPrefab;

        [Header("Overview Layer Masks")]
        public LayerMask avoidOverviewMask;
        public LayerMask snappingMask;
        
        [Header("Placing LayerMask")]
        public LayerMask blockingLayers;
        public LayerMask groundMask;

        //Preview of Build
        private GameObject previewGO = null;
        private BuildPreview buildPreviewScript = null;
        private float rotationOffset = 0f;
        public float RotationOffset => rotationOffset;

        [Header("General Attributes")]
        public float maxBuildDistance = 5f;
        public float stickTolerance = 1.5f;
        public float maxFoundationHeight = 1.5f;
        public bool building = false;

        private Ray ray;
        private RaycastHit hit;
        private RaycastHit[] hits;
        private SnappingPoint currentSnapPoint;
        public SnappingPoint CurrentSnapPoint => currentSnapPoint;
        #endregion

        void Update()
        {
            //If not building exit Update
            if(!building) return;

            //OVERVIEW
            buildPreviewScript.TryPlace();
            if(IsSnappable()){                      //Si c'est snappable
                if(!buildPreviewScript.isSnapped)   //Et que c'est pas encore snappé
                    Snap();   
            }                                       //On snapp
            else{                                   //Si c pas snappable
                if(buildPreviewScript.isSnapped)    //Et que c'était marqué comme snappable
                    UnSnap();                       //On désnapp

                DoOverviewRay();                    //Overview flottante
                buildPreviewScript.TryPlace();
            }

            //USER INPUTS
            if(Input.GetKeyDown(KeyCode.R)){                //R : Rotation
                HandleRotation();
            }

            else if (Input.GetMouseButtonDown(0)){          //Mouse Click : Place Build
                if(buildPreviewScript.placable)
                    Build();
                else
                    Debug.Log("Cannot build, preview is not snapped");
            }
        }

        private void HandleRotation()
        {
            rotationOffset = (rotationOffset+90)%360;
            if(buildPreviewScript.isSnapped && buildPreviewScript.prefab.buildType == BuildType.Stair){
                buildPreviewScript.UpdateSnappedRotation();
            }
        }

        public void StartPreview(GameObject go)
        {
            CancelBuild();
            previewGO = Instantiate(go,Vector3.zero,Quaternion.identity);
            buildPreviewScript = previewGO.GetComponent<BuildPreview>();
            building = true;
        }

        public void CancelBuild()
        {
            Destroy(previewGO);
            previewGO = null;
            buildPreviewScript = null;
            currentSnapPoint = null;
            building = false;
        }

        public void Build()
        {
            Building buildingParent;

            if(!buildPreviewScript.isSnapped)
                buildingParent = (Instantiate(buildingPrefab,Vector3.zero,Quaternion.identity,editor_buildingsParent)).GetComponent<Building>();
            else
                buildingParent =  currentSnapPoint.buildingParent;

            BuildElement build = (Instantiate(buildPreviewScript.prefab.gameObject,
                                                buildPreviewScript.transform.position,
                                                buildPreviewScript.transform.rotation,
                                                buildingParent.BE_parent.transform)).GetComponent<BuildElement>();
            
            buildingParent.AddBuild(build,currentSnapPoint);
            
            Destroy(previewGO);
            previewGO = null;
            buildPreviewScript = null;
            building = false;
        }

        private void Snap()
        {
            buildPreviewScript.SnapTo(currentSnapPoint);
        }

        private void UnSnap()
        {
            buildPreviewScript.UnSnap();
        }

        private bool IsSnappable()
        {
            ray = playerCamera.ScreenPointToRay(Input.mousePosition);

            hits = Physics.RaycastAll(ray,maxBuildDistance,snappingMask);
            for(int i=0; i<hits.Length && hits[i].collider.gameObject.layer == 14; i++)
            {
                if(buildPreviewScript.IsCorrectSnappingTag(hits[i].collider.tag)){
                    if(currentSnapPoint?.gameObject != hits[i].collider.gameObject){
                        currentSnapPoint = hits[i].collider.GetComponent<SnappingPoint>();
                        buildPreviewScript.isSnapped = false;                           //On remet isSnapped à false dans le cas où des SP sont alignés
                    }
                    return !currentSnapPoint.used;
                }
            }
            
            return false;
        }

        private void DoOverviewRay()
        {
            ray = playerCamera.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit, maxBuildDistance, avoidOverviewMask))
            {
                previewGO.transform.position = hit.point - buildPreviewScript.GroundOffset;
                previewGO.transform.rotation = Quaternion.Euler(0, playerCamera.transform.rotation.eulerAngles.y + rotationOffset,0);
            }
            else
            {
                previewGO.transform.position = playerCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, playerCamera.nearClipPlane)) 
                                                + playerCamera.transform.forward * maxBuildDistance
                                                - buildPreviewScript.GroundOffset;
                previewGO.transform.rotation = Quaternion.Euler(0, playerCamera.transform.rotation.eulerAngles.y + rotationOffset,0);
            }
        }
    }
}