using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Joueur
{
    public class StatsController : MonoBehaviour
    {
    #pragma warning disable 649
        [SerializeField] PlayerData Data;
    #pragma warning restore 649
        private float staminaTimer = 0.0f;
        private float hungerTimer = 0.0f;

        void Update ()
        {
            refreshStamina();
            refreshSpeed();
            refreshHunger();
        }

        private void refreshSpeed()
        {
            //modifying speed
            switch (Data.speedState)
            {
                case PlayerData.State.crouching:
                    Data.speed = 3.0f * Data.Agility;
                    break;
                case PlayerData.State.running:
                    Data.speed = 9.0f * Data.Agility;
                    break;
                default:
                    Data.speed = 5.0f * Data.Agility;
                    break;
            }
        }

        private void refreshStamina()
        {
            if (Data.isIdle)
            {
                checkTimer();
            }
            else
            {
                switch (Data.speedState)
                {
                    case PlayerData.State.walking:
                    case PlayerData.State.crouching:
                        checkTimer();
                        break;
                    case PlayerData.State.running:
                        looseStamina(5f * Time.deltaTime);
                        break;
                    default:
                        break;
                }
            }
        }

        private void refreshHunger()
        {
            if (hungerTimer <= 0f)
            {
                looseHunger(1*Time.deltaTime);
            }
            hungerTimer -= Time.deltaTime;
        }

        private void checkTimer()
        {
            if (staminaTimer >= 1f)
                gainStamina(15f * Time.deltaTime);
            else
                staminaTimer += Time.deltaTime;
        }

        public void gainStamina(float gain)
        {
            if (Data.Stamina + gain >= Data.MaxStamina)
            {
                Data.Stamina = Data.MaxStamina;
            }
            else
            {
                Data.Stamina += gain;
            }
        }

        public void looseStamina(float loss)
        {
            staminaTimer = 0f;
            if (Data.Stamina - loss <= 0f)
            {
                Data.Stamina = 0f;
            }
            else
            {
                Data.Stamina -= loss;
            }
        }

        public void gainLife(float gain)
        {
            if (Data.Life + gain >= Data.MaxLife)
            {
                Data.Life = Data.MaxLife;
            }
            else
            {
                Data.Life += gain;
            }
        }

        public void looseLife(float loss)
        {
            staminaTimer = 0f;
            if (Data.Life + loss <= 0f)
            {
                Data.Life = 0f;
            }
            else
            {
                Data.Life -= loss;
            }
        }

        public void gainHunger(float gain)
        {
            hungerTimer = 30f;
            if (Data.Hunger + gain >= Data.MaxHunger)
            {
                Data.Hunger = Data.MaxHunger;
            }
            else
            {
                Data.Hunger += gain;
            }
        }

        public void looseHunger(float loss)
        {
            if (Data.Hunger - loss <= 0f)
            {
                Data.Hunger = 0f;
            }
            else{
                Data.Hunger -= loss;
            }
        }
    }
}