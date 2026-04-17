using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrecipitationGen
{
    public static void ComputePrecipitations(HexGrid grid, WorldGenParameters parameters)
    {
        Dictionary<AxialCoordinate, float> basePrecs = new Dictionary<AxialCoordinate, float>();
        
        foreach (HexData data in grid.GetValidHexes())
        {
            if (data.ExtraData.IsSea) continue;

            int numHexesUpwindFromSea = 1;
            HexData currentHex = data;
            int maxHexesFromSea = Mathf.RoundToInt(parameters.MaxPrecipitationDistanceFromSea * grid.Width);

            while (numHexesUpwindFromSea < maxHexesFromSea)
            {
                Vector2 windDirection = grid.GetWindDirection(currentHex.Coord);
                Vector2 upWindDirection = -windDirection;
                AxialCoordinate neighborCoord = currentHex.Coord + AxialGeometry.ConvertVectorToAxialDirection(upWindDirection);

                if (grid.TryGetHex(neighborCoord, out HexData neighborHex))
                {
                    if (neighborHex.ExtraData.IsSea) break;

                    numHexesUpwindFromSea++;
                    currentHex = neighborHex;
                }

                else { break; }
            }

            basePrecs[data.Coord] = (maxHexesFromSea - numHexesUpwindFromSea) / (float)maxHexesFromSea;
        }

        foreach (HexData data in grid.GetValidHexes())
        {
            if (!data.ExtraData.IsSea) data.ExtraData.SetPrecipitation(basePrecs[data.Coord]);
        }
    }
}
