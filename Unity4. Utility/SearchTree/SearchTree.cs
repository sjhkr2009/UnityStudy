using System;
using System.Collections.Generic;

/// <summary>
/// Trie 구조를 기반으로, 입력된 단어들에 대한 초성 검색 및 부분 문자열 검색 기능을 제공합니다.
/// </summary>
public class SearchTree : IDisposable {
    private SearchNode Root { get; }

    public SearchTree() {
        Root = new SearchNode();
    }

    public int DataCount => Root.ChildCount();

    public void Insert(SearchData searchData) {
        AddWordRecursive(searchData.Word, searchData);
        AddWordRecursive(HangulUtils.GetChosung(searchData.Word), searchData);
        foreach (var extraWord in searchData.ExtraWords) {
            AddWordRecursive(extraWord, searchData);
        }
    }

    private void AddWordRecursive(string word, SearchData searchData) {
        for (int i = 0; i < word.Length; i++) {
            AddWord(word.Substring(i), searchData);
        }
    }

    private void AddWord(string word, SearchData searchData) {
        var node = Root;
        foreach (var c in word) {
            if (!node.Children.ContainsKey(c)) {
                node.Children[c] = new SearchNode();
            }

            node = node.Children[c];
        }

        node.IsEndOfWord = true;
        node.Products.Add(searchData);
    }

    public List<SearchData> Search(string prefix) {
        var node = Root;
        foreach (var ch in prefix) {
            if (!node.Children.ContainsKey(ch)) {
                return new List<SearchData>();
            }

            node = node.Children[ch];
        }

        return node.GetAllDataRecursive();
    }
    
    public void Clear() {
        Root.ClearData();
    }

    public void Dispose() {
        Clear();
    }
}