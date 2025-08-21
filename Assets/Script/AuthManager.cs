using System;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

public class AuthManager : MonoBehaviour
{
    [Header("Canvases")]
    public GameObject Canvas_Auth;
    public GameObject Canvas_Main;

    [Header("Inputs (Auth)")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    private const string RefreshKey = "sb_refresh_token";
private const string EmailKey   = "sb_email_dev_only";   // alpha seulement
private const string PassKey    = "sb_pass_dev_only";    // alpha seulement

[SerializeField] private bool devAutoLogin = true;       // coche dans l’Inspector pour l’alpha

    // REMPLACE par tes vraies valeurs
    private const string SUPABASE_URL = "https://ftckktypraexkhqkecxq.supabase.co";
    private const string SUPABASE_ANON = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImZ0Y2trdHlwcmFleGtocWtlY3hxIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDcwODY2NDAsImV4cCI6MjA2MjY2MjY0MH0.K7yOKp-ieqlAzrPoUr9dpeW0NAJ6WkrshFJ6K5EEna0";



    // ---------------- Lifecycle ----------------
    private async void Awake()
    {
        // IMPORTANT : tu as déjà Managed Stripping = Low + link.xml
        var opt = new Supabase.SupabaseOptions
        {
            AutoRefreshToken = true,
            PersistSession = true
        };

        // <= Ton SDK : initialisation statique + Instance
        await Supabase.Client.InitializeAsync(SUPABASE_URL, SUPABASE_ANON, opt);

        // Event: quand la session change
        Supabase.Client.Instance.Auth.StateChanged += (_, state) =>
        {
            var s = Supabase.Client.Instance.Auth.CurrentSession;
            Debug.Log("[Auth] StateChanged: " + state + " | session null? " + (s == null));
            if (s != null && !string.IsNullOrEmpty(s.RefreshToken))
            {
                PlayerPrefs.SetString("sb_refresh_token", s.RefreshToken);
                PlayerPrefs.Save();
                Debug.Log("[Auth] Refresh token saved.");
            }
        };
    }

    private async void Start()
{
    Debug.Log("[Auth] Start — CurrentUser? " + (Supabase.Client.Instance.Auth.CurrentUser != null));
    var hasRefresh = PlayerPrefs.HasKey(RefreshKey);
    Debug.Log("[Auth] Has saved refresh? " + hasRefresh);

    // 1) Déjà restauré par le SDK ?
    if (Supabase.Client.Instance.Auth.CurrentUser != null)
    {
        Debug.Log("[Auth] Session restaurée automatiquement.");
        SwitchToMainCanvas();
        return;
    }

    // 2) Tentative via RefreshToken sauvegardé
    if (hasRefresh)
    {
        var rt = PlayerPrefs.GetString(RefreshKey, "");
        if (!string.IsNullOrEmpty(rt))
        {
        

            // Essai n°2 : sans param (si la lib a gardé la session en mémoire)
            try
            {
                Debug.Log("[Auth] Trying RefreshSession() …");
                var s2 = await Supabase.Client.Instance.Auth.RefreshSession();
                if (Supabase.Client.Instance.Auth.CurrentUser != null)
                {
                    Debug.Log("[Auth] Restored via RefreshSession().");
                    SwitchToMainCanvas();
                    return;
                }
            }
            catch (Exception ex2)
            {
                Debug.Log("[Auth] RefreshSession() failed: " + ex2.Message);
            }
        }
    }

    // 3) Sinon → écran login
    Debug.Log("[Auth] No session restored, showing login.");

// Fallback alpha : auto-login avec identifiants sauvegardés
if (devAutoLogin && PlayerPrefs.HasKey(EmailKey) && PlayerPrefs.HasKey(PassKey))
{
    var savedEmail = PlayerPrefs.GetString(EmailKey, "");
    var savedPass  = PlayerPrefs.GetString(PassKey,  "");
    if (!string.IsNullOrEmpty(savedEmail) && !string.IsNullOrEmpty(savedPass))
    {
        try
        {
            Debug.Log("[Auth] Fallback: trying SignIn with saved dev creds…");
            var s = await Supabase.Client.Instance.Auth.SignIn(savedEmail, savedPass);
            if (s != null && !string.IsNullOrEmpty(s.AccessToken))
            {
                Debug.Log("[Auth] Fallback SignIn OK.");
                SwitchToMainCanvas();
                return;
            }
        }
        catch (Exception ex3)
        {
            Debug.Log("[Auth] Fallback SignIn failed: " + ex3.Message);
        }
    }
}

        ShowLogin();
}

    // ---------------- UI handlers ----------------
    public async void OnSignInClicked()
    {
        var email = emailInput ? emailInput.text.Trim() : "";
        var pass = passwordInput ? passwordInput.text : "";

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pass))
        {
            Debug.LogWarning("[Auth] Email/Mot de passe vide.");
            return;
        }

        try
        {
            Debug.Log("[Auth] SignIn…");
            var session = await Supabase.Client.Instance.Auth.SignIn(email, pass);
            if (session != null && !string.IsNullOrEmpty(session.AccessToken))
            {
                if (!string.IsNullOrEmpty(session.RefreshToken))
                {
                    PlayerPrefs.SetString(RefreshKey, session.RefreshToken);
                    PlayerPrefs.Save();
                }
                Debug.Log("[Auth] Login OK.");

                if (!string.IsNullOrEmpty(session.RefreshToken))
{
    PlayerPrefs.SetString(RefreshKey, session.RefreshToken);
    PlayerPrefs.Save();
    Debug.Log("[Auth] Refresh token saved.");
}

                // Alpha only: auto‑login au prochain lancement
                if (devAutoLogin)
                {
                    PlayerPrefs.SetString(EmailKey, email);
                    PlayerPrefs.SetString(PassKey, pass);
                    PlayerPrefs.Save();
                    Debug.Log("[Auth] Dev creds saved (alpha only).");
                }

                SwitchToMainCanvas();
            }
            else
            {
                Debug.LogError("[Auth] SignIn: session nulle.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("[Auth] SignIn FAILED: " + ex);
        }
    }

    public async void OnSignOutClicked()
    {
        try { await Supabase.Client.Instance.Auth.SignOut(); } catch {}
PlayerPrefs.DeleteKey(RefreshKey);

// Alpha only: on retire aussi les identifiants auto-login
PlayerPrefs.DeleteKey(EmailKey);
PlayerPrefs.DeleteKey(PassKey);

PlayerPrefs.Save();
ShowLogin();
    }


    // ---------------- UI helpers ----------------
    private void ShowLogin()
    {
        if (Canvas_Auth) Canvas_Auth.SetActive(true);
        if (Canvas_Main) Canvas_Main.SetActive(false);
    }

    private void SwitchToMainCanvas()
    {
        if (Canvas_Auth) Canvas_Auth.SetActive(false);
        if (Canvas_Main) Canvas_Main.SetActive(true);
    }
    
    public async void SignUp()
    {
        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        var response = await Supabase.Client.Instance.Auth.SignUp(email, password);

        if (response.User != null)
        {
            Debug.Log("✅ Utilisateur créé !");
        }
        else
        {
            Debug.LogError("❌ Erreur lors de la création du compte.");
        }
    }
}