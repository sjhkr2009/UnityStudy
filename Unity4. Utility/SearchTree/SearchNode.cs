using System;
using System.Collections.Generic;

public class SearchNode : IDisposable {
    public Dictionary<char, SearchNode> Children { get; private set; }
    public bool IsEndOfWord { get; set; }
    public List<SearchData> Products { get; private set; }

    public SearchNode() {
        Children = new Dictionary<char, SearchNode>();
        IsEndOfWord = false;
        Products = new List<SearchData>();
    }
    
    public List<SearchData> GetAllDataRecursive() {
        var results = new List<SearchData>();
        if (IsEndOfWord) {
            results.AddRange(Products);
        }

        foreach (var child in Children.Values) {
            results.AddRange(child.GetAllDataRecursive());
        }

        return results;
    }

    public int ChildCount() {
        var count = Children.Count;
        foreach (var child in Children) {
            count += child.Value.ChildCount();
        }

        return count;
    }
    
    public void ClearData() {
        foreach (var child in Children.Values) {
            child.ClearData();
        }
        Children.Clear();
        Products.Clear();
        IsEndOfWord = false;
    }

    public void Dispose() {
        ClearData();
    }
}