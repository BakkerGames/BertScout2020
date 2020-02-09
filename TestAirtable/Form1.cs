using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AirtableApiClient;

namespace TestAirtable
{
    public partial class Form1 : Form
    {

        static public string AirtableBase = "appeBQ6HTf90jtgwo";
        static public string AirtableKey = "keyKaMroYDAVnmnPQ";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            //AirtableListRecordsResponse res;
            //AirtableBase airtable = new AirtableBase(AirtableKey, AirtableBase);
            //try
            //{
            //    res = airtable.ListRecords("Match").Result;
            //}
            //catch (Exception ex)
            //{
            //    textBox1.Text = ex.Message;
            //    return;
            //}
            //textBox1.Text = "";
            //foreach (AirtableRecord ar in res.Records)
            //{
            //    textBox1.Text += ar.ToString();
            //    textBox1.Text += "\r\n";
            //}
            //textBox1.Text += "--- Done ---";


            string offset = null;
            string errorMessage = null;
            string eventKey = "2020test";
            var records = new List<AirtableRecord>();

            using (AirtableBase airtableBase = new AirtableBase(AirtableKey, AirtableBase))
            {
                // Use 'offset' and 'pageSize' to specify the records that you want
                // to retrieve.
                // Only use a 'do while' loop if you want to get multiple pages
                // of records.

                do
                {
                    Task<AirtableListRecordsResponse> task = airtableBase.ListRecords(
                           "Match",
                           offset,
                           null /*fieldsArray*/,
                           $"EventKey='{eventKey}'" /*filterByFormula*/,
                           null /*maxRecords*/,
                           null /*pageSize*/,
                           null /*sort*/,
                           null /*view*/);

                    AirtableListRecordsResponse response = await task;

                    if (response.Success)
                    {
                        records.AddRange(response.Records.ToList());
                        offset = response.Offset;
                    }
                    else if (response.AirtableApiError is AirtableApiException)
                    {
                        errorMessage = response.AirtableApiError.ErrorMessage;
                        break;
                    }
                    else
                    {
                        errorMessage = "Unknown error";
                        break;
                    }
                } while (offset != null);
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                textBox1.Text = errorMessage;
            }
            else
            {
                foreach (AirtableRecord ar in records)
                {
                    bool addComma = false;
                    textBox1.AppendText("{");
                    foreach (KeyValuePair<string, object> kv in ar.Fields)
                    {
                        if (addComma)
                        {
                            textBox1.AppendText(",");
                        }
                        addComma = true;
                        textBox1.AppendText($"\"{kv.Key}\":");
                        switch (Type.GetTypeCode(kv.Value.GetType()))
                        {
                            case TypeCode.String:
                                textBox1.AppendText($"\"{kv.Value}\"");
                                break;
                            default:
                                textBox1.AppendText($"{kv.Value}");
                                break;
                        }
                    }
                    textBox1.AppendText("}\r\n");
                }
            }
        }
    }
}
