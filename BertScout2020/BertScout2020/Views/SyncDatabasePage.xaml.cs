using AirtableApiClient;
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
                                      .Replace("\"Id\":null,", ""));
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
            int RecordCount = 0;
            while (true)
            {
                int tempCount = AirtableSendRecords().Result;
                if (tempCount < 0)
                {
                    // error message displayed already
                    return;
                }
                if (tempCount == 0)
                {
                    break;
                }
                // can only send 10 at a time, so keep trying
                RecordCount += tempCount;
                // only send one or two requests per second (max is 5, don't go that high)
                System.Threading.Thread.Sleep(500);
            }
            Label_Results.Text = "";
            Label_Results.Text += $"Records found: {RecordCount}\r\n";
        }

        private void Button_Airtable_Download_Clicked(object sender, EventArgs e)
        {
            AirtableFetchRecords();
        }

        async private Task<int> AirtableSendRecords()
        {
            int RecordCount = 0;
            int NewCount = 0;
            int UpdatedCount = 0;
            using (AirtableBase airtableBase = new AirtableBase(App.AirtableKey, App.AirtableBase))
            {
                List<Fields> newRecordList = new List<Fields>();
                List<IdFields> updatedRecordList = new List<IdFields>();
                foreach (EventTeamMatch match in App.Database.GetEventTeamMatchesAsync().Result)
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
                    // only allowed 10 creates per batch
                    if (RecordCount >= 5)
                    {
                        continue;
                    }
                    RecordCount++;
                    if (string.IsNullOrEmpty(match.AirtableId))
                    {
                        Fields fields = new Fields();
                        JObject jo = match.ToJson();
                        NewCount++;
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
                        if (match.Changed % 2 == 0) // even, don't upload
                        {
                            continue;
                        }
                        match.Changed++; // make even
                        IdFields fields = new IdFields(match.AirtableId.ToString());
                        JObject jo = match.ToJson();
                        UpdatedCount++;
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
                if (NewCount > 0)
                {
                    AirtableCreateUpdateReplaceMultipleRecordsResponse result;
                    result = await airtableBase.CreateMultipleRecords("Match",
                                                                newRecordList.ToArray());
                    if (!result.Success)
                    {
                        Label_Results.Text = "Error uploading:\r\n";
                        Label_Results.Text += $"{result.AirtableApiError.ErrorMessage}\r\n";
                        return -1;
                    }
                    foreach (AirtableRecord rec in result.Records)
                    {
                        EventTeamMatch match = App.Database.GetEventTeamMatchAsyncUuid(rec.GetField("Uuid").ToString());
                        if (match.Changed % 2 == 1)
                        {
                            match.Changed++; // make even so it doesn't send again
                        }
                        match.AirtableId = rec.Id;
                        await App.Database.SaveEventTeamMatchAsync(match);
                    }
                }
                if (UpdatedCount > 0)
                {
                    AirtableCreateUpdateReplaceMultipleRecordsResponse result;
                    result = airtableBase.UpdateMultipleRecords("Match",
                                                                updatedRecordList.ToArray()).Result;
                    if (!result.Success)
                    {
                        Label_Results.Text = "Error uploading:\r\n";
                        Label_Results.Text += $"{result.AirtableApiError.ErrorMessage}\r\n";
                        return -1;
                    }
                    foreach (AirtableRecord rec in result.Records)
                    {
                        EventTeamMatch match = App.Database.GetEventTeamMatchAsyncUuid(rec.GetField("Uuid").ToString());
                        match.Changed++;
                        await App.Database.SaveEventTeamMatchAsync(match);
                    }
                }
                return RecordCount;
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
            int NewCount = 0;
            int UpdateCount = 0;
            int IgnoreCount = 0;

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
                    if (m.Changed >= (long)ar.Fields["Changed"])
                    {
                        // this record is newer, ignore airtable
                        IgnoreCount++;
                        continue;
                    }
                }
                if (m == null)
                {
                    m = new EventTeamMatch();
                    m.AirtableId = ar.Id;
                    NewCount++;
                }
                else
                {
                    UpdateCount++;
                }
                // convert to JObject and back so individual match fields are not needed here
                jo = m.ToJson();
                foreach (KeyValuePair<string, object> kv in ar.Fields)
                {
                    if (kv.Key == "Id" || kv.Key == "AirtableId")
                    {
                        continue;
                    }
                    int intValue;
                    if (int.TryParse(kv.Value.ToString(), out intValue))
                    {
                        if (kv.Key == "Changed")
                        {
                            // only update if lower
                            if (m.Changed < intValue)
                            {
                                m.Changed = intValue;
                            }
                        }
                        else
                        {
                            jo.SetValue(kv.Key, intValue);
                        }
                    }
                    else
                    {
                        jo.SetValue(kv.Key, kv.Value.ToString());
                    }
                }
                // Rebuild the EventTeamMatch from the JObject data
                m = EventTeamMatch.FromJson(jo);
                if (m.Changed % 2 == 1)
                {
                    m.Changed++;
                }
                // save to the database
                await App.Database.SaveEventTeamMatchAsync(m);
            }

            Label_Results.Text = "";
            Label_Results.Text += $"Records found: {RecordCount}\r\n";
            Label_Results.Text += $"Records added: {NewCount}\r\n";
            Label_Results.Text += $"Records updated: {UpdateCount}\r\n";
            Label_Results.Text += $"Records ignored: {IgnoreCount}";
        }
    }
}
