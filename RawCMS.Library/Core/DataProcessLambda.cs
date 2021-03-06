﻿//******************************************************************************
// <copyright file="license.md" company="RawCMS project  (https://github.com/arduosoft/RawCMS)">
// Copyright (c) 2019 RawCMS project  (https://github.com/arduosoft/RawCMS)
// RawCMS project is released under GPL3 terms, see LICENSE file on repository root at  https://github.com/arduosoft/RawCMS .
// </copyright>
// <author>Daniele Fontani, Emanuele Bucarelli, Francesco Mina'</author>
// <autogenerated>true</autogenerated>
//******************************************************************************
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace RawCMS.Library.Core
{
    public enum SavePipelineStage
    {
        PreSave = 0x0,
        PostSave = 0x1,
    }

    public abstract class DataProcessLambda : Lambda
    {
        public abstract SavePipelineStage Stage { get; }

        public abstract void Execute(string collection, ref JObject item, ref Dictionary<string, object> dataContext);
    }

    public abstract class PostSaveLambda : DataProcessLambda
    {
        public override SavePipelineStage Stage => SavePipelineStage.PostSave;
    }

    public abstract class PreSaveLambda : DataProcessLambda
    {
        public override SavePipelineStage Stage => SavePipelineStage.PreSave;
    }
}