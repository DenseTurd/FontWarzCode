using UnityEngine;
using System.Collections.Generic;

public class Mixer : MonoBehaviour
{
    [Range(0, 1)] public float earn;
    [Range(0, 1)] public float spawn;
    [Range(0, 1)] public float takeDamage;
    [Range(0, 1)] public float heal;
    [Range(0, 1)] public float ded;
    [Range(0, 1)] public float liberate;
    [Range(0, 1)] public float select;
    [Range(0, 1)] public float music;
    [Range(0, 1)] public float reflect;
    [Range(0, 1)] public float vet;
    [Range(0, 1)] public float ranged;
    [Range(0, 1)] public float melee;
    [Range(0, 1)] public float titleSeqVox;
    [Range(0, 1)] public float whoosh1;
    [Range(0, 1)] public float whoosh2;
    [Range(0, 1)] public float whoosh3;
    [Range(0, 1)] public float epicSynth;
    [Range(0, 1)] public float celebration;
    [Range(0, 1)] public float hQFall;

    public Dictionary<AudioSource, float> channels;
    Sound soundGuy;

    private void Start()
    {
        RefreshChannels();
    }

    void RefreshChannels()
    {
        soundGuy = GetComponent<Sound>();
        channels = new Dictionary<AudioSource, float>
        {
            { soundGuy.earn, earn },
            { soundGuy.spawn, spawn },
            { soundGuy.takeDamage, takeDamage },
            { soundGuy.heal, heal },
            { soundGuy.ded, ded },
            { soundGuy.select, select },
            { soundGuy.music, music },
            { soundGuy.liberate, liberate},
            { soundGuy.reflect, reflect},
            { soundGuy.vet, vet},
            { soundGuy.ranged, ranged},
            { soundGuy.melee, melee},
            { soundGuy.titleSeqVox, titleSeqVox},
            { soundGuy.whoosh1, whoosh1},
            { soundGuy.whoosh2, whoosh2},
            { soundGuy.whoosh3, whoosh3},
            { soundGuy.epicSynth, epicSynth},
            { soundGuy.celebration, celebration},
            { soundGuy.hQFall, hQFall}
        };
    }

    public void OnValidate()
    {
        RefreshChannels();
    }
}
