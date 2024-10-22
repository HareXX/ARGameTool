using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class StringSimilarity : MonoBehaviour
{
    // 定义常见的中文语气助词
    private static readonly HashSet<string> StopWords = new HashSet<string>
    {
        "啊", "呀", "吧", "吗", "哦", "嘛", "哟", "呢", "呗", "哈", "嘿", "哇","的"
    };

    // 去掉语气助词
    private static string RemoveStopWords(string input)
    {
        foreach (var word in StopWords)
        {
            input = Regex.Replace(input, word, ""); // 使用正则替换掉语气助词
        }
        return input;
    }


   // 计算两个字符串的Levenshtein距离
    private static int LevenshteinDistance(string source, string target)
    {
        if (string.IsNullOrEmpty(source)) return target.Length;
        if (string.IsNullOrEmpty(target)) return source.Length;

        int[,] distance = new int[source.Length + 1, target.Length + 1];

        // 初始化距离矩阵
        for (int i = 0; i <= source.Length; i++) distance[i, 0] = i;
        for (int j = 0; j <= target.Length; j++) distance[0, j] = j;

        // 计算距离
        for (int i = 1; i <= source.Length; i++)
        {
            for (int j = 1; j <= target.Length; j++)
            {
                int cost = (source[i - 1] == target[j - 1]) ? 0 : 1;
                distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
            }
        }

        return distance[source.Length, target.Length];
    }


    // 计算两个中文字符串的相似度
    public static double CalculateSimilarity(string string1, string string2)
    {
        // 去除语气助词
        string processedString1 = RemoveStopWords(string1);
        string processedString2 = RemoveStopWords(string2);

        // 计算Levenshtein距离
        int levenshteinDistance = LevenshteinDistance(processedString1, processedString2);

        // 根据Levenshtein距离计算相似度
        int maxLength = Math.Max(processedString1.Length, processedString2.Length);
        return (maxLength - levenshteinDistance) / (double)maxLength;
    }
}

