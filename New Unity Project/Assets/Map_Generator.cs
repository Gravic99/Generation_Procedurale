using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map_Generator : MonoBehaviour
{
    public enum GenerationType
    {
        RANDOM, PERLONOISE
    }
    public GenerationType generationType;

    public int MapWidth;
    public int MapHeigth;

    public float noiseScale;
    public int octave;
    [Range(0, 1)]
    public float persistance;


    public float lacunarity;
    public bool autoUpdate;
    public int seed;
    public Vector2 offset;
    public Tilemap tilemap;

    public TerrainType[] regionGroundAndRock;
    public TerrainType[] regionsOre;

    public void GenerateMap()
    {
        OnValidate();
        if (generationType == GenerationType.PERLONOISE)
        {
            GenerateMapWithNoise();
        }
        else if (generationType == GenerationType.RANDOM)
        {
            GenerateMapWithRandom();
        }
    }

    private void GenerateMapWithNoise()
    {
        float[,] noiseMapGround = Noise.GenerateNoiseMap(MapWidth, MapHeigth, seed, noiseScale, octave, persistance, lacunarity, offset);
        float[,] noiseMapGisement = Noise.GenerateNoiseMap(MapWidth, MapHeigth, seed + 1, noiseScale, octave, persistance, lacunarity, offset);
        float[,] noiseMapOre = Noise.GenerateNoiseMap(MapWidth, MapHeigth, seed + 2, noiseScale, octave, persistance, lacunarity, offset);
        TileBase[] customTimeMap = new TileBase[MapWidth * MapHeigth];
        for (int y = 0; y < MapHeigth; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                if (noiseMapGisement[x, y] >= 0.8)
                {
                    float rndOre = noiseMapOre[x, y];
                    customTimeMap[y * MapWidth + x] = FindTileFromGissement(rndOre);
                }
                else
                {
                    float rnd = noiseMapGround[x, y];
                    customTimeMap[y * MapWidth + x] = FindTileFromRegion(rnd);
                }

            }

        }
        setTileMap(customTimeMap);
    }

    private void GenerateMapWithRandom()
    {
        TileBase[] customTimeMap = new TileBase[MapWidth * MapHeigth];
        for (int y = 0; y < MapHeigth; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {

                float rnd = UnityEngine.Random.Range(0f, 1f);
                customTimeMap[y * MapWidth + x] = FindTileFromRegion(rnd);
            }

        }
        setTileMap(customTimeMap);
    }

    private TileBase FindTileFromRegion(float rnd)
    {
        for (int i = 0; i < regionGroundAndRock.Length; i++)
        {
            if (rnd <= regionGroundAndRock[i].height)
            {

                return regionGroundAndRock[i].tile;
            }
        }
        return regionGroundAndRock[0].tile;
    }
    private TileBase FindTileFromGissement(float rnd)
    {
        for (int i = 0; i < regionsOre.Length; i++)
        {
            if (rnd <= regionsOre[i].height)
            {
                return regionsOre[i].tile;
            }
        }
        return regionsOre[0].tile;
    }

    private void setTileMap(TileBase[] customTimeMap)
    {
        for (int y = 0; y < MapHeigth; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), customTimeMap[y * MapWidth + x]);
            }
        }
    }


    private void OnValidate()
    {
        if (MapHeigth < 1)
        {
            MapHeigth = 1;
        }
        if (MapWidth < 1)
        {
            MapWidth = 1;
        }
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octave < 1)
        {
            octave = 1;
        }
    }
}


[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public TileBase tile;

}