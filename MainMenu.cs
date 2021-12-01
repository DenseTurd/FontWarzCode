using UnityEngine.UI;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public RectTransform guideCanvas;
    Vector3 guideCanvasClosedPos;
    Quaternion guideCanvasClosedRot;
    Vector3 guideCanvasOpenPos = Vector3.zero;
    Quaternion guideCanvasOpenRot = Quaternion.identity;

    float T;
    bool openingGuide;

    private void Start()
    {
        guideCanvasClosedPos = guideCanvas.transform.localPosition;
        guideCanvasClosedRot = guideCanvas.transform.localRotation;
    }
    public void StartRound()
    {
        CamManager.Instance.MoveToBattleView(gameObject);
        Sound.Guy.Music();
    }

    public void OpenGuide()
    {
        openingGuide = true;
    }

    public void CloseGuide()
    {
        openingGuide = false;
    }

    void Update()
    {
        T += openingGuide ? Time.deltaTime : -Time.deltaTime;
        LerpGuidePanel();
    }

    void LerpGuidePanel()
    {
        if (T < 0) T = 0;
        if (T > 1) T = 1;

        guideCanvas.localPosition = Vector3.Lerp(guideCanvasClosedPos, guideCanvasOpenPos, T.EaseOut());
        guideCanvas.localRotation = Quaternion.Lerp(guideCanvasClosedRot, guideCanvasOpenRot, T.EaseOut());
    }
}
