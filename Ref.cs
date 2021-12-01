using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class Ref : MonoBehaviour
{
    #region Instance
    public static Ref Instance { get; set; }
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
    #endregion

    public GameObject winLoseCanvas;
    int losingTeam;

    public static void ImDefeated(int team) => Instance.InstanceImDefeated(team);
    void InstanceImDefeated(int team)
    {
        losingTeam = team;
        CamManager.Instance.MoveToRubbleView(losingTeam);
        Sound.Guy.Celebration();
        Sound.Guy.HQFall();
    }

    public static void SetUI() => Instance.InstanceSetUi();
    public void InstanceSetUi()
    {
        Overseer.Instance.ui.gameObject.SetActive(false);
        winLoseCanvas.SetActive(true);
        WinLoseCanvas.Instance.Init(losingTeam);
    }

    public void LibSurrender() => Overseer.Instance.teamDict[0]._com.Ded();
    public void ComicSurrender() => Overseer.Instance.teamDict[1]._com.Ded();

    public static void Again() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
