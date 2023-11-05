using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using Newtonsoft.Json;
using System.Data;
using Mono.Data.Sqlite;
using System.Threading.Tasks;

public class ISS_Coordinates : MonoBehaviour
{
    public ISS_Data IssData;
    private string jsonResponse;
    private string connectionString;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hello");

        // Set up the SQLite database
        connectionString = "URI=file:" + Application.dataPath + "/ISSData.db";
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS iss_data (
                                    timestamp INTEGER PRIMARY KEY,
                                    latitude REAL,
                                    longitude REAL,
                                    altitude REAL,
                                    velocity REAL)";
                cmd.ExecuteNonQuery();
            }
            connection.Close();
        }
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
    //int i = 1436029892;
    // Update is called once per frame
    private async void FetchISSData()
    {
        //i = i + 10;
        string url = "https://api.wheretheiss.at/v1/satellites/25544";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    jsonResponse = await response.Content.ReadAsStringAsync();
                    IssData = JsonConvert.DeserializeObject<ISS_Data>(jsonResponse);
                    Debug.Log($"Latitude(alpha): {IssData.latitude}");
                    Debug.Log($"Longitude(beta): {IssData.longitude}");
                    Debug.Log($"Altitude(z): {IssData.altitude}");
                    Debug.Log($"Velocity(v): {IssData.velocity}");
                    Debug.Log($"Timestamp(t): {IssData.timestamp}");

                    InsertDataIntoDatabase(IssData);
                }
                else
                {
                    Debug.Log($"Error: {response.StatusCode}");
                }
            }
    }
    private void InsertDataIntoDatabase(ISS_Data data)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO iss_data (timestamp, latitude, longitude, altitude, velocity) VALUES (@timestamp, @latitude, @longitude, @altitude, @velocity)";

                // Add parameters to prevent SQL injection
                cmd.Parameters.Add(new SqliteParameter("@timestamp", data.timestamp));
                cmd.Parameters.Add(new SqliteParameter("@latitude", data.latitude));
                cmd.Parameters.Add(new SqliteParameter("@longitude", data.longitude));
                cmd.Parameters.Add(new SqliteParameter("@altitude", data.altitude));
                cmd.Parameters.Add(new SqliteParameter("@velocity", data.velocity));

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Debug.LogError("Error inserting data into SQLite database: " + ex.Message);
                }
            }
            connection.Close();
        }
    }
}
