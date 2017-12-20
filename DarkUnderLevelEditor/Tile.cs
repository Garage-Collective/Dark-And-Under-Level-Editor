using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DarkUnderLevelEditor {

    public partial class Tile : UserControl {

        private Byte[] data = new Byte[30];
        private int index = 0;

        public Tile() {

            InitializeComponent();

            dgTile.Rows.Add("");
            dgTile.Rows.Add("");
            dgTile.Rows.Add("");
            dgTile.Rows.Add("");
            dgTile.Rows.Add("");
            dgTile.Rows.Add("");
            dgTile.Rows.Add("");
            dgTile.Rows.Add("");
            dgTile.Rows.Add("");
            dgTile.Rows.Add("");
            dgTile.Rows.Add("");
            dgTile.Rows.Add("");
            dgTile.Rows.Add("");
            dgTile.Rows.Add("");
            dgTile.Rows.Add("");

        }

        public String Title {
            get { return lblName.Text; }
            set { lblName.Text = value; }
        }

        public int Index {
            get { return index; }
            set { index = value; }
        }

        public Byte[] Data {

            get { return data; }
            set { data = value;

                for (Byte y = 0; y < 15; y++) {

                    for (Byte x = 0; x < 15; x++) {
    
                        if ((data[((y / 8) * 15) + x] & (1 << (y % 8))) > 0) {

                            dgTile.Rows[y].Cells[x].Style.BackColor = Color.Gray;

                        }
                        else {

                            dgTile.Rows[y].Cells[x].Style.BackColor = Color.White;

                        }

                    }

                }
            
            }

        }

        private void dgTile_Paint(object sender, PaintEventArgs e) {

            dgTile.ClearSelection();
            dgTile.CurrentCell = null;

        }

        public Bitmap GetImage() {

            Bitmap b = new Bitmap(32, 32);
            Graphics g = Graphics.FromImage(b);

            g.FillRectangle(Brushes.White, 0, 0, 30, 30);

            for (Byte y = 0; y < 15; y++) {

                for (Byte x = 0; x < 15; x++) {

                    if ((data[((y / 8) * 15) + x] & (1 << (y % 8))) > 0) {

                        g.FillRectangle(Brushes.Black, x * 2, y * 2, 2, 2);

                    }
                    
                }

            }


            return b;

        }

    }
    
}
