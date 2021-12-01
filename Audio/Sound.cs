using System;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    #region Instance
    public static Sound Guy { get; set; }
    private void Awake()
    {
        if (Guy == null)
        {
            Guy = this;
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion

    public float masterVol = 0.7f;
    public Mixer mixer;

    public AudioSource earn;
    public AudioSource spawn;
    public AudioSource takeDamage;
    public AudioSource heal;
    public AudioSource ded;
    public AudioSource liberate;
    public AudioSource reflect;
    public AudioSource vet;
    public AudioSource ranged;
    public AudioSource melee;

    public AudioSource select;
    public AudioSource music;
    public AudioSource titleSeqVox;
    public AudioSource whoosh1;
    public AudioSource whoosh2;
    public AudioSource whoosh3;
    public AudioSource epicSynth;
    public AudioSource celebration;
    public AudioSource hQFall;

    public List<AudioSource> spareSources;

    public void OnEnable() => AudioMenu.OnSomeChange += UpdateAudioSettings;

    public void OnDisable() => AudioMenu.OnSomeChange -= UpdateAudioSettings;

    void UpdateAudioSettings(AudioMenu audioMenu)
    {
        MasterVol(audioMenu.MasterVolSlider.value);
        MusicVol(audioMenu.MusicVolSlider.value);
    }


    public void Start() => mixer = GetComponent<Mixer>();
    public void MasterVol(float vol)
    {
        masterVol = vol;
        UpdateVols();
    }
    public void MusicVol(float value) 
    { 
        mixer.channels[music] = value;
        UpdateVols();
    }

    private void UpdateVols()
    {
        foreach (var kvp in mixer.channels)
        {
            float volume = masterVol.Squared();
            volume *= mixer.channels[kvp.Key].Squared();
            kvp.Key.volume = volume;
        }
    }

    public void Music() => PlaySource(music);

    public void Earn(int team) => PlaySource(earn, team);
    public void Spawn(int team) => PlaySource(spawn, team);
    public void TakeDamage() => PlaySource(takeDamage);
    public void Heal() => PlaySource(heal);
    public void Ded() => PlaySource(ded);
    public void Liberate() => PlaySource(liberate);
    public void Select() => PlaySource(select);
    public void Reflect() => PlaySource(reflect);
    public void Vet() => PlaySource(vet);
    public void Ranged() => PlaySource(ranged);
    public void Melee() => PlaySource(melee);

    public void TitleSeqVox() => PlaySource(titleSeqVox);
    public void StopTitleSeqVox() => titleSeqVox.Stop();
    public void Whoosh1() => PlaySourceLayered(whoosh1);
    public void Whoosh2() => PlaySourceLayered(whoosh2);
    public void Whoosh3() => PlaySourceLayered(whoosh3);
    public void StopWhoosh()
    {
        whoosh1.Stop();
        whoosh2.Stop();
        whoosh3.Stop();

        for (int i = 0; i < spareSources.Count; i++)
        {
            spareSources[i].Stop();
        }
    }

    public void EpicSynth() => PlaySource(epicSynth);
    public void StopEpicSynth() => epicSynth.Stop();
    public void Celebration() => PlaySource(celebration);
    public void HQFall() => PlaySource(hQFall);

    void PlaySourceLayered(AudioSource source)
    {
        if (source.isPlaying)
        {
            for (int i = 0; i < spareSources.Count; i++)
            {
                if (!spareSources[i].isPlaying)
                {
                    spareSources[i].clip = source.clip;
                    PlaySource(spareSources[i], mixer.channels[source]);
                    return;
                }
            }
            Debug.Log("Not enough spare sources");
        }
        else
        {
            PlaySource(source);
        }
    }

    void PlaySource(AudioSource source)
    {
        source.volume = masterVol.Squared();
        source.volume *= mixer.channels[source].Squared();
        source.Play();
    }
    void PlaySource(AudioSource source, float vol)
    {
        source.volume = masterVol.Squared();
        source.volume *= vol.Squared();
        source.Play();
    }
    void PlaySource(AudioSource source, int team)
    {
        source.volume = masterVol.Squared();
        source.volume *= mixer.channels[source].Squared();
        source.panStereo = TeamToPan(team);
        source.Play();
    }

    float TeamToPan(int team) => (team - 0.5f) * 2;    
}
