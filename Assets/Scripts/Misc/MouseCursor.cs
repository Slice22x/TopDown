using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseCursor : MonoBehaviour
{
    public GameObject MiddlePoint;
    public GameObject[] Cross = new GameObject[4];
    public Shooting Gun;
    public float Pos;
    void Start()
    {
        Cursor.visible = false;
    }

    public void SetColour(Color Col)
    {
        for (int i = 0; i < Cross.Length; i++)
        {
            Cross[i].GetComponent<SpriteRenderer>().color = Col;
        }
    }

    public void ResetGun()
    {
        Gun = null;
    }

    void FixedUpdate()
    {

        Cursor.visible = false;
        if(Gun == null)
        {
            Gun = Movement.Instance.GetComponentInChildren<Shooting>();
            Pos = 0;
        }

        if(Pos == 0)
        {
            for (int i = 0; i < Cross.Length; i++)
            {
                Cross[i].SetActive(false);
            }
        }

        else
        {
            for (int i = 0; i < Cross.Length; i++)
            {
                Cross[i].SetActive(true);
            }
        }

        if(Gun != null)
        {
            Pos = 0.2f * Gun.Weapon.MaxSpread;
            for (int i = 0; i < Cross.Length; i++)
            {
                Cross[i].SetActive(true);
            }
        }

        Cross[0].transform.position = transform.position - new Vector3(0f, Pos);
        Cross[1].transform.position = transform.position - new Vector3(Pos, 0f);
        Cross[2].transform.position = transform.position - new Vector3(0f, -Pos);
        Cross[3].transform.position = transform.position - new Vector3(-Pos, 0f);

        Vector2 MousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        transform.position = MousePos;
    }
}
