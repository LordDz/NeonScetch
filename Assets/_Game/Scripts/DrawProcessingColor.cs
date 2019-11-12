using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.PostProcessing;
using System.Collections.Generic;

public class DrawProcessingColor : MonoBehaviour
{
    public PostProcessProfile processProfile;
    private LineRenderer lineRenderer;

    public List<Material> ListNeonMaterials;

    private void Start()
    {
        lineRenderer = FindObjectOfType<LineRenderer>();
    }

    public void ChangeColor()
    {
        int index = Random.Range(0, ListNeonMaterials.Count);
        lineRenderer.material = ListNeonMaterials[index];
    }

    public void ChangeColorOld()
    {
        var bloom = processProfile.GetSetting<Bloom>();
        ColorParameter color = new ColorParameter();
        color.value = Color.green;
        bloom.color = color;
    }
}
