﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
namespace SharedInterfaces
{
    /// <summary>
    ///     Allows an object to implement a DataAdapter, and represents a set of methods
    ///     and mapping action-related properties that are used to fill and update a System.Data.DataSet
    /// </summary>
    public interface IDataFetcher
    {
        IEnumerable<IChangeableData> GetData();
        
    }


}
