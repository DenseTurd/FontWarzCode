using UnityEngine;

public class LiberationBar : MonoBehaviour
{
    public int team;
    void Update()
    {
        transform.localScale = new Vector3(Liberation.Instance.GetCharge(team), 1, 1);    
    }
}
