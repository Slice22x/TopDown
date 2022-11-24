using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EffectUIManager : MonoBehaviour
{
    [System.Serializable]
    public struct Effect
    {
        public Health.EffectType Type;
        public Sprite[] Animation;
        public float DurationLength;
        public Color FillColour;
    }

    public static EffectUIManager Instance;

    public List<Effect> EffectImages;
    public Health.EffectType CurrentEffect;
    public Image Fill;
    public float Intival;
    [SerializeField]int Counter;
    public float Duration;
    // Use this for initialization
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentEffect != Health.EffectType.None)
        {
            var effect = EffectImages.Find(item => item.Type == CurrentEffect);
            Intival -= Time.deltaTime;
            Fill.color = effect.FillColour;
            if (Intival <= 0 && Counter <= effect.Animation.Length)
            {
                Counter++;
                GetComponent<Image>().sprite = effect.Animation[0];
                Intival = effect.DurationLength / Duration;
            }

        }
        else
        {
            GetComponent<Image>().color = Color.clear;
            Fill.color = Color.red;
            Counter = 0;
        }


    }
}
