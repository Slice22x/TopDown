using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{
    public Dictionary<int, Image> HeartImgs = new Dictionary<int, Image>();
    public Sprite FullHeart;
    public Sprite EmptyHeart;
    public Health PlayerHealth;
    void Start()
    {
        HeartImgs.Clear();
        foreach (HeartAdder item in FindObjectsOfType<HeartAdder>())
        {
            item.LoadHearts();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerHealth == null)
        {
            PlayerHealth = FindObjectOfType<Health>();
        }
        if (PlayerHealth.Hearts > PlayerHealth.NumberOfHearts)
        {
            PlayerHealth.Hearts = PlayerHealth.NumberOfHearts;
        }

        for (int i = 0; i < HeartImgs.Count; i++)
        {
            if (i < PlayerHealth.Hearts)
            {
                HeartImgs[i].sprite = FullHeart;
            }
            else
            {
                HeartImgs[i].sprite = EmptyHeart;
            }

            if (i < PlayerHealth.NumberOfHearts)
            {
                HeartImgs[i].enabled = true;
            }
            else
            {
                HeartImgs[i].enabled = false;
            }
        }
    }
}
