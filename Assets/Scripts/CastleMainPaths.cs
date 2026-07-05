using UnityEngine;

public class CastleMainPaths : MonoBehaviour
{
    public GameObject defaultEscape;
    public GameObject keyStolenPath;

    void Start()
    {
        if (WorldState.Instance.keyStolen)
        {
            defaultEscape.SetActive(false);
            keyStolenPath.SetActive(true);
        }
        else {
            defaultEscape.SetActive(true);
            keyStolenPath.SetActive(false);
        }
    }

}
