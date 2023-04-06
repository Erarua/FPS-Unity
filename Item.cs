using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int cost;
    public string name;

    public Item(int cost,string name)
    {
        this.cost = cost;
        this.name = name;
    }
}
