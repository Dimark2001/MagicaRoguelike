using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalToNextLevel : MonoBehaviour
{
    public void Update()
    {
        if(!transform.gameObject.activeSelf) return;
        
        if (Vector3.Distance(transform.position, LevelManager.Instance.GetPlayerPos()) < 2f)
        {
            var lm = LevelManager.Instance;
            lm.currentLevel++;
            SceneManager.LoadScene(lm.currentLevel);
        }
    }
}
