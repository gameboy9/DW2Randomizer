// Ultimate note 1:  Map pointers = DDA5-DFA6, Actual map = DFA7-F907
// 0 = Special square
// 00 = UL Castle
// 01 = 1 Grass
// 02 = 1 desert
// 03 = 1 trees
// 04 = 1 water
// 05 = 1 mountain
// 06 = 1 long grass
// 07 = 1 hill
// 08 = 1 swamp
// 09 = Bridge horizontal
// 0A = Tower
// 0B = Monolith
// 0C = Cave
// 0D = Bridge vertical
// 0E = Village left
// 0F = Village right
// 10 = DL Castle
// 11 = DR Castle
// 12 = UR Castle
// 13 = Shallow water (Shoals)
// 14 = Water w/ NE coastline
// 15 = Water with N coastline
// 16 = Water w/ NW coastline
// 17 = Water w/ W coastline
// 18 = Water w/ E coastline
// 19 = Water w/ SW coastline
// 1A = Water w/ S coastline
// 1B = Water w/ SE coastline
// 1C = Water w/ NW coast corner
// 1D = Water w/ NE coast corner
// 1E = Water w/ SE coast corner
// 1F = Water w/ SW coast corner
// 2 = Grass
// 4 = Desert
// 6 = Trees
// 8 = Water
// A = Mountains
// C = Short Grass
// E = Hills
// 3/5/7/9/B/D/F = Add 16 to the quantity
//
// 0016 - Horizontal
// 0017 - Vertical
// 0031 - Map #
// 003B - Map bank?
// 003C - Map bank?
// A27E - World Map warps from town to world
// A32C - Stairs from
// BD13 - Upstairs/Travel doors
//
// Eye of Malroth - 1969C - Map #, 196A7 & 196AB = Horizontal Range, 196B1 & 196B4 = Vertical Range
// Cave To Rhone - E008 - Horizontal, E00E - Vertical
// Moon Fragment - 198EF & 198F3 = Horizontal Range, 198F9 & 198FD = Vertical Range
// Watergate Key - DFF0 - Horizontal, DFF6 & DFFA - Vertical Range, E086 - Replacement tile
// Moon Fragment - 198EF & 198F3 = Horizontal Range, 198F9 & 198FD = Vertical Range
// Sea Cave - E144 & E148 = Horizontal Range, E14E & E152 = Vertical Range, E158 - Shallow Water (make sure # is less than this to maintain!), E15C - tile to replace
//
// ORDER:  Map, Horizontal, Vertical
// Starts at 0xa28e +/-
// int byteToUse = (lnI == 0 ? 0xa28f : lnI == 1 ? 0xa295 : lnI == 2 ? 0xa29b : lnI == 3 ? 0xa2a1 : lnI == 4 ? 0xa2a4 : lnI == 5 ? 0xa2e9 : 0xa2b3);
// Midenhall -> World - A27E-80 (and A2FD-F)
// int byteToUse = (lnI == 0 ? 0xa292 : lnI == 1 ? 0xa298 : lnI == 2 ? 0xa29e : lnI == 3 ? 0xa2a7 : lnI == 4 ? 0xa2aa : lnI == 5 ? 0xa2ad : 0xa2b0);
// Leftwyne -> World - A281-3
// Cannock -> World - A284-6
// Hamlin -> World - A287-9
// Moonbrooke Castle -> World - A28A-C
// Lianport -> World - A28D-F
// Tantegel -> World - A290-2
// Osterfair -> World - A293-5
// Zahan -> World - A296-8
// Tuhn -> World - A299-B
// Wellgarth -> World - A29C-E
// Beran -> World - A29F-A1 (Return point:  A276-8)
// Hargon's Castle -> World - A2A2-4
// Midenhall island shrine -> World - A2A5-7
// Shrine SW of Cannock -> World - A2A8-A
// Shrine N of Lianport -> World - A2AB-D
// Rainbow drop shrine -> World - A2AE-B0
// Beran Shrine -> World - A2B1-3
// Fire Shrine -> World - A2B4-6
// Rhone Shrine -> World - A2B7-9
// Moonbrooke Shrine (west) -> World - A2BA-C
// Moonbrooke Shrine (east) -> World - A2BD-F
// Shrine before Rhone -> World - A2C0-2
// Shrine S of Midenhall -> World - A2C3-5
// Rubiss Shrine -> World - A2C6-8
// Zahan Shrine -> World - A2C9-B
// Cave to Hamlin (wrong way) -> World - A2CC-E
// Lake cave -> World - A2CF-D1
// Sea Cave -> World - A2D2-4
// Wind Tower -> World - A2D5-7
// Charlock Castle -> World - A2D8-A
// Lighthouse -> World - A2DB-D
// Rhone Cave (south) -> World - A2DE-E0
// Moon tower -> World - A2E1-3
// Dragon's Horn (south) -> World - A2E4-6
// Dragon's Horn (north) -> World - A2E7-9
// Swamp Cave (north) -> World - A2EA-C
// Spring Of Bravery -> World - A2ED-F
// Cave to Hamlin (right way) -> World - A2F3-5
// Rhone Cave (north) -> World - A2F6-8
// Swamp Cave (south) -> World - A2F9-B
// Mirror Of Ra Location - 9F08, 9F09, earning 9F0A
// Treasures location - 9F0C, 9F0D, earning 9F0E
// World Leaf location - 9F10, 9F11 (0x19F20-1, earning 0x19F22)
using System;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using System.Collections.Generic;

namespace DW2Randomizer
{
    public partial class Form1 : Form
    {
        byte[] romData;
        byte[] romData2;
        int[] maxPower = { 93, 50, 30, 20 };
        int randomLevel = 0;
        bool loading = true;
        int[,] map = new int[256, 256];
        int[,] island = new int[256, 256];

        public Form1()
        {
            InitializeComponent();
        }

        private bool randomizeMapv3(Random r1)
        {
            for (int lnI = 0; lnI < 256; lnI++)
                for (int lnJ = 0; lnJ < 256; lnJ++)
                {
                    if (chkSmallMap.Checked && (lnI >= 128 || lnJ >= 128))
                    {
                        map[lnI, lnJ] = 0x05;
                        island[lnI, lnJ] = 200;
                    }
                    else
                    {
                        map[lnI, lnJ] = 0x04;
                        island[lnI, lnJ] = -1;
                    }
                }

            for (int lnI = 0; lnI < 1; lnI++)
            {
                int x = 0;
                int y = 0;
                bool legal = false;
                while (!legal)
                {
                    int startX = (r1.Next() % (chkSmallMap.Checked ? 128 : 256));
                    int startY = (r1.Next() % (chkSmallMap.Checked ? 128 : 256));
                    if (mapLegalStart(startY, startX, (lnI == 5 ? 15 : 10)))
                    {
                        map[startY, startX] = 0x01;
                        island[startY, startX] = lnI;
                        legal = true;
                        x = startX;
                        y = startY;
                    }
                }

                int islandSize = (r1.Next() % 20000) + 30000; // (lnI == 0 ? 1500 : lnI == 1 ? 2500 : lnI == 2 ? 1500 : lnI == 3 ? 1500 : lnI == 4 ? 5000 : 5000);
                islandSize /= (chkSmallMap.Checked ? 4 : 1);
                int mapTries = 0;
                int tiles = 0;
                int terrain = 0;
                for (int lnJ = 0; lnJ < islandSize; lnJ++)
                {
                    bool legal2 = false;
                    while (!legal2)
                    {
                        // pick a direction at random
                        // Now pepper the land with random terrain

                        int dir = (r1.Next() % 4);
                        if (mapLegalPlot(y, x, dir, lnI))
                        {
                            int maxLimit = (chkSmallMap.Checked ? 126 : 254);
                            if (dir == 0 && y == 1) continue;
                            if (dir == 1 && x == maxLimit) continue;
                            if (dir == 2 && y == maxLimit) continue;
                            if (dir == 3 && x == 1) continue;
                            if (dir == 0)
                                y = (y == 0 ? 255 : y - 1);
                            else if (dir == 1)
                                x = (x == 255 ? 0 : x + 1);
                            if (dir == 2)
                                y = (y == 255 ? 0 : y + 1);
                            if (dir == 3)
                                x = (x == 0 ? 255 : x - 1);

                            if (map[y, x] != 0x04) { lnJ--; mapTries++; }
                            if (tiles == 0) {
                                tiles = r1.Next() % 500;
                                terrain = 4;
                                while (terrain == 4 || terrain == 5)
                                    terrain = r1.Next() % 7 + 1;
                                terrain = terrain;
                            }
                            if (map[y, x] != 0x0a)
                                map[y, x] = terrain; // (lnI == 0 ? 0x01 : lnI == 1 ? 0x06 : lnI == 2 ? 0x03 : lnI == 3 ? 0x02 : lnI == 4 ? 0x07 : 0x05);
                            island[y, x] = lnI;
                            tiles--;
                            legal2 = true;
                        }
                        else
                        {
                            mapTries++;
                        }
                    }
                    if (mapTries >= 50000)
                    {
                        // Retry the map again.
                        return false;
                    }
                }
            }
            
            int lakeNumber = 256;
            int maxPlots = 0;
            int maxLake = 0;
            int lastIsland = 0;
            for (int lnI = 0; lnI < 256; lnI++)
                for (int lnJ = 0; lnJ < 256; lnJ++)
                {
                    if (island[lnI, lnJ] == -1)
                    {
                        int plots = lakePlot(lakeNumber, lnI, lnJ);
                        if (plots <= 10)
                        {
                            plots = lakePlot(lakeNumber, lnI, lnJ, true, lastIsland);
                        }
                        else if (plots > maxPlots)
                        {
                            maxPlots = plots;
                            maxLake = lakeNumber;
                        }
                        lakeNumber++;
                    }
                    else
                    {
                        lastIsland = island[lnI, lnJ];
                    }
                }

            // Remove tiny gaps
            //for (int lnI = 3; lnI < (chkSmallMap.Checked ? 128 : 256); lnI++)
            //    for (int lnJ = 3; lnJ < (chkSmallMap.Checked ? 128 : 256); lnJ++)
            //    {
            //        int lowY = (lnI - 1 == -1 ? 255 : lnI - 1);
            //        int lowY2 = (lowY - 1 == -1 ? 255 : lowY - 1);
            //        int lowY3 = (lowY2 - 1 == -1 ? 255 : lowY2 - 1);
            //        int lowX = (lnJ - 1 == -1 ? 255 : lnJ - 1);
            //        int lowX2 = (lowX - 1 == -1 ? 255 : lowX - 1);
            //        int lowX3 = (lowX2 - 1 == -1 ? 255 : lowX2 - 1);

            //        int islandUsed = island[lnI, lnJ];
            //        if (islandUsed <= 5 && map[lowY, lnJ] == 0x04 && island[lowY2, lnJ] == islandUsed)
            //        {
            //            island[lowY, lnJ] = islandUsed;
            //            map[lowY, lnJ] = (islandUsed == 0 ? 0x01 : islandUsed == 1 ? 0x06 : islandUsed == 2 ? 0x03 : islandUsed == 3 ? 0x02 : islandUsed == 4 ? 0x07 : 0x05);
            //        }
            //        if (islandUsed <= 5 && map[lowY, lnJ] == 0x04 && map[lowY2, lnJ] == 0x04 && island[lowY3, lnJ] == islandUsed)
            //        {
            //            island[lowY, lnJ] = island[lowY2, lnJ] = islandUsed;
            //            map[lowY, lnJ] = map[lowY2, lnJ] = (islandUsed == 0 ? 0x01 : islandUsed == 1 ? 0x06 : islandUsed == 2 ? 0x03 : islandUsed == 3 ? 0x02 : islandUsed == 4 ? 0x07 : 0x05);
            //        }

            //        if (islandUsed <= 5 && map[lnI, lowX] == 0x04 && island[lnI, lowX2] == islandUsed)
            //        {
            //            island[lnI, lowX] = islandUsed;
            //            map[lnI, lowX] = (islandUsed == 0 ? 0x01 : islandUsed == 1 ? 0x06 : islandUsed == 2 ? 0x03 : islandUsed == 3 ? 0x02 : islandUsed == 4 ? 0x07 : 0x05);
            //        }
            //        if (islandUsed <= 5 && map[lnI, lowX] == 0x04 && map[lnI, lowX2] == 0x04 && island[lnI, lowX3] == islandUsed)
            //        {
            //            island[lnI, lowX] = island[lnI, lowX2] = islandUsed;
            //            map[lnI, lowX] = map[lnI, lowX2] = (islandUsed == 0 ? 0x01 : islandUsed == 1 ? 0x06 : islandUsed == 2 ? 0x03 : islandUsed == 3 ? 0x02 : islandUsed == 4 ? 0x07 : 0x05);
            //        }
            //    }

            //// Make sure Hargon's Castle is thick enough so we can have a path...
            //for (int lnI = 2; lnI < (chkSmallMap.Checked ? 128 : 256); lnI++)
            //    for (int lnJ = 2; lnJ < (chkSmallMap.Checked ? 128 : 256); lnJ++)
            //    {
            //        int lowY = (lnI - 1 == -1 ? 255 : lnI - 1);
            //        int lowY2 = (lowY - 1 == -1 ? 255 : lowY - 1);
            //        int lowX = (lnJ - 1 == -1 ? 255 : lnJ - 1);
            //        int lowX2 = (lowX - 1 == -1 ? 255 : lowX - 1);
            //        if (island[lnI, lnJ] == 5)
            //        {
            //            map[lnI, lowX] = map[lnI, lowX2] = map[lowY, lowX2] = map[lowY, lowX] = map[lowY, lnJ] = map[lowY2, lnJ] = map[lowY2, lowX] = map[lowY2, lowX2] = 0x05;
            //            island[lnI, lowX] = island[lnI, lowX2] = island[lowY, lowX2] = island[lowY, lowX] = island[lowY, lnJ] = island[lowY2, lnJ] = island[lowY2, lowX] = island[lowY2, lowX2] = 5;
            //        }
            //    }

            //// Make sure Hargon's Castle is blocked off by mountains
            //for (int lnI = 1; lnI < (chkSmallMap.Checked ? 127 : 255); lnI++)
            //    for (int lnJ = 1; lnJ < (chkSmallMap.Checked ? 127 : 255); lnJ++)
            //    {
            //        int lowY = (lnI - 1 == -1 ? 255 : lnI - 1);
            //        int highY = (lnI + 1 == 256 ? 0 : lnI + 1);
            //        int lowX = (lnJ - 1 == -1 ? 255 : lnJ - 1);
            //        int highX = (lnJ + 1 == 256 ? 0 : lnJ + 1);
            //        if (island[lnI, lnJ] == 5)
            //        {
            //            if (island[lowY, lnJ] != maxLake && island[highY, lnJ] != maxLake && island[lnI, lowX] != maxLake && island[lnI, highX] != maxLake)
            //            {
            //                map[lnI, lnJ] = 0x07;
            //                if (map[lowY, lnJ] == 0x04)
            //                {
            //                    map[lnI, lnJ] = 0x07;
            //                    island[lnI, lnJ] = 5;
            //                }
            //            }
            //        }
            //    }

            // Moon Tower

            //bool moonLegal = false;
            //while (!moonLegal)
            //{
            //    int x = r1.Next() % (chkSmallMap.Checked ? 117 : 245);
            //    int y = r1.Next() % (chkSmallMap.Checked ? 117 : 245);
            //    if (validPlot(y, x, 1, 1, new int[] { 0 }))
            //    {
            //        for (int lnJ = 1; lnJ <= 5; lnJ++)
            //            for (int lnK = 1; lnK <= 5; lnK++)
            //            {
            //                if (lnJ == 1 || lnJ == 5 || lnK == 1 || lnK == 5)
            //                {
            //                    map[y + lnJ, x + lnK] = 0x04;
            //                    //romData[0xa2ef] = (byte)(x + lnK);
            //                    //romData[0xa2f0] = (byte)(y + lnJ);
            //                    //romData[0x3e018] = (byte)(x + lnK);
            //                    //romData[0x3e01e] = (byte)(y + lnJ);
            //                }
            //                else if (((lnJ == 2 || lnJ == 4) && lnK >= 2 && lnK <= 4) || (lnJ == 3 && (lnK == 2 || lnK == 4)))
            //                    map[y + lnJ, x + lnK] = 0x01;
            //                else if (lnJ == 3 && lnK == 3)
            //                {
            //                    map[y + lnJ, x + lnK] = 0x0a;
            //                    // Place tower location into the ROM data now.
            //                    romData[0xa2f2] = (byte)(x + lnK);
            //                    romData[0xa2f3] = (byte)(y + lnJ);
            //                }
            //                // Also need to update the ROM to indicate the Rhone Cave location.
            //                //romData[0x196a7] = (byte)x;
            //                //romData[0x196ab] = (byte)(x + 8);
            //                //romData[0x196b1] = (byte)y;
            //                //romData[0x196b5] = (byte)(y + 6);
            //            }

            //        for (int lnJ = 6; lnJ <= 8; lnJ++)
            //        {
            //            map[y + 1, x + lnJ] = 0x05;
            //            map[y + 2, x + lnJ] = 0x02;
            //            map[y + 3, x + lnJ] = 0x05;
            //        }

            //        // Place tower location into the ROM data now.
            //        romData[0x3dffe] = 0x13;
            //        romData[0x3e004] = 0x12;
            //        romData[0x3e000] = (byte)(y + 2);
            //        romData[0x3e006] = (byte)(x + 5);
            //        romData[0x3e00a] = (byte)(x + 9);

            //        int lnRiver = 9;
            //        while (island[y + 2, x + lnRiver] != maxLake)
            //        {
            //            map[y + 2, x + lnRiver] = 0x04;
            //            lnRiver++;
            //        }

            //        moonLegal = true;
            //    }
            //}

            //bool seaLegal = false;
            //while (!seaLegal)
            //{
            //    int x = r1.Next() % (chkSmallMap.Checked ? 123 : 251);
            //    int y = r1.Next() % (chkSmallMap.Checked ? 121 : 249);
            //    if (validPlot(y, x, 1, 1, new int[] { 0 }))
            //    {
            //        if (validSeaPlot(y, x, maxLake, 12))
            //        {
            //            List<int> seaCaveShoals = new List<int> { 1, 2, 3, 5, 6, 8, 9, 10, 14, 15, 19, 20, 24, 25, 26, 28, 29, 31, 32, 33 };
            //            List<int> seaCaveMountains = new List<int> { 7, 11, 12, 13, 16, 18 };
            //            int seaCaveCave = 17;
            //            int lnTileCounter = 0;

            //            for (int lnJ = 0; lnJ < 7; lnJ++)
            //                for (int lnK = 0; lnK < 5; lnK++)
            //                {
            //                    if (seaCaveMountains.Contains(lnTileCounter)) map[y + lnJ, x + lnK] = 0x05;
            //                    else if (seaCaveShoals.Contains(lnTileCounter)) map[y + lnJ, x + lnK] = 0x13;
            //                    else if (seaCaveCave == lnTileCounter)
            //                    {
            //                        map[y + lnJ, x + lnK] = 0x0c;
            //                        // Also need to update the ROM to indicate where the Sea Cave is in case the Moon Fragment was used.
            //                        romData[0xa2e3] = (byte)(x + lnK);
            //                        romData[0xa2e4] = (byte)(y + lnJ);

            //                        romData[0x198ef] = romData[0x3e154] = (byte)(x - 2);
            //                        romData[0x198f3] = romData[0x3e158] = (byte)(x + 5 + 2);
            //                        romData[0x198f9] = romData[0x3e15e] = (byte)(y - 2);
            //                        romData[0x198fd] = romData[0x3e162] = (byte)(y + 7 + 2);
            //                    }
            //                    lnTileCounter++;
            //                }
            //            seaLegal = true;
            //        }
            //    }
            //}

            bool treeLegal = false;
            while (!treeLegal)
            {
                int x = r1.Next() % (chkSmallMap.Checked ? 121 : 249);
                int y = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                if (validPlot(y, x, 5, 5, new int[] { 0 }))
                {
                    map[y + 1, x + 1] = 0x05;
                    map[y + 1, x + 2] = 0x05;
                    map[y + 1, x + 3] = 0x05;
                    map[y + 2, x + 1] = 0x05;
                    map[y + 2, x + 2] = 0x03;
                    map[y + 2, x + 3] = 0x05;
                    map[y + 3, x + 1] = 0x05;
                    map[y + 3, x + 2] = 0x03;
                    map[y + 3, x + 3] = 0x05;
                    // Also need to update the ROM to indicate the World Tree location.
                    romData[0x19f20] = (byte)(x + 2);
                    romData[0x19f21] = (byte)(y + 2);

                    treeLegal = true;
                }
            }

            //bool rubissLegal = false;
            //while (!rubissLegal)
            //{
            //    int rubissX = r1.Next() % (chkSmallMap.Checked ? 128 : 256);
            //    int rubissY = r1.Next() % (chkSmallMap.Checked ? 128 : 256);
            //    if (mapLegalStart(rubissY, rubissX, 4, 256))
            //    {
            //        if (validSeaPlot(rubissY, rubissX, maxLake))
            //        {
            //            map[rubissY, rubissX] = 0x0b;
            //            romData[0xa2d7] = (byte)rubissX;
            //            romData[0xa2d8] = (byte)rubissY;
            //            rubissLegal = true;
            //        }
            //    }
            //}

            bool treasuresLegal = false;
            while (!treasuresLegal)
            {
                int x = r1.Next() % (chkSmallMap.Checked ? 121 : 249);
                int y = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                if (validPlot(y, x, 5, 5, new int[] { 0 }))
                {
                    map[y + 1, x + 1] = 0x13;
                    map[y + 1, x + 2] = 0x13;
                    map[y + 1, x + 3] = 0x13;
                    map[y + 2, x + 1] = 0x13;
                    map[y + 2, x + 2] = 0x03;
                    map[y + 2, x + 3] = 0x13;
                    map[y + 3, x + 1] = 0x13;
                    map[y + 3, x + 2] = 0x03;
                    map[y + 3, x + 3] = 0x13;
                    // Also need to update the ROM to indicate the World Tree location.
                    romData[0x19f1c] = (byte)(x + 2);
                    romData[0x19f1d] = (byte)(y + 2);

                    treasuresLegal = true;
                }


                //int treasuresX = r1.Next() % (chkSmallMap.Checked ? 128 : 256);
                //int treasuresY = r1.Next() % (chkSmallMap.Checked ? 128 : 256);
                //if (mapLegalStart(treasuresY, treasuresX, 4, 256))
                //{
                //    if (validSeaPlot(treasuresY, treasuresX, maxLake))
                //    {
                //        map[treasuresY, treasuresX] = 0x13;
                //        // Also need to update the ROM to indicate the treasures spot.  (make sure it's vertical - 1!)
                //        romData[0x19f1c] = (byte)treasuresX;
                //        romData[0x19f1d] = (byte)(treasuresY + 1);
                //        treasuresLegal = true;
                //    }
                //}
            }

            //bool rhoneLegal = false;
            //while (!rhoneLegal)
            //{
            //    int x = r1.Next() % (chkSmallMap.Checked ? 120 : 248);
            //    int y = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
            //    if (validPlot(y, x, 6, 8, new int[] { 0 }))
            //    {
            //        int rhoneCaveX = r1.Next() % 6;
            //        for (int lnJ = 0; lnJ < 6; lnJ++)
            //            for (int lnK = 0; lnK < 8; lnK++)
            //            {
            //                if (lnJ == 1 && lnK == rhoneCaveX + 1)
            //                {
            //                    map[y + lnJ, x + lnK] = 0x0c;
            //                    romData[0xa2ef] = (byte)(x + lnK);
            //                    romData[0xa2f0] = (byte)(y + lnJ);
            //                    romData[0x3e018] = (byte)(x + lnK);
            //                    romData[0x3e01e] = (byte)(y + lnJ);
            //                }
            //                else if (lnK != 0 && lnK != 7 && lnJ == 1) map[y + lnJ, x + lnK] = 0x05;
            //                else if (lnK != 0 && lnK != 7 && lnJ > 1) map[y + lnJ, x + lnK] = 0x08;
            //                // Also need to update the ROM to indicate the Rhone Cave location.
            //                romData[0x196a7] = (byte)x;
            //                romData[0x196ab] = (byte)(x + 8);
            //                romData[0x196b1] = (byte)y;
            //                romData[0x196b5] = (byte)(y + 6);
            //            }

            //        rhoneLegal = true;
            //    }
            //}

            // Lake Cave

            //bool lakeLegal = false;
            //while (!lakeLegal)
            //{
            //    int x = r1.Next() % (chkSmallMap.Checked ? 128 : 256);
            //    int y = r1.Next() % (chkSmallMap.Checked ? 128 : 256);
            //    if (validPlot(y, x, 1, 1, new int[] { 0, 1 }))
            //    {
            //        map[y, x] = 0x0c;
            //        romData[0xa2e0] = (byte)(x);
            //        romData[0xa2e1] = (byte)(y);
            //        //for (int lnJ = 1; lnJ < 6; lnJ++)
            //        //    for (int lnK = 1; lnK < 6; lnK++)
            //        //    {
            //        //        if (lnJ == 1 || lnJ == 5)
            //        //        {
            //        //            if (lnK == 1 || lnK == 5)
            //        //                map[y + lnJ, x + lnK] = 0x06;
            //        //            else
            //        //                map[y + lnJ, x + lnK] = 0x04;
            //        //        }
            //        //        else if (lnJ == 2 || lnJ == 4)
            //        //            map[y + lnJ, x + lnK] = 0x04;
            //        //        else
            //        //        {
            //        //            if (lnK == 0 || lnK == 1)
            //        //                map[y + lnJ, x + lnK] = 0x09;
            //        //            else if (lnK == 3)
            //        //            {
            //        //                map[y + lnJ, x + lnK] = 0x0c;
            //        //                romData[0xa2e0] = (byte)(x + lnK);
            //        //                romData[0xa2e1] = (byte)(y + lnJ);
            //        //            }
            //        //            else
            //        //                map[y + lnJ, x + lnK] = 0x09;
            //        //        }
            //        //    }
            //        lakeLegal = true;
            //    }
            //}

            // Establish Midenhall location
            bool midenOK = false;
            int midenX = 300;
            int midenY = 300;
            while (!midenOK)
            {
                midenX = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                midenY = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                if (validPlot(midenY, midenX, 2, 2, new int[] { 0 }))
                    midenOK = true;
            }

            // Mirror Of Ra
            bool mirrorLegal = false;
            while (!mirrorLegal)
            {
                int x = 300;
                int y = 300;
                while (x >= (chkSmallMap.Checked ? 128 : 256) || y >= (chkSmallMap.Checked ? 128 : 256) || x <= 0 || y <= 0)
                {
                    x = r1.Next() % (chkSmallMap.Checked ? 28 : 56) - (chkSmallMap.Checked ? 14 : 28) + midenX;
                    y = r1.Next() % (chkSmallMap.Checked ? 28 : 56) - (chkSmallMap.Checked ? 14 : 28) + midenY;
                }
                if (validPlot(y, x, 5, 6, new int[] { 0 }))
                {
                    for (int lnJ = 1; lnJ < 4; lnJ++)
                        for (int lnK = 1; lnK < 5; lnK++)
                        {
                            if (lnJ == 1 || lnK == 1 || lnK == 4)
                                map[y + lnJ, x + lnK] = 0x13;
                            else
                                map[y + lnJ, x + lnK] = 0x08;
                        }
                    // Also need to update the ROM to indicate the new Mirror Of Ra search spot.
                    romData[0x19f18] = (byte)(x + 2);
                    romData[0x19f19] = (byte)(y + 2);

                    mirrorLegal = true;
                }
            }

            // We'll place all of the castles now.
            // Midenhall can go anywhere.  But Cannock has to be 48 squares or less away from there.
            // Don't place Hargon's Castle for now.  OK, place it for now.  But I may change my mind later.
            for (int lnI = 0; lnI < 7; lnI++)
            {
                int x = 300;
                int y = 300;
                if (lnI == 0) { x = midenX; y = midenY; }
                else if (lnI == 1)
                {
                    while (x >= (chkSmallMap.Checked ? 127 : 255) && y >= (chkSmallMap.Checked ? 127 : 255) || x <= 0 || y <= 0)
                    {
                        x = r1.Next() % (chkSmallMap.Checked ? 30 : 60) - (chkSmallMap.Checked ? 15 : 30) + midenX;
                        y = r1.Next() % (chkSmallMap.Checked ? 30 : 60) - (chkSmallMap.Checked ? 15 : 30) + midenY;
                    }
                } else
                {
                    x = r1.Next() % (chkSmallMap.Checked ? 125 : 253);
                    y = r1.Next() % (chkSmallMap.Checked ? 125 : 253);
                }

                if (validPlot(y, x, 2, 2, new int[] { 0 }))
                {
                    map[y + 0, x + 0] = 0x00;
                    map[y + 0, x + 1] = 0x12;
                    map[y + 1, x + 0] = 0x10;
                    map[y + 1, x + 1] = 0x11;

                    int byteToUse = (lnI == 0 ? 0xa28f : lnI == 1 ? 0xa295 : lnI == 2 ? 0xa29b : lnI == 3 ? 0xa2a1 : lnI == 4 ? 0xa2a4 : lnI == 5 ? 0xa2e9 : 0xa2b3);
                    romData[byteToUse] = (byte)(x + 1);
                    romData[byteToUse + 1] = (byte)(y + 1);
                    if (lnI == 5) // Charlock castle, out of order as far as byte sequence is concerned.
                    {
                        romData[0xa334] = (byte)(x);
                        romData[0xa335] = (byte)(y + 1);
                    }
                    else
                    {
                        romData[byteToUse + 0x7e] = (byte)(x);
                        romData[byteToUse + 1 + 0x7e] = (byte)(y + 1);
                    }
                    //if (lnI == 6)
                    //{
                    //    romData[0xa301] = (byte)(x);
                    //    romData[0xa302] = (byte)(y + 1);
                    //}

                    // Return points
                    if (lnI == 0 || lnI == 1 || lnI == 3 || lnI == 4)
                    {
                        int byteMultiplier = lnI - (lnI >= 3 ? 1 : 0);
                        romData[0xa27a + (3 * byteMultiplier)] = (byte)x;
                        if (map[y + 2, x] == 0x04)
                            romData[0xa27a + (3 * byteMultiplier) + 1] = (byte)(y + 2);
                        else
                            romData[0xa27a + (3 * byteMultiplier) + 1] = (byte)(y + 1);
                        shipPlacement(0x1bf84 + (2 * byteMultiplier), y, x, maxLake);
                    }
                }
                else
                    lnI--;
            }

            // Now we'll place all of the towns now.
            // Leftwyne must be 15 squares or less away from Midenhall.  Hamlin has to be 30 squares or less away from Midenhall.
            for (int lnI = 0; lnI < 7; lnI++)
            {
                int x = 300;
                int y = 300;
                if (lnI == 0)
                {
                    while (x >= (chkSmallMap.Checked ? 127 : 255) && y >= (chkSmallMap.Checked ? 127 : 255) || x <= 0 || y <= 0)
                    {
                        x = r1.Next() % (chkSmallMap.Checked ? 30 : 60) - (chkSmallMap.Checked ? 15 : 30) + midenX;
                        y = r1.Next() % (chkSmallMap.Checked ? 30 : 60) - (chkSmallMap.Checked ? 15 : 30) + midenY;
                    }
                }
                else if (lnI == 1)
                {
                    while (x >= (chkSmallMap.Checked ? 127 : 255) || y >= (chkSmallMap.Checked ? 127 : 255) || x <= 0 || y <= 0)
                    {
                        x = r1.Next() % (chkSmallMap.Checked ? 60 : 120) - (chkSmallMap.Checked ? 30 : 60) + midenX;
                        y = r1.Next() % (chkSmallMap.Checked ? 60 : 120) - (chkSmallMap.Checked ? 30 : 60) + midenY;
                    }
                }
                else
                {
                    x = r1.Next() % (chkSmallMap.Checked ? 125 : 253);
                    y = r1.Next() % (chkSmallMap.Checked ? 125 : 253);
                }

                if (validPlot(y, x, 1, 2, new int[] { 0 }))
                {
                    map[y, x + 0] = 0x0e;
                    map[y, x + 1] = 0x0f;

                    int byteToUse2 = (lnI == 0 ? 0xa292 : lnI == 1 ? 0xa298 : lnI == 2 ? 0xa29e : lnI == 3 ? 0xa2a7 : lnI == 4 ? 0xa2aa : lnI == 5 ? 0xa2ad : 0xa2b0);
                    romData[byteToUse2] = (byte)(x + 1);
                    romData[byteToUse2 + 1] = (byte)(y);
                    romData[byteToUse2 + 0x7e] = (byte)(x);
                    romData[byteToUse2 + 1 + 0x7e] = (byte)(y);

                    // Return points
                    if (lnI == 2)
                        shipPlacement(0x3d6be, y, x, maxLake);
                    // Return points
                    else if (lnI == 1)
                    {
                        romData[0xa27a + 18] = (byte)(x);
                        if (map[y + 1, x] == 0x04)
                            romData[0xa27a + 19] = (byte)(y);
                        else
                            romData[0xa27a + 19] = (byte)(y + 1);
                        shipPlacement(0x1bf84 + 12, y, x, maxLake);
                    }
                    else if (lnI == 6)
                    {
                        romData[0xa27a + 12] = (byte)(x);
                        if (map[y + 1, x] == 0x04)
                            romData[0xa27a + 13] = (byte)(y);
                        else
                            romData[0xa27a + 13] = (byte)(y + 1);
                        // We are placing the ship in both Beran and the Rhone Shrine at the same time.
                        shipPlacement(0x1bf84 + 8, y, x, maxLake);
                        shipPlacement(0x1bf84 + 10, y, x, maxLake);
                    }
                }
                else
                    lnI--;
            }

            // Then the monoliths.
            // All of these can go anywhere.
            for (int lnI = 0; lnI < 13; lnI++)
            {
                int x = r1.Next() % (chkSmallMap.Checked ? 125 : 253);
                int y = r1.Next() % (chkSmallMap.Checked ? 125 : 253);

                if (validPlot(y, x, 1, 1, new int[] { 0 }))
                {
                    map[y, x] = 0x0b;

                    int byteToUse2 = 0xa2b6 + (lnI * 3); // (lnI < 11 ? 0xa2b6 + (lnI * 3) : 0xa2da);
                    romData[byteToUse2] = (byte)(x);
                    romData[byteToUse2 + 1] = (byte)(y);

                    // Return points
                    if (lnI == 6)
                    {
                        romData[0xa27a + 15] = (byte)(x);
                        if (map[y + 1, x] == 0x04)
                            romData[0xa27a + 16] = (byte)(y);
                        else
                            romData[0xa27a + 16] = (byte)(y + 1);
                    }
                }
                else
                    lnI--;
            }

            // Then the caves.
            // Make sure the lake and spring cave is no more than 48 squares outside of Midenhall
            for (int lnI = 0; lnI < 9; lnI++)
            {
                int x = 300;
                int y = 300;
                if (lnI == 1 || lnI == 5)
                {
                    while (x >= (chkSmallMap.Checked ? 127 : 255) && y >= (chkSmallMap.Checked ? 127 : 255) || x <= 0 || y <= 0)
                    {
                        x = r1.Next() % (chkSmallMap.Checked ? 32 : 64) - (chkSmallMap.Checked ? 16 : 32) + midenX;
                        y = r1.Next() % (chkSmallMap.Checked ? 32 : 64) - (chkSmallMap.Checked ? 16 : 32) + midenY;
                    }
                }
                else
                {
                    x = r1.Next() % (chkSmallMap.Checked ? 125 : 253);
                    y = r1.Next() % (chkSmallMap.Checked ? 125 : 253);
                }

                if (validPlot(y, x, 1, 1, new int[] { 0 }))
                {
                    map[y, x] = 0x0c;

                    int byteToUse2 = (lnI == 0 ? 0xa2dd : lnI == 1 ? 0xa2e0 : lnI == 2 ? 0xa2e3 : lnI == 3 ? 0xa2ef : lnI == 4 ? 0xa2fb : lnI == 5 ? 0xa2fe : lnI == 6 ? 0xa304 : lnI == 7 ? 0xa307 : 0xa30a);
                    romData[byteToUse2] = (byte)x;
                    romData[byteToUse2 + 1] = (byte)(y);
                }
                else
                    lnI--;
            }

            // Finally the towers
            // Need to make sure the wind tower is no more than 64 squares outside of Midenhall
            for (int lnI = 0; lnI < 5; lnI++)
            {
                int x = 300;
                int y = 300;
                if (lnI == 0)
                {
                    while (x >= (chkSmallMap.Checked ? 127 : 255) && y >= (chkSmallMap.Checked ? 127 : 255) || x <= 0 || y <= 0)
                    {
                        x = r1.Next() % (chkSmallMap.Checked ? 28 : 14) - (chkSmallMap.Checked ? 14 : 28) + midenX;
                        y = r1.Next() % (chkSmallMap.Checked ? 28 : 14) - (chkSmallMap.Checked ? 14 : 28) + midenY;
                    }
                }
                else
                {
                    x = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                    y = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                }

                // Need to make sure it's a valid 7x7 plot due to dropping with the Cloak of wind...
                if (validPlot(y, x, 3, 3, new int[] { 0 }))
                {
                    map[y + 3, x + 3] = 0x0a;

                    int byteToUse2 = (lnI == 0 ? 0xa2e6 : lnI == 1 ? 0xa2ec : lnI == 2 ? 0xa2f2 : lnI == 3 ? 0xa2f5 : 0xa2f8);
                    romData[byteToUse2] = (byte)(x + 3);
                    romData[byteToUse2 + 1] = (byte)(y + 3);
                }
                else
                    lnI--;
            }

            // Need to change this where 5 zones out are safe, the rest are not.
            int[,] monsterZones = new int[16, 16];
            for (int lnI = 0; lnI < 16; lnI++)
                for (int lnJ = 0; lnJ < 16; lnJ++)
                    monsterZones[lnI, lnJ] = 0xff;

            int midenMZX = midenX / 16;
            int midenMZY = midenY / 16;

            for (int lnI = 0; lnI < 16; lnI++)
                for (int lnJ = 0; lnJ < 16; lnJ++)
                {
                    int mzY = lnI;
                    int mzX = lnJ;

                    if (midenMZX - mzX >= -1 && midenMZX - mzX <= 1 && midenMZY - mzY >= -1 && midenMZY - mzY <= 1)
                        monsterZones[mzY, mzX] = r1.Next() % 9;
                    else if (midenMZX - mzX >= -2 && midenMZX - mzX <= 2 && midenMZY - mzY >= -2 && midenMZY - mzY <= 2)
                        monsterZones[mzY, mzX] = r1.Next() % 18;
                    else if (midenMZX - mzX >= -4 && midenMZX - mzX <= 4 && midenMZY - mzY >= -4 && midenMZY - mzY <= 4)
                        monsterZones[mzY, mzX] = r1.Next() % 5 + 0x0d;
                    else
                    {
                        while (monsterZones[mzY, mzX] > 0x27 || (monsterZones[mzY, mzX] >= 0x1c && monsterZones[mzY, mzX] <= 0x1f))
                            monsterZones[mzY, mzX] = r1.Next() % 19 + 0x15;
                        if (monsterZones[mzY, mzX] == 0x26) monsterZones[mzY, mzX] = 0x39;
                        if (monsterZones[mzY, mzX] == 0x27) monsterZones[mzY, mzX] = 0x3b;
                    }
                    monsterZones[mzY, mzX] += (64 * (r1.Next() % 4));

                    //if (monsterZones[mzY, mzX] == 0xff)
                    //{

                    //    if (island[lnI, lnJ] == 0)
                    //        monsterZones[mzY, mzX] = r1.Next() % 9;
                    //    else if (island[lnI, lnJ] == 1)
                    //        monsterZones[mzY, mzX] = r1.Next() % 5 + 0x0d;
                    //    else if (island[lnI, lnJ] == 5)
                    //        monsterZones[mzY, mzX] = r1.Next() % 2 + 0x32;
                    //    else if (island[lnI, lnJ] >= 2 && island[lnI, lnJ] <= 4)
                    //    {
                    //        while (monsterZones[mzY, mzX] > 0x27 || (monsterZones[mzY, mzX] >= 0x1c && monsterZones[mzY, mzX] <= 0x1f))
                    //            monsterZones[mzY, mzX] = r1.Next() % 19 + 0x15;
                    //        if (monsterZones[mzY, mzX] == 0x26) monsterZones[mzY, mzX] = 0x39;
                    //        if (monsterZones[mzY, mzX] == 0x27) monsterZones[mzY, mzX] = 0x3b;
                    //    }

                    //    if (island[lnI, lnJ] >= 0 && island[lnI, lnJ] <= 5)
                    //        monsterZones[mzY, mzX] += (64 * (r1.Next() % 4));
                    //}
                }

            // Now let's enter all of this into the ROM...
            int lnPointer = 0x9f97;

            for (int lnI = 0; lnI <= 256; lnI++) // <---- There is a final pointer for lnI = 256, probably indicating the conclusion of the map.
            {
                romData[0xdda5 + (lnI * 2)] = (byte)(lnPointer % 256);
                romData[0xdda6 + (lnI * 2)] = (byte)(lnPointer / 256);

                int lnJ = 0;
                while (lnI < 256 && lnJ < 256)
                {
                    if (map[lnI, lnJ] >= 1 && map[lnI, lnJ] <= 7)
                    {
                        int tileNumber = 0;
                        int numberToMatch = map[lnI, lnJ];
                        while (lnJ < 256 && tileNumber < 32 && map[lnI, lnJ] == numberToMatch && tileNumber < 32)
                        {
                            tileNumber++;
                            lnJ++;
                        }
                        romData[lnPointer + 0x4010] = (byte)((0x20 * numberToMatch) + (tileNumber - 1));
                        lnPointer++;
                    }
                    else
                    {
                        romData[lnPointer + 0x4010] = (byte)map[lnI, lnJ];
                        lnPointer++;
                        lnJ++;
                    }
                }
            }
            //lnPointer = lnPointer;
            if (lnPointer >= 0xb8f7)
            {
                MessageBox.Show("WARNING:  The map might have taken too much ROM space...");
                // Might have to compress further to remove one byte stuff
                // Must compress the map by getting rid of further 1 byte lakes
            }

            if (chkSmallMap.Checked)
            {
                romData[0x10083] = 0x85;
                romData[0x10084] = 0xd5;
                romData[0x10085] = 0xa5;
                romData[0x10086] = 0x17;
                romData[0x10087] = 0x29;
                romData[0x10088] = 0x78;
                romData[0x10089] = 0x0a;
            }

            // Enter monster zones
            for (int lnI = 0; lnI < 16; lnI++)
                for (int lnJ = 0; lnJ < 16; lnJ++)
                {
                    if (monsterZones[lnI, lnJ] == 0xff)
                        monsterZones[lnI, lnJ] = (r1.Next() % 60) + ((r1.Next() % 4) * 64);
                    romData[0x103d6 + (lnI * 16) + lnJ] = (byte)monsterZones[lnI, lnJ];
                }

            return true;
        }

