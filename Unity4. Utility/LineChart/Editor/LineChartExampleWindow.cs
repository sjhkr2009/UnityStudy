using System;
using System.Collections.Generic;
using System.Linq;
using LineChart;
using UnityEditor;
using UnityEngine;

public class LineChartExampleWindow : EditorWindow
{
    private Dictionary<string, bool> _visibleLines = new();
    private ChartData _allChart;
    private ChartData _filteredChart;

    [MenuItem("Test/Test Line Chart")]
    public static void ShowWindow()
    {
        var window = GetWindow<LineChartExampleWindow>();
        window.Show();
    }

    private void OnEnable()
    {
        wantsMouseMove = true;

        SetData();
    }

    private void SetData()
    {
        var line1 = new LineData("스킬 A", new[]
        {
            new PointData(0.0f, 0),
            new PointData(0.5f, 5),
            new PointData(1.0f, 35),
            new PointData(1.5f, 40),
            new PointData(2.0f, 60),
        })
        {
            Style = new LineStyle("#00FF00", thickness: 2f)
        };
        
        var line2 = new LineData("스킬 B", new[]
        {
            new PointData(0.0f, 0),
            new PointData(0.5f, 25),
            new PointData(1.0f, 40),
            new PointData(1.5f, 45),
            new PointData(2.0f, 50),
        })
        {
            Style = new LineStyle("#FFFF00", thickness: 2f)
        };
        
        var line3 = new LineData("스킬 C", new[]
        {
            new PointData(0.0f, 0),
            new PointData(0.5f, 15),
            new PointData(1.0f, 30),
            new PointData(1.5f, 45),
            new PointData(2.0f, 60),
        })
        {
            Style = new LineStyle("#00FFFF", thickness: 2f)
        };

        _allChart = new ChartData("시간당 피해량")
        {
            XAxis = new AxisData("Time", "sec"),
            YAxis = new AxisData("Damage")
        };
        
        _allChart.LineDatas.Add(line1);
        _allChart.LineDatas.Add(line2);
        _allChart.LineDatas.Add(line3);

        foreach (var lineData in _allChart.LineDatas)
        {
            _visibleLines[lineData.Name] = true;
        }
    }

    private void OnGUI()
    {
        if (_allChart == null)
            return;

        if (Event.current.type == EventType.MouseMove)
            Repaint();

        DrawToggles();
        BuildFilteredChart();
        
        if (_filteredChart.LineDatas.Count == 0)
        {
            EditorGUILayout.HelpBox("표시할 데이터가 없습니다.", MessageType.Info);
            return;
        }

        Rect chartRect = GUILayoutUtility.GetRect(
            600f,
            350f,
            GUILayout.ExpandWidth(false),
            GUILayout.ExpandHeight(false)
        );
        
        var hover = LineChartRenderer.Draw(chartRect, _filteredChart);
        if (hover.HasHit && Event.current.type == EventType.Repaint)
            DrawHover(hover);
    }
    
    private void DrawToggles()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginChangeCheck();
        GUILayout.Label("표시할 데이터:", GUILayout.Width(90f));

        foreach (var lineData in _allChart.LineDatas)
        {
            _visibleLines[lineData.Name] = EditorGUILayout.ToggleLeft(lineData.Name, _visibleLines.GetValueOrDefault(lineData.Name, true));
        }

        if (EditorGUI.EndChangeCheck())
        {
            _filteredChart = null;
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(4f);
    }
    
    private void BuildFilteredChart()
    {
        if (_filteredChart != null)
            return;
        
        var filtered = new ChartData(_allChart.Title)
        {
            XAxis = _allChart.XAxis,
            YAxis = _allChart.YAxis
        };

        foreach (var lineData in _allChart.LineDatas)
        {
            if (_visibleLines.GetValueOrDefault(lineData.Name, true))
                filtered.LineDatas.Add(lineData);
        }

        _filteredChart = filtered;
    }

    private void DrawHover(ChartHoverInfo hover)
    {
        Handles.BeginGUI();
        Handles.color = Color.white;
        Handles.DrawSolidDisc(hover.ScreenPosition, Vector3.forward, 4f);
        Handles.EndGUI();

        var text = string.Join('\n', hover.HitPoints.Select(p => $"{p.LineName} : {p.HitPoint.Y}"));

        var prevTextColor = GUI.contentColor;
        var prevColor = GUI.backgroundColor;
        GUI.color = Color.white;
        GUI.backgroundColor = Color.darkGreen;
        GUIContent content = new GUIContent(text);
        Vector2 size = GUI.skin.box.CalcSize(content);

        Vector2 mousePos = Event.current.mousePosition;
        Rect tooltipRect = new Rect(
            mousePos.x + 15f,
            mousePos.y - size.y * 0.5f,
            size.x + 8f,
            size.y
        );
        
        GUI.Box(tooltipRect, content);
        GUI.color = prevTextColor;
        GUI.backgroundColor = prevColor;
    }
}
