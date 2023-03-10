using Gameplay.Character;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float radiusAgro;
    [SerializeField] public float force;
    [SerializeField] public float stoppingDistance;
    [SerializeField] public int dmg;
    
    [SerializeField] public float timeToDeath;
    [SerializeField] public float timeKnockBack;
}