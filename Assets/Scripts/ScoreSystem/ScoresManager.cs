using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
public class ScoresManager : Singleton<ScoresManager>
{
    public static Action<(int, string)[]> UpdatedScoresEvent;
    (int, string)[] scores = new (int, string)[5];
    
    private string token = ""; // ACA VA EL TOKEN DE GIST DE GITHUB DE PAPITO, Acaso creias que te iba a dar el mio?
    [SerializeField] private string gistId = "d8d3ad587a97d875706d671a89883bb7"; // El ID de tu Gist
    private string gistApiUrl = "https://api.github.com/gists/";

    void Start()
    {
        StartCoroutine(ReadScoresCoroutine());
    }

    public void RequestCheckActualScores()
    {
        StartCoroutine(ReadScoresCoroutine());
    }
    // Crear o actualizar el Gist   
    public void AddScore(string name)
    {
        if (name == "") name = "Anónimo";
        else if (name.Length > 10) name = name.Substring(0, 10);
        name = name.Replace(" ", "_").Replace("\n", "_").Replace("\r", "_");

        int score = GameManager.Instance.ActualScore;

        (int, string) scoreTuple = (score, name);
        // Leer los valores nuevamente
        // Esperar a que se actualicen
        StartCoroutine(AddScoreCoroutine(scoreTuple));
    }

    IEnumerator AddScoreCoroutine((int, string) scoreTuple)
    {
        yield return StartCoroutine(ReadScoresCoroutine());
        // Comprobar si el score es mayor que el minimo
        if (scoreTuple.Item1 <= scores[4].Item1)
        {
            Debug.Log("Score is lower than the minimum");
            yield return null;
        }
        else
        {
            //Buscar la posicion del score en el array
            int position = Array.FindIndex(scores, s => s.Item1 < scoreTuple.Item1);
            // Insertar el score en la posicion, desplazando el resto
            for (int i = 4; i > position; i--)
            {
                scores[i] = scores[i - 1];
            }
            scores[position] = scoreTuple;
            string content = string.Join("\n", scores.Select(s => $"{s.Item1} {s.Item2}"));
            // Actualizar el Gist
            StartCoroutine(UpdateGistCoroutine(content));
        }
    }

    private IEnumerator UpdateGistCoroutine(string content)
    {
        // Escapar el contenido para JSON
        string escapedContent = content.Replace("\n", "\\n").Replace("\"", "\\\"");
        string jsonData = "{\"files\": {\"gistfile1.txt\": {\"content\": \"" + escapedContent + "\"}}}";
        
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
        StartCoroutine(ReadScoresCoroutine());
    }

    private IEnumerator ReadScoresCoroutine()
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
                            ParseScores(content);
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

    private void ParseScores(string content)
    {
        // Dividir el contenido en líneas
        string[] lines = content.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue; // Saltar líneas vacías

            // Dividir la línea en puntaje y nombre
            int spaceIndex = line.IndexOf(' ');
            if (spaceIndex == -1)
            {
                Debug.LogWarning($"Formato incorrecto en la línea {i + 1}: {line}");
                continue;
            }

            string scorePart = line.Substring(0, spaceIndex).Trim();
            string namePart = line.Substring(spaceIndex + 1).Trim();

            if (int.TryParse(scorePart, out int score))
            {
                scores[i] = (score, namePart);
            }
            else
            {
                Debug.LogWarning($"No se pudo convertir el puntaje en la línea {i + 1}: {line}");
            }
        }
        UpdatedScoresEvent?.Invoke(scores);
    }

}
