using System.Collections;
using UnityEngine;
using Cinemachine;

public class CamShake : MonoBehaviour
{
    #region Instance
    public static CamShake Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
    CinemachineBasicMultiChannelPerlin noise;
    public bool shaking;

    float varyIntensity;
    float endTime;
    Coroutine shakingCoroutine;


    // you should make it so subsequent shakes cant ovveride a more intense shake
    void Start()
    {
        noise = FindObjectOfType<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public static void Shake(float duration, float amount)
    {
        if (Instance.shakingCoroutine != null)
        {
            Instance.StopCoroutine(Instance.shakingCoroutine);
        }
        Instance.shaking = false;
        Instance.endTime = Time.time + duration;
        Instance.varyIntensity = 1;
        Instance.shakingCoroutine = Instance.StartCoroutine(Instance.CShake(duration, amount));
    }

    public IEnumerator CShake(float duration, float amount)
    {
        while (Time.time < endTime)
        {
            shaking = true;

            noise.m_AmplitudeGain = amount * varyIntensity;
            noise.m_FrequencyGain = amount * varyIntensity;

            varyIntensity -= Time.deltaTime / duration;

            yield return null;
        }
        if(Time.time >= endTime)
        {
            noise.m_AmplitudeGain = 0;
            noise.m_FrequencyGain = 0;
            shaking = false;
            shakingCoroutine = null;
        }
        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
    }
}