        private bool randomizeMapv2(Random r1)
        {
            for (int lnI = 0; lnI < 256; lnI++)
                for (int lnJ = 0; lnJ < 256; lnJ++)
                {
                    if (chkSmallMap.Checked && (lnI >= 128 || lnJ >= 128))
                    {
                        map[lnI, lnJ] = 0x05;
                        island[lnI, lnJ] = 200;
                    }
                    else
                    {
                        map[lnI, lnJ] = 0x04;
                        island[lnI, lnJ] = -1;
                    }
                }

            for (int lnI = 0; lnI < 6; lnI++)
            {
                int x = 0;
                int y = 0;
                bool legal = false;
                while (!legal)
                {
                    int startX = (r1.Next() % (chkSmallMap.Checked ? 128 : 256));
                    int startY = (r1.Next() % (chkSmallMap.Checked ? 128 : 256));
                    if (lnI == 3 && island[startY, startX] == 2)
                    {
                        int spaces = 0;
                        while (island[startY, startX] == 2 && startY >= 5) // spaces <= 256
                        {
                            startY--;
                            //startY = (startY == -1 ? 255 : startY);
                            spaces++;
                        }                            
                        if (mapLegalPlot(startY, startX, 0, lnI) && spaces >= 3 && spaces <= 240 && startY >= 5)
                        {
                            startY--;
                            int startY2 = (startY - 1 < 0 ? startY + 256 - 1 : startY - 1);
                            int startY3 = (startY2 - 1 < 0 ? startY2 + 256 - 1 : startY2 - 1);
                            if (map[startY, startX] == 0x04 && map[startY2, startX] == 0x04 && map[startY3, startX] == 0x04)
                            {
                                x = startX;
                                y = startY;
                                legal = true;
                                // IMMEDIATELY place the Dragon's Horn towers
                                map[startY + 3, startX] = 0x0a;
                                romData[0xa2f6] = (byte)(startY + 3);
                                romData[0xa2f5] = (byte)(startX);

                                map[startY - 1, startX] = 0x0a;
                                romData[0xa2f9] = (byte)(startY - 1);
                                romData[0xa2f8] = (byte)(startX);
                            }
                        }
                    }
                    else if (lnI == 3) legal = false;
                    else if (mapLegalStart(startY, startX, (lnI == 5 ? 15 : 10)))
                    {
                        map[startY, startX] = 0x01;
                        island[startY, startX] = lnI;
                        legal = true;
                        x = startX;
                        y = startY;
                    }
                }

                int islandSize = (lnI == 0 ? 1500 : lnI == 1 ? 2500 : lnI == 2 ? 1500 : lnI == 3 ? 1500 : lnI == 4 ? 5000 : 5000);
                islandSize /= (chkSmallMap.Checked ? 4 : 1);

                islandSize += r1.Next() % islandSize;
                int tiles = 0;
                int terrain = 0;
                int mapTries = 0;
                for (int lnJ = 0; lnJ < islandSize; lnJ++)
                {
                    bool legal2 = false;
                    while (!legal2)
                    {
                        // pick a direction at random
                        int dir = (r1.Next() % 4);
                        if (x == 1 && dir == 3) mapTries = 0;
                        if (mapLegalPlot(y, x, dir, lnI))
                        {
                            //int maxLimit = (chkSmallMap.Checked ? 126 : 254);
                            //if (dir == 0 && y == 1) continue;
                            //if (dir == 1 && x == maxLimit) continue;
                            //if (dir == 2 && y == maxLimit) continue;
                            //if (dir == 3 && x == 1) continue;
                            if (dir == 0)
                                y = (y == 0 ? 255 : y - 1);
                            else if (dir == 1)
                                x = (x == 255 ? 0 : x + 1);
                            if (dir == 2)
                                y = (y == 255 ? 0 : y + 1);
                            if (dir == 3)
                                x = (x == 0 ? 255 : x - 1);

                            if (map[y, x] != 0x04) { lnJ--; mapTries++; }
                            if (tiles == 0)
                            {
                                tiles = r1.Next() % 500;
                                terrain = 4;
                                while (terrain == 4 || terrain == 5)
                                    terrain = r1.Next() % 7 + 1;
                            }
                            if (map[y, x] != 0x0a)
                                map[y, x] = (lnI <= 4 ? terrain : 0x05);
                                //map[y, x] = (lnI == 0 ? 0x01 : lnI == 1 ? 0x06 : lnI == 2 ? 0x03 : lnI == 3 ? 0x02 : lnI == 4 ? 0x07 : 0x05);
                            tiles--;
                            island[y, x] = lnI;
                            legal2 = true;
                        } else
                        {
                            mapTries++;
                        }
                    }
                    if (mapTries >= 50000)
                    {
                        // Retry the map again.
                        return false;
                    }
                }
            }

            int lakeNumber = 256;
            int maxPlots = 0;
            int maxLake = 0;
            int lastIsland = 0;
            for (int lnI = 0; lnI < 256; lnI++)
                for (int lnJ = 0; lnJ < 256; lnJ++)
                {
                    if (lnI == 9 && lnJ == 65) lastIsland = lastIsland;
                    if (island[lnI, lnJ] == -1)
                    {
                        int plots = lakePlot(lakeNumber, lnI, lnJ);
                        //if (plots <= 10)
                        //{
                        //    plots = lakePlot(lakeNumber, lnI, lnJ, true, lastIsland);
                        //}
                        if (plots > maxPlots)
                        {
                            maxPlots = plots;
                            maxLake = lakeNumber;
                        }
                        lakeNumber++;
                    } else
                    {
                        lastIsland = island[lnI, lnJ];
                    }
                }

            lastIsland = -1;
            for (int lnI = 0; lnI < 256; lnI++)
                for (int lnJ = 0; lnJ < 256; lnJ++)
                {
                    if (island[lnI, lnJ] != maxLake && island[lnI, lnJ] >= 256 && lastIsland != -1)
                        lakePlot(island[lnI, lnJ], lnI, lnJ, true, lastIsland);
                    else if (island[lnI, lnJ] < 256)
                        lastIsland = island[lnI, lnJ];
                    //if (island[lnI, lnJ] == -1)
                    //{
                    //    int plots = lakePlot(lakeNumber, lnI, lnJ);
                    //    //if (plots <= 10)
                    //    //{
                    //    //    plots = lakePlot(lakeNumber, lnI, lnJ, true, lastIsland);
                    //    //}
                    //    if (plots > maxPlots)
                    //    {
                    //        maxPlots = plots;
                    //        maxLake = lakeNumber;
                    //    }
                    //    lakeNumber++;
                    //}
                    //else
                    //{
                    //    lastIsland = island[lnI, lnJ];
                    //}
                }

            // Remove tiny gaps
            //for (int lnI = 3; lnI < (chkSmallMap.Checked ? 128 : 256); lnI++)
            //    for (int lnJ = 3; lnJ < (chkSmallMap.Checked ? 128 : 256); lnJ++)
            //    {
            //        int lowY = (lnI - 1 == -1 ? 255 : lnI - 1);
            //        int lowY2 = (lowY - 1 == -1 ? 255 : lowY - 1);
            //        int lowY3 = (lowY2 - 1 == -1 ? 255 : lowY2 - 1);
            //        int lowX = (lnJ - 1 == -1 ? 255 : lnJ - 1);
            //        int lowX2 = (lowX - 1 == -1 ? 255 : lowX - 1);
            //        int lowX3 = (lowX2 - 1 == -1 ? 255 : lowX2 - 1);

            //        int islandUsed = island[lnI, lnJ];
            //        if (islandUsed <= 5 && map[lowY, lnJ] == 0x04 && island[lowY2, lnJ] == islandUsed)
            //        {
            //            island[lowY, lnJ] = islandUsed;
            //            map[lowY, lnJ] = (islandUsed == 0 ? 0x01 : islandUsed == 1 ? 0x06 : islandUsed == 2 ? 0x03 : islandUsed == 3 ? 0x02 : islandUsed == 4 ? 0x07 : 0x05);
            //        }
            //        if (islandUsed <= 5 && map[lowY, lnJ] == 0x04 && map[lowY2, lnJ] == 0x04 && island[lowY3, lnJ] == islandUsed)
            //        {
            //            island[lowY, lnJ] = island[lowY2, lnJ] = islandUsed;
            //            map[lowY, lnJ] = map[lowY2, lnJ] = (islandUsed == 0 ? 0x01 : islandUsed == 1 ? 0x06 : islandUsed == 2 ? 0x03 : islandUsed == 3 ? 0x02 : islandUsed == 4 ? 0x07 : 0x05);
            //        }

            //        if (islandUsed <= 5 && map[lnI, lowX] == 0x04 && island[lnI, lowX2] == islandUsed)
            //        {
            //            island[lnI, lowX] = islandUsed;
            //            map[lnI, lowX] = (islandUsed == 0 ? 0x01 : islandUsed == 1 ? 0x06 : islandUsed == 2 ? 0x03 : islandUsed == 3 ? 0x02 : islandUsed == 4 ? 0x07 : 0x05);
            //        }
            //        if (islandUsed <= 5 && map[lnI, lowX] == 0x04 && map[lnI, lowX2] == 0x04 && island[lnI, lowX3] == islandUsed)
            //        {
            //            island[lnI, lowX] = island[lnI, lowX2] = islandUsed;
            //            map[lnI, lowX] = map[lnI, lowX2] = (islandUsed == 0 ? 0x01 : islandUsed == 1 ? 0x06 : islandUsed == 2 ? 0x03 : islandUsed == 3 ? 0x02 : islandUsed == 4 ? 0x07 : 0x05);
            //        }
            //    }

            // Make sure Hargon's Castle is thick enough so we can have a path...
            for (int lnI = 2; lnI < (chkSmallMap.Checked ? 128 : 256); lnI++)
                for (int lnJ = 2; lnJ < (chkSmallMap.Checked ? 128 : 256); lnJ++)
                {
                    int lowY = (lnI - 1 == -1 ? 255 : lnI - 1);
                    int lowY2 = (lowY - 1 == -1 ? 255 : lowY - 1);
                    int lowX = (lnJ - 1 == -1 ? 255 : lnJ - 1);
                    int lowX2 = (lowX - 1 == -1 ? 255 : lowX - 1);
                    if (island[lnI, lnJ] == 5)
                    {
                        map[lnI, lowX] = map[lnI, lowX2] = map[lowY, lowX2] = map[lowY, lowX] = map[lowY, lnJ] = map[lowY2, lnJ] = map[lowY2, lowX] = map[lowY2, lowX2] = 0x05;
                        island[lnI, lowX] = island[lnI, lowX2] = island[lowY, lowX2] = island[lowY, lowX] = island[lowY, lnJ] = island[lowY2, lnJ] = island[lowY2, lowX] = island[lowY2, lowX2] = 5;
                    }
                }

            int tiles1 = 0;
            int terrain1 = 4;
            // Make sure Hargon's Castle is blocked off by mountains
            for (int lnI = 0; lnI < (chkSmallMap.Checked ? 128 : 256); lnI++)
                for (int lnJ = 0; lnJ < (chkSmallMap.Checked ? 128 : 256); lnJ++)
                {
                    int lowY = (lnI - 1 == -1 ? 255 : lnI - 1);
                    int highY = (lnI + 1 == 256 ? 0 : lnI + 1);
                    int lowX = (lnJ - 1 == -1 ? 255 : lnJ - 1);
                    int highX = (lnJ + 1 == 256 ? 0 : lnJ + 1);
                    if (island[lnI, lnJ] == 5)
                    {
                        if (island[lowY, lnJ] != maxLake && island[highY, lnJ] != maxLake && island[lnI, lowX] != maxLake && island[lnI, highX] != maxLake)
                        {
                            if (tiles1 <= 0)
                            {
                                tiles1 = r1.Next() % 500;
                                terrain1 = 4;
                                while (terrain1 == 4)
                                    terrain1 = r1.Next() % 7 + 1;
                            }
                            map[lnI, lnJ] = 0x07; // terrain1; // 0x07;
                            tiles1--;
                            if (map[lowY, lnJ] == 0x04)
                            {
                                map[lnI, lnJ] = 0x07; // terrain1; // 0x07;
                                island[lnI, lnJ] = 5;
                                tiles1--;
                            }
                        }
                    }
                }

            // Now to pursue map compression...
            bool mapBad = true;
            int tilesToCompress = 1;

            while (mapBad && tilesToCompress <= 16)
            {
                int bytesUsed = 0;
                for (int lnI = 0; lnI < 256; lnI++)
                {
                    int lnJ = 0;
                    while (lnI < 256 && lnJ < 256)
                    {
                        if (map[lnI, lnJ] >= 1 && map[lnI, lnJ] <= 7)
                        {
                            int tileNumber = 0;
                            int numberToMatch = map[lnI, lnJ];
                            while (lnJ < 256 && tileNumber < 32 && map[lnI, lnJ] == numberToMatch && tileNumber < 32)
                            {
                                tileNumber++;
                                lnJ++;
                            }
                            bytesUsed++;
                        }
                        else
                        {
                            bytesUsed++;
                            lnJ++;
                        }
                    }
                }

                if (bytesUsed <= 6350) // Actually 6496, but there are more features coming to the so we need some insurance.
                    mapBad = false;
                else
                {
                    int sameTerrain = 0;
                    int previousTerrain = 4;
                    bool firstTerrain = true;
                    for (int lnI = 0; lnI < 256; lnI++)
                    {
                        sameTerrain = 0;
                        previousTerrain = map[lnI, 0];
                        for (int lnJ = 0; lnJ < 256; lnJ++)
                        {
                            if (lnI == 9 && lnJ == 64) sameTerrain = sameTerrain;
                            if (map[lnI, lnJ] == previousTerrain)
                            {
                                sameTerrain++;
                            }
                            else {
                                // Can't compress the first terrain, nor any terrain that is not a terrain that can be compressed (i.e. towns, castles, caves, towers, monoliths, swamps)
                                if (sameTerrain <= tilesToCompress && !firstTerrain)
                                {
                                    bool compressOK = true;
                                    if (map[lnI, lnJ] > 7 || map[lnI, lnJ] == 4)
                                        compressOK = false;
                                    if (lnI > 0 && lnI < 255 && lnJ > 0 && lnJ < 255 && (map[lnI, lnJ - 1] == 0x04 && map[lnI, lnJ + 1] == 0x04 && map[lnI - 1, lnJ] == 0x04 && map[lnI + 1, lnJ] == 0x04))
                                    {
                                        // Do not compress; it is probably a key island
                                        compressOK = false;
                                    }

                                    if (map[lnI, lnJ] == 0x04 && island[lnI, lnJ - 1] == 5 && previousTerrain == 5)
                                    {
                                        // Do not compress the mountain edge of Hargon's Island.
                                        compressOK = false;
                                    }

                                    int currentIsland = island[lnI, lnJ];

                                    // Do not compress water that's not on the same island.
                                    for (int lnK = 1; lnK <= tilesToCompress; lnK++)
                                    {
                                        if (lnJ - lnK < 0) break;
                                        if (island[lnI, lnJ - lnK] != currentIsland)
                                            compressOK = false;
                                        if (map[lnI, lnJ - lnK] > 7)
                                            compressOK = false;
                                    }

                                    if (island[lnI, lnJ] == 5 && map[lnI, lnJ - 1] == 0x05 || map[lnI, lnJ] == 0x05)
                                        compressOK = false; // Don't compress the mountain border

                                    if (compressOK)
                                    {
                                        // Look at the current terrain.  Make the previous tiles that terrain.  Unless the current terrain is water, then make the current terrain the previous terrain.
                                        //if (map[lnI, lnJ] == 4) map[lnI, lnJ] = previousTerrain;
                                        //else
                                        //{
                                        for (int lnK = 1; lnK <= tilesToCompress; lnK++)
                                        {
                                            if (lnJ - lnK < 0) break;
                                            map[lnI, lnJ - lnK] = map[lnI, lnJ];
                                        }
                                        //}
                                    }
                                }
                                firstTerrain = false;
                                previousTerrain = map[lnI, lnJ];
                                sameTerrain = 0;
                            }
                        }
                    }
                    tilesToCompress++;
                }
            }

            // Moon Tower

            bool moonLegal = false;
            while (!moonLegal)
            {
                int x = r1.Next() % (chkSmallMap.Checked ? 117 : 245);
                int y = r1.Next() % (chkSmallMap.Checked ? 117 : 245);
                if (validPlot(y, x, 10, 10, new int[] { 0, 1, 2, 3, 4 }))
                {
                    for (int lnJ = 1; lnJ <= 5; lnJ++)
                        for (int lnK = 1; lnK <= 5; lnK++)
                        {
                            if (lnJ == 1 || lnJ == 5 || lnK == 1 || lnK == 5)
                            {
                                map[y + lnJ, x + lnK] = 0x04;
                                //romData[0xa2ef] = (byte)(x + lnK);
                                //romData[0xa2f0] = (byte)(y + lnJ);
                                //romData[0x3e018] = (byte)(x + lnK);
                                //romData[0x3e01e] = (byte)(y + lnJ);
                            }
                            else if (((lnJ == 2 || lnJ == 4) && lnK >= 2 && lnK <= 4) || (lnJ == 3 && (lnK == 2 || lnK == 4)))
                                map[y + lnJ, x + lnK] = 0x01;
                            else if (lnJ == 3 && lnK == 3)
                            {
                                map[y + lnJ, x + lnK] = 0x0a;
                                // Place tower location into the ROM data now.
                                romData[0xa2f2] = (byte)(x + lnK);
                                romData[0xa2f3] = (byte)(y + lnJ);
                            }
                            // Also need to update the ROM to indicate the Rhone Cave location.
                            //romData[0x196a7] = (byte)x;
                            //romData[0x196ab] = (byte)(x + 8);
                            //romData[0x196b1] = (byte)y;
                            //romData[0x196b5] = (byte)(y + 6);
                        }

                    for (int lnJ = 6; lnJ <= 8; lnJ++)
                    {
                        map[y + 1, x + lnJ] = 0x05;
                        map[y + 2, x + lnJ] = 0x02;
                        map[y + 3, x + lnJ] = 0x05;
                    }

                    // Place tower location into the ROM data now.
                    romData[0x3dffe] = 0x13;
                    romData[0x3e004] = 0x12;
                    romData[0x3e000] = (byte)(y + 2);
                    romData[0x3e006] = (byte)(x + 5);
                    romData[0x3e00a] = (byte)(x + 9);

                    int riverX = x + 9;
                    riverX = (riverX == 256 ? 0 : riverX);
                    while (island[y + 2, riverX] != maxLake)
                    {
                        map[y + 2, riverX] = 0x04;
                        riverX++;
                        riverX = (riverX == 256 ? 0 : riverX);
                    }

                    moonLegal = true;
                }
            }

            bool seaLegal = false;
            while (!seaLegal)
            {
                int x = r1.Next() % (chkSmallMap.Checked ? 123 : 251);
                int y = r1.Next() % (chkSmallMap.Checked ? 121 : 249);
                if (validPlot(y, x, 7, 5, new int[] { maxLake }))
                {
                    if (validSeaPlot(y, x, maxLake, 12))
                    {
                        List<int> seaCaveShoals = new List<int> { 1, 2, 3, 5, 6, 8, 9, 10, 14, 15, 19, 20, 24, 25, 26, 28, 29, 31, 32, 33 };
                        List<int> seaCaveMountains = new List<int> { 7, 11, 12, 13, 16, 18 };
                        int seaCaveCave = 17;
                        int lnTileCounter = 0;

                        for (int lnJ = 0; lnJ < 7; lnJ++)
                            for (int lnK = 0; lnK < 5; lnK++)
                            {
                                if (seaCaveMountains.Contains(lnTileCounter)) map[y + lnJ, x + lnK] = 0x05;
                                else if (seaCaveShoals.Contains(lnTileCounter)) map[y + lnJ, x + lnK] = 0x13;
                                else if (seaCaveCave == lnTileCounter)
                                {
                                    map[y + lnJ, x + lnK] = 0x0c;
                                    // Also need to update the ROM to indicate where the Sea Cave is in case the Moon Fragment was used.
                                    romData[0xa2e3] = (byte)(x + lnK);
                                    romData[0xa2e4] = (byte)(y + lnJ);

                                    romData[0x198ef] = romData[0x3e154] = (byte)(x - 2);
                                    romData[0x198f3] = romData[0x3e158] = (byte)(x + 5 + 2);
                                    romData[0x198f9] = romData[0x3e15e] = (byte)(y - 2);
                                    romData[0x198fd] = romData[0x3e162] = (byte)(y + 7 + 2);
                                }
                                lnTileCounter++;
                            }
                        seaLegal = true;
                    }
                }
            }

            bool treeLegal = false;
            while (!treeLegal)
            {
                int x = r1.Next() % (chkSmallMap.Checked ? 121 : 249);
                int y = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                if (validPlot(y, x, 1, 1, new int[] { maxLake }))
                {
                    // Confirm that the starting point is no more than 8 squares away from main land.
                    if (validSeaPlot(y, x, maxLake))
                    {
                        map[y, x] = 0x03;
                        // Also need to update the ROM to indicate the World Tree location.
                        romData[0x19f20] = (byte)(x);
                        romData[0x19f21] = (byte)(y);

                        treeLegal = true;
                    }
                }
            }

            bool rubissLegal = false;
            while (!rubissLegal)
            {
                int rubissX = r1.Next() % (chkSmallMap.Checked ? 128 : 256);
                int rubissY = r1.Next() % (chkSmallMap.Checked ? 128 : 256);
                if (mapLegalStart(rubissY, rubissX, 4, 256))
                {
                    if (validSeaPlot(rubissY, rubissX, maxLake))
                    {
                        map[rubissY, rubissX] = 0x0b;
                        romData[0xa2d7] = (byte)rubissX;
                        romData[0xa2d8] = (byte)rubissY;
                        rubissLegal = true;
                    }
                }
            }

            bool treasuresLegal = false;
            while (!treasuresLegal)
            {
                int treasuresX = r1.Next() % (chkSmallMap.Checked ? 128 : 256);
                int treasuresY = r1.Next() % (chkSmallMap.Checked ? 128 : 256);
                if (mapLegalStart(treasuresY, treasuresX, 4, 256))
                {
                    if (validSeaPlot(treasuresY, treasuresX, maxLake))
                    {
                        map[treasuresY, treasuresX] = 0x13;
                        // Also need to update the ROM to indicate the treasures spot.  (make sure it's vertical - 1!)
                        romData[0x19f1c] = (byte)treasuresX;
                        romData[0x19f1d] = (byte)(treasuresY + 1);
                        treasuresLegal = true;
                    }
                }
            }

            bool rhoneLegal = false;
            while (!rhoneLegal)
            {
                int x = r1.Next() % (chkSmallMap.Checked ? 120 : 248);
                int y = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                if (validPlot(y, x, 6, 8, new int[] { 0, 1, 2, 3, 4 }))
                {
                    int rhoneCaveX = r1.Next() % 6;
                    for (int lnJ = 0; lnJ < 6; lnJ++)
                        for (int lnK = 0; lnK < 8; lnK++)
                        {
                            if (lnJ == 1 && lnK == rhoneCaveX + 1)
                            {
                                map[y + lnJ, x + lnK] = 0x0c;
                                romData[0xa2ef] = (byte)(x + lnK);
                                romData[0xa2f0] = (byte)(y + lnJ);
                                romData[0x3e018] = (byte)(x + lnK);
                                romData[0x3e01e] = (byte)(y + lnJ);
                            }
                            else if (lnK != 0 && lnK != 7 && lnJ == 1) map[y + lnJ, x + lnK] = 0x05;
                            else if (lnK != 0 && lnK != 7 && lnJ > 1) map[y + lnJ, x + lnK] = 0x08;
                            // Also need to update the ROM to indicate the Rhone Cave location.
                            romData[0x196a7] = (byte)x;
                            romData[0x196ab] = (byte)(x + 8);
                            romData[0x196b1] = (byte)y;
                            romData[0x196b5] = (byte)(y + 6);
                        }

                    rhoneLegal = true;
                }
            }

            // Lake Cave

            bool lakeLegal = false;
            while (!lakeLegal)
            {
                int x = r1.Next() % (chkSmallMap.Checked ? 128 : 256);
                int y = r1.Next() % (chkSmallMap.Checked ? 128 : 256);
                if (validPlot(y, x, 1, 1, new int[] { 0, 1 }))
                {
                    map[y, x] = 0x0c;
                    romData[0xa2e0] = (byte)(x);
                    romData[0xa2e1] = (byte)(y);
                    //for (int lnJ = 1; lnJ < 6; lnJ++)
                    //    for (int lnK = 1; lnK < 6; lnK++)
                    //    {
                    //        if (lnJ == 1 || lnJ == 5)
                    //        {
                    //            if (lnK == 1 || lnK == 5)
                    //                map[y + lnJ, x + lnK] = 0x06;
                    //            else
                    //                map[y + lnJ, x + lnK] = 0x04;
                    //        }
                    //        else if (lnJ == 2 || lnJ == 4)
                    //            map[y + lnJ, x + lnK] = 0x04;
                    //        else
                    //        {
                    //            if (lnK == 0 || lnK == 1)
                    //                map[y + lnJ, x + lnK] = 0x09;
                    //            else if (lnK == 3)
                    //            {
                    //                map[y + lnJ, x + lnK] = 0x0c;
                    //                romData[0xa2e0] = (byte)(x + lnK);
                    //                romData[0xa2e1] = (byte)(y + lnJ);
                    //            }
                    //            else
                    //                map[y + lnJ, x + lnK] = 0x09;
                    //        }
                    //    }
                    lakeLegal = true;
                }
            }

            // Mirror Of Ra
            bool mirrorLegal = false;
            while (!mirrorLegal)
            {
                int x = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                int y = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                if (validPlot(y, x, 5, 6, new int[] { 0, 1 }))
                {
                    for (int lnJ = 1; lnJ < 4; lnJ++)
                        for (int lnK = 1; lnK < 5; lnK++)
                        {
                            if (lnJ == 1 || lnK == 1 || lnK == 4)
                                map[y + lnJ, x + lnK] = 0x13;
                            else
                                map[y + lnJ, x + lnK] = 0x08;
                        }
                    // Also need to update the ROM to indicate the new Mirror Of Ra search spot.
                    romData[0x19f18] = (byte)(x + 2);
                    romData[0x19f19] = (byte)(y + 2);

                    mirrorLegal = true;
                }
            }

            // We'll place all of the castles now.
            for (int lnI = 0; lnI < 7; lnI++)
            {
                int x = r1.Next() % (chkSmallMap.Checked ? 125 : 253);
                int y = r1.Next() % (chkSmallMap.Checked ? 125 : 253);

                if (validPlot(y, x, 2, 2, (lnI == 0 || lnI == 1 ? new int[] { 0 } : lnI == 6 ? new int[] { 5 } : new int[] { 0, 1, 2, 3, 4 })))
                {
                    map[y + 0, x + 0] = 0x00;
                    map[y + 0, x + 1] = 0x12;
                    map[y + 1, x + 0] = 0x10;
                    map[y + 1, x + 1] = 0x11;

                    int byteToUse = (lnI == 0 ? 0xa28f : lnI == 1 ? 0xa295 : lnI == 2 ? 0xa29b : lnI == 3 ? 0xa2a1 : lnI == 4 ? 0xa2a4 : lnI == 5 ? 0xa2e9 : 0xa2b3);
                    romData[byteToUse] = (byte)(x + 1);
                    romData[byteToUse + 1] = (byte)(y + 1);
                    if (lnI == 5) // Charlock castle, out of order as far as byte sequence is concerned.
                    {
                        romData[0xa334] = (byte)(x);
                        romData[0xa335] = (byte)(y + 1);
                    }
                    else
                    {
                        romData[byteToUse + 0x7e] = (byte)(x);
                        romData[byteToUse + 1 + 0x7e] = (byte)(y + 1);
                    }
                    //if (lnI == 6)
                    //{
                    //    romData[0xa331] = (byte)(x);
                    //    romData[0xa332] = (byte)(y + 1);
                    //}

                    // Return points
                    if (lnI == 0 || lnI == 1 || lnI == 3 || lnI == 4)
                    {
                        int byteMultiplier = lnI - (lnI >= 3 ? 1 : 0);
                        romData[0xa27a + (3 * byteMultiplier)] = (byte)x;
                        if (map[y + 2, x] == 0x04)
                            romData[0xa27a + (3 * byteMultiplier) + 1] = (byte)(y + 2);
                        else
                            romData[0xa27a + (3 * byteMultiplier) + 1] = (byte)(y + 1);
                        shipPlacement(0x1bf84 + (2 * byteMultiplier), y, x, maxLake);
                    }
                }
                else
                    lnI--;
            }

            // Now we'll place all of the towns now.
            for (int lnI = 0; lnI < 7; lnI++)
            {
                int x = r1.Next() % (chkSmallMap.Checked ? 125 : 253);
                int y = r1.Next() % (chkSmallMap.Checked ? 125 : 253);

                if (validPlot(y, x, 1, 2, (lnI == 0 ? new int[] { 0 } : lnI == 1 ? new int[] { 1 } : lnI == 2 ? new int[] { 3 } : new int[] { 0, 1, 2, 3, 4 })))
                {
                    map[y, x + 0] = 0x0e;
                    map[y, x + 1] = 0x0f;

                    int byteToUse2 = (lnI == 0 ? 0xa292 : lnI == 1 ? 0xa298 : lnI == 2 ? 0xa29e : lnI == 3 ? 0xa2a7 : lnI == 4 ? 0xa2aa : lnI == 5 ? 0xa2ad : 0xa2b0);
                    romData[byteToUse2] = (byte)(x + 1);
                    romData[byteToUse2 + 1] = (byte)(y);
                    romData[byteToUse2 + 0x7e] = (byte)(x);
                    romData[byteToUse2 + 1 + 0x7e] = (byte)(y);

                    // Return points
                    if (lnI == 2)
                        shipPlacement(0x3d6be, y, x, maxLake);
                    // Return points
                    else if (lnI == 1)
                    {
                        romData[0xa27a + 18] = (byte)(x);
                        if (map[y + 1, x] == 0x04)
                            romData[0xa27a + 19] = (byte)(y);
                        else
                            romData[0xa27a + 19] = (byte)(y + 1);
                        shipPlacement(0x1bf84 + 12, y, x, maxLake);
                    }
                    else if (lnI == 6)
                    {
                        romData[0xa27a + 12] = (byte)(x);
                        if (map[y + 1, x] == 0x04)
                            romData[0xa27a + 13] = (byte)(y);
                        else
                            romData[0xa27a + 13] = (byte)(y + 1);
                        // We are placing the ship in both Beran and the Rhone Shrine at the same time.
                        shipPlacement(0x1bf84 + 8, y, x, maxLake);
                        shipPlacement(0x1bf84 + 10, y, x, maxLake);
                    }
                }
                else
                    lnI--;
            }

            // Then the monoliths.
            for (int lnI = 0; lnI < 12; lnI++)
            {
                int x = r1.Next() % (chkSmallMap.Checked ? 125 : 253);
                int y = r1.Next() % (chkSmallMap.Checked ? 125 : 253);

                if (validPlot(y, x, 1, 1, (lnI == 1 ? new int[] { 0 } : lnI == 8 ? new int[] { 1 } : lnI == 7 ? new int[] { 2 } : lnI == 6 ? new int[] { 5 } : new int[] { 0, 1, 2, 3, 4 })))
                {
                    map[y, x] = 0x0b;

                    int byteToUse2 = (lnI < 11 ? 0xa2b6 + (lnI * 3) : 0xa2da);
                    romData[byteToUse2] = (byte)(x);
                    romData[byteToUse2 + 1] = (byte)(y);

                    // Return points
                    if (lnI == 6)
                    {
                        romData[0xa27a + 15] = (byte)(x);
                        if (map[y + 1, x] == 0x04)
                            romData[0xa27a + 16] = (byte)(y);
                        else
                            romData[0xa27a + 16] = (byte)(y + 1);
                    }
                }
                else
                    lnI--;
            }

            // Then the caves.
            for (int lnI = 0; lnI < 6; lnI++)
            {
                int x = r1.Next() % (chkSmallMap.Checked ? 123 : 253);
                int y = r1.Next() % (chkSmallMap.Checked ? 123 : 253);

                if (validPlot(y, x, 1, 1, (lnI == 2 ? new int[] { 0 } : lnI == 3 ? new int[] { 1 } : lnI == 4 ? new int[] { 5 } : new int[] { 0, 1, 2, 3, 4 })))
                {
                    map[y, x] = 0x0c;

                    int byteToUse2 = (lnI == 0 ? 0xa2dd : lnI == 1 ? 0xa2fb : lnI == 2 ? 0xa2fe : lnI == 3 ? 0xa304 : lnI == 4 ? 0xa307 : 0xa30a);
                    romData[byteToUse2] = (byte)x;
                    romData[byteToUse2 + 1] = (byte)(y);
                }
                else
                    lnI--;
            }

            // Finally the towers
            for (int lnI = 0; lnI < 2; lnI++)
            {
                int x = r1.Next() % (chkSmallMap.Checked ? 125 : 253);
                int y = r1.Next() % (chkSmallMap.Checked ? 125 : 253);

                // Need to make sure it's a valid 7x7 plot due to dropping with the Cloak of wind...
                if (validPlot(y, x, 7, 7, (lnI == 0 ? new int[] { 0, 1 } : new int[] { 0, 1, 2, 3, 4 })))
                {
                    map[y + 3, x + 3] = 0x0a;

                    int byteToUse2 = (lnI == 0 ? 0xa2e6 : 0xa2ec);
                    romData[byteToUse2] = (byte)(x + 3);
                    romData[byteToUse2 + 1] = (byte)(y + 3);
                }
                else
                    lnI--;
            }

            int[,] monsterZones = new int[16, 16];
            for (int lnI = 0; lnI < 16; lnI++)
                for (int lnJ = 0; lnJ < 16; lnJ++)
                    monsterZones[lnI, lnJ] = 0xff;

            for (int lnI = 0; lnI < 256; lnI++)
                for (int lnJ = 0; lnJ < 256; lnJ++)
                {
                    int mzY = lnI / 16;
                    int mzX = lnJ / 16;

                    if (monsterZones[mzY, mzX] == 0xff)
                    {
                        if (island[lnI, lnJ] == 0)
                            monsterZones[mzY, mzX] = r1.Next() % 9;
                        else if (island[lnI, lnJ] == 1)
                            monsterZones[mzY, mzX] = r1.Next() % 5 + 0x0d;
                        else if (island[lnI, lnJ] == 5)
                            monsterZones[mzY, mzX] = r1.Next() % 2 + 0x32;
                        else if (island[lnI, lnJ] >= 2 && island[lnI, lnJ] <= 4)
                        {
                            while (monsterZones[mzY, mzX] > 0x27 || (monsterZones[mzY, mzX] >= 0x1c && monsterZones[mzY, mzX] <= 0x1f))
                                monsterZones[mzY, mzX] = r1.Next() % 19 + 0x15;
                            if (monsterZones[mzY, mzX] == 0x26) monsterZones[mzY, mzX] = 0x39;
                            if (monsterZones[mzY, mzX] == 0x27) monsterZones[mzY, mzX] = 0x3b;
                        }

                        if (island[lnI, lnJ] >= 0 && island[lnI, lnJ] <= 5)
                            monsterZones[mzY, mzX] += (64 * (r1.Next() % 4));
                    }
                }

            // Now let's enter all of this into the ROM...
            int lnPointer = 0x9f97;

            for (int lnI = 0; lnI <= 256; lnI++) // <---- There is a final pointer for lnI = 256, probably indicating the conclusion of the map.
            {
                romData[0xdda5 + (lnI * 2)] = (byte)(lnPointer % 256);
                romData[0xdda6 + (lnI * 2)] = (byte)(lnPointer / 256);

                int lnJ = 0;
                while (lnI < 256 && lnJ < 256)
                {
                    if (map[lnI, lnJ] >= 1 && map[lnI, lnJ] <= 7)
                    {
                        int tileNumber = 0;
                        int numberToMatch = map[lnI, lnJ];
                        while (lnJ < 256 && tileNumber < 32 && map[lnI, lnJ] == numberToMatch && tileNumber < 32)
                        {
                            tileNumber++;
                            lnJ++;
                        }
                        romData[lnPointer + 0x4010] = (byte)((0x20 * numberToMatch) + (tileNumber - 1));
                        lnPointer++;
                    }
                    else
                    {
                        romData[lnPointer + 0x4010] = (byte)map[lnI, lnJ];
                        lnPointer++;
                        lnJ++;
                    }
                }
            }
            //lnPointer = lnPointer;
            if (lnPointer >= 0xb8f7)
            {
                MessageBox.Show("WARNING:  The map might have taken too much ROM space...");
                // Might have to compress further to remove one byte stuff
                // Must compress the map by getting rid of further 1 byte lakes
            }

            if (chkSmallMap.Checked)
            {
                romData[0x10083] = 0x85;
                romData[0x10084] = 0xd5;
                romData[0x10085] = 0xa5;
                romData[0x10086] = 0x17;
                romData[0x10087] = 0x29;
                romData[0x10088] = 0x78;
                romData[0x10089] = 0x0a;
            }

            // Enter monster zones
            for (int lnI = 0; lnI < 16; lnI++)
                for (int lnJ = 0; lnJ < 16; lnJ++)
                {
                    if (monsterZones[lnI, lnJ] == 0xff)
                        monsterZones[lnI, lnJ] = (r1.Next() % 60) + ((r1.Next() % 4) * 64);
                    romData[0x103d6 + (lnI * 16) + lnJ] = (byte)monsterZones[lnI, lnJ];
                }

            return true;
        }

