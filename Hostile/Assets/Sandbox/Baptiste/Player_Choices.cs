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
        private Text remainingPoints;
        private Text LifeText;
        private Text AgilityText;
        private Text DexterityText;
        private Text StrengthText;
        private Text StaminaText;
        private Text HungerText;
        
        private int Skills = 5;
        private int LifePoints = 100;
        private float AgilityPoints = 1f;
        private float DexterityPoints = 1f;
        private int HungerPoints = 130;
        private int StaminaPoints = 120;
        private float StrengthPoints = 1f;


        public void Start()
        {
            PrefabPlayer = this.gameObject;
            Pannel.SetActive(true);
            remainingPoints = Pannel.transform.Find("Points").GetComponent<Text>();
            LifeText = Pannel.transform.Find("LifeStats").GetComponent<Text>();
            AgilityText = Pannel.transform.Find("AgilityStats").GetComponent<Text>();
            DexterityText = Pannel.transform.Find("DexterityStats").GetComponent<Text>();
            StrengthText = Pannel.transform.Find("StrengthStats").GetComponent<Text>();
            StaminaText = Pannel.transform.Find("StaminaStats").GetComponent<Text>();
            HungerText = Pannel.transform.Find("HungerStats").GetComponent<Text>();
            Cursor.lockState = CursorLockMode.None;
            remainingPoints.text = "" + Skills;
            LifeText.text = "" + LifePoints;
            AgilityText.text = "" + AgilityPoints;
            DexterityText.text = "" + DexterityPoints;
            StrengthText.text = "" + StrengthPoints;
            StaminaText.text = "" + StaminaPoints;
            HungerText.text = "" + HungerPoints;
            StartCoroutine(EndChoice());
        }

        public void LifeUp(bool up)
        {
            if (up)
            {
                LifePoints += 10;
                Skills--;
            }
            else if (LifePoints > 100)
            {
                LifePoints -= 10;
                Skills++;
            }
            remainingPoints.text = "" + Skills;
            LifeText.text = "" + LifePoints;
            if (Skills == 0)
            {
                Pannel.SetActive(false);
            }
        }

        public void HungerUp(bool up)
        {
           if (up)
            {
                HungerPoints += 10;
                Skills--;
            }
            else if (HungerPoints > 130)
            {
                HungerPoints -= 10;
                Skills++;
            }
            remainingPoints.text = "" + Skills;
            HungerText.text = "" + HungerPoints;
            if (Skills == 0)
            {
                Pannel.SetActive(false);
            }
        }

        public void StaminaUp(bool up)
        {
            if (up)
            {
                StaminaPoints += 5;
                Skills--;
            }
            else if (StaminaPoints > 120)
            {
                StaminaPoints -= 5;
                Skills++;
            }
            remainingPoints.text = "" + Skills;
            StaminaText.text = "" + StaminaPoints;
            if (Skills == 0)
            {
                Pannel.SetActive(false);
            }
        }

        public void StrengthUp(bool up)
        {
            if (up)
            {
                StrengthPoints += 0.1f;
                Skills--;
            }
            else if (StrengthPoints > 0.1f)
            {
                StrengthPoints -= 0.1f;
                Skills++;
            }
            remainingPoints.text = "" + Skills;
            StrengthText.text = "" + StrengthPoints;
            if (Skills == 0)
            {
                Pannel.SetActive(false);
            }
        }

        public void AgilityUp(bool up)
        {
            if (up)
            {
                AgilityPoints += 0.1f;
                Skills--;
            }
            else if (AgilityPoints > 0.1f)
            {
                AgilityPoints -= 0.1f;
                Skills++;
            }
            remainingPoints.text = "" + Skills;
            AgilityText.text = "" + AgilityPoints;
            if (Skills == 0)
            {
                Pannel.SetActive(false);
            }
        }

        public void DexterityUp(bool up)
        {
            if (up)
            {
                DexterityPoints += 0.1f;
                Skills--;
            }
            else if (StrengthPoints > 0.1f)
            {
                DexterityPoints -= 0.1f;
                Skills++;
            }
            remainingPoints.text = "" + Skills;
            DexterityText.text = "" + DexterityPoints;
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