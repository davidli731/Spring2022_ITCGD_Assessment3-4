using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private AudioSource audioSource;
    //private static float introDuration = 10.0f;
    private static float fadeTime = 0.2f;

    public AudioClip[] clips;
    public static bool playScaredMusic = false;
    public static bool playNormalMusic = true;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //HandleMusic();

        if (playScaredMusic)
        {
            PlayMusic(MusicClips.Scared);
        }
        else if (playNormalMusic)
        {
            PlayMusic(MusicClips.Calm);
        }
    }

    /// <summary>
    /// Control audioSource volume and decide which clip to play
    /// </summary>
    /*private void HandleMusic()
    {
        if (audioSource.clip == clips[(int)MusicClips.Intro] && audioSource.isPlaying)
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
                    audioSource.clip = clips[(int)MusicClips.Calm];
                    audioSource.Play();
                }
            }
        }

        if (audioSource.clip == clips[(int)MusicClips.Calm] && audioSource.isPlaying)
        {
            if (audioSource.volume < 1)
            {
                audioSource.volume += Time.deltaTime / fadeTime;
            }
        }
    }*/

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
