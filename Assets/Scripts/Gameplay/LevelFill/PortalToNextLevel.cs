using Gameplay.Character;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PortalToNextLevel : MonoBehaviour
{
    private bool _isLastStage = false;
    private bool isPortal = false;

    private void Start()
    {
        isPortal = false;
    }

    public void Update()
    {
        if(!transform.gameObject.activeSelf) return;
        if(isPortal) return;
        if (Vector3.Distance(transform.position, LevelManager.Instance.GetPlayerPos()) < 2f)
        {
            isPortal = true;
            Player.Instance.GetHp(Player.Instance.maxHp/2);
            LevelManager.Instance.Coins = 0;
            EventGameManager.Instance.OnCoinChange?.Invoke();
            EventGameManager.Instance.OnPlayerHpChange?.Invoke();
            LevelManager.Instance.countLevel++;
            var lm = LevelManager.Instance;
            if (lm.currentLevel == 5)
            {
                lm.currentLevel = 1;
                SceneManager.LoadScene(lm.currentLevel);
                return;
            }
            if (lm.currentLevel == 4)
                _isLastStage = true;
            if(_isLastStage)            
                lm.currentLevel = Random.Range(1, 5);
            else
                lm.currentLevel++;
            
            SceneManager.LoadScene(lm.currentLevel);
        }
    }
}
