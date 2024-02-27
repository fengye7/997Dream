using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAmulet : MonoBehaviour
{
    private bool IsCollectedFirst = false;
    private bool IsCollectedSecond = false;
    private bool IsCollectedThird = false;
    public void CollectFragment(string name)
    {
        if (name == "FragmentFirst")
            IsCollectedFirst = true;
        else if (name == "FragmentSecond")
            IsCollectedSecond = true;
        else if (name == "FragmentFThird")
            IsCollectedThird = true;
        CheckIfCollectedEnoughFragments();
    }
    private void CheckIfCollectedEnoughFragments()
    {
        if (IsCollectedFirst && IsCollectedSecond & IsCollectedThird) 
        {
            //success
        }
    }
}
