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
                lblIntensityDesc.Text = "Small changes to monster zones, boss fights, character stats, and spell learning.  No changes to treasures or shops will occur.";
        }

        private void radModerateIntensity_CheckedChanged(object sender, EventArgs e)
        {
            if (radModerateIntensity.Checked)
                lblIntensityDesc.Text = "Moderate changes to monster zones, boss fights, character stats, and spell learning.  Substantial shop changes to treasures and shops will occur, but all of them will stay in their respective zones.";

        }

        private void radHeavyIntensity_CheckedChanged(object sender, EventArgs e)
        {
            if (radHeavyIntensity.Checked)
                lblIntensityDesc.Text = "Major changes to monster zones, boss fights, character stats, and spell learning.  Expect major shop changes and treasure scrambling(key items will stay in their respective zones), but you will still be able to buy anything " +
                    "that you would normally buy at a shop and find that you normally would find in a treasure box.";

        }

        private void radInsaneIntensity_CheckedChanged(object sender, EventArgs e)
        {
            if (radInsaneIntensity.Checked)
                lblIntensityDesc.Text = "The ultimate randomization!  Complete changes to monsters, including all stats(except HP, MP, Attack, Defense) and abilities, " +
                    "complete changes to all items and where they reside, treasure and/or store(key items appear before they are required), with prices recalculated " +
                    "according to power and ability, and complete randomization to when spells are learned, as well as all statistics.(stat overflow will be avoided)";

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtSeed.Text = (DateTime.Now.Ticks % 2147483647).ToString();

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
                if (chkHalfExpGoldReq.Checked) halfExpAndGoldReq();
                if (chkDoubleXP.Checked) doubleExp();
                if (radSlightIntensity.Checked || radModerateIntensity.Checked || radHeavyIntensity.Checked)
                    randomize();
                else
                    superRandomize();
            }

            saveRom();
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

        private void loadRom(bool extra = false)
        {
            romData = File.ReadAllBytes(txtFileName.Text);
            if (extra)
                romData2 = File.ReadAllBytes(Path.Combine(Path.GetDirectoryName(txtFileName.Text), "DW2Rando_" + txtSeed.Text + ".nes"));
        }

        private void saveRom()
        {
            string finalFile = Path.Combine(Path.GetDirectoryName(txtFileName.Text), "DW2Rando_" + txtSeed.Text + ".nes");
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
            romData[0x10356 + (4 * 4) + 2] = 0x47;
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
            special = true;
            // We'll divide all of these by two later...
            int[] weaponcost = new int[] { 20, 200, 2500, 26000, 60, 100, 330, 770, 25000, 1500, 4000, 15000, 8000, 16000, 4000, 500 };
            int[] armorcost = new int[] { 30, 1250, 70, 32767, 150, 390, 6400, 1250, 1000, 32000, 48000 };
            int[] shieldcost = new int[] { 90, 21500, 2000, 8800, 15000 };
            int[] helmetcost = new int[] { 20000, 3150, 15000 };
            // Adjusting item costs for the super randomizer, where they could be made available for purchasing in a store!
            int[] itemcost = new int[] { 10, 300, 300, 0, 0, 5000, 600, 0, 8000, 8000, 1000, 1500, 640, 10000, 50000, 70, 40, 80, 2, 3000, 1500, 2000, 0, 8, 15, 10000, 2, 2 };

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

            // House of healing cost halved
            romData[0x18659] = (20 / 2);

            // Inn prices halved
            byte[] inns = { 4, 6, 8, 12, 20, 2, 25, 30, 40, 30 };
            for (int lnI = 0; lnI < inns.Length; lnI++)
                romData[0x19f90 + lnI] = (byte)(inns[lnI] / 2);
        }

        private void superRandomize()
        {
            int randomModifier = 0;
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

            byte[] monsterSize = { 8, 5, 5, 7, 5, 8, 5, 7, 5, 4, 5, 4, 5, 7, 4,
                                  8, 5, 4, 5, 5, 4, 4, 4, 5, 4, 2, 4, 4, 4, 4, 4,
                                  4, 4, 4, 5, 4, 4, 4, 7, 2, 4, 4, 4, 5, 4, 2, 2,
                                  8, 4, 5, 4, 7, 1, 2, 4, 4, 4, 4, 3, 4, 5, 1, 1,
                                  4, 2, 7, 4, 3, 1, 4, 2, 4, 3, 4, 2, 3, 1, 2, 3, 1, 1, 1 };

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
                enemyStats[1] = (byte)((r1.Next() % 16) * 16);
                int gp = enemyStats[2] + (r1.Next() % (lnI + 1));
                enemyStats[2] = (byte)(lnI == 0x33 || gp > 255 ? 255 : gp); // Gold Orc gold = 255
                float xp = romData[byteValStart + 3] + ((romData[byteValStart + 8] / 64) * 256) + ((romData[byteValStart + 9] / 64) * 1024);

                enemyStats[4] = (byte)(r1.Next() % 256);
                enemyStats[5] += (byte)(r1.Next() % (enemyStats[5] * 3 / 2) - (enemyStats[5] / 2));

                byte[] res1 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 3, 4, 5, 6, 7 };
                byte[] res2 = { 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 3, 4, 5, 6, 7 };
                byte[] res3 = { 0, 0, 0, 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 5, 6, 7 };
                byte[] res4 = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
                byte[] res5 = { 0, 1, 2, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6, 7, 7, 7 };
                byte[] res6 = { 0, 1, 2, 3, 4, 5, 5, 5, 6, 6, 6, 6, 7, 7, 7, 7 };
                byte[] res7 = { 0, 1, 2, 3, 4, 5, 6, 7, 7, 7, 7, 7, 7, 7, 7, 7 };
                if (lnI < 12)
                {
                    enemyStats[7] = (byte)(((r1.Next() % 7) * 64) + (res1[r1.Next() % 16] * 8) + (res1[r1.Next() % 16]));
                    enemyStats[8] = (byte)((res1[r1.Next() % 16] * 8) + (res1[r1.Next() % 16]));
                    enemyStats[9] = (byte)((res1[r1.Next() % 16] * 8) + (res1[r1.Next() % 16]));
                }
                else if (lnI < 24)
                {
                    enemyStats[7] = (byte)(((r1.Next() % 7) * 64) + (res2[r1.Next() % 16] * 8) + (res2[r1.Next() % 16]));
                    enemyStats[8] = (byte)((res2[r1.Next() % 16] * 8) + (res2[r1.Next() % 16]));
                    enemyStats[9] = (byte)((res2[r1.Next() % 16] * 8) + (res2[r1.Next() % 16]));
                }
                else if (lnI < 36)
                {
                    enemyStats[7] = (byte)(((r1.Next() % 7) * 64) + (res3[r1.Next() % 16] * 8) + (res3[r1.Next() % 16]));
                    enemyStats[8] = (byte)((res3[r1.Next() % 16] * 8) + (res3[r1.Next() % 16]));
                    enemyStats[9] = (byte)((res3[r1.Next() % 16] * 8) + (res3[r1.Next() % 16]));
                }
                else if (lnI < 47)
                {
                    enemyStats[7] = (byte)(((r1.Next() % 7) * 64) + (res4[r1.Next() % 16] * 8) + (res4[r1.Next() % 16]));
                    enemyStats[8] = (byte)((res4[r1.Next() % 16] * 8) + (res4[r1.Next() % 16]));
                    enemyStats[9] = (byte)((res4[r1.Next() % 16] * 8) + (res4[r1.Next() % 16]));
                }
                else if (lnI < 58)
                {
                    enemyStats[7] = (byte)(((r1.Next() % 7) * 64) + (res5[r1.Next() % 16] * 8) + (res5[r1.Next() % 16]));
                    enemyStats[8] = (byte)((res5[r1.Next() % 16] * 8) + (res5[r1.Next() % 16]));
                    enemyStats[9] = (byte)((res5[r1.Next() % 16] * 8) + (res5[r1.Next() % 16]));
                }
                else if (lnI < 69)
                {
                    enemyStats[7] = (byte)(((r1.Next() % 7) * 64) + (res6[r1.Next() % 16] * 8) + (res6[r1.Next() % 16]));
                    enemyStats[8] = (byte)((res6[r1.Next() % 16] * 8) + (res6[r1.Next() % 16]));
                    enemyStats[9] = (byte)((res6[r1.Next() % 16] * 8) + (res6[r1.Next() % 16]));
                }
                else
                {
                    enemyStats[7] = (byte)(((r1.Next() % 7) * 64) + (res7[r1.Next() % 16] * 8) + (res7[r1.Next() % 16]));
                    enemyStats[8] = (byte)((res7[r1.Next() % 16] * 8) + (res7[r1.Next() % 16]));
                    enemyStats[9] = (byte)((res7[r1.Next() % 16] * 8) + (res7[r1.Next() % 16]));
                }

                byte[] enemyPatterns = { 0, 0, 0, 0, 0, 0, 0, 0 };
                bool[] enemyPage2 = { false, false, false, false, false, false, false, false };
                bool concentration = false;

                // 40% chance of being a basic attack monster... and not a Magician, Enchanter, Sorcerer, Magic Baboon, Magidrakee
                if (r1.Next() % 100 < 40 && lnI != 0x14 && lnI != 0x10 && lnI != 0x19 && lnI != 0x1c && lnI != 0x2e)
                {
                    // ... but do place a 50% chance for "funny" attacks...
                    if (r1.Next() % 2 == 1)
                    {
                        for (int lnJ = 0; lnJ < 8; lnJ++)
                        {
                            // 37.5% chance of setting a different attack.
                            byte random = (byte)(r1.Next() % 12);
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
                } else
                {
                    for (int lnJ = 0; lnJ < 8; lnJ++)
                    {
                        // 50% chance of setting a different attack.
                        byte random = (byte)(r1.Next() % 64);
                        if (random >= 1 && random <= 31) // 0 would be fine, but it's already set.
                        {
                            if (random == 30 && concentration)
                                continue; // do NOT set the concentration bit again.  Maintain regular attack.
                            if (random >= 16)
                                enemyPage2[lnJ] = true;
                            enemyPatterns[lnJ] = (byte)(random % 16);
                        }
                    }
                }
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
                    enemyStats[4] = 255;
                    enemyStats[5] = 1;
                }
                if (lnI == 0x05) // Healer
                    enemyPatterns[0] = (byte)((r1.Next() % 3) + 12); // heal, healmore, healall

                enemyStats[10] = (byte)(enemyPatterns[0] + (enemyPatterns[1] * 16));
                enemyStats[11] = (byte)(enemyPatterns[2] + (enemyPatterns[3] * 16));
                enemyStats[12] = (byte)(enemyPatterns[4] + (enemyPatterns[5] * 16));
                enemyStats[13] = (byte)(enemyPatterns[6] + (enemyPatterns[7] * 16));
                enemyStats[14] = (byte)((enemyPage2[0] ? 1 : 0) + (enemyPage2[1] ? 2 : 0) + (enemyPage2[2] ? 4 : 0) + (enemyPage2[3] ? 8 : 0) + 
                    (enemyPage2[4] ? 16 : 0) + (enemyPage2[5] ? 32 : 0) + (enemyPage2[6] ? 64 : 0) + (enemyPage2[7] ? 128 : 0));

                // Set XP value using a special formula.  Set bits 3, 8, and 9

                //First page
                //#$0 Attack, #$1 Heroic Attack, #$2 Poison Attack, #$3 Faint Attack, #$4 Parry, #$5 Run Away,
                //#$6 Firebal, #$7 Firebane, #$8 Explodet, #$9 Heal*, #$A Healmore*, #$B Heal All*, #$C Heal**,
                //#$D Healmore**, #$E Heal All**, #$F Revive

                //float atkMult = 1;
                //float atkBonus = 0;
                //for (int lnJ = 0; lnJ < 8; lnJ++)
                //{
                //    if (!enemyPage2[lnJ])
                //    {
                //        switch (enemyPatterns[lnJ])
                //        {
                //            case 1:
                //                atkMult *= (float)1.05;
                //                atkBonus += (float)0.25;
                //                break;
                //            case 2:
                //                atkMult *= (float)1.02;
                //                atkBonus += (float)0.1;
                //                break;
                //            case 3:
                //                atkMult *= (float)1.02;
                //                atkBonus += (float)0.1;
                //                break;
                //            case 6: // Really depends on an all vs. none attack, as well as strength, but for now...
                //                atkMult *= (float)1.01;
                //                atkBonus += (float)0.05;
                //                break;
                //            case 7: // Really depends on an all vs. none attack, as well as strength, but for now...
                //                atkMult *= (float)1.03;
                //                atkBonus += (float)0.5;
                //                break;
                //            case 8: // Really depends on an all vs. none attack, as well as strength, but for now...
                //                atkMult *= (float)1.05;
                //                atkBonus += (float)1.0;
                //                break;
                //            case 9: // Really depends on an all vs. none attack, as well as strength, but for now...
                //            case 12:
                //                atkMult *= (float)1.01;
                //                atkBonus += (float)0.05;
                //                break;
                //            case 10: // Really depends on an all vs. none attack, as well as strength, but for now...
                //            case 13:
                //                atkMult *= (float)1.02;
                //                atkBonus += (float)0.25;
                //                break;
                //            case 11: // Really depends on an all vs. none attack, as well as strength, but for now...
                //            case 14:
                //                atkMult *= (float)1.04;
                //                atkBonus += (float)0.5;
                //                break;
                //            case 15:
                //                atkMult *= (float)1.04;
                //                atkBonus += (float)0.5;
                //                break;
                //        }
                //    }
                //    else
                //    {
                //        //Second page
                //        //#$0 Defence, #$1 Increase, #$2 Sleep, #$3 Stopspell, #$4 Surround, #$5 Defeat, $6 Sacrifice***,
                //        //#$7 Weak Flames, #$8 Strong Flames, #$9 Deadly Flames, #$A Poison Breath, #$B Sweet Breath,
                //        //#$C Call For Help, #$D Two Attacks, #$E concentration byte, #$F Dance Strange Jig

                //        switch (enemyPatterns[lnJ])
                //        {
                //            case 0:
                //                atkMult *= (float)1.005;
                //                atkBonus += (float)0.05;
                //                break;
                //            case 1:
                //                atkMult *= (float)1.005;
                //                atkBonus += (float)0.05;
                //                break;
                //            case 2:
                //                atkMult *= (float)1.02;
                //                atkBonus += (float)0.2;
                //                break;
                //            case 3:
                //                atkMult *= (float)1.01;
                //                atkBonus += (float)0.05;
                //                break;
                //            case 4:
                //                atkMult *= (float)1.01;
                //                atkBonus += (float)0.05;
                //                break;
                //            case 5:
                //                atkMult *= (float)1.06;
                //                atkBonus += (float)1.0;
                //                break;
                //            case 6:
                //                atkMult *= (float)1.06;
                //                atkBonus += (float)1.0;
                //                break;
                //            case 7:
                //                atkMult *= (float)1.01;
                //                atkBonus += (float)0.1;
                //                break;
                //            case 8:
                //                atkMult *= (float)1.03;
                //                atkBonus += (float)0.5;
                //                break;
                //            case 9:
                //                atkMult *= (float)1.06;
                //                atkBonus += (float)1.0;
                //                break;
                //            case 10:
                //                atkMult *= (float)1.01;
                //                atkBonus += (float)0.1;
                //                break;
                //            case 11:
                //                atkMult *= (float)1.02;
                //                atkBonus += (float)0.2;
                //                break;
                //            case 12:
                //                //atkMult *= (float)1.005;
                //                break;
                //            case 13:
                //                atkMult *= (float)1.03;
                //                atkBonus += (float)0.25;
                //                break;
                //            case 14:
                //                //atkMult *= (float)1.01;
                //                break;
                //            case 15:
                //                atkMult *= (float)1.02;
                //                atkBonus += (float)0.1;
                //                break;
                //        }
                //    }
                //}
                //xp += atkBonus;
                //xp *= atkMult;

                //float resMult = 1;
                //for (int lnJ = 7; lnJ <= 9; lnJ++)
                //{
                //    int p1 = enemyStats[lnJ] % 8;
                //    int p2 = (enemyStats[lnJ] % 64) / 8;
                //    float resSingle = 1;
                //    resSingle = (float)(p1 == 0 ? 0 : p1 == 1 ? 1 : p1 == 2 ? 2 : p1 == 3 ? 3 : p1 == 4 ? 4 : p1 == 5 ? 6 : p1 == 6 ? 8 : 10);
                //    resSingle *= (float)(lnJ == 7 ? 1 : lnJ == 8 ? .9 : .9);
                //    resMult *= (1 + (resSingle / 100));
                //    resSingle = (float)(p2 == 0 ? 0 : p2 == 1 ? 1 : p2 == 2 ? 2 : p2 == 3 ? 3 : p2 == 4 ? 4 : p2 == 5 ? 6 : p2 == 6 ? 8 : 10);
                //    resSingle *= (float)(lnJ == 7 ? 1 : lnJ == 8 ? .6 : .5);
                //    resMult *= (1 + (resSingle / 100));
                //}
                //xp *= resMult;

                if (lnI == 0x49)
                    lnI = 0x49;

                xp = (float)Math.Round(xp, 0);
                byte xp1 = (byte)(xp > 4095 ? 255 : (xp % 256));
                byte xp2 = (byte)(xp > 4095 ? 192 : (Math.Floor(xp / 256) % 4) * 64);
                byte xp3 = (byte)(xp > 4095 ? 192 : (Math.Floor(xp / 1024)) * 64);

                enemyStats[3] = xp1;
                enemyStats[8] = (byte)(enemyStats[8] + xp2);
                enemyStats[9] = (byte)(enemyStats[9] + xp3);

                for (int lnJ = 0; lnJ < 15; lnJ++)
                    romData[byteValStart + lnJ] = enemyStats[lnJ];
            }

            // Totally randomize monster zones (but make sure the first 20 zones have easier monsters) (10356-10389, 10519-10680, )
            for (int lnI = 0; lnI < 68; lnI++)
            {
                int byteToUse = 0x10519 + (lnI * 6);
                bool zone = false;
                for (int lnJ = 0; lnJ < 6; lnJ++)
                {
                    // First 11 zones have a 50% chance of a monster in each byte.  All 6 bytes will be at least 128... we don't want any "special fights" in these zones.
                    if (lnI < 11)
                    {
                        if (r1.Next() % 2 == 0)
                        {
                            zone = true;
                            romData[byteToUse + lnJ] = (byte)((r1.Next() % (lnI + 6)) + 1);
                        }
                        else
                            romData[byteToUse + lnJ] = 127;
                    }
                    else if (lnI < 21) // For the next 10 zones, it's a 67% chance.  Still no special fights.
                    {
                        if (r1.Next() % 3 < 2)
                        {
                            zone = true;
                            romData[byteToUse + lnJ] = (byte)((r1.Next() % (lnI + 12)) + 1);
                        }
                        else
                            romData[byteToUse + lnJ] = 127;
                    }
                    else if (lnI == 42 || lnI == 43 || lnI == 44 || lnI == 54 || lnI == 55 ) // Sea cave.  No special bout here.
                    {
                        if (r1.Next() % 5 < 4)
                        {
                            zone = true;
                            romData[byteToUse + lnJ] = (byte)((r1.Next() % 37) + 41);
                        }
                        else
                            romData[byteToUse + lnJ] = 127;
                    }
                    else if (lnI == 45 || lnI == 46 || lnI == 47 || lnI == 48 || lnI == 49) // Rhone cave.  Introduce Atlas chance.
                    {
                        if (r1.Next() % 5 < 4)
                        {
                            zone = true;
                            romData[byteToUse + lnJ] = (byte)((r1.Next() % 28) + 51);
                        }
                        else
                            romData[byteToUse + lnJ] = 127;
                    }
                    else if (lnI == 50 || lnI == 51) // Rhone area.  Introduce Bazuzu chance.
                    {
                        if (r1.Next() % 10 < 9)
                        {
                            zone = true;
                            romData[byteToUse + lnJ] = (byte)((r1.Next() % 18) + 61);
                        }
                        else
                            romData[byteToUse + lnJ] = 127;
                    }
                    else if (lnI == 52 || lnI == 53) // Hargon's Castle.
                    {
                        if (r1.Next() % 10 < 9)
                        {
                            zone = true;
                            romData[byteToUse + lnJ] = (byte)((r1.Next() % 13) + 66);
                        }
                        else
                            romData[byteToUse + lnJ] = 127;
                    }
                    else // Finally, a 80% chance.  Also introduce a 50% chance of the 19 "special bouts".
                    {
                        if (r1.Next() % 5 < 4)
                        {
                            zone = true;
                            romData[byteToUse + lnJ] = (byte)((r1.Next() % 77) + 1);
                        } else
                            romData[byteToUse + lnJ] = 127;
                    }
                }
                if (!zone)
                    romData[byteToUse + 5] = (byte)((r1.Next() % (lnI < 11 ? lnI + 6 : lnI < 21 ? lnI + 10 : 78)) + 1);

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
                    for (int lnJ = 0; lnJ < 6; lnJ++) {
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

            // Randomize which items equate to which effects (except the Wizard's Ring, Medical Herb, and Antidote Herb) (13537-1353b)
            for (int lnI = 0; lnI < 5; lnI++)
            {
                // randomize from 1-35
                romData[0x13537 + lnI] = (byte)((r1.Next() % 35) + 1);
            }

            // Totally randomize weapons, armor, shields, helmets (13efb-13f1d, 1a00e-1a08b for pricing)
            byte[] maxPower = { 0, 0, 0, 0 };
            for (int lnI = 0; lnI < 35; lnI++)
            {
                byte power = 0;
                if (lnI == 0 || lnI == 16)
                    power = (byte)(r1.Next() % 10);
                else if (lnI < 16)
                    power = (byte)(r1.Next() % 100);
                else if (lnI < 27)
                    power = (byte)(r1.Next() % 70);
                else if (lnI < 31)
                    power = (byte)(r1.Next() % 40);
                else
                    power = (byte)(r1.Next() % 30);
                power = (byte)(r1.Next() % (lnI < 16 ? (7 * (lnI + 1)) : lnI < 27 ? (7 * (lnI - 15)) : lnI < 31 ? (7 * (lnI - 26)) : (7 * (lnI - 30))));
                power += 1; // To avoid 0 power... and a non-selling item...
                maxPower[(lnI < 16 ? 0 : lnI < 27 ? 1 : lnI < 31 ? 2 : 3)] = (power > maxPower[(lnI < 16 ? 0 : lnI < 27 ? 1 : lnI < 31 ? 2 : 3)] ? power : 
                    maxPower[(lnI < 16 ? 0 : lnI < 27 ? 1 : lnI < 31 ? 2 : 3)]);
                romData[0x13efb + lnI] = power;

                double price = Math.Round((lnI < 16 ? Math.Pow(power, 2.3) : lnI < 27 ? Math.Pow(power, 2.4) : lnI < 31 ? Math.Pow(power, 2.8) : Math.Pow(power, 3.0)), 0);
                price = (float)Math.Round(price, 0);

                romData[0x1a00e + (lnI * 2) + 0] = (byte)(price % 256);
                romData[0x1a00e + (lnI * 2) + 1] = (byte)(Math.Floor(price / 256));
            }

            // Totally randomize who can equip (1a3ce-1a3f0).  At least one person can equip something...
            for (int lnI = 0; lnI < 35; lnI++)
            {
                if (lnI == 0 || lnI == 16) romData[0x1a3ce + lnI] = 7; // everyone can equip the first weapon and armor.
                else romData[0x1a3ce + lnI] = (byte)((r1.Next() % 7) + 1);
            }

            // Totally randomize spell strengths (18be0, 13be8, 13bf0, 127d5-1286a for strength, 134fa-13508 for cost, 13509-13517 for 3/4 cost)
            // No need to do this right now because we really can't adjust the spells to begin with.
            //for (int lnI = 0; lnI < 30; lnI++)
            //{
            //    if (lnI == 28) continue; // Skip the antidote herb

            //}

            // Randomize field item strengths (124b2(Wizard's Ring), 18be0(Heal), 18be8(Healmore), 18bf0(Healall), 19602(Medical Herb) only)

            // Totally randomize spell learning (13edb-13eea, 13eeb-13efa, 1ae76-1ae95, 1b63c-1b727(text), separate the two casters with "ff ff")
            // Text - 0 to 9 (00-09), a-z (0a-23), A-Z(24-3d)
            byte level = 1;
            for (int lnI = 0; lnI < 32; lnI++)
            {
                if (lnI == 15) { level = 1; continue; }
                if (lnI == 31) continue; // We can't figure out how to get an eighth command spell in there yet.
                if (lnI == 4 || lnI == 8 || lnI == 20 || lnI == 24) continue; // Heal/Healmore MUST be learned at level 1, so leave that byte alone.

                level += (byte)((r1.Next() % 7) + 1);
                romData[0x13edb + lnI] = level;
                if (lnI == 3 || lnI == 7 || lnI == 15 || lnI == 19 || lnI == 23) level = 1; // Reset with each fight page (4x2 spells each) and command page (8 spells each)
            }

            // Totally randomize treasures... but make sure key items exist before they are needed! (19e41-19f15, 19f1a-19f2a, 19c79, 19c84)
            int[] treasureAddrZ0 = { 0x19e41, 0x19c79 }; // 2
            int[] treasureAddrZ1 = { 0x19ed9, 0x19edd, 0x19ee1, 0x19e79, 0x19e7d,
                                     0x19e81, 0x19e85, 0x19e89, 0x19e8d, 0x19e91,
                                     0x19f0d, 0x19f11, 0x19f15, 0x19f1a }; // Cloak of wind/Mirror Of Ra and previous; 14
            int[] treasureAddrZ2 = { 0x19f32, 0x19eb5, 0x19ef9, 0x19f01, 0x19f05,
                                     0x19f09, 0x19f1e, 0x19f22, 0x19f2a, 0x19b5c }; // Pre-Golden key; 10
            int[] treasureAddrZ3 = { 0x19e45, 0x19e49, 0x19e4d, 0x19e51, 0x19e55,
                                     0x19e59, 0x19e5d, 0x19e61, 0x19e65, 0x19e69,
                                     0x19e6d, 0x19e71, 0x19e75, 0x19ef9, 0x19f01,
                                     0x19f05, 0x19f09 }; // Golden key to moon tower; Jailor's required by here; 17
            int[] treasureAddrZ4 = { 0x19ee5, 0x19ee9, 0x19eed, 0x19ef1, 0x19ef5 }; // Moon Tower; 5
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

            List<byte> treasureList = new List<byte>();
            byte[] legalTreasures = { 0x01, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
                                      0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f,
                                      0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x28, 0x29, 0x2A, 0x2B, 0x2C, 0x2d, 0x2e, 0x2f,
                                      0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x37, 0x38, 0x39, 0x3b, 0x3c, 0x3d, 0x40, 0x43, 0x44 };
            for (int lnI = 0; lnI < 64; lnI++)
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
            // Totally randomize stores (cannot have Jailor's Key in a weapons store) (19f9a-1a00b)
            for (int lnI = 0; lnI < 18; lnI++)
            {
                int byteToUse = 0x19f9a + (lnI * 6);
                // Always have one item in store.  Let chances of having another item = 91%/83%/75%/67%/58%
                byte chance = 11;
                bool fail = false;
                for (int lnJ = 0; lnJ < 6; lnJ++)
                {
                    if (!fail && r1.Next() % 12 <= chance - lnJ)
                    {
                        // Add item
                        byte treasure = (byte)((r1.Next() % 61) + 1); // 0x00, 0x3E, and 0x3F we can't get...
                        if (!(treasure == 0x3A || treasure == 0x27 || treasure == 0x36 || treasure == 0x28 || treasure == 0x2B) && !(lnI < 8 && treasure == 0x39))
                        {
                            romData[byteToUse + lnJ] = treasure;
                        } else
                            fail = true;
                    } else
                    {
                        romData[byteToUse + lnJ] = 0;
                        fail = true;
                    }
                }

                // Go through to find duplicates.Any duplicates found-> 00.  108 items total.
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

            // Verify that key items are available in either a store or a treasure chest in the right zone.
            byte[] keyItems = { 0x2b, 0x2e, 0x37, 0x39, 0x26, 0x28, 0x40, 0x43, 0x44 };
            byte[] keyTreasure = { 16, 16, 26, 43, 48, 56, 64, 64, 64 };
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
                    if (romData[allTreasure[tRand]] != keyTreasure[0] && romData[allTreasure[tRand]] != keyTreasure[1] && romData[allTreasure[tRand]] != keyTreasure[2] &&
                        romData[allTreasure[tRand]] != keyTreasure[3] && romData[allTreasure[tRand]] != keyTreasure[4] && romData[allTreasure[tRand]] != keyTreasure[5] &&
                        romData[allTreasure[tRand]] != keyTreasure[6] && romData[allTreasure[tRand]] != keyTreasure[7] && romData[allTreasure[tRand]] != keyTreasure[8])
                    {
                        romData[allTreasure[tRand]] = keyItems[lnI];
                        legal = true;
                    }
                }

            }

            // Randomize starting stats.  Do not exceed 16 strength and agility, and 40 HP/MP. (13dd1-13ddc)
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
            int avgStrength = ((maxStrength / 50));
            int[] avg7 = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12, 12, 13, 13, 14, 15 };
            int[] avg5 = { 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 4, 4, 5, 5, 6, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            int[] avg4 = { 0, 0, 0, 0, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
            int[] avg3 = { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 4, 4, 4, 5, 6, 7, 8, 9, 10 };
            // Randomize stat gains... but don't put any stat above 255! (13ddd-13eda)
            // 98 bytes for Midenhall, 88 bytes for Cannock, 68 bytes for Moonbrooke.  First break at 210, second break at 250.
            for (int lnI = 0; lnI < 254; lnI++)
            {
                int byteToUse = 0x13ddd + lnI;

                int statToUse1 = (lnI > 244 ? lnI % 2 : (lnI > 206 ? lnI % 4 : lnI % 6)) * 2;
                int statToUse2 = (lnI > 244 ? lnI % 2 : (lnI > 206 ? lnI % 4 : lnI % 6)) * 2 + 1;

                int randomModifier1 = 0;
                int randomModifier2 = 0;

                if (lnI % 2 == 0)
                {
                    int r1Result = (r1.Next() % 30);
                    r1Result = (r1Result < 0 ? 0 : r1Result);
                    randomModifier1 = (avgStrength == 5 ? avg5[r1Result] : avgStrength == 4 ? avg4[r1Result] : avg3[r1Result]);
                    if (stats[statToUse1] + randomModifier1 > maxStrength)
                        randomModifier1 = 0;

                    int r2Result = (r1.Next() % 30);
                    r2Result = (r2Result < 0 ? 0 : r2Result);
                    randomModifier2 = (statToUse2 == 1 ? avg7[r2Result] : statToUse2 == 5 ? avg5[r2Result] : avg7[r2Result]);
                    if (stats[statToUse2] + randomModifier2 > maxAgility)
                        randomModifier2 = 0;
                }
                else
                {
                    int r1Result = (r1.Next() % 30);
                    r1Result = (r1Result < 0 ? 0 : r1Result);
                    randomModifier1 = avg5[r1Result];
                    if (stats[statToUse1] + randomModifier1 > 255)
                        randomModifier1 = 0;

                    int r2Result = (r1.Next() % 30);
                    r2Result = (r2Result < 0 ? 0 : r2Result);
                    randomModifier2 = (statToUse2 == 3 ? 0 : statToUse2 == 7 ? avg5[r2Result] : avg7[r2Result]);
                    if (stats[statToUse2] + randomModifier2 > 255)
                        randomModifier2 = 0;
                }

                romData[byteToUse] = (byte)((randomModifier1 * 16) + randomModifier2);
                stats[statToUse1] += (byte)randomModifier1;
                stats[statToUse2] += (byte)randomModifier2;
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
                //// Give it 30 randomizations.  If the attack doesn't go under 24, then give him the last randomized weapon.
                //for (int lnJ = 0; lnJ < 30; lnJ++)
                //{
                //    int rand = r1.Next() % 16;
                //    if (romData[0x13efb + rand] < 24 || lnJ >= 29)
                //    {
                //        romData[byteToUse + 0] = (byte)(64 + rand);
                //        break;
                //    }
                //}

                //// Give it 30 randomizations.  If the defense doesn't go under 16, then give him the last randomized armor.
                //for (int lnJ = 0; lnJ < 30; lnJ++)
                //{
                //    int rand = (r1.Next() % 11) + 16;
                //    if (romData[0x13efb + rand] < 16 || lnJ >= 29)
                //    {
                //        romData[byteToUse + 1] = (byte)(64 + rand);
                //        break;
                //    }
                //}
            }
        }

        private void randomize()
        {
            Random r1;
            try
            {
                r1 = new Random(int.Parse(txtSeed.Text));
            } catch
            {
                MessageBox.Show("Invalid seed.  It must be a number from 0 to 2147483648.");
                return;
            }

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

            bool jailorsClear = false;
            while (!jailorsClear)
            {
                if (intensity == 2)
                {
                    shuffle(weaponAddrZ1, r1);
                    shuffle(weaponAddrZ2, r1);
                    shuffle(itemAddrZ1, r1);
                    shuffle(itemAddrZ2, r1);
                }
                else if (intensity == 3)
                    shuffle(allItems, r1, false);

                // Need to make sure the Jailor's key is in an item store, or the acquisition of such key won't occur.
                for (int lnI = 0; lnI < 60; lnI++)
                {
                    if (romData[0x19fca + lnI] == 0x39)
                        jailorsClear = true;
                }
            }

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
                int count = 0;
                for (int lnJ = 0; lnJ < 6; lnJ++)
                {
                    if (!(monsterZones[lnJ] == 127 || monsterZones[lnJ] == 255))
                    {
                        if (monsterZones[lnJ] > 128)
                            sum += (monsterZones[lnJ] - 128);
                        else
                            sum += monsterZones[lnJ];
                        count++;
                    }
                }
                int average = (count == 0 ? 16 : sum / count);
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

            // rearrange "special bouts".  There are 19 "special bouts" that can be defined in each of the zones.
            for (int lnI = 0; lnI < 19; lnI++)
            {
                int byteToUse = 0x106b1 + (lnI * 4);
                byte[] monsterZones = { romData[byteToUse + 0], romData[byteToUse + 1], romData[byteToUse + 2], romData[byteToUse + 3] };
                int sum = 0;
                int count = 0;
                for (int lnJ = 0; lnJ < 4; lnJ++)
                {
                    if (!(monsterZones[lnJ] == 255))
                    {
                        sum += monsterZones[lnJ];
                        count++;
                    }
                        
                }
                int average = sum / count;
                for (int lnJ = 0; lnJ < 4; lnJ++)
                {
                    randomModifier = (intensity == 1 ? (r1.Next() % 5) - 2 : (intensity == 2 ? (r1.Next() % 9) - 4 : (r1.Next() % 17) - 8));
                    if (monsterZones[lnJ] == 255 && randomModifier != 0)
                    {
                        if (average + randomModifier > 0)
                            romData[byteToUse + lnJ] = (byte)(average + randomModifier);
                    }
                    else {
                        int test = (monsterZones[lnJ]);
                        if (test + randomModifier <= 0 || test + randomModifier >= 78) // You shouldn't randomly run into Atlas, Bazuzu, Zarlox, Hargon, or Malroth
                            romData[byteToUse + lnJ] = 255;
                        else
                            romData[byteToUse + lnJ] = (byte)(monsterZones[lnJ] + randomModifier);
                    }
                }

                // Assure there is at least one monster available in each zone.
                for (int lnJ = 0; lnJ < 4; lnJ++)
                {
                    if (romData[byteToUse + lnJ] != 255)
                        break;
                    else if (lnJ == 3)
                        romData[byteToUse + lnJ] = (byte)(average);
                }
            }

            // rearrange "boss fights".  There are 13 "boss fights".  Boss fights 9-13 will stay the same, except it might have more monsters showing up to make things even more fun!
            for (int lnI = 0; lnI < 13; lnI++)
            {
                int byteToUse = 0x10356 + (lnI * 4);
                // "Zone" order:  monster, quantity, monster, quantity.  The first pairing has priority...
                byte[] monsterZones = { romData[byteToUse + 0], romData[byteToUse + 2] };
                byte[] monsterQuantity = { romData[byteToUse + 3], romData[byteToUse + 1] }; // The two groups are going to be swapped mid-routine.
                int sum = ((monsterZones[0] == 255 ? 0 : monsterZones[0]) + (monsterZones[1] == 255 ? 0 : monsterZones[1]));
                int average = sum / ((monsterZones[0] == 255 ? 0 : 1) + (monsterZones[1] == 255 ? 0 : 1));

                if (lnI < 8)
                {
                    for (int lnJ = 0; lnJ < 2; lnJ++)
                    {
                        if (monsterZones[lnJ] == 255) continue;

                        randomModifier = (intensity == 1 ? (r1.Next() % 5) - 2 : (intensity == 2 ? (r1.Next() % 9) - 4 : (r1.Next() % 17) - 8));
                        int test = (monsterZones[lnJ]);
                        if (!(test + randomModifier <= 0 || test + randomModifier >= 78)) // You shouldn't randomly run into Atlas, Bazuzu, Zarlox, Hargon, or Malroth
                            romData[byteToUse + (lnJ * 2)] = (byte)(monsterZones[lnJ] + randomModifier);
                    }
                }

                // Make the main boss the first two bytes instead of the second, like all of the bouts programmed...
                swap(byteToUse + 2, byteToUse + 0);
                swap(byteToUse + 3, byteToUse + 1);

                // Now let's see if we can get a second group into the picture...
                for (int lnJ = 0; lnJ < 2; lnJ++)
                {
                    randomModifier = (intensity == 1 ? (r1.Next() % 5) - 2 : (intensity == 2 ? (r1.Next() % 9) - 4 : (r1.Next() % 17) - 8));
                    if (monsterQuantity[lnJ] + randomModifier < 1 && lnJ == 1) // only remove the second group.  NEVER remove the first.
                    {
                        romData[byteToUse + 2] = 255;
                        romData[byteToUse + 3] = 0;
                    }
                    else if (monsterQuantity[lnJ] + randomModifier < 1 && lnJ == 0)
                        romData[byteToUse + 1] = 1;
                    else if (monsterQuantity[lnJ] + randomModifier > monsterSize[monsterZones[1] - 1] && lnJ == 0) // monsterZones[1] is now the first entry, not the second as before.
                        // We have to max the first group in hopes to squeeze in a second.  We don't need to limit the second group... the battle code will take care of that.
                        romData[byteToUse + 1] = monsterSize[monsterZones[1] - 1]; 
                    else
                    {
                        romData[byteToUse + 1 + (lnJ * 2)] = (byte)(monsterQuantity[lnJ] + randomModifier);
                        if (lnJ == 1 && romData[byteToUse + 2] == 255)
                        { // We'll need to figure out a monster to join the others...
                            randomModifier = (intensity == 1 ? (r1.Next() % 5) - 2 : (intensity == 2 ? (r1.Next() % 9) - 4 : (r1.Next() % 17) - 8));
                            int test = (average + randomModifier);
                            if (test >= 1 && test <= 78) // You shouldn't randomly run into Atlas, Bazuzu, Zarlox, Hargon, or Malroth
                                romData[byteToUse + 2] = (byte)(test);
                            else
                                romData[byteToUse + 2] = (byte)(average - randomModifier); // Reverse direction of the modifier to guarantee a second group.
                        } else if (lnJ == 0 && romData[byteToUse + 2] == 255) // reswap to avoid three Bazuzus
                        {
                            swap(byteToUse + 2, byteToUse + 0);
                            swap(byteToUse + 3, byteToUse + 1);
                        }
                    }
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
                if (lnI == 4 || lnI == 8 || lnI == 15 || lnI == 20 || lnI == 24)
                    continue; // Spell #16 is also not learned.  Also, heal/healmore MUST be learned at level 1.
                randomModifier = (intensity == 1 ? (r1.Next() % 3) - 1 : (intensity == 2 ? (r1.Next() % 7) - 3 : (r1.Next() % 13) - 6));
                byte spellLevel = romData[0x13edb + lnI];
                if (spellLevel + randomModifier <= 0)
                    spellLevel = 1;
                else
                    spellLevel = (byte)(spellLevel + randomModifier);
                romData[0x13edb + lnI] = spellLevel;
            }

            for (int lnI = 0; lnI < 4; lnI++)
                for (int lnJ = lnI + 1; lnJ < 31; lnJ++)
                {
                    if (romData[0x13edb + lnJ] <= romData[0x13edb + lnI])
                    {
                        romData[0x13edb + lnJ] = (byte)(romData[0x13edb + lnI] + 1);
                        lnJ = lnI;
                    }
                }

            for (int lnI = 4; lnI < 8; lnI++)
                for (int lnJ = lnI + 1; lnJ < 15; lnJ++)
                {
                    if (romData[0x13edb + lnJ] <= romData[0x13edb + lnI])
                    {
                        romData[0x13edb + lnJ] = (byte)(romData[0x13edb + lnI] + 1);
                        lnJ = lnI;
                    }
                }

            for (int lnI = 8; lnI < 15; lnI++)
                for (int lnJ = lnI + 1; lnJ < 15; lnJ++)
                {
                    if (romData[0x13edb + lnJ] <= romData[0x13edb + lnI])
                    { 
                        romData[0x13edb + lnJ] = (byte)(romData[0x13edb + lnI] + 1);
                        lnJ = lnI;
                    }
                }

            for (int lnI = 16; lnI < 20; lnI++)
                for (int lnJ = lnI + 1; lnJ < 31; lnJ++)
                {
                    if (romData[0x13edb + lnJ] <= romData[0x13edb + lnI])
                    {
                        romData[0x13edb + lnJ] = (byte)(romData[0x13edb + lnI] + 1);
                        lnJ = lnI;
                    }
                }

            for (int lnI = 20; lnI < 24; lnI++)
                for (int lnJ = lnI + 1; lnJ < 31; lnJ++)
                {
                    if (romData[0x13edb + lnJ] <= romData[0x13edb + lnI])
                    {
                        romData[0x13edb + lnJ] = (byte)(romData[0x13edb + lnI] + 1);
                        lnJ = lnI;
                    }
                }

            for (int lnI = 24; lnI < 31; lnI++)
                for (int lnJ = lnI + 1; lnJ < 31; lnJ++)
                {
                    if (romData[0x13edb + lnJ] <= romData[0x13edb + lnI])
                    { 
                        romData[0x13edb + lnJ] = (byte)(romData[0x13edb + lnI] + 1);
                        lnJ = lnI;
                    }
                }

            for (int lnI = 0; lnI < 31; lnI++) // Don't exceed level 29 for any spell.  I'd rather not wait 65,536+ points to get neccessary spells.
            {
                if (lnI == 15) continue;
                if (romData[0x13edb + lnI] > 29)
                    romData[0x13edb + lnI] = 29;
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
                for (int lnI = 0; lnI < 10; lnI++)
                    compareComposeString("itemContents" + lnI.ToString("X2"), writer, 0x19f9a + 48 + (6 * lnI), 6);
                for (int lnI = 0; lnI < 68; lnI++)
                    compareComposeString("monsterZones" + lnI.ToString("X2"), writer, (0x10519 + (6 * lnI)), 6);
                for (int lnI = 0; lnI < 19; lnI++)
                    compareComposeString("monsterSpecial" + lnI.ToString("X2"), writer, (0x106b1 + (4 * lnI)), 4);
                for (int lnI = 0; lnI < 13; lnI++)
                    compareComposeString("monsterBoss" + lnI.ToString("X2"), writer, (0x10356 + (4 * lnI)), 4);
                compareComposeString("statStart", writer, 0x13dd1, 12);
                for (int lnI = 0; lnI < 35; lnI++)
                {
                    compareComposeString("statUps" + lnI.ToString(), writer, 0x13ddd + (6 * lnI), 6);
                }
                for (int lnI = 0; lnI < 10; lnI++)
                {
                    compareComposeString("statUps" + (lnI + 35).ToString(), writer, 0x13ddd + 210 + (4 * lnI), 4);
                }
                for (int lnI = 0; lnI < 5; lnI++)
                {
                    compareComposeString("statUps" + (lnI + 45).ToString(), writer, 0x13ddd + 250 + (2 * lnI), 2);
                }
                compareComposeString("spellLearning", writer, 0x13edb, 32);
                compareComposeString("spellsLearned", writer, 0x1ae76, 32);
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
                    writer.WriteLine(txtFileName.Text);
        }

        private void txtFileName_Leave(object sender, EventArgs e)
        {
            runChecksum();
        }
    }
}
