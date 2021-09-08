using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    public LayerMask mask;

    [Header("PARAMETERS")]
    public float damage;
    public float speed;
    public bool isFake;

    [Header("EFFECTS")]
    public GameObject Impact;
    public GameObject Blood;
    void Start()
    {
        Destroy(gameObject, 5f);
    }
    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if (!isFake)
        {
            RaycastHit hit;

            Debug.DrawLine(transform.position, transform.position + transform.forward / 2);
            if (Physics.Linecast(transform.position, transform.position + transform.forward / 2, out hit, mask))
            {
                print(hit.collider.name);
                if (hit.collider.gameObject.tag != "Player")
                {
                    GameObject imp = Instantiate(Impact, gameObject.transform.position, Quaternion.identity);
                }
                else
                {
                    GameObject imp = Instantiate(Blood, gameObject.transform.position, Quaternion.identity);
                    hit.collider.gameObject.GetComponent<CharacterMovement>().stats.RemoveHp(damage);
                }

                Destroy(gameObject);
            }
        }
    }
}
