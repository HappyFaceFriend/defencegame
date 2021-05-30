using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SpawnCall
{
    public readonly int id;
    public readonly int routeNumber;
    public readonly float time;
    public SpawnCall(int id, int routeNumber, float time)
    {
        this.id = id;
        this.routeNumber = routeNumber;
        this.time = time;
    }
}
public class WaveData
{
    public int waveNumber;
    public List<SpawnCall> spawnCalls;
    public WaveData(int waveNumber)
    {
        this.waveNumber = waveNumber;
        spawnCalls = new List<SpawnCall>();
    }
}
public class LevelSpawnData
{
    public List<WaveData> WaveDatas { get { return waveDatas; } }
    List<WaveData> waveDatas;
    enum NextRead { Wave, SpawnCall, Anything}
    NextRead state;
    public LevelSpawnData(string dataFilePath)
    {
        string rawData = FileUtils.ReadFile(dataFilePath);
        string[] lines = FileUtils.SplitLines(rawData);
        waveDatas = new List<WaveData>();
        state = NextRead.Wave;
        try
        {
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Trim().Equals("") || lines[i].StartsWith("//"))
                    continue;
                else if (lines[i].StartsWith("#"))
                {
                    if (state != NextRead.Wave && state != NextRead.Anything)
                        throw new Exception(String.Format("line {0}: This line shouldn't be a wave.", i));
                    waveDatas.Add(new WaveData(waveDatas.Count));
                    state = NextRead.SpawnCall;
                }
                else
                {
                    if (state != NextRead.SpawnCall && state != NextRead.Anything)
                        throw new Exception(String.Format("line {0}: This line shouldn't be a spawncall.", i));
                    try
                    {
                        string[] words = lines[i].Split('\t');

                        SpawnCall spawnCall = new SpawnCall(int.Parse(words[0]), int.Parse(words[1]), uint.Parse(words[2]) / 1000.0f);
                        waveDatas[waveDatas.Count-1].spawnCalls.Add(spawnCall);
                        state = NextRead.Anything;
                    }
                    catch
                    {
                        throw new Exception(String.Format("line {0}: Wrong spawncall format ({1})", i, lines[i]));
                    }
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
            waveDatas = new List<WaveData>();
        }
    }
}
