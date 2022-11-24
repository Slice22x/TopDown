using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallRippleEff : MonoBehaviour
{
    public Material RippleMaterial;
    public float MaxAmount = 50f;

    [Range(0, 1)]
    public float Friction = .9f;

    private float Amount = 0f;

    private void Start()
    {
        Call();
    }

    public void Call()
    {
        this.Amount = this.MaxAmount;
        this.RippleMaterial.SetFloat("_CenterX", 0.5f);
        this.RippleMaterial.SetFloat("_CenterY", 0.5f);
    }
    
    void Update()
    {
        this.RippleMaterial.SetFloat("_Amount", this.Amount);
        this.Amount *= this.Friction;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, this.RippleMaterial);
    }
}
