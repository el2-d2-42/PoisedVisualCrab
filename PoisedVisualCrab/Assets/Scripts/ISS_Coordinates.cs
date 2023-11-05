using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using Newtonsoft.Json;
using System.Data;
using Mono.Data.Sqlite;

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
                cmd.Parameters.Add(new SqliteParameter("@timestamp", data.Timestamp));
                cmd.Parameters.Add(new SqliteParameter("@latitude", data.Latitude));
                cmd.Parameters.Add(new SqliteParameter("@longitude", data.Longitude));
                cmd.Parameters.Add(new SqliteParameter("@altitude", data.Altitude));
                cmd.Parameters.Add(new SqliteParameter("@velocity", data.Velocity));

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
