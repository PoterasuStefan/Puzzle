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

namespace Puzzle_Poterasu_Stefan
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

        }



        private void button1_Click(object sender, EventArgs e)
        {
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.RowCount = 2;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp|All Files|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Get the selected file name and display it in the TextBox
                string selectedImagePath = openFileDialog.FileName;

                // Load the image into the PictureBox
                try
                {
                    // Dispose of the existing image, if any
                    if (pictureBox1.Image != null)
                        pictureBox1.Image.Dispose();

                    // Load the new image
                    pictureBox1.Image = Image.FromFile(selectedImagePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // Get the original image
            Image originalImage = pictureBox1.Image;

            // Split the image into four quadrants
            Image[] splitImages = SplitImage4(originalImage);

            // Clear existing controls from the TableLayoutPanel
            tableLayoutPanel1.Controls.Clear();

            // Display each split image in the TableLayoutPanel
            for (int row = 0; row < 2; row++)
            {
                for (int col = 0; col < 2; col++)
                {
                    PictureBox picBox = new PictureBox();
                    picBox.Dock = DockStyle.Fill;
                    picBox.SizeMode = PictureBoxSizeMode.Zoom;
                    picBox.Image = splitImages[row * 2 + col];
                    tableLayoutPanel1.Controls.Add(picBox, col, row);
                }
            }

        }
        private Image[] SplitImage4(Image originalImage)
        {
            // Get the size of each quadrant
            int quadrantWidth = originalImage.Width / 2;
            int quadrantHeight = originalImage.Height / 2;

            // Create four separate images for each quadrant
            Image[] splitImages = new Image[4];

            for (int i = 0; i < 4; i++)
            {
                // Create a new bitmap for the current quadrant
                splitImages[i] = new Bitmap(quadrantWidth, quadrantHeight);

                // Create a graphics object from the new bitmap
                using (Graphics g = Graphics.FromImage(splitImages[i]))
                {
                    // Draw the portion of the original image into the new bitmap
                    g.DrawImage(originalImage, new Rectangle(0, 0, quadrantWidth, quadrantHeight),
                        new Rectangle(i % 2 * quadrantWidth, i / 2 * quadrantHeight, quadrantWidth, quadrantHeight),
                        GraphicsUnit.Pixel);
                }
            }

            return splitImages;
        }
        private Image[] SplitImage9(Image originalImage)
        {
            // Get the size of each quadrant
            int quadrantWidth = originalImage.Width / 3;
            int quadrantHeight = originalImage.Height / 3;

            // Create four separate images for each quadrant
            Image[] splitImages = new Image[9];

            for (int i = 0; i < 9; i++)
            {
                // Create a new bitmap for the current quadrant
                splitImages[i] = new Bitmap(quadrantWidth, quadrantHeight);

                // Create a graphics object from the new bitmap
                using (Graphics g = Graphics.FromImage(splitImages[i]))
                {
                    // Draw the portion of the original image into the new bitmap
                    g.DrawImage(originalImage, new Rectangle(0, 0, quadrantWidth, quadrantHeight),
                        new Rectangle(i % 3 * quadrantWidth, i / 3 * quadrantHeight, quadrantWidth, quadrantHeight),
                        GraphicsUnit.Pixel);
                }
            }

            return splitImages;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.RowCount = 3;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp|All Files|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Get the selected file name and display it in the TextBox
                string selectedImagePath = openFileDialog.FileName;

                // Load the image into the PictureBox
                try
                {
                    // Dispose of the existing image, if any
                    if (pictureBox1.Image != null)
                        pictureBox1.Image.Dispose();

                    // Load the new image
                    pictureBox1.Image = Image.FromFile(selectedImagePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                // Check if the control is a PictureBox
                if (control is PictureBox picBox)
                {
                    // Attach the Click event handler
                    picBox.Click += PictureBox_Click;
                }
            }

            // Get the original image
            Image originalImage = pictureBox1.Image;

            // Split the image into nine quadrants
            Image[] splitImages = SplitImage9(originalImage);

            // Shuffle the split images randomly
            RandomizeArray(splitImages);

            // Clear existing controls from the TableLayoutPanel
            tableLayoutPanel1.Controls.Clear();

            // Display each split image in the TableLayoutPanel
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    PictureBox picBox = new PictureBox();
                    picBox.Dock = DockStyle.Fill;
                    picBox.SizeMode = PictureBoxSizeMode.Zoom;
                    picBox.Image = splitImages[row * 3 + col];
                    tableLayoutPanel1.Controls.Add(picBox, col, row);
                }
            }

        }

        private PictureBox lastClickedPictureBox = null;
        private PictureBox NextTolastClickedPictureBox = null;

        private void PictureBox_Click(object sender, EventArgs e)
        {
            // Handle the click event here
            if (sender is PictureBox clickedPictureBox)
            {
                // Store the clicked image in the variable
                NextTolastClickedPictureBox=lastClickedPictureBox;
                lastClickedPictureBox = clickedPictureBox;

                if(NextTolastClickedPictureBox != null)
                {
                    lastClickedPictureBox.Image = NextTolastClickedPictureBox.Image;
                    NextTolastClickedPictureBox.Image = lastClickedPictureBox.Image;    
                }

                // Perform actions based on the clicked PictureBox
                
            }
        }
        private void RandomizeArray<T>(T[] array)
        {
            Random random = new Random();

            for (int i = array.Length - 1; i > 0; i--) 
            {
                int j = random.Next(0, i + 1);
                T temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }








        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
