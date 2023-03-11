using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalToNextLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var lm = LevelManager.Instance;
        lm.currentLevel++;
        SceneManager.LoadScene(lm.currentLevel);
    }
}
