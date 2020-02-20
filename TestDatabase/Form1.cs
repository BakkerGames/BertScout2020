using BertScout2020Data.Data;
using BertScout2020Data.Models;
using System;
using System.IO;
using System.Windows.Forms;

namespace TestDatabase
{
    public partial class Form1 : Form
    {
        static public BertScout2020Database database;

        public Form1()
        {
            InitializeComponent();
        }

        public static string GetMyDocumentsPath()
        {
            return "C:\\Users\\chime\\AppData\\Local\\Packages\\4561a6e5-9cb5-4aa3-96d4-2bab0e0735a8_s9xa4h54xqsg0\\LocalState";
        }

        public static BertScout2020Database Database
        {
            get
            {
                if (database == null)
                {
                    string myDocumentsPath = GetMyDocumentsPath();
                    string myDatabasePath = Path.Combine(myDocumentsPath, BertScout2020Database.dbFilename);
                    if (File.Exists(myDatabasePath))
                    {
                        database = new BertScout2020Database(myDatabasePath);
                    }
                }
                return database;
            }
        }

        async private void button1_Click(object sender, EventArgs e)
        {
            FRCEvent f;
            f = await Database.GetEventAsyncUuid("64fe511c-33bd-45b1-a457-ab7bebdb4e65");
            textBox1.Text = f.ToString();
        }
    }
}
