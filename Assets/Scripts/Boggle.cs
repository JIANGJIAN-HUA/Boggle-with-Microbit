using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boggle
{
    static List<string> FindVoc = new List<string>();

    // Alphabet size
    static readonly int SIZE = 26;

    private static int M;
    private static int N;

    public Boggle(int rows, int cols)
    {
        M = rows;
        N = cols;
    }

    // trie Node
    public class TrieNode
    {
        public TrieNode[] Child = new TrieNode[SIZE];

        // isLeaf is true if the node represents
        // end of a word
        public bool leaf;

        // constructor
        public TrieNode()
        {
            leaf = false;
            for (int i = 0; i < SIZE; i++)
                Child[i] = null;
        }
    }

    // If not present, inserts a key into the trie
    // If the key is a prefix of trie node, just
    // marks leaf node
    static void insert(TrieNode root, string Key)
    {
        int n = Key.Length;
        TrieNode pChild = root;
        //Debug.Log("n: " + n);

        for (int i = 0; i < n; i++)
        {
            int index = Key[i] - 'A';
            //Debug.Log("index: " + index);

            if (pChild.Child[index] == null)
                pChild.Child[index] = new TrieNode();

            pChild = pChild.Child[index];
        }

        // make last node as leaf node
        pChild.leaf = true;
    }

    // function to check that current location
    // (i and j) is in matrix range
    static bool isSafe(int i, int j, bool[,] visited)
    {
        return (i >= 0 && i < M && j >= 0 && j < N && !visited[i, j]);
    }

    // A recursive function to print all words present on boggle
    static void searchWord(TrieNode root, char[,] boggle, int i,
                           int j, bool[,] visited, String str)
    {
        //Debug.Log("[" + i + ", " + j + "]");
        // if we found word in trie / dictionary
        if (root.leaf == true)
        {
            //Debug.Log(str);
            SaveVoc(str);
        }

        // If both I and j in range and we visited
        // that element of matrix first time
        if (isSafe(i, j, visited))
        {
            // make it visited
            visited[i, j] = true;

            // traverse all child of current root
            for (int K = 0; K < SIZE; K++)
            {
                if (root.Child[K] != null)
                {
                    // current character
                    char ch = (char)(K + 'A');

                    // Recursively search reaming character of word
                    // in trie for all 8 adjacent cells of
                    // boggle[i, j]
                    if (isSafe(i + 1, j + 1, visited) && boggle[i + 1, j + 1] == ch)
                        searchWord(root.Child[K], boggle, i + 1, j + 1,
                                   visited, str + ch);
                    if (isSafe(i, j + 1, visited) && boggle[i, j + 1] == ch)
                        searchWord(root.Child[K], boggle, i, j + 1,
                                   visited, str + ch);
                    if (isSafe(i - 1, j + 1, visited) && boggle[i - 1, j + 1] == ch)
                        searchWord(root.Child[K], boggle, i - 1, j + 1,
                                   visited, str + ch);
                    if (isSafe(i + 1, j, visited) && boggle[i + 1, j] == ch)
                        searchWord(root.Child[K], boggle, i + 1, j,
                                   visited, str + ch);
                    if (isSafe(i + 1, j - 1, visited) && boggle[i + 1, j - 1] == ch)
                        searchWord(root.Child[K], boggle, i + 1, j - 1,
                                   visited, str + ch);
                    if (isSafe(i, j - 1, visited) && boggle[i, j - 1] == ch)
                        searchWord(root.Child[K], boggle, i, j - 1,
                                   visited, str + ch);
                    if (isSafe(i - 1, j - 1, visited) && boggle[i - 1, j - 1] == ch)
                        searchWord(root.Child[K], boggle, i - 1, j - 1,
                                   visited, str + ch);
                    if (isSafe(i - 1, j, visited) && boggle[i - 1, j] == ch)
                        searchWord(root.Child[K], boggle, i - 1, j,
                                   visited, str + ch);
                }
            }

            // make current element unvisited
            visited[i, j] = false;
        }
    }

    // Prints all words present in dictionary.
    static void findWords(char[,] boggle, TrieNode root)
    {
        // Mark all characters as not visited
        bool[,] visited = new bool[M, N];
        TrieNode pChild = root;

        string str = "";

        // traverse all matrix elements
        for (int i = 0; i < M; i++)
        {
            for (int j = 0; j < N; j++)
            {
                // we start searching for word in dictionary
                // if we found a character which is child
                // of Trie root
                if (pChild.Child[(boggle[i, j]) - 'A'] != null)
                {
                    str = str + boggle[i, j];
                    searchWord(pChild.Child[(boggle[i, j]) - 'A'],
                               boggle, i, j, visited, str);
                    str = "";
                }
            }
        }
    }
    public List<string> FindWord(char[,] boggle)
    {
        // Let the given dictionary be following
        FindVoc.Clear();
        string[] dictionary = new string[Constants.VocNum];
        for (int i = 0; i < Constants.VocNum; i++)
        {
            dictionary[i] = GetVoc.Voc[i].English.ToUpper();
            //Debug.Log(dictionary[i]);
        }

        // root Node of trie
        TrieNode root = new TrieNode();

        // insert all words of dictionary into trie
        int n = dictionary.Length;
        for (int i = 0; i < n; i++)
            insert(root, dictionary[i]);

        findWords(boggle, root);

        // return find Vocabulary
        return FindVoc;
    }

    static void SaveVoc(string VocStr)
    {
        bool found = FindVoc.Contains(VocStr);

        if (!found)
        {
            FindVoc.Add(VocStr);
        }
    }
}
