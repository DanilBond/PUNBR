using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

[System.Serializable]
public class RandomObjectsSpawn
{
    public int CountOfRandomObjects;
    public GameObject[] RandomObjects;
    public float endZ;
    public float beginZ;
    public float endX;
    public float beginX;
    public float offsetY;
}
public class MapBuilder : MonoBehaviour
{
    [Header("TEXTURES")]
    public Texture2D Texture;

    [Header("VARIABLES")]
    public float localOffset;

    [Header("VECTORS_AND_QUATERNIONS")]
    public Vector3 worldOffset;

    [Header("LINKS")]
    public BlocksScriptable BlocksScriptable;
    public RandomObjectsSpawn ROS;

    [Header("TEXTURES")]
    public Texture2D[] t1;
    public Texture2D[] t2;
    public Texture2D[] t3;

    [Header("TRANSFORMS")]
    public Transform staticMeshes;
    public Transform shadedMeshes;
    public Transform runtimeMeshes;
    
    
    void Start()
    {
        GenerateMapFrom3Textures(t1[Random.Range(0, t1.Length)], t2[Random.Range(0, t2.Length)], t3[Random.Range(0,t3.Length)]);
        SpawnRandomObjectsOnRandomPosition();
    }

    
    void Update()
    {
        
    }

    public void GenerateLevel()
    {
        for (int x = 0; x < Texture.width; x++)
        {
            for (int y = 0; y < Texture.height; y++)
            {
                Color col = Texture.GetPixel(x, y);
                Vector3 pos = new Vector3((x*localOffset) + worldOffset.x, worldOffset.y, (y*localOffset) + worldOffset.z);
                for (int c = 0; c < BlocksScriptable.Blocks.Length; c++)
                {
                    if(col == BlocksScriptable.Blocks[c].color)
                    {
                        GameObject Object = Instantiate(BlocksScriptable.Blocks[c].obj[Random.Range(0,BlocksScriptable.Blocks[c].obj.Length)], pos, Quaternion.identity);
                    }
                }
            }
        }
    }

   

