using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


namespace Joueur
{
    public class Player_Stats : MonoBehaviour
    {

        [SerializeField] private float MaxLife;
        private float CurrentLife;
        [SerializeField] private float MaxStamina;
        [SerializeField] private float Stamina;
        [SerializeField] private float MaxHunger;
        [SerializeField] private float Agility;
        [SerializeField] private float Dexterity;
        private float Hunger;
        //if we allow a more important inventory
        [SerializeField] private float Strength;


        //getters == IsSomething ## getters AND setters == HasSomething
        public bool IsAlive => CurrentLife > 0f;
        public bool IsHungry => Hunger <= 0f;
        public bool IsWorn => Stamina <= 0f;

        public float HasHunger
        {
            get => Hunger;
            set
            { 
                if (value > MaxHunger)
                {
                    Hunger = MaxHunger;
                }
                else
                {
                    if (value < 0f)
                    {
                        Hunger = 0f;
                    }
                    else
                    {
                        Hunger = value;
                    }
                }
            } 
        }

        public float HasStamina
        {
            get => Stamina;
            set
            { 
                if (value > MaxStamina)
                {
                    Stamina = MaxStamina;
                }
                else
                {
                    if (value < 0f)
                    {
                        Stamina = 0f;
                    }
                    else
                    {
                        Stamina = value;
                    }
                }
            }
        }

        public float HasLife
        {
            get => CurrentLife;
            set
            {
                if (value > MaxLife)
                {
                    CurrentLife = MaxLife;
                }
                else if (value <= 0f)
                {
                    CurrentLife = -1f;
                }
                else
                {
                    CurrentLife = value;
                }
                Debug.Log($"current life = {this.CurrentLife}");
            }
        }

        public float GetStrength
        {
            get => Strength;
        }

        public float GetAgility
        {
            get => Agility;
        }

        public float GetDexterity
        {
            get => Dexterity;
        }
        //end of getters and setters

        public void Choosing(int healthPoints, int hungerPoints, int staminaPoints, float strengthPoints, float AgilityPoints, float DexterityPoints)
        {
            MaxLife = healthPoints;
            MaxHunger = hungerPoints;
            MaxStamina = staminaPoints;
            Strength = strengthPoints;
            Agility = AgilityPoints;
            Dexterity = DexterityPoints;

            CurrentLife = MaxLife;
            Hunger = MaxHunger;
            Stamina = MaxStamina;
        }
    }
}