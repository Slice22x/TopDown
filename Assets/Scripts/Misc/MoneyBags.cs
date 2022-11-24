using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyBags : MonoBehaviour
{
    public int Money;
    public Sprite[] MoneySprites;
    private SpriteRenderer Renderer;
    void Start()
    {
        Money = Random.Range(1, 150);
        Renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Money >= 100)
        {
            Renderer.sprite = MoneySprites[0];
        }
        else if(Money >= 50 && Money < 100)
        {
            Renderer.sprite = MoneySprites[1];
        }
        else if(Money > 1 && Money < 50)
        {
            Renderer.sprite = MoneySprites[2];
        }
        else
        {
            Debug.LogError("Incorrect Value");
        }

        float WaitTime = 5f;
        WaitTime -= Time.unscaledDeltaTime;
        if (WaitTime <= 0)
        {
            LeanTween.move(gameObject, Movement.Instance.transform, 3f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            MoneyManager.Instance.AddMoney(Money * ComboSystem.Instance.Combo);
            Destroy(gameObject);
        }
    }
}
