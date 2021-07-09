﻿using System;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Models.Repositories.AppStateRepository
{
    public interface IAppStateEntryRepository
    {
        public Task UpdateValueAsync(string key, string value);

        public Task<(DateTime lastUpdate, DateTime lastChange)> GetLastUpdateAndLastChangeDatesAsync();

        public Task UpdateLastChangeTimeAsync();
    }
}
