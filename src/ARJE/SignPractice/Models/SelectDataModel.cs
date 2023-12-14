﻿using ARJE.Shared.Models;
using ARJE.Utils.Avalonia.MVC.Models;

namespace ARJE.SignPractice.Models
{
    public sealed record SelectDataModel(OnDiskModelTrainingConfigCollection ConfigCollection)
        : DataModelBase
    {
    }
}
