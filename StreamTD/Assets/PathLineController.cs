using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PathLineController : MonoBehaviour
{
    private LineRenderer _rend;

    private Color[] _colors = new[]
    {
        Color.blue,
        Color.cyan,
        Color.green,
        Color.magenta,
        Color.red,
        Color.yellow
    };

    private int _currentColorIndex;
    void Start()
    {
        _rend = GetComponent<LineRenderer>();
        StartCoroutine(ChangeColorCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        var currentOffset = _rend.material.GetTextureOffset("_MainTex");
        _rend.material.SetTextureOffset("_MainTex", currentOffset + Vector2.left * Time.deltaTime);
    }

    IEnumerator ChangeColorCoroutine()
    {
        while (true)
        {
            _currentColorIndex = (_currentColorIndex + 1) % _colors.Length;
            var currentColor = _rend.material.GetColor("_EmissionColor");
            for (int i = 0; i < 1000; i++)
            {
                _rend.material.SetColor("_EmissionColor", Color.Lerp(currentColor, _colors[_currentColorIndex], i*0.001f));
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
