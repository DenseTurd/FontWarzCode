using UnityEngine;
using UnityEngine.EventSystems;
public class DeSelectCollider : MonoBehaviour
{
    public void OnMouseDown()
    {
        if (Mouse.overUi) return;
        Selector.DeSelect();
    }
}
