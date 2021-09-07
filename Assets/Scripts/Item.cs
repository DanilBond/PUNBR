using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item", menuName = "Data/Item", order = 1)]
public class Item : ScriptableObject
{
    public enum Type
    {
        weapon,
        medical
    }
    public Type type;
    new public string name;
    public Sprite sprite;
    public GameObject prefab;
}
