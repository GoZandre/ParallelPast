using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaler : MonoBehaviour
{
    //Time controller

    public void StopTime()
    {
        Time.timeScale = 0;
    }

    public void StartTime()
    {
        Time.timeScale = 1;
    }

    public IEnumerator StopTimeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StopTime();
    }

    public IEnumerator StopTimeNextFrame()
    {
        yield return new WaitForFixedUpdate();
        StopTime();
    }
}
