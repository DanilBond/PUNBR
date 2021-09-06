using System.Collections.Generic;
using UnityEngine;


public class ObsceneLanguage : MonoBehaviour
{
    char[] delimiterChars = { ' ', ',', '.', ':', '\t', '\n', '\r' };
    [TextArea]
    public string allWords;
    public string[] temp;
    string[] parced;
    public List<string> wordList;

    private void Start()
    {
        GenerateList();
    }
    void GenerateList()
    {
        parced = allWords.Split(delimiterChars);

        for (int i = 0; i < parced.Length; i++)
        {
            if (parced[i] != temp[0])
            {
                wordList.Add(parced[i]);
            }
        }
    }
}
