using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UI : MonoBehaviour
{
    public ItemControl itemPrefab;
    public List<Item> liberationItems;
    public List<Item> comicItems;

    public Transform browserL;
    public Transform browserR;

    public TMP_Text team0Monies;
    public TMP_Text team1Monies;

    public Button libSurrender;
    public Button comicSurrender;

    public GameObject audioMenuButton;
    public GameObject audioMenu;
    public void Init()
    {
        foreach (var ting in liberationItems)
        {
            ItemControl ic = Instantiate(itemPrefab, browserL);
            ic.Init(ting);
            KeyboardThings.Instance.itemControls.Add(ic);
            if (Overseer.Instance.ais > 1)
                ic.button.interactable = false;
        }

        foreach (var ting in comicItems)
        {
            ItemControl ic = Instantiate(itemPrefab, browserR);
            ic.Init(ting);
            KeyboardThings.Instance.itemControls.Add(ic);
            if (Overseer.Instance.ais > 0)
                ic.button.interactable = false;
        }
        SetMonies();
        SetSurrenderButtons();
    }

    void SetSurrenderButtons()
    {
        if (Overseer.Instance.teamDict[0].isAi) libSurrender.interactable = false;
        if (Overseer.Instance.teamDict[1].isAi) comicSurrender.interactable = false;
    }

    public void SetMonies()
    {
        team0Monies.text = Overseer.Instance.teamDict[0].monies.ToString();
        team1Monies.text = Overseer.Instance.teamDict[1].monies.ToString();
    }

    public void ShowHideAudioMenu()
    {
        if (audioMenu.activeSelf) 
        { 
            HideAudioMenu();
            return;
        }
            
        if (!audioMenu.activeSelf) ShowAudioMenu();
    }

    void ShowAudioMenu()
    {
        audioMenu.SetActive(true);
    }

    public void HideAudioMenu()
    {
        audioMenu.SetActive(false);
    }
}
