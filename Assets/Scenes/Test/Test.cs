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

            ComputeTemperatures(grid);

            ComputePrecipitations(grid);

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
            float latitude = grid.GetLatitude01(data.Coord);
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
                Vector2 upWindDirection = -windDirection;
                AxialCoordinate neighborCoord = data.Coord + AxialGeometry.ConvertVectorToAxialDirection(upWindDirection);
                if (grid.TryGetHex(neighborCoord, out HexData neighborHex))
                {
                    float neighborTemp = baseTemps[neighborCoord];
                    float newTemp = Mathf.Lerp(baseTemps[data.Coord], neighborTemp, Mathf.Abs(windDirection.magnitude));
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

    public void ComputePrecipitations(HexGrid grid)
    {
        Dictionary<AxialCoordinate, float> baseHums = new Dictionary<AxialCoordinate, float>();
        Dictionary<AxialCoordinate, float> windAdjustedHums = new Dictionary<AxialCoordinate, float>();
        Dictionary<AxialCoordinate, float> accumulatedPrecs = new Dictionary<AxialCoordinate, float>();

        foreach (HexData data in grid.Grid.Values)
        {
            baseHums[data.Coord] = 0f;
            accumulatedPrecs[data.Coord] = 0f;
        }

        for (int windPasses = 0; windPasses < _rainPasses; windPasses++)
        {
            foreach (HexData data in grid.Grid.Values)
            {
                Vector2 windDirection = grid.GetWindDirection(data.Coord);
                Vector2 upWindDirection = -windDirection;
                AxialCoordinate neighborCoord = data.Coord + AxialGeometry.ConvertVectorToAxialDirection(upWindDirection);
                if (grid.TryGetHex(neighborCoord, out HexData neighborHex))
                {
                    float neighborHum = baseHums[neighborCoord];
                    float newHum = Mathf.Lerp(baseHums[data.Coord], neighborHum, Mathf.Abs(windDirection.magnitude));

                    if (data.ExtraData.IsSea) // evaporate
                    {
                        newHum = Mathf.Lerp(newHum, 1f, data.ExtraData.Temperature);
                    }

                    else // precipitate
                    {
                        float dh = data.ExtraData.Elevation - neighborHex.ExtraData.Elevation;
                        float uplift = Mathf.Max(0f, dh);
                        float upliftFactor = 0.01f;

                        float rainThisStep = newHum > _baseRain? _baseRain : 0f;

                        accumulatedPrecs[data.Coord] += rainThisStep;
                        newHum -= rainThisStep;
                    }

                    windAdjustedHums[data.Coord] = newHum;
                }
            }

            foreach (HexData data in grid.Grid.Values)
            {
                if (windAdjustedHums.TryGetValue(data.Coord, out float windAdjustedHum))
                {
                    baseHums[data.Coord] = windAdjustedHum;
                }
            }
        }

        float maxPrec = 0f;

        foreach (HexData data in grid.Grid.Values)
        {
            float currentPrec = accumulatedPrecs[data.Coord];
            if (currentPrec > maxPrec) maxPrec = currentPrec;
        }

        foreach (HexData data in grid.Grid.Values)
        {
            accumulatedPrecs[data.Coord] /= maxPrec;
        }

        foreach (HexData data in grid.Grid.Values)
        {
            data.ExtraData.SetPrecipitation(accumulatedPrecs[data.Coord]);
        }
    }
}
