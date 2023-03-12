using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartPos : MonoBehaviour
{
    [SerializeField] private float time = 0.2f;
    private void Update()
    {
        if (!(time >= 0)) return;
        time -= Time.deltaTime;
        var pl = LevelManager.Instance.player;
        pl.navMeshAgent.enabled = false;
        pl.transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        pl.navMeshAgent.enabled = true;
    }
}
