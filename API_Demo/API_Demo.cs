using System.Collections;
using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;


public class ISSData
{
    public string name;
    public float latitude;
    public float longitude;
    public float altitude;
    public float velocity;
    public string visibility;
    public float footprint;
    public long timestamp;
}


public class ISSPositionFetcher : MonoBehaviour
{
    private const string ISS_API_URL = "https://api.wheretheiss.at/v1/satellites/25544";

    private async void Start()
    {
        var issData = await FetchISSData();
        if (issData != null)
        {
            Debug.Log($"ISS Current Latitude: {issData.latitude}, Longitude: {issData.longitude}");
        }
    }

    private async Task<ISSData> FetchISSData()
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                var response = await client.GetStringAsync(ISS_API_URL);
                ISSData data = JsonUtility.FromJson<ISSData>(response);
                return data;
            }
            catch (HttpRequestException e)
            {
                Debug.LogError($"Request error: {e.Message}");
                return null;
            }
        }
    }
}