        private bool validSeaPlot(int y, int x, int maxLake, int steps = 8)
        {
            int y1 = y;
            int x1 = x;
            for (int lnI = 0; lnI < steps; lnI++)
            {
                y1--;
                if (y1 == 0 || y1 == (chkSmallMap.Checked ? 128 : 256)) return false;
                y1 = (y1 < 0 ? y1 + 256 : y1);
                if (island[y1, x1] != maxLake && lnI == 0) return false;
                if (island[y1, x1] != maxLake) return true;
            }
            y1 = y;
            for (int lnI = 0; lnI < steps; lnI++)
            {
                y1++;
                if (y1 == 0 || y1 == (chkSmallMap.Checked ? 128 : 256)) return false;
                y1 = (y1 >= 256 ? y1 - 256 : y1);
                if (island[y1, x1] != maxLake && lnI == 0) return false;
                if (island[y1, x1] != maxLake) return true;
            }

            y1 = y;
            for (int lnI = 0; lnI < steps; lnI++)
            {
                x1++;
                if (x1 == 0 || x1 == (chkSmallMap.Checked ? 128 : 256)) return false;
                x1 = (x1 >= 256 ? x1 - 256 : x1);
                if (island[y1, x1] != maxLake && lnI == 0) return false;
                if (island[y1, x1] != maxLake) return true;
            }
            x1 = x;
            for (int lnI = 0; lnI < steps; lnI++)
            {
                x1--;
                if (x1 == 0 || x1 == (chkSmallMap.Checked ? 128 : 256)) return false;
                x1 = (x1 < 0 ? x1 + 256 : x1);
                if (island[y1, x1] != maxLake && lnI == 0) return false;
                if (island[y1, x1] != maxLake) return true;
            }
            return false;
        }

        private bool validPlot(int y, int x, int height, int width, int[] legalIsland)
        {
            //y++;
            //x++;
            for (int lnI = 0; lnI < height; lnI++)
                for (int lnJ = 0; lnJ < width; lnJ++)
                {
                    if (y + lnI >= (chkSmallMap.Checked ? 128 : 256) || x + lnJ >= (chkSmallMap.Checked ? 128 : 256)) return false;

                    int legalY = (y + lnI >= 256 ? y - 256 + lnI : y + lnI);
                    int legalX = (x + lnJ >= 256 ? x - 256 + lnJ : x + lnJ);

                    bool ok = false;
                    for (int lnK = 0; lnK < legalIsland.Length; lnK++)
                        if (island[legalY, legalX] == legalIsland[lnK])
                            ok = true;
                    if (!ok) return false;
                    // map[legalY, legalX] == 0x04 || 
                    if (map[legalY, legalX] == 0x00 || map[legalY, legalX] == 0x05 || map[legalY, legalX] == 0x0a || map[legalY, legalX] == 0x0b || map[legalY, legalX] == 0x0c ||
                        map[legalY, legalX] == 0x0e || map[legalY, legalX] == 0x0f || map[legalY, legalX] == 0x10 || map[legalY, legalX] == 0x11 || map[legalY, legalX] == 0x12 || map[legalY, legalX] == 0x13)
                        return false;
                }
            return true;
        }

        private int lakePlot(int lakeNumber, int y, int x, bool fill = false, int islandNumber = -1)
        {
            bool first = true;
            List<int> toPlot = new List<int>();
            int plots = 1;
            //if (islandNumber >= 0) plots = 1;
            while (first || toPlot.Count != 0)
            {
                if (!first)
                {
                    y = toPlot[0];
                    toPlot.RemoveAt(0);
                    x = toPlot[0];
                    toPlot.RemoveAt(0);
                } else
                {
                    if (fill)
                        map[y, x] = (islandNumber == 0 ? 0x01 : islandNumber == 1 ? 0x06 : islandNumber == 2 ? 0x03 : islandNumber == 3 ? 0x02 : islandNumber == 4 ? 0x07 : 0x05);
                    first = false;
                }

                for (int dir = 0; dir < 5; dir++)
                {
                    int dirX = (dir == 4 ? x - 1 : dir == 2 ? x + 1 : x);
                    dirX = (dirX == 256 ? 0 : dirX == -1 ? 255 : dirX);
                    int dirY = (dir == 1 ? y - 1 : dir == 3 ? y + 1 : y);
                    dirY = (dirY == 256 ? 0 : dirY == -1 ? 255 : dirY);

                    if (island[dirY, dirX] == -1 || (island[dirY, dirX] == lakeNumber && fill))
                    {
                        plots++;
                        island[dirY, dirX] = (fill ? islandNumber : lakeNumber);
                        if (fill)
                            map[dirY, dirX] = (islandNumber == 0 ? 0x01 : islandNumber == 1 ? 0x06 : islandNumber == 2 ? 0x03 : islandNumber == 3 ? 0x02 : islandNumber == 4 ? 0x07 : 0x05);

                        if (dir != 0)
                        {
                            toPlot.Add(dirY);
                            toPlot.Add(dirX);
                        }
                        //plots += lakePlot(lakeNumber, y, x, fill);
                    }
                }
            }

            return plots;
        }

        private bool mapLegalStart(int y, int x, int size = 10, int islandNumber = -1)
        {
            for (int lnI = 0 - size; lnI <= size; lnI++)
                for (int lnJ = 0 - size; lnJ <= size; lnJ++)
                {
                    if (x + lnI < 0 || x + lnI >= (chkSmallMap.Checked ? 128 : 256) || y + lnJ < 0 || y + lnJ >= (chkSmallMap.Checked ? 128 : 256)) return false;
                    int x1 = (x + lnI < 0 ? x + lnI + 256 : x + lnI > 255 ? x + lnI - 256 : x + lnI);
                    int y1 = (y + lnJ < 0 ? y + lnJ + 256 : y + lnJ > 255 ? y + lnJ - 256 : y + lnJ);
                    
                    if (island[y1, x1] != islandNumber) return false;
                }

            return true;
        }

        private bool mapLegalPlot(int y, int x, int dir, int islandNumber)
        {
            x = (dir == 1 ? x + 1 : dir == 3 ? x - 1 : x);
            y = (dir == 0 ? y - 1 : dir == 2 ? y + 1 : y);

            int minRange = (islandNumber == 5 ? -5 : -1);
            int maxRange = (islandNumber == 5 ? 5 : 1);

            for (int lnI = minRange; lnI <= maxRange; lnI++)
                for (int lnJ = minRange; lnJ <= maxRange; lnJ++)
                {
                    //if (x + lnI < 0 || x + lnI >= (chkSmallMap.Checked ? 128 : 256) || y + lnJ < 0 || y + lnJ >= (chkSmallMap.Checked ? 128 : 256)) return false;
                    int x1 = (x + lnI < 0 ? x + lnI + 256 : x + lnI > 255 ? x + lnI - 256 : x + lnI);
                    int y1 = (y + lnJ < 0 ? y + lnJ + 256 : y + lnJ > 255 ? y + lnJ - 256 : y + lnJ);

                    if (island[y1, x1] != islandNumber && island[y1, x1] != -1) return false;
                }

            return true;
        }

        private int randomizeTile(Random r1, int lastTile, int upperTile)
        {
            // 90% chance of tile repeating
            if (r1.Next() % 10 != 0)
                return lastTile;

            // If not, 75% chance of tile repeating the last row's tile.
            if (r1.Next() % 4 == 0)
                return upperTile;

            // If not, 33% chance of tile being water straight out.
            if (r1.Next() % 3 == 0)
                return 4;

            // Otherwise, straight random tile from 1-7.
            return ((r1.Next() % 7) + 1);
        }

