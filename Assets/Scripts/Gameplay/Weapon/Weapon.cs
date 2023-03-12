using UnityEngine;

namespace Gameplay.Weapon
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] public int dmg;
        [SerializeField] public float lifeTime;
        [SerializeField] public float endValue;
        [SerializeField] public float duration;
        [SerializeField] public float speed;
    }
}