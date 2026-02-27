using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    [SerializeField] private float _rainPasses;
    [SerializeField] private float _baseRain;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
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
                elevation = elevation > 0.5 ? (elevation - 0.5f) * 2f : 0f;
                data.ExtraData.SetElevation(elevation);
            }

            TemperatureGen.ComputeTemperatures(grid);

            PrecipitationGen.ComputePrecipitations(grid);

            _rawImageHexPreview.texture = TextureUtilities.GetTexture(TextureUtilities.GetPixelsFromHexGrid(grid, _hPoints * 3, _vPoints * 3), _hPoints * 3, _vPoints * 3);
        }
    }
}
