using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SortLayers : MonoBehaviour
{
    SpriteRenderer[] Rends;
    public int Offset;
    void Start()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Rends = FindObjectsOfType<SpriteRenderer>();
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneType.Instance.ThisScene == SceneType.TypeOfScene.Village)
        {
            if(Rends != null)
            {
                foreach (Renderer i in Rends)
                {
                    if (i != null)
                    {
                        i.sortingOrder = ((int)(i.transform.position.y * -100) + Offset);
                    }

                }
            }
        }

    }
}
