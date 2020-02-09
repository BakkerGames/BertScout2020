using AirtableApiClient;
using BertScout2020.Services;
using BertScout2020Data.Models;
using Common.JSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

/*
<StackLayout Orientation="Horizontal">
    <Label
        Text="IPAddress:"
        VerticalOptions="Center" 
        HorizontalOptions="Center" 
        FontSize="24"
        />
    <Entry
        x:Name="Entry_IpAddress"
        Text=""
        FontSize="24"
        HorizontalOptions="FillAndExpand"
        IsPassword="False"
        IsSpellCheckEnabled="False"
        IsTextPredictionEnabled="False"
        TextChanged="Entry_IpAddress_TextChanged"
        />
</StackLayout>
<Button
    x:Name="Button_Upload"
    Text="Upload Data"
    Clicked="Button_Upload_Clicked"
    BackgroundColor="#B19CD9"
    HorizontalOptions="Center"
    VerticalOptions="Center"
    FontSize="24"
    Margin="10"
    WidthRequest="300"
    IsEnabled="True"
    />
<Button
    x:Name="Button_Download"
    Text="Download Data"
    Clicked="Button_Download_Clicked"
    BackgroundColor="#B19CD9"
    HorizontalOptions="Center"
    VerticalOptions="Center"
    FontSize="24"
    Margin="10"
    WidthRequest="300"
    IsEnabled="True"
    />
<Button
    x:Name="Button_Reset_Upload"
    Text="Reset Upload"
    Clicked="Button_Reset_Upload_Clicked"
    BackgroundColor="#B19CD9"
    HorizontalOptions="Center"
    VerticalOptions="Center"
    FontSize="24"
    Margin="10"
    WidthRequest="300"
    IsEnabled="True"
    />
*/

