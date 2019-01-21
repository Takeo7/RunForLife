﻿using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using System.Collections.Generic;

public class PlayFabLogin : MonoBehaviour
{
    #region Singleton
    public static PlayFabLogin instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion

    public Text DebugText;

    public GameObject LogInWindow;

    public void Start()
    {
        //Note: Setting title Id here can be skipped if you have set the value in Editor Extensions already.
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = "C3D0"; // Please change this value to your own titleId from PlayFab Game Manager
        }
        if (!PlayerPrefs.HasKey("FirstTime"))
        {
            LogInWindow.SetActive(true);
        }
        else
        {
            LogInPlayFabDeviceID();
        }
    }

    #region Register User
    public void RegisterUserPlayFab()
    {
        var request = new RegisterPlayFabUserRequest { Email = PlayerPrefs.GetString("Email"), Username = PlayerPrefs.GetString("Username"), Password = PlayerPrefs.GetString("Password"), RequireBothUsernameAndEmail = false };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);
    }
    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("PlayFab - Register Successful");
        DebugText.text = "PlayFab - Register Successful";
        if (PlayerPrefs.GetString("Email") == "")
        {
            LogInPlayFabUsername();
        }
        else if (PlayerPrefs.GetString("Username") == "")
        {
            LogInPlayFabEmail();
        }
        else
        {
            LogInPlayFabUsername();
        }
        LinkPlayFabDeviceID();
    }

    private void OnRegisterFailure(PlayFabError error)
    {
        Debug.LogWarning("PlayFab - Register Failed");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
        DebugText.text = "PlayFab - Register Failed || " + error;
    }
    #endregion

    #region Link DeviceID
    public void LinkPlayFabDeviceID()
    {
        var request = new LinkAndroidDeviceIDRequest { AndroidDeviceId = SystemInfo.deviceUniqueIdentifier };
        PlayFabClientAPI.LinkAndroidDeviceID(request, OnLinkDeviceIDSuccess,OnLinkDeviceIDFailure);
    }

    private void OnLinkDeviceIDSuccess(LinkAndroidDeviceIDResult result)
    {
        Debug.Log("PlayFab - Linked Android ID Successful");
        DebugText.text = "PlayFab - Linked Android ID Successful";
    }

    private void OnLinkDeviceIDFailure(PlayFabError error)
    {
        Debug.LogWarning("PlayFab - LinkAndroidID Failed");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
        DebugText.text = "PlayFab - LinkAndroidID Failed || " + error;
    }
    #endregion

    public void LogInPlayFabOS()
    {
        /*if (Application.platform == RuntimePlatform.Android)
        {
            var request = new LoginWithGoogleAccountRequest { CreateAccount = true, ServerAuthCode = PlayGamesPlatform.Instance.GetServerAuthCode() };
            PlayFabClientAPI.LoginWithGoogleAccount(request, OnLoginSuccess, OnLoginFailure);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            /*var request = new LoginWithGoogleAccountRequest { CreateAccount = true };
            PlayFabClientAPI.LoginWithGoogleAccount(request, OnLoginSuccess, OnLoginFailure);
        }
        else
        {
            var request = new LoginWithCustomIDRequest { CustomId = "TestPlayer", CreateAccount = true };
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
        }*/
        Debug.Log("PlayFab - LogInOS");
    }

    #region LogIn DeviceID
    public void LogInPlayFabDeviceID()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            var request = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = SystemInfo.deviceUniqueIdentifier};
            PlayFabClientAPI.LoginWithAndroidDeviceID(request, OnLogInDeviceIDSuccess, OnLogInDeviceIDFailure);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            var request = new LoginWithIOSDeviceIDRequest { DeviceId = SystemInfo.deviceUniqueIdentifier };
            PlayFabClientAPI.LoginWithIOSDeviceID(request, OnLogInDeviceIDSuccess, OnLogInDeviceIDFailure);
        }
        
    }

    private void OnLogInDeviceIDSuccess(LoginResult result)
    {
        Debug.Log("PlayFab - LogIn Device Successful");
        DebugText.text = "PlayFab - LogIn Device Successful";
    }

    private void OnLogInDeviceIDFailure(PlayFabError error)
    {
        Debug.LogWarning("PlayFab - LogIn Device Failed");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
        DebugText.text = "PlayFab - LogIn Device Failed || " + error;
    }

    #endregion

    #region LogIn Custom
    public void LogInCustomPlayFab()
    {
        var request = new LoginWithCustomIDRequest { CustomId = "TestPlayer"};
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginCustomSuccess, OnLoginCustomFailure);
    }
    private void OnLoginCustomSuccess(LoginResult result)
    {
        Debug.Log("PlayFab - Login Custom Successful");
        DebugText.text = "PlayFab - Login Custom Successful";
    }

    private void OnLoginCustomFailure(PlayFabError error)
    {
        Debug.LogWarning("PlayFab - Login Custom Failed");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
        DebugText.text = "PlayFab - Login Custom Failed || " + error;
    }
    #endregion

    #region LogIn Email
    public void LogInPlayFabEmail()
    {
        var request = new LoginWithEmailAddressRequest { Email = PlayerPrefs.GetString("Email"), Password = PlayerPrefs.GetString("Password") };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginEmailSuccess, OnLoginEmailFailure);
    }
    private void OnLoginEmailSuccess(LoginResult result)
    {
        Debug.Log("PlayFab - Login Email Successful");
        DebugText.text = "PlayFab - Login Email Successful";
    }

    private void OnLoginEmailFailure(PlayFabError error)
    {
        Debug.LogWarning("PlayFab - Login Email Failed");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
        DebugText.text = "PlayFab - Login Email Failed || " + error;
    }
    #endregion

    #region LogIn Username
    public void LogInPlayFabUsername()
    {
        var request = new LoginWithPlayFabRequest { Username = PlayerPrefs.GetString("Username"), Password = PlayerPrefs.GetString("Password") };
        PlayFabClientAPI.LoginWithPlayFab(request, OnLogInUsernameSuccess, OnLoginUsernameFailure);
    }

    private void OnLogInUsernameSuccess(LoginResult result)
    {
        Debug.Log("PlayFab - Login Username Successful");
        DebugText.text = "PlayFab - Login Username Successful";
    }

    private void OnLoginUsernameFailure(PlayFabError error)
    {
        Debug.LogWarning("PlayFab - Login Username Failed");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
        DebugText.text = "PlayFab - Login Username Failed || " + error;
    }
    #endregion

    #region Upload Data
    public void UploadUserData()
    {
        if (PlayFabAuthenticationAPI.IsEntityLoggedIn())
        {
            Debug.Log("PlayFab - UpdateData");
            Dictionary<string, string> data = new Dictionary<string, string>();
            data = CharacterReferences.instance.playerInfo.GetData();
            var request = new UpdateUserDataRequest { Data = data };
            PlayFabClientAPI.UpdateUserData(request, OnUpdateUserDataSuccess, OnUpdateUserDataFailure);
        }
    }

    private void OnUpdateUserDataSuccess(UpdateUserDataResult result)
    {
        Debug.Log("PlayFab - UpdatedDataSuccess");
        DebugText.text = "PlayFab - UpdatedDataSuccess";
    }
    private void OnUpdateUserDataFailure(PlayFabError error)
    {
        Debug.Log("PlayFab - UpdatedDataERROR");
        Debug.Log(error);
        DebugText.text = "PlayFab - UpdatedDataError";
        DebugText.text = ""+error;
    }
    #endregion

    #region Get Data
    public void GetPlayFabData()
    {
        var request = new GetUserDataRequest { };
        PlayFabClientAPI.GetUserData(request, OnGetDataSuccess, OnGetDataFailure);
    }

    private void OnGetDataSuccess(GetUserDataResult result)
    {
        CharacterReferences.instance.playerInfo.SetData(result.Data);
        Debug.Log("PlayFab - GetDataSuccess");
        DebugText.text = "PlayFab - GetDataSuccess";
    }

    private void OnGetDataFailure(PlayFabError error)
    {
        Debug.Log("PlayFab - GetDataERROR");
        Debug.Log(error);
        DebugText.text = "PlayFab - GetDataError";
        DebugText.text = "" + error;
    }
    #endregion

    #region Get Inventory
    public void GetPlayFabInventory()
    {
        var request = new GetUserInventoryRequest { };
        PlayFabClientAPI.GetUserInventory(request, OnGetInventorySuccess, OnGetInventoryFailure);
    }

    private void OnGetInventorySuccess(GetUserInventoryResult result)
    {
        CharacterReferences.instance.playerInfo.SetCurrency(result.VirtualCurrency["Coins"], result.VirtualCurrency["Gems"]);
        //get items
        Debug.Log("PlayFab - GetDataSuccess");
        DebugText.text = "PlayFab - GetDataSuccess";
    }

    private void OnGetInventoryFailure(PlayFabError error)
    {
        Debug.Log("PlayFab - GetDataERROR");
        Debug.Log(error);
        DebugText.text = "PlayFab - GetDataError";
        DebugText.text = "" + error;
    }
    #endregion
}
