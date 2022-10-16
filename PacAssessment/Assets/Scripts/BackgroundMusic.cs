using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private AudioSource audioSource;
    private static float fadeTime = 0.2f;

    public AudioClip[] clips;
    public static bool playScaredMusic = false;
    public static bool playDeadMusic = false;
    public static bool playNormalMusic = true;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playScaredMusic)
        {
            PlayMusic(MusicClips.Scared);
        }
        else if (playDeadMusic)
        {
            PlayMusic(MusicClips.Dead);
        }
        else if (playNormalMusic)
        {
            PlayMusic(MusicClips.Calm);
        }
    }

    /// <summary>
    /// Set music clip
    /// </summary>
    /// <param name="clip"></param>
    private void PlayMusic(MusicClips clip)
    {
        if (audioSource.clip != clips[(int)clip] && audioSource.isPlaying)
        {
            if (audioSource.volume > 0)
            {
                audioSource.volume -= Time.deltaTime / fadeTime;
            }
            else
            {
                audioSource.Stop();
                audioSource.clip = clips[(int)clip];
                audioSource.Play();
            }
        }

        if (audioSource.clip == clips[(int)clip] && audioSource.isPlaying)
        {
            if (audioSource.volume < 1)
            {
                audioSource.volume += Time.deltaTime / fadeTime;
            }
        }
    }
}
