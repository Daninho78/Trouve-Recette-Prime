using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class SupabaseRPC
{
    private const string SupabaseUrl = "https://ftckktypraexkhqkecxq.supabase.co"; // remplace par ton URL Supabase
    private const string SupabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImZ0Y2trdHlwcmFleGtocWtlY3hxIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDcwODY2NDAsImV4cCI6MjA2MjY2MjY0MH0.K7yOKp-ieqlAzrPoUr9dpeW0NAJ6WkrshFJ6K5EEna0";
 // remplace par ta clé anonyme Supabase

    public static async Task<Guid> InsertBookRPC(string title)
    {
        string functionName = "insert_book_return_id";
        string url = $"{SupabaseUrl}/rest/v1/rpc/{functionName}";

        string jsonData = $"{{\"title\": \"{title}\"}}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("apikey", SupabaseKey);
            request.SetRequestHeader("Authorization", $"Bearer {SupabaseKey}");

            var operation = request.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"❌ Erreur RPC : {request.error}");
                return Guid.Empty;
            }
            else
            {
                try
                {
                    string responseText = request.downloadHandler.text;
string trimmed = responseText.Trim('"'); // Enlève les guillemets du JSON brut
Debug.Log("🔁 ID brut retourné : " + trimmed);

if (Guid.TryParse(trimmed, out Guid parsedId))
{
    Debug.Log("✅ Livre inséré avec ID (RPC) : " + parsedId);
    return parsedId;
}
else
{
    Debug.LogError("❌ Impossible de parser l'ID retourné.");
    return Guid.Empty;
}
                }
                catch (Exception ex)
                {
                    Debug.LogError($"❌ Erreur de parsing : {ex.Message}");
                    return Guid.Empty;
                }
            }
        }
    }

    public static async Task<Guid> InsertRecipeRPC(Guid bookID, string title, int duration)
    {

        string functionName = "insert_recipe_return_id";

        string url = $"{SupabaseUrl}/rest/v1/rpc/{functionName}";

        string jsonData = $"{{\"book_id\":\"{bookID}\",\"title\":\"{title}\",\"duration\":{duration}}}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("apikey", SupabaseKey);
            request.SetRequestHeader("Authorization", $"Bearer {SupabaseKey}");

            var operation = request.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"❌ Erreur RPC recette : {request.error}");
                return Guid.Empty;
            }

            try
            {
                string responseText = request.downloadHandler.text;
                string trimmed = responseText.Trim('"');
                Debug.Log("📦 ID brut retourné (recette) : " + trimmed);

                if (Guid.TryParse(trimmed, out Guid parsedId))
                {
                    Debug.Log("✅ Recette insérée avec ID (RPC) : " + parsedId);
                    return parsedId;
                }
                else
                {
                    Debug.LogError("❌ Impossible de parser l'ID de recette retourné.");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("❌ Erreur de parsing recette : " + ex.Message);
            }
        }

        return Guid.Empty;
    }

    public static async Task<Guid> InsertIngredientRPC(string name)
{
    string functionName = "insert_ingredient_return_id";
    string url = $"{SupabaseUrl}/rest/v1/rpc/{functionName}";

    // Nettoyage pour éviter les caractères invalides
    name = name.Trim().Replace("\"", "").Replace("\\", "");

    string jsonData = $"{{\"ingredient_name\":\"{name}\"}}";
    byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
    Debug.Log("🔍 URL RPC ingrédient : " + url);
    Debug.Log("🔍 Données envoyées RPC ingrédient : " + jsonData);

    using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("apikey", SupabaseKey);
            request.SetRequestHeader("Authorization", $"Bearer {SupabaseKey}");

            var operation = request.SendWebRequest();
            while (!operation.isDone)
                await Task.Yield();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"❌ Erreur RPC ingrédient : {request.error}");
                return Guid.Empty;
            }

            try
            {
                string responseText = request.downloadHandler.text;
                string trimmed = responseText.Trim('"');
                Debug.Log("📦 ID brut retourné (ingrédient) : " + trimmed);

                if (Guid.TryParse(trimmed, out Guid parsedId))
                {
                    Debug.Log("✅ Ingrédient inséré avec ID (RPC) : " + parsedId);
                    return parsedId;
                }
                else
                {
                    Debug.LogError("❌ Impossible de parser l'ID d'ingrédient retourné.");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("❌ Erreur de parsing ingrédient : " + ex.Message);
            }
        }

    return Guid.Empty;
}

public static async Task<bool> InsertRecipeIngredientRPC(Guid recipeId, Guid ingredientId)
{
    string functionName = "insert_recipe_ingredient";

    string url = $"{SupabaseUrl}/rest/v1/rpc/{functionName}";

    string jsonData = $"{{\"recipe_id\":\"{recipeId}\",\"ingredient_id\":\"{ingredientId}\"}}";
    byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

    using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
    {
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("apikey", SupabaseKey);
        request.SetRequestHeader("Authorization", $"Bearer {SupabaseKey}");

        var operation = request.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"❌ Erreur RPC liaison recette-ingrédient : {request.error}");
            return false;
        }

        Debug.Log("✅ Liaison recette-ingrédient insérée avec succès.");
        return true;
    }
}


    [Serializable]
    private class IdResponse
    {
        public string id;
    }
}