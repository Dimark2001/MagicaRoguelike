using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    
    void Awake()
    {
        if(Instance != null)
            Destroy(gameObject);

        Instance = this;
    }
}
