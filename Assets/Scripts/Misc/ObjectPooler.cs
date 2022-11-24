using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public List<GameObject> PooledObj = new List<GameObject>();

    public static ObjectPooler Instance;

    public void Awake()
    {
        Instance = this;
    }

    public void Populate(GameObject P, int Count)
    {
        for (int i = 0; i <= Count; i++)
        {
            GameObject N = Instantiate(P);
            N.SetActive(false);
            PooledObj.Add(N);
            N = null;
        }
    }

    public GameObject GetFromPool(GameObject ToFind)
    {
        for (int i = 0; i < PooledObj.Count; i++)
        {
            if(!PooledObj[i].activeInHierarchy)
            {
                if (PooledObj[i] == ToFind)
                    return PooledObj[i];
                else
                    Debug.Log("No");
            }
        }
        return null;
    }

    public void DestroyObj(GameObject D)
    {
        D.SetActive(false);
    }
}
