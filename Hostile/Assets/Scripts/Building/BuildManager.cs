using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildingSystem
{
    public class BuildManager : MonoBehaviour
    {
        public GameObject foundation;
        public GameObject pillar;
        public GameObject wall;
        public GameObject ceiling;
        public GameObject stair_l;
        public GameObject stair_u;
        public GameObject doorframe;
        public GameObject windows;
        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha0)){
                BuildCore.instance.CancelBuild();
            }
            if(Input.GetKeyDown(KeyCode.Alpha1)){
                BuildCore.instance.StartPreview(foundation);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2)){
                BuildCore.instance.StartPreview(pillar);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha3)){
                BuildCore.instance.StartPreview(wall);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha4)){
                BuildCore.instance.StartPreview(ceiling);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha5)){
                BuildCore.instance.StartPreview(stair_l);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha6)){
                BuildCore.instance.StartPreview(stair_u);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha7)){
                BuildCore.instance.StartPreview(doorframe);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha8)){
                BuildCore.instance.StartPreview(windows);
            }
        }
    }
}