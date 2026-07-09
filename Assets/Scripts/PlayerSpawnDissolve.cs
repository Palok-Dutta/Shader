using System;
using UnityEngine;
using PrimeTween;
using UnityEngine.VFX;

public class PlayerSpawnDissolve : MonoBehaviour
{
    [SerializeField] private VisualEffect m_VfxGraph;
    [SerializeField] private Renderer m_PlayerRenderer;
    [SerializeField] private float m_Duration;
    [SerializeField] private float m_StartAmount;
    [SerializeField] private float m_EndAmount;
    [SerializeField] private Animator m_PlayerAnimator;
    
    [Header("Spawn Colors")]
    [SerializeField] private Color m_Color1;
    [SerializeField] private Color m_Color2;
    [SerializeField] private Color m_Color3;
    
    [Header("Dissolve Colors")]
    [SerializeField] private Color m_DissolveColor1;
    [SerializeField] private Color m_DissolveColor2;
    [SerializeField] private Color m_DissolveColor3;
    
    private static readonly int _dissolveThreshold = Shader.PropertyToID("_DissolveThreshold");
    private static int _color1 = Shader.PropertyToID("_Color_Start");
    private static int _color2 = Shader.PropertyToID("_Color_Mid");
    private static int _color3 = Shader.PropertyToID("_Color_End");
    private Material[] _mats;

    private void Awake()
    {
        _mats = m_PlayerRenderer.materials;
        m_VfxGraph.Stop();
    }

    private void OnEnable()
    {
        SetColor(m_Color1, m_Color2, m_Color3);
        SetSpawn(m_StartAmount);

        Tween.Custom(m_StartAmount, m_EndAmount, m_Duration, onValueChange: SetSpawn, ease: Ease.OutSine);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_PlayerAnimator.SetTrigger("Run");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (m_VfxGraph != null)
            {
                m_VfxGraph.Play();
            }
            SetColor(m_DissolveColor1, m_DissolveColor2, m_DissolveColor3);
            Tween.Custom(-m_StartAmount, -m_EndAmount, m_Duration, onValueChange: SetSpawn, ease: Ease.OutSine);
        }
    }

    private void SetSpawn(float value)
    {
        foreach (var mat in _mats)
        {
            mat.SetFloat(_dissolveThreshold, value);
        }
        if (m_VfxGraph != null)
        {
            m_VfxGraph.SetFloat("DissolveThreshold", value);
        }
    }

    private void SetColor(Color startColor, Color midColor, Color endColor)
    {
        foreach (var mat in _mats)
        {
            mat.SetColor(_color1, startColor);
            mat.SetColor(_color2, midColor);
            mat.SetColor(_color3, endColor);
        }
    }
}
