﻿using Orion.WeatherApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.WeatherApi.Repository
{
    public interface IHistoryRepository
    {
        void AddHistory(HystoryModel history);
    }
}
