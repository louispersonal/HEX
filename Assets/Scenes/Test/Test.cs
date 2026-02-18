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

            ComputeTemperatures(grid);

            foreach (HexData data in grid.Grid.Values)
            {
                float precipitation = (10f - NumHexesFromSea(data, grid, out HexData seaHex)) / 10f;
                data.ExtraData.SetPrecipitation(precipitation);
            }

            _rawImageHexPreview.texture = TextureUtilities.GetTexture(TextureUtilities.GetPixelsFromHexGrid(grid, _hPoints * 3, _vPoints * 3), _hPoints * 3, _vPoints * 3);
        }
    }

    public int NumHexesFromSea(HexData data, HexGrid grid, out HexData seaHex)
    {
        seaHex = null;
        if (data.ExtraData.IsSea) return 0;
        int maxNumber = 8;
        for (int n = 1; n < maxNumber; n++)
        {
            List<HexData> hexes = HexGridGeometry.HexesInRingOfRadiusOfHex(grid, data, n);
            foreach (HexData hex in hexes)
            {
                if (hex.ExtraData.IsSea)
                {
                    seaHex = hex;
                    return n;
                }
            }
        }
        return maxNumber;
    }

    public void ComputeTemperatures(HexGrid grid)
    {
        Dictionary<AxialCoordinate, float> baseTemps = new Dictionary<AxialCoordinate, float>();
        Dictionary<AxialCoordinate, float> windAdjustedTemps = new Dictionary<AxialCoordinate, float>();
        foreach (HexData data in grid.Grid.Values)
        {
            float latitude = grid.GetLatitude(data.Coord);
            baseTemps[data.Coord] = ComputeBaseTemperature(latitude, data.ExtraData.Elevation, data.Coord);
        }

        foreach (HexData data in grid.Grid.Values)
        {
            float coastalDistance = NumHexesFromSea(data, grid, out HexData seaHex);
            if (coastalDistance > 0 && seaHex != null)
            {
                float coastalFactor = 1f / (1f + (coastalDistance / 8f));
                baseTemps[data.Coord] = Mathf.Lerp(baseTemps[data.Coord], baseTemps[seaHex.Coord], coastalFactor * 0.6f);
            }
        }

        for (int windPasses = 0; windPasses < 4; windPasses++)
        {
            foreach (HexData data in grid.Grid.Values)
            {
                Vector2 windDirection = grid.GetWindDirection(data.Coord);
                AxialCoordinate horizontalNeighborCoord = windDirection.x > 0 ? data.Coord + AxialDirections.Directions[(int)AxialCardinalDirections.W] : data.Coord + AxialDirections.Directions[(int)AxialCardinalDirections.E];
                if (grid.TryGetHex(horizontalNeighborCoord, out HexData horizontalNeighborHex))
                {
                    float neighborTemp = baseTemps[horizontalNeighborCoord];
                    float newTemp = Mathf.Lerp(baseTemps[data.Coord], neighborTemp, Mathf.Abs(windDirection.x));
                    windAdjustedTemps[data.Coord] = newTemp;
                }

                AxialCoordinate verticalNeighborCoord = windDirection.y > 0 ? data.Coord + AxialDirections.Directions[(int)AxialCardinalDirections.NE] : data.Coord + AxialDirections.Directions[(int)AxialCardinalDirections.SW];
                if (grid.TryGetHex(verticalNeighborCoord, out HexData verticalNeighborHex))
                {
                    float neighborTemp = baseTemps[verticalNeighborCoord];
                    float newTemp = Mathf.Lerp(baseTemps[data.Coord], neighborTemp, Mathf.Abs(windDirection.y));
                    windAdjustedTemps[data.Coord] = newTemp;
                }

            }

            foreach (HexData data in grid.Grid.Values)
            {
                if (windAdjustedTemps.TryGetValue(data.Coord, out float windAdjustedTemp))
                {
                    baseTemps[data.Coord] = windAdjustedTemp;
                }
            }
        }

        foreach (HexData data in grid.Grid.Values)
        {
            data.ExtraData.SetTemperature(baseTemps[data.Coord]);
        }
    }

    public float ComputeBaseTemperature(float latitude, float elevation, AxialCoordinate coord)
    {
        float baseTemp = (1 - Mathf.Pow(latitude, 2)) - (elevation * 0.128532f);
        float noiseSize = 0.3f;
        float noiseSampleRate = 0.02f;
        Vector2 noiseSamplePoint = AxialGeometry.AxialToCartesian(coord, noiseSampleRate);
        float noise = Mathf.PerlinNoise(noiseSamplePoint.x, noiseSamplePoint.y);
        noise = (noise * 2f) - 1f;
        return Mathf.Clamp01(baseTemp + (noiseSize * noise));
    }
}
