using UnityEngine;

public class CamManager : MonoBehaviour
{
    #region Instance
    public static CamManager Instance { get; private set; }

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
    Vector3 battleViewPos = new Vector3(0, 10, -20);
    Quaternion battleViewRot = Quaternion.Euler(20, 0, 0);
    Vector3 startingPos;
    Quaternion startingRot;

    public GameObject comicRubbleView;
    public GameObject libRubbleView;
    Vector3 comicRubblePos;
    Vector3 libRubblePos;
    Quaternion comicRubbleRot;
    Quaternion libRubbleRot;

    Vector3 losersRubblePos;
    Quaternion losersRubbleRot;

    public GameObject vCam;
    public GameObject mainMenu;
    bool lerpingCamToBattle;
    bool lerpingCamToRubble;
    float T;

    private void Start()
    {
        startingPos = vCam.transform.position;
        startingRot = vCam.transform.rotation;
        comicRubblePos = comicRubbleView.transform.position;
        comicRubbleRot = comicRubbleView.transform.rotation;
        libRubblePos = libRubbleView.transform.position;
        libRubbleRot = libRubbleView.transform.rotation;
    }

    public void MoveToBattleView(GameObject menu)
    {
        mainMenu = menu;
        lerpingCamToBattle = true;
    }

    public void MoveToRubbleView(int team)
    {
        lerpingCamToRubble = true;
        losersRubblePos = team == 0 ? libRubblePos : comicRubblePos;
        losersRubbleRot = team == 0 ? libRubbleRot : comicRubbleRot;
        T = 0;
    }

    void Update()
    {
        if (lerpingCamToBattle)
        {
            T += Time.deltaTime/2;

            if (T >= 1)
            {
                T = 1;
                lerpingCamToBattle = false;
                mainMenu.SetActive(false);
                Overseer.Instance.Init();
            }

            LerpCam(startingPos, battleViewPos, startingRot, battleViewRot);
        }

        if (lerpingCamToRubble)
        {
            T += Time.deltaTime * 0.5f;

            if (T >= 1)
            {
                T = 1;
                lerpingCamToRubble = false;
                Ref.SetUI();
            }

            LerpCam(battleViewPos, losersRubblePos, battleViewRot, losersRubbleRot);
        }
    }

    private void LerpCam(Vector3 from, Vector3 to, Quaternion qFrom, Quaternion qTo)
    {
        vCam.transform.position = Vector3.Lerp(from, to, T.EaseOut());
        vCam.transform.rotation = Quaternion.Lerp(qFrom, qTo, T.EaseOut());
    }
}
