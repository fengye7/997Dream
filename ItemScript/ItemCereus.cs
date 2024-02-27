using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemCereus : MonoBehaviour
{
    private bool Iswater;
    public void FirstDay()
    {
        Iswater = true;
    }
    public void SecondDay()
    {
        if (Iswater)
            AttemptOutcome();
    }

    private void AttemptOutcome()
    {
        float randomValue = Random.Range(0f, 1f);
        if (randomValue < 0.5f)
        {
            //success
        }
        else
        {
            //lose
        }
    }
}
