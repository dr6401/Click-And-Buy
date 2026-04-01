using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;
    // Pooling
    [SerializeField] private AudioMixerGroup sfxMixerGroup;
    [SerializeField] private int sfxPoolSize = 15;
    //private int poolIndex = 0;
    //[SerializeField] private AudioSource[] sfxAudioSourcePool;

    //private float lastPopSfxPlayedTime;
    //private float popSfxInterval = 0.02f;

    
    [Header("Audio Clips")]
    [SerializeField] private AudioClip hoverButtonSFX;
    [SerializeField] private AudioClip clickedButtonSFX;
    [SerializeField] private AudioClip moneySpentSFX;
    [SerializeField] private AudioClip notEnoughMoneyButtonClickSFX;
    [SerializeField] private AudioClip moneyEarnedSFX;
    [SerializeField] private AudioClip moneyLostSFX;
    
    [SerializeField] private AudioClip victorySFX;
    [SerializeField] private AudioClip defeatSFX;
    
    [SerializeField] private AudioClip wheelSpinSFX;
    [Header("DEBUG")]
    [SerializeField] private bool dontPlayMusic = true;

    private void Awake()
    {
        /*sfxAudioSourcePool = new AudioSource[sfxPoolSize];
        for (int i = 0; i < sfxPoolSize; i++)
        {
            sfxAudioSourcePool[i] = gameObject.AddComponent<AudioSource>();
            sfxAudioSourcePool[i].playOnAwake = false;
            sfxAudioSourcePool[i].loop = false;
            sfxAudioSourcePool[i].outputAudioMixerGroup = sfxMixerGroup;
        }*/

#if UNITY_EDITOR
        if (dontPlayMusic)
        {
            musicAudioSource.volume = 0;
        }
#endif
    }


    public void PlayHoverButtonSFX()
    {
        sfxAudioSource.pitch = 1f;
        sfxAudioSource.PlayOneShot(hoverButtonSFX);
    }

    public void PlayClickedButtonSFX()
    {
        sfxAudioSource.pitch = 1f;
        sfxAudioSource.PlayOneShot(clickedButtonSFX);
    }

    public void PlayMoneySpentSFX()
    {
        sfxAudioSource.pitch = 1f;
        sfxAudioSource.PlayOneShot(moneySpentSFX);
    }

    public void PlayNotEnoughMoneySFX()
    {
        sfxAudioSource.pitch = 1f;
        sfxAudioSource.PlayOneShot(notEnoughMoneyButtonClickSFX);
    }
    
    public void PlayMoneyEarnedSFX()
    {
        sfxAudioSource.pitch = 1f;
        sfxAudioSource.PlayOneShot(moneyEarnedSFX);
    }

    public void PlayMoneyLostSFX()
    {
        sfxAudioSource.pitch = Random.Range(0.9f, 1.1f);
        sfxAudioSource.PlayOneShot(moneyLostSFX);
    }
    
    public void PlayVictorySFX()
    {
        sfxAudioSource.pitch = 1f;
        sfxAudioSource.PlayOneShot(victorySFX);
    }

    public void PLayDefeatSFX()
    {
        sfxAudioSource.pitch = 1f;
        sfxAudioSource.PlayOneShot(defeatSFX);
    }
    
    public void PlayWheelSpinSFX()
    {
        sfxAudioSource.pitch = 1f;
        sfxAudioSource.PlayOneShot(wheelSpinSFX);
    }
    private void OnEnable()
    {
        GameEvents.onMoneySpent += PlayMoneySpentSFX;
        GameEvents.onNotEnoughMoney += PlayNotEnoughMoneySFX;
        GameEvents.onMoneyEarned += PlayMoneyEarnedSFX;
        GameEvents.onMoneyLost += PlayMoneyLostSFX;
        GameEvents.onVictory += PlayVictorySFX;
        GameEvents.onDefeat += PLayDefeatSFX;
        GameEvents.OnUpgradesOffered += PlayWheelSpinSFX;
    }
    private void OnDisable()
    {
        GameEvents.onMoneySpent -= PlayMoneySpentSFX;
        GameEvents.onNotEnoughMoney -= PlayNotEnoughMoneySFX;
        GameEvents.onMoneyEarned -= PlayMoneyEarnedSFX;
        GameEvents.onMoneyLost -= PlayMoneyLostSFX;
        GameEvents.onVictory -= PlayVictorySFX;
        GameEvents.onDefeat -= PLayDefeatSFX;
        GameEvents.OnUpgradesOffered -= PlayWheelSpinSFX;
    }
}
