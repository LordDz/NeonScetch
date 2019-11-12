using UnityEngine;
using System.Collections;
using DigitalGlitches;

public class ResetDraw : MonoBehaviour
{
    private CameraGlitch cameraGlitch;
    private PenMarker penMarker;
    private ResetSound resetSound;
    private DrawProcessingColor drawProcessingColor;

    private bool canReset = true;

    public float TimeMinVisible = 1f;
    public float TimeMaxVisibile = 1.5f;


    private float timeShow = 0f;

    // Use this for initialization
    void Start()
    {
        penMarker = FindObjectOfType<PenMarker>();

        cameraGlitch = FindObjectOfType<CameraGlitch>();
        resetSound = FindObjectOfType<ResetSound>();
        drawProcessingColor = FindObjectOfType<DrawProcessingColor>();
        cameraGlitch.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime();
        CheckInputs();
    }

    void UpdateTime()
    {
        if (timeShow > 0f)
        {
            timeShow -= Time.deltaTime;
            if (timeShow <= 0)
            {
                ResetDone();
            }
        }
    }

    private void CheckInputs()
    {
        if (Input.GetButton("Reset") && canReset)
        {
            canReset = false;
            cameraGlitch.enabled = true;
            timeShow = Random.Range(TimeMinVisible, TimeMaxVisibile);
            penMarker.FreezeInputs();
            resetSound.PlaySound();
        }
    }

    private void ResetDone()
    {
        penMarker.Reset();
        cameraGlitch.enabled = false;
        canReset = true;
        drawProcessingColor.ChangeColor();
    }


}
