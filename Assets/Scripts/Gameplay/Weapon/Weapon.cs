using UnityEngine;

namespace Gameplay.Weapon
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] public int dmg;
    }
}