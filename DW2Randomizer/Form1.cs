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
using System.Linq;

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
        int[,] zone = new int[16, 16];
        int[] maxIsland = new int[4];
        List<int> islands = new List<int>();

        public Form1()
        {
            InitializeComponent();
        }

        private void turnIntoDogs()
        {
            // Start at 0xA6E1 (Cannock), turn them into 0x09, and take it from there... NPCs are defined every five bytes.  
            // Figure out where that's being read so you can update each NPC appropriately for each location.
            // ROM execute to E594 bank 0F.
            // Cannock - 0xA6E1-0xA72C
            //for (int i = 0xa62e; i <= 0xa638; i += 5)
            //    romData[i] = 0x09;

            romData[0xa62e] = 0x09;
            romData[0xa633] = 0x09;
            romData[0xa638] = 0x09;
            romData[0xa63e] = 0x09;

            for (int i = 0xa643; i <= 0xa684; i += 5)
                romData[i] = 0x09;

            for (int i = 0xa69a; i <= 0xa6db; i += 5)
                romData[i] = 0x09;

            for (int i = 0xa6e1; i <= 0xa72c; i += 5)
                romData[i] = 0x09;

            for (int i = 0xa732; i <= 0xa778; i += 5)
                romData[i] = 0x09;

            for (int i = 0xa77e; i <= 0xa783; i += 5)
                romData[i] = 0x09;

            for (int i = 0xa789; i <= 0xa793; i += 5)
                romData[i] = 0x09;

            for (int i = 0xa799; i <= 0xa799; i += 5)
                romData[i] = 0x09;

            for (int i = 0xa79f; i <= 0xa7ef; i += 5)
                romData[i] = 0x09;

            for (int i = 0xa7f5; i <= 0xa838; i += 5)
                romData[i] = 0x09;

            romData[0xa83c] = 0x09;
            romData[0xa842] = 0x09;

            for (int i = 0xa848; i <= 0xa88e; i += 5)
                romData[i] = 0x09;

            for (int i = 0xa894; i <= 0xa8df; i += 5)
                romData[i] = 0x09;

            for (int i = 0xa8e5; i <= 0xa926; i += 5)
                romData[i] = 0x09;

            for (int i = 0xa92c; i <= 0xa936; i += 5)
                romData[i] = 0x09;

            for (int i = 0xa93c; i <= 0xa987; i += 5)
                romData[i] = 0x09;

            for (int i = 0xa98d; i <= 0xa9e2; i += 5)
                romData[i] = 0x09;

            for (int i = 0xa9e8; i <= 0xaa06; i += 5)
                romData[i] = 0x09;

            for (int i = 0xaa0c; i <= 0xaa16; i += 5)
                romData[i] = 0x09;

            romData[0xaa1c] = 0x09;
            romData[0xaa22] = 0x09;
            romData[0xaa28] = 0x09;
            romData[0xaa2d] = 0x09;
            romData[0xaa32] = 0x09;
            romData[0xaa38] = 0x09;
            romData[0xaa3e] = 0x09;
            romData[0xaa44] = 0x09;
            romData[0xaa4a] = 0x09;
            romData[0xaa50] = 0x09;
            romData[0xaa55] = 0x09;
            romData[0xaa5b] = 0x09;
            romData[0xaa61] = 0x09;
            romData[0xaa66] = 0x09;
            romData[0xaa6c] = 0x09;
            romData[0xaa71] = 0x09;
            romData[0xaa77] = 0x09;
            romData[0xaa7c] = 0x09;
            romData[0xaa82] = 0x09;
            romData[0xaa88] = 0x09;
            romData[0xaa8e] = 0x09;
            romData[0xaa93] = 0x09;
        }

        private bool randomizeMapv5(Random r1)
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

            int islandSize = (r1.Next() % 20000) + 30000; // (lnI == 0 ? 1500 : lnI == 1 ? 2500 : lnI == 2 ? 1500 : lnI == 3 ? 1500 : lnI == 4 ? 5000 : 5000);
            islandSize /= (chkSmallMap.Checked ? 4 : 1);

            // Set up three special zones.  Zone 1000 = 25 squares and has Cannock stuff.  Zone 2000 = 30 squares and has Moonbrooke stuff.  
            // Zone 3000 = 48 squares and has Hargon stuff.  It will be surrounded by eight tiles of mountains.
            // This takes up 94 / 256 of the total squares available.

            bool zonesCreated = false;
            while (!zonesCreated)
            {
                zone = new int[16, 16];
                if (createZone(3000, 48, true, r1) && createZone(1000, 25, false, r1) && createZone(2000, 32, false, r1))
                    zonesCreated = true;
            }

            markZoneSides();
            generateZoneMap(3000, true, islandSize * 24 / 256, r1);
            generateZoneMap(1000, false, islandSize * 25 / 256, r1);
            generateZoneMap(2000, false, islandSize * 32 / 256, r1);
            generateZoneMap(0, false, islandSize * 175 / 256, r1);
            createBridges(r1);
            resetIslands();


            // We should mark islands and inaccessible land...
            int lakeNumber = 256;

            int maxPlots = 0;
            int maxLake = 0;
            for (int lnI = 0; lnI < 256; lnI++)
                for (int lnJ = 0; lnJ < 256; lnJ++)
                {
                    if (island[lnI, lnJ] == -1)
                    {
                        int plots = lakePlot(lakeNumber, lnI, lnJ);
                        if (plots > maxPlots)
                        {
                            maxPlots = plots;
                            maxLake = lakeNumber;
                        }
                        lakeNumber++;
                    }
                }

            // Establish Midenhall location
            bool midenOK = false;
            int[] midenX = new int[4];
            int[] midenY = new int[4];
            while (!midenOK)
            {
                midenX[1] = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                midenY[1] = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                if (validPlot(midenY[1], midenX[1], 2, 2, new int[] { maxIsland[1] }))
                    midenOK = true;
            }

            // Cannock Cave
            midenOK = false;
            while (!midenOK)
            {
                midenX[2] = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                midenY[2] = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                if (validPlot(midenY[2], midenX[2], 1, 1, new int[] { maxIsland[2] }))
                    midenOK = true;
            }

            // Rhone Shrine
            midenOK = false;
            while (!midenOK)
            {
                midenX[3] = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                midenY[3] = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                if (validPlot(midenY[3], midenX[3], 1, 1, new int[] { maxIsland[3] }))
                    midenOK = true;
            }

            // Moonbrooke Shrine (west)
            midenOK = false;
            while (!midenOK)
            {
                midenX[0] = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                midenY[0] = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                if (validPlot(midenY[0], midenX[0], 1, 1, new int[] { maxIsland[0] }))
                    midenOK = true;
            }

            islands.Remove(maxIsland[1]);
            islands.Remove(maxIsland[2]);
            islands.Remove(maxIsland[3]);

            bool treeLegal = false;
            while (!treeLegal)
            {
                int x = r1.Next() % (chkSmallMap.Checked ? 121 : 249);
                int y = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                if (validPlot(y, x, 5, 5, islands.ToArray()) && reachable(y, x, true, midenX[1], midenY[1], maxLake))
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

            bool treasuresLegal = false;
            while (!treasuresLegal)
            {
                int x = r1.Next() % (chkSmallMap.Checked ? 121 : 249);
                int y = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                if (validPlot(y, x, 5, 5, islands.ToArray()) && reachable(y, x, true, midenX[1], midenY[1], maxLake))
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
            }

            // Mirror Of Ra
            bool mirrorLegal = false;
            while (!mirrorLegal)
            {
                int x = r1.Next() % (chkSmallMap.Checked ? 121 : 249);
                int y = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                if (validPlot(y, x, 5, 6, new int[] { maxIsland[2] }) && reachable(y, x, false, midenX[2], midenY[2], maxLake))
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
            // Midenhall can go anywhere.  But Cannock has to be 15-30 squares or less away from there.
            // Don't place Hargon's Castle for now.  OK, place it for now.  But I may change my mind later.
            for (int lnI = 0; lnI < 7; lnI++)
            {
                int x = 300;
                int y = 300;
                if (lnI == 0) { x = midenX[1]; y = midenY[1]; }
                else
                {
                    x = r1.Next() % (chkSmallMap.Checked ? 125 : 253);
                    y = r1.Next() % (chkSmallMap.Checked ? 125 : 253);
                }

                if (validPlot(y, x, 2, 2, (lnI == 0 || lnI == 1 ? new int[] { maxIsland[1] } : lnI == 6 ? new int[] { maxIsland[3] } : islands.ToArray())) && reachable(y, x, (lnI != 0 && lnI != 1), 
                    lnI == 6 ? midenX[3] : midenX[1], lnI == 6 ? midenY[3] : midenY[1], maxLake))
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
                    if (lnI == 3)
                    {
                        // Replace Tantegel music with the zone surrounding Tantegel.
                        romData[0x3e356] = (byte)((x / 8) * 8);
                        romData[0x3e35a] = (byte)(((x / 8) + 1) * 8);
                        romData[0x3e360] = (byte)((y / 8) * 8);
                        romData[0x3e364] = (byte)(((y / 8) + 1) * 8);
                    }
                    //if (lnI == 6)
                    //{
                    //    romData[0xa301] = (byte)(x);
                    //    romData[0xa302] = (byte)(y + 1);
                    //    romData[0xfd95] = 0x80;
                    //    romData[0xfd96] = 0x0d;
                    //    romData[0xfd97] = 0x18;
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
            // Leftwyne must be 15/30 squares or less away from Midenhall.  Hamlin has to be 30/60 squares or less away from Midenhall.
            for (int lnI = 0; lnI < 7; lnI++)
            {
                //if (lnI == 6) lnI = lnI;
                int x = r1.Next() % (chkSmallMap.Checked ? 125 : 253);
                int y = r1.Next() % (chkSmallMap.Checked ? 125 : 253);

                if (validPlot(y, x, 1, 2, (lnI == 0 ? new int[] { maxIsland[1] } : lnI == 1 ? new int[] { maxIsland[2] } : lnI == 2 ? new int[] { maxIsland[0] } : islands.ToArray())) 
                    && reachable(y, x, (lnI != 0 && lnI != 1 && lnI != 2), (lnI == 1 ? midenX[2] : lnI == 2 ? midenX[0] : midenX[1]), (lnI == 1 ? midenY[2] : lnI == 2 ? midenY[0] : midenY[1]), maxLake))
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
                if ((lnI == 0) && chkSmallMap.Checked) continue; // Remove the Midenhall Island shrine which is of no importance.
                // lnI == 1 is probably the Cannock shrine... want to put that in Zone 1...

                int x = r1.Next() % (chkSmallMap.Checked ? 125 : 253);
                int y = r1.Next() % (chkSmallMap.Checked ? 125 : 253);
                if (lnI == 6)
                {
                    x = midenX[3];
                    y = midenY[3];
                }
                else if (lnI == 7)
                {
                    x = midenX[0];
                    y = midenY[0];
                }

                if (validPlot(y, x, 1, 1, (lnI == 1 || lnI == 12 ? new int[] { maxIsland[1] } : lnI == 6 ? new int[] { maxIsland[3] } : lnI == 7 ? new int[] { maxIsland[0] } : lnI == 8 ? new int[] { maxIsland[2] } : islands.ToArray())) 
                    && reachable(y, x, (lnI != 1 && lnI != 12 && lnI != 8 && lnI != 7 && lnI != 6), lnI == 6 ? midenX[3] : lnI == 7 ? midenX[0] : lnI == 8 ? midenX[2] : midenX[1],
                    lnI == 6 ? midenY[3] : lnI == 7 ? midenY[0] : lnI == 8 ? midenY[2] : midenY[1], maxLake))
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
            // Make sure the lake and spring cave is no more than 16/32 squares outside of Midenhall
            for (int lnI = 0; lnI < 9; lnI++)
            {
                int x = 300;
                int y = 300;
                if (lnI == 6)
                {
                    x = midenX[2];
                    y = midenY[2];
                }
                else
                {
                    x = r1.Next() % (chkSmallMap.Checked ? 125 : 253);
                    y = r1.Next() % (chkSmallMap.Checked ? 125 : 253);
                }

                if (validPlot(y, x, 1, 1, (lnI == 0 || lnI == 6 ? new int[] { maxIsland[2] } : lnI == 1 || lnI == 5 ? new int[] { maxIsland[1] } : lnI == 7 ? new int[] { maxIsland[3] } : islands.ToArray())) 
                    && reachable(y, x, (lnI != 0 && lnI != 1 && lnI != 5 && lnI != 6 && lnI != 7), 
                    lnI == 0 || lnI == 6 ? midenX[2] : lnI == 7 ? midenX[3] : midenX[1], lnI == 0 || lnI == 6 ? midenY[2] : lnI == 7 ? midenY[3] : midenY[1], maxLake))
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
            // Need to make sure the wind tower is no more than 14/28 squares outside of Midenhall
            for (int lnI = 0; lnI < 5; lnI++)
            {
                if ((lnI == 3 || lnI == 4) && chkSmallMap.Checked) continue; // Remove the Dragon's Horns from the small map
                int x = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                int y = r1.Next() % (chkSmallMap.Checked ? 122 : 250);

                // Need to make sure it's a valid 7x7 plot due to dropping with the Cloak of wind...
                if (validPlot(y, x, 3, 3, (lnI == 0 ? new int[] { maxIsland[2] } : islands.ToArray())) 
                    && reachable(y, x, (lnI != 0), lnI == 0 ? midenX[2] : midenX[1], lnI == 0 ? midenY[2] : midenY[1], maxLake))
                {
                    map[y + 3, x + 3] = 0x0a;

                    int byteToUse2 = (lnI == 0 ? 0xa2e6 : lnI == 1 ? 0xa2ec : lnI == 2 ? 0xa2f2 : lnI == 3 ? 0xa2f5 : 0xa2f8);
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

            int midenMZX = midenX[1] / 8;
            int midenMZY = midenY[1] / 8;

            for (int mzX = 0; mzX < 16; mzX++)
                for (int mzY = 0; mzY < 16; mzY++)
                {
                    if (zone[mzX, mzY] / 1000 == 1)
                    {
                        if (Math.Abs(midenMZX - mzX) == 0 && Math.Abs(midenMZY - mzY) == 0)
                            monsterZones[mzY, mzX] = 0;
                        else if (Math.Abs(midenMZX - mzX) <= 1 && Math.Abs(midenMZY - mzY) <= 1)
                            monsterZones[mzY, mzX] = 2;
                        else if (Math.Abs(midenMZX - mzX) <= 1 || Math.Abs(midenMZY - mzY) <= 1)
                            monsterZones[mzY, mzX] = 1;
                        else if (Math.Abs(midenMZX - mzX) <= 2 || Math.Abs(midenMZY - mzY) <= 2)
                            monsterZones[mzY, mzX] = r1.Next() % 9;
                        else
                            monsterZones[mzY, mzX] = r1.Next() % 18;
                    }
                    else if (zone[mzX, mzY] / 1000 == 2)
                        monsterZones[mzY, mzX] = r1.Next() % 5 + 0x0d;
                    else if (zone[mzX, mzY] / 1000 == 3)
                        monsterZones[mzY, mzX] = r1.Next() % 2 + 0x32;
                    else
                    {
                        while (monsterZones[mzY, mzX] > 0x27 || (monsterZones[mzY, mzX] >= 0x1c && monsterZones[mzY, mzX] <= 0x1f))
                            monsterZones[mzY, mzX] = r1.Next() % 19 + 0x15;
                        if (monsterZones[mzY, mzX] == 0x26) monsterZones[mzY, mzX] = 0x39;
                        if (monsterZones[mzY, mzX] == 0x27) monsterZones[mzY, mzX] = 0x3b;
                    }

                    monsterZones[mzY, mzX] += (64 * (r1.Next() % 4));
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

            // Ensure monster zones are 8x8
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

        private void markZoneSides()
        {
            for (int x = 0; x < 16; x++)
                for (int y = 0; y < 16; y++)
                {
                    // 1 = north, 2 = east, 4 = south, 8 = west
                    if (y == 0) zone[x, y] += 1;
                    else if (zone[x, y - 1] / 1000 != zone[x, y] / 1000) zone[x, y] += 1;

                    if (x == 15) zone[x, y] += 2;
                    else if (zone[x + 1, y] / 1000 != zone[x, y] / 1000) zone[x, y] += 2;

                    if (y == 15) zone[x, y] += 4;
                    else if (zone[x, y + 1] / 1000 != zone[x, y] / 1000) zone[x, y] += 4;

                    if (x == 0) zone[x, y] += 8;
                    else if (zone[x - 1, y] / 1000 != zone[x, y] / 1000) zone[x, y] += 8;
                }
        }

        private void generateZoneMap(int zoneToUse, bool mountains, int islandSize, Random r1)
        {
            if (mountains)
                for (int x = 0; x < 16; x++)
                    for (int y = 0; y < 16; y++)
                        if (zone[x, y] / 1000 == zoneToUse / 1000 && zone[x, y] % 1000 > 0)
                            for (int x2 = x * 8; x2 < (x * 8) + 8; x2++)
                                for (int y2 = y * 8; y2 < (y * 8) + 8; y2++)
                                    map[y2, x2] = 0x05;

            int[] terrainTypes = { 1, 1, 1, 2, 2, 7, 7, 5, 3, 3, 3, 6, 6, 6 };

            for (int lnI = 0; lnI < 100; lnI++)
            {
                int swapper1 = r1.Next() % terrainTypes.Length;
                int swapper2 = r1.Next() % terrainTypes.Length;
                int temp = terrainTypes[swapper1];
                terrainTypes[swapper1] = terrainTypes[swapper2];
                terrainTypes[swapper2] = temp;
            }

            int lnMarker = -1;
            int totalLand = 0;

            while (totalLand < islandSize)
            {
                lnMarker++;
                lnMarker = (lnMarker >= terrainTypes.Length ? 0 : lnMarker);
                int sizeToUse = (r1.Next() % 400) + 150;
                //if (terrainTypes[lnMarker] == 5) sizeToUse /= 2;

                List<int> points = new List<int> { (r1.Next() % 125) + 2, (r1.Next() % 125) + 2 };
                if (validPoint(points[0], points[1], zoneToUse, mountains))
                {
                    while (sizeToUse > 0)
                    {
                        List<int> newPoints = new List<int>();
                        for (int lnI = 0; lnI < points.Count; lnI += 2)
                        {
                            int lnX = points[lnI];
                            int lnY = points[lnI + 1];

                            //if (lnX <= 1 || lnY <= 1 || lnY >= 126 || lnY >= 126) continue;

                            int direction = (r1.Next() % 16);
                            map[lnY, lnX] = terrainTypes[lnMarker];
                            island[lnY, lnX] = zoneToUse;
                            // 1 = North, 2 = east, 4 = south, 8 = west
                            if (direction % 8 >= 4 && lnY <= 125)
                            {
                                if (validPoint(lnX, lnY + 1, zoneToUse, mountains))
                                {
                                    if (map[lnY + 1, lnX] == 4)
                                        totalLand++;
                                    map[lnY + 1, lnX] = terrainTypes[lnMarker];
                                    island[lnY + 1, lnX] = zoneToUse;
                                    newPoints.Add(lnX);
                                    newPoints.Add(lnY + 1);
                                }
                            }
                            if (direction % 2 >= 1 && lnY >= 2)
                            {
                                if (validPoint(lnX, lnY - 1, zoneToUse, mountains))
                                {
                                    if (map[lnY - 1, lnX] == 4)
                                        totalLand++;
                                    map[lnY - 1, lnX] = terrainTypes[lnMarker];
                                    island[lnY - 1, lnX] = zoneToUse;
                                    newPoints.Add(lnX);
                                    newPoints.Add(lnY - 1);
                                }
                            }
                            if (direction % 4 >= 2 && lnX <= 125)
                            {
                                if (validPoint(lnX + 1, lnY, zoneToUse, mountains))
                                {
                                    if (map[lnY, lnX + 1] == 4)
                                        totalLand++;
                                    map[lnY, lnX + 1] = terrainTypes[lnMarker];
                                    island[lnY, lnX + 1] = zoneToUse;
                                    newPoints.Add(lnX + 1);
                                    newPoints.Add(lnY);
                                }
                            }
                            if (direction % 16 >= 8 && lnX >= 2)
                            {
                                if (validPoint(lnX - 1, lnY, zoneToUse, mountains))
                                {
                                    if (map[lnY, lnX - 1] == 4)
                                        totalLand++;
                                    map[lnY, lnX - 1] = terrainTypes[lnMarker];
                                    island[lnY, lnX - 1] = zoneToUse;
                                    newPoints.Add(lnX - 1);
                                    newPoints.Add(lnY);
                                }
                            }

                            int takeaway = 1 + (direction > 8 ? 1 : 0) + (direction % 8 > 4 ? 1 : 0) + (direction % 4 > 2 ? 1 : 0) + (direction % 2 > 1 ? 1 : 0);
                            sizeToUse--;
                        }
                        if (sizeToUse <= 0) break;
                        if (newPoints.Count != 0)
                            points = newPoints;
                    }
                }
            }

            // Fill in water...
            for (int lnY = 0; lnY < 128; lnY++)
                for (int lnX = 0; lnX < 125; lnX++)
                {
                    if (island[lnY, lnX] == zoneToUse && island[lnY, lnX + 1] == zoneToUse && island[lnY, lnX + 2] == zoneToUse && island[lnY, lnX + 3] == zoneToUse)
                    {
                        List<int> land = new List<int> { 1, 2, 3, 5, 6, 7 };
                        if (map[lnY, lnX] == map[lnY, lnX + 2] && map[lnY, lnX] != map[lnY, lnX + 1]) { map[lnY, lnX + 1] = map[lnY, lnX]; island[lnY, lnX + 1] = island[lnY, lnX]; }
                        if (lnX < 124 && land.Contains(map[lnY, lnX]) && !land.Contains(map[lnY, lnX + 1]) && !land.Contains(map[lnY, lnX + 2]) && land.Contains(map[lnY, lnX + 3]))
                        {
                            map[lnY, lnX + 1] = map[lnY, lnX];
                            map[lnY, lnX + 2] = map[lnY, lnX + 3];
                            island[lnY, lnX + 1] = island[lnY, lnX];
                            island[lnY, lnX + 2] = island[lnY, lnX + 3];
                        }
                    }
                }


            markIslands(zoneToUse);
        }

        private bool validPoint(int x, int y, int zoneToUse, bool mountains = false)
        {
            // Establish zone
            int zoneX = x / 8;
            int zoneY = y / 8;
            int zoneSides = zone[zoneX, zoneY] % 1000;
            if (zone[zoneX, zoneY] % 1000 != 0 && mountains) return false;
            if (zone[zoneX, zoneY] / 1000 != zoneToUse / 1000) return false;
            // 1 = north, 2 = east, 4 = south, 8 = west
            if (y % 8 == 0 && zoneSides % 2 == 1) return false;
            if (x % 8 == 7 && zoneSides % 4 >= 2) return false;
            if (y % 8 == 7 && zoneSides % 8 >= 4) return false;
            if (x % 8 == 0 && zoneSides % 16 >= 8) return false;

            return true;
        }

        private void markIslands(int zoneToUse)
        {
            // We should mark islands and inaccessible land...
            int landNumber = zoneToUse + 1;
            int maxLand = -2;

            int maxLandPlots = 0;
            int lastIsland = 0;
            for (int lnI = 0; lnI < 256; lnI++)
                for (int lnJ = 0; lnJ < 256; lnJ++)
                {
                    if (island[lnI, lnJ] == zoneToUse && map[lnI, lnJ] != 0x05)
                    {
                        int plots = landPlot(landNumber, lnI, lnJ, zoneToUse);
                        if (plots > maxLandPlots)
                        {
                            maxLandPlots = plots;
                            maxLand = landNumber;

                        }
                        islands.Add(landNumber);
                        landNumber++;

                        lastIsland = island[lnI, lnJ];
                    }
                }

            maxIsland[zoneToUse / 1000] = maxLand;
        }

        private void resetIslands()
        {
            for (int y = 0; y < 256; y++)
                for (int x = 0; x < 256; x++)
                {
                    if (island[y, x] != 200 && island[y, x] != -1)
                    {
                        island[y, x] /= 1000;
                        island[y, x] *= 1000;
                    }   
                }

            islands.Clear();

            markIslands(3000);
            markIslands(1000);
            markIslands(2000);
            markIslands(0);
        }

        private void createBridges (Random r1)
        {
            List<BridgeList> bridgePossible = new List<BridgeList>();
            List<islandLinks> islandPossible = new List<islandLinks>();
            // Create bridges for points three spaces or less from two distinctly numbered islands.  Extend islands if there is interference.
            for (int y = 1; y < 252; y++)
                for (int x = 1; x < 252; x++)
                {
                    if (y == 78 && x == 3) map[y, x] = map[y, x];
                    if (map[y, x] == 0x05 || map[y, x] == 0x04) continue;

                    for (int lnI = 2; lnI <= 4; lnI++)
                    {
                        if (island[y, x] != island[y + lnI, x] && island[y, x] / 1000 == island[y + lnI, x] / 1000 && map[y + lnI, x] != 0x04 && map[y + lnI, x] != 0x05)
                        {
                            bool fail = false;
                            for (int lnJ = 1; lnJ < lnI; lnJ++)
                            {
                                if (map[y + lnJ, x] != 0x04)
                                {
                                    fail = true;
                                    //map[y + lnJ, x - 1] = 0x04; map[y + lnJ, x + 1] = 0x04;
                                    //island[y + lnJ, x - 1] = 0x04; island[y + lnJ, x + 1] = 0x04;
                                } // else
                                //{
                                //    fail = true;
                                //}
                                //if (map[y + lnJ, x] != 0x04 || map[y + lnJ, x + 1] != 0x04 || map[y + lnJ, x - 1] != 0x04) fail = true;
                            }
                            if (!fail)
                            {
                                bridgePossible.Add(new BridgeList(x, y, true, lnI, island[y, x], island[y + lnI, x]));
                                if (islandPossible.Where(c => c.island1 == island[y, x] && c.island2 == island[y + lnI, x]).Count() == 0)
                                    islandPossible.Add(new islandLinks(island[y, x], island[y + lnI, x]));
                            }
                        }

                        if (island[y, x] != island[y, x + lnI] && island[y, x] / 1000 == island[y, x + lnI] / 1000 && map[y, x + lnI] != 0x04 && map[y, x + lnI] != 0x05)
                        {
                            bool fail = false;
                            for (int lnJ = 1; lnJ < lnI; lnJ++)
                            {
                                if (map[y, x + lnJ] != 0x04)
                                {
                                    fail = true;
                                //    map[y - 1, x + lnJ] = 0x04; map[y + 1, x + lnJ] = 0x04;
                                //    island[y - 1, x + lnJ] = 200; island[y + 1, x + lnJ] = 200;
                                //} else
                                //{
                                //    fail = true;
                                }

                                //if (map[y, x + lnJ] != 0x04 || map[y + 1, x + lnJ] != 0x04 || map[y - 1, x + lnJ] != 0x04) fail = true;
                            }
                            if (!fail)
                            {
                                bridgePossible.Add(new BridgeList(x, y, false, lnI, island[y, x], island[y, x + lnI]));
                                if (islandPossible.Where(c => c.island1 == island[y, x] && c.island2 == island[y, x + lnI]).Count() == 0)
                                    islandPossible.Add(new islandLinks(island[y, x], island[y, x + lnI]));
                            }
                        }
                    }
                }

            foreach (islandLinks islandLink in islandPossible)
            {
                List<BridgeList> test = bridgePossible.Where(c => c.island1 == islandLink.island1 && c.island2 == islandLink.island2).ToList();

                int tries = 50;
                bool pass = false;
                while (!pass && tries > 0)
                {
                    tries--;

                    // Choose one bridge out of the possibilities
                    BridgeList bridgeToBuild = test[r1.Next() % test.Count];
                    // Then confirm that the bridge is still possible...
                    int bridgeTest = map[bridgeToBuild.y, bridgeToBuild.x];
                    int bridgeTest2 = (bridgeToBuild.south ? map[bridgeToBuild.y + bridgeToBuild.distance, bridgeToBuild.x] : map[bridgeToBuild.y, bridgeToBuild.x + bridgeToBuild.distance]);

                    if (bridgeTest == 0x04 || bridgeTest == 0x0d || bridgeTest == 0x09 || bridgeTest2 == 0x04 || bridgeTest2 == 0x0d || bridgeTest2 == 0x09)
                        continue;

                    for (int lnI = 1; lnI <= bridgeToBuild.distance - 1; lnI++)
                    {
                        int bridgeTest3 = (bridgeToBuild.south ? map[bridgeToBuild.y + lnI, bridgeToBuild.x] : map[bridgeToBuild.y, bridgeToBuild.x + lnI]);
                        if (bridgeTest3 != 0x04)
                            continue;
                    }

                    for (int lnI = 1; lnI <= bridgeToBuild.distance - 1; lnI++)
                    {
                        if (bridgeToBuild.south)
                        {
                            map[bridgeToBuild.y + lnI, bridgeToBuild.x - 1] = 0x04; map[bridgeToBuild.y + lnI, bridgeToBuild.x + 1] = 0x04;
                            island[bridgeToBuild.y + lnI, bridgeToBuild.x - 1] = 0x04; island[bridgeToBuild.y + lnI, bridgeToBuild.x + 1] = 0x04;

                            map[bridgeToBuild.y + lnI, bridgeToBuild.x] = 0x0d;
                            island[bridgeToBuild.y + lnI, bridgeToBuild.x] = bridgeToBuild.island1;
                        }
                        else
                        {
                            map[bridgeToBuild.y - 1, bridgeToBuild.x + lnI] = 0x04; map[bridgeToBuild.y + 1, bridgeToBuild.x + lnI] = 0x04;
                            island[bridgeToBuild.y - 1, bridgeToBuild.x + lnI] = 200; island[bridgeToBuild.y + 1, bridgeToBuild.x + lnI] = 200;

                            map[bridgeToBuild.y, bridgeToBuild.x + lnI] = 0x09;
                            island[bridgeToBuild.y, bridgeToBuild.x + lnI] = bridgeToBuild.island1;
                        }
                    }
                    pass = true;
                }
            }
        }

        private class islandLinks
        {
            public int island1;
            public int island2;

            public islandLinks(int pI1, int pI2)
            {
                island1 = pI1; island2 = pI2;
            }
        }

        private class BridgeList {
            public int x;
            public int y;
            public bool south;
            public int distance;
            public int island1;
            public int island2;

            public BridgeList(int pX, int pY, bool pS, int pDist, int pI1, int pI2)
            {
                x = pX; y = pY; south = pS; distance = pDist; island1 = pI1; island2 = pI2;
            }
        }

        private bool createZone(int zoneNumber, int size, bool rectangle, Random r1)
        {
            int tries = 1000;
            bool firstZone = true;

            if (!rectangle)
            {
                while (size > 0 && tries > 0)
                {
                    int x = r1.Next() % 16;
                    int y = r1.Next() % 16;
                    int minX = x, maxX = x, minY = y, maxY = y;
                    if (!firstZone && zone[x, y] != zoneNumber)
                    {
                        continue;
                    }
                    if (firstZone)
                    {
                        firstZone = false;
                        zone[x, y] = zoneNumber;
                    }

                    tries--;
                    int direction = r1.Next() % 16;
                    int totalDirections = 0;
                    if (direction % 16 >= 8) totalDirections++;
                    if (direction % 8 >= 4) totalDirections++;
                    if (direction % 4 >= 2) totalDirections++;
                    if (direction % 2 >= 1) totalDirections++;
                    if (totalDirections > size) continue;

                    // 1 = north, 2 = east, 4 = south, 8 = west
                    if (direction % 16 >= 8 && x != 0 && zone[x - 1, y] == 0 && (minX <= (x - 1) || maxX - minX <= 11))
                    {
                        zone[x - 1, y] = zoneNumber;
                        minX = (x - 1 < minX ? x - 1 : minX);
                        size--;
                        tries = 100;
                    }
                    if (direction % 8 >= 4 && y != 15 && zone[x, y + 1] == 0 && (maxY >= (y + 1) || maxY - minY <= 11))
                    {
                        zone[x, y + 1] = zoneNumber;
                        maxY = (y + 1 > maxY ? y + 1 : maxY);
                        size--;
                        tries = 100;
                    }
                    if (direction % 4 >= 2 && x != 15 && zone[x + 1, y] == 0 && (minX >= (x + 1) || maxX - minX <= 11))
                    {
                        zone[x + 1, y] = zoneNumber;
                        maxX = (x + 1 > maxX ? x + 1 : maxX);
                        size--;
                        tries = 100;
                    }
                    if (direction % 2 >= 1 && y != 0 && zone[x, y - 1] == 0 && (minY <= (y - 1) || maxY - minY <= 11))
                    {
                        zone[x, y - 1] = zoneNumber;
                        minY = (y - 1 < minY ? y - 1 : minY);
                        size--;
                        tries = 100;
                    }
                }
                return (size <= 0);
            } else
            {
                int minMeasurement = (int)Math.Ceiling((double)size / 12);
                int maxMeasurement = (int)Math.Ceiling((double)size / minMeasurement);

                int length = ((r1.Next() % (maxMeasurement - minMeasurement)) + minMeasurement);
                int width = size / length;

                int x = (r1.Next() % (16 - length));
                int y = (r1.Next() % (16 - width));

                for (int i = x; i < x + length; i++)
                    for (int j = y; j < y + width; j++)
                        zone[i, j] = zoneNumber;

                // Snow definition
                romData[0x3e2b6] = (byte)(y * 8);
                romData[0x3e2ba] = (byte)((y + width) * 8);
                romData[0x3e2ac] = (byte)(x * 8);
                romData[0x3e2b0] = (byte)((x + length) * 8);

                // Tantegel definition - TODO:  Find romData location, then change so it's on an 8x8 grid around Tantegel
                //romData[0x3e2b6] = (byte)(y * 8);
                //romData[0x3e2ba] = (byte)((y + width) * 8);
                //romData[0x3e2ac] = (byte)(x * 8);
                //romData[0x3e2b0] = (byte)((x + length) * 8);

                return true;
            }
        }

        private bool reachable(int startY, int startX, bool water, int finishX, int finishY, int maxLake)
        {
            int x = startX;
            int y = startY;

            List<int> validPlots = new List<int> { 0, 1, 2, 3, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 };
            if (water) validPlots.Add(4);

            bool first = true;
            List<int> toPlot = new List<int>();
            bool[,] plotted = new bool[256, 256];

            while (first || toPlot.Count != 0)
            {
                if (!first)
                {
                    y = toPlot[0];
                    toPlot.RemoveAt(0);
                    x = toPlot[0];
                    toPlot.RemoveAt(0);
                }
                else
                {
                    first = false;
                }

                for (int dir = 0; dir < 5; dir++)
                {
                    int dirX = (dir == 4 ? x - 1 : dir == 2 ? x + 1 : x);
                    dirX = (dirX == 256 ? 0 : dirX == -1 ? 255 : dirX);
                    int dirY = (dir == 1 ? y - 1 : dir == 3 ? y + 1 : y);
                    dirY = (dirY == 256 ? 0 : dirY == -1 ? 255 : dirY);

                    if (validPlots.Contains(map[dirY, dirX]) && (map[dirY, dirX] != 4 || island[dirY, dirX] == maxLake))
                    {
                        if (dir != 0 && plotted[dirY, dirX] == false)
                        {
                            if (finishX == dirX && finishY == dirY)
                                return true;
                            toPlot.Add(dirY);
                            toPlot.Add(dirX);
                            plotted[dirY, dirX] = true;
                        }
                    }
                }
            }

            return false;
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
                    //if (lnI == 9 && lnJ == 65) lastIsland = lastIsland;
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
                            //if (lnI == 9 && lnJ == 64) sameTerrain = sameTerrain;
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

            // Adjust monster zones
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

        private int landPlot(int landNumber, int y, int x, int zoneToUse = 0)
        {
            bool first = true;
            List<int> toPlot = new List<int>();
            int plots = 1;
            while (first || toPlot.Count != 0)
            {
                if (!first)
                {
                    y = toPlot[0];
                    toPlot.RemoveAt(0);
                    x = toPlot[0];
                    toPlot.RemoveAt(0);
                }
                else
                {
                    first = false;
                }

                for (int dir = 0; dir < 5; dir++)
                {
                    int dirX = (dir == 4 ? x - 1 : dir == 2 ? x + 1 : x);
                    dirX = (dirX == 256 ? 0 : dirX == -1 ? 255 : dirX);
                    int dirY = (dir == 1 ? y - 1 : dir == 3 ? y + 1 : y);
                    dirY = (dirY == 256 ? 0 : dirY == -1 ? 255 : dirY);

                    if (island[dirY, dirX] == zoneToUse)
                    {
                        plots++;
                        island[dirY, dirX] = landNumber;

                        if (dir != 0)
                        {
                            toPlot.Add(dirY);
                            toPlot.Add(dirX);
                        }
                    }
                }
            }

            return plots;
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
            if (minDirection == 0)
            {
                lnJ = (finalY == 255 ? 0 : finalY + 1);
                while (map[lnJ, finalX] == 0x05)
                {
                    map[lnJ, finalX] = 0x07;
                    lnJ = (lnJ == 255 ? 0 : lnJ + 1);
                }
            }
            else if (minDirection == 1)
            {
                lnJ = (finalY == 0 ? 255 : finalY - 1);
                while (map[lnJ, finalX] == 0x05)
                {
                    map[lnJ, finalX] = 0x07;
                    lnJ = (lnJ == 0 ? 255 : lnJ - 1);
                }
            }
            else if (minDirection == 2)
            {
                lnK = (finalX == 0 ? 255 : finalX - 1);
                while (map[finalY, lnK] == 0x05)
                {
                    map[finalY, lnK] = 0x07;
                    lnK = (lnK == 0 ? 255 : lnK - 1);
                }
            }
            else
            {
                lnK = (finalX == 255 ? 0 : finalX + 1);
                while (map[finalY, lnK] == 0x05)
                {
                    map[finalY, lnK] = 0x07;
                    lnK = (lnK == 255 ? 0 : lnK + 1);
                }
            }
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
                    chkAllDogs.Checked = (reader.ReadLine() == "Y");
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

            // Temporary duplicate key item check removal
            romData[0x19db8] = 0x4c;
            romData[0x19db9] = 0xba;
            romData[0x19dba] = 0x9d;

            renamePrincePrincess();
            if (chkSpeedHacks.Checked)
                speedUpBattles();
            if (chkExperimental.Checked)
                experimentalSpeedHacks();
            skipPrologue();
            reviveAllCharsOnCOD();
            saveRom(true);
            if (chkAllDogs.Checked)
            {
                turnIntoDogs();
                saveRom(false);
            }
            
        }

        private void renamePrincePrincess()
        {
            // Rename the starting characters.
            for (int lnI = 0; lnI < 16; lnI++)
            {
                string name = (lnI < 8 ? txtPrinceName.Text : txtPrincessName.Text);
                int marker = 0;
                for (int lnJ = 0; lnJ < 8; lnJ++)
                {
                    romData[0x1ad49 + (8 * lnI) + lnJ] = 0x5f;
                    try
                    {
                        char character = Convert.ToChar(name.Substring(marker, 1));
                        if (character == 0x7e) // tilde... special character
                        {
                            char specChar1 = Convert.ToChar(name.Substring(marker + 1, 1));
                            char specChar2 = Convert.ToChar(name.Substring(marker + 2, 1));

                            int specChar = 0;

                            if ((specChar1 >= 0x30 && specChar1 <= 0x39) || (specChar1 >= 0x41 && specChar1 <= 0x46) || (specChar1 >= 0x61 && specChar1 <= 0x66))
                            {
                                specChar += 16 * ((specChar1 >= 0x30 && specChar1 <= 0x39) ? specChar1 - 47 : (specChar1 >= 0x41 && specChar1 <= 0x46) ? specChar1 - 55 : specChar1 - 87);
                            } else
                            {
                                marker += 3;
                                continue;
                            }

                            if ((specChar2 >= 0x30 && specChar2 <= 0x39) || (specChar2 >= 0x41 && specChar2 <= 0x46) || (specChar2 >= 0x61 && specChar2 <= 0x66))
                            {
                                specChar += ((specChar2 >= 0x30 && specChar2 <= 0x39) ? specChar2 - 47 : (specChar2 >= 0x41 && specChar2 <= 0x46) ? specChar2 - 55 : specChar2 - 87);
                            }
                            else
                            {
                                marker += 3;
                                continue;
                            }

                            romData[0x1ad49 + (8 * lnI) + lnJ] = (byte)(specChar);

                            marker += 3;
                            continue;
                        }

                        if (character >= 0x30 && character <= 0x39) // 0 to 9... 1-9
                            romData[0x1ad49 + (8 * lnI) + lnJ] = (byte)(character - 47);
                        else if (character >= 0x41 && character <= 0x5a) // A-Z... 36-61
                            romData[0x1ad49 + (8 * lnI) + lnJ] = (byte)(character - 29);
                        else if (character >= 0x61 && character <= 0x7a) // a-z... 10-35
                            romData[0x1ad49 + (8 * lnI) + lnJ] = (byte)(character - 87);
                        else if (character == 0x60)
                            romData[0x1ad49 + (8 * lnI) + lnJ] = 0x61;
                        else if (character == 0x22)
                            romData[0x1ad49 + (8 * lnI) + lnJ] = 0x64;
                        else if (character == 0x27)
                            romData[0x1ad49 + (8 * lnI) + lnJ] = 0x68;
                        else if (character == 0x2c)
                            romData[0x1ad49 + (8 * lnI) + lnJ] = 0x69;
                        else if (character == 0x2d)
                            romData[0x1ad49 + (8 * lnI) + lnJ] = 0x6a;
                        else if (character == 0x2e)
                            romData[0x1ad49 + (8 * lnI) + lnJ] = 0x6b;
                        else if (character == 0x26)
                            romData[0x1ad49 + (8 * lnI) + lnJ] = 0x6c;
                        else if (character == 0x3f)
                            romData[0x1ad49 + (8 * lnI) + lnJ] = 0x6e;
                        else if (character == 0x21)
                            romData[0x1ad49 + (8 * lnI) + lnJ] = 0x6f;
                        else if (character == 0x3b)
                            romData[0x1ad49 + (8 * lnI) + lnJ] = 0x70;
                        else if (character == 0x3a)
                            romData[0x1ad49 + (8 * lnI) + lnJ] = 0x74;
                        marker++;
                    }
                    catch
                    {
                        romData[0x1ad49 + (8 * lnI) + lnJ] = 0x5f; // no more characters to process - make the rest of the characters blank
                        marker++;
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
                    if (rp >= 40) randomPattern = 4;
                    else if (rp >= 30) randomPattern = 2; else randomPattern = 1;
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
                            //else if ((random == 7 || random == 8 || random == 21 || random == 22 || random == 24 || random == 25) && lnI <= 32 && randomLevel == 4)
                            //{
                            //    lnJ--;
                            //    continue;
                            //}
                            //else if ((random == 4 || random == 5 || random == 6 || random == 9 || random == 12 || random == 19 || random == 20 || random == 23 || random == 26) && lnI >= 51 && randomLevel == 4)
                            //{
                            //    lnJ--;
                            //    continue;
                            //}
                            else if (random >= 16)
                                enemyPage2[lnJ] = true;
                            else if (random == 30)
                                concentration = true;

                            enemyPatterns[lnJ] = (byte)(random % 16);
                        } else
                        {

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
                        // The higher the monster is, the more chances of double attack, from 5% to 80%.
                        if (r1.Next() % 100 <= lnI)
                        {
                            enemyPatterns[lnJ] = 13;
                            enemyPage2[lnJ] = true;
                        }
                        else
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
                    }
                } else if (randomPattern == 1)
                {
                    for (int lnJ = 0; lnJ < 8; lnJ++)
                    {
                        // The higher the monster is, the more chances of double attack, from 5% to 80%.
                        if (r1.Next() % 100 <= lnI)
                        {
                            enemyPatterns[lnJ] = 13;
                            enemyPage2[lnJ] = true;
                        }
                        else
                        {
                            enemyPatterns[lnJ] = 0;
                        }
                    }
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
                        romData[byteToUse + lnJ] = (byte)((r1.Next() % 37) + 42);
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
                        romData[byteToUse + lnJ] = (byte)((r1.Next() % 78) + 1);
                    }
                }

                byte specialBout = (byte)(r1.Next() % 20);
                if (((lnI >= 21 && lnI < 42) || lnI > 56))
                {
                    romData[byteToUse + 0] += 0;
                    romData[byteToUse + 1] += (byte)(specialBout >= 16 ? 128 : 0);
                    romData[byteToUse + 2] += (byte)(specialBout % 16 >= 8 ? 128 : 0);
                    romData[byteToUse + 3] += (byte)(specialBout % 8 >= 4 ? 128 : 0);
                    romData[byteToUse + 4] += (byte)(specialBout % 4 >= 2 ? 128 : 0);
                    romData[byteToUse + 5] += (byte)(specialBout % 2 >= 1 ? 128 : 0);
                }
                else
                    for (int lnJ = 0; lnJ < 6; lnJ++)
                        romData[byteToUse + lnJ] += 128;
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
            int[] weaponOrder = { 0, 4, 5, 1, 6, 7, 2, 9, 10, 14, 3, 12, 13, 15, 11 };
            int[] weaponStray = { 8 };
            int[] armorOrder = { 16, 20, 21, 24, 17, 23, 19, 18, 25, 26, 22 };
            int[] shieldOrder = { 27, 29, 28, 31, 30 };
            int[] helmetOrder = { 33, 32, 34 };

            if (randomLevel == 3 || randomLevel == 4)
            {
                for (int lnI = 0; lnI < weaponOrder.Length * 100; lnI++)
                {
                    int first = r1.Next() % weaponOrder.Length;
                    int second = r1.Next() % weaponOrder.Length;
                    int hold = weaponOrder[first];
                    weaponOrder[first] = weaponOrder[second];
                    weaponOrder[second] = hold;
                }

                for (int lnI = 0; lnI < armorOrder.Length * 100; lnI++)
                {
                    int first = r1.Next() % armorOrder.Length;
                    int second = r1.Next() % armorOrder.Length;
                    int hold = armorOrder[first];
                    armorOrder[first] = armorOrder[second];
                    armorOrder[second] = hold;
                }

                for (int lnI = 0; lnI < shieldOrder.Length * 100; lnI++)
                {
                    int first = r1.Next() % shieldOrder.Length;
                    int second = r1.Next() % shieldOrder.Length;
                    int hold = shieldOrder[first];
                    shieldOrder[first] = shieldOrder[second];
                    shieldOrder[second] = hold;
                }

                for (int lnI = 0; lnI < helmetOrder.Length * 100; lnI++)
                {
                    int first = r1.Next() % helmetOrder.Length;
                    int second = r1.Next() % helmetOrder.Length;
                    int hold = helmetOrder[first];
                    helmetOrder[first] = helmetOrder[second];
                    helmetOrder[second] = hold;
                }
            }

            int[] weaponPower = inverted_power_curve(2, 100, weaponOrder.Length, .5, r1);
            int[] weaponStrayPower = inverted_power_curve(2, 100, weaponStray.Length, .5, r1);
            int[] armorPower = inverted_power_curve(2, 70, armorOrder.Length, .5, r1);
            int[] shieldPower = inverted_power_curve(2, 40, shieldOrder.Length, .5, r1);
            int[] helmetPower = inverted_power_curve(2, 30, helmetOrder.Length, .5, r1);

            for (int lnI = 0; lnI < weaponPower.Length; lnI++)
            {
                romData[0x13efb + weaponOrder[lnI]] = (byte)weaponPower[lnI];
                double price = Math.Round(Math.Pow(weaponPower[lnI], 2.3));
                price *= ((string)cboGPReq.SelectedItem == "75%" ? .75 : (string)cboGPReq.SelectedItem == "50%" ? .5 : (string)cboGPReq.SelectedItem == "33%" ? .33 : 1);
                price = (price < 5 ? 5 : price);
                romData[0x1a00e + (weaponOrder[lnI] * 2) + 0] = (byte)(price % 256);
                romData[0x1a00e + (weaponOrder[lnI] * 2) + 1] = (byte)(Math.Floor(price / 256));
                maxPower[0] = (weaponPower[lnI] > maxPower[0] ? weaponPower[lnI] : maxPower[0]);
            }
            for (int lnI = 0; lnI < weaponStrayPower.Length; lnI++)
            {
                romData[0x13efb + weaponStray[lnI]] = (byte)weaponStrayPower[lnI];
                double price = Math.Round(Math.Pow(weaponStrayPower[lnI], 2.35));
                price *= ((string)cboGPReq.SelectedItem == "75%" ? .75 : (string)cboGPReq.SelectedItem == "50%" ? .5 : (string)cboGPReq.SelectedItem == "33%" ? .33 : 1);
                price = (price < 5 ? 5 : price);
                romData[0x1a00e + (weaponStray[lnI] * 2) + 0] = (byte)(price % 256);
                romData[0x1a00e + (weaponStray[lnI] * 2) + 1] = (byte)(Math.Floor(price / 256));
                maxPower[0] = (weaponPower[lnI] > maxPower[0] ? weaponPower[lnI] : maxPower[0]);
            }
            for (int lnI = 0; lnI < armorPower.Length; lnI++)
            {
                romData[0x13efb + armorOrder[lnI]] = (byte)armorPower[lnI];
                double price = Math.Round(Math.Pow(armorPower[lnI], 2.49));
                price *= ((string)cboGPReq.SelectedItem == "75%" ? .75 : (string)cboGPReq.SelectedItem == "50%" ? .5 : (string)cboGPReq.SelectedItem == "33%" ? .33 : 1);
                price = (price < 5 ? 5 : price);
                romData[0x1a00e + (armorOrder[lnI] * 2) + 0] = (byte)(price % 256);
                romData[0x1a00e + (armorOrder[lnI] * 2) + 1] = (byte)(Math.Floor(price / 256));
                maxPower[1] = (armorOrder[lnI] > maxPower[1] ? armorOrder[lnI] : maxPower[1]);
            }
            for (int lnI = 0; lnI < shieldPower.Length; lnI++)
            {
                romData[0x13efb + shieldOrder[lnI]] = (byte)shieldPower[lnI];
                double price = Math.Round(Math.Pow(shieldPower[lnI], 2.84));
                price *= ((string)cboGPReq.SelectedItem == "75%" ? .75 : (string)cboGPReq.SelectedItem == "50%" ? .5 : (string)cboGPReq.SelectedItem == "33%" ? .33 : 1);
                price = (price < 5 ? 5 : price);
                romData[0x1a00e + (shieldOrder[lnI] * 2) + 0] = (byte)(price % 256);
                romData[0x1a00e + (shieldOrder[lnI] * 2) + 1] = (byte)(Math.Floor(price / 256));
                maxPower[2] = (shieldOrder[lnI] > maxPower[2] ? shieldOrder[lnI] : maxPower[2]);
            }
            for (int lnI = 0; lnI < helmetPower.Length; lnI++)
            {
                romData[0x13efb + helmetOrder[lnI]] = (byte)helmetPower[lnI];
                double price = Math.Round(Math.Pow(helmetPower[lnI], 3.07));
                price *= ((string)cboGPReq.SelectedItem == "75%" ? .75 : (string)cboGPReq.SelectedItem == "50%" ? .5 : (string)cboGPReq.SelectedItem == "33%" ? .33 : 1);
                price = (price < 5 ? 5 : price);
                romData[0x1a00e + (helmetOrder[lnI] * 2) + 0] = (byte)(price % 256);
                romData[0x1a00e + (helmetOrder[lnI] * 2) + 1] = (byte)(Math.Floor(price / 256));
                maxPower[3] = (helmetOrder[lnI] > maxPower[3] ? helmetOrder[lnI] : maxPower[3]);
            }

             // Randomize starting equipment. (3c79f-3c7b6)  Target range:  6-24 attack, 4-16 defense.  If it can't be reached, assign lowest weapon and armor.
            // Remember to add 64 to the starting equipment!!!
            List<byte> legalWeapon = new List<byte>();
            List<byte> legalArmor = new List<byte>();
            for (int lnI = 0; lnI < 3; lnI++)
            {
                int byteToUse = 0x3c79f + (8 * lnI);
                // Randomize clothes, club, copper sword / clothes, leather armor
                romData[byteToUse + 0] = (byte)(64 + 1 + weaponOrder[r1.Next() % 3]);
                romData[byteToUse + 1] = (byte)(64 + 1 + armorOrder[r1.Next() % 2]);
                // Need to make sure that everyone is allowed to equip the randomly selected equipment.
                romData[0x1a3ce + weaponOrder[0]] = romData[0x1a3ce + weaponOrder[1]] = romData[0x1a3ce + weaponOrder[2]] = 
                    romData[0x1a3ce + armorOrder[0]] = romData[0x1a3ce + armorOrder[1]] = 7;
            }
        }

        private int[] inverted_power_curve(int min, int max, int arraySize, double powToUse, Random r1)
        {
            int range = max - min;
            double p_range = Math.Pow(range, 1 / powToUse);
            int[] points = new int[arraySize];
            for (int i = 0; i < arraySize; i++)
            {
                double section = (double)r1.Next() / int.MaxValue;
                points[i] = (int)Math.Round(max - Math.Pow(section * p_range, powToUse));
            }
            Array.Sort(points);
            return points;
        }

        private void randomizeWhoEquip(Random r1)
        {
            // Totally randomize who can equip (1a3ce-1a3f0).  At least one person can equip something...
            for (int lnI = 0; lnI < 35; lnI++)
            {
                //if ((lnI == 0 || lnI == 4 || lnI == 5 || lnI == 16 || lnI == 20) && chkEquipment.Checked) continue; // It's already set so everyone can equip, so let's not screw that up.
                romData[0x1a3ce + lnI] = (byte)((r1.Next() % 7) + 1);
                //for (int lnJ = 0; lnJ < 3; lnJ++)
                //{
                //    int byteToUse = 0x3c79f + (8 * lnJ);
                //    if (romData[0x1a3ce + lnI] % Math.Pow(2, lnJ + 1) < Math.Pow(2, lnJ) && (romData[byteToUse + 0] == lnI - 64 || romData[byteToUse + 1] == lnI - 64))
                //        romData[0x1a3ce + lnI] += (byte)Math.Pow(2, lnJ);
                //}
            }
        }

        private void randomizeSpellStrengths(Random r1)
        {
            // Totally randomize spell strengths (18be0, 13be8, 13bf0, 127d5-1286a for strength, 134fa-13508 for cost, 13509-13517 for 3/4 cost)
            byte healScore = (byte)((r1.Next() % 128) + 32);
            byte healMoreScore = (byte)((r1.Next() % 160) + 64);
            byte temp = 0;
            if (healMoreScore < healScore && randomLevel != 4)
            {
                temp = healScore;
                healScore = healMoreScore;
                healMoreScore = temp;
            }
            romData[0x18be0] = romData[0x127fe] = healScore;
            romData[0x18be8] = romData[0x12808] = healMoreScore;

            byte herbScore = (byte)((r1.Next() % 128) + 96);
            romData[0x19602] = romData[0x1285d] = herbScore;
            byte shieldScore = (byte)((r1.Next() % 192) + 32);
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
                if (randomLevel == 4)
                {
                    int target = ((r1.Next() % 3) + 2);
                    romData[byteToUse + 0] = (byte)(target);
                    int cmdTarget = (target <= 3 ? 1 : 0);
                    romData[monsterCmd[lnI]] = (byte)cmdTarget;
                }
                romData[byteToUse + 1] = (byte)((r1.Next() % 208) + 16);
            }

            if (randomLevel != 4)
            {
                for (int lnI = 0; lnI < 4; lnI++)
                    for (int lnJ = lnI + 1; lnJ < 4; lnJ++)
                        if (romData[monsterSpells[lnJ] + 1] < romData[monsterSpells[lnI] + 1])
                        {
                            int tempPower = romData[monsterSpells[lnI] + 1];
                            romData[monsterSpells[lnI] + 1] = romData[monsterSpells[lnJ] + 1];
                            romData[monsterSpells[lnJ] + 1] = (byte)tempPower;
                            lnJ = lnI;
                        }
            }

            if (randomLevel == 4)
            {
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
            // Midenhall = 0x19e41, 0x19e45, 0x19e49, 0x19e4d, 0x19e51, 0x19e55
            // Spring of Bravery = 0x19ed9, 0x19edd, 0x19ee1
            // Cannock = 0x19e59
            // Lake Cave = 0x19e79, 0x19e7d, 0x19e81, 0x19e85, 0x19e89, 0x19e8d, 0x19e91
            // Hamlin = 0x19f26
            // Fire shrine = 0x19f2a
            // Wind Tower = 0x19f0d, 0x19f11, 0x19f15
            // World Map = 0x19f1a, 0x19f1e, 0x19f22
            // Zahan = 0x19f32, 0x19e65
            // Charlock = 0x19eb5, 0x19e69, 0x19e6d, 0x19e71, 0x19e75
            // Osterfair = 0x19e5d, 0x19e61
            // Moon Tower = 0x19ee5, 0x19ee9, 0x19eed, 0x19ef1, 0x19ef5
            // Lighthouse = 0x19ef9, 0x19f01, 0x19f05, 0x19f09
            // Sea Cave = 0x19e95, 0x19e99, 0x19e9d, 0x19ea1, 0x19ea5, 0x19ea9, 0x19ead, 0x19eb1
            // Rhone Cave = 0x19eb9, 0x19ebd, 0x19ec1, 0x19ec5, 0x19ec9, 0x19ecd, 0x19ed1, 0x19ed5
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
                                      0x47, 0x49, 0x4a, 0x4b, 0x4d, 0x4e, 0x4f, 0x50, 0x5d, 0x60, 0x61, 0x62, 0x56 };
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
                // Cloak of wind and Moon fragment are not required in the small map.
                if (chkSmallMap.Checked && chkMap.Checked && (keyItems[lnI] == 0x2e || keyItems[lnI] == 0x26)) continue;
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
                            // Set echoing flute locations if there's a crest involved.  0x40 = Sun(0x199A3-4), 0x43 = Water(0x1999D-E), 0x44 = Life(0x199A9-A)
                            if (keyItems[lnI] == 0x40 || keyItems[lnI] == 0x43 || keyItems[lnI] == 0x44)
                            {
                                int crestByte = (keyItems[lnI] == 0x40 ? 0x199a3 : keyItems[lnI] == 0x43 ? 0x1999d : 0x199a9);
                                if (new List<int> { 0x19eb9, 0x19ebd, 0x19ec1, 0x19ec5, 0x19ec9, 0x19ecd, 0x19ed1, 0x19ed5 }.Contains(allTreasure[tRand]))
                                {
                                    romData[crestByte] = 0x37;
                                    romData[crestByte + 1] = 0x3f + 1;
                                }
                                else if (new List<int> { 0x19e41, 0x19e45, 0x19e49, 0x19e4d, 0x19e51, 0x19e55 }.Contains(allTreasure[tRand]))
                                {
                                    romData[crestByte] = 0x02;
                                    romData[crestByte + 1] = 0x04 + 1;
                                }
                                else if (new List<int> { 0x19ed9, 0x19edd, 0x19ee1 }.Contains(allTreasure[tRand]))
                                {
                                    romData[crestByte] = 0x40;
                                    romData[crestByte + 1] = 0x40 + 1;
                                }
                                else if (new List<int> { 0x19e59 }.Contains(allTreasure[tRand]))
                                {
                                    romData[crestByte] = 0x06;
                                    romData[crestByte + 1] = 0x06 + 1;
                                }
                                else if (new List<int> { 0x19e79, 0x19e7d, 0x19e81, 0x19e85, 0x19e89, 0x19e8d, 0x19e91 }.Contains(allTreasure[tRand]))
                                {
                                    romData[crestByte] = 0x2c;
                                    romData[crestByte + 1] = 0x2d + 1;
                                }
                                else if (new List<int> { 0x19f26 }.Contains(allTreasure[tRand]))
                                {
                                    romData[crestByte] = 0x07;
                                    romData[crestByte + 1] = 0x08 + 1;
                                }
                                else if (new List<int> { 0x19f2a }.Contains(allTreasure[tRand]))
                                {
                                    romData[crestByte] = 0x1e;
                                    romData[crestByte + 1] = 0x1e + 1;
                                }
                                else if (new List<int> { 0x19f0d, 0x19f11, 0x19f15 }.Contains(allTreasure[tRand]))
                                {
                                    romData[crestByte] = 0x58;
                                    romData[crestByte + 1] = 0x5f + 1;
                                }
                                else if (new List<int> { 0x19f1a, 0x19f1e, 0x19f22 }.Contains(allTreasure[tRand]))
                                {
                                    romData[crestByte] = 0x01;
                                    romData[crestByte + 1] = 0x01 + 1;
                                }
                                else if (new List<int> { 0x19f32, 0x19e65 }.Contains(allTreasure[tRand]))
                                {
                                    romData[crestByte] = 0x10;
                                    romData[crestByte + 1] = 0x10 + 1;
                                }
                                else if (new List<int> { 0x19eb5, 0x19e69, 0x19e6d, 0x19e71, 0x19e75 }.Contains(allTreasure[tRand]))
                                {
                                    romData[crestByte] = 0x34;
                                    romData[crestByte + 1] = 0x36 + 1;
                                }
                                else if (new List<int> { 0x19e5d, 0x19e61 }.Contains(allTreasure[tRand]))
                                {
                                    romData[crestByte] = 0x0f;
                                    romData[crestByte + 1] = 0x0f + 1;
                                }
                                else if (new List<int> { 0x19ee5, 0x19ee9, 0x19eed, 0x19ef1, 0x19ef5 }.Contains(allTreasure[tRand]))
                                {
                                    romData[crestByte] = 0x49;
                                    romData[crestByte + 1] = 0x4f + 1;
                                }
                                else if (new List<int> { 0x19ef9, 0x19f01, 0x19f05, 0x19f09 }.Contains(allTreasure[tRand]))
                                {
                                    romData[crestByte] = 0x50;
                                    romData[crestByte + 1] = 0x57 + 1;
                                }
                                else if (new List<int> { 0x19e95, 0x19e99, 0x19e9d, 0x19ea1, 0x19ea5, 0x19ea9, 0x19ead, 0x19eb1 }.Contains(allTreasure[tRand]))
                                {
                                    romData[crestByte] = 0x2e;
                                    romData[crestByte + 1] = 0x33 + 1;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void randomizeStores(Random r1)
        {
            if (!chkEquipment.Checked)
            {
                byte[] weapons = new byte[] { 2, 12, 27, 45, 8, 10, 15, 20, 7, 30, 40, 105, 55, 70, 40, 95 };
                int[] weaponcost = new int[] { 20, 200, 2500, 26000, 60, 100, 330, 770, 25000, 1500, 4000, 15000, 8000, 16000, 4000, 30000 };
                byte[] armor = new byte[] { 2, 35, 65, 60, 6, 12, 87, 35, 25, 47, 75 };
                int[] armorcost = new int[] { 30, 1250, 24000, 12000, 150, 390, 6400, 1250, 1000, 10000, 30000 };
                byte[] shields = new byte[] { 4, 18, 10, 40, 30 };
                int[] shieldcost = new int[] { 90, 21500, 2000, 8800, 30000 };
                byte[] helmets = new byte[] { 8, 6, 20 };
                int[] helmetcost = new int[] { 20000, 3150, 20000 };

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

                bool special = false;

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
            }

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
                        randomModifier = 4 + (r1.Next() % 16);
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

            if (randomLevel == 3 || randomLevel == 4)
            {
                for (int lnI = 0; lnI < 12; lnI++)
                {
                    if (lnI == 3) continue;
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

                        maxGains = maxStrInd; // - romData[0x13dd1 + lnI];
                    }
                    else maxGains = 255; // - romData[0x13dd1 + lnI];

                    if (randomLevel == 3)
                    {
                        if (lnI % 4 == 0)
                            maxGains = (lnI == 0 ? maxGains : lnI == 4 ? maxGains - 30 : maxGains - 70);
                        if (lnI % 4 == 1)
                            maxGains = (lnI == 1 ? maxGains - 80 : lnI == 5 ? maxGains - 40 : maxGains);
                        if (lnI % 4 == 2)
                            maxGains = (lnI == 2 ? 250 : lnI == 6 ? 210 : 170);
                        if (lnI % 4 == 3)
                            maxGains = (lnI == 7 ? 200 : 250);
                    }
                    else
                    {
                        if (lnI % 4 == 0) maxGains = (r1.Next() % (maxGains - 70)) + 70;
                        if (lnI % 4 == 1) maxGains = (r1.Next() % (maxGains - 120)) + 120;
                        if (lnI % 4 == 2) maxGains = (r1.Next() % (maxGains - 140)) + 140;
                        if (lnI % 4 == 3) maxGains = (r1.Next() % (maxGains - 140)) + 140;
                    }
                    //if (lnI == 3) maxGains = 0; // No MP for Midenhall

                    int arraySize = lnI < 4 ? 50 : lnI < 8 ? 45 : 35;
                    int[] values = inverted_power_curve(romData[0x13dd1 + lnI], maxGains, arraySize, 1.3, r1);
                    
                    //for (int lnJ = 0; lnJ < arraySize; lnJ++)
                    //    values[lnJ] = romData[0x13dd1 + lnI] + (r1.Next() % maxGains);

                    //Array.Sort(values);
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
        }

        private void experimentalSpeedHacks()
        {
            romData[0x3fdac] = 0xea;
            romData[0x3fdad] = 0xea;
            romData[0x3fdae] = 0xea;

            romData[0x3c20e] = 0xea;
            romData[0x3c20f] = 0xea;
        }

        private void speedUpBattles()
        {
            // Greatly increased battle speeds.
            romData [0x1adcf] = 0x01;
            romData [0x1add0] = 0x28;
            romData [0x1add1] = 0x40;
            // All ROM hacks will reduce shaking from taking damage, speeding up battles even further.
            romData[0x11038] = 2; // instead of 11
            romData[0x10ae9] = 1; // instead of 4, greatly reducing enemy flashing on them taking damage, reducing about 12 frames each time.
            romData[0x3c526] = 1; // instead of 10, greatly reducing flashes done for spell casting, removing 20 frames every time a spell is cast.
            romData[0x3fc49] = 1; // instead of 8, reducing transition from one character's move to another by 7 frames / transition.
            romData[0x110cc] = 1; // instead of 8, reducing flashing of super monsters (Atlas, Bazuzu, etc.)
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

        private void saveRom(bool calcChecksum)
        {
            //string options = (chkChangeStatsToRemix.Checked ? "r" : "");
            //options += (chkHalfExpGoldReq.Checked ? "h" : "");
            //options += (chkDoubleXP.Checked ? "d" : "");
            //options += (radSlightIntensity.Checked ? "l1" : radModerateIntensity.Checked ? "l2" : radHeavyIntensity.Checked ? "l3" : "l4");
            string finalFile = Path.Combine(Path.GetDirectoryName(txtFileName.Text), "DW2Random_" + txtSeed.Text + "_" + txtFlags.Text + ".nes");
            File.WriteAllBytes(finalFile, romData);
            lblIntensityDesc.Text = "ROM hacking complete!  (" + finalFile + ")";
            txtCompare.Text = finalFile;

            if (calcChecksum)
            {
                try
                {
                    using (var md5 = SHA1.Create())
                    {
                        using (var stream = File.OpenRead(finalFile))
                        {
                            lblNewChecksum.Text = BitConverter.ToString(md5.ComputeHash(stream)).ToLower().Replace("-", "");
                        }
                    }
                }
                catch
                {
                    lblSHAChecksum.Text = "????????????????????????????????????????";
                }
            }

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

            bool special = false;

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
                    goodMap = (chkSmallMap.Checked ? randomizeMapv5(r1) : randomizeMapv2(r1));
            }

            if (chkWhoCanEquip.Checked)
                randomizeWhoEquip(r1);
            if (chkEquipment.Checked)
                randomizeEquipment(r1);
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

            saveRom(true);
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
                    writer.WriteLine(chkAllDogs.Checked ? "Y" : "N");
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
            chkSpeedHacks.Checked = (txtFlags.Text.Contains("A"));
            chkExperimental.Checked = (txtFlags.Text.Contains("a"));

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
            if (chkSpeedHacks.Checked)
                flags += "A";
            if (chkExperimental.Checked)
                flags += "a";

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
            chkSmallMap.Checked = true;
            chkEquipment.Checked = true;
            chkEquipEffects.Checked = false;
            chkWhoCanEquip.Checked = true;
            chkMonsterStats.Checked = true;
            chkMonsterZones.Checked = true;
            chkSpellLearning.Checked = true;
            chkSpellStrengths.Checked = true;
            chkHeroStats.Checked = true;
            chkHeroStores.Checked = true;
            chkTreasures.Checked = true;
            chkXPRandomize.Checked = false;
            chkGPRandomize.Checked = false;

            radSlightIntensity.Checked = false;
            radModerateIntensity.Checked = false;
            radHeavyIntensity.Checked = true;
            radInsaneIntensity.Checked = false;

            chkChangeStatsToRemix.Checked = true;
            cboEncounterRate.SelectedIndex = 6;
            cboGPReq.SelectedIndex = 3;
            cboXPReq.SelectedIndex = 3;
        }

        private void btnCopyChecksum_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(lblNewChecksum.Text);
        }
    }
}