        private void randomizeMap(Random r1)
        {
            // We need to make four islands.  One to get the prince, one to get the princess, one for the approach to Lianport, one for Lianport itself, 
            // one for the "mainland", and one for "Hargon's Castle".
            for (int lnI = 0; lnI < 256; lnI++)
                for (int lnJ = 0; lnJ < 256; lnJ++)
                    map[lnI, lnJ] = 0x04;

            int[,] monsterZones = new int[16, 16];
            for (int lnI = 0; lnI < 16; lnI++)
                for (int lnJ = 0; lnJ < 16; lnJ++)
                    monsterZones[lnI, lnJ] = 0xff;

            int[,] islandSize = new int[,] { 
                { 0, 0 },
                { 0, 0 },
                { 0, 0 },
                { 0, 0 },
                { 0, 0 },
                { 0, 0 },
                { 0, 0 }, 
                { 0, 0 }, 
                { 0, 0 }, 
                { 0, 0 }, 
                { 0, 0 },
                { 0, 0 },
                { 0, 0 }, 
                { 0, 0 }, 
                { -1, -1 },
                { -1, -1 },
                { -1, -1 },
                { -1, -1 },
                { -1, -1 },
                { -1, -1 },
                { -1, -1 },
                { -1, -1 },
                { -1, -1 },
                { -1, -1 }};

            int[,] islandLocation = new int[,] { 
                { 0, 0 },
                { 0, 0 },
                { 0, 0 },
                { 0, 0 },
                { 0, 0 },
                { 0, 0 },
                { 0, 0 },
                { 0, 0 }, 
                { 0, 0 }, 
                { 0, 0 },
                { 0, 0 },
                { 0, 0 }, 
                { 0, 0 }, 
                { 0, 0 },
                { -1, -1 },
                { -1, -1 },
                { -1, -1 },
                { -1, -1 },
                { -1, -1 },
                { -1, -1 },
                { -1, -1 },
                { -1, -1 },
                { -1, -1 },
                { -1, -1 } };

            List<int> seaCaveShoals = new List<int> { 1, 2, 3, 5, 6, 8, 9, 10, 14, 15, 19, 20, 24, 25, 26, 28, 29, 31, 32, 33 };
            List<int> seaCaveMountains = new List<int> { 7, 11, 12, 13, 16, 18 };
            int seaCaveCave = 17;

            List<int> worldTreeMountains;
            List<int> worldTreeDesert;
            if (r1.Next() % 2 == 0)
            {
                worldTreeMountains = new List<int> { 1, 2, 3, 4, 5, 7, 8, 12, 13, 20, 21, 27, 28, 29, 33, 34, 36, 37, 38, 39, 40 };
                worldTreeDesert = new List<int> { 9, 10, 11, 14, 15, 16, 18, 19, 22, 23, 24, 25, 26, 30, 31, 32 };
            }
            else
            {
                worldTreeMountains = new List<int> { 1, 2, 3, 4, 5, 7, 8, 12, 13, 14, 21, 27, 28, 29, 33, 34, 36, 37, 38, 39, 40 };
                worldTreeDesert = new List<int> { 9, 10, 11, 15, 16, 18, 19, 20, 22, 23, 24, 25, 26, 30, 31, 32 };
            }
            int worldTree = 17;

            int lakeCaveBridgeDir = r1.Next() % 4;
            int lakeCaveRiverDir = r1.Next() % 4;
            while (lakeCaveRiverDir == lakeCaveBridgeDir)
                lakeCaveRiverDir = r1.Next() % 4;

            int moonTowerRiverDir = r1.Next() % 4;
            moonTowerRiverDir = 3;

            List<int> moonTowerMountains;
            List<int> moonTowerRiver = new List<int> { 43, 44, 45, 46, 47, 55, 56, 60, 61, 68, 74, 81, 87, 94, 100, 107, 108, 112, 113, 121, 122, 123, 124, 125 };
            List<int> moonTowerDesert;
            if (moonTowerRiverDir == 0)
            {
                moonTowerDesert = new List<int> { 4, 17, 30 };
                moonTowerMountains = new List<int> { 3, 16, 29, 5, 18, 31 };
            }
            else if (moonTowerRiverDir == 1)
            {
                moonTowerDesert = new List<int> { 104, 105, 106 };
                moonTowerMountains = new List<int> { 91, 92, 93, 117, 118, 119 };
            }
            else if (moonTowerRiverDir == 2)
            {
                moonTowerDesert = new List<int> { 138, 151, 164 };
                moonTowerMountains = new List<int> { 137, 150, 163, 139, 152, 165 };
            }
            else // if (moonTowerRiverDir == 3)
            {
                moonTowerDesert = new List<int> { 62, 63, 64 };
                moonTowerMountains = new List<int> { 49, 50, 51, 75, 76, 77 };
            }

            romData[0x3e006] = 0xff;
            romData[0x3e00a] = 0x00;

            int rhoneCaveX = r1.Next() % 6;
            int attempts = 0;
            int islands = 0;

            for (int lnI = 0; lnI < islandSize.GetLength(0); lnI++)
            {
                if (lnI >= 14 && attempts > 100)
                    break;
                attempts++;

                // Swap 2 with 1, swap 3 with 0
                int minArea = (lnI == 0 ? 1500 : lnI == 1 ? 2000 : lnI == 2 ? 2500 : lnI == 3 ? 1000 : lnI == 4 ? 4000 : lnI == 5 ? 3000 : 300);
                int maxArea = (lnI == 0 ? 3500 : lnI == 1 ? 4000 : lnI == 2 ? 5000 : lnI == 3 ? 2000 : lnI == 4 ? 10000 : lnI == 5 ? 6000 : 3000);
                int maxLength = 240;
                int minLength = (minArea / 240) + 1;
                minLength = (minLength < 6 ? 6 : minLength);

                //int maxLength = (lnI == 0 ? 100 : lnI == 1 ? 100 : lnI == 2 ? 100 : lnI == 3 ? 100 : lnI == 4 ? 150 : 150);

                bool legal = false;
                while (!legal)
                {
                    legal = true;

                    if (lnI <= 5 || lnI >= 14)
                    {
                        islandSize[lnI, 0] = minLength + (r1.Next() % (maxLength - minLength));
                        islandSize[lnI, 1] = minLength + (r1.Next() % (maxLength - minLength));

                        int totalArea = islandSize[lnI, 0] * islandSize[lnI, 1];
                        if (totalArea < minArea || totalArea > maxArea)
                        {
                            legal = false;
                            continue;
                        }
                    }
                    else if (lnI == 6) // sea cave
                    {
                        islandSize[lnI, 0] = 7;
                        islandSize[lnI, 1] = 5;
                    }
                    else if (lnI == 7) // treasures
                    {
                        islandSize[lnI, 0] = 1;
                        islandSize[lnI, 1] = 1;
                    }
                    else if (lnI == 8) // World Tree
                    {
                        islandSize[lnI, 0] = 6;
                        islandSize[lnI, 1] = 7;
                    }
                    else if (lnI == 9) // Rubiss Island
                    {
                        islandSize[lnI, 0] = 1;
                        islandSize[lnI, 1] = 1;
                    }
                    else if (lnI == 10) // Lake Cave
                    {
                        islandSize[lnI, 0] = 5;
                        islandSize[lnI, 1] = 5;
                    }
                    else if (lnI == 11) // Moon Tower
                    {
                        islandSize[lnI, 0] = 13;
                        islandSize[lnI, 1] = 13;
                    }
                    else if (lnI == 12) // rhone cave
                    {
                        islandSize[lnI, 0] = 4;
                        islandSize[lnI, 1] = 6;
                    }
                    else if (lnI == 13) // Mirror Of Ra spot
                    {
                        islandSize[lnI, 0] = 3;
                        islandSize[lnI, 1] = 4;
                    } 

                    // pick island starting spot randomly.
                    if (lnI == 1)
                    {
                        islandLocation[1, 0] = islandLocation[0, 0] + islandSize[0, 0] + 1;
                        if (islandLocation[1, 0] + islandSize[1, 0] > 255)
                        {
                            legal = false;
                            continue;
                        }
                        else
                            islandLocation[1, 1] = islandLocation[0, 1];
                        if (islandLocation[1, 1] + islandSize[1, 1] > 255)
                            islandLocation[1, 1] = 255 - islandSize[1, 1];
                    }
                    else
                    {
                        islandLocation[lnI, 0] = r1.Next() % (255 - islandSize[lnI, 0]) + 1;
                        islandLocation[lnI, 1] = r1.Next() % (255 - islandSize[lnI, 1]) + 1;
                    }

                    // Make sure it doesn't run into another island...
                    if (lnI <= 9 || lnI >= 14)
                    {
                        for (int lnJ = islandLocation[lnI, 0] - 1; lnJ < islandLocation[lnI, 0] + islandSize[lnI, 0] + 1; lnJ++)
                            for (int lnK = islandLocation[lnI, 1] - 1; lnK < islandLocation[lnI, 1] + islandSize[lnI, 1] + 1; lnK++)
                                if (map[lnJ, lnK] != 0x04)
                                {
                                    legal = false;
                                    break;
                                }
                    } else if (lnI >= 10 && lnI <= 13) // Except the Rhone cave.  There we want to make sure it IS on land...
                    {
                        for (int lnJ = islandLocation[lnI, 0] - 1; lnJ < islandLocation[lnI, 0] + islandSize[lnI, 0] + 1; lnJ++)
                            for (int lnK = islandLocation[lnI, 1] - 1; lnK < islandLocation[lnI, 1] + islandSize[lnI, 1] + 1; lnK++)
                                if (map[lnJ, lnK] == 0x04)
                                {
                                    legal = false;
                                    break;
                                }
                    }

                    if (lnI == 11 || lnI == 12)
                    {
                        // ... but not on Hargon's Island...
                        if (islandLocation[lnI, 0] >= islandLocation[5, 0] && islandLocation[lnI, 0] <= islandLocation[5, 0] + islandSize[5, 0] &&
                            islandLocation[lnI, 1] >= islandLocation[5, 1] && islandLocation[lnI, 1] <= islandLocation[5, 1] + islandSize[5, 1])
                        {
                            legal = false;
                            continue;
                        }
                    }
                    else if (lnI == 10 || lnI == 13) // Ensure the lake cave and the mirror of ra spot are on the first two islands.
                    {
                        if (!((islandLocation[lnI, 0] >= islandLocation[2, 0] && islandLocation[lnI, 0] <= islandLocation[2, 0] + islandSize[2, 0] &&
                            islandLocation[lnI, 1] >= islandLocation[2, 1] && islandLocation[lnI, 1] <= islandLocation[2, 1] + islandSize[2, 1]) || 
                            (islandLocation[lnI, 0] >= islandLocation[3, 0] && islandLocation[lnI, 0] <= islandLocation[3, 0] + islandSize[3, 0] &&
                            islandLocation[lnI, 1] >= islandLocation[3, 1] && islandLocation[lnI, 1] <= islandLocation[3, 1] + islandSize[3, 1])))
                        {
                            legal = false;
                            continue;
                        }
                    }

                    if (!legal)
                        continue;

                    if (lnI == 2)
                    {
                        bool towerLegal = false;
                        while (towerLegal == false)
                        {
                            // Need to locate the two Dragon's Horns now.
                            int dhLoc1 = (r1.Next() % (islandSize[1, 1])) + islandLocation[1, 1];
                            if (map[islandLocation[1, 0], dhLoc1 - 1] != 0x04 && map[islandLocation[1, 0] - 2, dhLoc1] != 0x04)
                            {
                                map[islandLocation[1, 0] + 1, dhLoc1 - 1] = 0x0a;
                                romData[0xa2f6] = (byte)(islandLocation[1, 0] + 1);
                                romData[0xa2f5] = (byte)(dhLoc1 - 1);

                                map[islandLocation[1, 0] - 3, dhLoc1] = 0x0a;
                                romData[0xa2f9] = (byte)(islandLocation[1, 0] - 3);
                                romData[0xa2f8] = (byte)(dhLoc1);
                                towerLegal = true;
                            }
                        }
                    }

                    if (lnI == 5) // Snow!
                    {
                        romData[0x3e2ac] = (byte)islandLocation[5, 1];
                        romData[0x3e2b0] = (byte)(islandLocation[5, 1] + islandSize[5, 1]);
                        romData[0x3e2b6] = (byte)islandLocation[5, 0];
                        romData[0x3e2ba] = (byte)(islandLocation[5, 0] + islandSize[5, 0]);
                    }

                    // We also have to adjust for monster zones.  On island 2, we only want monster zones 0x00-0x08.  On island 3, 0x0d-0x11.  On island 5, 0x32-0x33.  
                    // On all other islands, it's a free for all!
                    for (int lnM = (int)Math.Floor((double)islandLocation[lnI, 0] / 16); lnM <= Math.Ceiling((double)((islandLocation[lnI, 0] + islandSize[lnI, 0]) / 16)); lnM++)
                        for (int lnN = (int)Math.Floor((double)islandLocation[lnI, 1] / 16); lnN <= Math.Ceiling((double)((islandLocation[lnI, 1] + islandSize[lnI, 1]) / 16)); lnN++)
                        {
                            if (lnM >= 16 || lnN >= 16) break;
                            if (lnI == 2)
                                monsterZones[lnM, lnN] = r1.Next() % 9;
                            else if (lnI == 3)
                                monsterZones[lnM, lnN] = r1.Next() % 5 + 0x0d;
                            else if (lnI == 5)
                                monsterZones[lnM, lnN] = r1.Next() % 2 + 0x32;
                            else if (monsterZones[lnM, lnN] == 0xff)
                            {
                                while (monsterZones[lnM, lnN] > 0x27 || (monsterZones[lnM, lnN] >= 0x1c && monsterZones[lnM, lnN] <= 0x1f))
                                    monsterZones[lnM, lnN] = r1.Next() % 19 + 0x15;
                                if (monsterZones[lnM, lnN] == 0x26) monsterZones[lnM, lnN] = 0x39;
                                if (monsterZones[lnM, lnN] == 0x27) monsterZones[lnM, lnN] = 0x3b;
                            }
                            // In all cases, add 64 * (0..3) for sailing.
                            monsterZones[lnM, lnN] += (64 * (r1.Next() % 4));
                        }

                    int lnTileCounter = 0;
                    for (int lnJ = islandLocation[lnI, 0]; lnJ < islandLocation[lnI, 0] + islandSize[lnI, 0]; lnJ++)
                        for (int lnK = islandLocation[lnI, 1]; lnK < islandLocation[lnI, 1] + islandSize[lnI, 1]; lnK++)
                        {
                            if (lnI <= 5 || lnI >= 14)
                            {
                                if (lnI == 5 && (lnJ == islandLocation[lnI, 0] || lnJ == islandLocation[lnI, 0] + islandSize[lnI, 0] - 1 ||
                                    lnK == islandLocation[lnI, 1] || lnK == islandLocation[lnI, 1] + islandSize[lnI, 1] - 1))
                                    map[lnJ, lnK] = 0x05; // Place a mountain border around Hargon's island
                                else
                                    map[lnJ, lnK] = (lnI == 0 ? 0x02 : lnI == 1 ? 0x03 : lnI == 2 ? 0x01 : lnI == 3 ? 0x06 : lnI == 4 ? 0x07 : 0x07);
                            }
                            else if (lnI == 6)
                            {
                                if (seaCaveMountains.Contains(lnTileCounter)) map[lnJ, lnK] = 0x05;
                                else if (seaCaveShoals.Contains(lnTileCounter)) map[lnJ, lnK] = 0x13;
                                else if (seaCaveCave == lnTileCounter)
                                {
                                    map[lnJ, lnK] = 0x0c;
                                    // Also need to update the ROM to indicate where the Sea Cave is in case the Moon Fragment was used.
                                    romData[0xa2e3] = (byte)lnK;
                                    romData[0xa2e4] = (byte)lnJ;

                                    romData[0x198ef] = romData[0x3e154] = (byte)(islandLocation[lnI, 1] - 2);
                                    romData[0x198f3] = romData[0x3e158] = (byte)(islandLocation[lnI, 1] + islandSize[lnI, 1] + 2);
                                    romData[0x198f9] = romData[0x3e15e] = (byte)(islandLocation[lnI, 0] - 2);
                                    romData[0x198fd] = romData[0x3e162] = (byte)(islandLocation[lnI, 0] + islandSize[lnI, 0] + 2);

                                    //romData[0x3e154] = (byte)lnK;
                                    //romData[0x3e158] = (byte)lnJ;
                                    //romData[0x3e15e] = (byte)lnK;
                                    //romData[0x3e162] = (byte)lnJ;

                                }
                            }
                            else if (lnI == 7)
                            {
                                map[lnJ, lnK] = 0x13;
                                // Also need to update the ROM to indicate the treasures spot.  (make sure it's vertical - 1!)
                                romData[0x19f1c] = (byte)lnK;
                                romData[0x19f1d] = (byte)(lnJ + 1);
                            }
                            else if (lnI == 8)
                            {
                                if (worldTreeMountains.Contains(lnTileCounter)) map[lnJ, lnK] = 0x05;
                                else if (worldTreeDesert.Contains(lnTileCounter)) map[lnJ, lnK] = 0x02;
                                else if (worldTree == lnTileCounter) map[lnJ, lnK] = 0x03;
                                // Also need to update the ROM to indicate the World Tree location.
                                romData[0x19f20] = (byte)lnK;
                                romData[0x19f21] = (byte)lnJ;
                            }
                            else if (lnI == 9)
                            {
                                map[lnJ, lnK] = 0x0b;
                                romData[0xa2d7] = (byte)lnK;
                                romData[0xa2d8] = (byte)lnJ;
                            }
                            else if (lnI == 10)
                            {
                                if (lnTileCounter == 12) {
                                    map[lnJ, lnK] = 0x0c;
                                    romData[0xa2e0] = (byte)lnK;
                                    romData[0xa2e1] = (byte)lnJ;
                                } 
                                else if ((lakeCaveBridgeDir == 0 && (lnTileCounter == 2 || lnTileCounter == 7)) ||
                                    (lakeCaveBridgeDir == 2 && (lnTileCounter == 17 || lnTileCounter == 22)))
                                    map[lnJ, lnK] = 0x0d;
                                else if ((lakeCaveBridgeDir == 1 && (lnTileCounter == 10 || lnTileCounter == 11)) ||
                                    (lakeCaveBridgeDir == 3 && (lnTileCounter == 13 || lnTileCounter == 14)))
                                    map[lnJ, lnK] = 0x09;
                                else if (lnTileCounter != 0 && lnTileCounter != 4 && lnTileCounter != 20 && lnTileCounter != 24) map[lnJ, lnK] = 0x04;
                            }
                            else if (lnI == 11)
                            {
                                if (lnTileCounter == (6 * 13) + 6) {
                                    map[lnJ, lnK] = 0x0a;
                                    romData[0xa2f2] = (byte)lnK;
                                    romData[0xa2f3] = (byte)lnJ;
                                }
                                else if (moonTowerDesert.Contains(lnTileCounter)) {
                                    map[lnJ, lnK] = 0x02;

                                    if (moonTowerRiverDir == 0 || moonTowerRiverDir == 2)
                                    {
                                        romData[0x3e000] = (byte)lnK;
                                        romData[0x3e006] = (byte)(lnJ < romData[0x3e006] ? lnJ : romData[0x3e006]);
                                        romData[0x3e00a] = (byte)(lnJ > romData[0x3e00a] - 1 ? lnJ + 1 : romData[0x3e00a]);
                                    }
                                    else
                                    {
                                        romData[0x3dffe] = 0x13;
                                        romData[0x3e004] = 0x12;
                                        romData[0x3e000] = (byte)lnJ;
                                        romData[0x3e006] = (byte)(lnK < romData[0x3e006] ? lnK : romData[0x3e006]);
                                        romData[0x3e00a] = (byte)(lnK > romData[0x3e00a] - 1 ? lnK + 1 : romData[0x3e00a]);
                                    }
                                }
                                else if (moonTowerMountains.Contains(lnTileCounter)) map[lnJ, lnK] = 0x05;
                                else if (moonTowerRiver.Contains(lnTileCounter)) map[lnJ, lnK] = 0x04;
                            }
                            else if (lnI == 12)
                            {
                                if (lnTileCounter == rhoneCaveX) {
                                    map[lnJ, lnK] = 0x0c;
                                    romData[0xa2ef] = (byte)lnK;
                                    romData[0xa2f0] = (byte)lnJ;
                                    romData[0x3e018] = (byte)lnK;
                                    romData[0x3e01e] = (byte)lnJ;
                                }
                                else if (lnTileCounter < 6) map[lnJ, lnK] = 0x05;
                                else if (lnTileCounter != 18 && lnTileCounter != 23) map[lnJ, lnK] = 0x08;
                                // Also need to update the ROM to indicate the Rhone Cave location.
                                romData[0x196a7] = (byte)islandLocation[lnI, 1];
                                romData[0x196ab] = (byte)(islandLocation[lnI, 1] + islandSize[lnI, 1]);
                                romData[0x196b1] = (byte)islandLocation[lnI, 0];
                                romData[0x196b5] = (byte)(islandLocation[lnI, 0] + islandSize[lnI, 0]);
                            }
                            else if (lnI == 13)
                            {
                                if (lnTileCounter <= 4 || lnTileCounter == 7 || lnTileCounter == 8 || lnTileCounter == 11)
                                    map[lnJ, lnK] = 0x13;
                                else
                                {
                                    map[lnJ, lnK] = 0x08;
                                    // Also need to update the ROM to indicate the new Mirror Of Ra search spot.
                                    romData[0x19f18] = (byte)lnK;
                                    romData[0x19f19] = (byte)lnJ;
                                }
                            } 
                            lnTileCounter++;
                        }
                    if (lnI == 10 || lnI == 11) // Grow the river...
                    {
                        int riverDirection = (lnI == 10 ? lakeCaveRiverDir : moonTowerRiverDir);
                        int riverY = 0;
                        int riverX = 0;
                        if (lnI == 10)
                        {
                            riverY = islandLocation[lnI, 0] + (riverDirection == 0 ? -1 : riverDirection == 2 ? 5 : 2);
                            riverX = islandLocation[lnI, 1] + (riverDirection == 1 ? -1 : riverDirection == 3 ? 5 : 2);
                        } else if (lnI == 11)
                        {
                            riverY = islandLocation[lnI, 0] + (riverDirection == 0 ? -1 : riverDirection == 1 ? 8 : riverDirection == 2 ? 13 : 4);
                            riverX = islandLocation[lnI, 1] + (riverDirection == 0 ? 4 : riverDirection == 1 ? -1 : riverDirection == 2 ? 8 : 13);
                        }
                        bool recentBend = false;
                        int recentPermBend = 3;
                        int recentBendDirection = riverDirection;
                        int origRiverBend = riverDirection;

                        while (riverX >= 0 && riverX <= 255 && riverY >= 0 && riverY <= 255 && map[riverY, riverX] != 0x04)
                        {
                            map[riverY, riverX] = 0x04;
                            riverX += (riverDirection == 1 ? -1 : riverDirection == 3 ? 1 : 0);
                            riverY += (riverDirection == 0 ? -1 : riverDirection == 2 ? 1 : 0);
                            
                            if (recentPermBend > 0)
                            {
                                recentPermBend--;
                            }
                            // 75% chance of bend going back to where it was before (guaranteed if going against the original direction of the river)
                            else if (recentBend && (r1.Next() % 4 != 0 || riverDirection == origRiverBend - 2 || riverDirection == origRiverBend + 2))
                            {
                                riverDirection = recentBendDirection;
                                recentPermBend = 2;
                                recentBend = false;
                            }
                            else if (recentBend)
                            {
                                recentPermBend = 2;
                                recentBend = false;
                            }
                            else if (r1.Next() % 5 == 0) // 20% chance of a bend in the river
                            {
                                recentBend = true;
                                recentBendDirection = riverDirection;
                                riverDirection += (r1.Next() % 2 == 0 ? 1 : -1);
                                riverDirection = (riverDirection < 0 ? 3 : riverDirection > 3 ? 0 : riverDirection);
                            }
                        }
                    }
                }

                attempts = 0;
                if (lnI <= 5 || lnI >= 14) islands++;
                if (lnI <= 5) islands++;
            }

            // We'll place all of the castles now.
            for (int lnI = 0; lnI < 7; lnI++)
            {
                int castleX = 0;
                int castleY = 0;
                // Make sure the first two castles are on the first island.
                if (lnI == 0 || lnI == 1)
                {
                    castleY = (r1.Next() % islandSize[2, 0]) + islandLocation[2, 0];
                    castleX = (r1.Next() % islandSize[2, 1]) + islandLocation[2, 1];
                } else if (lnI == 6)
                {
                    castleY = (r1.Next() % (islandSize[5, 0] - 4)) + islandLocation[5, 0] + 2;
                    castleX = (r1.Next() % (islandSize[5, 1] - 4)) + islandLocation[5, 1] + 2;
                } else if (lnI == 2)
                {
                    // Osterfair castle - not on Hargon's Island
                    int randomLand = 5;
                    while (randomLand == 5) randomLand = generateRandomIsland(islands, r1);

                    castleY = (r1.Next() % islandSize[randomLand, 0]) + islandLocation[randomLand, 0];
                    castleX = (r1.Next() % islandSize[randomLand, 1]) + islandLocation[randomLand, 1];
                } else
                {
                    int randomLand = generateRandomIsland(islands, r1);
                    castleY = (r1.Next() % (islandSize[randomLand, 0] - 4)) + islandLocation[randomLand, 0] + 2;
                    castleX = (r1.Next() % (islandSize[randomLand, 1] - 4)) + islandLocation[randomLand, 1] + 2;
                }

                int byteToUse = (lnI == 0 ? 0xa28f : lnI == 1 ? 0xa295 : lnI == 2 ? 0xa29b : lnI == 3 ? 0xa2a1 : lnI == 4 ? 0xa2a4 : lnI == 5 ? 0xa2e9 : 0xa2b3);
                romData[byteToUse] = (byte)(castleX + 1);
                romData[byteToUse + 1] = (byte)(castleY + 1);
                if (lnI == 5) // Charlock castle, out of order as far as byte sequence is concerned.
                {
                    romData[0xa334] = (byte)(castleX);
                    romData[0xa335] = (byte)(castleY + 1);
                }
                else
                {
                    romData[byteToUse + 0x7e] = (byte)(castleX);
                    romData[byteToUse + 1 + 0x7e] = (byte)(castleY + 1);
                }
                if (lnI == 6)
                {
                    romData[0xa301] = (byte)(castleX);
                    romData[0xa302] = (byte)(castleY + 1);
                }

                // Return points
                if (lnI == 0 || lnI == 1 || lnI == 3 || lnI == 4)
                {
                    int byteMultiplier = lnI - (lnI >= 3 ? 1 : 0);
                    romData[0xa27a + (3 * byteMultiplier)] = (byte)(castleX);
                    if (map[castleX, castleY + 2] == 0x04)
                        romData[0xa27a + (3 * byteMultiplier) + 1] = (byte)(castleY + 2);
                    else
                        romData[0xa27a + (3 * byteMultiplier) + 1] = (byte)(castleY + 1);
                    shipPlacement(0x1bf84 + (2 * byteMultiplier), castleY, castleX);
                }

                for (int lnJ = 0; lnJ < 4; lnJ++)
                    map[castleY + (lnJ % 2), castleX + (lnJ / 2)] = (lnJ == 0 ? 0x00 : lnJ == 1 ? 0x10 : lnJ == 2 ? 0x12 : 0x11);
            }

            // Now we'll place all of the towns now.
            for (int lnI = 0; lnI < 7; lnI++)
            {
                int castleX = 0;
                int castleY = 0;
                // Make sure the first town is on the first island.
                if (lnI == 0)
                {
                    castleY = (r1.Next() % islandSize[2, 0]) + islandLocation[2, 0];
                    castleX = (r1.Next() % islandSize[2, 1]) + islandLocation[2, 1];
                }
                else if (lnI == 1)
                {
                    castleY = (r1.Next() % islandSize[3, 0]) + islandLocation[3, 0];
                    castleX = (r1.Next() % islandSize[3, 1]) + islandLocation[3, 1];
                }
                else if (lnI == 2)
                {
                    castleY = (r1.Next() % islandSize[0, 0]) + islandLocation[0, 0];
                    castleX = (r1.Next() % islandSize[0, 1]) + islandLocation[0, 1];
                }
                else
                {
                    int randomLand = generateRandomIsland(islands, r1);
                    castleY = (r1.Next() % (islandSize[randomLand, 0] - 4)) + islandLocation[randomLand, 0] + 2;
                    castleX = (r1.Next() % (islandSize[randomLand, 1] - 4)) + islandLocation[randomLand, 1] + 2;
                }

                int byteToUse = (lnI == 0 ? 0xa292 : lnI == 1 ? 0xa298 : lnI == 2 ? 0xa29e : lnI == 3 ? 0xa2a7 : lnI == 4 ? 0xa2aa : lnI == 5 ? 0xa2ad : 0xa2b0);
                romData[byteToUse] = (byte)(castleX + 1);
                romData[byteToUse + 1] = (byte)(castleY);
                romData[byteToUse + 0x7e] = (byte)(castleX);
                romData[byteToUse + 1 + 0x7e] = (byte)(castleY);

                for (int lnJ = 0; lnJ < 2; lnJ++)
                    map[castleY, castleX + lnJ] = (lnJ == 0 ? 0x0e : 0x0f);

                if (lnI == 2)
                    shipPlacement(0x3d6be, castleY, castleX);
                // Return points
                else if (lnI == 1)
                {
                    romData[0xa27a + 18] = (byte)(castleX);
                    if (map[castleX, castleY + 1] == 0x04)
                        romData[0xa27a + 19] = (byte)(castleY + 2);
                    else
                        romData[0xa27a + 19] = (byte)(castleY + 1);
                    shipPlacement(0x1bf84 + 12, castleY, castleX);
                }
                else if (lnI == 6)
                {
                    romData[0xa27a + 12] = (byte)(castleX);
                    if (map[castleX, castleY + 1] == 0x04)
                        romData[0xa27a + 13] = (byte)(castleY + 2);
                    else
                        romData[0xa27a + 13] = (byte)(castleY + 1);
                    shipPlacement(0x1bf84 + 8, castleY, castleX);
                    shipPlacement(0x1bf84 + 10, castleY, castleX);
                }
            }

            // Then the monoliths.
            for (int lnI = 0; lnI < 12; lnI++)
            {
                int castleX = 0;
                int castleY = 0;
                // Make sure the first monolith is on the first island.
                if (lnI == 1)
                {
                    castleY = (r1.Next() % islandSize[2, 0]) + islandLocation[2, 0];
                    castleX = (r1.Next() % islandSize[2, 1]) + islandLocation[2, 1];
                }
                else if (lnI == 8)
                {
                    castleY = (r1.Next() % islandSize[3, 0]) + islandLocation[3, 0];
                    castleX = (r1.Next() % islandSize[3, 1]) + islandLocation[3, 1];
                }
                else if (lnI == 7)
                {
                    castleY = (r1.Next() % islandSize[1, 0]) + islandLocation[1, 0];
                    castleX = (r1.Next() % islandSize[1, 1]) + islandLocation[1, 1];
                }
                else if (lnI == 6)
                {
                    castleY = (r1.Next() % (islandSize[5, 0] - 4)) + islandLocation[5, 0] + 2;
                    castleX = (r1.Next() % (islandSize[5, 1] - 4)) + islandLocation[5, 1] + 2;
                }
                else
                {
                    int randomLand = generateRandomIsland(islands, r1);
                    castleY = (r1.Next() % (islandSize[randomLand, 0] - 4)) + islandLocation[randomLand, 0] + 2;
                    castleX = (r1.Next() % (islandSize[randomLand, 1] - 4)) + islandLocation[randomLand, 1] + 2;
                }

                int byteToUse = (lnI < 11 ? 0xa2b6 + (lnI * 3) : 0xa2da);
                romData[byteToUse] = (byte)castleX;
                romData[byteToUse + 1] = (byte)(castleY);

                map[castleY, castleX] = 0x0b;

                if (lnI == 6)
                {
                    romData[0xa27a + 15] = (byte)(castleX);
                    if (map[castleX, castleY + 1] == 0x04)
                        romData[0xa27a + 16] = (byte)(castleY - 1);
                    else
                        romData[0xa27a + 16] = (byte)(castleY + 1);
                }
            }

            // Then the caves.
            for (int lnI = 0; lnI < 6; lnI++)
            {
                int castleX = 0;
                int castleY = 0;
                // Make sure the first cave is on the first island.
                if (lnI == 2)
                {
                    castleY = (r1.Next() % islandSize[2, 0]) + islandLocation[2, 0];
                    castleX = (r1.Next() % islandSize[2, 1]) + islandLocation[2, 1];
                }
                else if (lnI == 3)
                {
                    castleY = (r1.Next() % islandSize[3, 0]) + islandLocation[3, 0];
                    castleX = (r1.Next() % islandSize[3, 1]) + islandLocation[3, 1];
                }
                else if (lnI == 4)
                {
                    castleY = (r1.Next() % (islandSize[5, 0] - 4)) + islandLocation[5, 0] + 2;
                    castleX = (r1.Next() % (islandSize[5, 1] - 4)) + islandLocation[5, 1] + 2;
                }
                else
                {
                    int randomLand = generateRandomIsland(islands, r1);
                    castleY = (r1.Next() % (islandSize[randomLand, 0] - 4)) + islandLocation[randomLand, 0] + 2;
                    castleX = (r1.Next() % (islandSize[randomLand, 1] - 4)) + islandLocation[randomLand, 1] + 2;
                }
                map[castleY, castleX] = 0x0c;

                int byteToUse = (lnI == 0 ? 0xa2dd : lnI == 1 ? 0xa2fb : lnI == 2 ? 0xa2fe : lnI == 3 ? 0xa304 : lnI == 4 ? 0xa307 : 0xa30a);
                romData[byteToUse] = (byte)castleX;
                romData[byteToUse + 1] = (byte)(castleY);
            }

            // Finally the towers
            for (int lnI = 0; lnI < 2; lnI++)
            {
                int castleX = 0;
                int castleY = 0;
                int randomLand = generateRandomIsland(islands, r1);
                castleY = (r1.Next() % (islandSize[randomLand, 0] - 4)) + islandLocation[randomLand, 0] + 2;
                castleX = (r1.Next() % (islandSize[randomLand, 1] - 4)) + islandLocation[randomLand, 1] + 2;

                map[castleY, castleX] = 0x0a;
                int byteToUse = (lnI == 0 ? 0xa2e6 : 0xa2ec);
                romData[byteToUse] = (byte)castleX;
                romData[byteToUse + 1] = (byte)castleY;
            }

            // Now let's enter all of this into the ROM...
            int lnPointer = 0x9f97;

            for (int lnI = 0; lnI <= 256; lnI++) // <---- There is a final pointer for lnI = 256, probably indicating the conclusion of the map.
            {
                romData[0xdda5 + (lnI * 2)] = (byte)(lnPointer % 256);
                romData[0xdda6 + (lnI * 2)] = (byte)(lnPointer / 256);

                int lnJ = 0;
                while (lnI < 256 && lnJ < 256)
                {
                    if (map[lnI, lnJ] >= 1 && map[lnI, lnJ] <= 7)
                    {
                        int tileNumber = 0;
                        int numberToMatch = map[lnI, lnJ];
                        while (lnJ < 256 && tileNumber < 32 && map[lnI, lnJ] == numberToMatch && tileNumber < 32)
                        {
                            tileNumber++;
                            lnJ++;
                        }
                        romData[lnPointer + 0x4010] = (byte)((0x20 * numberToMatch) + (tileNumber - 1));
                        lnPointer++;
                    } else
                    {
                        romData[lnPointer + 0x4010] = (byte)map[lnI, lnJ];
                        lnPointer++;
                        lnJ++;
                    }
                }
            }

            // Enter monster zones
            for (int lnI = 0; lnI < 16; lnI++)
                for (int lnJ = 0; lnJ < 16; lnJ++)
                {
                    if (monsterZones[lnI, lnJ] == 0xff)
                        monsterZones[lnI, lnJ] = (r1.Next() % 60) + ((r1.Next() % 4) * 64);
                    romData[0x103d6 + (lnI * 16) + lnJ] = (byte)monsterZones[lnI, lnJ];
                }
        }

        private int generateRandomIsland(int islands, Random r1)
        {
            int randomLand = r1.Next() % islands;
            if (randomLand < 12) randomLand /= 2; else randomLand += 2;
            return randomLand;
        }

        private void shipPlacement(int byteToUse, int top, int left, int maxLake = 0)
        {
            int minDirection = -99;
            int minDistance = 999;
            int finalX = 0;
            int finalY = 0;
            int distance = 0;
            int lnJ = top;
            int lnK = left;
            for (int lnI = 0; lnI < 4; lnI++)
            {
                lnJ = top;
                lnK = left;
                if (lnI == 0)
                {
                    while (island[lnJ, lnK] != maxLake && distance < 200)
                    {
                        distance++;
                        lnJ = (lnJ == 0 ? 255 : lnJ - 1);
                    }
                } else if (lnI == 1)
                {
                    while (island[lnJ, lnK] != maxLake && distance < 200)
                    {
                        distance++;
                        lnJ = (lnJ == 255 ? 0 : lnJ + 1);
                    }
                }
                else if (lnI == 2)
                {
                    while (island[lnJ, lnK] != maxLake && distance < 200)
                    {
                        distance++;
                        lnK = (lnK == 255 ? 0 : lnK + 1);
                    }
                } else
                {
                    while (island[lnJ, lnK] != maxLake && distance < 200)
                    {
                        distance++;
                        lnK = (lnK == 0 ? 255 : lnK - 1);
                    }
                }
                if (distance < minDistance)
                {
                    minDistance = distance;
                    minDirection = lnI;
                    finalX = lnK;
                    finalY = lnJ;
                }
                distance = 0;
            }
            romData[byteToUse] = (byte)(finalX);
            romData[byteToUse + 1] = (byte)(finalY);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFileName.Text = openFileDialog1.FileName;
                runChecksum();
            }
        }

