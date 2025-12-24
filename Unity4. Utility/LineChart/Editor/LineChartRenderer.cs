using System.Collections.Generic;
using System.Globalization;
using LineChart;
using UnityEditor;
using UnityEngine;

public static class LineChartRenderer
{
    public static ChartHoverInfo Draw(Rect rect, ChartData chart)
    {
        if (chart == null)
            return ChartHoverInfo.None;

        var bounds = chart.CalculateBounds();
        if (!bounds.HasValue)
            return ChartHoverInfo.None;

        EditorGUI.DrawRect(rect, new Color(0.15f, 0.15f, 0.15f, 1f));

        const float paddingLeft = 60f;
        const float paddingRight = 100f;
        const float paddingTop = 30f;
        const float paddingBottom = 30f;

        var inner = new Rect(
            rect.x + paddingLeft,
            rect.y + paddingTop,
            rect.width - paddingLeft - paddingRight,
            rect.height - paddingTop - paddingBottom
        );

        if (inner.width <= 0f || inner.height <= 0f)
            return ChartHoverInfo.None;

        double minX = bounds.MinX;
        double maxX = bounds.MaxX;
        double minY = bounds.MinY;
        double maxY = bounds.MaxY;

        double spanX = maxX - minX;
        double spanY = maxY - minY;
        if (spanX <= double.Epsilon) spanX = 1.0;
        if (spanY <= double.Epsilon) spanY = 1.0;

        DrawAxis(inner, minX, maxX, minY, maxY);

        // Hover 탐색
        var evt = Event.current;
        Vector2 mousePos = evt?.mousePosition ?? Vector2.zero;
        const float pickRadius = 8f;
        float pickRadiusSqr = pickRadius * pickRadius;
        
        ChartHoverInfo info = ChartHoverInfo.None;

        foreach (var lineData in chart.LineDatas)
        {
            if (lineData.Points.Count < 2)
                continue;

            var polyPoints = new List<Vector3>(lineData.Points.Count);
            var markerPoints = new List<Vector3>(lineData.Points.Count);

            foreach (var p in lineData.Points)
            {
                float nx = (float)((p.X - minX) / spanX);
                float ny = (float)((p.Y - minY) / spanY);

                float px = inner.xMin + nx * inner.width;
                float py = inner.yMax - ny * inner.height;

                var screenPos = new Vector3(px, py, 0f);
                polyPoints.Add(screenPos);
                markerPoints.Add(screenPos);

                if (evt == null)
                    continue;

                if (evt.type != EventType.Repaint && evt.type != EventType.MouseMove)
                    continue;
                
                Vector2 diff = mousePos - (Vector2)screenPos;
                float d2 = diff.sqrMagnitude;
                if (d2 < pickRadiusSqr)
                {
                    if (!info.HasHit)
                        info = new ChartHoverInfo(screenPos, lineData.Name, p);
                    else
                        info.AddPoint(lineData.Name, p);
                }
            }

            // 선 그리기
            Color color = ParseColorHex(lineData.Style.ColorHex, Color.white);
            float thickness = Mathf.Max(1f, lineData.Style.Thickness);

            Handles.BeginGUI();
            Handles.color = color;
            Handles.DrawAAPolyLine(thickness, polyPoints.ToArray());
            for (int i = 0; i < markerPoints.Count; i++)
            {
                Handles.DrawWireDisc(markerPoints[i], Vector3.forward, 2.5f);
            }
            Handles.EndGUI();
        }
        
        if (!string.IsNullOrEmpty(chart.Title))
        {
            var titleRect = new Rect(rect.x, rect.y, rect.width, 18f);
            GUI.Label(titleRect, chart.Title, EditorStyles.boldLabel);
        }

        return info;
    }

    private static void DrawAxis(Rect inner, double minX, double maxX, double minY, double maxY)
    {
        Handles.BeginGUI();

        Handles.color = new Color(0.6f, 0.6f, 0.6f, 1f);

        // X축
        Handles.DrawLine(
            new Vector3(inner.xMin, inner.yMax, 0),
            new Vector3(inner.xMax, inner.yMax, 0)
        );

        // Y축
        Handles.DrawLine(
            new Vector3(inner.xMin, inner.yMin, 0),
            new Vector3(inner.xMin, inner.yMax, 0)
        );

        Handles.EndGUI();

        const int tickCount = 3;

        // X축
        for (int i = 0; i <= tickCount; i++)
        {
            float t = i / (float)tickCount;
            float x = Mathf.Lerp(inner.xMin, inner.xMax, t);
            double value = Mathf.Lerp((float)minX, (float)maxX, t);

            var labelRect = new Rect(x - 25f, inner.yMax + 2f, 50f, 16f);
            GUI.Label(labelRect, value.ToString("0.##"));
        }

        // Y축
        for (int i = 0; i <= tickCount; i++)
        {
            float t = i / (float)tickCount;
            float y = Mathf.Lerp(inner.yMax, inner.yMin, t);
            double value = Mathf.Lerp((float)minY, (float)maxY, t);

            var labelRect = new Rect(inner.xMin - 50f, y - 8f, 48f, 16f);
            GUI.Label(labelRect, value.ToString("0.##"), EditorStyles.miniLabel);
        }
    }

    private static Color ParseColorHex(string hex, Color fallback)
    {
        if (string.IsNullOrEmpty(hex))
            return fallback;

        hex = hex.Trim();
        if (hex[0] == '#')
            hex = hex.Substring(1);

        byte r, g, b, a = 255;

        try
        {
            if (hex.Length == 6)
            {
                r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
                b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
            }
            else if (hex.Length == 8)
            {
                r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
                b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
                a = byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
            }
            else
            {
                return fallback;
            }
        }
        catch
        {
            return fallback;
        }

        return new Color32(r, g, b, a);
    }
}
