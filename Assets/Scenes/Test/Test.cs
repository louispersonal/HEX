using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Test : MonoBehaviour
{
    public const float WIDTH_HEIGHT_RATIO = 1.5f;
    public const int FRACTAL_WIDTH_SPAN = 200;

    [SerializeField] private Vector2 _originPoint;
    [SerializeField] private UnityEngine.UI.RawImage _rawImage;

    [SerializeField] FractalBrownianMotionParameters _params;
    [SerializeField] private float _resolution;
    // Start is called before the first frame update
    void Start()
    {

        string debug = "Converstion test\n";
        AxialCoordinate axial = new AxialCoordinate(1, -1);
        debug += "axial: " + axial.ToString() + "\n";
        (int row, int col) oddR = AxialGeometry.AxialToOddR(axial);
        debug += "oddR: " + oddR.ToString() + "\n";
        Vector2 cartesian = AxialGeometry.AxialToRelativeCartesian(axial, Vector2.zero, 1);
        debug += "cartesian: " + cartesian.ToString() + "\n";
        Debug.Log(debug);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int _hPoints = Mathf.RoundToInt(FRACTAL_WIDTH_SPAN * _resolution);
            int _vPoints = Mathf.RoundToInt((FRACTAL_WIDTH_SPAN / WIDTH_HEIGHT_RATIO) * _resolution);

            Vector2 _boundPoint = _originPoint + new Vector2(FRACTAL_WIDTH_SPAN, FRACTAL_WIDTH_SPAN / WIDTH_HEIGHT_RATIO);
            float[] points = FractalBrownianMotion.FBMSampleArea(_originPoint, _boundPoint, _hPoints, _vPoints, _params);
            _rawImage.texture = TextureUtilities.GetTexture(TextureUtilities.GetHeightmapPixelArray(points), _hPoints, _vPoints);
        }
    }

}
