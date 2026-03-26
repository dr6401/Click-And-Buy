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
    private int poolIndex = 0;
    [SerializeField] private AudioSource[] sfxAudioSourcePool;

    private float lastPopSfxPlayedTime;
    private float popSfxInterval = 0.02f;
    private float lastMomentumFeastSfxPlayedTime;
    private float momentumFeastSfxInterval = 0.05f;
    
    [Header("Audio Clips")] [SerializeField]
    private AudioClip popSFX;
    [SerializeField] private AudioClip hoverButtonSFX;
    [SerializeField] private AudioClip clickedButtonSFX;
    [SerializeField] private AudioClip upgradeButtonClickSFX;
    [SerializeField] private AudioClip notEnoughMoneyButtonClickSFX;
    [SerializeField] private AudioClip slotMachineGamblingSFX;
    [SerializeField] private AudioClip gravityPulseSFX;

    private void Awake()
    {
        sfxAudioSourcePool = new AudioSource[sfxPoolSize];
        for (int i = 0; i < sfxPoolSize; i++)
        {
            sfxAudioSourcePool[i] = gameObject.AddComponent<AudioSource>();
            sfxAudioSourcePool[i].playOnAwake = false;
            sfxAudioSourcePool[i].loop = false;
            sfxAudioSourcePool[i].outputAudioMixerGroup = sfxMixerGroup;
        }
    }


    /*private void PlayAnimalPoppedSFX(AnimalType.TypeOfAnimal typeOfAnimal)
    {
        if (Time.time - lastPopSfxPlayedTime < popSfxInterval) return;
        lastPopSfxPlayedTime = Time.time;
        if (sfxPoolSize > 0)
        {
            var source = sfxAudioSourcePool[poolIndex];
            source.pitch = Random.Range(0.9f, 1.1f);
            source.PlayOneShot(popSFX);
            poolIndex = (poolIndex + 1) % sfxPoolSize;
        }
        else
        {
            sfxAudioSource.pitch = Random.Range(0.9f, 1.1f);
            sfxAudioSource.PlayOneShot(popSFX);
            //sfxAudioSource.pitch = 1f;
        }
    }*/

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

    public void PlayUpgradeButtonClickSFX()
    {
        sfxAudioSource.pitch = 1f;
        sfxAudioSource.PlayOneShot(upgradeButtonClickSFX);
    }

    public void PlayNotEnoughMoneyForUpgradeSFX()
    {
        sfxAudioSource.pitch = 1f;
        sfxAudioSource.PlayOneShot(notEnoughMoneyButtonClickSFX);
    }
    
    public void PlaySlotMachineGamblingSFX()
    {
        sfxAudioSource.pitch = 1f;
        sfxAudioSource.PlayOneShot(slotMachineGamblingSFX);
    }

    public void PlayGravityPulseSFX()
    {
        sfxAudioSource.pitch = Random.Range(0.9f, 1.1f);
        sfxAudioSource.PlayOneShot(gravityPulseSFX);
    }
    
    private void PlayGravityPulseSFX(Vector3 blackHole, float force, float duration)
    {
        sfxAudioSource.pitch = Random.Range(0.9f, 1.1f);
        sfxAudioSource.PlayOneShot(gravityPulseSFX);
    }

private void OnEnable()
    {
        //GameEvents.OnAnimalPopped += PlayAnimalPoppedSFX;
        GameEvents.OnGravityPulseTriggered += PlayGravityPulseSFX;
        GameEvents.OnStartSpinningUpgradeSlots += PlaySlotMachineGamblingSFX;
    }
    private void OnDisable()
    {
        //GameEvents.OnAnimalPopped -= PlayAnimalPoppedSFX;
        GameEvents.OnGravityPulseTriggered -= PlayGravityPulseSFX;
        GameEvents.OnStartSpinningUpgradeSlots -= PlaySlotMachineGamblingSFX;
    }
}
