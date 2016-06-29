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

        public Form1()
        {
            InitializeComponent();
        }

        private void randomizeMap(Random r1)
        {
            // We need to make four islands.  One to get the prince, one to get the princess, one for the approach to Lianport, one for Lianport itself, 
            // one for the "mainland", and one for "Hargon's Castle".
            int[,] map = new int[256,256];
            Array.Clear(map, 0, map.Length);
            int[,] islandSize = new int[6,2] { { 0, 0 }, 
                { 0, 0 }, 
                { 0, 0 }, 
                { 0, 0 }, 
                { 0, 0 }, 
                { 0, 0 } };

            for (int lnI = 0; lnI < 6; lnI++)
            {
                int minArea = (lnI == 0 ? 1500 : lnI == 1 ? 2000 : lnI == 2 ? 2500 : lnI == 3 ? 1000 : lnI == 4 ? 4000 : 3000);
                int maxArea = (lnI == 0 ? 3500 : lnI == 1 ? 4000 : lnI == 2 ? 5000 : lnI == 3 ? 2000 : lnI == 4 ? 10000 : 6000);
                int minLength = (lnI == 0 ? 5 : lnI == 1 ? 8 : lnI == 2 ? 10 : lnI == 3 ? 20 : lnI == 4 ? 40 : 30);
                int maxLength = (lnI == 0 ? 100 : lnI == 1 ? 100 : lnI == 2 ? 100 : lnI == 3 ? 100 : lnI == 4 ? 150 : 150);
                islandSize[lnI, 0] = minLength + (r1.Next() % (maxLength - minLength));
                islandSize[lnI, 1] = minLength + (r1.Next() % (maxLength - minLength));

                // pick island starting spot randomly.
                int startIsland1 = r1.Next() % (256 - islandSize[lnI, 0]);
                int startIsland2 = r1.Next() % (256 - islandSize[lnI, 0]);
                // Make sure it doesn't run into another island...

                for (int lnJ = 0; lnJ < islandSize[lnI, 0]; lnJ++)
                {
                    for (int lnK = 0; lnK < islandSize[lnI, 1]; lnK++)
                    {
                        int x = startIsland1 + lnJ - (startIsland1 + lnJ >= 256 ? 256 : 0);
                        map[lnJ, lnK] = 0x20;
                    }
                }
            }
            // Need to come up with a sea cave location
            // Need to place a Rhone Cave location
            // Need to place 6+6+8+13+7=40 locations.
            // MUST place Midenhall, Cannock, and Leftwyne, and the first transition monolith on island 1 for now.  Must NOT place any other transition place on island 1.
            // MUST place the second transition monolith on island 2.  Must NOT place any other transition place on island 2.
            // MUST place Lianport on island 3, next to a sea.  However, any transition place can be placed on island 3.
            // We also need to figure out three hidden locations and mark them appropriately.  The treasures spot, the mirror of ra spot, and the world tree.

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
                    // flagLoad(); <---- This gets called via the previous line.
                }
            }
            catch
            {
                // ignore error
            } finally
            {
                loading = false;}
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

            speedUpBattles();
            skipPrologue();
            reviveAllCharsOnCOD();
            saveRom();
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

                // evade rate...
                if (randomLevel == 4)
                    enemyStats[1] = (byte)((r1.Next() % 16) * 16);
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
                    xp = adjustEnemyStat(r1, xp, 1);

                // Agility
                if (randomLevel == 4)
                    enemyStats[4] = (byte)(r1.Next() % 256);
                else
                {
                    enemyStats[4] = adjustEnemyStat(r1, enemyStats[4], 1);
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
                    res1 = adjustEnemyStat(r1, res1, 2);
                    res2 = adjustEnemyStat(r1, res2, 2);
                    res3 = adjustEnemyStat(r1, res3, 2);
                    res4 = adjustEnemyStat(r1, res4, 2);
                    res5 = adjustEnemyStat(r1, res5, 2);
                    res6 = adjustEnemyStat(r1, res6, 2);
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
                } else if (randomLevel == 3)
                {
                    int rp = (r1.Next() % 4);
                    if (rp == 0 || rp == 1) randomPattern = 4;
                    if (rp == 2) randomPattern = 3; else randomPattern = 2;
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
                            else if ((random == 7 || random == 8 || random == 21 || random == 22 || random == 24 || random == 25) && lnI <= 32)
                            {
                                lnJ--;
                                continue;
                            }
                            else if ((random == 4 || random == 5 || random == 6 || random == 9 || random == 12 || random == 19 || random == 20 || random == 23 || random == 26) && lnI >= 51)
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
                    enemyStats[10] = (byte)(enemyPatterns[0] + (enemyPatterns[1] * 16));
                    enemyStats[11] = (byte)(enemyPatterns[2] + (enemyPatterns[3] * 16));
                    enemyStats[12] = (byte)(enemyPatterns[4] + (enemyPatterns[5] * 16));
                    enemyStats[13] = (byte)(enemyPatterns[6] + (enemyPatterns[7] * 16));
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

        private byte adjustEnemyStat(Random r1, int origStat, int adjLevel)
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
            finalStat = (finalStat < 0 ? 0 : finalStat > 255 ? 255 : finalStat);
            return (byte)finalStat;
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
                price = (float)Math.Round(price, 0);

                if ((string)cboGPReq.SelectedValue == "100%")
                    price *= 2;
                else if ((string)cboGPReq.SelectedValue == "75%")
                    price *= 1.5;
                else if ((string)cboGPReq.SelectedValue == "33%")
                    price *= .66667;

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
                    level = (byte)((r1.Next() % 26) + 2);
                else
                    level = (byte)((r1.Next() % 23) + 2);

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

                    int adder = (lnI / 2);
                    for (int lnJ = 0; lnJ < arraySize - 1; lnJ++)
                    {
                        int byteToUse = 0x13ddd + adder;
                        int valueToAdd = values[lnJ + 1] - values[lnJ];
                        if (lnI == 3) valueToAdd = 0;
                        if (lnI % 2 == 0)
                            romData[byteToUse] = (byte)((romData[byteToUse] % 16) + (valueToAdd * 16));
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

                    if (randomLevel == 1)
                    {
                        randomModifier1 += (r1.Next() % 3) - 1;
                        randomModifier2 += (r1.Next() % 3) - 1;
                    } else if (randomLevel == 2)
                    {
                        randomModifier1 += (r1.Next() % 5) - 2;
                        randomModifier2 += (r1.Next() % 5) - 2;
                    }
                    else if (randomLevel == 3)
                    {
                        randomModifier1 += (r1.Next() % 7) - 3;
                        randomModifier2 += (r1.Next() % 7) - 3;
                    }
                    if (lnI % 2 == 0)
                    {
                        if (stats[statToUse1] + randomModifier1 > maxStrength)
                            randomModifier1 = 0;
                        if (stats[statToUse2] + randomModifier2 > maxAgility)
                            randomModifier2 = 0;
                    } else
                    {
                        if (stats[statToUse1] + randomModifier1 > 255)
                            randomModifier1 = 0;
                        if (stats[statToUse2] + randomModifier2 > 255)
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
            //string options = (chkChangeStatsToRemix.Checked ? "_r" : "_");
            //options += (chkHalfExpGoldReq.Checked ? "h" : "");
            //options += (chkDoubleXP.Checked ? "d" : "");
            //options += (radSlightIntensity.Checked ? "_l1" : radModerateIntensity.Checked ? "_l2" : radHeavyIntensity.Checked ? "_l3" : "_l4");
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
                {
                    { 5, 7, 5, 2, 1, 2 }, // Slime
                    { 8, 9, 6, 3, 2, 3 }, // Big Slug
                    { 5, 11, 13, 4, 2, 4 }, // Iron Ant
                    { 9, 12, 8, 5, 3, 3 }, // Drakee
                    { 10, 14, 11, 8, 5, 5 }, // Wild Mouse
                    { 25, 15, 10, 20, 15, 5 }, // Healer
                    { 12, 18, 10, 8, 6, 6 }, // Ghost Mouse
                    { 13, 16, 13, 9, 8, 4 }, // Babble
                    { 12, 19, 13, 8, 6, 7 }, // Army Ant (deliberatly changed from 4XP/2GP to 6XP/7GP to make it more in line with other monsters in its class)
                    { 15, 17, 11, 11, 10, 10 }, // Magician
                    { 16, 19, 11, 15, 7, 5 }, // Big Rat
                    { 14, 22, 10, 11, 9, 9 }, // Big Cobra
                    { 14, 18, 13, 18, 18, 8 }, // Magic Ant
                    { 12, 14, 10, 14, 12, 10 }, // Magidrakee
                    { 21, 25, 40, 13, 14, 30 }, // Centipod
                    { 20, 28, 16, 12, 25, 50 }, // Man O' War
                    { 15, 20, 10, 16, 27, 20 }, // Lizard Fly
                    { 60, 25, 7, 12, 40, 25 }, // Zombie
                    { 15, 14, 40, 15, 18, 40 }, // Smoke
                    { 25, 35, 12, 25, 23, 25 }, // Ghost Rat
                    { 35, 40, 12, 18, 33, 45 }, // Baboon
                    { 32, 32, 11, 12, 29, 31 }, // Carnivog
                    { 20, 39, 110, 13, 33, 25 }, // Megapede
                    { 32, 38, 11, 16, 34, 80 }, // Sea Slug
                    { 42, 35, 13, 22, 36, 29 }, // Medusa Ball
                    { 40, 36, 14, 25, 37, 30 }, // Enchanter
                    { 28, 30, 9, 22, 32, 35 }, // Mud Man
                    { 38, 45, 12, 18, 40, 45 }, // Magic Baboon
                    { 40, 51, 16, 31, 44, 50 }, // Demighost
                    { 60, 57, 20, 30, 52, 47 }, // Gremlin
                    { 46, 45, 18, 23, 31, 25 }, // Poison Lily
                    { 0, 0, 0, 0, 0, 0 }, // Mummy Man
                    { 26, 30, 99, 30, 50, 62 }, // Gorgon
                    { 25, 70, 20, 42, 45, 55 }, // Saber Tiger
                    { 40, 51, 21, 30, 59, 43 }, // Dragonfly
                    { 51, 58, 19, 30, 50, 80 }, // Titan Tree
                    { 65, 63, 17, 33, 45, 82 }, // Undead
                    { 38, 75, 25, 41, 41, 58 }, // Basilisk
                    { 50, 55, 16, 39, 29, 42 }, // Goopi
                    { 60, 75, 23, 36, 61, 50 }, // Orc
                    { 60, 64, 24, 70, 52, 100 }, // Puppet Man
                    { 0, 0, 0, 0, 0, 0 }, // Mummy
                    { 63, 72, 27, 38, 67, 95 }, // Evil Tree
                    { 50, 60, 80, 45, 39, 62 }, // Gas
                    { 90, 51, 2, 20, 61, 51 }, // Hork
                    { 60, 75, 27, 41, 64, 45 }, // Hawk Man
                    { 55, 61, 28, 43, 72, 110 }, // Sorcerer
                    { 5, 37, 255, 100, 1015, 90 }, // Metal Slime
                    { 65, 82, 25, 57, 77, 97 }, // Hunter
                    { 50, 67, 30, 45, 92, 88 }, // Evil Eye
                    { 60, 74, 29, 52, 81, 83 }, // Hibabango
                    { 60, 65, 24, 49, 48, 30 }, // Graboopi
                    { 100, 80, 56, 57, 83, 255 }, // Gold Orc
                    { 67, 73, 28, 75, 132, 10 }, // Evil Clown
                    { 80, 103, 19, 21, 91, 100 }, // Ghoul
                    { 57, 75, 25, 48, 95, 83 }, // Vampirus
                    { 72, 83, 28, 53, 115, 80 }, // Mega Knight
                    { 80, 95, 76, 71, 128, 55 }, // Saber Lion
                    { 70, 55, 95, 61, 125, 150 }, // Metal Hunter
                    { 69, 80, 41, 57, 159, 121 }, // Oswarg
                    { 67, 74, 22, 55, 118, 81 }, // Dark Eye
                    { 60, 85, 51, 64, 107, 95 }, // Gargoyle
                    { 110, 99, 80, 60, 204, 181 }, // Orc King
                    { 82, 77, 47, 79, 182, 103 }, // Magic Vampirus
                    { 78, 109, 63, 55, 147, 123 }, // Berzerker
                    { 5, 75, 255, 200, 10150, 255 }, // Metal Babble
                    { 77, 115, 72, 65, 201, 135 }, // Hargon's Knight
                    { 115, 121, 42, 43, 257, 99 }, // Cyclops
                    { 90 , 115, 150, 80, 554, 120 }, // Attackbot
                    { 90, 120, 56, 62, 480, 147 }, // Green Dragon
                    { 180, 110, 70, 120, 734, 170 }, // Mace Master
                    { 65, 85, 54, 68, 315, 101 }, // Flame
                    { 89, 102, 69, 83, 321, 96 }, // Silver Batboon
                    { 92, 95, 73, 85, 412, 113 }, // Blizzard
                    { 175, 150, 51, 88, 580, 165 }, // Giant
                    { 138, 118, 110, 85, 542, 100 }, // Gold Batboon
                    { 230, 140, 135, 105, 1475, 235 }, // Bullwong
                    { 250, 195, 160, 85, 2500, 250 }, // Atlas
                    { 250, 127, 170, 75, 3350, 240 }, // Bazuzu
                    { 320, 176, 180, 120, 4750, 255 }, // Zarlox
                    { 460, 177, 165, 150, 0, 0 }, // Hargon
                    { 0, 0, 0, 0, 0, 0 } // Malroth
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

                int byteValStart = 0x13805 + (15 * lnI);
                romData[byteValStart + 0] = hp;
                romData[byteValStart + 5] = atk;
                romData[byteValStart + 6] = def;
                romData[byteValStart + 4] = agi;
                romData[byteValStart + 3] = xp1;
                romData[byteValStart + 8] = (byte)((romData[byteValStart + 8] % 64) + xp2);
                romData[byteValStart + 9] = (byte)((romData[byteValStart + 9] % 64) + xp3);
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
                randomizeMap(r1);

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
                compareComposeString("treasuresMiden", writer, 0x19e41, 21, 4);
                compareComposeString("treasuresCannock", writer, 0x19e59, 5, 4);
                compareComposeString("treasuresOsterfair", writer, 0x19e5d, 9, 4);
                compareComposeString("treasuresZahan", writer, 0x19e65, 5, 4);
                compareComposeString("treasuresCharlock1", writer, 0x19eb5, 5, 4);
                compareComposeString("treasuresCharlock2", writer, 0x19e69, 17, 4);
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

            if (txtFlags.Text.Contains("_r1")) radSlightIntensity.Checked = true;
            if (txtFlags.Text.Contains("_r2")) radModerateIntensity.Checked = true;
            if (txtFlags.Text.Contains("_r3")) radHeavyIntensity.Checked = true;
            if (txtFlags.Text.Contains("_r4")) radInsaneIntensity.Checked = true;
            cboGPReq.SelectedItem = (txtFlags.Text.Contains("_g1") ? "75%" : txtFlags.Text.Contains("_g2") ? "50%" : txtFlags.Text.Contains("_g3") ? "33%" : "100%");
            cboXPReq.SelectedItem = (txtFlags.Text.Contains("_x1") ? "75%" : txtFlags.Text.Contains("_x2") ? "50%" : txtFlags.Text.Contains("_x3") ? "33%" : "100%");
            cboEncounterRate.SelectedItem = (txtFlags.Text.Contains("_e1") ? "300%" : txtFlags.Text.Contains("_e2") ? "200%" : txtFlags.Text.Contains("_e3") ? "150%" : 
                txtFlags.Text.Contains("_e4") ? "75%" : txtFlags.Text.Contains("_e5") ? "50%" : txtFlags.Text.Contains("_e6") ? "33%" : txtFlags.Text.Contains("_e7") ? "25%" : "100%");
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

            flags += (radSlightIntensity.Checked ? "_r1" : radModerateIntensity.Checked ? "_r2" : radHeavyIntensity.Checked ? "_r3" : "_r4");
            flags += ((string)cboGPReq.SelectedItem == "75%" ? "_g1" : (string)cboGPReq.SelectedItem == "50%" ? "_g2" : (string)cboGPReq.SelectedItem == "33%" ? "_g3" : "");
            flags += ((string)cboXPReq.SelectedItem == "75%" ? "_x1" : (string)cboXPReq.SelectedItem == "50%" ? "_x2" : (string)cboXPReq.SelectedItem == "33%" ? "_x3" : "");
            flags += ((string)cboEncounterRate.SelectedItem == "300%" ? "_e1" : (string)cboEncounterRate.SelectedItem == "200%" ? "_e2" : (string)cboEncounterRate.SelectedItem == "150%" ? "_e3" : 
                (string)cboEncounterRate.SelectedItem == "75%" ? "_e4" : (string)cboEncounterRate.SelectedItem == "50%" ? "_e5" : (string)cboEncounterRate.SelectedItem == "33%" ? "_e6" : 
                (string)cboEncounterRate.SelectedItem == "25%" ? "_e7" : "");
            txtFlags.Text = flags;
        }

        private void determineFlags(object sender, EventArgs e)
        {
            determineFlag();
        }
    }
}