namespace BertScout2020.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SyncDatabasePage : ContentPage
    {
        public IDataStore<EventTeamMatch> SqlDataEventTeamMatches;
        public IDataStore<EventTeamMatch> WebDataEventTeamMatches;

        //private readonly string _mediaType = "application/json";

        //private static bool _initialSetup = false;
        private static bool _isBusy = false;

        //private int totalUploaded = 0;
        //private int toBeUploaded = 0;

        //private AirtableListRecordsResponse _airtableGet = new AirtableListRecordsResponse();

        public SyncDatabasePage()
        {
            InitializeComponent();
            Title = "Synchronize Data";
            //Entry_IpAddress.Text = App.syncIpAddress;
            Entry_KindleName.Text = App.kindleName;
            SqlDataEventTeamMatches = new SqlDataStoreEventTeamMatches(App.currFRCEventKey);
            WebDataEventTeamMatches = new WebDataStoreEventTeamMatches();
        }

        //private bool PrepareSync()
        //{
        //    if (!_initialSetup)
        //    {
        //        if (string.IsNullOrEmpty(Entry_IpAddress.Text))
        //        {
        //            Label_Results.Text = "Please enter IP Address";
        //            return false;
        //        }

        //        // save ip address
        //        _initialSetup = true;

        //        App.syncIpAddress = Entry_IpAddress.Text;
        //        Entry_IpAddress.IsEnabled = false;
        //        App.kindleName = Entry_KindleName.Text;
        //        Entry_KindleName.IsEnabled = false;

        //        string uri = App.syncIpAddress;
        //        if (!uri.EndsWith("/"))
        //        {
        //            uri += "/";
        //        }
        //        if (!uri.Contains(":"))
        //        {
        //            uri += "bertscout2020/";
        //        }
        //        if (!uri.StartsWith("http"))
        //        {
        //            uri = $"http://{uri}";
        //        }
        //        if (!uri.EndsWith("/"))
        //        {
        //            uri += "/";
        //        }

        //        App.client = new HttpClient(); ;
        //        App.client.BaseAddress = new Uri(uri);
        //        App.client.DefaultRequestHeaders.Accept.Clear();
        //        App.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));

        //    }

        //    return true;
        //}

        //private void Button_Upload_Clicked(object sender, EventArgs e)
        //{
        //    if (_isBusy)
        //    {
        //        return;
        //    }
        //    _isBusy = true;

        //    if (!PrepareSync())
        //    {
        //        _isBusy = false;
        //        return;
        //    }

        //    int addedCount = 0;
        //    int updatedCount = 0;

        //    Label_Results.Text = "Uploading data...";

        //    List<EventTeamMatch> matches = (List<EventTeamMatch>)SqlDataEventTeamMatches.GetItemsAsync().Result;

        //    try
        //    {

        //        // make and use a copy of the list because it will crash otherwise
        //        List<EventTeamMatch> copyOfMatches = new List<EventTeamMatch>();
        //        foreach (EventTeamMatch item in matches)
        //        {
        //            copyOfMatches.Add(item);
        //        }

        //        foreach (EventTeamMatch item in copyOfMatches)
        //        {
        //            if (item.EventKey != App.currFRCEventKey)
        //            {
        //                continue;
        //            }

        //            if (item.Changed > 0 && item.Changed % 2 == 1)
        //            {
        //                item.Changed++; // change odd to even = no upload next time
        //                if (item.Changed <= 2) // first time sending
        //                {
        //                    WebDataEventTeamMatches.AddItemAsync(item);
        //                    addedCount++;
        //                    totalUploaded++;
        //                }
        //                else
        //                {
        //                    WebDataEventTeamMatches.UpdateItemAsync(item);
        //                    updatedCount++;
        //                    totalUploaded++;
        //                }
        //                // save it so .Changed is updated
        //                // this modifies the original list "matches", which is why a copy is needed
        //                SqlDataEventTeamMatches.UpdateItemAsync(item);
        //            }

        //            if (addedCount + updatedCount >= 10)
        //            {
        //                break;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Label_Results.Text += $"\n\nError during transmission\n\n{ex.Message}\n\n{ex.InnerException}";
        //        _isBusy = false;
        //        return;
        //    }

        //    Label_Results.Text += $"\n\nAdded: {addedCount} - Updated: {updatedCount} - Total: {totalUploaded} of {matches.Count}";

        //    if (addedCount + updatedCount > 0)
        //    {
        //        Label_Results.Text += $"\n\nUpload again to send next batch";
        //    }
        //    else
        //    {
        //        Label_Results.Text += $"\n\nUpload complete";
        //    }

        //    _isBusy = false;
        //}

        //private void Button_Download_Clicked(object sender, EventArgs e)
        //{
        //    if (_isBusy)
        //    {
        //        return;
        //    }
        //    _isBusy = true;

        //    if (!PrepareSync())
        //    {
        //        _isBusy = false;
        //        return;
        //    }

        //    int addedCount = 0;
        //    int updatedCount = 0;
        //    int notChangedCount = 0;
        //    int lastId = 0;

        //    Label_Results.Text = "Downloading data...";

        //    List<EventTeamMatch> matches = (List<EventTeamMatch>)SqlDataEventTeamMatches.GetItemsAsync().Result;

        //    try
        //    {
        //        do
        //        {
        //            addedCount = 0;
        //            updatedCount = 0;
        //            notChangedCount = 0;
        //            string batchInfo = $"{App.currFRCEventKey}|{lastId}|10";
        //            HttpResponseMessage response = App.client.GetAsync($"api/EventTeamMatches?batchInfo={batchInfo}").Result;
        //            response.EnsureSuccessStatusCode();
        //            if (response.IsSuccessStatusCode)
        //            {
        //                string result = response.Content.ReadAsStringAsync().Result;
        //                JArray results = JArray.Parse(result);
        //                foreach (JObject obj in results)
        //                {
        //                    EventTeamMatch item = EventTeamMatch.Parse(obj.ToString());

        //                    if (lastId < item.Id.Value)
        //                    {
        //                        lastId = item.Id.Value;
        //                    }

        //                    EventTeamMatch oldItem = matches.FirstOrDefault(p => p.Uuid == item.Uuid);

        //                    if (oldItem == null)
        //                    {
        //                        item.Changed = 0; // downloaded records are excluded from sending
        //                        SqlDataEventTeamMatches.AddItemAsync(item);
        //                        addedCount++;
        //                    }
        //                    else if (oldItem.Changed > 0 && oldItem.Changed < item.Changed)
        //                    {
        //                        SqlDataEventTeamMatches.UpdateItemAsync(item);
        //                        updatedCount++;
        //                    }
        //                    else
        //                    {
        //                        notChangedCount++;
        //                    }
        //                }
        //            }

        //            if (addedCount + updatedCount + notChangedCount > 0)
        //            {
        //                Label_Results.Text += $"\n\nAdded: {addedCount} - Updated: {updatedCount} - Not Changed: {notChangedCount}";
        //            }
        //        }
        //        while (addedCount + updatedCount + notChangedCount > 0);

        //        Label_Results.Text += "\n\nDownload complete";

        //    }
        //    catch (Exception ex)
        //    {
        //        Label_Results.Text += $"\n\nError during transmission\n\n{ex.Message}\n\n{ex.InnerException}";
        //        _isBusy = false;
        //        return;
        //    }

        //    _isBusy = false;
        //}

        //private void Entry_IpAddress_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(Entry_IpAddress.Text))
        //    {
        //        // save ip address
        //        App.syncIpAddress = Entry_IpAddress.Text;
        //    }
        //}

        //private void Button_Reset_Upload_Clicked(object sender, EventArgs e)
        //{
        //    if (_isBusy)
        //    {
        //        return;
        //    }
        //    _isBusy = true;

        //    //List<string> downloadedUuids = new List<string>();
        //    List<EventTeamMatch> matches = (List<EventTeamMatch>)SqlDataEventTeamMatches.GetItemsAsync().Result;

        //    // make and use a copy of the list because it will crash otherwise
        //    List<EventTeamMatch> copyOfMatches = new List<EventTeamMatch>();
        //    foreach (EventTeamMatch item in matches)
        //    {
        //        copyOfMatches.Add(item);
        //    }

        //    foreach (EventTeamMatch item in copyOfMatches)
        //    {
        //        if (item.Changed > 0 && item.Changed % 2 == 0)
        //        {
        //            item.Changed--; // trigger resend
        //            SqlDataEventTeamMatches.UpdateItemAsync(item);
        //        }
        //    }

        //    Label_Results.Text = $"Reset complete";
        //    Label_Results.Text += "\n\nPlease upload data again";

        //    _isBusy = false;
        //}

        private void Entry_KindleName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Entry_KindleName.Text))
            {
                App.kindleName = Entry_KindleName.Text;
            }
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

            string path = Path.Combine(myDocumentsPath, $"{App.AppYear}_{App.currFRCEventKey}_{Entry_KindleName.Text}.json");

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
            Label_Results.Text = "Airtable Upload";
        }

        private void Button_Airtable_Download_Clicked(object sender, EventArgs e)
        {
            AirtableFetchRecords();
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
                    Task<AirtableListRecordsResponse> task = airtableBase.ListRecords(
                           "Match",
                           offset,
                           null /*fieldsArray*/,
                           $"EventKey='{App.currFRCEventKey}'" /*filterByFormula*/,
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
                Label_Results.Text = errorMessage;
            }
            else
            {
                foreach (AirtableRecord ar in records)
                {
                    bool addComma = false;
                    Label_Results.Text += "{";
                    foreach (KeyValuePair<string, object> kv in ar.Fields)
                    {
                        if (addComma)
                        {
                            Label_Results.Text +=",";
                        }
                        addComma = true;
                        Label_Results.Text += $"\"{kv.Key}\":";
                        switch (Type.GetTypeCode(kv.Value.GetType()))
                        {
                            case TypeCode.String:
                                Label_Results.Text += $"\"{kv.Value}\"";
                                break;
                            default:
                                Label_Results.Text += $"{kv.Value}";
                                break;
                        }
                    }
                    Label_Results.Text += "}\r\n";
                }
            }
        }
    }
}
