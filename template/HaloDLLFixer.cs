using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
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
            CheckForIllegalCrossThreadCalls = false;
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
            try
            {
                p = Process.GetProcessesByName("MCC-Win64-Shipping")[0];
                MCCMemory.OpenProcess(p.Id);
                attachToProcessToolStripMenuItem.Text = "Detach from process";
                attached = true;
            }
            catch
            {
                MessageBox.Show("\nMake sure MCC is running");
                //this.Close();
            }
            RP = new RuntimePatcher(MCCMemory, this);
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
                        comboBox1.Items.Add("JET PACK");
                        comboBox1.Items.Add("MEDUSA");
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
                RP.GetModule(tabControl1.SelectedTab.Name + ".dll");
            }
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //outputLog.Clear();
            int index = tabControl1.SelectedIndex;
            tabControl1.TabPages[index].Controls.Add(Poke);
            tabControl1.TabPages[index].Controls.Add(UnPoke);
            tabControl1.TabPages[index].Controls.Add(refresh);
            tabControl1.TabPages[index].Controls.Add(outputLog);
            tabControl1.TabPages[index].Controls.Add(comboBox1);
            UpdateModules();
            UpdateAddresses();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            string game = tabControl1.SelectedTab.Name + ".dll";
            File.WriteAllBytes(Application.StartupPath + "\\Saved\\" + game, RP.SaveBytes(game));
        }

        private void refresh_Click(object sender, EventArgs e)
        {
            UpdateModules();
            UpdateAddresses();
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
    }
}