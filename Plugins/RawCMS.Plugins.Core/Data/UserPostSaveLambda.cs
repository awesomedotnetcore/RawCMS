﻿//******************************************************************************
// <copyright file="license.md" company="RawCMS project  (https://github.com/arduosoft/RawCMS)">
// Copyright (c) 2019 RawCMS project  (https://github.com/arduosoft/RawCMS)
// RawCMS project is released under GPL3 terms, see LICENSE file on repository root at  https://github.com/arduosoft/RawCMS .
// </copyright>
// <author>Daniele Fontani, Emanuele Bucarelli, Francesco Mina'</author>
// <autogenerated>true</autogenerated>
//******************************************************************************
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RawCMS.Library.Core;
using RawCMS.Library.DataModel;
using RawCMS.Library.Service;
using RawCMS.Plugins.Core.Stores;
using System.Collections.Generic;

namespace RawCMS.Plugins.Core.Data
{
    public class UserPostsaveLambda : PostSaveLambda
    {
        public override string Name => "User Presave lambda";

        public override string Description => "provide normalized name and prevent password change";

        private readonly CRUDService service;

        public UserPostsaveLambda(CRUDService service)
        {
            this.service = service;
        }

        public override void Execute(string collection, ref JObject item, ref Dictionary<string, object> dataContext)
        {
            if (collection == "_users")
            {
                if (dataContext.ContainsKey("NewPassword"))
                {
                    string id = item["_id"].Value<string>();
                    JObject o = new JObject
                    {
                        ["UserId"] = id,
                        ["PasswordHash"] = RawUserStore.ComputePasswordHash(dataContext["NewPassword"] as string)
                    };

                    //Password cant' be changed during update
                    if (item.ContainsKey("_id") && !string.IsNullOrWhiteSpace(item["_id"].Value<string>()))
                    {
                        DataQuery query = new DataQuery()
                        {
                            RawQuery = JsonConvert.SerializeObject(new { UserId = id })
                        };

                        ItemList password = service.Query(collection, query);

                        if (password.Items.HasValues)
                        {
                            o["_id"] = password.Items[0]["_id"].Value<string>();
                            //patch password only
                            service.Update("_credentials", o, false);
                        }
                        else
                        {
                            service.Insert("_credentials", o);
                        }
                    }
                    else
                    {
                        service.Insert("_credentials", o);
                    }
                }
            }
        }
    }
}