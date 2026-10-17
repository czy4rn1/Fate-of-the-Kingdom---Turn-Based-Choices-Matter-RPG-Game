using System.Collections;
using UnityEngine;

public class WaitTimer
{
    public IEnumerator Wait(float seconds)
    {
        float timer = 0f;
        while (timer < seconds)
        {
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
