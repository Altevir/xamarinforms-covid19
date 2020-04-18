using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using XFCovid19.Helpers;
using XFCovid19.Interfaces;
using XFCovid19.Models;
using XFCovid19.Services;
using XFCovid19.ViewModel;

namespace XFCovid19.ViewModels
{
    public class SearchCountryViewModel : BaseViewModel
    {
        public SearchCountryViewModel(IRestService restService)
        {
            _service = restService;
            ClosePopUpCommand = new Command(async () => await ExecuteClosePopUpCommand());
            ItemTresholdReachedCommand = new Command(async () => await ExecuteItemTresholdReachedCommand());
            SearchCountryCommand = new Command(ExecuteSearchCountryCommand);
            CountrySelectedCommand = new Command<Country>(async (args) => await ExecuteCountrySelectedCommand(args));
            Countries = new ObservableCollection<Country>();
            _db = new LiteDbService<Country>();
            CurrentPage = 2;
            ItemTreshold = 1;
            SetDarkTheme();
        }

        private readonly IRestService _service;
        private LiteDbService<Country> _db;
        public Command ClosePopUpCommand { get; }
        public Command ItemTresholdReachedCommand { get; }
        public Command SearchCountryCommand { get; }
        public Command CountrySelectedCommand { get; }
        public ObservableCollection<Country> Countries { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        private int _itemTreshold;
        public int ItemTreshold
        {
            get { return _itemTreshold; }
            set { SetProperty(ref _itemTreshold, value); }
        }

        private string _seatchText;
        public string SearchText
        {
            get { return _seatchText; }
            set
            {
                SetProperty(ref _seatchText, value);
                if (string.IsNullOrEmpty(_seatchText))
                    SearchCountryCommand.Execute(_seatchText);
            }
        }

        private bool _showLoader;
        public bool ShowLoader
        {
            get { return _showLoader; }
            set { SetProperty(ref _showLoader, value); }
        }

        private string _totalCountries;
        public string TotalCountries
        {
            get { return _totalCountries; }
            set { SetProperty(ref _totalCountries, value); }
        }

        private bool _showInfo;
        public bool ShowInfo
        {
            get { return _showInfo; }
            set { SetProperty(ref _showInfo, value); }
        }

        private Country _countrySelected;
        public Country CountrySelected
        {
            get { return _countrySelected; }
            set { SetProperty(ref _countrySelected, value); }
        }

        private string _countriesFacing;
        public string CountriesFacing
        {
            get { return _countriesFacing; }
            set { SetProperty(ref _countriesFacing, value); }
        }

        private string _selectOneCountryList;
        public string SelectOneCountryList
        {
            get { return _selectOneCountryList; }
            set { SetProperty(ref _selectOneCountryList, value); }
        }

        private string _messageLoader;
        public string MessageLoader
        {
            get { return _messageLoader; }
            set { SetProperty(ref _messageLoader, value); }
        }

        private bool _isVisibleActivityIndicator;
        public bool IsVisibleActivityIndicator
        {
            get { return _isVisibleActivityIndicator; }
            set { SetProperty(ref _isVisibleActivityIndicator, value); }
        }

        private async Task ExecuteItemTresholdReachedCommand()
        {
            try
            {
                if (ItemTreshold == -1)
                {
                    IsVisibleActivityIndicator = false;
                    return;
                }

                IsVisibleActivityIndicator = true;

                if (CurrentPage < TotalPages)
                {
                    IEnumerable<Country> countries;
                    if (App.AppCultureInfo.Equals("pt"))
                        countries = _db.FindAll()
                            .OrderBy(p => p.countryPtBR)
                            .Skip((CurrentPage - 1) * 20)
                            .Take(20);
                    else
                        countries = _db.FindAll()
                            .Skip((CurrentPage - 1) * 20)
                            .Take(20);

                    foreach (var item in countries)
                        Countries.Add(item);

                    CurrentPage++;
                }

                if (CurrentPage == TotalPages)
                    ItemTreshold = -1;

                await Task.Delay(500);
            }
            catch { }
            finally
            {
                IsVisibleActivityIndicator = false;
            }
        }

        private async Task ExecuteClosePopUpCommand()
        {
            await PopupNavigation.Instance.PopAsync(true);
        }

        private void ExecuteSearchCountryCommand()
        {
            var cultureInfoPtBR = App.AppCultureInfo.Equals("pt");
            IEnumerable<Country> countries;

            if (string.IsNullOrEmpty(SearchText))
            {
                ItemTreshold = 1;
                Countries.Clear();

                if (cultureInfoPtBR)
                    countries = _db.FindAll().OrderBy(p => p.countryPtBR).Take(20);
                else
                    countries = _db.FindAll().OrderBy(p => p.country).Take(20);

                foreach (var item in _db.FindAll().Take(20))
                    Countries.Add(item);
            }
            else
            {
                ItemTreshold = -1;
                Countries.Clear();

                try
                {
                    if (cultureInfoPtBR)
                        countries = _db.FindAll()
                            .Where(p => p.countryPtBR.ToLower().RemoveAccents().Contains(SearchText.ToLower().RemoveAccents()))
                            .OrderBy(p => p.countryPtBR);
                    else
                        countries = _db.FindAll()
                            .Where(p => p.country.ToLower().Contains(SearchText.ToLower()));

                    foreach (var item in countries)
                        Countries.Add(item);
                }
                catch (Exception ex)
                {
                    var error = ex.Message;
                }
            }
        }

        public async Task InitData()
        {
            SelectOneCountryList = TranslateExtension.TranslateText("SelectOneCountryList");
            CountriesFacing = TranslateExtension.TranslateText("CountriesFacing");
            MessageLoader = TranslateExtension.TranslateText("MessageLoader");
            ShowLoader = true;
            var globalTotals = 0;

            if (InternetConnectivity())
            {
                try
                {
                    var response = await _service.GetGlobalTotals();
                    if (response != null)
                        globalTotals = response.affectedCountries;
                }
                catch { }
            }

            if (globalTotals > _db.FindAll().Count())
            {
                var countries = await _service.GetAllCountries();
                if (countries != null)
                    Helper.UpdateDataBase(_db, countries);

                GetAllCountries();
            }
            else
            {
                GetAllCountries();
            }

            TotalPages = _db.FindAll().Count() % 20;
            TotalCountries = _db.FindAll().Count().ToString();
            ShowLoader = false;
            ShowInfo = true;
        }

        private void GetAllCountries()
        {
            Countries.Clear();
            IEnumerable<Country> countries;
            countries = App.AppCultureInfo.Equals("pt") ? _db.FindAll().OrderBy(p => p.countryPtBR).Take(20) : _db.FindAll().Take(20);
            foreach (var item in countries)
                Countries.Add(item);
        }

        private async Task ExecuteCountrySelectedCommand(Country args)
        {
            MessagingCenter.Send(this, "countrySelected", $"{args.country}:{args.countryInfo.iso2}");
            await PopupNavigation.Instance.PopAsync(true);
        }

        void SetDarkTheme()
        {
            if (Preferences.Get("appDarkTheme", false))
            {
                Application.Current.Resources["SearchPageBackgroundColor"] = Color.FromHex("#2B2B2B");
                Application.Current.Resources["SearchPageLabelTextColor"] = Color.FromHex("#D5D5D5");
                Application.Current.Resources["SearchPageActivityIndColor"] = Color.FromHex("#D5D5D5");
            }
            else
            {
                Application.Current.Resources["SearchPageBackgroundColor"] = Color.FromHex("#EEE");
                Application.Current.Resources["SearchPageLabelTextColor"] = Color.FromHex("#000000");
                Application.Current.Resources["SearchPageActivityIndColor"] = Color.FromHex("#201F4C");
            }
        }
    }
}
