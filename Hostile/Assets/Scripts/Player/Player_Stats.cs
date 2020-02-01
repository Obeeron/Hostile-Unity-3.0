using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Joueur
{
    public class Player_Stats : MonoBehaviour 
    {

        //?stamina de crouch?
        private float MaxLife = 100f;
        private float CurrentLife;
        private float MaxStamina = 200f;
        [SerializeField] private float Stamina;
        private float MaxHunger = 250f;
        private float Hunger;
        //if we allow a more important inventory
        private int MaxInventory = 15;
        private int InventoryCount = 15;


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
        //end of getters and setters

        public void Choosing(int healthPoints, int hungerPoints, int staminaPoints, int inventoryPoints)
        {
            MaxLife += healthPoints * 10;
            MaxHunger += hungerPoints * 10;
            MaxStamina += staminaPoints * 10;
            MaxInventory += inventoryPoints * 10;
            starting();
        }

        private void starting()
        {
            CurrentLife = MaxLife;
            Hunger = MaxHunger;
            Stamina = MaxStamina;
            InventoryCount = MaxInventory;
        }
    }
}