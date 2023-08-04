using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Client
{
    public bool isManager;
    public int id;
    public string label;
    public string name;  
}

[System.Serializable]
public class ClientData
{
    public string address;
    public string name;
    public int points;
}

[System.Serializable]
public class APIResponse
{
    public Client[] clients;
    public Dictionary<string, ClientData> data = new Dictionary<string, ClientData>();
    public string label;
}






public class ClientDataManager : MonoBehaviour
{

    private static ClientDataManager instance;

    public static ClientDataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ClientDataManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("ClientDataManager");
                    instance = obj.AddComponent<ClientDataManager>();
                }
            }
            return instance;
        }
    }

    public event System.Action<APIResponse> DataFetched;

    private APIResponse apiResponse;

    public void SetData(APIResponse data)
    {
        apiResponse = data;
        DataFetched?.Invoke(apiResponse); 
        //event trigger 

    }
    public APIResponse GetData()
    {
        return apiResponse;
    }
}
