using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smarties : MonoBehaviour
{
    Enemies Enemy;
    public enum SizeState
    {
        Big = 3,
        Medium = 2,
        Small = 1
    }
    public SizeState StateSize;
    public Color ThisColor;
    void Start()
    {
        Enemy = GetComponent<Enemies>();
        Enemy.Health *= Random.Range((int)StateSize, (int)StateSize + 1);
        Enemy.Defence *= Random.Range((int)StateSize, (int)StateSize + 1);
        Enemy.Speed *= Random.Range((float)StateSize, (float)StateSize + 1f);
        Enemy.Damage *= Random.Range((int)StateSize, (int)StateSize + 1);
        transform.localScale = new Vector3((float)StateSize / 10f + 0.5f, (float)StateSize / 10f + 0.5f);
        ThisColor = Random.ColorHSV();
        GetComponentInChildren<SpriteRenderer>().color = ThisColor;
        transform.Find("Icon").GetComponent<SpriteRenderer>().color = ThisColor;
    }

    void Update()
    {
        if(Enemy.Health <= 0)
        {
            if(StateSize != SizeState.Small)
            {
                GameObject Smarties1 = Instantiate(EnemyFinder.Instance.GetEnemyfromName("SM").EnemyObjectt, transform.position + new Vector3(0.5f, -1f), Quaternion.identity);
                Smarties1.GetComponent<Smarties>().StateSize = (SizeState)(int)StateSize - 1;
                GameObject Smarties2 = Instantiate(EnemyFinder.Instance.GetEnemyfromName("SM").EnemyObjectt, transform.position + new Vector3(-0.5f, -1f), Quaternion.identity);
                Smarties2.GetComponent<Smarties>().StateSize = (SizeState)(int)StateSize - 1;
                Destroy(gameObject);
            }
        }
    }
}
