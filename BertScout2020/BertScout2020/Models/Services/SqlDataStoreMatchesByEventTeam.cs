﻿using BertScout2020Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BertScout2020.Services
{
    public class SqlDataStoreMatchesByEventTeam : IDataStore<EventTeamMatch>
    {
        private string _frcEventKey;
        private int _teamNumber;
        private List<EventTeamMatch> items;

        public SqlDataStoreMatchesByEventTeam(string frcEventKey, int teamNumber)
        {
            _frcEventKey = frcEventKey;
            _teamNumber = teamNumber;
        }

        private void FillList()
        {
            if (items == null)
            {
                // must complete, so don't async/await
                items = App.Database.GetEventTeamMatchesAsync(_frcEventKey, _teamNumber).Result;
            }
        }

        public async Task<bool> AddItemAsync(EventTeamMatch item)
        {
            FillList();
            if (item.Uuid == null)
            {
                item.Uuid = Guid.NewGuid().ToString();
            }
            await App.Database.SaveEventTeamMatchAsync(item);
            items.Add(item);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            FillList();
            var oldItem = items.Where((EventTeamMatch arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string uuid)
        {
            FillList();
            var oldItem = items.Where((EventTeamMatch arg) => arg.Uuid == uuid).FirstOrDefault();
            items.Remove(oldItem);
            return await Task.FromResult(true);
        }

        public async Task<EventTeamMatch> GetItemAsync(int id)
        {
            FillList();
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<EventTeamMatch> GetItemAsync(string uuid)
        {
            FillList();
            return await Task.FromResult(items.FirstOrDefault(s => s.Uuid == uuid));
        }

        public async Task<EventTeamMatch> GetItemAsync(string eventKey, int teamNumber, int matchNumber)
        {
            FillList();
            return await Task.FromResult(items.FirstOrDefault(s => s.EventKey == eventKey
                                                              && s.TeamNumber == teamNumber
                                                              && s.MatchNumber == matchNumber));
        }

        public Task<EventTeamMatch> GetItemByKeyAsync(string key)
        {
            // key = EventKey|TeamNumber|MatchNumber
            FillList();
            string[] keys = key.Split('|');
            //todo /*await*/
            return Task.FromResult(items.FirstOrDefault(s => s.EventKey == keys[0]
                                                              && s.TeamNumber == int.Parse(keys[1])
                                                              && s.MatchNumber == int.Parse(keys[2])));
        }

        public async Task<IEnumerable<EventTeamMatch>> GetItemsAsync(bool forceRefresh = false)
        {
            FillList();
            return await Task.FromResult(items);
        }

        public async Task<bool> UpdateItemAsync(EventTeamMatch item)
        {
            FillList();
            var oldItem = items.Where((EventTeamMatch arg) => arg.Uuid == item.Uuid).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);
            return await Task.FromResult(true);
        }
    }
}