        private void runChecksum()
        {
            try
            {
                using (var md5 = SHA1.Create())
                {
                    using (var stream = File.OpenRead(txtFileName.Text))
                    {
                        lblSHAChecksum.Text = BitConverter.ToString(md5.ComputeHash(stream)).ToLower().Replace("-", "");
                    }
                }
            } catch
            {
                lblSHAChecksum.Text = "????????????????????????????????????????";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loading = true;
            txtSeed.Text = (DateTime.Now.Ticks % 2147483647).ToString();

            try
            {
                using (TextReader reader = File.OpenText("lastFile.txt"))
                {
                    txtFileName.Text = reader.ReadLine();
                    runChecksum();
                    txtCompare.Text = reader.ReadLine();
                    txtSeed.Text = reader.ReadLine();
                    txtFlags.Text = reader.ReadLine();
                    txtPrinceName.Text = reader.ReadLine();
                    txtPrincessName.Text = reader.ReadLine();
                    // flagLoad(); <---- This gets called via the previous line.
                }
            }
            catch
            {
                // ignore error
                txtPrinceName.Text = "Bran";
                txtPrincessName.Text = "Peta";
            } finally
            {
                loading = false;
            }
        }

        private void btnNewSeed_Click(object sender, EventArgs e)
        {
            txtSeed.Text = (DateTime.Now.Ticks % 2147483647).ToString();
        }

        private void btnRandomize_Click(object sender, EventArgs e)
        {
            if (lblSHAChecksum.Text != lblReqChecksum.Text)
            {
                if (MessageBox.Show("The checksum of the ROM does not match the required checksum.  Patch anyway?", "Checksum mismatch", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
            }

            if (!loadRom())
                return;
            if (txtSeed.Text == "test1")
                halfExpAndGoldReq(true);
            else if (txtSeed.Text == "textGet")
            {
                textGet();
                return;
            }
            else
            {
                if (chkChangeStatsToRemix.Checked) changeStatsToRemix(); // Don't bother if insane random is checked, since all of the stats will change anyway!
                //if (chkHalfExpGoldReq.Checked)
                //if (chkDoubleXP.Checked)
                //doubleExp();
                halfExpAndGoldReq();
                superRandomize();
            }

            renamePrincePrincess();
            speedUpBattles();
            skipPrologue();
            reviveAllCharsOnCOD();
            saveRom();
        }

        private void renamePrincePrincess()
        {
            // Rename the starting characters.
            for (int lnI = 0; lnI < 16; lnI++)
            {
                string name = (lnI < 8 ? txtPrinceName.Text : txtPrincessName.Text);
                for (int lnJ = 0; lnJ < 8; lnJ++)
                {
                    romData[0x1ad49 + (8 * lnI) + lnJ] = 0x5f;
                    try
                    {
                        char character = Convert.ToChar(name.Substring(lnJ, 1));
                        if (character >= 0x30 && character <= 0x39)
                            romData[0x1ad49 + (8 * lnI) + lnJ] = (byte)(character - 47);
                        if (character >= 0x41 && character <= 0x5a)
                            romData[0x1ad49 + (8 * lnI) + lnJ] = (byte)(character - 29);
                        if (character >= 0x61 && character <= 0x7a)
                            romData[0x1ad49 + (8 * lnI) + lnJ] = (byte)(character - 87);
                    }
                    catch
                    {
                        romData[0x1ad49 + (8 * lnI) + lnJ] = 0x5f; // no more characters to process - make the rest of the characters blank
                    }
                }
            }
        }

        private void randomizeMonsterStats(Random r1)
        {
            // Totally randomize monsters (13805-13cd2)
            for (int lnI = 0; lnI < 80; lnI++) // Do not adjust Hargon or Malroth.
            {
                //0 - All Bits used for Maximum Hit Points
                //1 - Bits 0 - 3 unused, Bits 4 - 7 used for chance to evade attack out of 64
                //2 - All Bits used for Maximum Gold Dropped
                //3 - All Bits used for Experience Points Given
                //4 - All Bits used for Agility
                //5 - All Bits used for Attack Power
                //6 - All Bits used for Defense Power
                //7 - Bits 0, 1, 2 used for Damage Causing Spells Resistance out of 7
                //  ---- Bits 3, 4, 5 used for Sleep Resistance out of 7
                //  ---- Bits 6 and 7 used for Attack Pattern Probabilities
                //      List 1 Pattern 1 32 / 256, Pattern 2 32 / 256, Pattern 3 32 / 256, Pattern 4 32 / 256, Pattern 5 32 / 256, Pattern 6 32 / 256, Pattern 7 32 / 256, Pattern 8 32 / 256
                //      List 2 Pattern 1 38 / 256, Pattern 2 39 / 256, Pattern 3 33 / 256, Pattern 4 34 / 256, Pattern 5 30 / 256, Pattern 6 30 / 256, Pattern 7 26 / 256, Pattern 8 26 / 256
                //      List 3 Pattern 1 46 / 256, Pattern 2 42 / 256, Pattern 3 38 / 256, Pattern 4 34 / 256, Pattern 5 30 / 256, Pattern 6 26 / 256, Pattern 7 22 / 256, Pattern 8 18 / 256
                //      List 4 Pattern 1 100 / 256, Pattern 2 50 / 256, Pattern 3 28 / 256, Pattern 4 24 / 256, Pattern 5 19 / 256, Pattern 6 15 / 256, Pattern 7 12 / 256, Pattern 8 08 / 256
                //8 - Bits 0, 1, 2 used for Stopspell resistance out of 7
                //  ---- Bits 3, 4, 5 used for Defeat resistance out of 7
                //  ---- Bits 6 and 7 used for Experience Points Given in Multiples of 256
                //9 - Bits 0, 1, 2 used for Surround Resistance out of 7
                //  ---- Bits 3, 4, 5 used for Defense Resistance out of 7
                //  ---- Bits 6 and 7 used for Experience Points Given in Multiples of 1024
                //10 - Bits 0 - 3 used for Attack Pattern 1, Bits 4 - 7 used for Attack Pattern 2
                //11 - Bits 0 - 3 used for Attack Pattern 3, Bits 4 - 7 used for Attack Pattern 4
                //12 - Bits 0 - 3 used for Attack Pattern 5, Bits 4 - 7 used for Attack Pattern 6
                //13 - Bits 0 - 3 used for Attack Pattern 7, Bits 4 - 7 used for Attack Pattern 8
                //14 - Attack Pattern Second Page
                //  ---- Bit 0 Pattern 1, Bit 1 Pattern 2, Bit 2 Pattern 3, Bit 3 Pattern 4
                //  ---- Bit 4 Pattern 5, Bit 5 Pattern 6, Bit 6 Pattern 7, Bit 7 Pattern 8

                //First page
                //#$0 Attack, #$1 Heroic Attack, #$2 Poison Attack, #$3 Faint Attack, #$4 Parry, #$5 Run Away,
                //#$6 Firebal, #$7 Firebane, #$8 Explodet, #$9 Heal*, #$A Healmore*, #$B Heal All*, #$C Heal**,
                //#$D Healmore**, #$E Heal All**, #$F Revive

                //Second page
                //#$0 Defence, #$1 Increase, #$2 Sleep, #$3 Stopspell, #$4 Surround, #$5 Defeat, $6 Sacrifice***,
                //#$7 Weak Flames, #$8 Strong Flames, #$9 Deadly Flames, #$A Poison Breath, #$B Sweet Breath,
                //#$C Call For Help, #$D Two Attacks, #$E concentration byte, #$F Dance Strange Jig
                byte[] enemyStats = { romData[0x13805 + (lnI * 15) + 0], romData[0x13805 + (lnI * 15) + 1], romData[0x13805 + (lnI * 15) + 2], romData[0x13805 + (lnI * 15) + 3], romData[0x13805 + (lnI * 15) + 4],
                    romData[0x13805 + (lnI * 15) + 5], romData[0x13805 + (lnI * 15) + 6], romData[0x13805 + (lnI * 15) + 7], romData[0x13805 + (lnI * 15) + 8], romData[0x13805 + (lnI * 15) + 9],
                    romData[0x13805 + (lnI * 15) + 10], romData[0x13805 + (lnI * 15) + 11], romData[0x13805 + (lnI * 15) + 12], romData[0x13805 + (lnI * 15) + 13], romData[0x13805 + (lnI * 15) + 14] };

                int byteValStart = 0x13805 + (15 * lnI);

                // evade rate... randomize from 0-8 / 64
                if (randomLevel == 4)
                    enemyStats[1] = (byte)((r1.Next() % 9) * 16);
                else
                {
                    enemyStats[1] = (byte)(adjustEnemyStat(r1, enemyStats[1] / 16, 1) * 16);
                    //int evade = enemyStats[1] / 16;
                    //evade += (r1.Next() % (randomLevel == 3 ? 9 : randomLevel == 2 ? 7 : 5)) - (randomLevel == 3 ? 4 : randomLevel == 2 ? 3 : 2);
                    //evade = (evade < 0 ? 0 : evade > 15 ? 15 : evade);
                    //enemyStats[1] = (byte)(evade * 16);
                }
                if (chkGPRandomize.Checked)
                {
                    int gp = enemyStats[2]; // + (r1.Next() % (lnI + 1));
                    gp = adjustEnemyStat(r1, gp, 1);
                    enemyStats[2] = (byte)gp; // (lnI == 0x33 || gp > 255 ? 255 : gp); // Gold Orc gold = 255
                }

                int xp = romData[byteValStart + 3] + ((romData[byteValStart + 8] / 64) * 256) + ((romData[byteValStart + 9] / 64) * 1024);
                if (chkXPRandomize.Checked)
                    xp = adjustEnemyStat(r1, xp, 1, 4095);

                // Agility
                if (randomLevel == 4)
                    enemyStats[4] = (byte)(r1.Next() % 256);
                else
                {
                    enemyStats[4] = (byte)adjustEnemyStat(r1, enemyStats[4], 1);
                    //int agility = enemyStats[4];
                    //agility += (r1.Next() % (randomLevel == 3 ? (agility) : randomLevel == 2 ? (agility / 2) : (agility / 4))) - 
                    //    (randomLevel == 3 ? (agility / 2) : randomLevel == 2 ? (agility / 4) : (agility / 8));
                    //agility = (agility < 0 ? 0 : agility > 255 ? 255 : agility);
                    //enemyStats[4] = (byte)agility;
                }
                int totalAtk = enemyStats[5];
                totalAtk = adjustEnemyStat(r1, totalAtk, 1);
                //totalAtk += (r1.Next() % (randomLevel == 4 ? (totalAtk) : randomLevel == 3 ? (totalAtk * 3 / 4) : randomLevel == 2 ? (totalAtk / 2) : (totalAtk / 4))) -
                //        (randomLevel == 4 ? (totalAtk / 2) : randomLevel == 3 ? (totalAtk * 3 / 8) : randomLevel == 2 ? (totalAtk / 4) : (totalAtk / 8));
                //totalAtk = (totalAtk > 254 ? 254 : totalAtk);
                //int atkRandom = (r1.Next() % 3);
                //int atkDiv2 = (enemyStats[5] / 2) + 1;
                //if (atkRandom == 1)
                //{
                //    totalAtk += (r1.Next() % atkDiv2);
                //}
                //else if (atkRandom == 2)
                //{
                //    totalAtk -= (r1.Next() % atkDiv2);
                //}
                //totalAtk = (totalAtk > 254 ? 254 : totalAtk);
                enemyStats[5] = (byte)totalAtk;

                int res1 = (enemyStats[7] * 8) % 8;
                int res2 = enemyStats[7] % 8;
                int res3 = (enemyStats[8] * 8) % 8;
                int res4 = enemyStats[8] % 8;
                int res5 = (enemyStats[9] * 8) % 8;
                int res6 = enemyStats[9] % 8;

                if (randomLevel == 4)
                {
                    res1 = (r1.Next() % 8);
                    res2 = (r1.Next() % 8);
                    res3 = (r1.Next() % 8);
                    res4 = (r1.Next() % 8);
                    res5 = (r1.Next() % 8);
                    res6 = (r1.Next() % 8);
                } else
                {
                    res3 = (r1.Next() % (int)(Math.Round((decimal)(8 * lnI / 80)) + 1));
                    //res1 = adjustEnemyStat(r1, res1, 2);
                    //res2 = adjustEnemyStat(r1, res2, 2);
                    //res3 = adjustEnemyStat(r1, res3, 2);
                    //res4 = adjustEnemyStat(r1, res4, 2);
                    //res5 = adjustEnemyStat(r1, res5, 2);
                    //res6 = adjustEnemyStat(r1, res6, 2);
                }
                
                enemyStats[7] = (byte)(((r1.Next() % 4) * 64) + (res1 * 8) + res2);
                enemyStats[8] = (byte)((res3 * 8) + res4);
                enemyStats[9] = (byte)((res5 * 8) + res6);

                //byte[] res1 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 3, 4, 5, 6, 7 };
                //byte[] res2 = { 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 3, 4, 5, 6, 7 };
                //byte[] res3 = { 0, 0, 0, 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 5, 6, 7 };
                //byte[] res4 = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
                //byte[] res5 = { 0, 1, 2, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6, 7, 7, 7 };
                //byte[] res6 = { 0, 1, 2, 3, 4, 5, 5, 5, 6, 6, 6, 6, 7, 7, 7, 7 };
                //byte[] res7 = { 0, 1, 2, 3, 4, 5, 6, 7, 7, 7, 7, 7, 7, 7, 7, 7 };
                //if (lnI < 12)
                //{
                //    enemyStats[7] = (byte)(((r1.Next() % 4) * 64) + (res1[r1.Next() % 16] * 8) + (res1[r1.Next() % 16]));
                //    enemyStats[8] = (byte)((res1[r1.Next() % 16] * 8) + (res1[r1.Next() % 16]));
                //    enemyStats[9] = (byte)((res1[r1.Next() % 16] * 8) + (res1[r1.Next() % 16]));
                //}
                //else if (lnI < 24)
                //{
                //    enemyStats[7] = (byte)(((r1.Next() % 7) * 64) + (res2[r1.Next() % 16] * 8) + (res2[r1.Next() % 16]));
                //    enemyStats[8] = (byte)((res2[r1.Next() % 16] * 8) + (res2[r1.Next() % 16]));
                //    enemyStats[9] = (byte)((res2[r1.Next() % 16] * 8) + (res2[r1.Next() % 16]));
                //}
                //else if (lnI < 36)
                //{
                //    enemyStats[7] = (byte)(((r1.Next() % 7) * 64) + (res3[r1.Next() % 16] * 8) + (res3[r1.Next() % 16]));
                //    enemyStats[8] = (byte)((res3[r1.Next() % 16] * 8) + (res3[r1.Next() % 16]));
                //    enemyStats[9] = (byte)((res3[r1.Next() % 16] * 8) + (res3[r1.Next() % 16]));
                //}
                //else if (lnI < 47)
                //{
                //    enemyStats[7] = (byte)(((r1.Next() % 7) * 64) + (res4[r1.Next() % 16] * 8) + (res4[r1.Next() % 16]));
                //    enemyStats[8] = (byte)((res4[r1.Next() % 16] * 8) + (res4[r1.Next() % 16]));
                //    enemyStats[9] = (byte)((res4[r1.Next() % 16] * 8) + (res4[r1.Next() % 16]));
                //}
                //else if (lnI < 58)
                //{
                //    enemyStats[7] = (byte)(((r1.Next() % 7) * 64) + (res5[r1.Next() % 16] * 8) + (res5[r1.Next() % 16]));
                //    enemyStats[8] = (byte)((res5[r1.Next() % 16] * 8) + (res5[r1.Next() % 16]));
                //    enemyStats[9] = (byte)((res5[r1.Next() % 16] * 8) + (res5[r1.Next() % 16]));
                //}
                //else if (lnI < 69)
                //{
                //    enemyStats[7] = (byte)(((r1.Next() % 7) * 64) + (res6[r1.Next() % 16] * 8) + (res6[r1.Next() % 16]));
                //    enemyStats[8] = (byte)((res6[r1.Next() % 16] * 8) + (res6[r1.Next() % 16]));
                //    enemyStats[9] = (byte)((res6[r1.Next() % 16] * 8) + (res6[r1.Next() % 16]));
                //}
                //else
                //{
                //    enemyStats[7] = (byte)(((r1.Next() % 7) * 64) + (res7[r1.Next() % 16] * 8) + (res7[r1.Next() % 16]));
                //    enemyStats[8] = (byte)((res7[r1.Next() % 16] * 8) + (res7[r1.Next() % 16]));
                //    enemyStats[9] = (byte)((res7[r1.Next() % 16] * 8) + (res7[r1.Next() % 16]));
                //}

                byte[] level1Pattern = { 0, 2, 4, 5, 6, 9, 12, 16, 19, 20, 23, 28, 30 };
                byte[] level2Pattern = { 0, 1, 2, 3, 4, 6, 7, 9, 10, 13, 16, 17, 18, 22, 23, 24, 26, 27, 28, 29, 30, 31 };
                byte[] level3Pattern = { 0, 1, 3, 7, 10, 13, 15, 21, 22, 24, 27, 28, 29, 30, 31 };
                byte[] level4Pattern = { 1, 3, 8, 11, 14, 15, 18, 21, 22, 25, 27, 29, 30 };

                byte[] enemyPatterns = { 0, 0, 0, 0, 0, 0, 0, 0 };
                bool[] enemyPage2 = { false, false, false, false, false, false, false, false };
                bool concentration = false;

                //byte[] pattern1 = { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3 };
                //byte[] pattern2 = { 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 3, 3 };
                //byte[] pattern3 = { 0, 0, 0, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 3, 3, 4 };
                //byte[] pattern4 = { 0, 0, 0, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 4, 4, 4 };
                //byte[] pattern5 = { 0, 0, 1, 1, 2, 2, 2, 2, 2, 3, 3, 4, 4, 4, 4, 4 };
                //byte[] pattern6 = { 1, 1, 2, 2, 2, 2, 2, 2, 3, 4, 4, 4, 4, 4, 4, 4 };
                //byte[] pattern7 = { 2, 2, 2, 2, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 };

                //int enemyPattern = 0;

                //if (lnI < 12)
                //    enemyPattern = pattern1[r1.Next() % 16];
                //else if (lnI < 24)
                //    enemyPattern = pattern2[r1.Next() % 16];
                //else if (lnI < 36)
                //    enemyPattern = pattern3[r1.Next() % 16];
                //else if (lnI < 47)
                //    enemyPattern = pattern4[r1.Next() % 16];
                //else if (lnI < 58)
                //    enemyPattern = pattern5[r1.Next() % 16];
                //else if (lnI < 69)
                //    enemyPattern = pattern6[r1.Next() % 16];
                //else
                //    enemyPattern = pattern7[r1.Next() % 16];

                int randomPattern = 0;

                if (randomLevel == 4)
                {
                    randomPattern = 4;
                } else if (randomLevel == 3) // Basically make it equivalent to mcgrew's DW1 Randomizer
                {
                    int rp = (r1.Next() % 100);
                    if (rp >= 50) randomPattern = 4;
                    else if (rp >= 35) randomPattern = 2; else randomPattern = 1;
                } else if (randomLevel == 2)
                {
                    randomPattern = (r1.Next() % 5);
                } else
                {
                    randomPattern = (r1.Next() % 4);
                }

                if (randomPattern == 4)
                {
                    for (int lnJ = 0; lnJ < 8; lnJ++)
                    {
                        // 75% chance of setting a different attack.
                        byte random = (byte)(r1.Next() % 48);
                        if (random >= 1 && random <= 31) // 0 would be fine, but it's already set.
                        {
                            if (random == 30 && concentration)
                                continue; // do NOT set the concentration bit again.  Maintain regular attack.
                            else if ((random == 7 || random == 8 || random == 21 || random == 22 || random == 24 || random == 25) && lnI <= 32 && randomLevel == 4)
                            {
                                lnJ--;
                                continue;
                            }
                            else if ((random == 4 || random == 5 || random == 6 || random == 9 || random == 12 || random == 19 || random == 20 || random == 23 || random == 26) && lnI >= 51 && randomLevel == 4)
                            {
                                lnJ--;
                                continue;
                            }
                            else if (random >= 16)
                                enemyPage2[lnJ] = true;
                            else if (random == 30)
                                concentration = true;

                            enemyPatterns[lnJ] = (byte)(random % 16);
                        }
                    }
                } else if (randomPattern == 3)
                {
                    for (int lnJ = 0; lnJ < 8; lnJ++)
                    {
                        byte random = 0;
                        if (lnI < 20)
                            random = level1Pattern[r1.Next() % level1Pattern.Length];
                        else if (lnI < 40)
                            random = level2Pattern[r1.Next() % level2Pattern.Length];
                        else if (lnI < 60)
                            random = level3Pattern[r1.Next() % level3Pattern.Length];
                        else if (lnI < 80)
                            random = level4Pattern[r1.Next() % level4Pattern.Length];
                        if (random == 30 && concentration)
                            continue; // do NOT set the concentration bit again.  Maintain regular attack.
                        else if (random >= 16)
                            enemyPage2[lnJ] = true;
                        else if (random == 30)
                            concentration = true;
                        enemyPatterns[lnJ] = random;
                    }
                }
                else if (randomPattern == 2) // goofy attack monster
                {
                    for (int lnJ = 0; lnJ < 8; lnJ++)
                    {
                        // 50% chance of setting a different attack.
                        byte random = (byte)(r1.Next() % 10);
                        switch (random)
                        {
                            case 1:
                                enemyPatterns[lnJ] = 1;
                                break;
                            case 2:
                                enemyPatterns[lnJ] = 2;
                                break;
                            case 3:
                                enemyPatterns[lnJ] = 3;
                                break;
                            case 4:
                                enemyPatterns[lnJ] = 4;
                                break;
                            case 5:
                                enemyPatterns[lnJ] = 13;
                                enemyPage2[lnJ] = true;
                                break;
                            case 6:
                                if (!concentration)
                                {
                                    enemyPatterns[lnJ] = 14;
                                    enemyPage2[lnJ] = true;
                                    concentration = true;
                                }
                                break;
                        }
                    }
                } else if (randomPattern == 1)
                {
                    for (int lnJ = 0; lnJ < 8; lnJ++)
                        enemyPatterns[lnJ] = 0;
                } else
                {
                    // keep pattern the way it is.
                }

                //switch (enemyPattern)
                //{
                //    case 0: // leave everything alone; it's a basic attack monster.
                //        break;
                //    case 1: // Give the monster a little goofyness to their attack...
                //        for (int lnJ = 0; lnJ < 8; lnJ++)
                //        {
                //            // 50% chance of setting a different attack.
                //            byte random = (byte)(r1.Next() % 10);
                //            switch (random)
                //            {
                //                case 1:
                //                    enemyPatterns[lnJ] = 1;
                //                    break;
                //                case 2:
                //                    enemyPatterns[lnJ] = 2;
                //                    break;
                //                case 3:
                //                    enemyPatterns[lnJ] = 3;
                //                    break;
                //                case 4:
                //                    enemyPatterns[lnJ] = 4;
                //                    break;
                //                case 5:
                //                    enemyPatterns[lnJ] = 13;
                //                    enemyPage2[lnJ] = true;
                //                    break;
                //                case 6:
                //                    if (!concentration)
                //                    {
                //                        enemyPatterns[lnJ] = 14;
                //                        enemyPage2[lnJ] = true;
                //                        concentration = true;
                //                    }
                //                    break;
                //            }
                //        }
                //        break;
                //    case 2:
                //        for (int lnJ = 0; lnJ < 8; lnJ++)
                //        {
                //            // 75% chance of setting a different attack.
                //            byte random = (byte)(r1.Next() % 48);
                //            if (random >= 1 && random <= 31) // 0 would be fine, but it's already set.
                //            {
                //                if (random == 30 && concentration)
                //                    continue; // do NOT set the concentration bit again.  Maintain regular attack.
                //                else if (random >= 16)
                //                    enemyPage2[lnJ] = true;
                //                else if (random == 30)
                //                    concentration = true;
                //                enemyPatterns[lnJ] = (byte)(random % 16);
                //            }
                //        }
                //        break;
                //    case 3:
                //        for (int lnJ = 0; lnJ < 8; lnJ++)
                //        {
                //            // Normal, heroic, poison, faint, heal, healmore (both self and others), sleep, stopspell, sacrifice, weak flames, 
                //            // poison and sweet breaths, call for help, double attacks, and strange jigs.
                //            byte[] attackPattern = { 0, 0, 0, 0, 0, 1, 2, 3, 9, 10, 12, 13, 18, 19, 22, 23, 26, 27, 28, 29, 31 };
                //            byte random = (attackPattern[r1.Next() % attackPattern.Length]);
                //            if (random >= 1 && random <= 31) // 0 would be fine, but it's already set.
                //            {
                //                if (random == 30 && concentration)
                //                    continue; // do NOT set the concentration bit again.  Maintain regular attack.
                //                else if (random >= 16)
                //                    enemyPage2[lnJ] = true;
                //                else if (random == 30)
                //                    concentration = true;
                //                enemyPatterns[lnJ] = (byte)(random % 16);
                //            }
                //        }
                //        break;
                //    case 4:
                //        for (int lnJ = 0; lnJ < 8; lnJ++)
                //        {
                //            // Double attacks, heroic attacks, firebane, explodet, heal all (both self and party), revive, defeat, 
                //            // sacrifice, strong and deadly flames, and sweet breath.
                //            byte[] attackPattern = { 29, 29, 29, 29, 1, 1, 7, 8, 11, 14, 15, 21, 22, 24, 25, 27 };
                //            byte random = (attackPattern[r1.Next() % attackPattern.Length]);
                //            if (random >= 1 && random <= 31) // 0 would be fine, but it's already set.
                //            {
                //                if (random == 30 && concentration)
                //                    continue; // do NOT set the concentration bit again.  Maintain regular attack.
                //                else if (random >= 16)
                //                    enemyPage2[lnJ] = true;
                //                else if (random == 30)
                //                    concentration = true;
                //                enemyPatterns[lnJ] = (byte)(random % 16);
                //            }
                //        }
                //        break;
                //}

                if (lnI == 0x2f || lnI == 0x41) // Metal slime, Metal Babble
                {
                    enemyPatterns[0] = 5; // run away
                    enemyPage2[0] = false;
                    enemyPatterns[1] = 5; // run away
                    enemyPage2[1] = false;
                    enemyPatterns[2] = 5; // run away
                    enemyPage2[2] = false;
                    enemyPatterns[3] = 5; // run away
                    enemyPage2[3] = false;
                    if (lnI == 0x41)
                    {
                        enemyPatterns[4] = 5; // run away
                        enemyPage2[4] = false;
                        enemyPatterns[5] = 5; // run away
                        enemyPage2[5] = false;
                    }
                    enemyStats[4] = 255; // Agility = max
                    enemyStats[5] = 1; // Strength = minimum
                }
                //if (lnI == 0x05)
                //{ // Healer
                //    enemyPatterns[0] = (byte)((r1.Next() % 3) + 12); // heal, healmore, healall
                //    enemyPatterns[1] = (byte)((r1.Next() % 3) + 12); // heal, healmore, healall
                //}
                //if (lnI == 0x1F)
                //{ // Poison Lily
                //    enemyPatterns[0] = 2;
                //    enemyPage2[0] = false;
                //    enemyPatterns[1] = 10;
                //    enemyPage2[1] = true;
                //}
                //if (lnI == 0x0a || lnI == 0x0e || lnI == 0x1a || lnI == 0x1c || lnI == 0x2f || lnI == 0x40) // Magician, Magidrakee, Magic Baboon, Sorcerer, Magic Vampirus
                //{
                //    enemyPatterns[0] = (byte)((r1.Next() % 15) + 6); // any magic spell
                //    enemyPage2[0] = (enemyPatterns[0] >= 16);
                //    enemyPatterns[1] = (byte)((r1.Next() % 15) + 6); // any magic spell
                //    enemyPage2[1] = (enemyPatterns[1] >= 16);
                //}
                //if (lnI == 0x23 || lnI == 0x46 || lnI == 0x48) // Dragon Fly, Green Dragon, Flame
                //{
                //    enemyPatterns[0] = (byte)((r1.Next() % 3) + 7); // breathe flames
                //    enemyPage2[0] = true;
                //    enemyPatterns[1] = (byte)((r1.Next() % 3) + 7); // breathe flames
                //    enemyPage2[1] = true;
                //}

                if (randomPattern > 0)
                {
                    enemyStats[10] = (byte)((enemyPatterns[0] * 16) + enemyPatterns[1]);
                    enemyStats[11] = (byte)((enemyPatterns[2] * 16) + enemyPatterns[3]);
                    enemyStats[12] = (byte)((enemyPatterns[4] * 16) + enemyPatterns[5]);
                    enemyStats[13] = (byte)((enemyPatterns[6] * 16) + enemyPatterns[7]);
                    enemyStats[14] = (byte)((enemyPage2[0] ? 1 : 0) + (enemyPage2[1] ? 2 : 0) + (enemyPage2[2] ? 4 : 0) + (enemyPage2[3] ? 8 : 0) +
                        (enemyPage2[4] ? 16 : 0) + (enemyPage2[5] ? 32 : 0) + (enemyPage2[6] ? 64 : 0) + (enemyPage2[7] ? 128 : 0));
                }

                //if (lnI == 0x49)
                //    lnI = 0x49;

                xp = (xp < 0 ? 1 : xp); // (float)Math.Round(xp, 0);
                byte xp1 = (byte)(xp > 4095 ? 255 : (xp % 256));
                byte xp2 = (byte)(xp > 4095 ? 192 : ((xp / 256) % 4) * 64);
                byte xp3 = (byte)(xp > 4095 ? 192 : ((xp / 1024)) * 64);

                enemyStats[3] = xp1;
                enemyStats[8] = (byte)(enemyStats[8] + xp2);
                enemyStats[9] = (byte)(enemyStats[9] + xp3);

                for (int lnJ = 0; lnJ < 15; lnJ++)
                    romData[byteValStart + lnJ] = enemyStats[lnJ];
            }
        }

        private int adjustEnemyStat(Random r1, int origStat, int adjLevel, int maxStat = 255)
        {
            int maxAdjustment = origStat / (randomLevel == 4 ? 1 : randomLevel == 3 ? 2 : randomLevel == 2 ? 4 : 8) / adjLevel;
            if (maxAdjustment <= 1)
                return (byte)origStat;
            int finalStat = origStat;

            int direction = r1.Next() % 3;
            if (direction == 0)
            {
                finalStat = origStat - (r1.Next() % (maxAdjustment / 2));
            } else if (direction == 1)
            {
                finalStat = origStat + (r1.Next() % maxAdjustment);
            }
            finalStat = (finalStat < 0 ? 0 : finalStat > maxStat ? maxStat : finalStat);
            return finalStat;
        }

        private void randomizeMonsterZones(Random r1)
        {
            byte[] monsterSize = { 8, 5, 5, 7, 5, 8, 5, 7, 5, 4, 5, 4, 5, 7, 4,
                                  8, 5, 4, 5, 5, 4, 4, 4, 5, 4, 2, 4, 4, 4, 4, 4,
                                  4, 4, 4, 5, 4, 4, 4, 7, 2, 4, 4, 4, 5, 4, 2, 2,
                                  8, 4, 5, 4, 7, 1, 2, 4, 4, 4, 4, 3, 4, 5, 1, 1,
                                  4, 2, 7, 4, 3, 1, 4, 2, 4, 3, 4, 2, 3, 1, 2, 3, 1, 1, 1 };

            // Totally randomize monster zones (but make sure the first 20 zones have easier monsters) (10356-10389, 10519-10680, )
            for (int lnI = 0; lnI < 68; lnI++)
            {
                int byteToUse = 0x10519 + (lnI * 6);
                for (int lnJ = 0; lnJ < 6; lnJ++)
                {
                    // First 11 zones have a 50% chance of a monster in each byte.  All 6 bytes will be at least 128... we don't want any "special fights" in these zones.
                    if (lnI < 11)
                    {
                        romData[byteToUse + lnJ] = (byte)((r1.Next() % (lnI + 6)) + 1);
                    }
                    else if (lnI < 21) // For the next 10 zones, it's a 67% chance.  Still no special fights.
                    {
                        romData[byteToUse + lnJ] = (byte)((r1.Next() % (lnI + 12)) + 1);
                    }
                    else if (lnI == 42 || lnI == 43 || lnI == 44 || lnI == 54 || lnI == 55) // Sea cave.  No special bout here.
                    {
                        romData[byteToUse + lnJ] = (byte)((r1.Next() % 37) + 41);
                    }
                    else if (lnI == 45 || lnI == 46 || lnI == 47 || lnI == 48 || lnI == 49 || lnI == 56) // Rhone cave.  Introduce Atlas chance.
                    {
                        romData[byteToUse + lnJ] = (byte)((r1.Next() % 28) + 51);
                    }
                    else if (lnI == 50 || lnI == 51) // Rhone area.  Introduce Bazuzu chance.
                    {
                        romData[byteToUse + lnJ] = (byte)((r1.Next() % 18) + 62);
                    }
                    else if (lnI == 52 || lnI == 53) // Hargon's Castle.  Introduce Zarlox chance.
                    {
                        romData[byteToUse + lnJ] = (byte)((r1.Next() % 13) + 68);
                    }
                    else // Finally, a 80% chance.  Also introduce a 50% chance of the 19 "special bouts".
                    {
                        romData[byteToUse + lnJ] = (byte)((r1.Next() % 77) + 1);
                    }

                    //// First 11 zones have a 50% chance of a monster in each byte.  All 6 bytes will be at least 128... we don't want any "special fights" in these zones.
                    //if (lnI < 11)
                    //{
                    //    if (r1.Next() % 2 == 0)
                    //    {
                    //        zone = true;
                    //        romData[byteToUse + lnJ] = (byte)((r1.Next() % (lnI + 6)) + 1);
                    //    }
                    //    else
                    //        romData[byteToUse + lnJ] = 127;
                    //}
                    //else if (lnI < 21) // For the next 10 zones, it's a 67% chance.  Still no special fights.
                    //{
                    //    if (r1.Next() % 3 < 2)
                    //    {
                    //        zone = true;
                    //        romData[byteToUse + lnJ] = (byte)((r1.Next() % (lnI + 12)) + 1);
                    //    }
                    //    else
                    //        romData[byteToUse + lnJ] = 127;
                    //}
                    //else if (lnI == 42 || lnI == 43 || lnI == 44 || lnI == 54 || lnI == 55) // Sea cave.  No special bout here.
                    //{
                    //    if (r1.Next() % 5 < 4)
                    //    {
                    //        zone = true;
                    //        romData[byteToUse + lnJ] = (byte)((r1.Next() % 37) + 41);
                    //    }
                    //    else
                    //        romData[byteToUse + lnJ] = 127;
                    //}
                    //else if (lnI == 45 || lnI == 46 || lnI == 47 || lnI == 48 || lnI == 49 || lnI == 56) // Rhone cave.  Introduce Atlas chance.
                    //{
                    //    if (r1.Next() % 5 < 4)
                    //    {
                    //        zone = true;
                    //        romData[byteToUse + lnJ] = (byte)((r1.Next() % 28) + 51);
                    //    }
                    //    else
                    //        romData[byteToUse + lnJ] = 127;
                    //}
                    //else if (lnI == 50 || lnI == 51) // Rhone area.  Introduce Bazuzu chance.
                    //{
                    //    if (r1.Next() % 10 < 9)
                    //    {
                    //        zone = true;
                    //        romData[byteToUse + lnJ] = (byte)((r1.Next() % 18) + 62);
                    //    }
                    //    else
                    //        romData[byteToUse + lnJ] = 127;
                    //}
                    //else if (lnI == 52 || lnI == 53) // Hargon's Castle.  Introduce Zarlox chance.
                    //{
                    //    if (r1.Next() % 10 < 9)
                    //    {
                    //        zone = true;
                    //        romData[byteToUse + lnJ] = (byte)((r1.Next() % 13) + 68);
                    //    }
                    //    else
                    //        romData[byteToUse + lnJ] = 127;
                    //}
                    //else // Finally, a 80% chance.  Also introduce a 50% chance of the 19 "special bouts".
                    //{
                    //    if (r1.Next() % 5 < 4)
                    //    {
                    //        zone = true;
                    //        romData[byteToUse + lnJ] = (byte)((r1.Next() % 77) + 1);
                    //    }
                    //    else
                    //        romData[byteToUse + lnJ] = 127;
                    //}
                }
                //if (!zone)
                //    romData[byteToUse + 5] = (byte)((r1.Next() % (lnI < 11 ? lnI + 6 : lnI < 21 ? lnI + 10 : 78)) + 1);

                if (lnI == 0x17)
                    lnI = 0x17;

                byte specialBout = (byte)(r1.Next() % 20);
                if (((lnI >= 21 && lnI < 42) || lnI > 55))
                {
                    romData[byteToUse + 0] += 0;
                    romData[byteToUse + 1] += (byte)(specialBout >= 16 ? 128 : 0);
                    romData[byteToUse + 2] += (byte)(specialBout % 16 >= 8 ? 128 : 0);
                    romData[byteToUse + 3] += (byte)(specialBout % 8 >= 4 ? 128 : 0);
                    romData[byteToUse + 4] += (byte)(specialBout % 4 >= 2 ? 128 : 0);
                    romData[byteToUse + 5] += (byte)(specialBout % 2 >= 1 ? 128 : 0);
                }
                else
                {
                    for (int lnJ = 0; lnJ < 6; lnJ++)
                    {
                        romData[byteToUse + lnJ] += 128;
                    }
                }
            }

            // Randomize the 19 special battles (106b1-106fc)
            for (int lnI = 0; lnI < 19; lnI++)
            {
                int byteToUse = 0x106b1 + (4 * lnI);
                for (int lnJ = 0; lnJ < 4; lnJ++)
                {
                    if (r1.Next() % 2 == 1 || lnJ == 3)
                        romData[byteToUse + lnJ] = (byte)((r1.Next() % 77) + 1);
                }
            }

            // Randomize the first 12 boss fights, but make sure the last four of those involve Atlas, Bazuzu, Zarlox, and Hargon.
            // The 13th and final fight cannot be manipulated:  Malroth, and Malroth alone.
            for (int lnI = 0; lnI < 12; lnI++)
            {
                int byteToUse = 0x10356 + (lnI * 4);
                int boss1 = (lnI >= 8 ? 78 + (lnI - 8) : ((r1.Next() % 77) + 1));
                boss1 = (lnI == 0 ? (r1.Next() % 40) + 1 : boss1);
                int quantity1 = (boss1 >= 80 ? 1 : (r1.Next() % monsterSize[boss1]) + 1);
                int boss2 = (lnI == 0 ? (r1.Next() % 40) + 1 : (r1.Next() % 78) + 1);
                romData[byteToUse + 0] = (byte)boss1;
                romData[byteToUse + 1] = (byte)quantity1;
                romData[byteToUse + 2] = (byte)boss2;
                romData[byteToUse + 3] = 8; // It's too many monsters, but the width of the screen will trim the rest of the monsters off.
            }
        }

        private void randomizeEffects(Random r1)
        {
            // Randomize which items equate to which effects (except the Wizard's Ring, Medical Herb, and Antidote Herb) (13537-1353b)
            for (int lnI = 0; lnI < 5; lnI++)
            {
                // randomize from 1-35
                romData[0x13537 + lnI] = (byte)((r1.Next() % 35) + 1);
            }
        }

        private void randomizeEquipment(Random r1)
        {
            // Totally randomize weapons, armor, shields, helmets (13efb-13f1d, 1a00e-1a08b for pricing)

            byte[] maxPower = { 0, 0, 0, 0 };
            for (int lnI = 0; lnI < 35; lnI++)
            {
                byte power = 0;
                if (lnI == 0 || lnI == 16)
                    power = (byte)(r1.Next() % 10);
                else if (lnI < 16)
                    power = (byte)(Math.Pow(r1.Next() % 500, 2) / 2500); // max 100
                else if (lnI < 27)
                    power = (byte)(Math.Pow(r1.Next() % 500, 2) / 3570); // max 70
                else if (lnI < 31)
                    power = (byte)(Math.Pow(r1.Next() % 500, 2) / 6250); // max 40
                else
                    power = (byte)(Math.Pow(r1.Next() % 500, 2) / 8333); // max 30
                //power = (byte)(r1.Next() % (lnI < 16 ? (7 * (lnI + 1)) : lnI < 27 ? (7 * (lnI - 15)) : lnI < 31 ? (7 * (lnI - 26)) : (7 * (lnI - 30))));
                power += (byte)((lnI < 16 ? lnI : lnI < 27 ? lnI - 16 : lnI < 31 ? lnI - 27 : lnI - 31) + 1); // To avoid 0 power... and a non-selling item...
                maxPower[(lnI < 16 ? 0 : lnI < 27 ? 1 : lnI < 31 ? 2 : 3)] = (power > maxPower[(lnI < 16 ? 0 : lnI < 27 ? 1 : lnI < 31 ? 2 : 3)] ? power :
                    maxPower[(lnI < 16 ? 0 : lnI < 27 ? 1 : lnI < 31 ? 2 : 3)]);
                romData[0x13efb + lnI] = power;

                double price = Math.Round((lnI < 16 ? Math.Pow(power, 2.1) : lnI < 27 ? Math.Pow(power, 2.3) : lnI < 31 ? Math.Pow(power, 2.65) : Math.Pow(power, 2.85)), 0);
                // TO DO:  Round to the nearest 10 (after 100GP), 50(after 1000 GP), or 100 (after 2500 GP)
                
                if ((string)cboGPReq.SelectedItem == "100%")
                    price *= 2;
                else if ((string)cboGPReq.SelectedItem == "75%")
                    price *= 1.5;
                else if ((string)cboGPReq.SelectedItem == "33%")
                    price = price * 2 / 3;
                price = (float)Math.Round(price, 0);

                romData[0x1a00e + (lnI * 2) + 0] = (byte)(price % 256);
                romData[0x1a00e + (lnI * 2) + 1] = (byte)(Math.Floor(price / 256));
            }

            // Randomize starting equipment. (3c79f-3c7b6)  Target range:  6-24 attack, 4-16 defense.  If it can't be reached, assign lowest weapon and armor.
            // Remember to add 64 to the starting equipment!!!
            List<byte> legalWeapon = new List<byte>();
            List<byte> legalArmor = new List<byte>();
            for (int lnI = 0; lnI < 3; lnI++)
            {
                // Just give them the bamboo pole and the clothes for now.  We might randomize starting equipment later.
                int byteToUse = 0x3c79f + (8 * lnI);
                romData[byteToUse + 0] = 64 + 1;
                romData[byteToUse + 1] = 64 + 17;
            }
        }

        private void randomizeWhoEquip(Random r1)
        {
            // Totally randomize who can equip (1a3ce-1a3f0).  At least one person can equip something...
            for (int lnI = 0; lnI < 35; lnI++)
            {
                if (lnI == 0 || lnI == 16) romData[0x1a3ce + lnI] = 7; // everyone can equip the first weapon and armor.
                else romData[0x1a3ce + lnI] = (byte)((r1.Next() % 7) + 1);
            }
        }

        private void randomizeSpellStrengths(Random r1)
        {
            // Totally randomize spell strengths (18be0, 13be8, 13bf0, 127d5-1286a for strength, 134fa-13508 for cost, 13509-13517 for 3/4 cost)
            byte healScore = (byte)(r1.Next() % 255);
            romData[0x18be0] = romData[0x127fe] = healScore;
            byte healMoreScore = (byte)(r1.Next() % 255);
            romData[0x18be8] = romData[0x12808] = healMoreScore;
            byte herbScore = (byte)(r1.Next() % 255);
            romData[0x19602] = romData[0x1285d] = herbScore;
            byte shieldScore = (byte)(r1.Next() % 255);
            romData[0x12857] = shieldScore;

            int[] allySpells = { 0x127fd, 0x12807, 0x12811, 0x12857, 0x1285c };
            int[] allyCmd = { 0x13530, 0x13532, 0x13534, 0x13543, 0x13544 };
            for (int lnI = 0; lnI < allySpells.Length; lnI++)
            {
                int byteToUse = allySpells[lnI];
                int target = (r1.Next() % 2);
                romData[byteToUse + 0] = (byte)(target);
                int cmdTarget = (target == 0 ? 2 : 0);
                romData[allyCmd[lnI]] = (byte)cmdTarget;
            }

            int[] monsterSpells = { 0x127d5, 0x127e9, 0x127df, 0x12816, 0x127e4, 0x12848, 0x1284d };
            int[] monsterCmd = { 0x13528, 0x1352c, 0x1352a, 0x13535, 0x13511, 0x13513, 0x13540, 0x13541 };
            for (int lnI = 0; lnI < monsterSpells.Length; lnI++)
            {
                int byteToUse = monsterSpells[lnI];
                int target = ((r1.Next() % 3) + 2);
                romData[byteToUse + 0] = (byte)(target);
                romData[byteToUse + 1] = (byte)(r1.Next() % 160);
                int cmdTarget = (target <= 3 ? 1 : 0);
                romData[monsterCmd[lnI]] = (byte)cmdTarget;
            }

            int[] constMonsSpells = { 0x127e4, 0x1280c, 0x127da, 0x127f3, 0x127ee };
            int[] constMonsCmd = { 0x1352b, 0x13533, 0x13529, 0x1352e, 0x1352d };
            for (int lnI = 0; lnI < constMonsSpells.Length; lnI++)
            {
                int byteToUse = monsterSpells[lnI];
                int target = ((r1.Next() % 3) + 2);
                romData[byteToUse + 0] = (byte)(target);
                int cmdTarget = (target <= 3 ? 1 : 0);
                romData[monsterCmd[lnI]] = (byte)cmdTarget;
            }

            // Don't randomize the defense spell for now.
            //int[] defSpells = { 0x127f8, 0x12802 };
            //int[] defCmd = { 0x1352f, 0x13531 };
            //for (int lnI = 0; lnI < defSpells.Length; lnI++)
            //{
            //    int byteToUse = defSpells[lnI];
            //    int target = (lnI == 0 ? ((r1.Next() % 3) + 2) : (r1.Next() % 2));
            //    romData[byteToUse + 0] = (byte)(target);

            //    int def = r1.Next() % 65;
            //    romData[byteToUse + 1] = (byte)(def); // (def == 1 ? 0 : def == 2 ? 1 : def == 3 ? 2 : def == 4 ? 16 : def == 5 ? 32 : 64);

            //    int cmdTarget = (target == 1 || target == 4 ? 0 : target == 2 || target == 3 ? 1 : 2);
            //    romData[defCmd[lnI]] = (byte)cmdTarget;
            //}
        }

        private void randomizeSpellLearning(Random r1)
        {
            // Totally randomize spell learning (13edb-13eea, 13eeb-13efa, 1ae76-1ae95, 1b63c-1b727(text), separate the two casters with "ff ff")
            // Text - 0 to 9 (00-09), a-z (0a-23), A-Z(24-3d)
            byte level = 1;
            for (int lnI = 0; lnI < 32; lnI++)
            {
                if (lnI == 15 || lnI == 31) continue; // We can't figure out how to get an eighth command spell in there yet.
                if (lnI == 4 || lnI == 8 || lnI == 20 || lnI == 24) continue; // Heal/Healmore MUST be learned at level 1, so leave that byte alone.

                if (lnI < 16)
                    level = (byte)((r1.Next() % 25) + 2);
                else
                    level = (byte)((r1.Next() % 20) + 2);

                romData[0x13edb + lnI] = level;
            }

            for (int lnI = 0; lnI < 4; lnI++)
                for (int lnJ = lnI + 1; lnJ < 4; lnJ++)
                {
                    if (romData[0x13edb + lnJ] < romData[0x13edb + lnI])
                    {
                        swap(0x13edb + lnJ, 0x13edb + lnI);
                        lnJ = lnI;
                    }
                }

            for (int lnI = 4; lnI < 8; lnI++)
                for (int lnJ = lnI + 1; lnJ < 8; lnJ++)
                {
                    if (romData[0x13edb + lnJ] < romData[0x13edb + lnI])
                    {
                        swap(0x13edb + lnJ, 0x13edb + lnI);
                        lnJ = lnI;
                    }
                }

            for (int lnI = 8; lnI < 15; lnI++)
                for (int lnJ = lnI + 1; lnJ < 15; lnJ++)
                {
                    if (romData[0x13edb + lnJ] < romData[0x13edb + lnI])
                    {
                        swap(0x13edb + lnJ, 0x13edb + lnI);
                        lnJ = lnI;
                    }
                }

            for (int lnI = 16; lnI < 20; lnI++)
                for (int lnJ = lnI + 1; lnJ < 20; lnJ++)
                {
                    if (romData[0x13edb + lnJ] < romData[0x13edb + lnI])
                    {
                        swap(0x13edb + lnJ, 0x13edb + lnI);
                        lnJ = lnI;
                    }
                }

            for (int lnI = 20; lnI < 24; lnI++)
                for (int lnJ = lnI + 1; lnJ < 24; lnJ++)
                {
                    if (romData[0x13edb + lnJ] < romData[0x13edb + lnI])
                    {
                        swap(0x13edb + lnJ, 0x13edb + lnI);
                        lnJ = lnI;
                    }
                }

            for (int lnI = 24; lnI < 31; lnI++)
                for (int lnJ = lnI + 1; lnJ < 31; lnJ++)
                {
                    if (romData[0x13edb + lnJ] < romData[0x13edb + lnI])
                    {
                        swap(0x13edb + lnJ, 0x13edb + lnI);
                        lnJ = lnI;
                    }
                }

            // Shuffle prince fight spells
            int[] spellsLearned = { 1, 9, 6, 11, 3, 10, 4, 12 };
            for (int lnI = 0; lnI < 100; lnI++)
            {
                int numberToSwap1 = (r1.Next() % 8);
                int numberToSwap2 = (r1.Next() % 8);
                int swappy = spellsLearned[numberToSwap1];
                spellsLearned[numberToSwap1] = spellsLearned[numberToSwap2];
                spellsLearned[numberToSwap2] = swappy;
            }

            for (int lnI = 0; lnI < 8; lnI++)
                romData[0x1ae86 + lnI] = (byte)(spellsLearned[lnI]);

            // Shuffle prince command spells
            spellsLearned = new int[] { 9, 16, 20, 18, 11, 22, 23 };
            for (int lnI = 0; lnI < 100; lnI++)
            {
                int numberToSwap1 = (r1.Next() % 7);
                int numberToSwap2 = (r1.Next() % 7);
                int swappy = spellsLearned[numberToSwap1];
                spellsLearned[numberToSwap1] = spellsLearned[numberToSwap2];
                spellsLearned[numberToSwap2] = swappy;
            }

            for (int lnI = 0; lnI < 7; lnI++)
                romData[0x1ae76 + lnI] = (byte)(spellsLearned[lnI]);

            // Shuffle princess fight spells
            spellsLearned = new int[] { 2, 11, 5, 8, 7, 13, 14, 15 };
            for (int lnI = 0; lnI < 100; lnI++)
            {
                int numberToSwap1 = (r1.Next() % 8);
                int numberToSwap2 = (r1.Next() % 8);
                int swappy = spellsLearned[numberToSwap1];
                spellsLearned[numberToSwap1] = spellsLearned[numberToSwap2];
                spellsLearned[numberToSwap2] = swappy;
            }

            for (int lnI = 0; lnI < 8; lnI++)
                romData[0x1ae8e + lnI] = (byte)(spellsLearned[lnI]);

            // Shuffle princess command spells
            spellsLearned = new int[] { 11, 19, 16, 13, 18, 22, 21 };
            for (int lnI = 0; lnI < 100; lnI++)
            {
                int numberToSwap1 = (r1.Next() % 7);
                int numberToSwap2 = (r1.Next() % 7);
                int swappy = spellsLearned[numberToSwap1];
                spellsLearned[numberToSwap1] = spellsLearned[numberToSwap2];
                spellsLearned[numberToSwap2] = swappy;
            }

            for (int lnI = 0; lnI < 7; lnI++)
                romData[0x1ae7e + lnI] = (byte)(spellsLearned[lnI]);

            // Now time to enter the text of each spell for each character.
            int textByte = 0x1b634;
            for (int lnI = 0; lnI < 32; lnI++)
            {
                if (lnI < 8)
                    textByte += fillInSpells(textByte, romData[0x1ae86 + (lnI % 8)]);
                else if (lnI < 16)
                    textByte += fillInSpells(textByte, romData[0x1ae76 + (lnI % 8)]);
                else if (lnI < 24)
                    textByte += fillInSpells(textByte, romData[0x1ae8e + (lnI % 8)]);
                else
                    textByte += fillInSpells(textByte, romData[0x1ae7e + (lnI % 8)]);
            }
            // textByte should always finish at 0x1b727.

            // Finally, assign text slot for each spell.  You want to look through the order of each spell to accomplish this.
            for (int lnI = 1; lnI <= 23; lnI++)
            {
                int byteToUse = 0x1ae5f + lnI - 1;
                if (byteToUse == 0x1ae6f) continue; // This spell is not used.
                for (int lnJ = 0; lnJ < 32; lnJ++)
                {
                    int spellToCompare = 0xff;
                    if (lnJ < 8)
                        spellToCompare = romData[0x1ae86 + (lnJ % 8)];
                    else if (lnJ < 16)
                        spellToCompare = romData[0x1ae76 + (lnJ % 8)];
                    else if (lnJ < 24)
                        spellToCompare = romData[0x1ae8e + (lnJ % 8)];
                    else
                        spellToCompare = romData[0x1ae7e + (lnJ % 8)];

                    if (spellToCompare == lnI)
                    {
                        romData[byteToUse] = (byte)(lnJ);
                        break;
                    }
                }
            }
        }

        private int fillInSpells(int byteToUse, int spellToUse)
        {
            int[] byteArray;
            if (spellToUse == 0x01)
                byteArray = new int[] { 0x29, 0x12, 0x1b, 0x0e, 0x0b, 0x0a, 0x15, 0xff };
            else if (spellToUse == 0x02)
                byteArray = new int[] { 0x36, 0x15, 0x0e, 0x0e, 0x19, 0xff };
            else if (spellToUse == 0x03)
                byteArray = new int[] { 0x29, 0x12, 0x1b, 0x0e, 0x0b, 0x0a, 0x17, 0x0e, 0xff };
            else if (spellToUse == 0x04)
                byteArray = new int[] { 0x27, 0x0e, 0x0f, 0x0e, 0x0a, 0x1d, 0xff };
            else if (spellToUse == 0x05)
                byteArray = new int[] { 0x2c, 0x17, 0x0f, 0x0e, 0x1b, 0x17, 0x18, 0x1c, 0xff };
            else if (spellToUse == 0x06)
                byteArray = new int[] { 0x36, 0x1d, 0x18, 0x19, 0x1c, 0x19, 0x0e, 0x15, 0x15, 0xff };
            else if (spellToUse == 0x07)
                byteArray = new int[] { 0x36, 0x1e, 0x1b, 0x1b, 0x18, 0x1e, 0x17, 0x0d, 0xff };
            else if (spellToUse == 0x08)
                byteArray = new int[] { 0x27, 0x0e, 0x0f, 0x0e, 0x17, 0x0c, 0x0e, 0xff };
            else if (spellToUse == 0x09)
                byteArray = new int[] { 0x2b, 0x0e, 0x0a, 0x15, 0xff };
            else if (spellToUse == 0x0a)
                byteArray = new int[] { 0x2c, 0x17, 0x0c, 0x1b, 0x0e, 0x0a, 0x1c, 0x0e, 0xff };
            else if (spellToUse == 0x0b)
                byteArray = new int[] { 0x2b, 0x0e, 0x0a, 0x15, 0x16, 0x18, 0x1b, 0x0e, 0xff };
            else if (spellToUse == 0x0c)
                byteArray = new int[] { 0x36, 0x0a, 0x0c, 0x1b, 0x12, 0x0f, 0x12, 0x0c, 0x0e, 0xff };
            else if (spellToUse == 0x0d)
                byteArray = new int[] { 0x2b, 0x0e, 0x0a, 0x15, 0x0a, 0x15, 0x15, 0xff };
            else if (spellToUse == 0x0e)
                byteArray = new int[] { 0x28, 0x21, 0x19, 0x15, 0x18, 0x0d, 0x0e, 0x1d, 0xff };
            else if (spellToUse == 0x0f)
                byteArray = new int[] { 0x26, 0x11, 0x0a, 0x17, 0x0c, 0x0e, 0xff };
            else if (spellToUse == 0x10)
                byteArray = new int[] { 0x24, 0x17, 0x1d, 0x12, 0x0d, 0x18, 0x1d, 0x0e, 0xff };
            else if (spellToUse == 0x12)
                byteArray = new int[] { 0x32, 0x1e, 0x1d, 0x1c, 0x12, 0x0d, 0x0e, 0xff };
            else if (spellToUse == 0x13)
                byteArray = new int[] { 0x35, 0x0e, 0x19, 0x0e, 0x15, 0xff };
            else if (spellToUse == 0x14)
                byteArray = new int[] { 0x35, 0x0e, 0x1d, 0x1e, 0x1b, 0x17, 0xff };
            else if (spellToUse == 0x15)
                byteArray = new int[] { 0x32, 0x19, 0x0e, 0x17, 0xff };
            else if (spellToUse == 0x16)
                byteArray = new int[] { 0x36, 0x1d, 0x0e, 0x19, 0x10, 0x1e, 0x0a, 0x1b, 0x0d, 0xff };
            else if (spellToUse == 0x17)
                byteArray = new int[] { 0x35, 0x0e, 0x1f, 0x12, 0x1f, 0x0e, 0xff };
            else
                byteArray = new int[] { 0xff };
            for (int lnI = 0; lnI < byteArray.Length; lnI++)
                romData[byteToUse + lnI] = (byte)byteArray[lnI];
            return byteArray.Length;
        }

        private void randomizeTreasures(Random r1)
        {
            // Totally randomize treasures... but make sure key items exist before they are needed! (19e41-19f15, 19f1a-19f2a, 19c79, 19c84)
            int[] treasureAddrZ0 = { 0x19e41, 0x19c79 }; // 2
            int[] treasureAddrZ1 = { 0x19ed9, 0x19edd, 0x19ee1, 0x19e79, 0x19e7d,
                                     0x19e81, 0x19e85, 0x19e89, 0x19e8d, 0x19e91,
                                     0x19f0d, 0x19f11, 0x19f15, 0x19f1a }; // Cloak of wind/Mirror Of Ra and previous; 14
            int[] treasureAddrZ2 = { 0x19f32, 0x19eb5, 0x19ef9, 0x19f01, 0x19f05,
                                     0x19f09, 0x19f1e, 0x19f22, 0x19f2a }; // Pre-Golden key; 9
            int[] treasureAddrZ3 = { 0x19e45, 0x19e49, 0x19e4d, 0x19e51, 0x19e55,
                                     0x19e59, 0x19e5d, 0x19e61, 0x19e65, 0x19e69,
                                     0x19e6d, 0x19e71, 0x19e75, 0x19ef9, 0x19f01,
                                     0x19f05, 0x19f09 }; // Golden key to moon tower; Jailor's required by here; 17
            int[] treasureAddrZ4 = { 0x19f26, 0x19ee5, 0x19ee9, 0x19eed, 0x19ef1, 0x19ef5 }; // Hamlin and Moon Tower; 6
            int[] treasureAddrZ5 = { 0x19e95, 0x19e99, 0x19e9d, 0x19ea1, 0x19ea5,
                                     0x19ea9, 0x19ead, 0x19eb1 }; // Sea Cave; 8
            int[] treasureAddrZ6 = { 0x19eb9, 0x19ebd, 0x19ec1, 0x19ec5, 0x19ec9,
                                     0x19ecd, 0x19ed1, 0x19ed5 }; // Rhone Cave; 8
            List<int> allTreasureList = new List<int>();

            allTreasureList = addTreasure(allTreasureList, treasureAddrZ0);
            allTreasureList = addTreasure(allTreasureList, treasureAddrZ1);
            allTreasureList = addTreasure(allTreasureList, treasureAddrZ2);
            allTreasureList = addTreasure(allTreasureList, treasureAddrZ3);
            allTreasureList = addTreasure(allTreasureList, treasureAddrZ4);
            allTreasureList = addTreasure(allTreasureList, treasureAddrZ5);
            allTreasureList = addTreasure(allTreasureList, treasureAddrZ6);

            int[] allTreasure = allTreasureList.ToArray();

            // randomize starting gold
            romData[0x19c84] = (byte)(r1.Next() % 256);
            // Replace Dew's Yarn location with gold so people don't go there hoping they can also get the Magic Loom.
            romData[0x19b5c] = 0x49;

            List<byte> treasureList = new List<byte>();
            byte[] legalTreasures = { 0x01, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
                                      0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f,
                                      0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x29, 0x2a, 0x2f,
                                      0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x38, 0x3b, 0x3c, 0x3d,
                                      0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f };
            for (int lnI = 0; lnI < allTreasure.Length; lnI++)
            {
                if (lnI == 1)
                {
                    romData[allTreasure[lnI]] = romData[allTreasure[0]]; // This is so the player can also get 50 gold. (most of the time)
                    continue;
                }
                bool legal = false;
                while (!legal)
                {
                    byte treasure = (byte)((r1.Next() % legalTreasures.Length)); // the last two items we can't get...
                    treasure = legalTreasures[treasure];
                    if (!(treasureList.Contains(treasure) && ((treasure >= 0x24 && treasure <= 0x2e) || treasure == 0x32 || treasure == 0x37 || treasure == 0x38 || treasure == 0x39
                        || treasure == 0x40 || treasure == 0x42 || treasure == 0x44)))
                    {
                        legal = true;
                        treasureList.Add(treasure);
                        romData[allTreasure[lnI]] = treasure;
                    }
                }
            }

            // Verify that key items are available in either a store or a treasure chest in the right zone.
            // Mirror Of Ra, Cloak Of Wind, Golden Key, Jailor's Key, Moon Fragment, Eye Of Malroth, Three crests
            byte[] keyItems = { 0x2b, 0x2e, 0x37, 0x39, 0x26, 0x28, 0x40, 0x43, 0x44 };
            byte[] keyTreasure = { 16, 16, 25, 42, 48, 56, 64, 64, 64 };
            byte[] keyWStore = { 12, 12, 36, 48, 48, 48, 48, 48, 48 };
            byte[] keyIStore = { 24, 24, 54, 66, 66, 66, 66, 66, 66 };
            for (int lnI = 0; lnI < keyItems.Length; lnI++)
            {
                bool legal = false;
                for (int lnJ = 0; lnJ < keyTreasure[lnI]; lnJ++)
                {
                    if (romData[allTreasure[lnJ]] == keyItems[lnI])
                        legal = true;
                }
                for (int lnJ = 0; lnJ < keyWStore[lnI]; lnJ++)
                {
                    if (romData[0x19f9a + lnJ] == keyItems[lnI])
                        legal = true;
                }
                for (int lnJ = 0; lnJ < keyIStore[lnI]; lnJ++)
                {
                    if (romData[0x19f9a + 48 + lnJ] == keyItems[lnI])
                        legal = true;
                }

                // If legal = false, then the item was not found, so we'll have to place it in a treasure somewhere...
                while (!legal)
                {
                    byte tRand = (byte)(r1.Next() % keyTreasure[lnI]);
                    if (tRand != 0 && tRand != 1)
                    {
                        bool dupCheck = false;
                        for (int lnJ = 0; lnJ < keyItems.Length; lnJ++)
                        {
                            if (romData[allTreasure[tRand]] == keyItems[lnJ])
                                dupCheck = true;
                        }
                        if (dupCheck == false)
                        {
                            romData[allTreasure[tRand]] = keyItems[lnI];
                            legal = true;
                        }
                    }
                }
            }
        }

        private void randomizeStores(Random r1)
        {
            // Adjust prices 
            // Totally randomize stores (cannot have Jailor's Key in a weapons store) (19f9a-1a00b)
            for (int lnI = 0; lnI < 19; lnI++)
            {
                byte[] legalWeapons = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35 };
                byte[] legalItems = { 41, 42, 47, 48, 49, 50, 51, 52, 53, 56, 57, 59, 60, 61 };
                int byteToUse = 0x19f9a + (lnI * 6);

                for (int lnJ = 0; lnJ < 6; lnJ++)
                {
                    byte treasure;
                    if (lnI < 8)
                        treasure = legalWeapons[r1.Next() % legalWeapons.Length];
                    else
                        treasure = legalItems[r1.Next() % legalItems.Length];
                    romData[byteToUse + lnJ] = treasure;
                }

                // Always have one item in store.  Let chances of having another item = 94%/88%/81%/75%/69%/63% for weapons and 90%/80%/70%/60%/50% for items
                //byte chance = (byte)(lnI < 8 ? 15 : 9);
                //bool fail = false;
                //for (int lnJ = 0; lnJ < 6; lnJ++)
                //{
                //    if (!fail && r1.Next() % (chance + 1) <= chance - lnJ)
                //    {
                //        // Add item
                //        byte treasure;
                //        if (lnI < 8)
                //            treasure = legalWeapons[r1.Next() % legalWeapons.Length];
                //        else
                //            treasure = legalItems[r1.Next() % legalItems.Length];
                //        romData[byteToUse + lnJ] = treasure;
                //    }
                //    else
                //    {
                //        romData[byteToUse + lnJ] = 0;
                //        fail = true;
                //    }
                //}

                // Go through to find duplicates.  Any duplicates found-> 00.  114 items total.
                List<int> items = new List<int>();
                for (int lnJ = 0; lnJ < 6; lnJ++)
                {
                    if (!items.Contains(romData[byteToUse + lnJ]) && romData[byteToUse + lnJ] != 0)
                        items.Add(romData[byteToUse + lnJ]);
                }

                int[] itemArray = items.ToArray();
                for (int lnJ = 0; lnJ < 6; lnJ++)
                {
                    if (lnJ < itemArray.Length)
                        romData[byteToUse + lnJ] = (byte)itemArray[lnJ];
                    else
                        romData[byteToUse + lnJ] = 0;
                }
            }

            // House of healing cost randomized
            romData[0x18659] = (byte)(r1.Next() % 20);

            // Inn prices randomized
            // byte[] inns = { 4, 6, 8, 12, 20, 2, 25, 30, 40, 30 };
            for (int lnI = 0; lnI < 9; lnI++)
                romData[0x19f90 + lnI] = (byte)((r1.Next() % 20) + 1);
        }

        private void randomizeStats(Random r1)
        {

            int randomModifier = 0;

            // Randomize starting stats.  Do not exceed 16 strength and agility, and 40 HP/MP. (13dd1-13ddc)
            // Strength, agility, hp, mp
            byte[] stats = { romData[0x13dd1 + 0], romData[0x13dd1 + 1], romData[0x13dd1 + 2], romData[0x13dd1 + 3],
                romData[0x13dd1 + 4], romData[0x13dd1 + 5], romData[0x13dd1 + 6], romData[0x13dd1 + 7],
                romData[0x13dd1 + 8], romData[0x13dd1 + 9], romData[0x13dd1 + 10], romData[0x13dd1 + 11] };

            for (int lnI = 0; lnI < 12; lnI++)
            {
                if (lnI == 3) // Midenhall starts with 0 MP.
                    continue;
                switch (lnI % 4)
                {
                    case 0:
                    case 1:
                    case 3:
                        randomModifier = (r1.Next() % 16);
                        break;
                    case 2:
                        randomModifier = 24 + (r1.Next() % 16);
                        break;
                }

                if (romData[0x13dd1 + lnI] + randomModifier >= 0)
                {
                    romData[0x13dd1 + lnI] = (byte)(randomModifier);
                    stats[lnI] = romData[0x13dd1 + lnI];
                }
            }

            int maxStrength = 255 - maxPower[0];
            int maxAgility = 510 - ((maxPower[1] + maxPower[2] + maxPower[3]) * 2);
            maxAgility = (maxAgility > 255 ? 255 : maxAgility);

            if (randomLevel == 4)
            {
                for (int lnI = 0; lnI < 12; lnI++)
                {
                    int maxGains = 0;
                    if (lnI % 4 == 0)
                    {
                        // establish maxStrength based on maxPower FOR THAT PERSON.  
                        int maxStrInd = 0;
                        for (int lnJ = 0; lnJ < 16; lnJ++)
                        {
                            if (romData[0x13efb + lnJ] > maxStrInd)
                                if ((lnI == 0 && romData[0x1a3ce + lnJ] % 2 == 1) || (lnI == 4 && (romData[0x1a3ce + lnJ] % 4) >= 2) || (lnI == 8 && romData[0x1a3ce + lnJ] >= 4))
                                    maxStrInd = romData[0x13efb + lnJ];
                        }
                        maxStrInd = 255 - maxStrInd;
                        maxGains = maxStrInd - romData[0x13dd1 + lnI];
                    }
                    else if (lnI % 4 == 1)
                    {
                        int maxStrInd = 0;
                        int maxEquip = 0;
                        for (int lnJ = 16; lnJ < 27; lnJ++)
                        {
                            if (romData[0x13efb + lnJ] > maxEquip)
                                if ((lnI == 1 && romData[0x1a3ce + lnJ] % 2 == 1) || (lnI == 5 && (romData[0x1a3ce + lnJ] % 4) >= 2) || (lnI == 9 && romData[0x1a3ce + lnJ] >= 4))
                                    maxEquip = romData[0x13efb + lnJ];
                        }
                        maxStrInd += maxEquip;
                        maxEquip = 0;
                        for (int lnJ = 27; lnJ < 31; lnJ++)
                        {
                            if (romData[0x13efb + lnJ] > maxEquip)
                                if ((lnI == 1 && romData[0x1a3ce + lnJ] % 2 == 1) || (lnI == 5 && (romData[0x1a3ce + lnJ] % 4) >= 2) || (lnI == 9 && romData[0x1a3ce + lnJ] >= 4))
                                    maxEquip = romData[0x13efb + lnJ];
                        }
                        maxStrInd += maxEquip;
                        maxEquip = 0;
                        for (int lnJ = 31; lnJ < 35; lnJ++)
                        {
                            if (romData[0x13efb + lnJ] > maxEquip)
                                if ((lnI == 1 && romData[0x1a3ce + lnJ] % 2 == 1) || (lnI == 5 && (romData[0x1a3ce + lnJ] % 4) >= 2) || (lnI == 9 && romData[0x1a3ce + lnJ] >= 4))
                                    maxEquip = romData[0x13efb + lnJ];
                        }
                        maxStrInd += maxEquip;
                        maxStrInd = 510 - (2 * maxStrInd);
                        maxStrInd = (maxStrInd > 255 ? 255 : maxStrInd);

                        maxGains = maxStrInd - romData[0x13dd1 + lnI];
                    }
                    else maxGains = 255 - romData[0x13dd1 + lnI];

                    if (lnI % 4 == 0) maxGains = (r1.Next() % (maxGains - 70)) + 70;
                    if (lnI % 4 == 1) maxGains = (r1.Next() % (maxGains - 120)) + 120;
                    if (lnI % 4 == 2) maxGains = (r1.Next() % (maxGains - 140)) + 140;
                    if (lnI % 4 == 3) maxGains = (r1.Next() % (maxGains - 140)) + 140;
                    //if (lnI == 3) maxGains = 0; // No MP for Midenhall

                    int arraySize = lnI < 4 ? 50 : lnI < 8 ? 45 : 35;
                    int[] values = new int[arraySize];
                    for (int lnJ = 0; lnJ < arraySize; lnJ++)
                        values[lnJ] = romData[0x13dd1 + lnI] + (r1.Next() % maxGains);

                    Array.Sort(values);
                    for (int lnJ = 0; lnJ < arraySize - 1; lnJ++)
                        if (values[lnJ + 1] > values[lnJ] + 15)
                            values[lnJ + 1] = values[lnJ] + 15;

                    // Starting stat should be values[0].  This is what I'm doing wrong...
                    romData[0x13dd1 + lnI] = (byte)values[0];

                    int adder = (lnI / 2);
                    for (int lnJ = 0; lnJ < arraySize - 1; lnJ++)
                    {
                        int byteToUse = 0x13ddd + adder;
                        int valueToAdd = values[lnJ + 1] - values[lnJ];
                        if (lnI == 3) valueToAdd = 0;
                        if (lnI % 2 == 0)
                            romData[byteToUse] = (byte)(valueToAdd * 16); // You can reset the 2nd nibble since it's about to be changed anyway.  (byte)((romData[byteToUse] % 16) + (valueToAdd * 16));
                        else
                            romData[byteToUse] = (byte)((valueToAdd % 16) + romData[byteToUse]);
                        adder += (lnJ >= 44 ? 2 : lnJ >= 34 ? 4 : 6);
                    }
                }
            } else
            {
                for (int lnI = 0; lnI < 254; lnI++)
                {
                    int byteToUse = 0x13ddd + lnI;

                    int statToUse1 = (lnI > 244 ? lnI % 2 : (lnI > 206 ? lnI % 4 : lnI % 6)) * 2;
                    int statToUse2 = (lnI > 244 ? lnI % 2 : (lnI > 206 ? lnI % 4 : lnI % 6)) * 2 + 1;

                    int randomModifier1 = (romData[byteToUse] / 16);
                    int randomModifier2 = (romData[byteToUse] % 16);

                    randomModifier1 += (r1.Next() % (randomLevel == 1 ? 3 : randomLevel == 2 ? 5 : 7)) - randomLevel;
                    randomModifier2 += (r1.Next() % (randomLevel == 1 ? 3 : randomLevel == 2 ? 5 : 7)) - randomLevel;
                    //if (randomLevel == 1)
                    //{
                    //    randomModifier2 += (r1.Next() % 3) - 1;
                    //} else if (randomLevel == 2)
                    //{
                    //    randomModifier1 += (r1.Next() % 5) - 2;
                    //    randomModifier2 += (r1.Next() % 5) - 2;
                    //}
                    //else if (randomLevel == 3)
                    //{
                    //    randomModifier1 += (r1.Next() % 7) - 3;
                    //    randomModifier2 += (r1.Next() % 7) - 3;
                    //}
                    if (lnI % 2 == 0)
                    {
                        if (stats[statToUse1] + randomModifier1 > maxStrength || randomModifier1 < 0)
                            randomModifier1 = 0;
                        if (stats[statToUse2] + randomModifier2 > maxAgility || randomModifier2 < 0)
                            randomModifier2 = 0;
                    } else
                    {
                        if (stats[statToUse1] + randomModifier1 > 250 || randomModifier1 < 0)
                            randomModifier1 = 0;
                        if (stats[statToUse2] + randomModifier2 > 250 || statToUse2 == 3 || randomModifier2 < 0)
                            randomModifier2 = 0;
                    }

                    if (statToUse1 == 3) randomModifier2 = 0;
                    romData[byteToUse] = (byte)((randomModifier1 * 16) + randomModifier2);
                    stats[statToUse1] += (byte)randomModifier1;
                    stats[statToUse2] += (byte)randomModifier2;
                }
            }

            //int avgStrength = ((maxStrength / 50));
            //int[] avg7 = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12, 12, 13, 13, 14, 15 };
            //int[] avg5 = { 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 4, 4, 5, 5, 6, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            //int[] avg4 = { 0, 0, 0, 0, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
            //int[] avg3 = { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 4, 4, 4, 5, 6, 7, 8, 9, 10 };
            //// Randomize stat gains... but don't put any stat above 255! (13ddd-13eda)
            //// 98 bytes for Midenhall, 88 bytes for Cannock, 68 bytes for Moonbrooke.  First break at 210, second break at 250.
            //for (int lnI = 0; lnI < 254; lnI++)
            //{
            //    int byteToUse = 0x13ddd + lnI;

            //    int statToUse1 = (lnI > 244 ? lnI % 2 : (lnI > 206 ? lnI % 4 : lnI % 6)) * 2;
            //    int statToUse2 = (lnI > 244 ? lnI % 2 : (lnI > 206 ? lnI % 4 : lnI % 6)) * 2 + 1;

            //    int randomModifier1 = 0;
            //    int randomModifier2 = 0;

            //    if (lnI % 2 == 0)
            //    {
            //        int r1Result = (r1.Next() % 30);
            //        r1Result = (r1Result < 0 ? 0 : r1Result);
            //        randomModifier1 = (avgStrength == 5 ? avg5[r1Result] : avgStrength == 4 ? avg4[r1Result] : avg3[r1Result]);
            //        if (stats[statToUse1] + randomModifier1 > maxStrength)
            //            randomModifier1 = 0;

            //        int r2Result = (r1.Next() % 30);
            //        r2Result = (r2Result < 0 ? 0 : r2Result);
            //        randomModifier2 = (statToUse2 == 1 ? avg4[r2Result] : statToUse2 == 5 ? avg5[r2Result] : avg7[r2Result]);
            //        if (stats[statToUse2] + randomModifier2 > maxAgility)
            //            randomModifier2 = 0;
            //    }
            //    else
            //    {
            //        int r1Result = (r1.Next() % 30);
            //        r1Result = (r1Result < 0 ? 0 : r1Result);
            //        randomModifier1 = avg5[r1Result];
            //        if (stats[statToUse1] + randomModifier1 > 255)
            //            randomModifier1 = 0;

            //        int r2Result = (r1.Next() % 30);
            //        r2Result = (r2Result < 0 ? 0 : r2Result);
            //        randomModifier2 = (statToUse2 == 3 ? 0 : statToUse2 == 7 ? avg5[r2Result] : avg7[r2Result]);
            //        if (stats[statToUse2] + randomModifier2 > 255)
            //            randomModifier2 = 0;
            //    }

            //    romData[byteToUse] = (byte)((randomModifier1 * 16) + randomModifier2);
            //    stats[statToUse1] += (byte)randomModifier1;
            //    stats[statToUse2] += (byte)randomModifier2;
            //}
        }

        private void speedUpBattles()
        {
            // ALL ROM Hacks will have greatly increased battle speeds.
            romData [0x1adcf] = 0x01;
            romData [0x1add0] = 0x28;
            romData [0x1add1] = 0x40;
            // All ROM hacks will reduce shaking from taking damage, speeding up battles even further.
            romData[0x11038] = 2; // instead of 11
            romData[0x10ae9] = 1; // instead of 4, greatly reducing enemy flashing on them taking damage, reducing about 12 frames each time.
            romData[0x3c526] = 1; // instead of 10, greatly reducing flashes done for spell casting, removing 20 frames every time a spell is cast.
        }

        private void skipPrologue()
        {
            // ALL ROM hacks will skip the prologue.
            for (int lnI = 0; lnI < 12; lnI++)
                romData[0x1c1a5 + lnI] = 0xea;
        }

        private void reviveAllCharsOnCOD()
        {
            // All ROM hacks will revive ALL characters on a ColdAsACod.
            byte[] codData1 = { 0x20, 0x97, 0xff }; // replace with a jsr to a bunch of unused code at 0x3ffa7 (near the end of the ROM)
            byte[] codData2 = { 0x8d, 0x3b, 0x06, // save to the hero's address (this is the code we're replacing in codData1)
                                0xad, 0x51, 0x06, // load moonbrooke's status
                                0xc9, 0x04, // Check for moonbrooke existance
                                0xd0, 0x03, // If not, skip three bytes
                                0x4c, 0xca, 0xd2, // Else, change to the Rhone routine, reviving all characters with full health... although a bit awkwardly.
                                0xad, 0x3f, 0x06, // Load cannock's status
                                0xc9, 0x04, // Make sure he exists
                                0xd0, 0x0a, // If not, skip ten bytes.
                                0xa9, 0x84, // Revive the prince...
                                0x8d, 0x3f, 0x06, // Here, officially
                                0xa9, 0x01, // then give the prince 1 HP (which will later, by ENIX, change to MAX HP)
                                0x8d, 0x4d, 0x06, // and then make it official.
                                0x60 }; // jsr, i.e. end sub

            for (int lnI = 0; lnI < codData1.Length; lnI++)
                romData[0x3d293 + lnI] = codData1[lnI];
            for (int lnI = 0; lnI < codData2.Length; lnI++)
                romData[0x3ffa7 + lnI] = codData2[lnI];
        }

        private void textGet()
        {
            List<string> txtStrings = new List<string>();
            string tempWord = "";
            for (int lnI = 0; lnI < 1913; lnI++)
            {
                int starter = 0x1b2da;
                if (romData[starter + lnI] == 255)
                {
                    txtStrings.Add(tempWord);
                    tempWord = "";
                }
                else if (romData[starter + lnI] >= 0 && romData[starter + lnI] <= 9)
                {
                    tempWord += (char)(romData[starter + lnI] + 39);
                }
                else if (romData[starter + lnI] >= 10 && romData[starter + lnI] <= 35)
                {
                    tempWord += (char)(romData[starter + lnI] + 87);
                }
                else if (romData[starter + lnI] >= 36 && romData[starter + lnI] <= 61)
                {
                    tempWord += (char)(romData[starter + lnI] + 29);
                }
            }
            using (StreamWriter writer = File.CreateText(Path.Combine(Path.GetDirectoryName(txtFileName.Text), "DW2Strings.txt")))
            {
                int lnJ = 1;
                foreach (string word in txtStrings)
                {
                    writer.WriteLine(lnJ.ToString("X3") + "-" + word);
                    lnJ++;
                }
            }
        }

        private bool loadRom(bool extra = false)
        {
            try
            {
                romData = File.ReadAllBytes(txtFileName.Text);
                if (extra)
                    romData2 = File.ReadAllBytes(txtCompare.Text);
            }
            catch
            {
                MessageBox.Show("Empty file name(s) or unable to open files.  Please verify the files exist.");
                return false;
            }
            return true;
        }

        private void saveRom()
        {
            //string options = (chkChangeStatsToRemix.Checked ? "r" : "");
            //options += (chkHalfExpGoldReq.Checked ? "h" : "");
            //options += (chkDoubleXP.Checked ? "d" : "");
            //options += (radSlightIntensity.Checked ? "l1" : radModerateIntensity.Checked ? "l2" : radHeavyIntensity.Checked ? "l3" : "l4");
            string finalFile = Path.Combine(Path.GetDirectoryName(txtFileName.Text), "DW2Random_" + txtSeed.Text + "_" + txtFlags.Text + ".nes");
            File.WriteAllBytes(finalFile, romData);
            lblIntensityDesc.Text = "ROM hacking complete!  (" + finalFile + ")";
            txtCompare.Text = finalFile;
        }

        private void changeStatsToRemix()
        {
            maxPower[0] = 105;
            maxPower[1] = 87;
            maxPower[2] = 40;
            maxPower[3] = 20;

            byte[] weapons = new byte[] { 2, 12, 27, 45, 8, 10, 15, 20, 7, 30, 40, 105, 55, 70, 40, 95 };
            int[] weaponcost = new int[] { 20, 200, 2500, 26000, 60, 100, 330, 770, 25000, 1500, 4000, 15000, 8000, 16000, 4000, 500 };
            byte[] armor = new byte[] { 2, 35, 65, 60, 6, 12, 87, 35, 25, 47, 75 };
            int[] armorcost = new int[] { 30, 1250, 70, 32767, 150, 390, 6400, 1250, 1000, 50, 50 };
            byte[] shields = new byte[] { 4, 18, 10, 40, 30 };
            int[] shieldcost = new int[] { 90, 21500, 2000, 8800, 90 };
            byte[] helmets = new byte[] { 8, 6, 20 };
            int[] helmetcost = new int[] { 20000, 3150, 20 };

            byte[] equipwho = new byte[] { 7, 7, 7, 7, 3, 3, 3, 3, 3, 1,
                1, 1, 1, 3, 3, 1, 7, 7, 7, 7,
                3, 3, 3, 3, 1, 1, 1, 3, 3, 1,
                1, 1, 7, 1, 1 };

            // Establish monster array via 6 byte chunks.
            int[,] monsterData = new int[,] // order:  Hit points(0), attack(5), defense(6), agility(4), experience(mod 256)(3, 8(256x40), 9(1024x40)), gold(2)
            // Damage, sleep, stopspell, defeat, surround, defence resistance
                {
                    { 5, 7, 5, 2, 1, 2, 0, 0, 7, 0, 0, 0 }, // Slime
                    { 8, 9, 6, 3, 2, 3, 0, 0, 7, 0, 0, 0 }, // Big Slug
                    { 5, 11, 13, 4, 2, 4, 0, 0, 7, 0, 0, 0 }, // Iron Ant
                    { 9, 12, 8, 5, 3, 3, 0, 0, 7, 0, 1, 0 }, // Drakee
                    { 10, 14, 11, 8, 5, 5, 0, 0, 7, 0, 3, 0 }, // Wild Mouse
                    { 25, 15, 10, 20, 15, 5, 1, 2, 7, 1, 0, 0 }, // Healer
                    { 12, 18, 10, 8, 6, 6, 0, 0, 7, 0, 0, 0 }, // Ghost Mouse
                    { 13, 16, 13, 9, 8, 4, 0, 1, 7, 0, 0, 0 }, // Babble
                    { 12, 19, 13, 8, 6, 7, 0, 2, 7, 1, 1, 0 }, // Army Ant (deliberatly changed from 4XP/2GP to 6XP/7GP to make it more in line with other monsters in its class)
                    { 15, 17, 11, 11, 10, 10, 0, 0, 0, 1, 1, 0 }, // Magician
                    { 16, 19, 11, 15, 7, 50, 0, 0, 7, 1, 3, 0 }, // Big Rat
                    { 14, 22, 10, 11, 9, 9, 0, 1, 7, 1, 0, 0 }, // Big Cobra
                    { 14, 18, 13, 18, 18, 8, 0, 1, 0, 0, 3, 0 }, // Magic Ant
                    { 12, 14, 10, 14, 12, 10, 0, 2, 3, 0, 2, 0 }, // Magidrakee
                    { 21, 25, 40, 13, 14, 30, 0, 1, 7, 1, 0, 0 }, // Centipod
                    { 20, 28, 16, 12, 25, 50, 0, 1, 7, 0, 1, 0 }, // Man O' War
                    { 15, 20, 10, 16, 27, 20, 0, 2, 3, 1, 1, 0 }, // Lizard Fly
                    { 60, 25, 7, 12, 40, 25, 0, 7, 7, 7, 1, 0 }, // Zombie
                    { 15, 14, 40, 15, 18, 40, 5, 1, 2, 0, 0, 2 }, // Smoke
                    { 25, 35, 12, 25, 23, 25, 0, 1, 7, 0, 3, 7 }, // Ghost Rat
                    { 35, 40, 12, 18, 33, 45, 0, 2, 7, 0, 1, 0 }, // Baboon
                    { 32, 32, 11, 12, 29, 31, 0, 3, 7, 0, 7, 0 }, // Carnivog
                    { 20, 39, 110, 13, 33, 25, 0, 1, 7, 0, 2, 0 }, // Megapede
                    { 32, 38, 11, 16, 34, 80, 2, 1, 7, 3, 1, 0 }, // Sea Slug
                    { 42, 35, 13, 22, 36, 29, 0, 5, 0, 1, 0, 0 }, // Medusa Ball
                    { 40, 36, 14, 25, 37, 30, 1, 4, 0, 1, 0, 1 }, // Enchanter / Exorcist?
                    { 28, 30, 9, 22, 32, 35, 4, 0, 7, 2, 3, 2 }, // Mud Man
                    { 38, 45, 12, 18, 40, 45, 0, 2, 7, 1, 0, 1 }, // Magic Baboon
                    { 40, 51, 16, 31, 44, 50, 0, 3, 7, 3, 0, 1 }, // Demighost / Death God?
                    { 60, 57, 20, 30, 52, 47, 1, 3, 2, 2, 0, 2 }, // Gremlin
                    { 46, 45, 18, 23, 31, 25, 1, 2, 7, 1, 1, 1 }, // Poison Lily
                    { 0, 0, 0, 0, 0, 0, 0, 7, 7, 5, 2, 2 }, // Mummy Man
                    { 26, 30, 99, 30, 50, 62, 0, 3, 4, 1, 0, 0 }, // Gorgon
                    { 25, 70, 20, 42, 45, 55, 0, 1, 7, 2, 2, 0 }, // Saber Tiger
                    { 40, 51, 21, 30, 59, 43, 0, 7, 7, 3, 0, 0 }, // Dragonfly
                    { 51, 58, 19, 30, 50, 80, 0, 1, 7, 4, 3, 3 }, // Titan Tree
                    { 65, 63, 17, 33, 45, 82, 0, 2, 7, 7, 1, 2 }, // Undead
                    { 38, 75, 25, 41, 41, 58, 0, 0, 7, 1, 0, 2 }, // Basilisk
                    { 50, 55, 16, 39, 29, 42, 0, 1, 7, 2, 7, 0 }, // Goopi
                    { 60, 75, 23, 36, 61, 50, 1, 2, 7, 4, 1, 2 }, // Orc
                    { 60, 64, 24, 70, 52, 100, 3, 6, 7, 2, 0, 0 }, // Puppet Man
                    { 0, 0, 0, 0, 0, 0, 0, 7, 7, 1, 1, 3 }, // Mummy
                    { 63, 72, 27, 38, 67, 95, 0, 3, 1, 1, 0, 0 }, // Evil Tree
                    { 50, 60, 80, 45, 39, 62, 1, 7, 1, 1, 3, 1 }, // Gas
                    { 90, 51, 2, 20, 61, 51, 0, 2, 7, 7, 2, 7 }, // Hork
                    { 60, 75, 27, 41, 64, 45, 0, 2, 1, 2, 1, 0 }, // Hawk Man
                    { 55, 61, 28, 43, 72, 110, 1, 2, 2, 2, 2, 2 }, // Sorcerer
                    { 5, 37, 255, 100, 1015, 90, 7, 7, 7, 7, 7, 7 }, // Metal Slime
                    { 65, 82, 25, 57, 77, 97, 0, 0, 7, 1, 1, 0 }, // Hunter
                    { 50, 67, 30, 45, 92, 88, 1, 2, 7, 3, 0, 2 }, // Evil Eye
                    { 60, 74, 29, 52, 81, 83, 0, 1, 1, 3, 1, 0 }, // Hibabango
                    { 60, 65, 24, 49, 48, 30, 0, 2, 7, 2, 7, 7 }, // Graboopi
                    { 100, 80, 56, 57, 83, 255, 2, 2, 2, 5, 4, 1 }, // Gold Orc
                    { 67, 73, 28, 75, 132, 10, 0, 2, 2, 3, 3, 1 }, // Evil Clown
                    { 80, 103, 19, 21, 91, 100, 0, 1, 0, 3, 0, 1 }, // Ghoul
                    { 57, 75, 25, 48, 95, 83, 0, 2, 7, 1, 0, 1 }, // Vampirus
                    { 72, 83, 28, 53, 115, 80, 0, 1, 3, 4, 1, 1 }, // Mega Knight
                    { 80, 95, 76, 71, 128, 55, 2, 7, 7, 1, 0, 0 }, // Saber Lion
                    { 70, 55, 95, 61, 125, 150, 4, 3, 7, 5, 0, 0 }, // Metal Hunter
                    { 69, 80, 41, 57, 159, 121, 1, 0, 3, 4, 3, 0 }, // Oswarg
                    { 67, 74, 22, 55, 118, 81, 4, 7, 7, 1, 5, 4 }, // Dark Eye
                    { 60, 85, 51, 64, 107, 95, 0, 1, 2, 0, 1, 2 }, // Gargoyle
                    { 110, 99, 80, 60, 204, 181, 0, 1, 1, 4, 0, 2 }, // Orc King
                    { 82, 77, 47, 79, 182, 103, 4, 1, 2, 0, 0, 0 }, // Magic Vampirus
                    { 78, 109, 63, 55, 147, 123, 0, 0, 7, 1, 1, 0 }, // Berzerker
                    { 5, 75, 255, 200, 10150, 255, 7, 7, 7, 7, 7, 7 }, // Metal Babble
                    { 77, 115, 72, 65, 201, 135, 0, 7, 2, 2, 1, 0 }, // Hargon's Knight
                    { 115, 121, 42, 43, 257, 99, 0, 3, 7, 2, 0, 3 }, // Cyclops
                    { 90 , 115, 150, 80, 554, 120, 7, 7, 7, 7, 1, 0 }, // Attackbot
                    { 90, 120, 56, 62, 480, 147, 0, 2, 7, 1, 0, 0 }, // Green Dragon
                    { 180, 110, 70, 120, 734, 170, 3, 7, 2, 4, 0, 0 }, // Mace Master
                    { 65, 85, 54, 68, 315, 101, 7, 1, 7, 2, 0, 0 }, // Flame
                    { 89, 102, 69, 83, 321, 96, 0, 1, 7, 2, 4, 0 }, // Silver Batboon
                    { 92, 95, 73, 85, 412, 113, 0, 6, 4, 3, 2, 0 }, // Blizzard
                    { 175, 150, 51, 88, 580, 165, 1, 7, 7, 6, 2, 0 }, // Giant
                    { 138, 118, 110, 85, 542, 100, 3, 2, 1, 6, 2, 0 }, // Gold Batboon
                    { 230, 140, 135, 105, 1475, 235, 2, 2, 2, 6, 2, 0 }, // Bullwong
                    { 250, 195, 160, 85, 2500, 250, 7, 7, 7, 7, 3, 0 }, // Atlas
                    { 250, 127, 170, 75, 3350, 240, 5, 2, 7, 5, 0, 0 }, // Bazuzu
                    { 320, 176, 180, 120, 4750, 255, 3, 7, 7, 7, 5, 7 }, // Zarlox
                    { 460, 177, 165, 150, 0, 0, 2, 7, 4, 7, 7, 7 }, // Hargon
                    { 0, 0, 0, 0, 0, 0, 0, 6, 7, 7, 5, 0 } // Malroth
                }; 

            // Replace monster data
            for (int lnI = 0; lnI < 82; lnI++)
            {
                if (monsterData[lnI, 0] == 0)
                    continue;

                byte hp = (byte)(monsterData[lnI, 0] > 255 ? 255 : monsterData[lnI, 0]);
                byte atk = (byte)(monsterData[lnI, 1] > 255 ? 255 : monsterData[lnI, 1]);
                byte def = (byte)(monsterData[lnI, 2] > 255 ? 255 : monsterData[lnI, 2]);
                byte agi = (byte)(monsterData[lnI, 3] > 255 ? 255 : monsterData[lnI, 3]);
                byte xp1 = (byte)(monsterData[lnI, 4] > 4095 ? 255 : (monsterData[lnI, 4] % 256));
                byte xp2 = (byte)(monsterData[lnI, 4] > 4095 ? 192 : ((monsterData[lnI, 4] / 256) % 4) * 64);
                byte xp3 = (byte)(monsterData[lnI, 4] > 4095 ? 192 : ((monsterData[lnI, 4] / 1024)) * 64);
                byte gp = (monsterData[lnI, 5] > 255 ? (byte)255 : (byte)monsterData[lnI, 5]);
                byte res1 = (byte)((monsterData[lnI, 7] * 8) + monsterData[lnI, 6]);
                byte res2 = (byte)((monsterData[lnI, 9] * 8) + monsterData[lnI, 8]);
                byte res3 = (byte)((monsterData[lnI, 11] * 8) + monsterData[lnI, 10]);

                int byteValStart = 0x13805 + (15 * lnI);
                byte attackPattern = (byte)(romData[byteValStart + 7] / 64);
                romData[byteValStart + 0] = hp;
                romData[byteValStart + 5] = atk;
                romData[byteValStart + 6] = def;
                romData[byteValStart + 4] = agi;
                romData[byteValStart + 3] = xp1;
                romData[byteValStart + 7] = (byte)(res1 + (attackPattern * 64));
                romData[byteValStart + 8] = (byte)(res2 + xp2);
                romData[byteValStart + 9] = (byte)(res3 + xp3);
                romData[byteValStart + 2] = gp;
                if (lnI == 0x2f || lnI == 0x41) // Metal Slime/Metal Babble
                {
                    romData[byteValStart + 4] = 255; // Give strength of 1 and agility of 255 to vastly increase chances of running away.
                    romData[byteValStart + 5] = 1;
                    romData[byteValStart + 10] = 0x55;
                    romData[byteValStart + 11] = 0x55;
                    if (lnI == 0x2f) romData[byteValStart + 12] = 0x66; else romData[byteValStart + 12] = 0x55;
                    if (lnI == 0x2f) romData[byteValStart + 13] = 0x66; else romData[byteValStart + 13] = 0x77;
                    romData[byteValStart + 14] = 0x00;
                }
            }

            // Replace weapon data
            for (int lnI = 0; lnI < 16; lnI++)
                romData[0x13efb + lnI] = weapons[lnI];

            // Replace armor data
            for (int lnI = 0; lnI < 11; lnI++)
                romData[0x13f0b + lnI] = armor[lnI];

            // Replace shield data
            for (int lnI = 0; lnI < 5; lnI++)
                romData[0x13f16 + lnI] = shields[lnI];

            // Replace helmet data
            for (int lnI = 0; lnI < 3; lnI++)
                romData[0x13f1b + lnI] = helmets[lnI];

            // Replace equip data
            for (int lnI = 0; lnI < 35; lnI++)
                romData[0x1a3ce + lnI] = equipwho[lnI];

            // Change the Staff Of Thunder boss fight to the Mace Master
            romData[0x10356 + (1 * 4) + 2] = 0x47;
        }

        private void doubleExp()
        {
            // Replace monster data
            for (int lnI = 0; lnI < 82; lnI++)
            {
                if (lnI == 0x4c)
                    lnI = 0x4c;
                int byteValStart = 0x13805 + (15 * lnI);

                int xp = romData[byteValStart + 3] + ((romData[byteValStart + 8] / 64) * 256) + ((romData[byteValStart + 9] / 64) * 1024);
                if (lnI != 0x2f && lnI != 0x41)
                {
                    xp *= 3;
                    xp /= 2;
                }

                byte xp1 = (byte)(xp > 4095 ? 255 : (xp % 256));
                byte xp2 = (byte)(xp > 4095 ? 192 : ((xp / 256) % 4) * 64);
                byte xp3 = (byte)(xp > 4095 ? 192 : (xp / 1024) * 64);

                romData[byteValStart + 3] = xp1;
                romData[byteValStart + 8] = (byte)((romData[byteValStart + 8] % 64) + xp2);
                romData[byteValStart + 9] = (byte)((romData[byteValStart + 9] % 64) + xp3);
            }
        }

        private void halfExpAndGoldReq(bool special = false)
        {
            // Divide encounter rates by half.
            if ((string)cboEncounterRate.SelectedItem == "300%")
            {
                romData[0x1033c] = 30; // was 10
                romData[0x1033d] = 252; // was 84
                romData[0x1033e] = 24; // was 8
                romData[0x1033f] = 12; // was 4
                romData[0x10340] = 48; // was 16
                romData[0x10341] = 75; // was 25
                romData[0x10342] = 48; // was 16
            }
            if ((string)cboEncounterRate.SelectedItem == "200%")
            {
                romData[0x1033c] = 20; // was 10
                romData[0x1033d] = 168; // was 84
                romData[0x1033e] = 16; // was 8
                romData[0x1033f] = 8; // was 4
                romData[0x10340] = 32; // was 16
                romData[0x10341] = 50; // was 25
                romData[0x10342] = 32; // was 16
            }
            if ((string)cboEncounterRate.SelectedItem == "150%")
            {
                romData[0x1033c] = 15; // was 10
                romData[0x1033d] = 126; // was 84
                romData[0x1033e] = 12; // was 8
                romData[0x1033f] = 6; // was 4
                romData[0x10340] = 24; // was 16
                romData[0x10341] = 38; // was 25
                romData[0x10342] = 24; // was 16
            }
            if ((string)cboEncounterRate.SelectedItem == "75%")
            {
                romData[0x1033c] = 8; // was 10
                romData[0x1033d] = 63; // was 84
                romData[0x1033e] = 6; // was 8
                romData[0x1033f] = 3; // was 4
                romData[0x10340] = 12; // was 16
                romData[0x10341] = 18; // was 25
                romData[0x10342] = 12; // was 16
            }
            if ((string)cboEncounterRate.SelectedItem == "50%")
            {
                romData[0x1033c] = 5; // was 10
                romData[0x1033d] = 42; // was 84
                romData[0x1033e] = 4; // was 8
                romData[0x1033f] = 2; // was 4
                romData[0x10340] = 8; // was 16
                romData[0x10341] = 12; // was 25
                romData[0x10342] = 8; // was 16
            }
            if ((string)cboEncounterRate.SelectedItem == "33%")
            {
                romData[0x1033c] = 3; // was 10
                romData[0x1033d] = 28; // was 84
                romData[0x1033e] = 3; // was 8
                romData[0x1033f] = 1; // was 4
                romData[0x10340] = 5; // was 16
                romData[0x10341] = 8; // was 25
                romData[0x10342] = 5; // was 16
            }
            if ((string)cboEncounterRate.SelectedItem == "25%")
            {
                romData[0x1033c] = 3; // was 10
                romData[0x1033d] = 21; // was 84
                romData[0x1033e] = 2; // was 8
                romData[0x1033f] = 1; // was 4
                romData[0x10340] = 4; // was 16
                romData[0x10341] = 6; // was 25
                romData[0x10342] = 4; // was 16
            }

            // We'll divide all of these by two later...
            int[] weaponcost = new int[] { 20, 200, 2500, 26000, 60, 100, 330, 770, 25000, 1500, 4000, 15000, 8000, 16000, 4000, 500 };
            int[] armorcost = new int[] { 30, 1250, 70, 32767, 150, 390, 6400, 1250, 1000, 32000, 48000 };
            int[] shieldcost = new int[] { 90, 21500, 2000, 8800, 15000 };
            int[] helmetcost = new int[] { 20000, 3150, 15000 };
            // Adjusting item costs for the super randomizer, where they could be made available for purchasing in a store!
            int[] itemcost = new int[] { 10, 300, 300, 0, 0, 5000, 600, 0, 8000, 8000, 1000, 1500, 640, 10000, 20000, 70, 40, 80, 2, 3000, 1500, 2000, 0, 8, 15, 10000, 2, 2 };

            int[] midenhallExpReq = new int[] { 12, 20, 40, 68, 140, 280, 440, 800, 1000,
                  1100, 1400, 2300, 2400, 3000, 4000, 4000, 5000, 6000, 8000,
                10000, 12000, 13000, 15000, 17000, 20000, 23000, 25000, 25000, 30000,
                30000, 30000, 30000, 30000, 30000, 30000, 30000, 30000, 30000, 30000,
                40000, 50000, 50000, 50000, 50000, 50000, 50000, 50000, 50000, 30000 };
            int[] cannockExpReq = new int[] { 24, 36, 50, 90,
                180, 320, 600, 1100, 1600, 2000, 2200, 2800, 4000, 4000,
                5000, 6000, 7000, 9000, 11000, 13000, 15000, 15000, 16000, 18000,
                22000, 26000, 28000, 30000, 40000, 30000, 30000, 40000, 50000, 50000,
                40000, 60000, 60000, 60000, 60000, 60000, 20000, 60000, 60000, 40000 };
            int[] moonbrookeExpReq = new int[] { 100, 200, 300, 600,
                1200, 1800, 2200, 2600, 3000, 4000, 4000, 5000, 6000, 8000,
                11000, 15000, 16000, 20000, 22000, 25000, 30000, 40000, 50000, 30000,
                30000, 30000, 40000, 50000, 90000, 90000, 100000, 90000, 90000, 90000 };

            if ((string)cboXPReq.SelectedItem != "100%")
            {
                if ((string)cboXPReq.SelectedItem != "75%")
                    romData[0x11d69] = 255;  // last six levels are forced to be at least 65,535 points, unless you change this variable here.

                for (int lnI = 0; lnI < 49; lnI++)
                {
                    int xp = midenhallExpReq[lnI];
                    if ((string)cboXPReq.SelectedItem == "75%") xp = xp * 3 / 4;
                    if ((string)cboXPReq.SelectedItem == "50%") xp = xp / 2;
                    if ((string)cboXPReq.SelectedItem == "33%") xp = xp / 3;

                    romData[0x13cd3 + (lnI * 2)] = (byte)(special ? 1 : xp % 256);
                    romData[0x13cd4 + (lnI * 2)] = (byte)(special ? 0 : xp / 256);
                    if (lnI < 44)
                    {
                        xp = cannockExpReq[lnI];
                        if ((string)cboXPReq.SelectedItem == "75%") xp = xp * 3 / 4;
                        if ((string)cboXPReq.SelectedItem == "50%") xp = xp / 2;
                        if ((string)cboXPReq.SelectedItem == "33%") xp = xp / 3;

                        romData[0x13d35 + (lnI * 2)] = (byte)(special ? 1 : xp % 256);
                        romData[0x13d36 + (lnI * 2)] = (byte)(special ? 0 : xp / 256);
                    }
                    if (lnI < 34)
                    {
                        xp = moonbrookeExpReq[lnI];
                        if ((string)cboXPReq.SelectedItem == "75%") xp = xp * 3 / 4;
                        if ((string)cboXPReq.SelectedItem == "50%") xp = xp / 2;
                        if ((string)cboXPReq.SelectedItem == "33%") xp = xp / 3;
                        if (xp > 65535) xp -= 65535;

                        romData[0x13d8d + (lnI * 2)] = (byte)(special ? 1 : xp % 256);
                        romData[0x13d8e + (lnI * 2)] = (byte)(special ? 0 : xp / 256);
                    }
                }
            }

            // Replace weapon data
            for (int lnI = 0; lnI < 16; lnI++)
            {
                int gp = weaponcost[lnI];
                if ((string)cboGPReq.SelectedItem == "75%") gp = gp * 3 / 4;
                if ((string)cboGPReq.SelectedItem == "50%") gp = gp / 2;
                if ((string)cboGPReq.SelectedItem == "33%") gp = gp / 3;
                romData[0x1a00e + (lnI * 2)] = (byte)(special ? 1 : gp % 256);
                romData[0x1a00f + (lnI * 2)] = (byte)(special ? 0 : gp / 256);
            }

            // Replace armor data
            for (int lnI = 0; lnI < 11; lnI++)
            {
                int gp = armorcost[lnI];
                if ((string)cboGPReq.SelectedItem == "75%") gp = gp * 3 / 4;
                if ((string)cboGPReq.SelectedItem == "50%") gp = gp / 2;
                if ((string)cboGPReq.SelectedItem == "33%") gp = gp / 3;
                romData[0x1a02e + (lnI * 2)] = (byte)(special ? 1 : gp % 256);
                romData[0x1a02f + (lnI * 2)] = (byte)(special ? 0 : gp / 256);
            }

            // Replace shield data
            for (int lnI = 0; lnI < 5; lnI++)
            {
                int gp = shieldcost[lnI];
                if ((string)cboGPReq.SelectedItem == "75%") gp = gp * 3 / 4;
                if ((string)cboGPReq.SelectedItem == "50%") gp = gp / 2;
                if ((string)cboGPReq.SelectedItem == "33%") gp = gp / 3;
                romData[0x1a044 + (lnI * 2)] = (byte)(special ? 1 : gp % 256);
                romData[0x1a045 + (lnI * 2)] = (byte)(special ? 0 : gp / 256);
            }

            // Replace helmet data
            for (int lnI = 0; lnI < 3; lnI++)
            {
                int gp = helmetcost[lnI];
                if ((string)cboGPReq.SelectedItem == "75%") gp = gp * 3 / 4;
                if ((string)cboGPReq.SelectedItem == "50%") gp = gp / 2;
                if ((string)cboGPReq.SelectedItem == "33%") gp = gp / 3;
                romData[0x1a04e + (lnI * 2)] = (byte)(special ? 1 : gp % 256);
                romData[0x1a04f + (lnI * 2)] = (byte)(special ? 0 : gp / 256);
            }

            // Replace item data
            for (int lnI = 0; lnI < 28; lnI++)
            {
                int gp = itemcost[lnI];
                if ((string)cboGPReq.SelectedItem == "75%") gp = gp * 3 / 4;
                if ((string)cboGPReq.SelectedItem == "50%") gp = gp / 2;
                if ((string)cboGPReq.SelectedItem == "33%") gp = gp / 3;
                romData[0x1a054 + (lnI * 2)] = (byte)(special ? 1 : gp % 256);
                romData[0x1a055 + (lnI * 2)] = (byte)(special ? 0 : gp / 256);
            }

            // House of healing cost halved
            int gp1 = 20;
            if ((string)cboGPReq.SelectedItem == "75%") gp1 = gp1 * 3 / 4;
            if ((string)cboGPReq.SelectedItem == "50%") gp1 = gp1 / 2;
            if ((string)cboGPReq.SelectedItem == "33%") gp1 = gp1 / 3;

            romData[0x18659] = (byte)gp1;

            // Inn prices halved
            byte[] inns = { 4, 6, 8, 12, 20, 2, 25, 30, 40, 30 };
            for (int lnI = 0; lnI < inns.Length; lnI++)
            {
                int gp = inns[lnI];
                if ((string)cboGPReq.SelectedItem == "75%") gp = gp * 3 / 4;
                if ((string)cboGPReq.SelectedItem == "50%") gp = gp / 2;
                if ((string)cboGPReq.SelectedItem == "33%") gp = gp / 3;
                romData[0x19f90 + lnI] = (byte)gp;
            }
        }

        private void superRandomize()
        {
            Random r1;
            try
            {
                r1 = new Random(int.Parse(txtSeed.Text));
            }
            catch
            {
                MessageBox.Show("Invalid seed.  It must be a number from 0 to 2147483648.");
                return;
            }

            randomLevel = (radSlightIntensity.Checked ? 1 : radModerateIntensity.Checked ? 2 : radHeavyIntensity.Checked ? 3 : 4);

            if (chkMap.Checked)
            {
                bool goodMap = false;
                while (!goodMap)
                    goodMap = (chkSmallMap.Checked ? randomizeMapv3(r1) : randomizeMapv2(r1));
            }
                

            if (chkEquipment.Checked)
                randomizeEquipment(r1);
            if (chkWhoCanEquip.Checked)
                randomizeWhoEquip(r1);
            if (chkEquipEffects.Checked)
                randomizeEffects(r1);
            if (chkMonsterStats.Checked)
                randomizeMonsterStats(r1);
            if (chkMonsterZones.Checked)
                randomizeMonsterZones(r1);
            if (chkSpellLearning.Checked)
                randomizeSpellLearning(r1);
            if (chkSpellStrengths.Checked)
                randomizeSpellStrengths(r1);
            if (chkHeroStats.Checked)
                randomizeStats(r1);
            if (chkHeroStores.Checked)
                randomizeStores(r1); // Must do before treasures.
            if (chkTreasures.Checked)
                randomizeTreasures(r1);
        }

        private List<int> addTreasure(List<int> currentList, int[] treasureData)
        {
            for (int lnI = 0; lnI < treasureData.Length; lnI++)
                currentList.Add(treasureData[lnI]);
            return currentList;
        }

        private void shuffle(int[] treasureData, Random r1, bool keyItemAvoidance = false)
        {
            // Do not exceed these zones defined for the key items, or you're going to be stuck!
            int[] keyZoneMax = { 13, 13, 23, 40, 45, 53 }; // Cloak of wind, Mirror Of Ra, Golden Key, Jailor's Key, Moon Fragment, Eye Of Malroth
            List<byte> keyItems = new List<byte> { 0x2b, 0x2e, 0x37, 0x39, 0x26, 0x28 }; // When we reach insane randomness, we'll want to know what the key items are so we place them in the appropriate zones...

            // Shuffle each zone 15 times the length of the array for randomness.
            for (int lnI = 0; lnI < 15 * treasureData.Length; lnI++)
            {
                int swap1 = r1.Next() % treasureData.Length;
                int swap2 = r1.Next() % treasureData.Length;

                // Don't shuffle if key items would be swapped into inaccessible areas.
                if (keyItemAvoidance) {
                    int position1 = keyItems.IndexOf(romData[treasureData[swap1]]);
                    int position2 = keyItems.IndexOf(romData[treasureData[swap2]]);
                    if (position1 > -1 && swap2 > keyZoneMax[position1])
                        continue;
                    if (position2 > -1 && swap1 > keyZoneMax[position2])
                        continue;
                }

                swap(treasureData[swap1], treasureData[swap2]);
            }
        }

        private void swap(int firstAddress, int secondAddress)
        {
            byte holdAddress = romData[secondAddress];
            romData[secondAddress] = romData[firstAddress];
            romData[firstAddress] = holdAddress;
        }

        // Reserve for another time...
        private void button1_Click(object sender, EventArgs e)
        {
            if (!loadRom()) return;
            halfExpAndGoldReq(true);
            for (int lnI = 0; lnI < 68; lnI++)
            {
                int byteToUse = 0x10519 + (lnI * 6);
                byte valToUpdate = (byte)(129 + lnI);
                romData[byteToUse + 0] = valToUpdate;
                romData[byteToUse + 1] = valToUpdate;
                romData[byteToUse + 2] = valToUpdate;
                romData[byteToUse + 3] = valToUpdate;
                romData[byteToUse + 4] = valToUpdate;
                romData[byteToUse + 5] = valToUpdate;
            }
            saveRom();
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            if (!loadRom(true)) return;
            using (StreamWriter writer = File.CreateText(Path.Combine(Path.GetDirectoryName(txtFileName.Text), "DW2Compare.txt")))
            {
                for (int lnI = 0; lnI < 82; lnI++)
                    compareComposeString("monsters" + (lnI + 1).ToString("X2"), writer, (0x13805 + (15 * lnI)), 15);

                compareComposeString("midenhallEXP", writer, 0x13cd3, 100);
                compareComposeString("cannockEXP", writer, 0x13d35, 90);
                compareComposeString("moonbrookeEXP", writer, 0x13d8d, 70);
                compareComposeString("goldReq", writer, 0x1a00e, 126);

                compareComposeString("dewsyarn", writer, 0x19b5c, 1);
                compareComposeString("treasuresMiden", writer, 0x19e41, 20, 4);
                compareComposeString("treasuresCannock", writer, 0x19e59, 4, 4);
                compareComposeString("treasuresOsterfair", writer, 0x19e5d, 8, 4);
                compareComposeString("treasuresZahan1", writer, 0x19e65, 4, 4);
                compareComposeString("treasuresZahan2", writer, 0x19f32, 4, 4);
                compareComposeString("treasuresCharlock1", writer, 0x19eb5, 4, 4);
                compareComposeString("treasuresCharlock2", writer, 0x19e69, 16, 4);
                compareComposeString("treasuresLake", writer, 0x19e79, 29, 4);
                compareComposeString("treasuresSea", writer, 0x19e95, 33, 4);
                compareComposeString("treasuresRhone", writer, 0x19eb9, 33, 4);
                compareComposeString("treasuresSpring", writer, 0x19ed9, 13, 4);
                compareComposeString("treasuresMoon", writer, 0x19ee5, 21, 4);
                compareComposeString("treasuresLighthouse", writer, 0x19ef9, 21, 4);
                compareComposeString("treasuresWind", writer, 0x19f0d, 13, 4);
                compareComposeString("oddTreasure(1/9/13/16)", writer, 0x19f1a, 20, 4);

                for (int lnI = 0; lnI < 8; lnI++)
                    compareComposeString("weaponContents" + lnI.ToString("X2"), writer, 0x19f9a + (6 * lnI), 6);
                for (int lnI = 0; lnI < 11; lnI++)
                    compareComposeString("itemContents" + lnI.ToString("X2"), writer, 0x19f9a + 48 + (6 * lnI), 6);
                for (int lnI = 0; lnI < 68; lnI++)
                    compareComposeString("monsterZones" + lnI.ToString("X2"), writer, (0x10519 + (6 * lnI)), 6);
                for (int lnI = 0; lnI < 19; lnI++)
                    compareComposeString("monsterSpecial" + lnI.ToString("X2"), writer, (0x106b1 + (4 * lnI)), 4);
                for (int lnI = 0; lnI < 13; lnI++)
                    compareComposeString("monsterBoss" + lnI.ToString("X2"), writer, (0x10356 + (4 * lnI)), 4);
                compareComposeString("statStart", writer, 0x13dd1, 12);
                for (int lnI = 0; lnI < 35; lnI++)
                    compareComposeString("statUps" + lnI.ToString(), writer, 0x13ddd + (6 * lnI), 6);
                for (int lnI = 0; lnI < 10; lnI++)
                    compareComposeString("statUps" + (lnI + 35).ToString(), writer, 0x13ddd + 210 + (4 * lnI), 4);
                for (int lnI = 0; lnI < 5; lnI++)
                    compareComposeString("statUps" + (lnI + 45).ToString(), writer, 0x13ddd + 250 + (2 * lnI), 2);
                compareComposeString("spellLearning", writer, 0x13edb, 32);
                compareComposeString("spellsLearned", writer, 0x1ae76, 32);
                for (int lnI = 0; lnI < 28; lnI++)
                    compareComposeString("spellStats" + (lnI).ToString(), writer, 0x127d5 + (5 * lnI), 5);
                compareComposeString("spellCmd", writer, 0x13528, 28);
                compareComposeString("spellFieldHeal", writer, 0x18be0, 16, 8);
                compareComposeString("spellFieldMedical", writer, 0x19602, 1);

                compareComposeString("start1", writer, 0x3c79f, 8);
                compareComposeString("start2", writer, 0x3c79f + 8, 8);
                compareComposeString("start3", writer, 0x3c79f + 16, 8);
                compareComposeString("weapons", writer, 0x13efb, 16);
                compareComposeString("weaponcost (2.3)", writer, 0x1a00e, 32);
                compareComposeString("armor", writer, 0x13efb + 16, 11);
                compareComposeString("armorcost (2.4)", writer, 0x1a00e + 32, 22);
                compareComposeString("shields", writer, 0x13efb + 27, 5);
                compareComposeString("shieldcost (2.8)", writer, 0x1a00e + 54, 10);
                compareComposeString("helmets", writer, 0x13efb + 32, 3);
                compareComposeString("helmetcost (3.0)", writer, 0x1a00e + 64, 6);

            }
            lblIntensityDesc.Text = "Comparison complete!  (DW2Compare.txt)";
        }

        private StreamWriter compareComposeString(string intro, StreamWriter writer, int startAddress, int length, int skip = 1)
        {
            string final = "";
            string final2 = "";
            for (int lnI = 0; lnI < length; lnI += skip)
            {
                final += romData[startAddress + lnI].ToString("X2") + " ";
                final2 += romData2[startAddress + lnI].ToString("X2") + " ";
            }
            writer.WriteLine(intro);
            writer.WriteLine(final);
            writer.WriteLine(final2);
            writer.WriteLine();
            return writer;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (txtFileName.Text != "")
                using (StreamWriter writer = File.CreateText("lastFile.txt"))
                {
                    writer.WriteLine(txtFileName.Text);
                    writer.WriteLine(txtCompare.Text);
                    writer.WriteLine(txtSeed.Text);
                    writer.WriteLine(txtFlags.Text);
                    writer.WriteLine(txtPrinceName.Text);
                    writer.WriteLine(txtPrincessName.Text);
                }
        }

        private void txtFileName_Leave(object sender, EventArgs e)
        {
            runChecksum();
        }

        private void btnCompareBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtCompare.Text = openFileDialog1.FileName;
            }
        }

