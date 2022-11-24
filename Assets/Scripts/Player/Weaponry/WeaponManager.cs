using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class WeaponManager : MonoBehaviour
{

    public Shooting CurrentWeapon;
    public static GameObject NextWeapon;
    public GameObject ReloadIndicator;
    private Transform SpawnPoint;
    public float WaitTime = 3f;
    public bool NoPickup;

    public static WeaponManager Instance { get; private set; }

    void Start()
    {
        Instance = this;
        SpawnPoint = transform.GetChild(0).transform;
        ReloadIndicator = GameObject.FindGameObjectWithTag("Reload");
        NoPickup = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(CurrentWeapon != null)
            CurrentWeapon.transform.rotation = SpawnPoint.rotation;
        if (CurrentWeapon != null && Keyboard.current.qKey.isPressed)
        {
            if (CurrentWeapon.GetComponent<Shooting>().IsReloading)
                return;
            DropWeapon();
        }

        if (NoPickup && CurrentWeapon != null)
            DropWeapon();

        if (CurrentWeapon != null)
        {
            Vector2 MousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 LookDir = MousePos - GetComponent<Rigidbody2D>().position;
            float Ang = Mathf.Atan2(LookDir.y, LookDir.x) * Mathf.Rad2Deg - 90f;
            CurrentWeapon.transform.rotation = Quaternion.Euler(0f, 0f, Ang);

            //Debug.Log(Ang);

            Vector3 Scale = Vector3.one;
            if (Ang > -180 && Ang < 0)
            {
                Scale.x = 1f;
                SpawnPoint.transform.position = transform.position - new Vector3(-0.4f, -0.1f);
            }

            else if (Ang < 180 && Ang > 0 || Ang < -180 && Ang > -270)
            {
                Scale.x = -1f;
                SpawnPoint.transform.position = transform.position - new Vector3(0.4f, -0.1f);
            }

            CurrentWeapon.transform.localScale = Scale;
        }
    }

    public void DropWeapon()
    {
        CurrentWeapon.transform.parent = null;
        CurrentWeapon.transform.position = transform.position + new Vector3(0f, 3f, 0f);
        CurrentWeapon.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        CurrentWeapon.GetComponent<Shooting>().IsOnPlayer = false;
        CurrentWeapon = null;
        FindObjectOfType<MouseCursor>().ResetGun();
    }

    public void LoadWeapon(GameObject Next)
    {
        if(CurrentWeapon == null)
        {
            NextWeapon = Next;
            NextWeapon.transform.parent = SpawnPoint;
            NextWeapon.transform.position = SpawnPoint.position;
            GameObject TextHit = Instantiate((GameObject)Resources.Load("GunName", typeof(GameObject)), transform.position, Quaternion.identity);
            TextHit.GetComponentInChildren<TMP_Text>().text = NextWeapon.name;
            CurrentWeapon = NextWeapon.GetComponent<Shooting>();
        }

    }
}
