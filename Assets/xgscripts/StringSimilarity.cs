using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class StringSimilarity : MonoBehaviour
{
    // ���峣����������������
    private static readonly HashSet<string> StopWords = new HashSet<string>
    {
        "��", "ѽ", "��", "��", "Ŷ", "��", "Ӵ", "��", "��", "��", "��", "��","��"
    };

    // ȥ����������
    private static string RemoveStopWords(string input)
    {
        foreach (var word in StopWords)
        {
            input = Regex.Replace(input, word, ""); // ʹ�������滻����������
        }
        return input;
    }


   // ���������ַ�����Levenshtein����
    private static int LevenshteinDistance(string source, string target)
    {
        if (string.IsNullOrEmpty(source)) return target.Length;
        if (string.IsNullOrEmpty(target)) return source.Length;

        int[,] distance = new int[source.Length + 1, target.Length + 1];

        // ��ʼ���������
        for (int i = 0; i <= source.Length; i++) distance[i, 0] = i;
        for (int j = 0; j <= target.Length; j++) distance[0, j] = j;

        // �������
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


    // �������������ַ��������ƶ�
    public static double CalculateSimilarity(string string1, string string2)
    {
        // ȥ����������
        string processedString1 = RemoveStopWords(string1);
        string processedString2 = RemoveStopWords(string2);

        // ����Levenshtein����
        int levenshteinDistance = LevenshteinDistance(processedString1, processedString2);

        // ����Levenshtein����������ƶ�
        int maxLength = Math.Max(processedString1.Length, processedString2.Length);
        return (maxLength - levenshteinDistance) / (double)maxLength;
    }
}

