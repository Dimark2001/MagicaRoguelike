using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<BaseCharacter> enemyPrefabs;
    [SerializeField] private float range = 100.0f;

    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        var inVal = 0f;
        DOTween.To(() => inVal, x => inVal = x, 1, 10).OnComplete(() =>
        {
            SpawnEnemy(LevelManager.Instance.player.transform.position);
            Spawn();
        });
    }

    public void SpawnEnemy(Vector3 pos)
    {
        var parent = LevelManager.Instance.dynamicContainer;
        foreach (var enemyPrefab in enemyPrefabs)
        {
            tryAgain:
            if (RandomPoint(pos, range, out var point))
            {
                var enemy = Instantiate(enemyPrefab, point, Quaternion.identity, parent);
            }
            else
            {
                goto tryAgain;
            }
        }
    }
    
    private bool RandomPoint(Vector3 center, float zone, out Vector3 result)
    {
        var randomPoint = center + Random.insideUnitSphere * zone;
        if(NavMesh.SamplePosition(randomPoint, out var hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }
        result = Vector3.zero;
        return false;
    }
}
