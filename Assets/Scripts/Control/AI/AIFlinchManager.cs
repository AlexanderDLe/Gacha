using UnityEngine;

public class AIFlinchManager : MonoBehaviour
{
    public bool isFlinching = false;

    public void SetIsFlinching(bool value)
    {
        isFlinching = value;
    }
    public void FlinchStart() { }
    public void FlinchEnd()
    {
        SetIsFlinching(false);
    }
}
