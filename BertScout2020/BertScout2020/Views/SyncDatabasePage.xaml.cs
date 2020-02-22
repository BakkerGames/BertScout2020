﻿using AirtableApiClient;
using BertScout2020.Services;
using BertScout2020Data.Models;
using Common.JSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BertScout2020.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SyncDatabasePage : ContentPage
    {
        public IDataStore<EventTeamMatch> SqlDataEventTeamMatches;
        public IDataStore<EventTeamMatch> WebDataEventTeamMatches;

        private static bool _isBusy = false;

        public SyncDatabasePage()
        {
            InitializeComponent();
            Title = "Synchronize Data";
            //Entry_IpAddress.Text = App.syncIpAddress;
            SqlDataEventTeamMatches = new SqlDataStoreEventTeamMatches(App.currFRCEventKey);
            WebDataEventTeamMatches = new WebDataStoreEventTeamMatches();
        }

        private void Button_Export_Clicked(object sender, EventArgs e)
        {
            if (_isBusy)
            {
                return;
            }
            _isBusy = true;

            Label_Results.Text = "Exporting data...";

            List<EventTeamMatch> matches = (List<EventTeamMatch>)SqlDataEventTeamMatches.GetItemsAsync().Result;

            JArray exportJarray = new JArray();
            StringBuilder exportData = new StringBuilder();
            exportData.AppendLine("[");

            int exportCount = 0;
            foreach (EventTeamMatch item in matches)
            {
                if (exportCount > 0)
                {
                    exportData.AppendLine(",");
                }
                item.Id = null; // don't preserve id
                item.Changed = 0; // changed = 0 so downloaded data is not uploaded
                exportData.Append(item.ToString()
                                      .Replace("\"Id\":null,", "")
                                      .Replace("\"Changed\":0,", ""));
                exportCount++;
            }
            if (exportCount > 0)
            {
                exportData.AppendLine();
            }
            exportData.AppendLine("]");

            string myDocumentsPath = App.GetMyDocumentsPath();
            Label_Results.Text += $"\n\n{myDocumentsPath}";

            string path = Path.Combine(myDocumentsPath, $"{App.AppYear}_{App.currFRCEventKey}_{App.KindleName}.json");

            File.WriteAllText(path, exportData.ToString());

            Label_Results.Text += $"\n\nCount: {exportCount}";
            Label_Results.Text += $"\n\nExport complete";

            _isBusy = false;
        }

        private void Button_Import_Clicked(object sender, EventArgs e)
        {
            if (_isBusy)
            {
                return;
            }
            _isBusy = true;

            Label_Results.Text = "Importing data...";

            List<EventTeamMatch> matches = (List<EventTeamMatch>)SqlDataEventTeamMatches.GetItemsAsync().Result;

            string myDocumentsPath = App.GetMyDocumentsPath();

            string[] filenames = Directory.GetFiles(myDocumentsPath, $"{App.AppYear}_{App.currFRCEventKey}*.json");

            foreach (string path in filenames)
            {
                Label_Results.Text += $"\n\n{path}";

                string allMatchData = File.ReadAllText(path);

                JArray matchJsonData = JArray.Parse(allMatchData);

                Label_Results.Text += $"\n\nMatches found: {matchJsonData.Count}";

                int addedCount = 0;
                int updatedCount = 0;
                int notChangedCount = 0;

                foreach (JObject obj in matchJsonData)
                {
                    EventTeamMatch item = EventTeamMatch.Parse(obj.ToString());
                    EventTeamMatch oldItem = matches.FirstOrDefault(p => p.Uuid == item.Uuid);

                    if (oldItem == null)
                    {
                        item.Changed = 0; // downloaded records are excluded from uploading
                        SqlDataEventTeamMatches.AddItemAsync(item);
                        addedCount++;
                    }
                    else if (oldItem.Changed > 0 && oldItem.Changed < item.Changed)
                    {
                        SqlDataEventTeamMatches.UpdateItemAsync(item);
                        updatedCount++;
                    }
                    else
                    {
                        notChangedCount++;
                    }
                }

                Label_Results.Text += $"\n\nAdded: {addedCount} - Updated: {updatedCount} - Not Changed: {notChangedCount}";
            }

            Label_Results.Text += "\n\nImport complete";

            _isBusy = false;
        }

        private void Button_Airtable_Upload_Clicked(object sender, EventArgs e)
        {
            AirtableSendRecords();
        }

        private void Button_Airtable_Download_Clicked(object sender, EventArgs e)
        {
            AirtableFetchRecords();
        }

        async private void AirtableSendRecords()
        {
            int RecordCount = 0;
            int NewRecords = 0;
            int UpdatedRecords = 0;
            using (AirtableBase airtableBase = new AirtableBase(App.AirtableKey, App.AirtableBase))
            {
                List<Fields> newRecordList = new List<Fields>();
                List<IdFields> updatedRecordList = new List<IdFields>();
                foreach (EventTeamMatch match in await App.Database.GetEventTeamMatchesAsync())
                {
                    // only send matches from this event
                    if (match.EventKey != App.currFRCEventKey)
                    {
                        continue;
                    }
                    // only send matches from this device
                    if (match.DeviceName != App.KindleName)
                    {
                        continue;
                    }
                    if (match.Changed % 2 == 0) // even, don't upload
                    {
                        continue;
                    }
                    RecordCount++;
                    if (match.Changed == 1 || string.IsNullOrEmpty(match.AirtableId))
                    {
                        Fields fields = new Fields();
                        JObject jo = match.ToJson();
                        NewRecords++;
                        foreach (KeyValuePair<string, object> kv in jo.ToList())
                        {
                            if (kv.Key == "Id" || kv.Key == "AirtableId")
                            {
                                continue;
                            }
                            fields.AddField(kv.Key, kv.Value);
                        }
                        newRecordList.Add(fields);
                    }
                    else
                    {
                        IdFields fields = new IdFields(match.AirtableId.ToString());
                        JObject jo = match.ToJson();
                        UpdatedRecords++;
                        foreach (KeyValuePair<string, object> kv in jo.ToList())
                        {
                            if (kv.Key == "Id" || kv.Key == "AirtableId")
                            {
                                continue;
                            }
                            fields.AddField(kv.Key, kv.Value);
                        }
                        updatedRecordList.Add(fields);
                    }
                }
                if (RecordCount == 0)
                {
                    Label_Results.Text = "No records found";
                    return;
                }
                if (NewRecords > 0)
                {
                    AirtableCreateUpdateReplaceMultipleRecordsResponse result;
                    result = await airtableBase.CreateMultipleRecords("Match",
                                                                      newRecordList.ToArray());
                    if (!result.Success)
                    {
                        Label_Results.Text = $"Error uploading: {result.AirtableApiError.ErrorMessage}";
                        return;
                    }
                    foreach (AirtableRecord rec in result.Records)
                    {
                        EventTeamMatch match = App.Database.GetEventTeamMatchAsyncUuid(rec.GetField("Uuid").ToString());
                        match.Changed++;
                        match.AirtableId = rec.Id;
                        await App.Database.SaveEventTeamMatchAsync(match);
                    }
                }
                if (UpdatedRecords > 0)
                {
                    AirtableCreateUpdateReplaceMultipleRecordsResponse result;
                    result = await airtableBase.UpdateMultipleRecords("Match",
                                                                      updatedRecordList.ToArray());
                    if (!result.Success)
                    {
                        Label_Results.Text = $"Error uploading: {result.AirtableApiError.ErrorMessage}";
                        return;
                    }
                    foreach (AirtableRecord rec in result.Records)
                    {
                        EventTeamMatch match = App.Database.GetEventTeamMatchAsyncUuid(rec.GetField("Uuid").ToString());
                        match.Changed++;
                        await App.Database.SaveEventTeamMatchAsync(match);
                    }
                }
                Label_Results.Text += $"Records found: {RecordCount}\r\n";
                Label_Results.Text += $"New records: {NewRecords}\r\n";
                Label_Results.Text += $"Updated records: {UpdatedRecords}\r\n";
            }
        }

        async private void AirtableFetchRecords()
        {
            string offset = null;
            string errorMessage = null;
            var records = new List<AirtableRecord>();

            using (AirtableBase airtableBase = new AirtableBase(App.AirtableKey, App.AirtableBase))
            {
                do
                {
                    if (offset != null)
                    {
                        // Sleep for half a second so we don't make requests too fast.
                        // The free Airtable plan only allows 5 requests per second.
                        // Each download is 100 records (pagesize).
                        System.Threading.Thread.Sleep(500);
                    }

                    Task<AirtableListRecordsResponse> task = airtableBase.ListRecords(
                           "Match",
                           offset,
                           null /*fieldsArray*/,
                           $"EventKey='{App.currFRCEventKey}'" /*filterByFormula*/,
                           null /*maxRecords*/,
                           100 /*pageSize*/,
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
                Label_Results.Text = errorMessage;
                return;
            }

            int RecordCount = 0;
            int NewRecords = 0;

            foreach (AirtableRecord ar in records)
            {
                RecordCount++;
                JObject jo = new JObject();
                EventTeamMatch m = null;
                string uuid = ar.Fields["Uuid"].ToString();
                string airtableId = ar.Id;
                if (!string.IsNullOrEmpty(uuid))
                {
                    StringBuilder query = new StringBuilder();
                    query.Append("SELECT [EventTeamMatch].* FROM [EventTeamMatch]");
                    query.Append(" WHERE [EventTeamMatch].[Uuid] = '");
                    query.Append(uuid);
                    query.Append("'");
                    m = App.Database.GetEventTeamMatchAsyncUuid(uuid);
                }
                if (m == null)
                {
                    m = new EventTeamMatch();
                    m.AirtableId = ar.Id;
                    NewRecords++;
                }
                // convert to JObject and back so individual match fields are not needed here
                jo = m.ToJson();
                foreach (KeyValuePair<string, object> kv in ar.Fields)
                {
                    if (kv.Key == "Id" || kv.Key == "AirtableId")
                    {
                        continue;
                    }
                    // airtable sends all numbers as Long, must convert back to Int
                    int intValue;
                    if (int.TryParse(kv.Value.ToString(), out intValue))
                    {
                        jo.SetValue(kv.Key, intValue);
                    }
                    else
                    {
                        jo.SetValue(kv.Key, kv.Value.ToString());
                    }
                }
                // Rebuild the EventTeamMatch from the JObject data
                m = EventTeamMatch.FromJson(jo);
                // save to the database
                await App.Database.SaveEventTeamMatchAsync(m);
            }

            Label_Results.Text = $"Records found: {RecordCount}\r\nRecords added: {NewRecords}";
        }
    }
}
