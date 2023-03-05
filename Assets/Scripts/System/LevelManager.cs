using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public InputActionReference input;
    void Awake()
    {
        if(Instance != null)
            Destroy(gameObject);

        Instance = this;
    }
}
