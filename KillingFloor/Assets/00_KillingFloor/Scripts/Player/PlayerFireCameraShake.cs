using DG.Tweening;
using System;
using UnityEngine;

public class PlayerFireCameraShake : MonoBehaviour
{
    // ��� ī�޶� ��鸲
    [SerializeField] private Transform _camera;
    [SerializeField] private Vector3 _rotationStrength;
    private Vector3 _shakeRotation;

    private PlayerInputs input;

    private void Awake()
    {
        _camera = GetComponent<PlayerMovement>().transform.GetChild(0); // ������ Ÿ�� �������ֱ� : �÷��̾� ������ Ȧ��
        input = GetComponent<PlayerInputs>();
    }
    private static event Action Shake;

    private void Update()
    {
        // ���ػ����̸� ��¦ ��鸮���ϰ�
        if (input.aim)
        {
            _shakeRotation = new Vector3(0.3f, 0.3f, 0);
        }
        else
            _shakeRotation = _rotationStrength;
    }
    public static void Invoke()
    {
        Shake?.Invoke();
    }
    private void OnEnable() => Shake += CameraShake;
    private void OnDisable() => Shake -= CameraShake;

    private void CameraShake()
    {
        _camera.DOComplete();
        _camera.DOShakeRotation(0.3f, _shakeRotation);
    }

}
