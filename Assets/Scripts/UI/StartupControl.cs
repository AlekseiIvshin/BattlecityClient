using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class StartupControl : MonoBehaviour
{
    private string _serverAddress = "";
    private long _tickTime=-1;

    public void onTextChanged(string nextText)
    {
        _serverAddress = nextText;
    }

    public void onTickTimeChanged(string nextText)
    {
        _tickTime = long.Parse(nextText);
    }

    public void onStartConnect()
    {
        if (_serverAddress.Length ==0)
        {
            ClientState.serverAddress = ClientState.DEFAULT_SERVER;
        } else
        {
            ClientState.serverAddress = _serverAddress;
        }
        ClientState.tickTime = _tickTime> 0 ? _tickTime : ClientState.DEFAULT_TICK_TIME; 
        SceneManager.LoadScene("_Complete-Game");
    }
}