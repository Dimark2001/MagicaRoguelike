using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemySpawner : Singleton<EnemySpawner>
{
    [SerializeField] private List<BaseCharacter> enemyToSpawn;
    [SerializeField] private List<EnemyController> availableEnemy;
    [SerializeField] private List<BaseCharacter> enemyChallenge;
    [SerializeField] private float range = 100.0f;
    [SerializeField] private float timeToSpawn;
    [SerializeField] private float globalTime;
    [SerializeField] private bool pause;
    [SerializeField] private int lvlDifficulty = 100;
    [SerializeField] private int dif0;
    [SerializeField] private int dif1;
    [SerializeField] private int dif2;
    [SerializeField] private int dif3;
    [SerializeField] private int dif4;
    [SerializeField] private int dif5;
    // [SerializeField] private int dif6;
    // [SerializeField] private int dif7;
    // [SerializeField] private int dif8;

    private Sequence _sequence;
    private void Start()
    {
        Spawn();
    }

    public void ResetTime()
    {
        globalTime = 0;
        lvlDifficulty = 100;
        timeToSpawn = 4;
        enemyToSpawn = new List<BaseCharacter>();
    }

    private void Update()
    {
        if(LevelManager.Instance.currentLevel == 0) return;
        if(pause) return;
        
        globalTime += Time.deltaTime;

        if (globalTime >= dif0 && globalTime < dif1 && lvlDifficulty != 1)
        {
            lvlDifficulty = 0;
            _sequence.Kill();
            Spawn();
        }

        if (globalTime >= dif1 && globalTime < dif2 && lvlDifficulty != 3)
        {
            lvlDifficulty = 2;
            _sequence.Kill();
            Spawn();
        }

        if (globalTime >= dif2 && globalTime < dif3 && lvlDifficulty != 5)
        {
            lvlDifficulty = 4;
            _sequence.Kill();
            Spawn();
        }

        if (globalTime >= dif3 && globalTime < dif4 && lvlDifficulty != 7)
        {
            lvlDifficulty = 6;
            _sequence.Kill();
            Spawn();
        }

        if (globalTime >= dif4 && globalTime < dif5 && lvlDifficulty != 9)
        {
            _sequence.Kill();
            lvlDifficulty = 8;
            Spawn();
        }
    }

    private void Spawn()
    {
        if(LevelManager.Instance.GetPlayerPos() == Vector3.zero) return;

        switch (lvlDifficulty)
        {
            case 0:
                timeToSpawn = 6;
                lvlDifficulty++;
                AddEnemyInList(0, 2);
                break;
            case 2:
                timeToSpawn = 8;
                lvlDifficulty++;
                AddEnemyInList(0, 1);
                AddEnemyInList(1, 1);
                break;
            case 4:
                timeToSpawn = 12;
                lvlDifficulty++;
                AddEnemyInList(0, 1);
                AddEnemyInList(1, 1);
                AddEnemyInList(2, 1);
                break;
            case 6:
                timeToSpawn = 17;
                lvlDifficulty++;
                AddEnemyInList(0, 1);
                AddEnemyInList(1, 1);
                AddEnemyInList(2, 1);
                AddEnemyInList(3, 1);
                break;
            case 8:
                timeToSpawn = 22;
                lvlDifficulty++;
                AddEnemyInList(0, 1);
                AddEnemyInList(1, 1);
                AddEnemyInList(2, 1);
                AddEnemyInList(3, 1);
                AddEnemyInList(4, 1);
                break;
        }

        _sequence = DOTween.Sequence();
        var inVal = 0f;
        _sequence.Append(DOTween.To(() => inVal, x => inVal = x, 1, timeToSpawn).OnComplete(() =>
        {
            SpawnEnemy(LevelManager.Instance.GetPlayerPos());
            Spawn();
        }));
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
