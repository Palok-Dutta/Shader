using System;
using UnityEngine;
using PrimeTween;

public class PlayerSpawnDissolve : MonoBehaviour
{
    [SerializeField] private Renderer m_PlayerRenderer;
    [SerializeField] private float m_Duration;
    [SerializeField] private float m_StartAmount;
    [SerializeField] private float m_EndAmount;
    [SerializeField] private Animator m_PlayerAnimator;
    
    private static readonly int _dissolveThreshold = Shader.PropertyToID("_DissolveThreshold");
    private Material[] _mats;

    private void Awake()
    {
        _mats = m_PlayerRenderer.materials;
    }

    private void OnEnable()
    {
        SetSpawn(m_StartAmount);

        Tween.Custom(m_StartAmount, m_EndAmount, m_Duration, onValueChange: SetSpawn, ease: Ease.OutSine);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_PlayerAnimator.SetTrigger("Run");
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            Tween.Custom(-m_StartAmount, -m_EndAmount, m_Duration, onValueChange: SetSpawn, ease: Ease.OutSine);
        }
    }

    private void SetSpawn(float value)
    {
        foreach (var mat in _mats)
        {
            mat.SetFloat(_dissolveThreshold, value);
        }
    }
    
    private void SetDissolve(float value)
    {
        foreach (var mat in _mats)
        {
            mat.SetFloat(_dissolveThreshold, value);
        }
    }
}
