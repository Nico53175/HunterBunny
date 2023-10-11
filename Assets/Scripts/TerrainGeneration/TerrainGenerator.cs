using UnityEngine;

[RequireComponent(typeof(Terrain))]
public class TerrainGenerator : MonoBehaviour
{
    Terrain terrain;
    [SerializeField] int depth;
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] float scale;
    [SerializeField] int resolution;

    [Header("Noise Layers")]
    [SerializeField] float secondLayerScale = 2f;
    [SerializeField] float secondLayerAmplitude = 0.5f;

    [Header("Crater")]
    [SerializeField] float craterRadius = 50f;
    [SerializeField] float craterDepth = 0.1f;

    [Header("Crater Details")]
    [SerializeField] float craterStonyNoiseFrequency = 0.05f;
    [SerializeField] float craterStonyNoiseAmplitude = 0.1f;


    private void Start()
    {
        terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }
    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = resolution;
        terrainData.size = new Vector3(width, depth, height);
        terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;
    }
    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                heights[x, y] = CalculateHeight(x, y);
            }
        }
        CreateCrater(heights);
        return heights;
    }
    float CalculateHeight(int x, int y)
    {
        float xCoord = (float)x / width * scale;
        float yCoord = (float)y / height * scale;

        // Base layer of noise
        float baseNoise = Mathf.PerlinNoise(xCoord, yCoord);

        // Second layer of noise with different scale and amplitude
        float xCoord2 = xCoord * secondLayerScale;
        float yCoord2 = yCoord * secondLayerScale;
        float detailNoise = Mathf.PerlinNoise(xCoord2, yCoord2) * secondLayerAmplitude;

        // Combine the noises and normalize to [0, 1]
        float finalHeight = baseNoise + detailNoise;
        finalHeight = Mathf.Clamp01(finalHeight);

        return finalHeight;
    }

    // Crater
    private void CreateCrater(float[,] heights)
    {
        Vector2 absoluteCraterCenter = CalculateAbsoluteCraterCenter();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float distanceToCrater = Vector2.Distance(new Vector2(x, y), absoluteCraterCenter);

                if (distanceToCrater < craterRadius)
                {
                    // Create a gradient for the crater depth, making it deeper in the middle.
                    float depthGradient = 1f - (distanceToCrater / craterRadius);
                    float decrease = craterDepth * depthGradient * depthGradient; // Using square for a sharper depth change.

                    // Add some secondary noise for the stony texture inside the crater.
                    float stonyNoise = Mathf.PerlinNoise(x * craterStonyNoiseFrequency, y * craterStonyNoiseFrequency) * craterStonyNoiseAmplitude;

                    heights[x, y] -= (decrease + stonyNoise);
                    heights[x, y] = Mathf.Clamp01(heights[x, y]);
                }
            }
        }
    }

    private Vector2 CalculateAbsoluteCraterCenter()
    {
        return new Vector2(width * 0.5f, height * 0.5f);
    } 
}
