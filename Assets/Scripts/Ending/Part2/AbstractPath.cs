using System.Collections;
using UnityEngine;

public class AbstractPath : MonoBehaviour
{
    public virtual IEnumerator StartPath()
    {
        yield return null;
    }
}
