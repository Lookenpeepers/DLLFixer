using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Memory;
using System.Text.RegularExpressions;

namespace DLLFixer
{
    public class RuntimePatcher
    {
        BackgroundWorker BGW = new BackgroundWorker();
        Mem MCCMemory;
        HaloDLLFixer Main;
        public string Game = "-";
        long start = 0;
        long end = 0;
        bool InGame = false;
        List<Address> addresses = new List<Address>();
        ProcessModule _module;
        string output = "";
        byte[] file;
        public string HexFile = "";
        struct Address
        {
            public string name;
            public string address;
            public string value;
            public long offset;
            public int length;
        }
        
        public RuntimePatcher(Mem memory, HaloDLLFixer Form)
        {
            MCCMemory = memory;
            Main = Form;
            BGW.DoWork += BGW_DoWork;
            BGW.RunWorkerCompleted += BGW_RunWorkerCompleted;
        }

        private void BGW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Main.status.Text = "";
        }

        private void BGW_DoWork(object sender, DoWorkEventArgs e)
        {
            GetAddresses();
        }
        private int SearchAOB(string pattern)
        {
            pattern = pattern.Replace(" ", "").Replace("?",".");
            MatchCollection matches = Regex.Matches(HexFile, pattern);
            foreach(Match m in matches)
            {
                return (m.Index / 2);
            }
            return 0;
        }
        private string TimeStamp()
        {
            return "[" + System.DateTime.Now.ToString() + "] ";
        }
        long baseAddr = 0;
        private void GetAddresses()
        {
            output = "";
            addresses = new List<Address>();
            Address addr = new Address();
            string DLL = Main.tabControl1.SelectedTab.Name + ".dll";
            switch (DLL)
            {
                case "halo1.dll":
                    {
                        foreach (string item in Main.comboBox1.Items)
                        {
                            addr.name = item;
                            switch (item)
                            {
                                case "3RD PERSON":
                                    {
                                        addr.length = 36;
                                        try
                                        {
                                            long _a = MCCMemory.AoBScan(start, end, Games.HaloCE.ThirdPersonMask).Result.First();
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Disabled";
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            long _a = MCCMemory.AoBScan(start, end, Games.HaloCE.ThirdPersonMask).Result.First();
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4"); 
                                            addr.value = "Enabled";
                                            addresses.Add(addr);
                                        }
                                        break;
                                    }
                                case "BUMP POSSESSION":
                                    {
                                        addr.length = 1;
                                        //ALSO ADD JETPACK ADDRESS HERE
                                        try
                                        {
                                            long _a = (MCCMemory.AoBScan(start, end, Games.HaloCE.BumpPossessionMask, true).Result.First());
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Disabled";
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            long _a = (MCCMemory.AoBScan(start, end, Games.HaloCE.BumpPossessionPokedMask, true).Result.First());
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Enabled";
                                            addresses.Add(addr);
                                        }
                                        break;
                                    }
                                //case "JET PACK":
                                //    {
                                //        addr.length = 1;
                                //        try
                                //        {
                                //            long _a = (MCCMemory.AoBScan(start, end, Games.HaloCE.BumpPossessionMask, true).Result.First() - 0x1F);
                                //            addr.offset = _a - baseAddr;
                                //            addr.address = _a.ToString("X4");
                                //            addr.value = "Disabled";
                                //            addresses.Add(addr);
                                //        }
                                //        catch
                                //        {
                                //            long _a = (MCCMemory.AoBScan(start, end, Games.HaloCE.BumpPossessionPokedMask, true).Result.First() - 0x1F);
                                //            addr.offset = _a - baseAddr;
                                //            addr.address = _a.ToString("X4");
                                //            addr.value = "Enabled";
                                //            addresses.Add(addr);
                                //        }
                                //        break;
                                //    }
                                case "MEDUSA":
                                    {
                                        addr.length = 1;
                                        try
                                        {
                                            long _a = (MCCMemory.AoBScan(start, end, Games.HaloCE.MedusaMask, true).Result.First());
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Disabled";
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            long _a = (MCCMemory.AoBScan(start, end, Games.HaloCE.MedusaPokedMask, true).Result.First());
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Enabled";
                                            addresses.Add(addr);
                                        }
                                        break;
                                    }
                                case "PAN CAM":
                                    {
                                        addr.length = 1;
                                        try
                                        {
                                            long _a = MCCMemory.AoBScan(start, end, Games.HaloCE.PanCamMask, true).Result.First();
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Disabled";
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            long _a = MCCMemory.AoBScan(start, end, Games.HaloCE.PanCamPokedMask, true).Result.First();
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Enabled";
                                            addresses.Add(addr);
                                        }
                                        break;
                                    }
                                case "WIREFRAME":
                                    {
                                        addr.length = 1;
                                        try
                                        {
                                            long _a = (MCCMemory.AoBScan(start, end, Games.HaloCE.WireframeMask, true).Result.First() + 0x1E);
                                            addr.offset = _a - baseAddr;
                                            addr.address =_a.ToString("X4");
                                            addr.value = "Disabled";
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            long _a = (MCCMemory.AoBScan(start, end, Games.HaloCE.WireframeMask, true).Result.First() + 0x1E);
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Enabled";
                                            addresses.Add(addr);
                                        }
                                        break;
                                    }
                            }
                        }
                        break;
                    }
                case "groundhog.dll":
                    {
                        foreach (string item in Main.comboBox1.Items)
                        {
                            addr.name = item;
                            switch (item)
                            {
                                case "AI":
                                    {
                                        addr.length = 1;
                                        try
                                        {
                                            long _a = (MCCMemory.AoBScan(start, end, Games.Halo2A.AIMask).Result.First() + 3);
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Disabled";
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            long _a = (MCCMemory.AoBScan(start, end, Games.Halo2A.AIPokedMask).Result.First() + 3);
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Enabled";
                                            addresses.Add(addr);
                                        }
                                        break;
                                    }
                                case "3RD PERSON":
                                    {
                                        addr.length = 2;
                                        try
                                        {
                                            long _a = MCCMemory.AoBScan(start, end, Games.Halo2A.ThirdPersonMask).Result.First();
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Disabled";
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            long _a = MCCMemory.AoBScan(start, end, Games.Halo2A.ThirdPersonPokedMask).Result.First();
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Enabled";
                                            addresses.Add(addr);
                                        }
                                        break;
                                    }
                            }
                        }
                        break;
                    }
                case "halo3.dll":
                    {
                        foreach (string item in Main.comboBox1.Items)
                        {
                            addr.name = item;
                            switch (item)
                            {
                                case "AI":
                                    {
                                        addr.length = 1;
                                        try
                                        {
                                            long _a = (MCCMemory.AoBScan(start, end, Games.Halo3.AIMask).Result.First() + 3);
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Disabled";
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            long _a = (MCCMemory.AoBScan(start, end, Games.Halo3.AIPokedMask).Result.First() + 3);
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Enabled";
                                            addresses.Add(addr);
                                        }
                                        break;
                                    }
                                case "3RD PERSON":
                                    {
                                        addr.length = 2;
                                        try
                                        {
                                            long _a = MCCMemory.AoBScan(start, end, Games.Halo3.ThirdPersonMask).Result.First();
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Disabled";
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            long _a = MCCMemory.AoBScan(start, end, Games.Halo3.ThirdPersonPokedMask).Result.First();
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Enabled";
                                            addresses.Add(addr);
                                        }
                                        break;
                                    }
                            }
                        }
                        break;
                    }
                case "halo3odst.dll":
                    {
                        foreach (string item in Main.comboBox1.Items)
                        {
                            addr.name = item;
                            switch (item)
                            {
                                case "AI":
                                    {
                                        addr.length = 6;
                                        try
                                        {
                                            long _a = MCCMemory.AoBScan(start, end, Games.HaloODST.AIMask).Result.First() + 3;
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Disabled";
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            long _a = MCCMemory.AoBScan(start, end, Games.HaloODST.AIPokedMask).Result.First() + 3;
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Enabled";
                                            addresses.Add(addr);
                                        }
                                        break;
                                    }
                                case "3RD PERSON":
                                    {
                                        addr.length = 1;
                                        try
                                        {
                                            long _a = MCCMemory.AoBScan(start, end, Games.HaloODST.ThirdPersonMask).Result.First();
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Disabled";
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            long _a = MCCMemory.AoBScan(start, end, Games.HaloODST.ThirdPersonPokedMask).Result.First();
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Enabled";
                                            addresses.Add(addr);
                                        }
                                        break;
                                    }
                            }
                        }
                        break;
                    }
                case "haloreach.dll":
                    {
                        foreach (string item in Main.comboBox1.Items)
                        {
                            addr.name = item;
                            switch (item)
                            {
                                case "AI":
                                    {
                                        addr.length = 6;
                                        try
                                        {
                                            long _a = MCCMemory.AoBScan(start, end, Games.HaloReach.AIMask).Result.First() + 3;
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Disabled";
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            long _a = MCCMemory.AoBScan(start, end, Games.HaloReach.AIPokedMask).Result.First() + 3;
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Enabled";
                                            addresses.Add(addr);
                                        }
                                        break;
                                    }
                                case "3RD PERSON":
                                    {
                                        addr.length = 2;
                                        try
                                        {
                                            long _a = MCCMemory.AoBScan(start, end, Games.HaloReach.ThirdPersonMask).Result.First();
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Disabled";
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            long _a = MCCMemory.AoBScan(start, end, Games.HaloReach.ThirdPersonPokedMask).Result.First();
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Enabled";
                                            addresses.Add(addr);
                                        }
                                        break;
                                    }
                                case "PAN CAM":
                                    {
                                        addr.length = 19;
                                        try
                                        {
                                            long _a = MCCMemory.AoBScan(start, end, Games.HaloReach.PanCamMask).Result.First();
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Disabled";
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            long _a = MCCMemory.AoBScan(start, end, Games.HaloReach.PanCamPokedMask).Result.First();
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Enabled";
                                            addresses.Add(addr);
                                        }
                                        break;
                                    }
                                case "SCRIPTED AI IN MP":
                                    {
                                        addr.length = 2;
                                        try
                                        {
                                            long _a = MCCMemory.AoBScan(start, end, Games.HaloReach.ScriptedAIMask,true).Result.First();
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Disabled";
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            long _a = MCCMemory.AoBScan(start, end, Games.HaloReach.ScriptedAIPokedMask, true).Result.First();
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Enabled";
                                            addresses.Add(addr);
                                        }
                                        break;
                                    }
                                case "BETA RECOIL":
                                    {
                                        addr.length = 1;
                                        try
                                        {
                                            long _a = MCCMemory.AoBScan(start, end, Games.HaloReach.BetaRecoilMask).Result.First();
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Disabled";
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            long _a = MCCMemory.AoBScan(start, end, Games.HaloReach.BetaRecoilPokedMask).Result.First();
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Enabled";
                                            addresses.Add(addr);
                                        }
                                        break;
                                    }
                            }
                        }
                        break;
                    }
                case "halo4.dll":
                    {
                        foreach (string item in Main.comboBox1.Items)
                        {
                            addr.name = item;
                            switch (item)
                            {
                                case "SCRIPTED AI IN MP":
                                    {
                                        addr.length = 2;
                                        try
                                        {
                                            long _a = MCCMemory.AoBScan(start, end, Games.Halo4.AIMask).Result.First();
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Disabled";
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            long _a = MCCMemory.AoBScan(start, end, Games.Halo4.AIPokedMask).Result.First();
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Enabled";
                                            addresses.Add(addr);
                                        }
                                        break;
                                    }
                                case "3RD PERSON":
                                    {
                                        addr.length = 2;
                                        try
                                        {
                                            long _a = MCCMemory.AoBScan(start, end, Games.Halo4.ThirdPersonMask).Result.First();
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Disabled";
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            long _a = MCCMemory.AoBScan(start, end, Games.Halo4.ThirdPersonPokedMask).Result.First();
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Enabled";
                                            addresses.Add(addr);
                                        }
                                        break;
                                    }
                                case "CAMPAIGN THEATER":
                                    {
                                        addr.length = 3;
                                        try
                                        {
                                            long _a = MCCMemory.AoBScan(start, end, Games.Halo4.CampaignTheaterMask).Result.First();
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Disabled";
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            long _a = MCCMemory.AoBScan(start, end, Games.Halo4.CampaignTheaterPokedMask).Result.First();
                                            addr.offset = _a - baseAddr;
                                            addr.address = _a.ToString("X4");
                                            addr.value = "Enabled";
                                            addresses.Add(addr);
                                        }
                                        break;
                                    }
                            }
                        }
                        break;
                    }
            }
            Log();
            Main.Poke.Enabled = true;
            Main.UnPoke.Enabled = true;
            Main.outputLog.Text = output;
        }
        private void SetModule(ProcessModule Module)
        {
            Game = Module.ModuleName;
            start = (long)Module.BaseAddress;
            end = (long)Module.ModuleMemorySize + (long)Module.BaseAddress;
        }
        private bool IsInGame()
        {
            switch (Game)
            {
                case "halo1.dll":
                    {
                        //scan in game aob, if no result, not in game.
                        int results = MCCMemory.AoBScan(start, end, Games.HaloCE.InGameAOB, true).Result.Count();
                        if (results > 0)
                        {
                            return true;
                        }
                        break;
                    }
                case "groundhog.dll":
                    {
                        //scan in game aob, if no result, not in game.
                        int results = MCCMemory.AoBScan(start, end, Games.Halo2A.InGameAOB, true).Result.Count();
                        if (results > 0)
                        {
                            return true;
                        }
                        break;
                    }
                case "halo3.dll":
                    {
                        //scan in game aob, if no result, not in game.
                        int results = MCCMemory.AoBScan(start, end, Games.Halo3.InGameAOB, true).Result.Count();
                        if (results > 0)
                        {
                            return true;
                        }
                        break;
                    }
                case "halo3odst.dll":
                    {
                        //scan in game aob, if no result, not in game.
                        int results = MCCMemory.AoBScan(start, end, Games.HaloODST.InGameAOB, true).Result.Count();
                        if (results > 0)
                        {
                            return true;
                        }
                        break;
                    }
                case "haloreach.dll":
                    {
                        //scan in game aob, if no result, not in game.
                        int results = MCCMemory.AoBScan(start, end, Games.HaloReach.InGameAOB, true).Result.Count();
                        if (results > 0)
                        {
                            return true;
                        }
                        break;
                    }
                case "halo4.dll":
                    {
                        //scan in game aob, if no result, not in game.
                        int results = MCCMemory.AoBScan(start, end, Games.Halo4.InGameAOB, true).Result.Count();
                        if (results > 0)
                        {
                            return true;
                        }
                        break;
                    }
            }
            return false;
        }
        public void GetModule(string name)
        {
            bool gameFound = false;
            foreach (ProcessModule Module in MCCMemory.theProc.Modules)
            {
                if (Module.ModuleName == name)
                {
                    SetModule(Module);
                    gameFound = true;
                    InGame = true;
                    _module = Module;
                    baseAddr = (long)_module.BaseAddress;
                    break;
                }
                if (gameFound)
                {
                    //Main.outputLog.AppendText(TimeStamp() + "Detected game : " + Game + " Getting Addresses\n");
                    break;
                }
            }
            if (gameFound)
            {
                //CHECK IF IN GAME
                if (IsInGame())
                {
                    Main.status.Text = "Getting addresses...";
                    string fileName = Application.StartupPath + "\\Patches\\";
                    //check directory exists
                    if (Directory.Exists(fileName))
                    {
                        //check file exists
                        if (File.Exists(fileName + name))
                        {
                            //read all bytes
                            file = File.ReadAllBytes(fileName + name);
                            HexFile = BitConverter.ToString(file).Replace("-", "");
                        }
                        else
                        {
                            //file doesn't exist
                            DialogResult DR = MessageBox.Show("Would you like to create a patch for this dll?","",MessageBoxButtons.YesNo);
                            if (DR == DialogResult.Yes)
                            {
                                //attempt to copy file from MCC directory
                                if (Directory.Exists(Properties.Settings.Default.MCCPath))
                                {
                                    string MCCfilepath = Properties.Settings.Default.MCCPath + "\\" + name.Replace(".dll", "") + "\\" + name;
                                    if (File.Exists(MCCfilepath))
                                    {
                                        //copy into patch folder
                                        File.Copy(MCCfilepath, Application.StartupPath + "\\Patches\\" + name);
                                    }
                                }
                                else
                                {
                                    //prompt user to set MCC directory
                                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                                    fbd.Description = "Set MCC root directory ie. (C:\\Program Files (x86)\\Steam\\steamapps\\common\\Halo The Master Chief Collection)";
                                    fbd.ShowDialog();
                                    Properties.Settings.Default.MCCPath = fbd.SelectedPath;
                                    Properties.Settings.Default.Save();
                                }
                            }
                        }
                    }
                    
                    BGW.RunWorkerAsync();
                }
                else
                {
                    Main.outputLog.Text = "Not in Game";
                }
            }
            else
            {
                Main.outputLog.Text = (TimeStamp() + "Cannot find running instance of : " + name + "\n");
                Main.Poke.Enabled = false;
                Main.UnPoke.Enabled = false;
            }
        }
        private void PushToPatch(byte[] bytes,int off,string game,string Mask)
        {
            if (HexFile != "")
            {
                int offset = SearchAOB(Mask.Replace(" ", ""));
                if (offset != 0)
                {
                    List<byte> tmpFile = file.ToList();
                    for (var i = 0; i < bytes.Length; i++)
                    {
                        tmpFile[offset + off + i] = bytes.ElementAt(i);
                    }
                    File.WriteAllBytes(Application.StartupPath + "\\Patches\\" + game, tmpFile.ToArray());
                    file = File.ReadAllBytes(Application.StartupPath + "\\Patches\\" + game);
                    HexFile = BitConverter.ToString(file).Replace("-", "");
                }
            }
        }
        public void PokeDLL()
        {
            if (InGame)
            {
                Address _addr = addresses.Where(a => a.name == Main.comboBox1.Text).First();
                byte[] replacementBytes = new byte[] { };
                switch (Game)
                {
                    case "halo1.dll":
                        {
                            switch (_addr.name)
                            {
                                case "3RD PERSON":
                                    {
                                        replacementBytes = new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x48, 0x8B, 0x05, 0x4C, 0xB3, 0xF4, 0x01, 0x48, 0x8D, 0x54, 0x24, 0x48, 0x49, 0x6B, 0xCE, 0x54, 0x8B, 0x4C, 0x01, 0x10, 0xE8, 0x2A, 0xFB, 0xFF, 0xFF, 0x66, 0x05, 0x01, 0x00, 0x74 };
                                        PushToPatch(replacementBytes, 0, Game, Games.HaloCE.ThirdPersonMask);
                                        break;
                                    }
                                case "BUMP POSSESSION":
                                    {
                                        replacementBytes = new byte[] { 0x75 };
                                        PushToPatch(replacementBytes, 0, Game, Games.HaloCE.BumpPossessionMask);
                                        break;
                                    }
                                //case "JET PACK":
                                //    {
                                //        replacementBytes = new byte[] { 0x01 };
                                //        break;
                                //    }
                                case "MEDUSA":
                                    {
                                        replacementBytes = new byte[] { 0x75 };
                                        PushToPatch(replacementBytes, 0, Game, Games.HaloCE.MedusaMask);
                                        break;
                                    }
                                case "PAN CAM":
                                    {
                                        replacementBytes = new byte[] { 0x01 };
                                        break;
                                    }
                                case "WIREFRAME":
                                    {
                                        replacementBytes = new byte[] { 0x01 };
                                        break;
                                    }
                            }
                            break;
                        }
                    case "groundhog.dll":
                        {
                            switch (_addr.name)
                            {
                                case "AI":
                                    {
                                        replacementBytes = new byte[] { 0x90 };
                                        PushToPatch(replacementBytes, 3, Game, Games.Halo2A.AIMask);
                                        break;
                                    }
                                case "3RD PERSON":
                                    {
                                        replacementBytes = new byte[] { 0x90, 0x90 };
                                        PushToPatch(replacementBytes, 0, Game, Games.Halo2A.ThirdPersonMask);
                                        break;
                                    }
                            }
                            break;
                        }
                    case "halo3.dll":
                        {
                            switch (_addr.name)
                            {
                                case "AI":
                                    {
                                        replacementBytes = new byte[] { 0x90 };
                                        PushToPatch(replacementBytes,3,Game,Games.Halo3.AIMask);
                                        break;
                                    }
                                case "3RD PERSON":
                                    {
                                        replacementBytes = new byte[] { 0x90, 0x90 };
                                        PushToPatch(replacementBytes, 0, Game, Games.Halo3.ThirdPersonMask);
                                        break;
                                    }
                            }
                            break;
                        }
                    case "haloreach.dll":
                        {
                            switch (_addr.name)
                            {
                                case "AI":
                                    {
                                        replacementBytes = new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 };
                                        PushToPatch(replacementBytes, 3, Game, Games.HaloReach.AIMask);
                                        break;
                                    }
                                case "3RD PERSON":
                                    {
                                        replacementBytes = new byte[] { 0x90, 0x90 };
                                        PushToPatch(replacementBytes, 0, Game, Games.HaloReach.ThirdPersonMask);
                                        break;
                                    }
                                case "PAN CAM":
                                    {
                                        replacementBytes = new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x40, 0x38, 0xB9, 0x4A, 0x09, 0x00, 0x00, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, };
                                        PushToPatch(replacementBytes, 0, Game, Games.HaloReach.PanCamMask);
                                        break;
                                    }
                                case "SCRIPTED AI IN MP":
                                    {
                                        replacementBytes = new byte[] {  0x90, 0x90 };
                                        PushToPatch(replacementBytes, 0, Game, Games.HaloReach.ScriptedAIMask);
                                        break;
                                    }
                                case "BETA RECOIL":
                                    {
                                        replacementBytes = new byte[] { 0x85 };
                                        PushToPatch(replacementBytes, 0, Game, Games.HaloReach.BetaRecoilMask);
                                        break;
                                    }
                            }
                            break;
                        }
                    case "halo3odst.dll":
                        {
                            switch (_addr.name)
                            {
                                case "AI":
                                    {
                                        replacementBytes = new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 };
                                        PushToPatch(replacementBytes, 3, Game, Games.HaloODST.AIMask);
                                        break;
                                    }
                                case "3RD PERSON":
                                    {
                                        replacementBytes = new byte[] { 0xEB };
                                        PushToPatch(replacementBytes, 0, Game, Games.HaloODST.ThirdPersonMask);
                                        break;
                                    }
                            }
                            break;
                        }
                    case "halo4.dll":
                        {
                            switch (_addr.name)
                            {
                                case "SCRIPTED AI IN MP":
                                    {
                                        replacementBytes = new byte[] { 0x90, 0x90 };
                                        PushToPatch(replacementBytes, 0, Game, Games.Halo4.AIMask);
                                        break;
                                    }
                                case "3RD PERSON":
                                    {
                                        replacementBytes = new byte[] { 0x90, 0x90 };
                                        PushToPatch(replacementBytes, 0, Game, Games.Halo4.ThirdPersonMask);
                                        break;
                                    }
                                case "CAMPAIGN THEATER":
                                    {
                                        replacementBytes = new byte[] { 0x8B, 0xC7, 0x90 };
                                        PushToPatch(replacementBytes, 0, Game, Games.Halo4.CampaignTheaterMask);
                                        break;
                                    }
                            }
                            break;
                        }
                }
                if (replacementBytes.Length > 0)
                {
                    MCCMemory.WriteBytes(_addr.address, replacementBytes);
                    Address a = new Address();
                    a.offset = _addr.offset;
                    a.address = _addr.address;
                    a.name = _addr.name;
                    a.value = "Enabled";
                    int tmpIndex = addresses.IndexOf(_addr);
                    addresses.RemoveAt(tmpIndex);
                    addresses.Insert(tmpIndex, a);
                    Log();
                    //Main.status.Text = "Getting addresses...";
                    //BGW.RunWorkerAsync();
                    //Main.outputLog.AppendText(TimeStamp() + "New values successfully poked\n");
                }
            }
            //GetAddresses();
        }
        public void UnPokeDLL()
        {
            if (InGame)
            {
                Address _addr = addresses.Where(a => a.name == Main.comboBox1.Text).First();
                byte[] replacementBytes = new byte[] { };
                switch (Game)
                {
                    case "halo1.dll":
                        {
                            switch (_addr.name)
                            {
                                case "3RD PERSON":
                                    {
                                        replacementBytes = new byte[] { 0x0F, 0x85, 0x80, 0x00, 0x00, 0x00, 0x48, 0x8B, 0x05, 0x4C, 0xB3, 0xF4, 0x01, 0x48, 0x8D, 0x54, 0x24, 0x48, 0x49, 0x6B, 0xCE, 0x54, 0x8B, 0x4C, 0x01, 0x10, 0xE8, 0x2A, 0xFB, 0xFF, 0xFF, 0x66, 0x83, 0xF8, 0x01, 0x75 };
                                        PushToPatch(replacementBytes, 0, Game, Games.HaloCE.ThirdPersonPokedMask);
                                        break;
                                    }
                                case "BUMP POSSESSION":
                                    {
                                        replacementBytes = new byte[] { 0x74 };
                                        PushToPatch(replacementBytes, 0, Game, Games.HaloCE.BumpPossessionPokedMask);
                                        break;
                                    }
                                //case "JET PACK":
                                //    {
                                //        replacementBytes = new byte[] { 0x00 };
                                //        break;
                                //    }
                                case "MEDUSA":
                                    {
                                        replacementBytes = new byte[] { 0x74 };
                                        PushToPatch(replacementBytes, 0, Game, Games.HaloCE.MedusaPokedMask);
                                        break;
                                    }
                                case "PAN CAM":
                                    {
                                        replacementBytes = new byte[] { 0x00 };
                                        break;
                                    }
                                case "WIREFRAME":
                                    {
                                        replacementBytes = new byte[] { 0x00 };
                                        break;
                                    }
                            }
                            break;
                        }
                    case "groundhog.dll":
                        {
                            switch (_addr.name)
                            {
                                case "AI":
                                    {
                                        replacementBytes = new byte[] { 0x0F };
                                        PushToPatch(replacementBytes, 3, Game, Games.Halo2A.AIPokedMask);
                                        break;
                                    }
                                case "3RD PERSON":
                                    {
                                        replacementBytes = new byte[] { 0x74, 0x03 };
                                        PushToPatch(replacementBytes, 0, Game, Games.Halo2A.ThirdPersonPokedMask);
                                        break;
                                    }
                            }
                            break;
                        }
                    case "halo3.dll":
                        {
                            Main.outputLog.AppendText(_addr.address);
                            switch (_addr.name)
                            {
                                case "AI":
                                    {
                                        replacementBytes = new byte[] { 0x0F };
                                        PushToPatch(replacementBytes, 3, Game, Games.Halo3.AIPokedMask);
                                        break;
                                    }
                                case "3RD PERSON":
                                    {
                                        replacementBytes = new byte[] { 0x74, 0x0E };
                                        PushToPatch(replacementBytes, 3, Game, Games.Halo3.ThirdPersonPokedMask);
                                        break;
                                    }
                            }
                            break;
                        }
                    case "haloreach.dll":
                        {
                            switch (_addr.name)
                            {
                                case "AI":
                                    {
                                        replacementBytes = new byte[] { 0x0F, 0x84, 0xD5, 0x00, 0x00, 0x00 };
                                        PushToPatch(replacementBytes, 3, Game, Games.HaloReach.AIPokedMask);
                                        break;
                                    }
                                case "3RD PERSON":
                                    {
                                        replacementBytes = new byte[] { 0x74, 0x0E };
                                        PushToPatch(replacementBytes, 0, Game, Games.HaloReach.ThirdPersonPokedMask);
                                        break;
                                    }
                                case "PAN CAM":
                                    {
                                        replacementBytes = new byte[] { 0x0F, 0x84, 0xA4, 0x01, 0x00, 0x00, 0x40, 0x38, 0xB9, 0x4A, 0x09, 0x00, 0x00, 0x0F, 0x84, 0x97, 0x01, 0x00, 0x00, };
                                        PushToPatch(replacementBytes, 0, Game, Games.HaloReach.PanCamPokedMask);
                                        break;
                                    }
                                case "SCRIPTED AI IN MP":
                                    {
                                        replacementBytes = new byte[] { 0x00, 0x0B };
                                        PushToPatch(replacementBytes, 0, Game, Games.HaloReach.ScriptedAIPokedMask);
                                        break;
                                    }
                                case "BETA RECOIL":
                                    {
                                        replacementBytes = new byte[] { 0x84 };
                                        PushToPatch(replacementBytes, 0, Game, Games.HaloReach.BetaRecoilPokedMask);
                                        break;
                                    }
                            }
                            break;
                        }
                    case "halo3odst.dll":
                        {
                            switch (_addr.name)
                            {
                                case "AI":
                                    {
                                        replacementBytes = new byte[] { 0x0F, 0x84, 0xD5, 0x00, 0x00, 0x90 };
                                        PushToPatch(replacementBytes, 3, Game, Games.HaloODST.AIPokedMask);
                                        break;
                                    }
                                case "3RD PERSON":
                                    {
                                        replacementBytes = new byte[] { 0x75 };
                                        PushToPatch(replacementBytes, 0, Game, Games.HaloODST.ThirdPersonMask);
                                        break;
                                    }
                            }
                            break;
                        }
                    case "halo4.dll":
                        {
                            switch (_addr.name)
                            {
                                case "SCRIPTED AI IN MP":
                                    {
                                        replacementBytes = new byte[] { 0x74, 0x02 };
                                        PushToPatch(replacementBytes, 0, Game, Games.Halo4.AIPokedMask);
                                        break;
                                    }
                                case "3RD PERSON":
                                    {
                                        replacementBytes = new byte[] { 0x74, 0x03 };
                                        PushToPatch(replacementBytes, 0, Game, Games.Halo4.ThirdPersonPokedMask);
                                        break;
                                    }
                                case "CAMPAIGN THEATER":
                                    {
                                        replacementBytes = new byte[] { 0x0F, 0x44, 0xC7 };
                                        PushToPatch(replacementBytes, 0, Game, Games.Halo4.CampaignTheaterPokedMask);
                                        break;
                                    }
                            }
                            break;
                        }
                }
                if (replacementBytes.Length > 0)
                {
                    MCCMemory.WriteBytes(_addr.address, replacementBytes);
                    Address a = new Address();
                    a.offset = _addr.offset;
                    a.address = _addr.address;
                    a.name = _addr.name;
                    a.value = "Disabled";
                    int tmpIndex = addresses.IndexOf(_addr);
                    addresses.RemoveAt(tmpIndex);
                    addresses.Insert(tmpIndex, a);
                    Log();
                }
            }            
        }
        private void Log()
        {
            output = "";
            foreach(Address a in addresses)
            {
                output += "0x" + a.offset.ToString("X4") + " : " + a.address + " : " + a.value + " - " + a.name + "\n";
            }
            Main.outputLog.Text = output;
        }
    }
}