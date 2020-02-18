using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Joueur
{
    public class Player_Choices : MonoBehaviour
    {
    #pragma warning disable 649
        [SerializeField] private PlayerData Data;
        public GameObject Pannel;
    #pragma warning restore 649
        private GameObject PrefabPlayer;
        private Text remainingPoints;
        private Text LifeText;
        private Text AgilityText;
        private Text DexterityText;
        private Text StrengthText;
        private Text StaminaText;
        private Text HungerText;

        private int Skills = 5;
        private int Health = 100;
        private int Hunger = 100;
        private int Stamina = 100;
        private float Agility = 1f;
        private float Dexterity = 1f;
        private float Strength = 1f;


        public void Start()
        {
            PrefabPlayer = GameObject.Find("Robot Kyl");
            if (PrefabPlayer == null)
            {
                Debug.Log("An error has occured");
                this.enabled = false;
            }
            else if (Pannel != null)
            {
                PrefabPlayer.GetComponent<Animator>().enabled = false;
                PrefabPlayer.GetComponent<Player_Movement>().enabled = false;
                PrefabPlayer.GetComponent<Camera_Movement>().enabled = false;
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
                LifeText.text = "" + Health;
                HungerText.text = "" + Hunger;
                StaminaText.text = "" + Stamina;
                AgilityText.text = "" + Agility;
                DexterityText.text = "" + Dexterity;
                StrengthText.text = "" + Strength;
                StartCoroutine(EndChoice());
            }
            else 
            {
                Data.MaxLife = Health;
                Data.MaxHunger = Hunger;
                Data.MaxStamina = Stamina;
                Data.Strength = Strength;
                Data.Agility = Agility;
                Data.Dexterity = Dexterity + 0.5f;
                Data.Life = Health;
                Data.Hunger = Hunger;
                Data.Stamina = Stamina;

                this.enabled = false;
            }
        }

        public void LifeUp(bool up)
        {
            if (up)
            {
                if (Skills > 0)
                {
                    Health += 10;
                    Skills--;
                }
            }
            else if (Health > 100)
            {
                Health -= 10;
                Skills++;
            }
            remainingPoints.text = "" + Skills;
            LifeText.text = "" + Health;
        }

        public void HungerUp(bool up)
        {
            if (up)
            {
                if (Skills > 0)
                {
                    Hunger += 10;
                    Skills--;
                }
            }
            else if (Hunger > 100)
            {
                Hunger -= 10;
                Skills++;
            }
            remainingPoints.text = "" + Skills;
            HungerText.text = "" + Hunger;
        }

        public void StaminaUp(bool up)
        {
            if (up)
            {
                if (Skills > 0)
                {
                    Stamina += 5;
                    Skills--;
                }
            }
            else if (Stamina > 100)
            {
                Stamina -= 5;
                Skills++;
            }
            remainingPoints.text = "" + Skills;
            StaminaText.text = "" + Stamina;
        }

        public void StrengthUp(bool up)
        {
            if (up)
            {
                if (Skills > 0)
                {
                    Strength += 0.1f;
                    Skills--;
                }
            }
            else if (Strength > 1f)
            {
                Strength -= 0.1f;
                Skills++;
            }
            remainingPoints.text = "" + Skills;
            StrengthText.text = "" + Strength;
        }

        public void AgilityUp(bool up)
        {
            if (up)
            {
                if (Skills > 0)
                {
                    Agility += 0.1f;
                    Skills--;
                }
            }
            else if (Agility > 1f)
            {
                Agility -= 0.1f;
                Skills++;
            }
            remainingPoints.text = "" + Skills;
            AgilityText.text = "" + Agility;
        }

        public void DexterityUp(bool up)
        {
            if (up)
            {
                if (Skills > 0)
                {
                    Dexterity += 0.1f;
                    Skills--;
                }
            }
            else if (Dexterity > 1f)
            {
                Dexterity -= 0.1f;
                Skills++;
            }
            remainingPoints.text = "" + Skills;
            DexterityText.text = "" + Dexterity;
        }

        private IEnumerator EndChoice()
        {
            yield return new WaitForSeconds(15);
            Choosing(false);
        }


        public void Choosing(bool IsQuiting)
        {
            //if the player quits the screen or if the the time has ended
            if (IsQuiting)
                StopCoroutine(EndChoice());
            //if the player didn't choose, remainings points are put randomly into 1 stat only
            if (Skills > 0)
            {
                System.Random rand = new System.Random();
                int rnd = rand.Next(6);
                switch (rnd)
                {
                    case 1:
                        while (Skills > 0)
                        {
                            Skills--;
                            Health += 10;
                        }
                        break;
                    case 2:
                        while (Skills > 0)
                        {
                            Skills--;
                            Hunger += 10;
                        }
                        break;
                    case 3:
                        while (Skills > 0)
                        {
                            Skills--;
                            Stamina += 5;
                        }
                        break;
                    case 4:
                        while (Skills > 0)
                        {
                            Skills--;
                            Strength += 0.1f;
                        }
                        break;
                    case 5:
                        while (Skills > 0)
                        {
                            Skills--;
                            Agility += 0.1f;
                        }
                        break;
                    default:
                        while (Skills > 0)
                        {
                            Skills--;
                            Dexterity += 0.1f;
                        }
                        break;
                }
            }
            //aplies choices to the player's Data
            Data.MaxLife = Health;
            Data.MaxHunger = Hunger;
            Data.MaxStamina = Stamina;
            Data.Strength = Strength;
            Data.Agility = Agility;
            Data.Dexterity = Dexterity;
            Data.Life = Health;
            Data.Hunger = Hunger;
            Data.Stamina = Stamina;

            Pannel.SetActive(false);
            PrefabPlayer.GetComponent<Animator>().enabled = true;
            PrefabPlayer.GetComponent<Player_Movement>().enabled = true;
            PrefabPlayer.GetComponent<Camera_Movement>().enabled = true;
            this.enabled = false;
        }
    }
}

