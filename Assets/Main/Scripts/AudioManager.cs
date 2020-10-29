using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private AudioSource audioSource;
    public AudioClip tapSound;
    public AudioClip glassBreakSound;
    public bool vibrate;
    public bool soundOn;
    public List<AudioClip> coinSounds;
    [Range(0,5)]
    public int coinSoundIndex;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
            Destroy(gameObject);

    }
    public void PlayGlassBreakSound()
    {
        if (soundOn)
            audioSource.PlayOneShot(glassBreakSound);
    }
    public void PlayTapSound()
    {
        if (soundOn)
            audioSource.PlayOneShot(tapSound);
#if !UNITY_EDITOR
        if (vibrate)
            Vibration.Vibrate(25);
#endif
    }
    public void PlayCoinCollectSound()
    {
        if (soundOn)
            audioSource.PlayOneShot(coinSounds[coinSoundIndex]);
#if !UNITY_EDITOR
        if (vibrate)
            Vibration.Vibrate(25);
#endif


    }
    public void PlaySuccessSound()
    {
        if (soundOn)
            audioSource.PlayOneShot(coinSounds[5]);
#if !UNITY_EDITOR
        if (vibrate)
            Vibration.Vibrate(25);
#endif


    }
}
