using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public static class ButtonExtension
{
    public static void AddEventListener<T>(this Button button, T param, Action<T> OnClick)
    {
        button.onClick.AddListener(delegate ()
        {
            OnClick(param);
        });
    }
}

public class ShowData : MonoBehaviour
{
    private Tweener popUpTween;
    public GameObject Loading_text;
    public Dropdown filterDropdown;
    public GameObject clientListPrefab;
    public GameObject popUp_window;
    public Text popUp_Name, popUp_address, popUp_points;

    private APIResponse fetchedData_p;
    private List<Transform> clientListItems = new List<Transform>();

    void Start()
    {
        Loading_text.SetActive(true);
        popUp_window = GameObject.FindGameObjectWithTag("popUp");
        filterDropdown.onValueChanged.AddListener(FilterValueChanged);
        ClientDataManager.Instance.DataFetched += OnDataFetched;
        popUp_Name = popUp_window.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        popUp_points = popUp_window.transform.GetChild(0).GetChild(1).GetComponent<Text>();
        popUp_address = popUp_window.transform.GetChild(0).GetChild(2).GetComponent<Text>();
        popUp_window.SetActive(false);

    }


   


    private void FilterValueChanged(int index)
    {
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        ClearClientList();

        string selectedFilter = filterDropdown.options[filterDropdown.value].text;

        for (int i = 0; i < fetchedData_p.clients.Length; i++)
        {
            Debug.Log("jai");
            Client client = fetchedData_p.clients[i];
            string clientIdKey = client.id.ToString();

            if (selectedFilter == "All" ||
                (selectedFilter == "Managers" && client.isManager) ||
                (selectedFilter == "Non-Managers" && !client.isManager))
            {
                if (fetchedData_p.data.ContainsKey(clientIdKey))
                {
                    ClientData clientData = fetchedData_p.data[clientIdKey];

                    GameObject temp = Instantiate(clientListPrefab, transform);
                    temp.transform.GetChild(0).GetComponent<Text>().text = "Label : " + client.label;
                    temp.transform.GetChild(1).GetComponent<Text>().text = "Point : " + clientData.points.ToString();
                    temp.GetComponent<Button>().AddEventListener(i, ItemClicked);
                    clientListItems.Add(temp.transform);
                }
                else
                {
                    GameObject temp = Instantiate(clientListPrefab, transform);
                    temp.transform.GetChild(0).GetComponent<Text>().text = "Label : " + client.label;
                    Debug.LogWarning("Missing data for client with ID: " + client.id);
                    clientListItems.Add(temp.transform);

                }
            }
        }
        //Destroy(clientListPrefab);
    }

    void ItemClicked(int itemIndex)
    {
        popUp_window.SetActive(true);
        Debug.Log("Item no. : " + itemIndex);
        ClientData clientData = fetchedData_p.data[(itemIndex + 1).ToString()];
        popUp_Name.text = "Name : " + clientData.name;
        popUp_points.text = "Points : " + clientData.points.ToString();
        popUp_address.text = "Address : " + clientData.address;
        AnimatePopUp();

    }

    private void AnimatePopUp()
    {
        if (popUpTween != null && popUpTween.IsActive())
        {
            popUpTween.Complete();
            popUpTween.Kill();
        }

        popUp_window.transform.localScale = Vector3.zero;
        popUpTween = popUp_window.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }

    public void Close_popUpWindow()
    {
        if (popUpTween != null && popUpTween.IsActive())
        {
            popUpTween.Complete();
            popUpTween.Kill();
        }

        popUpTween = popUp_window.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack).OnComplete(() => popUp_window.SetActive(false));
    }

    

    private void ClearClientList()
    {
        foreach (Transform item in clientListItems)
        {
            Destroy(item.gameObject);
            Debug.Log("item destroyed Succesfully");
        }
        clientListItems.Clear();
    }

    public void OnDataFetched(APIResponse fetchedData)
    {
        fetchedData_p = fetchedData;
        ApplyFilter();
        Loading_text.SetActive(false);
    }
}
