﻿using BertScout2020Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BertScout2020.Services
{
    public class SqlDataStoreFRCEvents : IDataStore<FRCEvent>
    {
        private List<FRCEvent> items;

        public SqlDataStoreFRCEvents()
        {
        }

        private void FillList()
        {
            if (items == null)
            {
                // must complete, so don't async/await
                items = App.Database.GetEventsAsync().Result;
            }
        }

        public async Task<bool> AddItemAsync(FRCEvent item)
        {
            FillList();
            if (item.Uuid == null)
            {
                item.Uuid = Guid.NewGuid().ToString();
            }
            await App.Database.SaveFRCEventAsync(item);
            items = null;
            FillList();
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            FillList();
            var oldItem = items.Where((FRCEvent arg) => arg.Id == id).FirstOrDefault();
            await App.Database.DeleteFRCEventAsync(oldItem.Id.Value);
            items.Remove(oldItem);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string uuid)
        {
            FillList();
            var oldItem = items.Where((FRCEvent arg) => arg.Uuid == uuid).FirstOrDefault();
            await App.Database.DeleteFRCEventAsync(oldItem.Id.Value);
            items.Remove(oldItem);
            return await Task.FromResult(true);
        }

        public async Task<FRCEvent> GetItemAsync(int id)
        {
            FillList();
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<FRCEvent> GetItemAsync(string uuid)
        {
            FillList();
            return await Task.FromResult(items.FirstOrDefault(s => s.Uuid == uuid));
        }

        public async Task<FRCEvent> GetItemByKeyAsync(string key)
        {
            // key = EventKey
            FillList();
            return await Task.FromResult(items.FirstOrDefault(s => s.EventKey == key));
        }

        public async Task<IEnumerable<FRCEvent>> GetItemsAsync(bool forceRefresh = false)
        {
            FillList();
            return await Task.FromResult(items);
        }

        public async Task<bool> UpdateItemAsync(FRCEvent item)
        {
            FillList();
            var oldItem = items.Where((FRCEvent arg) => arg.Uuid == item.Uuid).FirstOrDefault();
            item.Id = oldItem.Id;
            items.Remove(oldItem);
            await App.Database.SaveFRCEventAsync(item);
            items = null;
            FillList();
            return await Task.FromResult(true);
        }
    }
}
