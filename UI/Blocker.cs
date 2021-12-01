using UnityEngine;

public class Blocker : MonoBehaviour
{
    public void OnMouseEnter()
    {
        Mouse.overUi = true;
    }

    public void OnMouseExit()
    {
        Mouse.overUi = false;
    }
}
