using System;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.Collections;

public class ISS_Orbit : MonoBehaviour
{
    Dictionary<string, float> payload = new Dictionary<string, float>();

    float timeDiff, xvel, yvel, zvel;

    void Start()
    {
        payload = GetCoordinates();
        transform.position = new Vector3(payload["an"], payload["bn"], payload["zn"]);
        Debug.Log("About to start coroutine");
        StartCoroutine(GetCoordinatesPeriodically());
    }

    void Update()
    {

        timeDiff = payload["tn"] - payload["tn1"];

        float x = (payload["an"] - payload["an1"]);
        float y = (payload["bn"] - payload["bn1"]);
        float z = (payload["zn"] - payload["zn1"]);
        //Debug.Log(timeDiff);

        float[] XandY = ConvertLatLonToXY(payload["an"], payload["bn"]); // Declare XandY as float array
        float fx = XandY[0]; // Use the X coordinate from the result
        float fy = XandY[1]; // Use the Y coordinate from the result

        //Debug.Log(fx + "----" + payload["an"]);

        //xvel = x / timeDiff;
        //yvel = y / timeDiff;
        //zvel = z / timeDiff;

        transform.position = new Vector3(fy, fx, XandY[2]);

        //Debug.Log("X Velocity " + xvel + ", Y Velocity " + yvel + ", Z Velocity" + zvel);
    }

    private IEnumerator GetCoordinatesPeriodically()
    {
        while (true)
        {
            Debug.Log("Running inside COroutine");
            payload = GetCoordinates();
            yield return new WaitForSeconds(1);
        }
    }

    public static float[] ConvertLatLonToXY(float latitude, float longitude)
    {
        latitude *= Mathf.Deg2Rad;
        longitude *= Mathf.Deg2Rad;

        float x = 6371 * Mathf.Cos(latitude) * Mathf.Cos(longitude);
        float y = 6371 * Mathf.Sin(longitude) * Mathf.Cos(latitude);
        float z = 6371 * Mathf.Sin(latitude);


        return new float[] { x, y, z };
    }

    Dictionary<string, float> GetCoordinates()
    {
        Debug.Log("Running");
        string connectionString = "URI=file:" + Application.dataPath + "/ISSData.db";
        Dictionary<string, float> payload = new Dictionary<string, float>();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM iss_data ORDER BY timestamp DESC LIMIT 2"; // Get the last two rows

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        payload.Add("an1", (float)reader["latitude"]);
                        payload.Add("bn1", (float)reader["longitude"]);
                        payload.Add("zn1", (float)reader["altitude"]);
                        payload.Add("tn1", (long)reader["timestamp"]);
                        Debug.Log((long)reader["timestamp"]);

                    }

                    if (reader.Read())
                    {
                        payload.Add("an", (float)reader["latitude"]);
                        payload.Add("bn", (float)reader["longitude"]);
                        payload.Add("zn", (float)reader["altitude"]);
                        payload.Add("tn", (long)reader["timestamp"]);
                    }
                }
            }

            connection.Close();
        }

        return payload;
    }

}