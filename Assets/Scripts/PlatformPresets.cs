using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPresets
{
    public List<int[,]> presets;
    public PlatformPresets()
    {
        presets = new List<int[,]>();
        int[,] preset;

        preset = new int[1,2] { { 1, 1 } };
        presets.Add(preset);

        preset = new int[1,3] { { 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[1,4] { { 1, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[1,5] { { 1, 1, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[2,2] { { 1, 1 }, { 0, 1 } };
        presets.Add(preset);

        preset = new int[2,2] { { 1, 1 }, { 1, 0 } };
        presets.Add(preset);

        preset = new int[2, 3] { { 1, 1, 1 }, { 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[2, 3] { { 1, 1, 1 }, { 0, 1, 1 } };
        presets.Add(preset);

        preset = new int[2, 3] { { 1, 1, 1 }, { 1, 1, 0 } };
        presets.Add(preset);

        preset = new int[3, 2] { { 1, 1 }, { 1, 1 }, { 1, 1 }};
        presets.Add(preset);

        preset = new int[3, 2] { { 1, 1 }, { 1, 1 }, { 0, 1 } };
        presets.Add(preset);

        preset = new int[3, 2] { { 1, 1 }, { 1, 1 }, { 1, 0 } };
        presets.Add(preset);

        preset = new int[3, 2] { { 1, 1 }, { 0, 1 }, { 0, 1 } };
        presets.Add(preset);

        preset = new int[3, 2] { { 1, 1 }, { 1, 0 }, { 1, 0 } };
        presets.Add(preset);

        preset = new int[3, 3] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[3, 3] { { 1, 1, 1 }, { 1, 1, 1 }, { 0, 1, 1 } };
        presets.Add(preset);

        preset = new int[3, 3] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 0 } };
        presets.Add(preset);

        preset = new int[3, 3] { { 1, 1, 1 }, { 1, 1, 1 }, { 0, 0, 1 } };
        presets.Add(preset);

        preset = new int[3, 3] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 0, 0 } };
        presets.Add(preset);

        preset = new int[3, 3] { { 1, 1, 1 }, { 0, 1, 1 }, { 0, 1, 1 } };
        presets.Add(preset);

        preset = new int[3, 3] { { 1, 1, 1 }, { 0, 1, 1 }, { 1, 1, 0 } };
        presets.Add(preset);

        preset = new int[2, 4] { { 1, 1, 1, 1 }, { 1, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[2, 4] { { 1, 1, 1, 1 }, { 0, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[2, 4] { { 1, 1, 1, 1 }, { 1, 1, 1, 0 } };
        presets.Add(preset);

        preset = new int[2, 4] { { 1, 1, 1, 1 }, { 0, 0, 1, 1 } };
        presets.Add(preset);

        preset = new int[2, 4] { { 1, 1, 1, 1 }, { 1, 1, 0, 0 } };
        presets.Add(preset);

        preset = new int[3, 4] { { 1, 1, 1, 1 }, { 1, 1, 1, 1 }, { 1, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[3, 4] { { 1, 1, 1, 1 }, { 1, 1, 1, 1 }, { 0, 0, 1, 1 } };
        presets.Add(preset);

        preset = new int[3, 4] { { 1, 1, 1, 1 }, { 1, 1, 1, 1 }, { 1, 1, 0, 0 } };
        presets.Add(preset);

        preset = new int[3, 4] { { 1, 1, 1, 1 }, { 0, 1, 1, 1 }, { 0, 0, 1, 1 } };
        presets.Add(preset);

        preset = new int[3, 4] { { 1, 1, 1, 1 }, { 1, 1, 1, 0 }, { 1, 1, 0, 0 } };
        presets.Add(preset);

        preset = new int[4, 3] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[4, 3] { { 1, 1, 1 }, { 1, 1, 1 }, { 0, 1, 1 }, { 0, 0, 1 } };
        presets.Add(preset);

        preset = new int[4, 3] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 0 }, { 1, 0, 0 } };
        presets.Add(preset);

        preset = new int[4, 3] { { 1, 1, 1 }, { 1, 1, 1 }, { 0, 0, 1 }, { 0, 0, 1 } };
        presets.Add(preset);

        preset = new int[4, 3] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 0, 0 }, { 1, 0, 0 } };
        presets.Add(preset);

        preset = new int[4, 4] { { 1, 1, 1, 1 }, { 1, 1, 1, 1 }, { 1, 1, 1, 1 }, { 1, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[4, 4] { { 1, 1, 1, 1 }, { 1, 1, 1, 1 }, { 1, 1, 0, 0 }, { 1, 1, 0, 0 } };
        presets.Add(preset);

        preset = new int[4, 4] { { 1, 1, 1, 1 }, { 1, 1, 1, 1 }, { 0, 0, 1, 1 }, { 0, 0, 1, 1 } };
        presets.Add(preset);

        preset = new int[4, 4] { { 1, 1, 1, 1 }, { 1, 1, 1, 0 }, { 1, 1, 0, 0 }, { 1, 0, 0, 0 } };
        presets.Add(preset);

        preset = new int[4, 4] { { 1, 1, 1, 1 }, { 0, 1, 1, 1 }, { 0, 0, 1, 1 }, { 0, 0, 0, 1 } };
        presets.Add(preset);

        preset = new int[2, 5] { { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[2, 5] { { 1, 1, 1, 1, 1 }, { 0, 0, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[2, 5] { { 1, 1, 1, 1, 1 }, { 1, 1, 1, 0, 0 } };
        presets.Add(preset);

        preset = new int[3, 5] { { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[3, 5] { { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 0, 0, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[3, 5] { { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 0, 0 } };
        presets.Add(preset);

        preset = new int[3, 5] { { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 0, 0, 0, 1, 1 } };
        presets.Add(preset);

        preset = new int[3, 5] { { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 0, 0, 0 } };
        presets.Add(preset);

        preset = new int[3, 5] { { 1, 1, 1, 1, 1 }, { 0, 0, 1, 1, 1 }, { 0, 0, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[3, 5] { { 1, 1, 1, 1, 1 }, { 1, 1, 1, 0, 0 }, { 1, 1, 1, 0, 0 } };
        presets.Add(preset);

        preset = new int[3, 5] { { 1, 1, 1, 1, 1 }, { 0, 0, 1, 1, 1 }, { 0, 0, 0, 1, 1 } };
        presets.Add(preset);

        preset = new int[3, 5] { { 1, 1, 1, 1, 1 }, { 1, 1, 1, 0, 0 }, { 1, 1, 0, 0, 0 } };
        presets.Add(preset);

        preset = new int[5, 3] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[5, 3] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 }, { 0, 1, 1 }, { 0, 1, 1 } };
        presets.Add(preset);

        preset = new int[5, 3] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 0 }, { 1, 1, 0 } };
        presets.Add(preset);

        preset = new int[4, 5] { { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[4, 5] { { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 0, 0 }, { 1, 1, 1, 0, 0 } };
        presets.Add(preset);

        preset = new int[4, 5] { { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 0, 0, 1, 1, 1 }, { 0, 0, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[4, 5] { { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 0, 0, 0 }, { 1, 1, 0, 0, 0 } };
        presets.Add(preset);

        preset = new int[4, 5] { { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 0, 0, 0, 1, 1 }, { 0, 0, 0, 1, 1 } };
        presets.Add(preset);

        preset = new int[4, 5] { { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 0, 0 }, { 1, 1, 0, 0, 0 } };
        presets.Add(preset);

        preset = new int[4, 5] { { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 0, 0, 1, 1, 1 }, { 0, 0, 0, 1, 1 } };
        presets.Add(preset);

        preset = new int[5, 4] { { 1, 1, 1, 1 }, { 1, 1, 1, 1 }, { 1, 1, 1, 1 }, { 1, 1, 1, 1 }, { 1, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[5, 4] { { 1, 1, 1, 1 }, { 1, 1, 1, 1 }, { 1, 1, 1, 1 }, { 1, 1, 0, 0 }, { 1, 1, 0, 0 } };
        presets.Add(preset);

        preset = new int[5, 4] { { 1, 1, 1, 1 }, { 1, 1, 1, 1 }, { 1, 1, 1, 1 }, { 0, 0, 1, 1 }, { 0, 0, 1, 1 } };
        presets.Add(preset);

        preset = new int[5, 5] { { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 0, 0, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[5, 5] { { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 0, 0 } };
        presets.Add(preset);

        preset = new int[5, 5] { { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 0, 0, 1, 1, 1 }, { 0, 0, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[5, 5] { { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 0, 0 }, { 1, 1, 1, 0, 0 } };
        presets.Add(preset);

        preset = new int[5, 5] { { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 0, 0, 1, 1, 1 }, { 0, 0, 1, 1, 1 }, { 0, 0, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[5, 5] { { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 0, 0 }, { 1, 1, 1, 0, 0 }, { 1, 1, 1, 0, 0 } };
        presets.Add(preset);

        preset = new int[5, 5] { { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 0, 0, 1, 1, 1 }, { 0, 0, 0, 1, 1 } };
        presets.Add(preset);

        preset = new int[5, 5] { { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 0, 0 }, { 1, 1, 0, 0, 0 } };
        presets.Add(preset);

        preset = new int[5, 5] { { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 0, 0, 1, 1, 1 }, { 0, 0, 0, 1, 1 }, { 0, 0, 0, 1, 1 } };
        presets.Add(preset);

        preset = new int[5, 5] { { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 0, 0 }, { 1, 1, 0, 0, 0 }, { 1, 1, 0, 0, 0 } };
        presets.Add(preset);

        preset = new int[5, 5] { { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 0, 0, 0, 1, 1 }, { 0, 0, 0, 1, 1 }, { 0, 0, 0, 1, 1 } };
        presets.Add(preset);

        preset = new int[5, 5] { { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 0, 0, 0 }, { 1, 1, 0, 0, 0 }, { 1, 1, 0, 0, 0 } };
        presets.Add(preset);

        preset = new int[3, 6] { { 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 0, 0 } };
        presets.Add(preset);

        preset = new int[3, 6] { { 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1 }, { 0, 0, 1, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[3, 7] { { 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 0, 0, 0 } };
        presets.Add(preset);

        preset = new int[3, 7] { { 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1 }, { 0, 0, 0, 1, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[3, 8] { { 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 0, 0, 0 } };
        presets.Add(preset);

        preset = new int[3, 8] { { 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1 }, { 0, 0, 0, 1, 1, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[3, 9] { { 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 0, 0, 0, 0 } };
        presets.Add(preset);

        preset = new int[3, 9] { { 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 0, 0, 0, 0, 1, 1, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[3, 10] { { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 } };
        presets.Add(preset);

        preset = new int[3, 10] { { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[3, 6] { { 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 0, 0 } };
        presets.Add(preset);

        preset = new int[3, 6] { { 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1 }, { 0, 0, 1, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[3, 7] { { 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 0, 0, 0 } };
        presets.Add(preset);

        preset = new int[3, 7] { { 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1 }, { 0, 0, 0, 1, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[3, 8] { { 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 0, 0, 0 } };
        presets.Add(preset);

        preset = new int[3, 8] { { 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1 }, { 0, 0, 0, 1, 1, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[3, 9] { { 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 0, 0, 0, 0 } };
        presets.Add(preset);

        preset = new int[3, 9] { { 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 0, 0, 0, 0, 1, 1, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[3, 10] { { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 } };
        presets.Add(preset);

        preset = new int[3, 10] { { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[3, 6] { { 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 0, 0 } };
        presets.Add(preset);

        preset = new int[3, 6] { { 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1 }, { 0, 0, 1, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[3, 7] { { 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 0, 0, 0 } };
        presets.Add(preset);

        preset = new int[3, 7] { { 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1 }, { 0, 0, 0, 1, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[3, 8] { { 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 0, 0, 0 } };
        presets.Add(preset);

        preset = new int[3, 8] { { 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1 }, { 0, 0, 0, 1, 1, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[3, 9] { { 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 0, 0, 0, 0 } };
        presets.Add(preset);

        preset = new int[3, 9] { { 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 0, 0, 0, 0, 1, 1, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[3, 10] { { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 } };
        presets.Add(preset);

        preset = new int[3, 10] { { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[3, 6] { { 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 0, 0 } };
        presets.Add(preset);

        preset = new int[3, 6] { { 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1 }, { 0, 0, 1, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[3, 7] { { 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 0, 0, 0 } };
        presets.Add(preset);

        preset = new int[3, 7] { { 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1 }, { 0, 0, 0, 1, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[3, 8] { { 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 0, 0, 0 } };
        presets.Add(preset);

        preset = new int[3, 8] { { 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1 }, { 0, 0, 0, 1, 1, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[3, 9] { { 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 0, 0, 0, 0 } };
        presets.Add(preset);

        preset = new int[3, 9] { { 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 0, 0, 0, 0, 1, 1, 1, 1, 1 } };
        presets.Add(preset);

        preset = new int[3, 10] { { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 } };
        presets.Add(preset);

        preset = new int[3, 10] { { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1 } };
        presets.Add(preset);
    }
}
