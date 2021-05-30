using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

class FileUtils
{
    static string LINE_SPLIT = @"\r\n|\n\r|\n|\r";

    public static string[] SplitLines(string text)
    {
        return Regex.Split(text, LINE_SPLIT);
    }
    public static string ReadFile(string filePath)
    {
        string path = Application.dataPath + "/" + filePath;
#if UNITY_EDITOR
        if (!File.Exists(path)) Debug.LogWarning("Can't find file : " + path);
#endif
        string data = File.ReadAllText(path, System.Text.Encoding.UTF8);
        return data;
    }
}
