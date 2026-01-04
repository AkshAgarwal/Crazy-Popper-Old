using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private AudioClip popClip, gameOverClip, winnerClip, backgroundMusic;
    public AudioSource audioSrc, mainMusicSourceOnLevel;
    private void Start()
    {
        if (instance == null)
            instance = this;
        //PLAY BACKGROUND SOUND CLIP
        backgroundMusic = Resources.Load("Audio/poppersBackgroundMusic") as AudioClip;
        audioSrc = GetComponent<AudioSource>();
        mainMusicSourceOnLevel.clip = backgroundMusic;
        mainMusicSourceOnLevel.Play();
        //LOAD OTHER SOUND CLIP
        popClip = Resources.Load("Audio/pop3") as AudioClip;
        winnerClip = Resources.Load("Audio/applauseShort") as AudioClip;
        gameOverClip = Resources.Load("Audio/awh") as AudioClip;
    }
    private void Awake()
    {
        GameObject[] soundManager = GameObject.FindGameObjectsWithTag("SoundManager");
        if (soundManager.Length > 1)
            Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
    }
    public void Play(string soundClip)
    {
        switch (soundClip)
        {
            case "Pop":
                audioSrc.PlayOneShot(popClip);
                break;
            case "Winner":
                audioSrc.PlayOneShot(winnerClip);
                break;
            case "GameOver":
                audioSrc.PlayOneShot(gameOverClip);
                break;
            default:
                Debug.Log("Invalid SoundClip");
                break;
        }
    }
}
