using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeyboardThings : MonoBehaviour
{
    #region Instance
    public static KeyboardThings Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    #endregion

    public List<ItemControl> itemControls;
    bool initialised;

    public void Init()
    {
        initialised = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

        if (!initialised) return;

        //GetCurrentKeyDown();

        for (int i = 0; i < itemControls.Count; i++)
        {
            if (Input.GetKeyDown(itemControls[i].item.key))
            {
                if (!Overseer.Instance.teamDict[itemControls[i].item.team].isAi)
                    itemControls[i].OnClick();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = Time.timeScale == 1 ? 0 : 1;
            bool frozen = Time.timeScale == 0 ? true : false;
            Overseer.Instance.ui.audioMenuButton.SetActive(frozen);
            if (!frozen) Overseer.Instance.ui.HideAudioMenu();
        }
    }

    static readonly KeyCode[] keyCodes = Enum.GetValues(typeof(KeyCode))
                                                        .Cast<KeyCode>()
                                                        //.Where(k => (int)k < (int)KeyCode.Mouse0) // only if we don't want to detect mouse and joystick inputs
                                                        .ToArray();
    static KeyCode? GetCurrentKeyDown()
    {
        if (!Input.anyKey)
        {
            return null;
        }

        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKey(keyCodes[i]))
            {
                Debug.Log($"Detected key: {keyCodes[i]}");
                return keyCodes[i];
            }
        }

        return null;
    }
}
