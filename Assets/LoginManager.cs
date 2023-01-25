using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{

    [SerializeField]
    private GameObject LoginMenu;

    [SerializeField]
    private GameObject LoadingMenu;

    [SerializeField]
    private TMP_InputField ipField;

    public void ConnectWithIp()
    {
        NetworkCore.instance.ConnectWithIPInput(ipField.text);
    }

    public void GoToPongSceneButton()
    {
        LoginMenu.SetActive(false);
        LoadingMenu.SetActive(true);
        StartCoroutine(GoToPongScene());
    }

    IEnumerator GoToPongScene()
    {
        bool loadScene = false;

        //Deja connecter au moment de la coroutine
        if (NetworkCore.instance.isConnected)
        {
            loadScene = true;
        }

        //En attente de connexion
        while (NetworkCore.instance.attemptToConnect && !loadScene)
        {
            yield return new WaitForSeconds(0.1f);
            if (NetworkCore.instance.isConnected)
            {
                loadScene = true;
            }
        }

        if (loadScene)
        {
            SceneManager.LoadScene("PongScene");
        }
        else
        {
            LoginMenu.SetActive(true);
            LoadingMenu.SetActive(false);
        }
        yield return null;
    }
}
