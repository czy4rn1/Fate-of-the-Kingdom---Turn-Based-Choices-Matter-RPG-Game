using System.Collections;
using UnityEngine;

public class DeserterPath : AbstractPath
{
    override public IEnumerator StartPath()
    {
        yield return null;
    }
}
