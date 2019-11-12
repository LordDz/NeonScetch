using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class PenMarker : MonoBehaviour
{
    private SerialPort stream;
    private float moveSpeed = 2f;
    public GameObject BrushToSpawn;

    private bool inputsFrozen = false;

    public LineRenderer lineRenderer;
    //public LineRenderer lineGlow;

    private Vector3 prevPos;
    private List<Vector3> listLinePositions;
    private float lineWidth = 0.1f;


    public float TimePerUpdate = 0f;
    private float waitTime = 0;
    // Update is called once per frame

    void Start()
    {
        stream = new SerialPort("/dev/ttyACM0", 9600);
        stream.ReadTimeout = 50;
        stream.Open();
        listLinePositions = new List<Vector3>();
        prevPos = transform.localPosition;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
    }

    void Update()
    {
        // if (TimePerUpdate > 0 && waitTime > 0)
        // {
        //     waitTime -= Time.deltaTime;
        // }
        // InputMovement();

        // Ping Arduino to send new coordinates only when asked for it
        WriteToArduino(string.Format("{0}\r\n", "PING"));

        // Check if there are any new coordinates in stream from Arduino
        CheckForNewPos();
    }

    void CheckForNewPos()
    {
        StartCoroutine
        (
            AsynchronousReadFromArduino
            ((string s) => ArduinoMovement(s),     // Callback
                () => Debug.LogError("Error!"), // Error callback
                10000f                          // Timeout (milliseconds)
            )
        );
    }
    public void WriteToArduino(string message)
    {
        stream.WriteLine(message);
        stream.BaseStream.Flush();
    }
    private char[] separators = { ',', ';' };
    private String[] posHolder;
    void ArduinoMovement(string s)
    {
        if (inputsFrozen)
            return;

        posHolder = s.Split(separators);
        // Debug.Log("x: " + posHolder[0] + " y: " + posHolder[1]);

        float horizontal = float.Parse(posHolder[0]);
        float vertical = float.Parse(posHolder[1]);

        var pos = transform.localPosition;
        pos = new Vector3(8*horizontal - 4, 6*-vertical + 3);

        if (prevPos != pos && waitTime <= 0)
        {
            waitTime = TimePerUpdate;
            UpdateLine(pos);
        }
        prevPos = pos;
        transform.localPosition = pos;
    }
    void InputMovement()
    {
        if (inputsFrozen)
            return;

        var horizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        var vertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        var pos = transform.localPosition;
        pos = new Vector3(pos.x + horizontal, pos.y + vertical);

        if (prevPos != pos && waitTime <= 0)
        {
            waitTime = TimePerUpdate;
            UpdateLine(pos);
        }
        prevPos = pos;
        transform.localPosition = pos;
    }

    public void FreezeInputs()
    {
        inputsFrozen = true;
    }

    public void Reset()
    {
        listLinePositions.Clear();
        lineRenderer.positionCount = 0;
        inputsFrozen = false;
    }

    private void UpdateLine(Vector3 pos)
    {
        listLinePositions.Add(pos);

        int count = listLinePositions.Count;
        lineRenderer.positionCount = count;
        lineRenderer.SetPosition(count - 1, pos);
    }

    public IEnumerator AsynchronousReadFromArduino(Action<string> callback, Action fail = null, float timeout = float.PositiveInfinity)
    {
        DateTime initialTime = DateTime.Now;
        DateTime nowTime;
        TimeSpan diff = default(TimeSpan);

        string dataString = null;

        do
        {
            try
            {
                dataString = stream.ReadLine();
            }
            catch (TimeoutException)
            {
                dataString = null;
            }

            if (dataString != null)
            {
                callback(dataString);
                yield break; // Terminates the Coroutine
            }
            else
                yield return null; // Wait for next frame

            nowTime = DateTime.Now;
            diff = nowTime - initialTime;

        } while (diff.Milliseconds < timeout);

        if (fail != null)
            fail();
        yield return null;
    }
}
