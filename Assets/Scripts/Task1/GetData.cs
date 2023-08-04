using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class GetData : MonoBehaviour
{
    private const string apiUrl = "https://qa2.sunbasedata.com/sunbase/portal/api/assignment.jsp?cmd=client_data";

    IEnumerator Start()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error fetching data: " + webRequest.error);
            }
            else
            {
                string jsonData = webRequest.downloadHandler.text;

                APIResponse response = JsonConvert.DeserializeObject<APIResponse>(jsonData);

                Debug.Log("Json data: " + jsonData);
                Debug.Log("Clients count: " + response.clients.Length);
                Debug.Log("Data Count: " + response.data.Count);

                ClientDataManager.Instance.SetData(response); // data store
            }
        }
    }
}
