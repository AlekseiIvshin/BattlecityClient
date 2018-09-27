using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class StartupControl : MonoBehaviour
{
    private string _serverAddress = "";

    public void onTextChanged(string nextText)
    {
        _serverAddress = nextText;
    }

    public void onStartConnect()
    {
        if (_serverAddress.Length ==0)
        {
            ClientState.serverAddress = ClientState.DEFAULT_SERVER_URL;
        } else
        {
            ClientState.serverAddress = _serverAddress;
        }
        SceneManager.LoadScene("_Complete-Game");
    }
}