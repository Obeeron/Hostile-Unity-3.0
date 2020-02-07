using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Joueur
{
    public class Player_Choices : MonoBehaviour
    {
        public GameObject Pannel;
        private GameObject PrefabPlayer;
        
        private int Skills = 5;
        private int LifePoints = 0;
        private float AgilityPoints = 0;
        private float DexterityPoints = 0;
        private int HungerPoints = 0;
        private int StaminaPoints = 0;
        private int StrengthPoints = 0;


        public void Start()
        {
            PrefabPlayer = this.gameObject;
            Pannel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }

        public void Update()
        {
            StartCoroutine(EndChoice());
        }

        public void LifeUp()
        {
            LifePoints++;
            Skills--;
            if (Skills == 0)
            {
                Pannel.SetActive(false);
            }
        }

        public void HungerUp()
        {
            HungerPoints++;
            Skills--;
            if (Skills == 0)
            {
                Pannel.SetActive(false);
            }
        }

        public void StaminaUp()
        {
            StaminaPoints++;
            Skills--;
            if(Skills == 0)
            {
                Pannel.SetActive(false);
            }
        }

        public void StrengthUp()
        {
            StrengthPoints++;
            Skills--;
            if (Skills == 0)
            {
                Pannel.SetActive(false);
            }
        }

        public void AgilityUp()
        {
            AgilityPoints++;
            Skills--;
            if (Skills == 0)
            {
                Pannel.SetActive(false);
            }
        }

        public void DexterityUp()
        {
            DexterityPoints++;
            Skills--;
            if (Skills == 0)
            {
                Pannel.SetActive(false);
            }
        }

        private IEnumerator EndChoice()
        {
            yield return new WaitForSeconds(15);
            if (Skills > 0)
            {
                Pannel.SetActive(false);
            }
            while (Skills > 0)
            {
                LifePoints++;
                Skills--;
            }
            PrefabPlayer.GetComponent<Player_Stats>().Choosing(LifePoints, HungerPoints, StaminaPoints, StrengthPoints, AgilityPoints, DexterityPoints);
            PrefabPlayer.GetComponent<Animator>().enabled = true;
            PrefabPlayer.GetComponent<Player_Movement>().enabled = true;
            PrefabPlayer.GetComponent<Camera_Movement>().enabled = true;
            PrefabPlayer.GetComponent<Player_Choices>().enabled = false;
        }
    }
}