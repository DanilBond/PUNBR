using System.Collections;
using UnityEngine;
using TMPro;
using Photon.Pun;


[System.Serializable]
public class Register
{
    public TMP_InputField emailField;
    public TMP_InputField nameField;
    public TMP_InputField passwordField;
}

[System.Serializable]
public class Login
{
    public TMP_InputField nameField;
    public TMP_InputField passwordField;
}

[System.Serializable]
public class Guest
{
    public TMP_InputField nameField;
}

[System.Serializable]
public class Data
{
    public int id;
    public string email;
    public string name;
    public string password;    
}

public class DataBase : MonoBehaviour
{
   // public ObsceneLanguage OL;
    public Register register;
    public Login login;
    public Guest guest;

    public string dataBaseLink;

    //public TextMeshProUGUI NickName;
    public GameObject AccountPanel;

    private void Start()
    {
        if (PlayerPrefs.HasKey("isLogined"))
        {
            if(PlayerPrefs.GetString("isLogined") == "true")
            {
                AccountPanel.SetActive(false);
                PhotonNetwork.NickName = PlayerPrefs.GetString("nickName");
                GetComponent<NetworkManager>().Init();
            }
        }
    }

    public void RegisterUser()
    {
       // foreach (string i in OL.wordList)
       // {
            //if (register.nameField.text.ToLower() == i)
           // {
           //     Debug.Log("Bad word");
           //     return;
          //  }
       // }
        //foreach (string i in OL.wordList)
        //{
          //  if (register.passwordField.text.ToLower() == i)
          //  {
          //      Debug.Log("Bad word");
                return;
          //  }
       // }
        StartCoroutine(RequestRegister(register.emailField.text, register.nameField.text, register.passwordField.text));
    }
    public void LoginUser()
    {
        //foreach (string i in OL.wordList)
        //{
            //if (login.nameField.text.ToLower() == i)
            //{
            //    Debug.Log("Bad word");
            //    return;
            //}
       // }
       // foreach (string i in OL.wordList)
       // {
            //if (login.passwordField.text.ToLower() == i)
            //{
            //    Debug.Log("Bad word");
            //    return;
            //}
       // }
        StartCoroutine(RequestLogin(login.nameField.text, login.passwordField.text));
    }

    public void GuestUser()
    {
        //foreach (string i in OL.wordList)
        //{
        //    if (guest.nameField.text.ToLower() == i)
        //    {
        //        Debug.Log("Bad word");
        //        return;
        //    }
        //}
        StartCoroutine(RequestGuest("", guest.nameField.text, ""));
    }

    IEnumerator RequestLogin(string name, string password)
    {
        
        if (name != "" && password != "")
        {
            WWWForm form = new WWWForm();
            form.AddField("action", "Login");
            form.AddField("name", name);
            form.AddField("password", password);

            WWW www = new WWW(dataBaseLink, form);
            yield return www;

            if (www.text == "null")
            {
                Debug.Log("User not found");
            }
            var Json = JsonUtility.FromJson<Data>(www.text);
            if (name == Json.name && password == Json.password)
            {
                PhotonNetwork.NickName = Json.name;
                AccountPanel.SetActive(false);
                PlayerPrefs.SetString("isLogined", "true");
                PlayerPrefs.SetString("nickName", Json.name);
                GetComponent<NetworkManager>().Init();
            }
            else
            {
                if (password != Json.password)
                {
                    Debug.Log("Password is incorrect");
                }
            }
            //www.Dispose();
        }
        else
        {
            Debug.Log("Fill all fields");
        }
    }

    IEnumerator RequestRegister(string email, string name, string password)
    {
        if (name != "" && password != "" && email != "")
        {
            WWWForm form = new WWWForm();
            form.AddField("action", "Registration");
            form.AddField("email", email);
            form.AddField("name", name);
            form.AddField("password", password);

            WWW www = new WWW(dataBaseLink, form);
            yield return www;

            Debug.Log(www.text);
            if(www.text == "Registration sucsess")
            {
                PhotonNetwork.NickName = name;
                AccountPanel.SetActive(false);
                PlayerPrefs.SetString("isLogined", "true");
                PlayerPrefs.SetString("nickName", name);
                GetComponent<NetworkManager>().Init();
            }
            //www.Dispose();
        }
        else
        {
            Debug.Log("Fill all fields");
        }
    }

    IEnumerator RequestGuest(string email, string name, string password)
    {
        if (name != "")
        {
            WWWForm form = new WWWForm();
            form.AddField("action", "Registration");
            email = "guest" + Random.Range(0, 1000000) + "@guest.com";
            password = Random.Range(0, 10000000).ToString();
            form.AddField("email", email);
            form.AddField("name", name);
            form.AddField("password", password);

            WWW www = new WWW(dataBaseLink, form);
            yield return www;

            Debug.Log(www.text);
            if (www.text == "Registration sucsess")
            {
                PhotonNetwork.NickName = name;
                AccountPanel.SetActive(false);
                PlayerPrefs.SetString("isLogined", "true");
                PlayerPrefs.SetString("nickName", name);
                GetComponent<NetworkManager>().Init();
            }
            //www.Dispose();
        }
        else
        {
            Debug.Log("Fill all fields");
        }
    }
}
