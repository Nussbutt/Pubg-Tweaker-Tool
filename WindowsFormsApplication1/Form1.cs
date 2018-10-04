// ===============================
// AUTHOR      :Nussbutt
// CREATE DATE :04/05/2018
// PURPOSE     :Allow Users to easily convert mouse sensitivities & alter settings for PUBG
// ===============================
// Change History: Everything works?
//
//==================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class formPutt : Form
    {
        //instantiate everything
        double normSens, tarSens, scopeSens, twoSens, threeSens, fourSens, sixSens, eightSens, fifteenSens, csSens, owSens, vertMult;
        int[] scopeLines = new int[9];
        int vertMultLine = -1;
        static string path = Environment.GetEnvironmentVariable("LocalAppData") + @"\TslGame\Saved\Config\WindowsNoEditor\GameUserSettings.ini";
        static string inputPath = Environment.GetEnvironmentVariable("LocalAppData") + @"\TslGame\Saved\Config\WindowsNoEditor\input.ini";
        Dictionary<string, int> whatLine;
        string[] mouseLine;
        string[] GUSetting_list = System.IO.File.ReadAllLines(path);
        string[] input_list = System.IO.File.ReadAllLines(inputPath);

        /**ComboBox Game
         * Change the sens depending on what game is selected.
         */
        private void cmbGame_SelectedIndexChanged(object sender, EventArgs e)
        {
            csSens = Math.Round(((normSens / (80 / double.Parse(Find_Val(whatLine["fpsFov"], GUSetting_list)))) * 101.0101), 3);
            if (cmbGame.SelectedIndex == 0)
            {
                txtSens.Text = csSens.ToString();
            }
            else
            {
                owSens = Math.Round((csSens / 30) * 100, 2);
                txtSens.Text = owSens.ToString();
            }
        }
        /**Convert Button
         * Converts input sens to pubg and displays them
         */
        private void btnConvert_Click(object sender, EventArgs e)
        {
            normSens = double.Parse(convertSens(0));
            lblNorm.Text = "------> " + normSens;

            tarSens = Math.Round(normSens * double.Parse(txtTarPer.Text) / 100, 6);
            lblEntTar.Text = "Targeting: " + tarSens + " <------";

            scopeSens = Math.Round(double.Parse(convertSens(1)) * (double.Parse(txtScope.Text) / 100), 6);
            lblEntScope.Text = "Scoping: " + scopeSens + " <------";

            twoSens = Math.Round(double.Parse(convertSens(2)) * (double.Parse(txtTwox.Text) / 100), 6);
            lblEntTwox.Text = "2x: " + twoSens + " <------";

            threeSens = Math.Round(double.Parse(convertSens(3)) * (double.Parse(txtThreex.Text) / 100), 6);
            lblEntThreex.Text = "3x: " + threeSens + " <------";

            fourSens = Math.Round(double.Parse(convertSens(4)) * (double.Parse(txtFourx.Text) / 100), 6);
            lblEntFourx.Text = "4x: " + fourSens + " <------";

            sixSens = Math.Round(double.Parse(convertSens(6)) * (double.Parse(txtSixx.Text) / 100), 6);
            lblEntSixx.Text = "6x: " + sixSens + " <------";

            eightSens = Math.Round(double.Parse(convertSens(8)) * (double.Parse(txtEightx.Text) / 100), 6);
            lblEntEightx.Text = "8x: " + eightSens + " <------";

            fifteenSens = Math.Round(double.Parse(convertSens(15)) * (double.Parse(txtFifteenx.Text) / 100), 6);
            lblEntFifteenx.Text = "15x: " + fifteenSens + " <------";
        }
        /**Save button
         * Writes edits to GameUserSettings.ini and Input.ini
         */
        private void btnSave_Click(object sender, EventArgs e)
        {
            //edit sensitivities
            string temp, tmpSens = "";
            for (int i = 0; i < 9; i++)
            {
                if (i == 0)
                    tmpSens = normSens.ToString();
                else if (i == 1)
                    tmpSens = tarSens.ToString();
                else if (i == 2)
                    tmpSens = scopeSens.ToString();
                else if (i == 3)
                    tmpSens = twoSens.ToString();
                else if (i == 4)
                    tmpSens = threeSens.ToString();
                else if (i == 5)
                    tmpSens = fourSens.ToString();
                else if (i == 6)
                    tmpSens = sixSens.ToString();
                else if (i == 7)
                    tmpSens = eightSens.ToString();
                else if (i == 8)
                    tmpSens = fifteenSens.ToString();
                //make sure the final string is always 8 characters before writeing it to the line
                tmpSens = (tmpSens + "000000").Substring(0, 8);
                temp = mouseLine[scopeLines[i]].Substring(0, mouseLine[scopeLines[i]].IndexOf('=') + 1) + tmpSens;
                temp = temp + mouseLine[scopeLines[i]].Substring(temp.IndexOf(tmpSens) + 8);
                mouseLine[scopeLines[i]] = temp;
            }
            //edit motion blur, vsync
            if(chkMblur.Checked == true)
                GUSetting_list[whatLine["mBlur"]] = GUSetting_list[whatLine["mBlur"]].Substring(0, GUSetting_list[whatLine["mBlur"]].IndexOf("=") + 1) + "False";
            else
                GUSetting_list[whatLine["mBlur"]] = GUSetting_list[whatLine["mBlur"]].Substring(0, GUSetting_list[whatLine["mBlur"]].IndexOf("=") + 1) + "True";
            if(chkVsync.Checked == true)
                GUSetting_list[whatLine["vSync"]] = GUSetting_list[whatLine["vSync"]].Substring(0, GUSetting_list[whatLine["vSync"]].IndexOf("=") + 1) + "False";
            else
                GUSetting_list[whatLine["vSync"]] = GUSetting_list[whatLine["vSync"]].Substring(0, GUSetting_list[whatLine["vSync"]].IndexOf("=") + 1) + "True";

            //edit AA, Post Processing, Shadows, Textures, Effects, Foliage, View Distance, ScreenScale
            GUSetting_list[whatLine["aaQual"]] = GUSetting_list[whatLine["aaQual"]].Substring(0, GUSetting_list[whatLine["aaQual"]].IndexOf("=") + 1) + cmbAA.SelectedIndex;
            GUSetting_list[whatLine["pProc"]] = GUSetting_list[whatLine["pProc"]].Substring(0, GUSetting_list[whatLine["pProc"]].IndexOf("=") + 1) + cmbPP.SelectedIndex;
            GUSetting_list[whatLine["shadow"]] = GUSetting_list[whatLine["shadow"]].Substring(0, GUSetting_list[whatLine["shadow"]].IndexOf("=") + 1) + cmbShadows.SelectedIndex;
            GUSetting_list[whatLine["tex"]] = GUSetting_list[whatLine["tex"]].Substring(0, GUSetting_list[whatLine["tex"]].IndexOf("=") + 1) + cmbTex.SelectedIndex;
            GUSetting_list[whatLine["eFX"]] = GUSetting_list[whatLine["eFX"]].Substring(0, GUSetting_list[whatLine["eFX"]].IndexOf("=") + 1) + cmbEfx.SelectedIndex;
            GUSetting_list[whatLine["grass"]] = GUSetting_list[whatLine["grass"]].Substring(0, GUSetting_list[whatLine["grass"]].IndexOf("=") + 1) + cmbFoli.SelectedIndex;
            GUSetting_list[whatLine["vDist"]] = GUSetting_list[whatLine["vDist"]].Substring(0, GUSetting_list[whatLine["vDist"]].IndexOf("=") + 1) + cmbVdist.SelectedIndex;

            GUSetting_list[whatLine["scale"]] = GUSetting_list[whatLine["scale"]].Substring(0, GUSetting_list[whatLine["scale"]].IndexOf("=") + 1) + txtScale.Text;

            //new Vert Fix
            //if (chkVert.Checked == true)
            //    mouseLine[vertMultLine] = mouseLine[vertMultLine].Substring(0, mouseLine[vertMultLine].IndexOf("=") + 1) + "1.000000";
            //else
            //    mouseLine[vertMultLine] = mouseLine[vertMultLine].Substring(0, mouseLine[vertMultLine].IndexOf("=") + 1) + "0.700000";

            //join the mouse lines back
            var newLine = string.Join(",", mouseLine);
            GUSetting_list[whatLine["mouseSens"]] = newLine;

            //Warn Users about going over 103 FoV
            DialogResult dialogResult;
            if (double.Parse(txtFov.Text) > 103)
            {
                dialogResult = MessageBox.Show("Using an Fov above 103 may get you banned.\nWould you like to continue?", "Warning!", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    GUSetting_list[whatLine["fpsFov"]] = GUSetting_list[whatLine["fpsFov"]].Substring(0, GUSetting_list[whatLine["fpsFov"]].IndexOf("=") + 1) + txtFov.Text;
                }
                else if (dialogResult == DialogResult.No)
                {
                    MessageBox.Show("Setting Fov to 103", "Good choice.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    GUSetting_list[whatLine["fpsFov"]] = GUSetting_list[whatLine["fpsFov"]].Substring(0, GUSetting_list[whatLine["fpsFov"]].IndexOf("=") + 1) + "103";
                }
            }
            else
                GUSetting_list[whatLine["fpsFov"]] = GUSetting_list[whatLine["fpsFov"]].Substring(0, GUSetting_list[whatLine["fpsFov"]].IndexOf("=") + 1) + txtFov.Text;

            //Save the files

            /** OLD VERT SENS FIX
            //Input file for verical sensitivity fix
            string[] newList = { "1", "2", "3", "4", "5", "6"};
            if (chkVert.Checked == true)
            {
                newList = new string[] 
                { "[/Script/Engine.InputSettings]" , "AxisMappings=(AxisName=\"Turn\",Key=MouseX,Scale=1.000000)", "AxisMappings=(AxisName=\"LookUp\",Key=MouseY,Scale=-1.428571)", "AxisMappings=(AxisName=\"WorldMapZoom\",Key=MouseWheelAxis,Scale=1.000000)", "AxisMappings=(AxisName=\"UI_MapMoveX\",Key=MouseX,Scale=1.000000)", "AxisMappings=(AxisName=\"UI_MapMoveY\",Key=MouseY,Scale=-1.000000)"};
            }
            else
            {
                newList = new string[]{ " " };
            }
            input_list = newList;
            */

            //EMPTY input.ini
            input_list = new string[] { " " }; ;

            //Make GameUserSettings writeable, write to GameUserSettings
            //Make Input writeable, write to Input, make Input read only again
            System.IO.File.SetAttributes(path, System.IO.FileAttributes.Normal);
            System.IO.File.WriteAllLines(path, GUSetting_list);
            System.IO.File.SetAttributes(inputPath, System.IO.FileAttributes.Normal);
            System.IO.File.WriteAllLines(inputPath, input_list);
            System.IO.File.SetAttributes(inputPath, System.IO.FileAttributes.ReadOnly);
            //Make GameUserSettings read-only if the user wants
            dialogResult = MessageBox.Show("Would you like to make the file read-only?\n\n(Prevents game from overwriteing any modified settings)", "", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                System.IO.File.SetAttributes(path, System.IO.FileAttributes.ReadOnly);
            }
            else if (dialogResult == DialogResult.No)
            {
                System.IO.File.SetAttributes(path, System.IO.FileAttributes.Normal);
            }
            MessageBox.Show("Success!!!", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        

        /**Find_Val method - Returns string, takes int position & list
         * Finds "=" character in given line and returns what comes after it
         */
        public string Find_Val(int pos, string[] list)
        {
            string value = "0.00";
            if (pos == -1)
            {
                Console.WriteLine("ERRRRRORRRRRRR - FIX IT!");
            }
            else
                value = list[pos].Substring(list[pos].IndexOf("=") + 1);
            return value;
        }
        /**Find_lastConvSens - returns int position, takes int position
         * checks any of the 3 lines ahead of current contain LastConvertedMouseSens, returns that position (or -1 if not found)
         */
        public int Find_lastConvSens(int curLine)
        {
            for (int i = curLine; i <= curLine + 3; i++)
            {
                if(mouseLine[i].Contains("LastConvertedSensitivity"))
                    return i;
            }
            return -1;
        }
        /**ConvertSens - returns string, takes int (scope multiple)
         * if its no scope(0) use specific formula considering FOV
         * if the scope is anything else use formula not considering fov (FOV is locked when ads/Scoped)
         */
        public string convertSens(int isScope)
        {
            double newSens;
            decimal value;
            //CS or overwatch?
            if (cmbGame.SelectedIndex == 0)
                newSens = double.Parse(txtSens.Text);
            else
                newSens = (double.Parse(txtSens.Text) / 100) * 30;
            //what scope
            if (isScope == 0)
                newSens = (newSens / 101.0101) * (80/double.Parse(txtFov.Text));
            else
                newSens = newSens / 88.33;
            return (newSens.ToString() + "00000000").Substring(0, 8);
        }

        public formPutt()
        {
            InitializeComponent();
            Load += new EventHandler(Form1_Load);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        /**Onload
         * Read and display important information
         */
        private void Form1_Load(object sender, EventArgs e)
        {
            whatLine = new Dictionary<string, int>();
            //load file into list, check for...
            //fov, mouse sens(more to do with that), aaqual, blur, vsync, postprocess, shadow, tex, fx, grass, vdist, scale
            int numSeq = 0;
            while (GUSetting_list.Length > numSeq)
            {
                //ignore TslPersistantData! *Quick duplicate key fix*
                if (GUSetting_list[numSeq].Contains("TslPersistantData"))
                {
                    Console.WriteLine(GUSetting_list[numSeq]);
                    GUSetting_list[numSeq] = "";
                }


                if (GUSetting_list[numSeq].Contains("CustomInputSettins"))
                    whatLine.Add("mouseSens", numSeq);
                else if (GUSetting_list[numSeq].Contains("FpsCameraFov"))
                    whatLine.Add("fpsFov", numSeq);
                else if (GUSetting_list[numSeq].Contains("bMotionBlur"))
                    whatLine.Add("mBlur", numSeq);
                else if (GUSetting_list[numSeq].Contains("bUseVSync"))
                    whatLine.Add("vSync", numSeq);
                else if (GUSetting_list[numSeq].Contains("sg.AntiAliasingQuality"))
                    whatLine.Add("aaQual", numSeq);
                else if (GUSetting_list[numSeq].Contains("sg.PostProcessQuality"))
                    whatLine.Add("pProc", numSeq);
                else if (GUSetting_list[numSeq].Contains("sg.ShadowQuality"))
                    whatLine.Add("shadow", numSeq);
                else if (GUSetting_list[numSeq].Contains("sg.TextureQuality"))
                    whatLine.Add("tex", numSeq);
                else if (GUSetting_list[numSeq].Contains("sg.EffectsQuality"))
                    whatLine.Add("eFX", numSeq);
                else if (GUSetting_list[numSeq].Contains("sg.FoliageQuality"))
                    whatLine.Add("grass", numSeq);
                else if (GUSetting_list[numSeq].Contains("sg.ViewDistanceQuality"))
                    whatLine.Add("vDist", numSeq);
                else if (GUSetting_list[numSeq].Contains("ScreenScale"))
                    whatLine.Add("scale", numSeq);                
                numSeq++;
            }
            /**--------YE OLD VERT CHECK
            //check for input fix
            numSeq = 0;
            while (input_list.Length > numSeq)
            {
                if (input_list[numSeq].Contains("Key=MouseY,Scale"))
                    chkVert.Checked = true;
                else
                    chkVert.Checked = false;

                numSeq++;
            }
            */
            if (vertMult == 1)
                chkVert.Checked = true;
            else
                chkVert.Checked = false;

            //split the mouse line stuff, makes it easier to read and write
            //make sure to save what line corosponds to each scope sensitivitiy
            numSeq = 0;
            mouseLine = GUSetting_list[whatLine["mouseSens"]].Split(',');

            while(mouseLine.Length > numSeq)
            {
                if (mouseLine[numSeq].Contains("(GamePad"))
                    numSeq = mouseLine.Length - 1;

                if (mouseLine[numSeq].Contains("SensitiveName=\"Normal\""))
                {
                    scopeLines[0] = Find_lastConvSens(numSeq);
                    string temp = Find_Val(scopeLines[0], mouseLine);
                    normSens = double.Parse(new string(temp.Where(c => !char.Equals(c, ')')).ToArray()));
                    lblNorm.Text = "------> " + normSens;
                }
                else if (mouseLine[numSeq].Contains("SensitiveName=\"Targeting\""))
                {
                    scopeLines[1] = Find_lastConvSens(numSeq);
                    string temp = Find_Val(scopeLines[1], mouseLine);
                    //this displays every character from temp that isnt ')' as a string then converts it into a double
                    tarSens = double.Parse(new string(temp.Where(c => !char.Equals(c,')')).ToArray()));
                    lblEntTar.Text = "Targeting: " + tarSens + " <------";
                }
                else if (mouseLine[numSeq].Contains("SensitiveName=\"Scoping\""))
                {
                    scopeLines[2] = Find_lastConvSens(numSeq);
                    string temp = Find_Val(scopeLines[2], mouseLine);
                    scopeSens = double.Parse(new string(temp.Where(c => !char.Equals(c, ')')).ToArray()));
                    lblEntScope.Text = "Scoping: " + scopeSens + " <------";
                }
                else if (mouseLine[numSeq].Contains("SensitiveName=\"Scope2X\""))
                {
                    scopeLines[3] = Find_lastConvSens(numSeq);
                    string temp = Find_Val(scopeLines[3], mouseLine);
                    twoSens = double.Parse(new string(temp.Where(c => !char.Equals(c, ')')).ToArray()));
                    lblEntTwox.Text = "2x: " + twoSens + " <------";
                }
                else if (mouseLine[numSeq].Contains("SensitiveName=\"Scope3X\""))
                {
                    scopeLines[4] = Find_lastConvSens(numSeq);
                    string temp = Find_Val(scopeLines[4], mouseLine);
                    threeSens = double.Parse(new string(temp.Where(c => !char.Equals(c, ')')).ToArray()));
                    lblEntThreex.Text = "3x: " + threeSens + " <------";
                }
                else if (mouseLine[numSeq].Contains("SensitiveName=\"Scope4X\""))
                {
                    scopeLines[5] = Find_lastConvSens(numSeq);
                    string temp = Find_Val(scopeLines[5], mouseLine);
                    fourSens = double.Parse(new string(temp.Where(c => !char.Equals(c, ')')).ToArray()));
                    lblEntFourx.Text = "4x: " + fourSens + " <------";
                }
                else if (mouseLine[numSeq].Contains("SensitiveName=\"Scope6X\""))
                {
                    scopeLines[6] = Find_lastConvSens(numSeq);
                    string temp = Find_Val(scopeLines[6], mouseLine);
                    sixSens = double.Parse(new string(temp.Where(c => !char.Equals(c, ')')).ToArray()));
                    lblEntSixx.Text = "6x: " + sixSens + " <------";
                }
                else if (mouseLine[numSeq].Contains("SensitiveName=\"Scope8X\""))
                {
                    scopeLines[7] = Find_lastConvSens(numSeq);
                    string temp = Find_Val(scopeLines[7], mouseLine);
                    eightSens = double.Parse(new string(temp.Where(c => !char.Equals(c, ')')).ToArray()));
                    lblEntEightx.Text = "8x: " + eightSens + " <------";
                }
                else if (mouseLine[numSeq].Contains("SensitiveName=\"Scope15X\""))
                {
                    scopeLines[8] = Find_lastConvSens(numSeq);
                    string temp = Find_Val(scopeLines[8], mouseLine);
                    fifteenSens = double.Parse(new string(temp.Where(c => !char.Equals(c,')')).ToArray()));
                    lblEntFifteenx.Text = "15x: " + fifteenSens + " <------";
                }
                
                //NEW vert sens
                else if (mouseLine[numSeq].Contains("MouseVerticalSensitivityMultiplier"))
                {
                    vertMultLine = numSeq;
                    string temp = Find_Val(vertMultLine, mouseLine);
                    vertMult = double.Parse(temp);
                    //IF vertMult is not 1.428571 fix is not applied
                }
                numSeq++;
            }
            
            //read/display settings
            txtFov.Text = Find_Val(whatLine["fpsFov"], GUSetting_list);

            if (Find_Val(whatLine["mBlur"], GUSetting_list).Contains("False"))
                chkMblur.Checked = true;
            if (Find_Val(whatLine["vSync"], GUSetting_list).Contains("False"))
                chkVsync.Checked = true;

            cmbAA.SelectedIndex = int.Parse(Find_Val(whatLine["aaQual"], GUSetting_list));
            cmbPP.SelectedIndex = int.Parse(Find_Val(whatLine["pProc"], GUSetting_list));
            cmbShadows.SelectedIndex = int.Parse(Find_Val(whatLine["shadow"], GUSetting_list));
            cmbTex.SelectedIndex = int.Parse(Find_Val(whatLine["tex"], GUSetting_list));
            cmbEfx.SelectedIndex = int.Parse(Find_Val(whatLine["eFX"], GUSetting_list));
            cmbFoli.SelectedIndex = int.Parse(Find_Val(whatLine["grass"], GUSetting_list));
            cmbVdist.SelectedIndex = int.Parse(Find_Val(whatLine["vDist"], GUSetting_list));
            txtScale.Text = Find_Val(whatLine["scale"], GUSetting_list);
            
            cmbGame.SelectedIndex = 0;
            //calculate CS and OW sens from current sensitivity
            csSens = Math.Round(((normSens / (80 / double.Parse(Find_Val(whatLine["fpsFov"], GUSetting_list)))) * 101.0101),3);
            owSens = Math.Round((csSens / 30) * 100, 2);
            txtSens.Text = csSens.ToString();

        }
    }
}
