using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextCounter : MonoBehaviour
{
    public LeanTweenType Ease;
    public LeanTweenType RotEase;

    public bool GunName;

    public static TextCounter Instance;

    int Damage;
    float Timer;

    bool SetAlp;

    [SerializeField] ParticleSystem Explode;

    private void Start()
    {
        Timer = 3f;
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (GunName)
        {
            LeanTween.rotateZ(gameObject,Random.Range(10f, -10f), 0.5f);
            LeanTween.moveY(gameObject, transform.position.y + 1, 0.5f);
            LeanTween.alpha(transform.GetChild(0).gameObject, 0f, 0.5f);
            LeanTween.scale(gameObject, Vector3.zero, 0.5f).setEase(Ease).setOnComplete(DestroyGameObject);
        }
    }

    public void Update()
    {
        if (!GunName)
        {
            GetComponentInChildren<TMP_Text>().text = Damage.ToString();
            Timer -= Time.deltaTime;
            if (Timer <= 0f)
            {
                if (!SetAlp && Timer <= 0f)
                {
                    Explode.Play();
                    LeanTween.alpha(transform.GetChild(0).gameObject, 0f, 0.5f).setOnComplete(DestroyGameObject);
                    SetAlp = true;
                }
            }
        }
    }

    public void Acum(int Dam, bool Crit)
    {
        if (Crit)
        {
            GetComponentInChildren<TMP_Text>().outlineColor = Color.white;
            GetComponentInChildren<TMP_Text>().faceColor = Color.red;
            LeanTween.rotateZ(gameObject, Random.Range(20f, -20f), 0.2f).setEase(RotEase);
        }
        if (!Crit)
        {
            GetComponentInChildren<TMP_Text>().outlineColor = Color.red;
            GetComponentInChildren<TMP_Text>().faceColor = Color.cyan;
            LeanTween.rotateZ(gameObject,0f, 0.5f);
        }
        Damage += Dam;
        LeanTween.moveY(gameObject, transform.position.y + 1, 0.5f);
        LeanTween.scale(gameObject, transform.localScale + new Vector3(0.1f, 0.1f), 0.5f).setEase(Ease);
        Timer = 3;
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
