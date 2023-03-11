using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class SelectorRandomObject : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs;
    [SerializeField] private bool noObject;
    void Awake()
    {
        if(transform.childCount > 0)
            Destroy(transform.GetChild(0).gameObject);
        
        var length = prefabs.Count;
        if (noObject) length++;

        var r = Random.Range(0, length);
        if(r < prefabs.Count)
            Instantiate(prefabs[r], transform);
    }

    [Button()]
    private void InEditorCastomize()
    {
        if(transform.childCount > 0)
            DestroyImmediate(transform.GetChild(0).gameObject);
        
        var length = prefabs.Count;
        if (noObject) length++;

        var r = Random.Range(0, length);
        if(r < prefabs.Count)
            Instantiate(prefabs[r], transform);
    }
}
