using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class CinemachineShake : MonoBehaviour {

    public static CinemachineShake Instance { get; private set; }

    public Volume Vol;

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeTimer;
    private float shakeTimerTotal;
    private float startingIntensity;

    private void Awake() 
    {
        Instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        Vol = GameObject.Find("Global Volume").GetComponent<Volume>();
    }

    public void ShakeCamera(float intensity, float time) 
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = 
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;

        startingIntensity = intensity;
        shakeTimerTotal = time;
        shakeTimer = time;
    }

    public IEnumerator EffectCamera(LensDistortion Component, float Adder, float TargetValue, float Rate, bool Return)
    {
        Vol.profile.TryGet(out Component);
        while (Mathf.Abs(Component.intensity.value) < Mathf.Abs(TargetValue))
        {
            Component.intensity.value += Adder;
            yield return new WaitForSeconds(Rate);
            Debug.Log("Changing");
        }
        if (Return)
        {
            while (Mathf.Abs(Component.intensity.value) >= 0.1f)
            {
                Component.intensity.value -= Adder;
                yield return new WaitForSeconds(Rate);
                Debug.Log("Changing");
            }
            Component.intensity.value = 0f;
        }
        StopCoroutine("EffectCamera");
    }

    private void Update() 
    {
        if (shakeTimer > 0) {
            shakeTimer -= Time.deltaTime;
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 
                Mathf.Lerp(startingIntensity, 0f, 1 - shakeTimer / shakeTimerTotal);
        }
    }

}
