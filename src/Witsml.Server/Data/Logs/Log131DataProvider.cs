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

using System.Linq;
using System.Xml.Linq;
using Energistics.DataAccess.WITSML131;
using Energistics.DataAccess.WITSML131.ReferenceData;
using PDS.Framework;
using PDS.Witsml.Data.Logs;

namespace PDS.Witsml.Server.Data.Logs
{
    /// <summary>
    /// Data provider that implements support for WITSML API functions for <see cref="Log"/>.
    /// </summary>
    public partial class Log131DataProvider
    {
        /// <summary>
        /// Sets additional default values for the specified data object.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        partial void SetAdditionalDefaultValues(Log dataObject)
        {
            // Ensure ObjectGrowing is false during AddToStore
            dataObject.ObjectGrowing = false;

            // Ensure Direction
            if (!dataObject.Direction.HasValue)
                dataObject.Direction = LogIndexDirection.increasing;

            if (dataObject.LogCurveInfo != null)
            {
                // Ensure UID
                dataObject.LogCurveInfo
                    .Where(x => string.IsNullOrWhiteSpace(x.Uid))
                    .ForEach(x => x.Uid = x.Mnemonic);

                // Ensure index curve is first
                dataObject.LogCurveInfo.MoveToFirst(dataObject.IndexCurve.Value);
            }
        }

        /// <summary>
        /// Sets additional default values for the specified data object during update.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        /// <param name="element">The element.</param>
        partial void UpdateAdditionalDefaultValues(Log dataObject, XElement element)
        {
            // Prevent unnecessary processing
            if (dataObject?.LogCurveInfo == null || !dataObject.LogCurveInfo.Any(x => string.IsNullOrWhiteSpace(x.Uid)))
                return;

            var uri = dataObject.GetUri();
            var current = DataAdapter.Get(uri);
            var ns = element.GetDefaultNamespace();

            foreach (var lci in element.Descendants(ns + "logCurveInfo"))
            {
                var uidAttribute = lci.Attribute("uid");
                var mnemonicElement = lci.Element(ns + "mnemonic");

                // Skip the curve if it has a UID or if no mnemonic was specified
                if (!string.IsNullOrWhiteSpace(uidAttribute?.Value) || string.IsNullOrWhiteSpace(mnemonicElement?.Value))
                    continue;

                // Use existing curve UID, if available; otherwise, use the mnemonic
                var curve = current.LogCurveInfo.GetByMnemonic(mnemonicElement.Value);
                var uid = curve?.Uid ?? mnemonicElement.Value;

                // Update entity with UID
                curve = dataObject.LogCurveInfo.GetByMnemonic(mnemonicElement.Value);
                if (curve != null)
                    curve.Uid = uid;

                // Create a new XAttribute or update the existing one
                if (uidAttribute == null)
                {
                    lci.Add(new XAttribute("uid", uid));
                }
                else
                {
                    uidAttribute.SetValue(uid);
                }
            }
        }
    }
}
