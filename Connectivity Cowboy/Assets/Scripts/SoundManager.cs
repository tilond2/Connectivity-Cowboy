using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    float timer = 0.5f;
    public float masterVolume = 1f;
    public Sound[] sounds;
    private Dictionary<Sound, float> soundTimerDictionary;
    private void Start()
    {
        soundTimerDictionary = new Dictionary<Sound, float>();
        foreach (Sound s in sounds)
        {
            soundTimerDictionary[s] = -1000f;
            s.setVolume = s.volume;
        }
        
    }
    private void Update()
    {
        timer -= Time.deltaTime;
    }
    private bool CanPlaySound(Sound s)
    {
        if (soundTimerDictionary.ContainsKey(s))
        {
            float lastPlayed = soundTimerDictionary[s];
            if (lastPlayed + s.delay <= Time.time)
                return true;
            else
                return false;
        }
        else
            return false;
    }
    private Sound findSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("no sound");
            return null;
        }
        return s;
    }
    private void GenerateSound(Sound s)
    {
        GameObject obj = new GameObject("Sound");
        obj.tag = "Sound";
        AudioSource audio = obj.AddComponent<AudioSource>();
        audio.clip = s.clip;
        audio.volume = s.volume;
        audio.pitch = s.pitch;
        audio.loop = s.loop;
        audio.mute = s.mute;
        audio.Play();
        soundTimerDictionary[s] = Time.time;
        //s.source.Play();
        if (!audio.loop)
            UnityEngine.Object.Destroy(obj, audio.clip.length);
    }
    // Start is called before the first frame update
    public void PlaySound(string name, bool oneShot)
    {
        Sound s = findSound(name);
        if (s == null) return;
        if (oneShot || CanPlaySound(s))
        {
            GenerateSound(s);
        }
    }
    public void PlaySound(string name, Vector3 position, bool oneShot)
    {
        Sound s = findSound(name);
        if (s == null) return;
        if (oneShot || CanPlaySound(s))
        {
            GameObject obj = new GameObject("Sound");
            obj.tag = "Sound";
            obj.transform.position = new Vector3(position.x, position.y, 0);
            AudioSource audio = obj.AddComponent<AudioSource>();
            audio.transform.position = new Vector3(position.x, position.y, 0);
            audio.clip = s.clip;
            audio.volume = s.volume * masterVolume;
            audio.pitch = s.pitch;
            audio.loop = s.loop;
            audio.mute = s.mute;
            audio.minDistance = 0f;
            audio.maxDistance = 20f;
            audio.spatialBlend = 1f;
            audio.rolloffMode = AudioRolloffMode.Linear;
            audio.dopplerLevel = 0f;
            soundTimerDictionary[s] = Time.time;
            audio.Play();
            //s.source.Play();
            if (!audio.loop)
                UnityEngine.Object.Destroy(obj, audio.clip.length);
        }
    }

    public void Mute()
    {
        foreach (Sound s in sounds)
        {
            if (!s.mute) { s.mute = true; } else { s.mute = false; };
        }
        GameObject[] playingSounds = GameObject.FindGameObjectsWithTag("Sound");
        foreach (GameObject s in playingSounds)
        {
            AudioSource audio = s.GetComponent<AudioSource>();
            if (!audio.mute) { audio.mute = true; } else { audio.mute = false; };
        }
    }
    public void MuteFirst()
    {
        GameObject[] playingSounds = GameObject.FindGameObjectsWithTag("Sound");
        foreach (GameObject s in playingSounds)
        {
            AudioSource audio = playingSounds[0].GetComponent<AudioSource>();
            if (!audio.mute) { audio.mute = true; } else { audio.mute = false; };
        }
    }
    public void Volume(float vol)
    {
        masterVolume = vol;
        GameObject[] playingSounds = GameObject.FindGameObjectsWithTag("Sound");
        foreach (Sound s in sounds)
        {
            s.volume = s.setVolume * masterVolume;
        }
        foreach (GameObject s in playingSounds)
        {
            AudioSource audio = s.GetComponent<AudioSource>();
            audio.volume = 0.25f * masterVolume;
        }
    }
}
