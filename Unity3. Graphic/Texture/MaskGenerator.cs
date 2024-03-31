using System;
using System.Collections.Generic;
using UnityEngine;

public static class MaskGenerator {
    private static readonly Color32 MASKED = new Color32(255, 255, 255, 255);
    private static readonly Color32 TRANSPARENT = new Color32(0, 0, 0, 0);

    public static Texture2D CreateMaskTexture(int border, int tileWidth, int tileHeight, int boardWidth, int boardHeight, IList<bool> visibles, bool rounded = false) {
        var width = boardWidth * tileWidth + border * 2;
        var height = boardHeight * tileHeight + border * 2;
        var tex = new Texture2D(width, height, TextureFormat.Alpha8, false) {
            name = "Generated Mask"
        };

        Color32[] colors = new Color32[width * height];
        Array.Fill(colors, TRANSPARENT);

        for (int i = 0; i < visibles.Count; ++i) {
            if (visibles[i]) {
                int x = i % boardWidth;
                int y = i / boardWidth;

                int spx = x * tileWidth;
                int epx = (x + 1) * tileWidth + border * 2;
                for (int px = spx; px < epx; ++px) {
                    int spy = y * tileHeight;
                    int epy = (y + 1) * tileHeight + border * 2;
                    for (int py = spy; py < epy; ++py) {
                        colors[py * width + px] = MASKED;
                    }
                }
            }
        }

        if (rounded) {
            for (int i = 0; i < visibles.Count; ++i) {
                if (visibles[i]) {
                    int x = i % boardWidth;
                    int y = i / boardWidth;
                    
                    //  4방향의 모서리를 둥글게 처리한다
                    if (GetVisible(visibles, x - 1, y, boardWidth, boardHeight) == false && 
                        GetVisible(visibles, x, y - 1, boardWidth, boardHeight) == false) {
                        MakeTileEdgeRound(colors, x, y, -1, -1, tileWidth, tileHeight, width);
                    }
                    if (GetVisible(visibles, x + 1, y, boardWidth, boardHeight) == false && 
                        GetVisible(visibles, x, y - 1, boardWidth, boardHeight) == false) {
                        MakeTileEdgeRound(colors, x, y, 1, -1, tileWidth, tileHeight, width);
                    } 
                    if (GetVisible(visibles, x + 1, y, boardWidth, boardHeight) == false && 
                        GetVisible(visibles, x, y + 1, boardWidth, boardHeight) == false) {
                        MakeTileEdgeRound(colors, x, y, 1, 1, tileWidth, tileHeight, width);
                    } 
                    if (GetVisible(visibles, x - 1, y, boardWidth, boardHeight) == false && 
                        GetVisible(visibles, x, y + 1, boardWidth, boardHeight) == false) {
                        MakeTileEdgeRound(colors, x, y, -1, 1, tileWidth, tileHeight, width);
                    }
                }
            }
        }
        
        tex.SetPixels32(colors);
        tex.Apply(false);
        
        return tex;
    }

    private static void MakeTileEdgeRound(Color32[] colors, int tilePosX, int tilePosY, int deltaX, int deltaY, int tileWidth, int tileHeight, int texWidth) {
        if (deltaX == 0 || deltaY == 0) {
            Debug.LogError("MaskGenerator.MakeTileEdgeRound :: deltaX, deltaY cannot zero!!");
            return;
        }
        
        var startX = (int)((tilePosX + deltaX * 0.5F + 0.5F) * tileWidth);
        var startY = (int)((tilePosY + deltaY * 0.5F + 0.5F) * tileHeight);
        var endX = startX - (int)(deltaX * tileWidth * 0.1F);
        var endY = startY - (int)(deltaY * tileHeight * 0.1F);
        if (deltaX > 0) {
            startX--;
            endX--;
        }

        if (deltaY > 0) {
            startY--;
            endY--;
        }

        var curY = startY;
        var limit = Math.Pow((tileWidth + tileHeight) * 0.05F, 2);

        int repeatCountY = Mathf.Abs((endY - curY) / deltaY);
        for (int y = 0; y < repeatCountY; y++) {
            var curX = startX;
            int repeatCountX = Mathf.Abs((endX - curX) / deltaX);
            for (int x = 0; x < repeatCountX; x++) {
                var distance = Math.Pow(curX - endX, 2) + Math.Pow(curY - endY, 2);
                if (distance > limit) {
                    colors[curY * texWidth + curX] = TRANSPARENT;
                }

                curX -= deltaX;
            }
            curY -= deltaY;
        }
    }

    private static bool GetVisible(IList<bool> visibles, int x, int y, int width, int height) {
        if (x < 0 || y < 0) return false;
        if (x >= width || y >= height) return false;
        
        var index = x + y * width;
        return visibles[index];
    }
}