using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using UnityEditor;
using UnityEngine.SceneManagement;


namespace Joueur
{
    public class StatsController : MonoBehaviourPunCallbacks
    {

        #region Singleton
        public static StatsController instance;

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogWarning("il y a dejà une instance de statsController");
                return;
            }
            instance = this;
        }
        #endregion

#pragma warning disable 649
        [SerializeField] PlayerData Data;
    #pragma warning restore 649
        public UnityEvent OnDeath;
        public UIBarUpdate barStamina;
        public UIBarUpdate barHealth;
        public UIBarUpdate barHunger;
        private float staminaTimer = 0.0f;
        private float hungerTimer = 0.0f;
        private int MenuScene = 0;
        private bool isAlive = true;

        void Update ()
        {
            if (isAlive)
            {
                refreshStamina();
                refreshSpeed();
                refreshHunger();
            }
        }

        public void UpdateChoppingStrength(float st)
        {
            Data.ChoppingStrength = st;
        }
        public void UpdateMiningStrength(float st)
        {
            Data.MiningStrength = st;
        }

        public void UpdateStrength(float st)
        {
            Data.Strength = st;
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
                    Data.speed = 7.0f * Data.Agility;
                    break;
                default:
                    Data.speed = 3.0f * Data.Agility;
                    break;
            }
        }

        private void refreshStamina()
        {
            if (Data.isIdle)
            {
                if(!Data.isOnJump)
                    checkTimer();
            }
            else
            {
                switch (Data.speedState)
                {
                    case PlayerData.State.walking:
                    case PlayerData.State.crouching:
                        if(!Data.isOnJump)
                            checkTimer();
                        break;
                    case PlayerData.State.running:
                        if(!Data.isOnJump)
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
            if (Data.Hunger <= 0f)
            {
                looseLife(0.4f * Time.deltaTime);
            }
        }

        private void checkTimer()
        {
            if (staminaTimer >= 1f)
            {
                gainStamina(15f * Time.deltaTime);
            }
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
            barStamina?.Barupdate(Data.Stamina/Data.MaxStamina);
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
            barStamina?.Barupdate(Data.Stamina/Data.MaxStamina);
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
            barHealth?.Barupdate(Data.Life/Data.MaxLife);
        }

        public void looseLife(float loss)
        {
            staminaTimer = 0f;
            if (Data.Life + loss <= 0f)
            {
                Data.Life = -1f;
                isAlive = false;
                OnDeath?.Invoke();
            }
            else
            {
                Data.Life -= loss;
            }
            barHealth?.Barupdate(Data.Life/Data.MaxLife);
        }

        public void getHit(float dmg)
        {
            looseLife(dmg);
            Debug.Log("ur gettting hit from getHit in statsController");
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
            barHunger?.Barupdate(Data.Hunger/Data.MaxHunger);
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
            barHunger?.Barupdate(Data.Hunger/Data.MaxHunger);
        }
        
        public override void OnLeftRoom()
        {
            Data.Life = 100f;
            Data.speedState = PlayerData.State.walking;
            SceneManager.LoadScene(MenuScene);
        }
    }
}