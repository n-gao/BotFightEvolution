using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map {
    public int MapSizeX {get; private set;}
    public int MapSizeY {get; private set;}
    public Field[,] Fields {get; private set;}

    public Map(int x = 20, int y = 20) {
        MapSizeX = x;
        MapSizeY = y;
        Fields = new Field[x, y];
        for(int i = 0; i < x; i++) {
            for (int j = 0; j < y; j++) {
                Fields[i, j] = new Field();
            }
        }
    }
}
