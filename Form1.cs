using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Tesseract;
using Tesseract.Interop;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
namespace case_4
{

    public partial class Form1 : Form
    {
        private string filePath = string.Empty;
        private static string pathRU = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\tessdata";
        private static string lung = "rus";
        private static Bitmap image;
        private static TesseractEngine tesseract = new TesseractEngine(pathRU, lung);
        private static string[] strings;

        public Form1()
        {
            InitializeComponent();
            tesseract.SetVariable(pathRU, lung);
            tesseract.SetVariable("rus.unicharset", "1234567890АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ");
            tesseract.SetVariable("rus", true);
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult res = openFileDialog1.ShowDialog();
            // Пользователь выбирает файл 
            if (res == DialogResult.OK)
            {
                filePath = openFileDialog1.FileName;
                image = new Bitmap(filePath);
                pictureBox1.Image = image;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Документ не выбран");
            }
            else
            {
                strings = tesseract.Process(image).GetText().ToLower().Split(' ', ',', '!', '?', '.', ')', '(', '-', '_');
                int s1 = 0;
                bool type = false;
                while (s1 < strings.Length)
                {
                    // Проверка на Счет-фактура
                    if (strings[s1] == "счет" && strings[s1+1] == "фактура" && type == false) { MessageBox.Show("Документ: Счет-фактура"); type = true; }
                    // Проверка на  ОРДЕР
                    else if (strings[s1] == "орден" && type == false) { MessageBox.Show("Документ: Ордер"); type = true; }
                    // проверка на Отчет
                    else if (strings[s1] == "отчет" && type == false) { MessageBox.Show("Документ: Отчет"); type = true; }
                    // проверка на АКТ
                    else if (strings[s1] == "акт" && type == false) { MessageBox.Show(" Документ: Акт"); type = true; }
                    // проверка на Справка
                    else if (strings[s1] == "справка" && type == false) { MessageBox.Show("Документ: Справка"); type = true; }
                    // проверка на Счет
                    else if (strings[s1] ==  "счет" && type == false) { MessageBox.Show("Документ: Счет"); type = true; }
                        
                    textBox1.Text += strings[s1] +"\t";
                    s1 ++;
                }
            }
            tesseract.Dispose();
        }
    }
}


