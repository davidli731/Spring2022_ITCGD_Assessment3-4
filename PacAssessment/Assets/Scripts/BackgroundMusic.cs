using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private AudioSource audioSource;
    private static float introDuration = 10.0f;
    private static float fadeTime = 2.0f;

    public AudioClip[] clips;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMusic();
    }

    /// <summary>
    /// Control audioSource volume and decide which clip to play
    /// </summary>
    private void HandleMusic()
    {
        if (audioSource.clip == clips[0] && audioSource.isPlaying)
        {
            if (Time.time > introDuration)
            {
                if (audioSource.volume > 0)
                {
                    audioSource.volume -= Time.deltaTime / fadeTime;
                }
                else
                {
                    audioSource.Stop();
                    audioSource.clip = clips[1];
                    audioSource.Play();
                }
            }
        }

        if (audioSource.clip == clips[1] && audioSource.isPlaying)
        {
            if (audioSource.volume < 1)
            {
                audioSource.volume += Time.deltaTime / fadeTime;
            }
        }
    }
}
