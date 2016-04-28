﻿//----------------------------------------------------------------------- 
// PDS.Witsml.Server, 2016.1
//
// Copyright 2016 Petrotechnical Data Systems
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-----------------------------------------------------------------------

using System.ComponentModel.Composition;
using Energistics.DataAccess.WITSML141;
using PDS.Framework;

namespace PDS.Witsml.Server.Data.Logs
{
    /// <summary>
    /// Provides mappings for <see cref="Log" /> data object types.
    /// </summary>
    /// <seealso cref="PDS.Witsml.Server.Data.DataObjectMapper" />
    [ExportType(typeof(Log141Mapper), typeof(DataObjectMapper))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class Log141Mapper : DataObjectMapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Log141Mapper"/> class.
        /// </summary>
        /// <param name="container">The composition container.</param>
        [ImportingConstructor]
        public Log141Mapper(IContainer container) : base(container, typeof(Log))
        {
        }
    }
}