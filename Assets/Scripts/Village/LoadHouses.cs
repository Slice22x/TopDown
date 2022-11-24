using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.SceneManagement.SceneManager;
using UnityEngine.SceneManagement;

public class LoadHouses : MonoBehaviour
{

    public List<HouseInfo> HouseScenes = new List<HouseInfo>();

    void Start()
    {
        foreach (HouseInfo i in HouseScenes)
        {
            LoadSceneAsync(i.HouseName,LoadSceneMode.Additive);
        }   
    }

    public void UnloadHouses()
    {
        foreach(HouseInfo i in HouseScenes)
        {
            if (GetSceneByName(i.HouseName).isLoaded)
            {
                UnloadSceneAsync(i.HouseName);
            }
        }
    }

    void Update()
    {
        
    }
}
