using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenMarker : MonoBehaviour
{
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
        listLinePositions = new List<Vector3>();
        prevPos = transform.localPosition;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
    }

    void Update()
    {
        if (TimePerUpdate > 0 && waitTime > 0)
        {
            waitTime -= Time.deltaTime;
        }
        InputMovement();
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
}
