﻿
using App1.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        internal static object Create(object myRealmAppId)
        {
            throw new NotImplementedException();
        }

        protected override void OnResume()
        {
        }
    }
}
