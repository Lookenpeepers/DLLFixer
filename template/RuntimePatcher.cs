using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Memory;

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
        struct Address
        {
            public string name;
            public string address;
            public string value;
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

        public byte[] SaveBytes(string Game)
        {
            byte[] output = new byte[] { };
            switch (Game)
            {
                case "halo1.dll":
                    {
                        output = MCCMemory.ReadBytes(_module.BaseAddress.ToString("X4"), Games.HaloCE.fileSize);
                        break;
                    }
                case "groundhog.dll":
                    {
                        output = MCCMemory.ReadBytes(_module.BaseAddress.ToString("X4"), Games.Halo2A.fileSize);
                        break;
                    }
                case "halo3.dll":
                    {
                        output = MCCMemory.ReadBytes(_module.BaseAddress.ToString("X4"), Games.Halo3.fileSize);
                        break;
                    }
                case "halo3odst.dll":
                    {
                        output = MCCMemory.ReadBytes(_module.BaseAddress.ToString("X4"), Games.HaloODST.fileSize);
                        break;
                    }
                case "haloreach.dll":
                    {
                        output = MCCMemory.ReadBytes(_module.BaseAddress.ToString("X4"), Games.HaloReach.fileSize);
                        break;
                    }
                case "halo4.dll":
                    {
                        output = MCCMemory.ReadBytes(_module.BaseAddress.ToString("X4"), Games.Halo4.fileSize);
                        break;
                    }
            }
            return output;
        }
        private string TimeStamp()
        {
            return "[" + System.DateTime.Now.ToString() + "] ";
        }
        private void GetAddresses()
        {
            output = "";
            addresses = new List<Address>();
            Address addr = new Address();
            switch (Main.tabControl1.SelectedTab.Name + ".dll")
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
                                        try
                                        {
                                            addr.address = MCCMemory.AoBScan(start, end, Games.HaloCE.ThirdPersonMask).Result.First().ToString("X4");
                                            addr.value = "Disabled";
                                           // output += (addr.address + " : Disabled - " + item + "\n");
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            addr.address = MCCMemory.AoBScan(start, end, Games.HaloCE.ThirdPersonPokedMask).Result.First().ToString("X4");
                                            addr.value = "Enabled";
                                            //output += (addr.address + " : Enabled - " + item + "\n");
                                            addresses.Add(addr);
                                        }
                                        break;
                                    }
                                case "BUMP POSSESSION":
                                    {
                                        //ALSO ADD JETPACK ADDRESS HERE
                                        try
                                        {
                                            addr.address = (MCCMemory.AoBScan(start, end, Games.HaloCE.BumpPossessionMask,true).Result.First()-0x1D).ToString("X4");
                                            addr.value = "Disabled";
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            addr.address = (MCCMemory.AoBScan(start, end, Games.HaloCE.BumpPossessionPokedMask,true).Result.First()-0x1D).ToString("X4");
                                            addr.value = "Enabled";
                                            addresses.Add(addr);
                                        }
                                        break;
                                    }
                                case "JET PACK":
                                    {
                                        try
                                        {
                                            addr.address = (MCCMemory.AoBScan(start, end, Games.HaloCE.BumpPossessionMask, true).Result.First() - 0x1F).ToString("X4");
                                            addr.value = "Disabled";
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            addr.address = (MCCMemory.AoBScan(start, end, Games.HaloCE.BumpPossessionPokedMask, true).Result.First() - 0x1F).ToString("X4");
                                            addr.value = "Enabled";
                                            addresses.Add(addr);
                                        }
                                        break;
                                    }
                                case "MEDUSA":
                                    {
                                        try
                                        {
                                            addr.address = (MCCMemory.AoBScan(start, end, Games.HaloCE.BumpPossessionMask, true).Result.First() - 0x1A).ToString("X4");
                                            addr.value = "Disabled";
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            addr.address = (MCCMemory.AoBScan(start, end, Games.HaloCE.BumpPossessionPokedMask, true).Result.First() - 0x1A).ToString("X4");
                                            addr.value = "Enabled";
                                            addresses.Add(addr);
                                        }
                                        break;
                                    }
                                case "PAN CAM":
                                    {
                                        try
                                        {
                                            addr.address = MCCMemory.AoBScan(start, end, Games.HaloCE.PanCamMask, true).Result.First().ToString("X4");
                                            addr.value = "Disabled";
                                            //   output += (addr.address + " : Disabled - " + item + "\n");
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            addr.address = MCCMemory.AoBScan(start, end, Games.HaloCE.PanCamPokedMask, true).Result.First().ToString("X4");
                                            addr.value = "Enabled";
                                            //    output += (addr.address + " : Enabled - " + item + "\n");
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
                                        try
                                        {
                                            addr.address = (MCCMemory.AoBScan(start, end, Games.Halo2A.AIMask).Result.First() + 3).ToString("X4");
                                            addr.value = "Disabled";
                                        //    output += addr.address + " : Disabled - " + item + "\n";
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            addr.address = (MCCMemory.AoBScan(start, end, Games.Halo2A.AIPokedMask).Result.First() + 3).ToString("X4");
                                            addr.value = "Enabled";
                                         //   output += (addr.address + " : Enabled - " + item + "\n");
                                            addresses.Add(addr);
                                        }
                                        break;
                                    }
                                case "3RD PERSON":
                                    {
                                        try
                                        {
                                            addr.address = MCCMemory.AoBScan(start, end, Games.Halo2A.ThirdPersonMask).Result.First().ToString("X4");
                                            addr.value = "Disabled";
                                         //   output += (addr.address + " : Disabled - " + item + "\n");
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            addr.address = MCCMemory.AoBScan(start, end, Games.Halo2A.ThirdPersonPokedMask).Result.First().ToString("X4");
                                            addr.value = "Enabled";
                                        //    output += (addr.address + " : Enabled - " + item + "\n");
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
                                        try
                                        {
                                            addr.address = (MCCMemory.AoBScan(start, end, Games.Halo3.AIMask).Result.First() + 3).ToString("X4");
                                            addr.value = "Disabled";
                                       //     output += (addr.address + " : Disabled - " + item + "\n");
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            addr.address = (MCCMemory.AoBScan(start, end, Games.Halo3.AIPokedMask).Result.First() + 3).ToString("X4");
                                            addr.value = "Enabled";
                                        //    output += (addr.address + " : Enabled - " + item + "\n");
                                            addresses.Add(addr);
                                        }
                                        break;
                                    }
                                case "3RD PERSON":
                                    {
                                        try
                                        {
                                            addr.address = MCCMemory.AoBScan(start, end, Games.Halo3.ThirdPersonMask).Result.First().ToString("X4");
                                            addr.value = "Disabled";
                                        //    output += (addr.address + " : Disabled - " + item + "\n");
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            addr.address = MCCMemory.AoBScan(start, end, Games.Halo3.ThirdPersonPokedMask).Result.First().ToString("X4");
                                            addr.value = "Enabled";
                                         //   output += (addr.address + " : Enabled - " + item + "\n");
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
                                        try
                                        {
                                            addr.address = (MCCMemory.AoBScan(start, end, Games.HaloODST.AIMask).Result.First() + 3).ToString("X4");
                                            addr.value = "Disabled";
                                          //  output += (addr.address + " : Disabled - " + item + "\n");
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            addr.address = (MCCMemory.AoBScan(start, end, Games.HaloODST.AIPokedMask).Result.First() + 3).ToString("X4");
                                            addr.value = "Enabled";
                                           // output += (addr.address + " : Enabled - " + item + "\n");
                                            addresses.Add(addr);
                                        }
                                        break;
                                    }
                                case "3RD PERSON":
                                    {
                                        try
                                        {
                                            addr.address = MCCMemory.AoBScan(start, end, Games.HaloODST.ThirdPersonMask).Result.First().ToString("X4");
                                            addr.value = "Disabled";
                                           // output += (addr.address + " : Disabled - " + item + "\n");
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            addr.address = MCCMemory.AoBScan(start, end, Games.HaloODST.ThirdPersonPokedMask).Result.First().ToString("X4");
                                            addr.value = "Enabled";
                                          //  output += (addr.address + " : Enabled - " + item + "\n");
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
                                        try
                                        {
                                            addr.address = (MCCMemory.AoBScan(start, end, Games.HaloReach.AIMask).Result.First() + 3).ToString("X4");
                                            addr.value = "Disabled";
                                           // output += (addr.address + " : Disabled - " + item + "\n");
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            addr.address = (MCCMemory.AoBScan(start, end, Games.HaloReach.AIPokedMask).Result.First() + 3).ToString("X4");
                                            addr.value = "Enabled";
                                          //  output += (addr.address + " : Enabled - " + item + "\n");
                                            addresses.Add(addr);
                                        }
                                        break;
                                    }
                                case "3RD PERSON":
                                    {
                                        try
                                        {
                                            addr.address = MCCMemory.AoBScan(start, end, Games.HaloReach.ThirdPersonMask).Result.First().ToString("X4");
                                            addr.value = "Disabled";
                                         //   output += (addr.address + " : Disabled - " + item + "\n");
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            addr.address = MCCMemory.AoBScan(start, end, Games.HaloReach.ThirdPersonPokedMask).Result.First().ToString("X4");
                                            addr.value = "Enabled";
                                           // output += (addr.address + " : Enabled - " + item + "\n");
                                            addresses.Add(addr);
                                        }
                                        break;
                                    }
                                case "PAN CAM":
                                    {
                                        try
                                        {
                                            addr.address = MCCMemory.AoBScan(start, end, Games.HaloReach.PanCamMask).Result.First().ToString("X4");
                                            addr.value = "Disabled";
                                            output += (addr.address + " : Disabled - " + item + "\n");
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            addr.address = MCCMemory.AoBScan(start, end, Games.HaloReach.PanCamPokedMask).Result.First().ToString("X4");
                                            addr.value = "Enabled";
                                          //  output += (addr.address + " : Enabled - " + item + "\n");
                                            addresses.Add(addr);
                                        }
                                        break;
                                    }
                                case "SCRIPTED AI IN MP":
                                    {
                                        try
                                        {
                                            addr.address = MCCMemory.AoBScan(start, end, Games.HaloReach.ScriptedAIMask,true).Result.First().ToString("X4");
                                            addr.value = "Disabled";
                                          //  output += (addr.address + " : Disabled - " + item + "\n");
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            addr.address = MCCMemory.AoBScan(start, end, Games.HaloReach.ScriptedAIPokedMask,true).Result.First().ToString("X4");
                                            addr.value = "Enabled";
                                           // output += (addr.address + " : Enabled - " + item + "\n");
                                            addresses.Add(addr);
                                        }
                                        break;
                                    }
                                case "BETA RECOIL":
                                    {
                                        try
                                        {
                                            addr.address = MCCMemory.AoBScan(start, end, Games.HaloReach.BetaRecoilMask).Result.First().ToString("X4");
                                            addr.value = "Disabled";
                                            //output += (addr.address + " : Disabled - " + item + "\n");
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            addr.address = MCCMemory.AoBScan(start, end, Games.HaloReach.BetaRecoilPokedMask).Result.First().ToString("X4");
                                            addr.value = "Enabled";
                                           // output += (addr.address + " : Enabled - " + item + "\n");
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
                                        try
                                        {
                                            addr.address = (MCCMemory.AoBScan(start, end, Games.Halo4.AIMask).Result.First()).ToString("X4");
                                            addr.value = "Disabled";
                                           // output += (addr.address + " : Disabled - " + item + "\n");
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            addr.address = (MCCMemory.AoBScan(start, end, Games.Halo4.AIPokedMask).Result.First()).ToString("X4");
                                            addr.value = "Enabled";
                                           // output += (addr.address + " : Enabled - " + item + "\n");
                                            addresses.Add(addr);
                                        }
                                        break;
                                    }
                                case "3RD PERSON":
                                    {
                                        try
                                        {
                                            addr.address = MCCMemory.AoBScan(start, end, Games.Halo4.ThirdPersonMask).Result.First().ToString("X4");
                                            addr.value = "Disabled";
                                           // output += (addr.address + " : Disabled - " + item + "\n");
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            addr.address = MCCMemory.AoBScan(start, end, Games.Halo4.ThirdPersonPokedMask).Result.First().ToString("X4");
                                            addr.value = "Enabled";
                                            //output += (addr.address + " : Enabled - " + item + "\n");
                                            addresses.Add(addr);
                                        }
                                        break;
                                    }
                                case "CAMPAIGN THEATER":
                                    {
                                        try
                                        {
                                            addr.address = MCCMemory.AoBScan(start, end, Games.Halo4.CampaignTheaterMask).Result.First().ToString("X4");
                                            addr.value = "Disabled";
                                            //output += (addr.address + " : Disabled - " + item + "\n");
                                            addresses.Add(addr);
                                        }
                                        catch
                                        {
                                            addr.address = MCCMemory.AoBScan(start, end, Games.Halo4.CampaignTheaterPokedMask).Result.First().ToString("X4");
                                            addr.value = "Enabled";
                                            //output += (addr.address + " : Enabled - " + item + "\n");
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
                                        break;
                                    }
                                case "BUMP POSSESSION":
                                    {
                                        replacementBytes = new byte[] { 0x01 };
                                        break;
                                    }
                                case "JET PACK":
                                    {
                                        replacementBytes = new byte[] { 0x01 };
                                        break;
                                    }
                                case "MEDUSA":
                                    {
                                        replacementBytes = new byte[] { 0x01 };
                                        break;
                                    }
                                case "PAN CAM":
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
                                        break;
                                    }
                                case "3RD PERSON":
                                    {
                                        replacementBytes = new byte[] { 0x90, 0x90 };
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
                                        break;
                                    }
                                case "3RD PERSON":
                                    {
                                        replacementBytes = new byte[] { 0x90, 0x90 };
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
                                        break;
                                    }
                                case "3RD PERSON":
                                    {
                                        replacementBytes = new byte[] { 0x90, 0x90 };
                                        break;
                                    }
                                case "PAN CAM":
                                    {
                                        replacementBytes = new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x40, 0x38, 0xB9, 0x4A, 0x09, 0x00, 0x00, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, };
                                        break;
                                    }
                                case "SCRIPTED AI IN MP":
                                    {
                                        replacementBytes = new byte[] {  0x90, 0x90 };
                                        break;
                                    }
                                case "BETA RECOIL":
                                    {
                                        replacementBytes = new byte[] { 0x85 };
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
                                        break;
                                    }
                                case "3RD PERSON":
                                    {
                                        replacementBytes = new byte[] { 0xEB };
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
                                        break;
                                    }
                                case "3RD PERSON":
                                    {
                                        replacementBytes = new byte[] { 0x90, 0x90 };
                                        break;
                                    }
                                case "CAMPAIGN THEATER":
                                    {
                                        replacementBytes = new byte[] { 0x8B, 0xC7, 0x90 };
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
                                        break;
                                    }
                                case "BUMP POSSESSION":
                                    {
                                        replacementBytes = new byte[] { 0x00 };
                                        break;
                                    }
                                case "JET PACK":
                                    {
                                        replacementBytes = new byte[] { 0x00 };
                                        break;
                                    }
                                case "MEDUSA":
                                    {
                                        replacementBytes = new byte[] { 0x00 };
                                        break;
                                    }
                                case "PAN CAM":
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
                                        break;
                                    }
                                case "3RD PERSON":
                                    {
                                        replacementBytes = new byte[] { 0x74, 0x03 };
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
                                        break;
                                    }
                                case "3RD PERSON":
                                    {
                                        replacementBytes = new byte[] { 0x74, 0x0E };
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
                                        break;
                                    }
                                case "3RD PERSON":
                                    {
                                        replacementBytes = new byte[] { 0x74, 0x0E };
                                        break;
                                    }
                                case "PAN CAM":
                                    {
                                        replacementBytes = new byte[] { 0x0F, 0x84, 0xA4, 0x01, 0x00, 0x00, 0x40, 0x38, 0xB9, 0x4A, 0x09, 0x00, 0x00, 0x0F, 0x84, 0x97, 0x01, 0x00, 0x00, };
                                        break;
                                    }
                                case "SCRIPTED AI IN MP":
                                    {
                                        replacementBytes = new byte[] { 0x00, 0x0B };
                                        break;
                                    }
                                case "BETA RECOIL":
                                    {
                                        replacementBytes = new byte[] { 0x84 };
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
                                        break;
                                    }
                                case "3RD PERSON":
                                    {
                                        replacementBytes = new byte[] { 0x75 };
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
                                        break;
                                    }
                                case "3RD PERSON":
                                    {
                                        replacementBytes = new byte[] { 0x74, 0x03 };
                                        break;
                                    }
                                case "CAMPAIGN THEATER":
                                    {
                                        replacementBytes = new byte[] { 0x0F, 0x44, 0xC7 };
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
                    a.address = _addr.address;
                    a.name = _addr.name;
                    a.value = "Disabled";
                    int tmpIndex = addresses.IndexOf(_addr);
                    addresses.RemoveAt(tmpIndex);
                    addresses.Insert(tmpIndex, a);
                    Log();
                    //Main.status.Text = "Getting addresses...";
                    //BGW.RunWorkerAsync();
                    //GetAddresses();
                    //Main.outputLog.AppendText(TimeStamp() + "Old values successfully poked\n");
                }
            }            
        }
        private void Log()
        {
            output = "";
            foreach(Address a in addresses)
            {
                output += a.address + " : " + a.value + " - " + a.name + "\n";
            }
            Main.outputLog.Text = output;
        }
    }
}