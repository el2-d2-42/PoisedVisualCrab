using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using Newtonsoft.Json;

public class ISS_Coordinates : MonoBehaviour
{
    public ISS_Data IssData;
    private string jsonResponse;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hello");
        StartCoroutine(FetchISSDataPeriodically());
    }

    private IEnumerator FetchISSDataPeriodically()
    {
        while (true)
        {
            FetchISSData();
            yield return new WaitForSeconds(1);
        }
    }

    // Update is called once per frame
    private async void FetchISSData()
    {
        string url = "https://api.wheretheiss.at/v1/satellites/25544";
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                jsonResponse = await response.Content.ReadAsStringAsync();
                IssData = JsonConvert.DeserializeObject<ISS_Data>(jsonResponse);
                Debug.Log($"Payload: \n" +
                    $"Latitude(alpha): {IssData.Latitude}\n" +
                    $"Longitude(beta): {IssData.Longitude}\n" +
                    $"Altitude(z): {IssData.Altitude}\n" +
                    $"Velocity(v): {IssData.Velocity}\n" +
                    $"Timestamp(t): {IssData.Timestamp}\n");
            }
            else
            {
                Debug.Log($"Error: {response.StatusCode}");
            }
        }
    }
}
