using Appzr.Handlers;
using Appzr.ViewModels;
using System;
using System.Windows.Forms;

namespace Appzr
{
    public partial class NewAppForm : Form
    {
        public NewAppForm()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show(this, "Nome do sotfware é obrigatorio!", "Appzr", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtName.Focus();
                return;
            }

            if (String.IsNullOrWhiteSpace(txtUrl.Text))
            {
                MessageBox.Show(this, "Url do sotfware é obrigatoria!", "Appzr", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtUrl.Focus();
                return;
            }

            var app = new AppVM()
            {
                Name = txtName.Text,
                Url = txtUrl.Text,
                Description = txtDescription.Text
            };

            if (DataHandler.Add(app))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
