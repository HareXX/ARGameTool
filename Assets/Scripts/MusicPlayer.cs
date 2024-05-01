using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    private List<AudioClip> playlist = new List<AudioClip>();
    private int currentTrackIndex = 0;

    // ��ײ�����Ч
    public AudioClip collisionSound;

    // ������ײ��Ч
    public AudioSource ColisionSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        LoadMusicFiles();
        OutputPlaylist();
    }
    public AudioSource GetAudioSource()
    {
        return audioSource;
    }

    void LoadMusicFiles()
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Music");
        foreach (var clip in clips)
        {
            playlist.Add(clip);
        }
    }

    public void PlaySong()
    {
        if (playlist.Count > 0 && !audioSource.isPlaying)
        { 
            if(ColisionSource.isPlaying)
            {
                ColisionSource.Stop();
            }
            audioSource.clip = playlist[currentTrackIndex];
            audioSource.Play();
            Debug.Log($"Now playing: {audioSource.clip.name}");
            OutputPlaylist();
        }
    }

    public void NextSong()
    {
        if (audioSource.isPlaying && playlist.Count > 0)
        {
            StopSong();
            currentTrackIndex = (currentTrackIndex + 1) % playlist.Count;
            PlaySong();
        }
    }

    public void StopSong()
    {
        if (audioSource.isPlaying || ColisionSource.isPlaying)
        {
            Debug.Log($"Stopping: {audioSource.clip.name}");
            audioSource.Stop();
            OutputPlaylist();
        }
    }

    public void PreviousSong()
    {
        if (playlist.Count > 0)
        {
            if (currentTrackIndex == 0)
            {
                currentTrackIndex = playlist.Count - 1;
            }
            else
            {
                currentTrackIndex--;
            }
            PlaySong();
        }
    }

    // �����ǰ���ֶ���
    private void OutputPlaylist()
    {
        Debug.Log("Current Playlist:");
        for (int i = 0; i < playlist.Count; i++)
        {
            string isCurrent = (i == currentTrackIndex) ? " <-- Playing Now" : "";
            Debug.Log($"{i + 1}: {playlist[i].name}{isCurrent}");
        }
    }

    // �ֲ���ײ�����Ч
    // ������ײ��Ч
    public void PlayCollisionSound()
    {
        if (collisionSound != null)
        {
            ColisionSource.PlayOneShot(collisionSound);
            Debug.Log("Playing collision sound");
        }
        else
        {
            Debug.LogWarning("Collision sound is not assigned.");
        }
    }
}
