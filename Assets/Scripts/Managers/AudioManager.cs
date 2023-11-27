
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> dayBackgroundMusics;
    [SerializeField] private List<AudioClip> nightBackgroundMusics;
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip volumeMusic;

    private AudioSource source;

    private AudioMixer mixer;

    void Start()
    {
        source = GetComponent<AudioSource>();
        mixer = Resources.Load("AudioMixer") as AudioMixer;

        print(mixer);
        // Assign Master to Audio Source
        var group = mixer.FindMatchingGroups("Master");
        source.outputAudioMixerGroup = group[0];
    }

    public void PlayNext() //TODO when has gameplay
    {
        Stop();
    }

    private void ChangeTrack(string sceneName)
    {
        Stop();

        switch(sceneName)
        {
            case SceneDirector.SceneNames.MAIN_MENU_SCENE:
                source.clip = mainMenuMusic;
                break;
        }
    }

    public void Play(string sceneName)
    {
        ChangeTrack(sceneName);

        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }

    public void SetSFXVolume(float volume)
    {
        mixer.SetFloat("SFX", Convert(volume));
    }

    public void SetVOVolume(float volume)
    {
        mixer.SetFloat("VO", Convert(volume));
    }

    public void SetMasterVolume(float volume)
    {
        mixer.SetFloat("Master", Convert(volume));
    }

    float Convert(float volume)
    {
        if (volume <= 0) return -80.0f;
        return Mathf.Log10(volume) * 20.0f;
    }

    public void GetVolumes(out float master, out float sfx, out float vos)
    {
        mixer.GetFloat("Master",out master);
        mixer.GetFloat("VO", out vos);
        mixer.GetFloat("SFX", out sfx);
    }
}
