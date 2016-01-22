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

        public Form1()
        {
            InitializeComponent();
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
            using (var md5 = SHA1.Create())
            {
                using (var stream = File.OpenRead(txtFileName.Text))
                {
                    lblSHAChecksum.Text = BitConverter.ToString(md5.ComputeHash(stream)).ToLower().Replace("-", "");
                }
            }
        }

        private void radSlightIntensity_CheckedChanged(object sender, EventArgs e)
        {
            if (radSlightIntensity.Checked)
                lblIntensityDesc.Text = "Small changes to monster zones and boss fights will occur.  Expect the same final three bosses, " +
                    "and slight changes of difficulty to other boss fights.  No changes to treasures or shops will occur, and the stats for all three party members will remain the same.";
        }

        private void radModerateIntensity_CheckedChanged(object sender, EventArgs e)
        {
            if (radModerateIntensity.Checked)
                lblIntensityDesc.Text = "Moderate changes to monster zones and boss fights will occur.  Expect the same final three bosses, " +
                    "but significant changes to the other boss fights.  Substantial shop changes to treasures and shops will occur, but all of them will stay in their respective zones.  " +
                    "Finally, expect slight stat modifications to all three party members.(+/- 5 HP/MP, 2 Str/Agi to start, +/- 1 point to all for stat ups, +/- 3 levels for spell learning)";

        }

        private void radHeavyIntensity_CheckedChanged(object sender, EventArgs e)
        {
            if (radHeavyIntensity.Checked)
                lblIntensityDesc.Text = "Major changes to monster zones and boss fights will occur.  Expect some differences with the final three bosses, " +
                    "and big changes to the other boss fights.  Expect major shop changes and treasure scrambling(key items will stay in their respective zones), but you will still be able to buy anything " +
                    "that you would normally buy at a shop and find that you normally would find in a treasure box.  Finally, expect significant stat changes to all three party members.  " + 
                    "(+/- 15 HP/MP, 5 Str/Agi to start, +/- 3 points to all for level ups, +/- 10 levels for spell learning, but no higher than level 30(Cannock)/25(Moonbrooke))";

        }

        private void radInsaneIntensity_CheckedChanged(object sender, EventArgs e)
        {
            if (radInsaneIntensity.Checked)
                lblIntensityDesc.Text = "The ultimate randomization!  Complete changes to monsters, including all stats and abilities, with a recalculation of experience according to difficulty, " +
                    "complete changes to all items and where they reside, treasure and/or store(key items will stay in their respective zones), with prices recalculated " +
                    "according to power and ability, and complete randomization to all spells, when they are learned(but no higher than level 30(Cannock)/25(Moonbrooke), and all statistics.(attack/defense overflow with non-cursed items will be avoided)";

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                using (TextReader reader = File.OpenText("lastFile.txt"))
                {
                    txtFileName.Text = reader.ReadLine();
                    runChecksum();
                }
            }
            catch
            {
                // ignore error
            }

            radSlightIntensity_CheckedChanged(null, null);
        }

        private void btnRandomize_Click(object sender, EventArgs e)
        {
            loadRom();
            if (chkChangeStatsToRemix.Checked && !radInsaneIntensity.Checked) changeStatsToRemix(); // Don't bother if insane random is checked, since all of the stats will change anyway!
            if (chkHalfExpGoldReq.Checked) halfExpAndGoldReq();
            randomize();

            saveRom();
        }

        private void loadRom(bool extra = false)
        {
            romData = File.ReadAllBytes(txtFileName.Text);
            if (extra)
                romData2 = File.ReadAllBytes(Path.Combine(Path.GetDirectoryName(txtFileName.Text), "DW2Rando.nes"));
        }

        private void saveRom()
        {
            string finalFile = Path.Combine(Path.GetDirectoryName(txtFileName.Text), "DW2Rando.nes");
            File.WriteAllBytes(finalFile, romData);
            lblIntensityDesc.Text = "ROM hacking complete!  (" + finalFile + ")";
        }

        private void changeStatsToRemix()
        {
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
                    { 12, 19, 13, 8, 4, 2 }, // Army Ant
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
                    { 50, 75, 255, 200, 10150, 255 }, // Metal Babble
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
        }

        private void halfExpAndGoldReq(bool special = false)
        {
            // We'll divide all of these by two later...
            int[] weaponcost = new int[] { 20, 200, 2500, 26000, 60, 100, 330, 770, 25000, 1500, 4000, 15000, 8000, 16000, 4000, 500 };
            int[] armorcost = new int[] { 30, 1250, 70, 32767, 150, 390, 6400, 1250, 1000, 50, 50 };
            int[] shieldcost = new int[] { 90, 21500, 2000, 8800, 90 };
            int[] helmetcost = new int[] { 20000, 3150, 20 };
            int[] itemcost = new int[] { 10, 0, 300, 0, 0, 6, 400, 0, 40, 30, 70, 1500, 640, 10000, 500, 70, 40, 80, 2, 2, 2, 2000, 0, 8, 15, 2600, 2, 2 };

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
                30000, 30000, 40000, 50000, 24464, 24464, 34464, 24464, 24464, 24464 }; // last six levels are forced to be at least 65,535 points.

            for (int lnI = 0; lnI < 49; lnI++)
            {
                romData[0x13cd3 + (lnI * 2)] = (byte)(special ? 1 : (midenhallExpReq[lnI] / 2) % 256);
                romData[0x13cd4 + (lnI * 2)] = (byte)(special ? 0 : (midenhallExpReq[lnI] / 2) / 256);
                if (lnI < 44)
                {
                    romData[0x13d35 + (lnI * 2)] = (byte)(special ? 1 : (cannockExpReq[lnI] / 2) % 256);
                    romData[0x13d36 + (lnI * 2)] = (byte)(special ? 0 : (cannockExpReq[lnI] / 2) / 256);
                }
                if (lnI < 34)
                {
                    romData[0x13d8d + (lnI * 2)] = (byte)(special || lnI >= 28 ? 1 : (moonbrookeExpReq[lnI] / 2) % 256);
                    romData[0x13d8e + (lnI * 2)] = (byte)(special || lnI >= 28 ? 0 : (moonbrookeExpReq[lnI] / 2) / 256);
                }
            }

            // Replace weapon data
            for (int lnI = 0; lnI < 16; lnI++)
            {
                romData[0x1a00e + (lnI * 2)] = (byte)(special ? 1 : (weaponcost[lnI] / 2) % 256);
                romData[0x1a00f + (lnI * 2)] = (byte)(special ? 0 : (weaponcost[lnI] / 2) / 256);
            }

            // Replace armor data
            for (int lnI = 0; lnI < 11; lnI++)
            {
                romData[0x1a02e + (lnI * 2)] = (byte)(special ? 1 : (armorcost[lnI] / 2) % 256);
                romData[0x1a02f + (lnI * 2)] = (byte)(special ? 0 : (armorcost[lnI] / 2) / 256);
            }

            // Replace shield data
            for (int lnI = 0; lnI < 5; lnI++)
            {
                romData[0x1a044 + (lnI * 2)] = (byte)(special ? 1 : (shieldcost[lnI] / 2) % 256);
                romData[0x1a045 + (lnI * 2)] = (byte)(special ? 0 : (shieldcost[lnI] / 2) / 256);
            }

            // Replace helmet data
            for (int lnI = 0; lnI < 3; lnI++)
            {
                romData[0x1a04e + (lnI * 2)] = (byte)(special ? 1 : (helmetcost[lnI] / 2) % 256);
                romData[0x1a04f + (lnI * 2)] = (byte)(special ? 0 : (helmetcost[lnI] / 2) / 256);
            }

            // Replace item data
            for (int lnI = 0; lnI < 28; lnI++)
            {
                romData[0x1a054 + (lnI * 2)] = (byte)(special ? 1 : (itemcost[lnI] / 2) % 256);
                romData[0x1a055 + (lnI * 2)] = (byte)(special ? 0 : (itemcost[lnI] / 2) / 256);
            }
        }

        private void randomize()
        {
            Random r1 = new Random((int)DateTime.Now.Ticks % 2147483647);

            int intensity = (radSlightIntensity.Checked ? 1 : (radModerateIntensity.Checked ? 2 : (radHeavyIntensity.Checked ? 3 : 4)));
            byte[] monsterSize = { 8, 5, 5, 7, 5, 8, 5, 7, 5, 4, 5, 4, 5, 7, 4,
                                  8, 5, 4, 5, 5, 4, 4, 4, 5, 4, 2, 4, 4, 4, 4, 4,
                                  4, 4, 4, 5, 4, 4, 4, 7, 2, 4, 4, 4, 5, 4, 2, 2,
                                  8, 4, 5, 4, 7, 1, 2, 4, 4, 4, 4, 3, 4, 5, 1, 1,
                                  4, 2, 7, 4, 3, 1, 4, 2, 4, 3, 4, 2, 3, 1, 2, 3, 1, 1, 1 };

            int[] treasureAddrZ0 = { 0x19e41, 0x19c79 };
            int[] treasureAddrZ1 = { 0x19ed9, 0x19edd, 0x19ee1, 0x19e79, 0x19e7d, 0x19e81, 0x19e85, 0x19e89, 0x19e8d, 0x19e91, 0x19f0d, 0x19f11, 0x19f15, 0x19f1a }; // Cloak of wind/Mirror Of Ra and previous
            int[] treasureAddrZ2 = { 0x19f32, 0x19eb5, 0x19ef9, 0x19f01, 0x19f05, 0x19f09, 0x19f1e, 0x19f22, 0x19f2a, 0x19b5c }; // Pre-Golden, Jailor's, and Watergate keys
            int[] treasureAddrZ3 = { 0x19e45, 0x19e49, 0x19e4d, 0x19e51, 0x19e55, 0x19e59, 0x19e5d, 0x19e61,
                                    0x19e65, 0x19e69, 0x19e6d, 0x19e71, 0x19e75, 0x19ef9, 0x19f01, 0x19f05, 0x19f09 }; // Golden key to moon tower
            int[] treasureAddrZ4 = { 0x19ee5, 0x19ee9, 0x19eed, 0x19ef1, 0x19ef5 }; // Moon Tower
            int[] treasureAddrZ5 = { 0x19e95, 0x19e99, 0x19e9d, 0x19ea1, 0x19ea5, 0x19ea9, 0x19ead, 0x19eb1 }; // Sea Cave
            int[] treasureAddrZ6 = { 0x19eb9, 0x19ebd, 0x19ec1, 0x19ec5, 0x19ec9, 0x19ecd, 0x19ed1, 0x19ed5 }; // Rhone Cave
            List<int> allTreasureList = new List<int>();

            allTreasureList = addTreasure(allTreasureList, treasureAddrZ1);
            allTreasureList = addTreasure(allTreasureList, treasureAddrZ2);
            allTreasureList = addTreasure(allTreasureList, treasureAddrZ3);
            allTreasureList = addTreasure(allTreasureList, treasureAddrZ4);
            allTreasureList = addTreasure(allTreasureList, treasureAddrZ5);
            allTreasureList = addTreasure(allTreasureList, treasureAddrZ6);

            int[] allTreasure = allTreasureList.ToArray();

            // Shuffle treasure routine -> moderate randomness
            if (intensity == 2) {
                shuffle(treasureAddrZ1, r1);
                shuffle(treasureAddrZ2, r1);
                shuffle(treasureAddrZ3, r1);
                shuffle(treasureAddrZ4, r1);
                shuffle(treasureAddrZ5, r1);
                shuffle(treasureAddrZ6, r1);
            } else if (intensity == 3) // heavy randomness
                shuffle(allTreasure, r1, true);

            // Shuffle weapon, armor, and item stores.  Zone 1 = Pre-ship, zone 2 = Post-ship
            List<int> weaponTemp = new List<int>();
            for (int lnI = 0; lnI < 18; lnI++)
                weaponTemp.Add(0x19f9a + lnI);
            int[] weaponAddrZ1 = weaponTemp.ToArray();
            weaponTemp.Clear();

            for (int lnI = 0; lnI < 30; lnI++)
                weaponTemp.Add(0x19fac + lnI);
            int[] weaponAddrZ2 = weaponTemp.ToArray();
            weaponTemp.Clear();

            for (int lnI = 0; lnI < 24; lnI++)
                weaponTemp.Add(0x19fca + lnI);
            int[] itemAddrZ1 = weaponTemp.ToArray();
            weaponTemp.Clear();

            for (int lnI = 0; lnI < 36; lnI++)
                weaponTemp.Add(0x19fe8 + lnI);
            int[] itemAddrZ2 = weaponTemp.ToArray();
            weaponTemp.Clear();

            weaponTemp = addTreasure(weaponTemp, weaponAddrZ1);
            weaponTemp = addTreasure(weaponTemp, weaponAddrZ2);
            weaponTemp = addTreasure(weaponTemp, itemAddrZ1);
            weaponTemp = addTreasure(weaponTemp, itemAddrZ2);
            int[] allItems = weaponTemp.ToArray();
            weaponTemp.Clear();

            if (intensity == 2)
            {
                shuffle(weaponAddrZ1, r1);
                shuffle(weaponAddrZ2, r1);
                shuffle(itemAddrZ1, r1);
                shuffle(itemAddrZ2, r1);
            } else if (intensity == 3)
                shuffle(allItems, r1, false);

            // Finally, go through each six item block to find duplicates.  Any duplicates found -> 00.  108 items total.
            for (int lnI = 0; lnI < 18; lnI++)
            {
                List<int> items = new List<int>();
                for (int lnJ = 0; lnJ < 6; lnJ++)
                {
                    int location = 0x19f9a + (lnI * 6) + lnJ;
                    if (!items.Contains(romData[location]) && romData[location] != 0)
                        items.Add(romData[location]);
                }

                int[] itemArray = items.ToArray();
                for (int lnJ = 0; lnJ < 6; lnJ++)
                {
                    int location = 0x19f9a + (lnI * 6) + lnJ;
                    if (lnJ < itemArray.Length)
                        romData[location] = (byte)itemArray[lnJ];
                    else
                        romData[location] = 0;
                }
            }

            int randomModifier = 0;

            // rearrange monster zones
            for (int lnI = 0; lnI < 68; lnI++)
            {
                int byteToUse = 0x10519 + (lnI * 6);
                byte[] monsterZones = { romData[byteToUse + 0], romData[byteToUse + 1], romData[byteToUse + 2], romData[byteToUse + 3], romData[byteToUse + 4], romData[byteToUse + 5] };
                int sum = 0;
                for (int lnJ = 0; lnJ < 6; lnJ++)
                {
                    if (!(monsterZones[lnJ] == 127 || monsterZones[lnJ] == 255))
                    {
                        if (monsterZones[lnJ] > 128)
                            sum += (monsterZones[lnJ] - 128);
                        else
                            sum += monsterZones[lnJ];
                    }
                }
                int average = sum / 6;
                for (int lnJ = 0; lnJ < 6; lnJ++)
                {
                    randomModifier = (intensity == 1 ? (r1.Next() % 5) - 2 : (intensity == 2 ? (r1.Next() % 9) - 4 : (r1.Next() % 17) - 8));
                    if ((monsterZones[lnJ] == 127 || monsterZones[lnJ] == 255) && randomModifier != 0)
                    {
                        if (average + randomModifier > 0)
                            romData[byteToUse + lnJ] = (byte)(monsterZones[lnJ] == 127 ? average + randomModifier : average + randomModifier + 128);
                    }
                    else {
                        int test = (monsterZones[lnJ] >= 128 ? monsterZones[lnJ] - 128 : monsterZones[lnJ]);
                        if (test + randomModifier <= 0 || test + randomModifier >= 78) // You shouldn't randomly run into Atlas, Bazuzu, Zarlox, Hargon, or Malroth
                            romData[byteToUse + lnJ] = (byte)(monsterZones[lnJ] > 128 ? 255 : 127);
                        else
                            romData[byteToUse + lnJ] = (byte)(monsterZones[lnJ] + randomModifier);
                    }
                }

                // Assure there is at least one monster available in each zone.
                for (int lnJ = 0; lnJ < 6; lnJ++)
                {
                    if (romData[byteToUse + lnJ] != 127 && romData[byteToUse + lnJ] != 255)
                        break;
                    else if (lnJ == 5)
                        romData[byteToUse + lnJ] = (byte)(monsterZones[lnJ] > 128 ? average + 128 : average);
                }
            }

            // rearrange character statistics (0x13dd1-0x13ddc to start, 0x13ddd-0x13eda for stat ups.  First byte is Str/Agi, second is HP/MP.  [% 16] for Agi/MP, [/ 16] for Str/HP)
            // MAXIMUM STRENGTH:  160 (160 + 95(most powerful non-cursed weapon) = 255, maximum attack power allowed by the game)
            // MAXIMUM AGILITY:  255 (pretty sure going over that will force reset to 0)
            // MAXIMUM HP/MP:  255 (see line above)
            byte[] stats = { romData[0x13dd1 + 0], romData[0x13dd1 + 1], romData[0x13dd1 + 2], romData[0x13dd1 + 3],
                romData[0x13dd1 + 4], romData[0x13dd1 + 5], romData[0x13dd1 + 6], romData[0x13dd1 + 7],
                romData[0x13dd1 + 8], romData[0x13dd1 + 9], romData[0x13dd1 + 10], romData[0x13dd1 + 11] };
            
            for (int lnI = 0; lnI < 12; lnI++)
            {
                if (lnI == 3) // Midenhall starts with 0 MP.
                    continue;
                if (lnI % 4 >= 2)
                    randomModifier = (intensity == 1 ? (r1.Next() % 7) - 3 : (intensity == 2 ? (r1.Next() % 13) - 6 : (r1.Next() % 25) - 12));
                else
                    randomModifier = (intensity == 1 ? (r1.Next() % 3) - 1 : (intensity == 2 ? (r1.Next() % 7) - 3 : (r1.Next() % 13) - 6));

                if (romData[0x13dd1 + lnI] + randomModifier >= 0)
                {
                    romData[0x13dd1 + lnI] = (byte)(romData[0x13dd1 + lnI] + randomModifier);
                    stats[lnI] = romData[0x13dd1 + lnI];
                }
            }

            // 98 bytes for Midenhall, 88 bytes for Cannock, 68 bytes for Moonbrooke.  First break at 210, second break at 250.
            for (int lnI = 0; lnI < 254; lnI++)
            {
                int byteToUse = 0x13ddd + lnI;
                int randomModifier1 = (intensity == 1 ? (r1.Next() % 3) - 1 : (intensity == 2 ? (r1.Next() % 7) - 3 : (r1.Next() % 13) - 6));
                int randomModifier2 = (intensity == 1 ? (r1.Next() % 3) - 1 : (intensity == 2 ? (r1.Next() % 7) - 3 : (r1.Next() % 13) - 6));

                int part1 = romData[byteToUse] / 16;
                int part2 = romData[byteToUse] % 16;

                if (part1 + randomModifier1 < 0) part1 = 0;
                else if (part1 + randomModifier1 > 15) part1 = 15;
                else part1 = part1 + randomModifier1;

                int statToUse1 = (lnI > 244 ? lnI % 2 : (lnI > 206 ? lnI % 4 : lnI % 6)) * 2;
                int statToUse2 = (lnI > 244 ? lnI % 2 : (lnI > 206 ? lnI % 4 : lnI % 6)) * 2 + 1;

                if (lnI % 2 == 0)
                    if (stats[statToUse1] + part1 > 160)
                        part1 = (stats[statToUse1] + part1) - 160;
                else
                    if (stats[statToUse1] + part1 > 255)
                        part1 = (stats[statToUse1] + part1) - 255;

                if (part2 + randomModifier2 < 0) part2 = 0;
                else if (part2 + randomModifier2 > 15) part2 = 15;
                else part2 = part2 + randomModifier2;

                if (stats[statToUse2] + part2 > 255)
                    part2 = (stats[statToUse2] + part2) - 255;

                if ((lnI > 244 ? lnI % 2 : (lnI > 206 ? lnI % 4 : lnI % 6)) == 1)
                    part2 = 0; // Midenhall - 0 MP.

                romData[byteToUse] = (byte)((part1 * 16) + part2);
                stats[statToUse1] += (byte)part1;
                stats[statToUse2] += (byte)part2;
            }
            
            // rearrange spell learning levels
            for (int lnI = 0; lnI < 31; lnI++) // Spell #32 is not learned.
            {
                if (lnI == 15)
                    continue; // Spell #16 is also not learned.
                randomModifier = (intensity == 1 ? (r1.Next() % 3) - 1 : (intensity == 2 ? (r1.Next() % 7) - 3 : (r1.Next() % 13) - 6));
                byte spellLevel = romData[0x13edb + lnI];
                if (spellLevel + randomModifier <= 0)
                    spellLevel = 1;
                else
                    spellLevel = (byte)(spellLevel + randomModifier);
                romData[0x13edb + lnI] = spellLevel;
            }

            for (int lnI = 8; lnI < 15; lnI++)
                for (int lnJ = lnI + 1; lnJ < 15; lnJ++)
                {
                    if (romData[0x13edb + lnJ] < romData[0x13edb + lnI])
                    { // swap the two bytes, then restart lnJ <--- THIS DOES NOT WORK; SPELLS GET ALL SCREWED UP.  Instead, make the better spell be learned one level higher.
                        //swap(0x13edb + lnI, 0x13edb + lnJ);
                        //swap(0x1ae76 + lnI - 8, 0x1ae76 + lnJ - 8);
                        romData[0x13edb + lnJ - 8] = (byte)(romData[0x13edb + lnI - 8] + 1);
                        lnJ = lnI;
                    }
                }

            for (int lnI = 24; lnI < 31; lnI++)
                for (int lnJ = lnI + 1; lnJ < 31; lnJ++)
                {
                    if (romData[0x13edb + lnJ] < romData[0x13edb + lnI])
                    { // swap the two bytes, then restart lnJ <--- THIS DOES NOT WORK; SPELLS GET ALL SCREWED UP.  Instead, make the better spell be learned one level higher.
                        //swap(0x13edb + lnI, 0x13edb + lnJ);
                        //swap(0x1ae76 + lnI - 16, 0x1ae76 + lnJ - 16);
                        romData[0x13edb + lnJ - 16] = (byte)(romData[0x13edb + lnI - 16] + 1);
                        lnJ = lnI;
                    }
                }
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

        private void button1_Click(object sender, EventArgs e)
        {
            loadRom();
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
            loadRom(true);
            using (StreamWriter writer = File.CreateText(Path.Combine(Path.GetDirectoryName(txtFileName.Text), "DW2Compare.txt")))
            {
                for (int lnI = 0; lnI < 82; lnI++)
                    compareComposeString("monsters" + (lnI + 1).ToString("X2"), writer, (0x13805 + (15 * lnI)), 15);

                compareComposeString("midenhallEXP", writer, 0x13cd3, 100);
                compareComposeString("cannockEXP", writer, 0x13d35, 90);
                compareComposeString("moonbrookeEXP", writer, 0x13d8d, 70);
                compareComposeString("goldReq", writer, 0x1a00e, 126);

                compareComposeString("dewsyarn", writer, 0x19b5c, 1);
                compareComposeString("treasures", writer, 0x19e41, 216);
                compareComposeString("oddTreasure(1/9/13/16)", writer, 0x19f1a, 17);

                for (int lnI = 0; lnI < 18; lnI++)
                    compareComposeString("shopContents" + lnI.ToString("X2"), writer, 0x19f9a + (6 * lnI), 6);
                for (int lnI = 0; lnI < 68; lnI++)
                    compareComposeString("monsterZones" + lnI.ToString("X2"), writer, (0x10519 + (6 * lnI)), 6);
                compareComposeString("statStart", writer, 0x13dd1, 12);
                compareComposeString("statUps", writer, 0x13ddd, 260);
                compareComposeString("spellLearning", writer, 0x13edb, 32);
                compareComposeString("spellsLearned", writer, 0x1ae76, 32);
            }
            lblIntensityDesc.Text = "Comparison complete!  (DW2Compare.txt)";
        }

        private StreamWriter compareComposeString(string intro, StreamWriter writer, int startAddress, int length)
        {
            string final = "";
            string final2 = "";
            for (int lnI = 0; lnI < length; lnI++)
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
                    writer.WriteLine(txtFileName.Text);
        }

        private void txtFileName_Leave(object sender, EventArgs e)
        {
            runChecksum();
        }
    }
}
