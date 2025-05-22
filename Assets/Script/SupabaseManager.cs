using System;
using UnityEngine;
using Supabase;

public class SupabaseManager : MonoBehaviour
{
    public static SupabaseManager Instance;

    private Client supabaseClient;

    [Header("Supabase Settings")]
    public string url = "https://ftckktypraexkhqkecxq.supabase.co";
    public string anonKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImZ0Y2trdHlwcmFleGtocWtlY3hxIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDcwODY2NDAsImV4cCI6MjA2MjY2MjY0MH0.K7yOKp-ieqlAzrPoUr9dpeW0NAJ6WkrshFJ6K5EEna0";

    private async void Awake()
    {
        // Si une instance existe déjà, on détruit ce nouvel objet pour ne pas avoir deux connexions actives
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // Sinon, on définit cette instance comme la référence unique et on la conserve même entre les scènes
        Instance = this;
        DontDestroyOnLoad(gameObject);

        var options = new Supabase.SupabaseOptions
        {
            AutoConnectRealtime = true
        };

        
        await Client.InitializeAsync(url, anonKey, options);
        supabaseClient = Client.Instance;

        Debug.Log("Connexion Supabase réussie !");
    }

    public Client GetClient()
    {
        return supabaseClient;
    }
  
}
