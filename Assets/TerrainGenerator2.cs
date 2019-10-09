using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TerrainGenerator2 : MonoBehaviour
{

    public int Tdepth = 20;
    public int Twidth = 256;
    public int Theight = 256;
    public float scale = 20f;

    public float offsetX = 100f;
    public float offsetY = 100f;

    [RangeAttribute(1f, 10f)]
    public float flatness = 1f;
    [RangeAttribute(1f, 20f)]
    public float frequency = 1f;
    [RangeAttribute(1f, 10f)]
    public int octaves = 8;
    Texture2D image;
    Terrain terrain;




    // Start is called before the first frame update
    void Start()
    {
        
        offsetX = Random.Range(0f, 9999f);
        offsetY = Random.Range(0f, 9999f);
    }


    // Update is called once per frame
    void Update()
    {
        terrain = GetComponent<Terrain>();

        
        image = new Texture2D(terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight);
        image.LoadImage(File.ReadAllBytes("Assets/map.jpg"));

    
        float[,] heightmap = terrain.terrainData.GetHeights(0, 0, terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight);
        

        terrain.terrainData.heightmapResolution = Twidth + 1;
        
        for (int i = 0; i < terrain.terrainData.heightmapHeight; ++i)
        {

            for (int j = 0; j < terrain.terrainData.heightmapWidth; ++j)
            {
                float x = i / (float)terrain.terrainData.heightmapWidth;
                float y = j / (float)terrain.terrainData.heightmapHeight;



                float height = image.GetPixel(i, j).b;
                float current_frequency = frequency;
                float amplitude = 1;
                for (int z = 0; z < octaves; ++z)
                {
                    height = height + Mathf.PerlinNoise(x * current_frequency, y * current_frequency) * amplitude;
                    amplitude /= 2;
                    current_frequency *= 2;
                }


                heightmap[i, j] = height / flatness;
                                
            }
        }
        terrain.terrainData.SetHeights(0, 0, heightmap);
        
        offsetX += Time.deltaTime * 5f;
    }
        
    
        
        float Height(int x, int y)
    {
        float xCoord = (float)x / Twidth * scale + offsetX;
        float yCoord = (float)y / Theight * scale + offsetY;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}
