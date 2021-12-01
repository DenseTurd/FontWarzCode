using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleSequence : MonoBehaviour
{
    public GameObject canvas;
    public TMP_Text text;

    List<string> titleSentences;
    int currentSentence;
    float scale;
    bool shaken;
    float shakeDur = 0.4f;
    float shakeMag = 1.4f;
    float closeTitleSequenceTimer;
    bool closing;

    float startingDelay = 0.2f;
    bool started;

    float whooshTime = 2.9f;
    const float whooshTimeReduction = 0.13f;
    float whooshTimer;

    bool titleSeqVox;
    float titleSeqVoxDelay = 2.4f;

    bool startedZooms;
    const float zoomingTextTime = 2.4f;
    List<float> zoomTimers;
    int zoomTimersIndex = -1;

    float epicSynthVol = 0;
    private bool restartedSynth;

    void Start()
    {
        titleSentences = new List<string>
        {
            "DenseTurd Productions presents:",
            "Long McTitleson's:",
            "The sequel to STRONG NUMBAZ!:",
            "(DenseTurd's smash hit submission to last year's rainbow game jam)",
            "STRONG NUMBAZ! 2:",
            "FONT WARZ!",
            "This time, it's letters!:",
            "The Liberation",
            "Vs", 
            "Comic Revolt",
            "The civil sans war:",
            "Ultimate edition!"
        };

        zoomTimers = new List<float>();
        foreach (var str in titleSentences)
        {
            zoomTimers.Add(1000);
        }
    }
    void StartItAll()
    {
        StartNextWhoosh();
        started = true;
    }

    void Update()
    {
        Skip();

        StartTiming();

        if (!started) return;

        WhooshTiming();
        ZoomTiming();
        SynthVol();
        CloseTiming();
        VoxTiming();

        if (!startedZooms) return;

        Scale();
        Shake();

        text.transform.localScale = new Vector3(scale, scale, scale);
    }

    void VoxTiming()
    {
        if (!titleSeqVox)
        {
            titleSeqVoxDelay -= Time.deltaTime;
            if (titleSeqVoxDelay <= 0)
            {
                titleSeqVox = true;
                Sound.Guy.TitleSeqVox();
            }
        }
    }

    void Skip()
    {
        if (Input.anyKeyDown)
        {
            Sound.Guy.StopWhoosh();
            Sound.Guy.StopTitleSeqVox();
            canvas.SetActive(false);
        }
    }

    void CloseTiming()
    {
        if (closing)
        {
            //Debug.Log("Closing");
            closeTitleSequenceTimer -= Time.deltaTime;
            if (closeTitleSequenceTimer <= 0)
            {
                //Debug.Log("closed");
                Sound.Guy.StopEpicSynth();
                canvas.SetActive(false);
            }
        }
    }

    void SynthVol()
    {
        epicSynthVol += (1 / whooshTime) * Time.deltaTime * 0.25f;
        Sound.Guy.epicSynth.volume = epicSynthVol.Squared();
    }

    void ZoomTiming()
    {
        for (int i = 0; i < zoomTimers.Count; i++)
        {
            zoomTimers[i] -= Time.deltaTime;
            if (zoomTimers[i] <= 0)
            {
                zoomTimers[i] = 1000;
                SetUpNextZoom();
            }
        }
    }

    void StartTiming()
    {
        startingDelay -= Time.deltaTime;
        if (startingDelay <= 0)
        {
            if (!started)
                StartItAll();
        }
    }

    void WhooshTiming()
    {
        whooshTimer -= Time.deltaTime;
        if (whooshTimer <= 0) StartNextWhoosh();
    }

    void StartNextWhoosh()
    {
        whooshTimer = whooshTime;
        whooshTime -= whooshTimeReduction;

        zoomTimersIndex++;
        if (zoomTimersIndex > zoomTimers.Count - 1) return;

        zoomTimers[zoomTimersIndex] = zoomingTextTime;

        if (zoomTimersIndex < 4)
            Sound.Guy.Whoosh1();
        else if (zoomTimersIndex < 10)
            Sound.Guy.Whoosh2();
        else
            Sound.Guy.Whoosh2();
    }

    void SetUpNextZoom()
    {
        startedZooms = true;

        if (currentSentence > titleSentences.Count -1) return;

        text.text = titleSentences[currentSentence];
        currentSentence++;

        text.transform.localScale = Vector3.zero;
        scale = 0;

        shakeDur += currentSentence < titleSentences.Count -1 ? 0.1f : 0.5f;
        shakeMag += currentSentence < titleSentences.Count -1 ? 0.4f : 3f;

        shaken = false;

        if (currentSentence == titleSentences.Count - 1)
        {
            closeTitleSequenceTimer = shakeDur * 3;
            closing = true;
        }
    }

    void Shake()
    {
        if (!shaken)
            if (scale == 1)
            {
                CamShake.Shake(shakeDur, shakeMag);
                shaken = true;
            }
    }

    void Scale()
    {
        if (scale < 1)
        {
            scale += (Time.deltaTime) * 5;
            restartedSynth = false;
        }

        if (scale >= 1)
        {
            scale = 1;

            if (restartedSynth) return;

            RestartSynth();
        }
    }

    void RestartSynth()
    {
        Sound.Guy.StopEpicSynth();
        Sound.Guy.EpicSynth();
        epicSynthVol = 0f;
        restartedSynth = true;
    }
}
