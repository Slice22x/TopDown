using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Create Player")]
public class PlayerInfo : ScriptableObject
{
    public int Level;
    public bool HasSeenStory;
    public int Money;
    public List<ItemScriptableObject> Inv = new List<ItemScriptableObject>();
    [Range(-100,100)]
    public int Reputation;

    public bool VillageSide()
    {
        if(Reputation < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float RepModifiyer(float Value, bool IsBenifit)
    {
        //Calculate the RepMod

        float x = Mathf.Abs(Reputation);
        x /= 1000f;

        if (IsBenifit)
        {
            return Value + Value * x;
        }
        else
        {
            return Value - Value * x;
        }
    }
}
