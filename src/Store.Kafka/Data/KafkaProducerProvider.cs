﻿//----------------------------------------------------------------------- 
// PDS WITSMLstudio Core, 2018.3
//
// Copyright 2018 PDS Americas LLC
// 
// Licensed under the PDS Open Source WITSML Product License Agreement (the
// "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
//     http://www.pds.group/WITSMLstudio/OpenSource/ProductLicenseAgreement
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-----------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;
using Confluent.Kafka;

namespace PDS.WITSMLstudio.Store.Data
{
    [Export(typeof(IKafkaProducerProvider))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class KafkaProducerProvider : IKafkaProducerProvider
    {
        private readonly Lazy<IProducer<string, string>> _producer;

        [ImportingConstructor]
        public KafkaProducerProvider()
        {
            _producer = new Lazy<IProducer<string, string>>(BuildProducer);
        }

        public IProducer<string, string> Producer => _producer.Value;

        protected virtual IProducer<string, string> BuildProducer()
        {
            var config = KafkaUtil.CreateProducerConfig();

            var producer = new ProducerBuilder<string, string>(config)
                .SetErrorHandler(KafkaUtil.OnClientError)
                .SetLogHandler(KafkaUtil.OnClientWarning)
                .Build();

            return producer;
        }
    }
}
