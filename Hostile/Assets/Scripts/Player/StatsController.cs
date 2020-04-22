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
        [Header("Datas")]
        [SerializeField] PlayerData Data;
        [SerializeField] LightingManager timeref;
        [Header("UI Reference")]
        [SerializeField] public UIBarUpdate barStamina;
        [SerializeField] public UIBarUpdate barHealth;
        [SerializeField] public UIBarUpdate barHunger;
        [Header("Event")]
        [SerializeField] public UnityEvent OnDeath;
    #pragma warning restore 649
        
        private float staminaTimer = 0.0f;
        private float hungerTimer = 0.0f;
        private float coldTimer = 0.0f;
        [SerializeField] private float crouchSpeed = 2f;
        [SerializeField] private float walkSpeed = 4f;
        [SerializeField] private float runSpeed = 5.5f;
        private int MenuScene = 0;
        private bool isAlive = true;
        private bool isStarving = false;
        private bool isLowStamina = false;
        private bool isNight = false;
        public Player_Sound_Reference sounds;

        private bool isBreathless = false;
        private PhotonView PV;

        void Start()
        {
            OnDeath.AddListener(delegate { reset();});
            PV = sounds.gameObject.GetComponent<PhotonView>();
        }

        void Update ()
        {
            if (isAlive)
            {
                refreshStamina();
                refreshSpeed();
                refreshHunger();
            }
            checkday();
            if (isNight)
            {
                //FIXME
            }
            if (isLowStamina && !isBreathless && sounds.source3.volume < 0.00033f)
            {
                Debug.Log(PV.gameObject.name);
                sounds.PlayBreathing(PV.ViewID, 0);
                isBreathless = true;
            }
            if(!isLowStamina && isBreathless && sounds.source3.volume > 0.00033f)
            {
                Debug.Log("not anymore !");
                sounds.PlayBreathing(PV.ViewID, 1);
                isBreathless = false;
            }
        }

        private void checkday()
        {
            if (timeref.TimePercent >= 60f)
                isNight = true;
            else
                isNight = false;
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
                    Data.speed = crouchSpeed * Data.Agility;
                    break;
                case PlayerData.State.running:
                    if (isLowStamina)
                    {
                        float decreasingSpeed = Data.Stamina / (Data.MaxStamina/10f);
                        Data.speed = Mathf.Lerp(walkSpeed, runSpeed, decreasingSpeed);
                    }
                    else
                        Data.speed = runSpeed * Data.Agility;
                    break;
                default:
                    Data.speed = walkSpeed * Data.Agility;
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
            if(Data.Stamina <= Data.MaxStamina/10f)
                isLowStamina = true;
            else
                isLowStamina = false;
        }

        private void refreshHunger()
        {
            if (hungerTimer <= 0f)
            {
                looseHunger(1*Time.deltaTime);
            }
            if (Data.Hunger <= 0f)
            {
                if (hungerTimer <= -11f)
                {
                    hungerTimer = -1.0f;
                    this.getHit(2f);
                }
            }
            hungerTimer -= Time.deltaTime;
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
            if (Data.Life - loss <= 0f)
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

        public void getHit(float dmg, int pv = 0)
        {
            looseLife(dmg);
            sounds.Play(sounds.gameObject.transform.position,2,1); //hit
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

        public void reset()
        {
            Data.Life = 100f;
            Data.MaxStamina = 100f;
            Data.MaxHunger = 100f;
            Data.MaxLife = 100;
            Data.Life = 100;
            Data.Stamina = 100;
            Data.Hunger = 600;
            Data.Agility = 1;
            Data.Dexterity = 1;
            Data.Strength = 1;
            Data.ChoppingStrength = 1;
            Data.MiningStrength = 1;
            Data.Damage = 5;
            Data.speedState = PlayerData.State.walking;
            Data.isIdle = true;
            Data.isOnJump = false;
        }
        
        public override void OnLeftRoom()
        {
            reset();
            Data.speedState = PlayerData.State.walking;
            SceneManager.LoadScene(MenuScene);
        }
    }
}