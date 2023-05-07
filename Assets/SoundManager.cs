using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : PersistentSingleton<SoundManager>
{ 
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;
    // Dictionary to map scene names to music track names
    public Dictionary<string, string> sceneMusicMap = new Dictionary<string, string>()
    {
        {"MainMenu", "Main_Menu"},
        {"GameWorld", "Village_Theme"},
        {"EndingScene", "Ending_Theme"}
        // Add additional mappings for each scene and corresponding music track
    };
    private void Start()
    {
        //PlayMusic("Main_Menu"); // Start from MainMenu

        PlayMusic(sceneMusicMap[SceneManager.GetActiveScene().name]);
        SceneManager.sceneLoaded += OnSceneLoaded; // Register the OnSceneLoaded method to be called when a scene is loaded
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Play the appropriate music for the new scene
        PlayMusic(sceneMusicMap[scene.name]);
    }
    public void PlayMusic(string name)
    {
        Sound sound = Array.Find(musicSounds, x => x.name == name);

        if (sound == null) 
        { 
            Debug.Log("Music Not Found!"); 
        }
        else 
        { 
            musicSource.clip = sound.clip; 
            musicSource.Play(); 
        }
    }
    public void PlaySfx(string name)
    {
        Sound sound = Array.Find(sfxSounds, x => x.name == name);

        if (sound == null)
        {
            Debug.Log("SFX Not Found!");
        }
        else
        {
            sfxSource.PlayOneShot(sound.clip);
        }
    }
    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }
    public void ToggleSfx()
    {
        sfxSource.mute = !sfxSource.mute;
    }
    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    public void SfxVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