    Texture2D CreateBorders(Texture2D tex, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            tex.SetPixel(x, 0, Color.black);
            tex.SetPixel(x, height-1, Color.black);
        }
        for (int y = 0; y < height; y++)
        {
            tex.SetPixel(-1, y, Color.black);
            tex.SetPixel(0, y, Color.black);
           // tex.SetPixel(width, y, Color.black);
        }
        return tex;
    }

    void CreateRandomImage(Texture2D tex)
    {

        for (int x = 0; x < tex.width; x++)
        {
            for (int y = 0; y < tex.height; y++)
            {
                int rand = Random.Range(0, 2);
                Color col = new Color();
                if (rand == 0)
                {
                    col = Color.black;
                }
                else
                {
                    col = Color.white;
                }
                tex.SetPixel(0, y, Color.black);

            }
        }
    }


    public void SpawnRandomObjectsOnRandomPosition()
    {
        for (int i = 0; i < ROS.CountOfRandomObjects; i++)
        {
            Vector3 pos = new Vector3(Random.Range(ROS.beginX, ROS.endX), ROS.offsetY, Random.Range(ROS.beginZ, ROS.endZ));
            float Axis = Random.Range(0, 360);
            Quaternion rot = Quaternion.Euler(0,Axis,0);
            GameObject obj = Instantiate(ROS.RandomObjects[Random.Range(0, ROS.RandomObjects.Length)], pos, rot);
            obj.AddComponent<CollisionDestroyer>();
            obj.AddComponent<MeshCollider>().convex = true;
            obj.AddComponent<Rigidbody>();
            obj.GetComponent<Rigidbody>().isKinematic = false;
            obj.GetComponent<Rigidbody>().useGravity = false;
            RigidbodyConstraints rc = RigidbodyConstraints.FreezeAll;
            obj.GetComponent<Rigidbody>().constraints = rc;
            Physics.IgnoreCollision(obj.GetComponent<MeshCollider>(), GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>(),true);
            obj.transform.parent = runtimeMeshes;
           
        }
        Invoke("CombineDelay", 2.5f);
    }

    void CombineDelay()
    {
        CombineMeshes(runtimeMeshes);
    }


    public void GenerateMapFrom3Textures(Texture2D tex1, Texture2D tex2, Texture2D tex3)
    {
       
        for (int x = 0; x < tex1.width; x++)
        {
            for (int y = 0; y < int.Parse((tex1.height / 3).ToString("0")); y++)
            {
                Color col = tex1.GetPixel(x, y);
                 
                Vector3 pos = new Vector3((x * localOffset) + worldOffset.x, worldOffset.y, (y * localOffset) + worldOffset.z);
                for (int c = 0; c < BlocksScriptable.Blocks.Length; c++)
                {
                    if (col == BlocksScriptable.Blocks[c].color)
                    {
                        GameObject Object = Instantiate(BlocksScriptable.Blocks[c].obj[Random.Range(0, BlocksScriptable.Blocks[c].obj.Length)], pos, Quaternion.identity);
                        if (BlocksScriptable.Blocks[c].isStatic)
                        {
                            Object.transform.parent = staticMeshes.transform;
                        }
                        else if (BlocksScriptable.Blocks[c].isShaded)
                        {
                            Object.transform.parent = shadedMeshes.transform;
                        }
                    }
                }
            }
        }


        for (int x = 0; x < tex2.width; x++)
        {
            for (int y = int.Parse((tex2.height / 3).ToString("0")); y < tex2.height; y++)
            {
                Color col = tex2.GetPixel(x, y);
                
                Vector3 pos = new Vector3((x * localOffset) + worldOffset.x, worldOffset.y, (y * localOffset) + worldOffset.z);
                for (int c = 0; c < BlocksScriptable.Blocks.Length; c++)
                {
                    if (col == BlocksScriptable.Blocks[c].color)
                    {
                        GameObject Object = Instantiate(BlocksScriptable.Blocks[c].obj[Random.Range(0, BlocksScriptable.Blocks[c].obj.Length)], pos, Quaternion.identity);
                        if (BlocksScriptable.Blocks[c].isStatic)
                        {
                            Object.transform.parent = staticMeshes.transform;
                        }
                        else if (BlocksScriptable.Blocks[c].isShaded)
                        {
                            Object.transform.parent = shadedMeshes.transform;
                        }
                    }
                }
            }
        }


        for (int x = 0; x < tex3.width; x++)
        {
            for (int y = int.Parse((tex3.height / 3).ToString("0")); y < tex3.height; y++)
            {
                Color col = tex3.GetPixel(x, y);
                
                Vector3 pos = new Vector3((x * localOffset) + worldOffset.x, worldOffset.y, (y * localOffset) + worldOffset.z);
                for (int c = 0; c < BlocksScriptable.Blocks.Length; c++)
                {
                    if (col == BlocksScriptable.Blocks[c].color)
                    {
                        GameObject Object = Instantiate(BlocksScriptable.Blocks[c].obj[Random.Range(0, BlocksScriptable.Blocks[c].obj.Length)], pos, Quaternion.identity);
                        if (BlocksScriptable.Blocks[c].isStatic)
                        {
                            Object.transform.parent = staticMeshes.transform;
                        }
                        else if (BlocksScriptable.Blocks[c].isShaded)
                        {
                            Object.transform.parent = shadedMeshes.transform;
                        }
                    }
                }
            }
        }

        CombineMeshes(staticMeshes);
    }

    void CombineMeshes(Transform Parent)
    {
        MeshCombiner mc1 = Parent.gameObject.AddComponent<MeshCombiner>();
        mc1.CreateMultiMaterialMesh = true;
        mc1.DeactivateCombinedChildren = true;
        mc1.CombineMeshes(false);
        foreach (Transform i in mc1.transform)
        {
            if (i.GetComponent<BoxCollider>())
            {
                GameObject collision = GameObject.CreatePrimitive(PrimitiveType.Cube);
                collision.transform.position = i.transform.position;
                collision.transform.rotation = i.transform.rotation;
                collision.transform.localScale = new Vector3(i.GetComponent<BoxCollider>().size.x, i.GetComponent<BoxCollider>().size.y+1, i.GetComponent<BoxCollider>().size.z);
                collision.GetComponent<MeshRenderer>().enabled = false;
            }
            else if (i.GetComponent<CapsuleCollider>())
            {
                GameObject collision = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                collision.transform.position = i.transform.position;
                collision.transform.rotation = i.transform.rotation;
                collision.transform.localScale = new Vector3(1, 2, 1);
                collision.GetComponent<MeshRenderer>().enabled = false;
            }
            else if (i.GetComponent<SphereCollider>())
            {
                GameObject collision = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                collision.transform.position = i.transform.position;
                collision.transform.rotation = i.transform.rotation;
                collision.GetComponent<MeshRenderer>().enabled = false;

            }
        }
    }
}
