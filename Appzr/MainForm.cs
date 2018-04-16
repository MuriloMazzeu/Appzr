using Appzr.Handlers;
using Appzr.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Appzr
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            DataHandler.Initialize();

            myApps.DisplayMember = "Name";
            myApps.ValueMember = "Id";                        
            FillMyApps();
            myApps.ClearSelected();
        }

        private void FillMyApps()        
            => myApps.DataSource = DataHandler.List<AppVM>(app => app.InactiveAt == null);        

        private void sobreToolStripMenuItem_Click(object sender, EventArgs e)
            => new AboutBox().ShowDialog();

        private void btnClear_Click(object sender, EventArgs e)
            => myApps.ClearSelected();

        private void novoAppToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (new NewAppForm().ShowDialog(this) == DialogResult.OK)
            {
                FillMyApps();
                MessageBox.Show(this, "App cadastrado com sucesso!", "Appzr", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            var items = myApps.SelectedItems.Cast<AppVM>().ToArray();
            if(items.Length > 0)
            {
                var msg = items.Length > 1 ? "Deseja realmente remover esses itens?" : "Deseja realmente remover esse item?";
                var confirm = MessageBox.Show(this, msg, "Appzr", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if(confirm == DialogResult.Yes)
                {
                    DataHandler.Remove(items);
                    FillMyApps();
                }
            }
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            var urls = myApps.SelectedItems.Cast<AppVM>().Select(app => app.Url).ToArray();
            await UrlHandler.OpenInBrowserAsync(urls);
        }
    }
}
