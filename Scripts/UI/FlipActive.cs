using UnityEngine;

public class FlipActive : MonoBehaviour
{
    public GameObject obj;

    public void SwapActive()
    {
        obj.SetActive(!obj.activeSelf);
    }
}
