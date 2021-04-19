using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using Memory;

namespace DLLFixer
{
    public partial class HaloDLLFixer : Form
    {
        RuntimePatcher RP;
        Mem MCCMemory = new Mem();
        Process p;
        bool attached = false;
        public HaloDLLFixer()
        {
            InitializeComponent();
        }
        private void DLLFixForAI_Shown(object sender, EventArgs e)
        {
            //CheckForIllegalCrossThreadCalls = false;
            attachToProcessToolStripMenuItem.PerformClick();
            UpdateAddresses();
        }

        private class MyRenderer : ToolStripProfessionalRenderer
        {
            public MyRenderer() : base(new MyColors()) { }
            protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
            {
                base.OnRenderMenuItemBackground(e);
                e.Item.BackColor = Color.FromArgb(255, 100, 100, 100);
            }
        }

        private class MyColors : ProfessionalColorTable
        {
            public override Color MenuBorder
            {
                get
                {
                    return Color.Black;
                }
            }
            public override Color MenuItemBorder
            {
                get
                {
                    return Color.Black;
                }
            }
            public override Color ImageMarginGradientBegin
            {
                get { return Color.FromArgb(255, 100, 100, 100); }
            }
            public override Color MenuItemSelected
            {
                get { return Color.FromArgb(255, 135, 135, 135); }
            }
            public override Color ToolStripDropDownBackground
            {
                get { return Color.FromArgb(255, 100, 100, 100); }
            }
            public override Color MenuItemPressedGradientBegin
            {
                get { return Color.FromArgb(255, 150, 150, 150); }
            }

            public override Color MenuItemPressedGradientEnd
            {
                get { return Color.FromArgb(255,150,150,150); }
            }
        }
        private void UpdateModules()
        {
            MCCMemory.theProc.Refresh();
            MCCMemory.modules.Clear();
            foreach (ProcessModule Module in MCCMemory.theProc.Modules)
            {
                if (!string.IsNullOrEmpty(Module.ModuleName) && !MCCMemory.modules.ContainsKey(Module.ModuleName))
                {
                    MCCMemory.modules.Add(Module.ModuleName, Module.BaseAddress);
                }
            }
        }
        private void AttachToProcess()
        {
            //EAC check
            if (CheckForEAC())
            {
                //EAC is running, cannot attach, return.
                MessageBox.Show("EAC is running");
                return;
            }
            try
            {
                p = Process.GetProcessesByName("MCC-Win64-Shipping")[0];
                MCCMemory.OpenProcess(p.Id);
                attachToProcessToolStripMenuItem.Text = "Detach from process";
                attached = true;
                RP = new RuntimePatcher(MCCMemory, this);
            }
            catch
            {
                try
                {
                    p = Process.GetProcessesByName("MCCWinStore-Win64-Shipping")[0];
                    MCCMemory.OpenProcess(p.Id);
                    attachToProcessToolStripMenuItem.Text = "Detach from process";
                    attached = true;
                    RP = new RuntimePatcher(MCCMemory, this);
                }
                catch
                {
                    MessageBox.Show("\nMake sure MCC is running");
                    //this.Close();
                }
            }           
        }
        private void DetachFromProcess()
        {
            try
            {
                p = null;
                MCCMemory.CloseProcess();
                attachToProcessToolStripMenuItem.Text = "Attach to process";
                attached = false;
            }
            catch
            {
                MessageBox.Show("Couldn't Detach");
            }
        }

        private void PokeDLLs_Click(object sender, EventArgs e)
        {
            //outputLog.Clear();
            if (attached)
            {
                UpdateModules();
                RP.PokeDLL();
            }
        }

        private void attachToProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!attached)
            {
                AttachToProcess();
            }
            else
            {
                DetachFromProcess();
            }
        }

        private void UnPoke_Click(object sender, EventArgs e)
        {
            outputLog.Clear();
            if (attached)
            {
                RP.UnPokeDLL();
            }
        }
        private void LoadList(string tabName)
        {
            switch (tabName)
            {
                case "halo1":
                    {
                        comboBox1.Items.Add("3RD PERSON");
                        comboBox1.Items.Add("BUMP POSSESSION");
                        comboBox1.Items.Add("PAN CAM");
                       // comboBox1.Items.Add("JET PACK");
                        comboBox1.Items.Add("MEDUSA");
                        comboBox1.Items.Add("WIREFRAME");
                        break;
                    }
                case "groundhog":
                    {
                        comboBox1.Items.Add("AI");
                        comboBox1.Items.Add("3RD PERSON");
                        break;
                    }
                case "halo3":
                    {
                        comboBox1.Items.Add("AI");
                        comboBox1.Items.Add("3RD PERSON");
                        break;
                    }
                case "halo3odst":
                    {
                        comboBox1.Items.Add("AI");
                        comboBox1.Items.Add("3RD PERSON");
                        break;
                    }
                case "haloreach":
                    {
                        comboBox1.Items.Add("AI");
                        comboBox1.Items.Add("BETA RECOIL");
                        comboBox1.Items.Add("3RD PERSON");
                        comboBox1.Items.Add("PAN CAM");
                        comboBox1.Items.Add("SCRIPTED AI IN MP");
                        break;
                    }
                case "halo4":
                    {
                        comboBox1.Items.Add("SCRIPTED AI IN MP");
                        comboBox1.Items.Add("3RD PERSON");
                        comboBox1.Items.Add("CAMPAIGN THEATER");
                        break;
                    }
            }
            comboBox1.SelectedIndex = 0;
        }
        private void UpdateAddresses()
        {
            comboBox1.Items.Clear();
           
            LoadList(tabControl1.SelectedTab.Name);
            if (attached)
            {
                RP.ModuleName = tabControl1.SelectedTab.Name + ".dll";
                if (!RP.InGameBGW.IsBusy && !RP.BGW.IsBusy)
                {
                    RP.InGameBGW.RunWorkerAsync();
                }
                else
                {
                    RP.InGameBGW.CancelAsync();
                    RP.BGW.CancelAsync();
                    UpdateAddresses();
                }
            }
        }
        private bool CheckForEAC()
        {

            if (Process.GetProcessesByName("EasyAntiCheat").Length > 0)
            {
                return true;
            }
            return false;
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            outputLog.Clear();
            int index = tabControl1.SelectedIndex;
            tabControl1.TabPages[index].Controls.Add(Poke);
            tabControl1.TabPages[index].Controls.Add(UnPoke);
            tabControl1.TabPages[index].Controls.Add(refresh);
            tabControl1.TabPages[index].Controls.Add(outputLog);
            tabControl1.TabPages[index].Controls.Add(comboBox1);
            status.Text = "";
            tabControl1.TabPages[index].Controls.Add(status);
            if (MCCMemory.theProc != null)
            {
                RefreshProc();
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            string game = tabControl1.SelectedTab.Name + ".dll";
        }
        private void RefreshProc()
        {
            UpdateModules();
            UpdateAddresses();
            //RP.HexFile = "";
        }
        private void refresh_Click(object sender, EventArgs e)
        {
            RefreshProc();
        }

        private void HaloDLLFixer_Load(object sender, EventArgs e)
        {
            menuStrip1.Renderer = new MyRenderer();

        }

        private void About_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        private void openPatchesFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = Application.StartupPath + "\\Patches";
            Process.Start("explorer.exe",path);
        }
    }
}