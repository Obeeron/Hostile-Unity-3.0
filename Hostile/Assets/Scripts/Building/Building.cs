using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildingSystem
{
    public class Building : MonoBehaviour
    {
        public List<BuildElement> buildElements;
        public List<SnappingPoint> snappingPoints;

        public Transform SP_parent;
        public Transform BE_parent;

        public void AddBuild(BuildElement build, SnappingPoint targetSnappingPoint){
            build.buildingParent = this;
            if(targetSnappingPoint != null) targetSnappingPoint.used = true;
            for(int i=0; i<build.snappingPoints.Count;i++){
                SnappingPoint sp_FromBE = build.snappingPoints[i];
                sp_FromBE.gameObject.SetActive(true);
                SnappingPoint sameSP_FromBuilding;
                FindMatchingSnappingPoint(sp_FromBE.transform.position, sp_FromBE.tag, out sameSP_FromBuilding);

                //This is a new snapping point
                if(sameSP_FromBuilding == null){
                    SnappingPoint newSP = NewSnappingPoint(sp_FromBE,build);
                    build.snappingPoints[i] = newSP;
                }
                //Snapping point already registered
                else{
                    sameSP_FromBuilding.buildElements.Add(build);
                    build.snappingPoints[i] = sameSP_FromBuilding;
                }
                
                Destroy(sp_FromBE.gameObject);
            }
        }
        
        public void FindMatchingSnappingPoint(Vector3 pos, string tag, out SnappingPoint sameSP_FromBuilding){
            sameSP_FromBuilding = null;

            foreach(SnappingPoint snappingPoint in snappingPoints){         //Loop through each SP of the Building
                if(ComparePositions(snappingPoint.transform.position,pos))                 //If SP from Building has the same position than SP from BE 
                {
                    if(snappingPoint.tag == tag){                           //If they both have the same tag
                        if(sameSP_FromBuilding!=null)                       //Shouldn't happen, only 1 SP at a specific position per tag
                            Debug.LogWarning("Shit! GetMatchingSnappingPoints : found 2 snapping points at the same place with the same:"+tag);
                        sameSP_FromBuilding = snappingPoint;
                    }
                    else{
                        snappingPoint.used = true;
                    }
                }
            }
        }

        private bool ComparePositions(Vector3 v1, Vector3 v2){
            return Mathf.Approximately(v1.x,v2.x) && Mathf.Approximately(v1.y,v2.y) && Mathf.Approximately(v1.z,v2.z);
        }

        private SnappingPoint NewSnappingPoint(SnappingPoint sp, BuildElement build){
                    SnappingPoint newSP = Instantiate(sp,sp.transform.position,sp.transform.rotation,SP_parent);
                    newSP.buildingParent = this;
                    newSP.buildElements.Add(build);
                    BoxCollider newSPBoxCollider = newSP.GetComponent<BoxCollider>();
                    newSPBoxCollider.size = GetAbsoluteColliderSize(sp.GetComponent<BoxCollider>());
                    
                    snappingPoints.Add(newSP);
                    return newSP;
        }

        private Vector3 GetAbsoluteColliderSize(BoxCollider box){
            Vector3 newSize =  new Vector3(box.size.x * box.transform.parent.localScale.x,
                                box.size.y * box.transform.parent.localScale.y,
                                box.size.z * box.transform.parent.localScale.z);
            return newSize;
        }
    }
}