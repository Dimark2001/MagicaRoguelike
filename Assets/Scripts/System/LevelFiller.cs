using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelFiller : Singleton<LevelFiller>
{
    [SerializeField] private GameObject rareChestTemplate;
    [SerializeField] private GameObject commonChestTemplate;
    [SerializeField] private List<Transform> chestSpawnPositions;
    [SerializeField] private int countCommon;
    [SerializeField] private int countRare;

    public void Start()
    {
        chestSpawnPositions = new List<Transform>();
        var points = FindObjectsOfType<Point>().Select(point => point.transform).ToList();
        chestSpawnPositions.AddRange(points);
        SpawnChest(countRare, rareChestTemplate);
        SpawnChest(countCommon, commonChestTemplate);
    }

    private void SpawnChest(int count, GameObject chestTemplate)
    {
        for (int i = 0; i < count; i++)
        {
            var r = Random.Range(0, chestSpawnPositions.Count);
            Instantiate(chestTemplate, chestSpawnPositions[r]);
            chestSpawnPositions.Remove(chestSpawnPositions[r]);
        }
    }
}
