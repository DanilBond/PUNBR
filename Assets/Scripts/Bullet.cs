using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    public bool IamBot;
    public float damage;
    public float force;
    public GameObject Impact;
    public GameObject Blood;
    void Start()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * force);
        //FindObjectOfType<ObjectPooling>().SetObject(gameObject, 5f);
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            int rand = Random.Range(0, 100);
            if(rand >= 85)
            {
                collision.gameObject.GetComponent<BotController>().Emoji();
            }
            collision.gameObject.GetComponent<BotController>().HP -= damage;
            GameObject imp = Instantiate(Blood, gameObject.transform.position, Quaternion.identity);
        }
        else
        {
            GameObject imp = Instantiate(Impact, gameObject.transform.position, Quaternion.identity);
        }
        //FindObjectOfType<ObjectPooling>().SetObject(gameObject);
        Destroy(gameObject);
    }
}
