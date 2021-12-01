using TMPro;
using System.Collections.Generic;
using UnityEngine;

public class Guide : MonoBehaviour
{
    public TMP_Text guide;
    int index;

    List<string> guideSections = new List<string>
    {
        "The goal is to destroy Comic Revolt's HQ (the castle on the right).\n\nYou'll need meringues to fund an army",
        "Workers gather meringues\n\nWorkers, troops, and a turret can be spawned by clicking their Icon or pressing the appropriate key",
        "Hover the mouse over Icons or units for info about them\n\nClick units to select them\n\nWhen avaliable, selected units can be liberated; receiving powerful buffs and abilities",
        "Set Humans/Ais to PvP for multiplayer\n\nSpace pauses (great for liberating troops while playing multiplayer)",
        "Have fun!\n\n(there's more to read if you're into it though)",
        "When units get kills they gain vet levels, healing some and getting stat boosts. You can see which stat's get boosted on a unit's tooltip \"Vet style:\"",
        "Units leave meringues on the ground when killed, be careful not to fund your opponents economy!",
        "You only get one turret, the turret is the only unit you can go into debt to purchase",
        "Read the liberation panel for your units and be strategic with your liberations\n\nLiberated units get a rainbow indicator",
        "The AI cheats, moreso at higher difficulties :)"
    };

    void Start()
    {
        guide.text = guideSections[0];
    }

    public void Next() => SwapText(+1);
    public void Prev() => SwapText(-1);

    void SwapText(int i)
    {
        index += i;
        if (index < 0) index = guideSections.Count - 1;
        if (index == guideSections.Count) index = 0;

        guide.text = guideSections[index];
    }
}
