using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmountOfEnemies : MonoBehaviour
{

    public List<GameObject> Enemies;
    public int Limit;

    public static AmountOfEnemies Instance;
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject item in Enemies)
        {
            if(item == null)
            {
                Enemies.Remove(item);
            }
        }
    }

    public void DestroyAllEnemies()
    {
        foreach (GameObject item in Enemies)
        {
            Destroy(item);
            Enemies.Remove(item);
        }
    }
}
