using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public enum State
    {
        walking,
        running,
        crouching
    }
    public float MaxLife = 100;
    public float MaxStamina = 100;
    public float MaxHunger = 600;
    public float Life = 100;
    public float Stamina = 100;
    public float Hunger = 600;
    public float Agility = 1;
    public float Dexterity = 1;
    public float Strength = 1;
    public State speedState;
    public bool isIdle;
    public float speed;
}
