﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MistikRota.ViewModel
{
    public class NotifyViewModelBase<T>
    {
        public List<T> Items { get; set; } //error message obj list oldu. çünkü bu sınıf miras olarak kullanıldı.
        public string Header { get; set; }
        public string Title { get; set; }
        public bool IsRedirecting { get; set; }
        public string RedirectingUrl { get; set; }
        public int RedirectingTimeout { get; set; }

        public NotifyViewModelBase()
        {
            Header = "Yönlendiriliyorsunuz!";
            Title = "Geçersiz İşlem";
            IsRedirecting = true;
            RedirectingUrl = "/Home/Index";
            RedirectingTimeout = 10000;
            Items = new List<T>();
        }
    }
}