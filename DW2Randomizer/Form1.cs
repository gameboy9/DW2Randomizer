using System;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace DW2Rando
{
    public partial class Form1 : Form
    {
        byte[] romData;

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
                using (var md5 = SHA1.Create())
                {
                    using (var stream = File.OpenRead(txtFileName.Text))
                    {
                        lblSHAChecksum.Text = BitConverter.ToString(md5.ComputeHash(stream)).ToLower().Replace("-", "");
                    }
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

        private void loadRom()
        {
            romData = File.ReadAllBytes(txtFileName.Text);
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
                    { 15, 17, 11, 11, 10, 1 }, // Magician
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
                    { 67, 73, 28, 75, 10, 132 }, // Evil Clown
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
                romData[byteValStart + 8] = (byte)((romData[byteValStart + 8] % 64) + (xp2 * 64));
                romData[byteValStart + 9] = (byte)((romData[byteValStart + 9] % 64) + (xp3 * 64));
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

        private void halfExpAndGoldReq()
        {
            // We'll divide all of these by two later...
            int[] weaponcost = new int[] { 20, 200, 2500, 26000, 60, 100, 330, 770, 25000, 1500, 4000, 15000, 8000, 16000, 4000, 500 };
            int[] armorcost = new int[] { 30, 1250, 70, 32767, 150, 390, 6400, 1250, 1000, 50, 50 };
            int[] shieldcost = new int[] { 90, 21500, 2000, 8800, 90 };
            int[] helmetcost = new int[] { 20000, 3150, 20 };

            int[] midenhallExpReq = new int[] { 6, 10, 20, 34, 70, 140, 220, 400, 500,
                  550, 700, 1150, 1200, 1500, 2000, 2000, 2500, 3000, 4000,
                5000, 6000, 6500, 7500, 8500, 10000, 11500, 12500, 12500, 15000,
                15000, 15000, 15000, 15000, 15000, 15000, 15000, 15000, 15000, 15000,
                20000, 25000, 25000, 25000, 25000, 25000, 25000, 25000, 25000, 15000 };
            int[] cannockExpReq = new int[] { 12, 18, 25, 45,
                90, 160, 300, 550, 800, 1000, 1100, 1400, 2000, 2000,
                2500, 3000, 3500, 4500, 5500, 6500, 7500, 7500, 8000, 9000,
                11000, 13000, 14000, 15000, 20000, 15000, 15000, 20000, 25000, 25000,
                20000, 30000, 30000, 30000, 30000, 30000, 10000, 30000, 30000, 20000 };
            int[] moonbrookeExpReq = new int[] { 50, 100, 150, 300,
                600, 900, 1100, 1300, 1500, 2000, 2000, 2500, 3000, 4000,
                5500, 7500, 8000, 10000, 11000, 12500, 15000, 20000, 25000, 15000,
                15000, 15000, 20000, 25000, 1, 1, 1, 1, 1, 1 }; // last six levels are forced to be at least 65,535 points.

            for (int lnI = 0; lnI < 49; lnI++)
            {
                romData[0x13cd3 + (lnI * 2)] = (byte)(midenhallExpReq[lnI] % 256);
                romData[0x13cd4 + (lnI * 2)] = (byte)(midenhallExpReq[lnI] / 256);
                if (lnI < 44)
                {
                    romData[0x13d35 + (lnI * 2)] = (byte)(cannockExpReq[lnI] % 256);
                    romData[0x13d36 + (lnI * 2)] = (byte)(cannockExpReq[lnI] / 256);
                }
                if (lnI < 34)
                {
                    romData[0x13d8d + (lnI * 2)] = (byte)(moonbrookeExpReq[lnI] % 256);
                    romData[0x13d8e + (lnI * 2)] = (byte)(moonbrookeExpReq[lnI] / 256);
                }
            }

            // Replace weapon data
            for (int lnI = 0; lnI < 16; lnI++)
            {
                romData[0x1a00e + (lnI * 2)] = (byte)((weaponcost[lnI] / 2) % 256);
                romData[0x1a00f + (lnI * 2)] = (byte)((weaponcost[lnI] / 2) / 256);
            }

            // Replace armor data
            for (int lnI = 0; lnI < 11; lnI++)
            {
                romData[0x1a02e + (lnI * 2)] = (byte)((armorcost[lnI] / 2) % 256);
                romData[0x1a02f + (lnI * 2)] = (byte)((armorcost[lnI] / 2) / 256);
            }

            // Replace shield data
            for (int lnI = 0; lnI < 5; lnI++)
            {
                romData[0x1a044 + (lnI * 2)] = (byte)((shieldcost[lnI] / 2) % 256);
                romData[0x1a045 + (lnI * 2)] = (byte)((shieldcost[lnI] / 2) / 256);
            }

            // Replace helmet data
            for (int lnI = 0; lnI < 3; lnI++)
            {
                romData[0x1a04e + (lnI * 2)] = (byte)((helmetcost[lnI] / 2) % 256);
                romData[0x1a04f + (lnI * 2)] = (byte)((helmetcost[lnI] / 2) / 256);
            }
        }

        private void randomize()
        {
            int intensity = (radSlightIntensity.Checked ? 1 : (radModerateIntensity.Checked ? 2 : (radHeavyIntensity.Checked ? 3 : 4)));
            byte[] monsterSize = { 8, 5, 5, 7, 5, 8, 5, 7, 5, 4, 5, 4, 5, 7, 4, 
                                  8, 5, 4, 5, 5, 4, 4, 4, 5, 4, 2, 4, 4, 4, 4, 4, 
                                  4, 4, 4, 5, 4, 4, 4, 7, 2, 4, 4, 4, 5, 4, 2, 2, 
                                  8, 4, 5, 4, 7, 1, 2, 4, 4, 4, 4, 3, 4, 5, 1, 1, 
                                  4, 2, 7, 4, 3, 1, 4, 2, 4, 3, 4, 2, 3, 1, 2, 3, 1, 1, 1};

        }
    }
}
