using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class Test : MonoBehaviour
{
    public const float WIDTH_HEIGHT_RATIO = 1.5f;
    public const int FRACTAL_WIDTH_SPAN = 200;

    [SerializeField] private Vector2 _originPoint;
    [SerializeField] private UnityEngine.UI.RawImage _rawImage;
    [SerializeField] private UnityEngine.UI.RawImage _rawImageHexPreview;

    [SerializeField] FractalBrownianMotionParameters _params;
    [SerializeField] private float _resolution;
    // Start is called before the first frame update
    void Start()
    {

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

            HexGrid grid = new HexGrid(HexGridGeometry.GenerateRectangularGrid(150, 100));
            var coords = AxialGeometry.ConvertAxialSetToBoundedCartesian(grid.Grid.Keys.ToList(), _originPoint, _boundPoint, out float size);

            foreach (HexData data in grid.Grid.Values)
            {
                float elevation = FractalBrownianMotion.FBM(coords[data.Coord], _params);
                data.ExtraData.SetElevation(elevation);
            }

            _rawImageHexPreview.texture = TextureUtilities.GetTexture(GetPixelsFromHexGrid(grid, _hPoints * 3, _vPoints * 3), _hPoints * 3, _vPoints * 3);
        }
    }

    private Color[] GetPixelsFromHexGrid(HexGrid grid, int horizontalPixels, int verticalPixels)
    {
        Color[] pixelArray = new Color[horizontalPixels * verticalPixels];

        var coords = AxialGeometry.ConvertAxialSetToBoundedCartesian(grid.Grid.Keys.ToList(), Vector2.zero, new Vector2(horizontalPixels, verticalPixels), out float size);

        foreach (AxialCoordinate axial in coords.Keys)
        {
            if (grid.TryGetHex(axial, out HexData data))
            {
                float elevation = data.ExtraData.Elevation;
                Vector2 pixelCoord = coords[axial];
                if (elevation > 0.5f) pixelArray[horizontalPixels * Mathf.RoundToInt(pixelCoord.y) + Mathf.RoundToInt(pixelCoord.x)] = Color.white;
            }
        }

        return pixelArray;
    }
}
