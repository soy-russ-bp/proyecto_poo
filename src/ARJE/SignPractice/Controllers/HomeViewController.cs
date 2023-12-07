﻿using ARJE.SignPractice.ViewModels;
using ARJE.SignPractice.Views;
using ARJE.Utils.Avalonia.ReactiveUI.MVC.Controllers;
using ARJE.Utils.Avalonia.ReactiveUI.MVC.DataModels;

namespace ARJE.SignPractice.Controllers
{
    public sealed class HomeViewController : ViewControllerBase<HomeView, NoDataModel, HomeViewModel>
    {
        public HomeViewController()
            : base(NoDataModel.None, new HomeViewModel())
        {
        }
    }
}
