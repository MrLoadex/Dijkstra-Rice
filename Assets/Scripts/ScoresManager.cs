using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json.Linq;

public class ScoresManager : Singleton<ScoresManager>
{

    (int, string)[] scores = new (int, string)[5];
    
    private string token = ""; // Asegúrate de mantener tu token seguro
    private string gistId = "d8d3ad587a97d875706d671a89883bb7"; // El ID de tu Gist
    private string gistApiUrl = "https://api.github.com/gists/";

    void Start()
    {
        ReadGist();
    }

    // Crear o actualizar el Gist
    public void CreateOrUpdateGist(string content)
    {
        StartCoroutine(UpdateGistCoroutine(content));
    }

    private IEnumerator UpdateGistCoroutine(string content)
    {
        // Crear el JSON con el contenido actualizado
        string jsonData = "{\"files\": {\"gistfile1.txt\": {\"content\": \"" + content + "\"}}}";
        
        // Crear la solicitud para actualizar el Gist usando PATCH
        UnityWebRequest request = new UnityWebRequest(gistApiUrl + gistId, "PATCH");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "token " + token);
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("User-Agent", "Unity");

        // Enviar la solicitud
        yield return request.SendWebRequest();
    }

    // Leer el Gist
    public void ReadGist()
    {
        StartCoroutine(ReadGistCoroutine());
    }

    private IEnumerator ReadGistCoroutine()
    {
        // Realizar una solicitud GET para leer el Gist
        UnityWebRequest request = UnityWebRequest.Get(gistApiUrl + gistId);
        request.SetRequestHeader("Authorization", "token " + token);
        request.SetRequestHeader("User-Agent", "Unity");

        // Esperar la respuesta
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;

            try
            {
                // Parsear el JSON usando Newtonsoft.Json
                JObject gist = JObject.Parse(jsonResponse);

                // Acceder a la propiedad "files"
                var files = gist["files"] as JObject;
                if (files != null && files.HasValues)
                {
                    foreach (var file in files)
                    {
                        string filename = file.Key;
                        string content = file.Value["content"]?.ToString();

                        if (!string.IsNullOrEmpty(content))
                        {
                            Debug.Log($"Contenido del Gist ({filename}):\n{content}");
                        }
                        else
                        {
                            Debug.LogWarning($"El archivo '{filename}' no contiene contenido.");
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("No se encontraron archivos en el Gist.");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error al parsear el JSON: " + ex.Message);
            }
        }
        else
        {
            Debug.LogError("Error al leer el Gist: " + request.error);
            Debug.LogError("Código de Estado HTTP: " + request.responseCode);
            Debug.LogError("Respuesta Completa: " + request.downloadHandler.text);
        }
    }
}