        private void txtFlags_TextChanged(object sender, EventArgs e)
        {
            flagLoad();
        }

        private void flagLoad()
        {
            // set checkboxes based on flags entered.
            chkChangeStatsToRemix.Checked = (txtFlags.Text.Contains("R"));
            chkXPRandomize.Checked = (txtFlags.Text.Contains("X"));
            chkGPRandomize.Checked = (txtFlags.Text.Contains("G"));
            chkMap.Checked = (txtFlags.Text.Contains("U"));
            chkSmallMap.Checked = (txtFlags.Text.Contains("u"));
            chkEquipment.Checked = (txtFlags.Text.Contains("Q"));
            chkWhoCanEquip.Checked = (txtFlags.Text.Contains("W"));
            chkEquipEffects.Checked = (txtFlags.Text.Contains("E"));
            chkMonsterStats.Checked = (txtFlags.Text.Contains("M"));
            chkMonsterZones.Checked = (txtFlags.Text.Contains("Z"));
            chkSpellLearning.Checked = (txtFlags.Text.Contains("L"));
            chkSpellStrengths.Checked = (txtFlags.Text.Contains("S"));
            chkHeroStats.Checked = (txtFlags.Text.Contains("H"));
            chkHeroStores.Checked = (txtFlags.Text.Contains("C"));
            chkTreasures.Checked = (txtFlags.Text.Contains("T"));

            if (txtFlags.Text.Contains("r1")) radSlightIntensity.Checked = true;
            if (txtFlags.Text.Contains("r2")) radModerateIntensity.Checked = true;
            if (txtFlags.Text.Contains("r3")) radHeavyIntensity.Checked = true;
            if (txtFlags.Text.Contains("r4")) radInsaneIntensity.Checked = true;
            cboGPReq.SelectedItem = (txtFlags.Text.Contains("g1") ? "75%" : txtFlags.Text.Contains("g2") ? "50%" : txtFlags.Text.Contains("g3") ? "33%" : "100%");
            cboXPReq.SelectedItem = (txtFlags.Text.Contains("x1") ? "75%" : txtFlags.Text.Contains("x2") ? "50%" : txtFlags.Text.Contains("x3") ? "33%" : "100%");
            cboEncounterRate.SelectedItem = (txtFlags.Text.Contains("e1") ? "300%" : txtFlags.Text.Contains("e2") ? "200%" : txtFlags.Text.Contains("e3") ? "150%" : 
                txtFlags.Text.Contains("e4") ? "75%" : txtFlags.Text.Contains("e5") ? "50%" : txtFlags.Text.Contains("e6") ? "33%" : txtFlags.Text.Contains("e7") ? "25%" : "100%");
        }

