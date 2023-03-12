using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PortalToNextLevel : MonoBehaviour
{
    private bool _isLastStage = false;
    
    public void Update()
    {
        if(!transform.gameObject.activeSelf) return;
        
        if (Vector3.Distance(transform.position, LevelManager.Instance.GetPlayerPos()) < 2f)
        {
            var lm = LevelManager.Instance;
            if (lm.currentLevel == 6)
            {
                lm.currentLevel = 1;
                LevelManager.Instance.Coins = 0;
                EventGameManager.Instance.OnCoinChange?.Invoke();
                SceneManager.LoadScene(lm.currentLevel);
                return;
            }
            if (lm.currentLevel == 5)
                _isLastStage = true;
            if(_isLastStage)            
                lm.currentLevel = Random.Range(1, 5);
            else
                lm.currentLevel++;
            LevelManager.Instance.Coins = 0;
            EventGameManager.Instance.OnCoinChange?.Invoke();
            SceneManager.LoadScene(lm.currentLevel);
        }
    }
}
