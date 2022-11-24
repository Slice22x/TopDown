using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThowingKnife : MonoBehaviour
{
    public GameObject Enemy;
    public bool CanThrowEnemy;
    public Enemies Script;
    void Start()
    {
        if(SceneType.Instance.ThisScene == SceneType.TypeOfScene.Village)
        {
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero);

        if(Enemy == null)
        {
            CanThrowEnemy = false;
            FindObjectOfType<MouseCursor>().SetColour(Color.red);
            Enemy = null;
        }

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                FindObjectOfType<MouseCursor>().SetColour(Color.green);
                Enemy = hit.collider.transform.Find("Outline").gameObject;
                Script = Enemy.GetComponentInParent<Enemies>();

                

                if (Script.InDazedState != true)
                {
                    FindObjectOfType<MouseCursor>().SetColour(Color.red);
                }

                if (Script.InDazedState != true)
                    return;

                Enemy.SetActive(true);
                CanThrowEnemy = true;
            }

        }
        if(hit.collider == null && Enemy != null)
        {
            CanThrowEnemy = false;
            Enemy.SetActive(false);
            FindObjectOfType<MouseCursor>().SetColour(Color.red);
            Enemy = null;
        }
    }
}
