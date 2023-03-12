using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class DeadBodyCleaner : Singleton<DeadBodyCleaner>
{
    public List<GameObject> enemyBody;
    public float timeToClean;
    private float _cleaningTime;
    private int _lastCountBody;
    private int _currentCountBody;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        enemyBody = new List<GameObject>();
        _currentCountBody = enemyBody.Count;
        _lastCountBody = _currentCountBody;
    }

    private void Update()
    {
        if(enemyBody.Count == 0) return;
        
        _currentCountBody = enemyBody.Count;
        if (_currentCountBody != _lastCountBody)
        {
            _cleaningTime = timeToClean;
            _lastCountBody = _currentCountBody;
        }
        if (_cleaningTime <= 0)
        {
            _cleaningTime = timeToClean;
            for (var i = 0; i < enemyBody.Count; i++)
            {
                var body = enemyBody[i];
                Destroy(body);
            }
            enemyBody.RemoveAll(a => a == null);
        }
        else
        {
            _cleaningTime -= Time.deltaTime;
        }
    }
}
