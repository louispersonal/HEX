using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WorldGenController : MonoBehaviour
{
    // Width and height of the texture in pixels.
    public int Columns;
    public int Rows;

    // The origin of the sampled area in the plane.
    public float xOrg;
    public float yOrg;

    public float LandThreshold;

    private float scale = 0.1f;

    private const int MAX_HEXES_FROM_WATER = 10;

    public static WorldGenController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {

    }

    public void SetElevations(BaseHexGrid hexGrid)
    {
        xOrg = Random.Range(100f, 10000f);
        yOrg = Random.Range(100f, 10000f);
        foreach (Hex hex in hexGrid.Grid.Values)
        {
            (int x, int y) oCoord = hex.Coord.ConvertToOddR();
            hex.SetElevation(CalcNoise(oCoord.x, oCoord.y));
        }
    }

    private float CalcNoise(float x, float y)
    {
        float xCoord = xOrg + x * scale;
        float yCoord = yOrg + y * scale;
        float value = Mathf.PerlinNoise(xCoord, yCoord);
        return value > LandThreshold? Normalize(value, LandThreshold, 1f) : 0;
    }

    public void SetTemperatures(BaseHexGrid hexGrid)
    {
        foreach (Hex hex in hexGrid.Grid.Values)
        {
            float y = hex.Coord.R;
            float value = 1f - Normalize(Mathf.Abs(y - Rows / 2), 0f, Rows / 2);
            hex.SetTemperature(value);
        }
    }

    public void SetPrecipitationVegetation(BaseHexGrid hexGrid)
    {
        foreach (Hex hex in hexGrid.Grid.Values)
        {
            int distance = DistanceToNearestWaterHex(hex);
            float value = 1f - Normalize(distance, 0f, MAX_HEXES_FROM_WATER);
            hex.SetPrecipitation(value);
            hex.CalculateVegetations();
        }
    }

    public int DistanceToNearestWaterHex(Hex hex)
    {
        for (int r = 0; r < MAX_HEXES_FROM_WATER; r++)
        {
            List<Hex> hexes = hex.HexesInRingOfRadius(r);
            foreach (Hex target in hexes)
            {
                if (target.IsSea) return r;
            }
        }

        return MAX_HEXES_FROM_WATER;
    }

    private float Normalize(float v, float floor, float ceiling)
    {
        return (v - floor) / (ceiling - floor);
    }

    private void Update()
    {

    }
}