        private void determineFlag()
        {
            if (loading)
                return;

            string flags = "";
            if (chkChangeStatsToRemix.Checked)
                flags += "R";
            if (chkXPRandomize.Checked)
                flags += "X";
            if (chkGPRandomize.Checked)
                flags += "G";
            if (chkMap.Checked)
                flags += "U";
            if (chkSmallMap.Checked)
                flags += "u";
            if (chkEquipment.Checked)
                flags += "Q";
            if (chkWhoCanEquip.Checked)
                flags += "W";
            if (chkEquipEffects.Checked)
                flags += "E";
            if (chkMonsterStats.Checked)
                flags += "M";
            if (chkMonsterZones.Checked)
                flags += "Z";
            if (chkSpellLearning.Checked)
                flags += "L";
            if (chkSpellStrengths.Checked)
                flags += "S";
            if (chkHeroStats.Checked)
                flags += "H";
            if (chkHeroStores.Checked)
                flags += "C";
            if (chkTreasures.Checked)
                flags += "T";

            flags += (radSlightIntensity.Checked ? "r1" : radModerateIntensity.Checked ? "r2" : radHeavyIntensity.Checked ? "r3" : "r4");
            flags += ((string)cboGPReq.SelectedItem == "75%" ? "g1" : (string)cboGPReq.SelectedItem == "50%" ? "g2" : (string)cboGPReq.SelectedItem == "33%" ? "g3" : "");
            flags += ((string)cboXPReq.SelectedItem == "75%" ? "x1" : (string)cboXPReq.SelectedItem == "50%" ? "x2" : (string)cboXPReq.SelectedItem == "33%" ? "x3" : "");
            flags += ((string)cboEncounterRate.SelectedItem == "300%" ? "e1" : (string)cboEncounterRate.SelectedItem == "200%" ? "e2" : (string)cboEncounterRate.SelectedItem == "150%" ? "e3" : 
                (string)cboEncounterRate.SelectedItem == "75%" ? "e4" : (string)cboEncounterRate.SelectedItem == "50%" ? "e5" : (string)cboEncounterRate.SelectedItem == "33%" ? "e6" : 
                (string)cboEncounterRate.SelectedItem == "25%" ? "e7" : "");
            txtFlags.Text = flags;
        }

        private void determineFlags(object sender, EventArgs e)
        {
            determineFlag();
        }

        private void btnUltraRando_Click(object sender, EventArgs e)
        {
            chkMap.Checked = true;
            chkEquipment.Checked = false;
            chkEquipEffects.Checked = false;
            chkWhoCanEquip.Checked = true;
            chkMonsterStats.Checked = true;
            chkMonsterZones.Checked = true;
            chkSpellLearning.Checked = true;
            chkSpellStrengths.Checked = false;
            chkHeroStats.Checked = true;
            chkHeroStores.Checked = true;
            chkTreasures.Checked = true;

            radSlightIntensity.Checked = false;
            radModerateIntensity.Checked = false;
            radHeavyIntensity.Checked = true;
            radInsaneIntensity.Checked = false;

            chkChangeStatsToRemix.Checked = true;
        }
    }
}
