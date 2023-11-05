using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;
using UnityEditor.PackageManager;

public class Raspery : MonoBehaviour
{
    private string jsonResponse;
    Raspery_Data data;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FetchRasPeriodically());
        
    }
    private IEnumerator FetchRasPeriodically()
    {
        while (true)
        {
            FetchRasperyLocation();
            yield return new WaitForSeconds(2);
        }
    }

    private async void FetchRasperyLocation()
    {
        string url = "http://10.249.12.16:5000/get-gps";

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                jsonResponse = await response.Content.ReadAsStringAsync();
                data = JsonConvert.DeserializeObject<Raspery_Data>(jsonResponse);
            }
        }
        Debug.Log("//////////////////" + data.longitude);           
    }
    public static float[] ConvertLatLonToXY(float latitude, float longitude)
    {
        latitude *= Mathf.Deg2Rad;
        longitude *= Mathf.Deg2Rad;

        float y = 6371 * Mathf.Cos(latitude) * Mathf.Cos(longitude);
        float x = 6371 * Mathf.Sin(longitude) * Mathf.Cos(latitude);
        float z = 6371 * Mathf.Sin(latitude);


        return new float[] { x, y, z };
    }

    // Update is called once per frame
    void Update()
    {
        float[] XandY = ConvertLatLonToXY(data.latitude, data.longitude);
        transform.position = new Vector3(XandY[0], XandY[1], XandY[2]);
    }
}
