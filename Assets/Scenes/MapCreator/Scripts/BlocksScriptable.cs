using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Block
{
    public Color color;
    public GameObject[] obj;
    public bool isStatic;
    public bool isShaded;
}
[CreateAssetMenu(fileName = "Blocks", menuName = "MapCreator/BlocksData", order = 1)]
public class BlocksScriptable : ScriptableObject
{
    public Block[] Blocks;
}
