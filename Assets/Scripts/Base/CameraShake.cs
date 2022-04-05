using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{

    private CinemachineVirtualCamera _camera;

    private void Awake()
    {
        _camera = GetComponent<CinemachineVirtualCamera>();
    }

    public IEnumerator Shake(float duration, float magnitude)
    {

        CinemachineBasicMultiChannelPerlin perlinShake = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlinShake.m_AmplitudeGain = magnitude;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            perlinShake.m_AmplitudeGain = Mathf.Lerp(magnitude, 0f, elapsed / duration);

            yield return null;
        }

        perlinShake.m_AmplitudeGain = 0.0f;

    }

    public void CancelCameraShake()
    {
        if (_camera != null)
        {
            CinemachineBasicMultiChannelPerlin perlinShake = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            perlinShake.m_AmplitudeGain = 0.0f;
        }
    }
}
