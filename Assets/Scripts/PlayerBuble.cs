using System;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PlayerBubble : MonoBehaviour
{
    public float deflateRate;
    public float inflateRateT0;
    public float inflateRateT1;
    public AnimationCurve _curve;
    public float maxSize;
    public float groundLoopDuration;

    public PlayerController player;


    public Vector3 iniScale = Vector3.zero;
    private Vector3 iniPos = Vector3.zero;
    private Vector3 targetScale = Vector3.zero;

    public Material[] materials;
    public MeshRenderer meshRenderer;

    public ParticleSystem[] particles;

    public bool popped = false;

    private int index;

    private PlayerStatusWidget _widget = null;

    public float balloonScale => playingAnim == false ? transform.localScale.x / iniScale.x : 1.0f;

    public void OnValidate()
    {
        if (player == null)
        {
            player = GetComponentInParent<PlayerController>();
        }

        if (meshRenderer == null)
        {
            meshRenderer = GetComponentInChildren<MeshRenderer>();
        }
    }

    public void Awake()
    {
        OnValidate();
        iniPos = transform.localPosition;
        iniScale = transform.localScale;
        targetScale = transform.localScale;
        foreach (var particle in particles)
        {
            particle.Stop();
        }
    }

    private float groundTime = 0.0f;
    private bool playingAnim = false;
    private float animCooldown = 0.0f;

    public void Update()
    {
        playingAnim = false;
        var deltaTime = Time.deltaTime;
        animCooldown -= deltaTime;
        if (player.isGrounded == true && player.blow == false && animCooldown <= 0.0f)
        {
            playingAnim = true;
            popped = false;
            groundTime += deltaTime;
            float t = Mathf.PingPong(groundTime / groundLoopDuration, 1.0f);
            targetScale = Vector3.Lerp(iniScale, iniScale * 2.0f, t);
        }
        else if (popped == false)
        {
            if (player.blow == true)
            {
                AudioManager.Instance.PlayBreathSound(playerIndex);
                animCooldown = 0.25f;
                player.blow = false;
                //AudioManager.Instance.PlayBreathSound();
                playingAnim = false;
                var t = (balloonScale - 1) / (maxSize - 1);
                t = _curve.Evaluate(t);
                var inflateRate = Mathf.Lerp(inflateRateT0, inflateRateT1, t);
                targetScale += inflateRate * Vector3.one * deltaTime;
            }
            else if (targetScale.x > iniScale.x)
            {
                targetScale -= deflateRate * Vector3.one * deltaTime;
            }
        }
        else
        {
            player.blow = false;
        }

        if (popped == false)
        {

            transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, 0.015f * deltaTime);
            transform.localPosition = iniPos + transform.forward * (iniScale.x - transform.localScale.x) * 0.5f;

            if (targetScale.x > maxSize * iniScale.x)
            {
                Pop();
            }
        }

        _widget.inflationBar.value = (balloonScale - 1) / (maxSize - 1);
    }

    public void Pop()
    {
        popped = true;
        targetScale = iniScale;
        transform.localScale = Vector3.zero;
        particles[player.index].Play();
        AudioManager.Instance.PlayPop();
        PlayerEventHandler.Instance.TriggerBubblePop();
    }

    private int playerIndex;
    public void SetMaterial(int index)
    {
        this.index = index;
        playerIndex = index;
        meshRenderer.material = materials[index % materials.Length];

        var obj = GameObject.Find($"PlayerStatus{index}");
        Debug.Assert(obj != null);
        _widget = obj.GetComponent<PlayerStatusWidget>();
        Debug.Assert(_widget != null);
    }
}