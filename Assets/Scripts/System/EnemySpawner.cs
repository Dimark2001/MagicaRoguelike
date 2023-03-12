using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<BaseCharacter> enemyToSpawn;
    [SerializeField] private List<EnemyController> availableEnemy;
    [SerializeField] private List<BaseCharacter> enemyChallenge;
    [SerializeField] private float range = 100.0f;
    [SerializeField] private float timeToSpawn;
    [SerializeField] private float globalTime;
    [SerializeField] private bool pause;
    [SerializeField] private int lvlDifficulty = 100;

    private void Start()
    {
        Spawn();
    }

    private void Update()
    {
        if(pause) return;
        
        globalTime += Time.deltaTime;

        if (globalTime >= 10 && globalTime < 30 && lvlDifficulty != 1)
            lvlDifficulty = 0;
        if (globalTime >= 30 && globalTime < 60 && lvlDifficulty != 3)
            lvlDifficulty = 2;
        if (globalTime >= 60 && globalTime < 120 && lvlDifficulty != 5)
            lvlDifficulty = 4;
        if (globalTime >= 120 && globalTime < 160 && lvlDifficulty !=7)
            lvlDifficulty = 6;
        if (globalTime >= 160 && globalTime < 200 && lvlDifficulty !=9)
            lvlDifficulty = 8;
        if (globalTime >= 200 && globalTime < 280 && lvlDifficulty != 11)
            lvlDifficulty = 10;
        if (globalTime >= 280 && globalTime < 400 && lvlDifficulty != 13)
            lvlDifficulty = 12;
        if (globalTime >= 400 && globalTime < 500 && lvlDifficulty != 15)
            lvlDifficulty = 14;
        if (globalTime >= 500)
            lvlDifficulty = 16;
        if (globalTime >= 600)
            lvlDifficulty = 18;
    }

    private void Spawn()
    {
        if(LevelManager.Instance.GetPlayerPos() == Vector3.zero) return;

        switch (lvlDifficulty)
        {
            case 0:
                lvlDifficulty++;
                AddEnemyInList(0, 2);
                break;
            case 2:
                lvlDifficulty++;
                AddEnemyInList(0, 1);
                AddEnemyInList(1, 1);
                break;
            case 4:
                lvlDifficulty++;
                AddEnemyInList(0, 1);
                AddEnemyInList(1, 1);
                AddEnemyInList(2, 1);
                break;
            case 6:
                lvlDifficulty++;
                AddEnemyInList(0, 1);
                AddEnemyInList(1, 1);
                AddEnemyInList(2, 1);
                AddEnemyInList(3, 1);
                break;
            case 8:
                lvlDifficulty++;
                AddEnemyInList(0, 1);
                AddEnemyInList(1, 1);
                AddEnemyInList(2, 1);
                AddEnemyInList(3, 1);
                AddEnemyInList(4, 1);
                break;
            case 10:
                break;
            case 12:
                break;
            case 14:
                break;
            case 16:
                break;
            case 18:
                break;
        }
        
        var inVal = 0f;
        DOTween.To(() => inVal, x => inVal = x, 1, timeToSpawn).OnComplete(() =>
        {
            SpawnEnemy(LevelManager.Instance.GetPlayerPos());
            Spawn();
        });
    }

    private void AddEnemyInList(int index, int count)
    {
        for (var i = 0; i < count; i++)
        {
            enemyToSpawn.Add(availableEnemy[index]);
        }
    }

    public void SpawnEnemy(Vector3 pos)
    {
        var parent = DeadBodyCleaner.Instance.transform;
        foreach (var enemyPrefab in enemyToSpawn)
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
