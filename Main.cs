using Microsoft.Win32;
using System.Buffers;
using System.Diagnostics;

namespace ProjectX___Dota2_Camera_Changer
{
    public partial class Main : Form
    {
        private string selectedFilePath;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }
       
        private float GetFloatingPointValue(string prompt)
        {
            string inputValue = Microsoft.VisualBasic.Interaction.InputBox(prompt, "Floating-Point Value", "");
            float searchValue;
            if (float.TryParse(inputValue, out searchValue))
            {
                return searchValue;
            }
            else
            {
                MessageBox.Show("Invalid floating-point value entered.");
                return float.NaN;
            }
        }
        private void ReplaceFloatingPointValue(string filePath, float searchValue, float newValue)
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);
            byte[] searchValueBytes = BitConverter.GetBytes(searchValue);
            byte[] newValueBytes = BitConverter.GetBytes(newValue);

            for (int i = 0; i < fileBytes.Length - searchValueBytes.Length; i++)
            {
                if (fileBytes.Skip(i).Take(searchValueBytes.Length).SequenceEqual(searchValueBytes))
                {
                    Array.Copy(newValueBytes, 0, fileBytes, i, newValueBytes.Length);
                    i += searchValueBytes.Length - 1; // Skip the remaining bytes of the search value
                }
            }

            File.WriteAllBytes(filePath, fileBytes);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Dynamic Link Library (*.dll)|*.dll";
            openFileDialog.Title = "Select a DLL File";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectedFilePath = openFileDialog.FileName;
                MessageBox.Show($"Selected DLL: {selectedFilePath}");
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedFilePath))
            {
                MessageBox.Show("Please select a DLL file first.");
                return;
            }

            float searchValue = GetFloatingPointValue("Enter the value to search:");
            if (float.IsNaN(searchValue))
            {
                MessageBox.Show("Invalid floating-point value entered.");
                return;
            }

            float newValue = GetFloatingPointValue("Enter the new value:");
            if (float.IsNaN(newValue))
            {
                MessageBox.Show("Invalid floating-point value entered.");
                return;
            }

            ReplaceFloatingPointValue(selectedFilePath, searchValue, newValue);

            MessageBox.Show("Value replaced successfully.");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Guide Guide = new Guide();
            Guide.ShowDialog();
        }
    }
}
