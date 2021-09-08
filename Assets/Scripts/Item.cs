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
        medical,
        ammo
    }
    public enum AmmoType
    {
        rifle,
        pistol,
        sniper,
        rocket
    }
    public enum Ammo
    {
        rifle,
        pistol,
        sniper,
        rocket
    }
    public Type type;
    public int id;
    new public string name;
    public Sprite sprite;
    public GameObject prefab;

    [Header("WEAPON PROPERTIES")]
    public int ammo;
    public AmmoType ammoType;
    public float shootRate;
    public AudioClip shootAudio;
    public float damage;

    [Header("AMMO PROPERTIES")]
    public Ammo ammoT;
    public int ammoToAdd;
}
