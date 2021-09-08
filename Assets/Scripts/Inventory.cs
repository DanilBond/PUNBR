using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;

[System.Serializable]
public class Ammo
{
    public int rifleAmmo;
    public int pistolAmmo;
    public int sniperAmmo;
    public int rocketAmmo;
}

public class Inventory : MonoBehaviour
{
    public PhotonView photonView;
    public Ammo ammo;

    public Item currentItem;
    public GameObject[] weapons;

    public GameObject weaponInfo;
    public Text currentAmmoTxt;
    public Text maxAmmo;
    public Image currentWeaponImage;
    public ProgressBar progressBar;

    public int currentAmmoCount;

    public float timer;
    void Start()
    {
        if (photonView == null)
            photonView = GetComponent<PhotonView>();
        StartCoroutine(Tick());
    }

    
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (photonView.IsMine)
        {
            if (other.gameObject.tag == "Item")
            {
                timer += Time.deltaTime * 70f;
                progressBar.gameObject.SetActive(true);
                progressBar.currentPercent = timer;
                if (timer >= 100f)
                {
                    AddItem(other.GetComponent<PickableItem>().item);
                    progressBar.currentPercent = 0;
                    progressBar.gameObject.SetActive(false);
                    timer = 0f;
                    other.GetComponent<PickableItem>().DestroyItem();
                }
            }
        }
    }

    

    private void OnTriggerExit(Collider other)
    {
        if (photonView.IsMine)
        {
            if (other.gameObject.tag == "Item")
            {
                progressBar.currentPercent = 0;
                progressBar.gameObject.SetActive(false);
                timer = 0f;
            }
        }
    }
    public void AddItem(Item it)
    {
        if(it.type == Item.Type.weapon)
        {
            currentItem = it;
            currentAmmoCount = it.ammo;
            weaponInfo.SetActive(true);
            currentWeaponImage.sprite = it.sprite;

            for (int i = 0; i < weapons.Length; i++)
            {
                if (i == it.id)
                {
                    weapons[i].SetActive(true);
                }
                else
                {
                    weapons[i].SetActive(false);
                }
            }
        }
        else if(it.type == Item.Type.ammo)
        {
            switch (it.ammoT)
            {
                case Item.Ammo.rifle:
                    ammo.rifleAmmo += it.ammoToAdd;
                    break;
                case Item.Ammo.pistol:
                    ammo.pistolAmmo += it.ammoToAdd;
                    break;
                case Item.Ammo.sniper:
                    ammo.sniperAmmo += it.ammoToAdd;
                    break;
                case Item.Ammo.rocket:
                    ammo.rocketAmmo += it.ammoToAdd;
                    break;
            }
        }
    }

    public IEnumerator Tick()
    {
        while (true)
        {
            yield return new WaitForSeconds(.1f);
            if(currentItem != null)
            {
                currentAmmoTxt.text = currentAmmoCount.ToString();

                switch (currentItem.ammoType)
                {
                    case Item.AmmoType.rifle:
                        maxAmmo.text = ammo.rifleAmmo.ToString();
                        break;
                    case Item.AmmoType.pistol:
                        maxAmmo.text = ammo.pistolAmmo.ToString();
                        break;
                    case Item.AmmoType.sniper:
                        maxAmmo.text = ammo.sniperAmmo.ToString();
                        break;
                    case Item.AmmoType.rocket:
                        maxAmmo.text = ammo.rocketAmmo.ToString();
                        break;
                }
            }
        }
    }
}
