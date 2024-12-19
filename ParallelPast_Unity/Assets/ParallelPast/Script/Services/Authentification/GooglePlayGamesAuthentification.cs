/*using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GooglePlayGamesAuthentification : MonoBehaviour
{
    public string Token;
    public string Error;

    public TMPro.TextMeshProUGUI tokenstring;
    public TMPro.TextMeshProUGUI errorstring;

    [SerializeField]
    private Button _manuallySignInButton;

    private void Awake()
    {
        PlayGamesPlatform.Activate();
        _manuallySignInButton.gameObject.SetActive(false);
    }

    async void Start()
    {
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await UnityServices.InitializeAsync();
            await LoginGooglePlayGames();
            await SignInToUnityServices(Token);

            await SetPlayerName();
        }      
    }

    public Task LoginGooglePlayGames()
    {
        var tcs = new TaskCompletionSource<object>();
        PlayGamesPlatform.Instance.Authenticate((success) =>
        {
            if (success == SignInStatus.Success)
            {
                Debug.Log("Login with Google Play games successful.");
                PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
                {
                    Debug.Log("Authorization code: " + code);
                    Token = code;
                    
                    // This token serves as an example to be used for SignInWithGooglePlayGames
                    tcs.SetResult(null);
                });
            }
            else
            {
                Error = "Failed to retrieve Google play games authorization code";
                Debug.Log("Login Unsuccessful");
                tcs.SetException(new Exception("Failed"));

                _manuallySignInButton.gameObject.SetActive(true);
            }
        });
        tokenstring.text = Token;
        errorstring.text = Error;
        return tcs.Task;
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        var tcs = new TaskCompletionSource<object>();
        if (status == SignInStatus.Success)
        {
            Debug.Log("Login with Google Play games successful.");
            PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
            {
                Debug.Log("Authorization code: " + code);
                Token = code;
                // This token serves as an example to be used for SignInWithGooglePlayGames
                tcs.SetResult(null);

            });

        }
        else
        {
            Error = "Failed to retrieve Google play games authorization code";
            Debug.Log("Login Unsuccessful");
            tcs.SetException(new Exception("Failed"));
            _manuallySignInButton.gameObject.SetActive(true);
        }
    }
    async Task SignInToUnityServices(string authCode)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(authCode);
            Debug.Log("Signed in to Unity Services with Google Play");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error signing in to Unity Services: {e.Message}");
        }
    }

    public async void ManualySignIn()
    {
        PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication);
        await SignInToUnityServices(Token);

        await SetPlayerName();
    }

    async Task SetPlayerName()
    {
        string displayName = PlayGamesPlatform.Instance.GetUserDisplayName();
        await AuthenticationService.Instance.UpdatePlayerNameAsync(displayName);
    }

}*/
