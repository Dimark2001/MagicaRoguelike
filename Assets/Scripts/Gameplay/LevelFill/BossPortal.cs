using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossPortal : MonoBehaviour
{
    [SerializeField] private List<EnemyController> bossList;
    [SerializeField] private GameObject portal;
    private EnemyController _boss;
    private bool _isBoss = false;
    private void Awake()
    {
        for (int i = 0; i < bossList.Count; i++) {
            var temp = bossList[i];
            int randomIndex = Random.Range(i, bossList.Count);
            bossList[i] = bossList[randomIndex];
            bossList[randomIndex] = temp;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(bossList.Count == 0) return;
        _boss = Instantiate(bossList.First(), transform.position, Quaternion.identity);
        bossList.Remove(bossList.First());
        GetComponent<Collider>().enabled = false;
    }

    private void Update()
    {
        if (_boss != null)
        {
            _isBoss = true;
            if(_boss.Hp <= 0)
                SpawnPortal();
        }

        if (_isBoss)
        {
            if(_boss == null)
                SpawnPortal();
        }
    }

    private void SpawnPortal()
    {
        portal.SetActive(true);
    }
}
