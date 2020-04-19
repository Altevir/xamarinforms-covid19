using Rg.Plugins.Popup.Extensions;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using XFCovid19.Helpers;
using XFCovid19.Interfaces;
using XFCovid19.Models;
using XFCovid19.Services;
using XFCovid19.ThemeResources;
using XFCovid19.ViewModel;
using XFCovid19.Views;

namespace XFCovid19.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel(IRestService restService)
        {
            _service = restService;
            _dbGlobal = new LiteDbService<GlobalTotals>();
            _db = new LiteDbService<Country>();
            ChangeCountryCommand = new Command(async (args) => await ExecuteChangeCountryCommand());
            RefreshGlobalCommand = new Command(async () => await ExecuteRefreshGlobalCommand());
            RefreshCountryCommand = new Command(async () => await ExecuteRefreshCountryCommand());
            NavigateToSearchCountryCommand = new Command(async () => await ExecuteNavigateToSearchCountryCommand());
            NavigateToReadMorePageCommand = new Command(async () => await ExecuteNavigateToReadMorePageCommand());
            ShowChangeLanguagePagePopUpCommand = new Command(async () => await ExecuteShowChangeLanguagePagePopUpCommand());
            ChangeThemeAppCommand = new Command(ExecuteChangeThemeAppCommand);
            GetAppVersion();
        }

        private readonly IRestService _service;
        public Command ChangeCountryCommand { get; }
        public Command RefreshGlobalCommand { get; }
        public Command RefreshCountryCommand { get; }
        public Command NavigateToSearchCountryCommand { get; }
        public Command NavigateToReadMorePageCommand { get; }
        public Command ShowChangeLanguagePagePopUpCommand { get; }
        public Command ChangeThemeAppCommand { get; }

        public string CountryISO2 { get; set; }
        private LiteDbService<Country> _db;
        private LiteDbService<GlobalTotals> _dbGlobal;

        private string _lastUpdateHeader;
        public string LastUpdateHeader
        {
            get { return _lastUpdateHeader; }
            set { SetProperty(ref _lastUpdateHeader, value); }
        }

        private string _globalUpdateHeader;
        public string GlobalUpdateHeader
        {
            get { return _globalUpdateHeader; }
            set { SetProperty(ref _globalUpdateHeader, value); }
        }

        private string _globalConfirmed;
        public string GlobalConfirmed
        {
            get { return _globalConfirmed; }
            set { SetProperty(ref _globalConfirmed, value); }
        }

        private string _globalRecovered;
        public string GlobalRecovered
        {
            get { return _globalRecovered; }
            set { SetProperty(ref _globalRecovered, value); }
        }

        private string _globalDeaths;
        public string GlobalDeaths
        {
            get { return _globalDeaths; }
            set { SetProperty(ref _globalDeaths, value); }
        }

        private string _iconArrow;
        public string IconArrow
        {
            get { return _iconArrow; }
            set { SetProperty(ref _iconArrow, value); }
        }

        private string _flagCode;
        public string FlagCode
        {
            get { return _flagCode; }
            set { SetProperty(ref _flagCode, value); }
        }

        private string _lastUpdateSubtitleCountry;
        public string LastUpdateSubtitleCountry
        {
            get { return _lastUpdateSubtitleCountry; }
            set { SetProperty(ref _lastUpdateSubtitleCountry, value); }
        }

        private string _countryConfirmed;
        public string CountryConfirmed
        {
            get { return _countryConfirmed; }
            set { SetProperty(ref _countryConfirmed, value); }
        }

        private string _countryRecovered;
        public string CountryRecovered
        {
            get { return _countryRecovered; }
            set { SetProperty(ref _countryRecovered, value); }
        }

        private string _countryDeaths;
        public string CountryDeaths
        {
            get { return _countryDeaths; }
            set { SetProperty(ref _countryDeaths, value); }
        }

        private string _countryFlag;
        public string CountryFlag
        {
            get { return _countryFlag; }
            set { SetProperty(ref _countryFlag, value); }
        }

        private string _countryNameSelected;
        public string CountryNameSelected
        {
            get { return _countryNameSelected; }
            set { SetProperty(ref _countryNameSelected, value); }
        }

        private string _appVersion;
        public string AppVersion
        {
            get { return _appVersion; }
            set { SetProperty(ref _appVersion, value); }
        }

        private string _confirmedHeader;
        public string ConfirmedHeader
        {
            get { return _confirmedHeader; }
            set { SetProperty(ref _confirmedHeader, value); }
        }

        private string _recoveredHeader;
        public string RecoveredHeader
        {
            get { return _recoveredHeader; }
            set { SetProperty(ref _recoveredHeader, value); }
        }

        private string _deathsHeader;
        public string DeathsHeader
        {
            get { return _deathsHeader; }
            set { SetProperty(ref _deathsHeader, value); }
        }

        private string _refreshHeader;
        public string RefreshHeader
        {
            get { return _refreshHeader; }
            set { SetProperty(ref _refreshHeader, value); }
        }

        private string _learnAboutCovid;
        public string LearnAboutCovid
        {
            get { return _learnAboutCovid; }
            set { SetProperty(ref _learnAboutCovid, value); }
        }

        private string _readMoreCovid;
        public string ReadMoreCovid
        {
            get { return _readMoreCovid; }
            set { SetProperty(ref _readMoreCovid, value); }
        }

        private string _hashTagCovid;
        public string HashTagCovid
        {
            get { return _hashTagCovid; }
            set { SetProperty(ref _hashTagCovid, value); }
        }

        public bool AppDarkTheme
        {
            get => Preferences.Get("appDarkTheme", false);
            set => Preferences.Set("appDarkTheme", value);
        }

        private GlobalTotals GlobalTotals { get; set; }
        private Country Country { get; set; }

        public async Task GetGlobalTotals(bool isBusyCountry = false)
        {
            IsBusy = true;
            IsBusyCountry = isBusyCountry;

            try
            {
                if (!InternetConnectivity())
                {
                    var response = _dbGlobal.FindAll().FirstOrDefault(p => p.globalKey.Equals("global"));
                    await Task.Delay(2000);
                    SetTotalGlobal(response, true);
                }
                else
                {
                    var response = await _service.GetGlobalTotals();
                    SetTotalGlobal(response);
                }
            }
            catch { }
            finally
            {
                IsBusy = false;
                IsBusyCountry = false;
            }
        }

        public async Task GetTotalsByCountry(string countryISO2)
        {
            if (IsBusyCountry)
                return;

            IsBusyCountry = true;
            CountryFlag = string.Empty;

            try
            {
                if (!InternetConnectivity())
                {
                    var response = _db.FindAll().FirstOrDefault(p => p.countryInfo.iso2.ToUpper() == countryISO2.ToUpper());
                    await Task.Delay(2000);
                    SetTotalCountry(response);
                }
                else
                {
                    var response = await _service.GetTotalsByCountry(countryISO2);
                    SetTotalCountry(response);
                }
            }
            catch { }
            finally
            {
                IsBusyCountry = false;
            }
        }

        private void SetTotalGlobal(GlobalTotals response, bool appOffLine = false)
        {
            if (response != null)
            {
                GlobalTotals = response;

                if (!appOffLine)
                {
                    response.globalKey = "global";
                    _dbGlobal.UpsertItem(response);
                }

                LastUpdateHeader = $"{TranslateExtension.TranslateText("MainHeaderSubtitle")} {GlobalTotals.updated.TransformLongToDateTime().FormatDateTime(App.AppCultureInfo)}";
                GlobalConfirmed = response.cases.TransformNumberToString();
                GlobalRecovered = response.recovered.TransformNumberToString();
                GlobalDeaths = response.deaths.TransformNumberToString();
            }
            else
            {
                LastUpdateHeader = "------------------------------";
                GlobalConfirmed = "-";
                GlobalRecovered = "-";
                GlobalDeaths = "-";
            }
        }

        private void SetTotalCountry(Country response)
        {
            if (response != null)
            {
                Country = response;
                LastUpdateSubtitleCountry = $"{TranslateExtension.TranslateText("CountrySubtitle")} {Country.updated.TransformLongToDateTime().FormatDateTime(App.AppCultureInfo)}";
                CountryConfirmed = response.cases.TransformNumberToString();
                CountryRecovered = response.recovered?.TransformNumberToString();
                CountryDeaths = response.deaths.TransformNumberToString();
                CountryFlag = response.countryInfo.flag;
                CountryNameSelected = TranslateExtension.TranslateText(Country.country);
            }
            else
            {
                LastUpdateSubtitleCountry = "------------------------------";
                CountryConfirmed = "-";
                CountryRecovered = "-";
                CountryDeaths = "-";
            }
        }

        private async Task GetAllCountries()
        {
            try
            {
                var response = await _service.GetAllCountries();
                if (response != null)
                {
                    foreach (var item in response)
                    {
                        //NÃO SÃO PAÍSES, SÃO NAVIOS DE CRUZEIRO o.O
                        if (item.country.ToLower().Contains("zaandam") ||
                            item.country.ToLower().Contains("diamond princess"))
                            continue;

                        item.countryPtBR = TranslateExtension.TranslateText(item.country);
                        _db.UpsertItem(item);
                    }
                }
            }
            catch { }
        }

        public async Task GetAll()
        {
            InitData();
            await GetGlobalTotals(true);
            await GetTotalsByCountry(CountryISO2);
            await GetAllCountries();
        }

        private void GetCountryByISO2(string countryISO2)
        {
            IconArrow = !AppDarkTheme ? "ic_chevron_down" : "ic_chevron_down_white";
            CountryNameSelected = TranslateExtension.TranslateText(countryISO2.Split(':')[0]);
            CountryISO2 = countryISO2.Split(':')[1];
            ChangeCountryCommand.Execute(CountryISO2);
        }

        private void ChangeLanguageApp()
        {
            Title = TranslateExtension.TranslateText("MainHeader");
            LastUpdateHeader = $"{TranslateExtension.TranslateText("MainHeaderSubtitle")} {GlobalTotals.updated.TransformLongToDateTime().FormatDateTime(App.AppCultureInfo)}";
            GlobalUpdateHeader = TranslateExtension.TranslateText("GlobalUpdateHeader");
            ConfirmedHeader = TranslateExtension.TranslateText("ConfirmedHeader");
            RecoveredHeader = TranslateExtension.TranslateText("RecoveredHeader");
            DeathsHeader = TranslateExtension.TranslateText("DeathsHeader");
            RefreshHeader = TranslateExtension.TranslateText("RefreshHeader");
            CountryNameSelected = TranslateExtension.TranslateText(Country.country);
            LastUpdateSubtitleCountry = $"{TranslateExtension.TranslateText("MainHeaderSubtitle")} {Country.updated.TransformLongToDateTime().FormatDateTime(App.AppCultureInfo)}";
            LearnAboutCovid = TranslateExtension.TranslateText("LearnAboutCovid");
            ReadMoreCovid = TranslateExtension.TranslateText("ReadMoreCovid");
            HashTagCovid = TranslateExtension.TranslateText("HashTagCovid");
        }

        private async Task ExecuteChangeCountryCommand()
        {
            await GetTotalsByCountry(CountryISO2);
        }

        private async Task ExecuteRefreshGlobalCommand()
        {
            if (!InternetConnectivity())
                await Navigation.PushPopupAsync(new InternetOffLinePage());

            await GetGlobalTotals();
        }

        private async Task ExecuteRefreshCountryCommand()
        {
            if (!InternetConnectivity())
                await Navigation.PushPopupAsync(new InternetOffLinePage());

            await GetTotalsByCountry(CountryISO2);
        }

        private async Task ExecuteNavigateToSearchCountryCommand()
        {
            if (IsTouched)
                return;

            IsTouched = true;
            IconArrow = !AppDarkTheme ? "ic_chevron_up" : "ic_chevron_up_white";
            await Navigation.PushPopupAsync(new SearchCountryPage());
            IconArrow = !AppDarkTheme ? "ic_chevron_down" : "ic_chevron_down_white";
            IsTouched = false;
        }

        private async Task ExecuteNavigateToReadMorePageCommand()
        {
            if (IsTouched)
                return;

            IsTouched = true;
            await Navigation.PushPopupAsync(new ReadMorePage());
            IsTouched = false;
        }

        void InitData()
        {
            Title = TranslateExtension.TranslateText("MainHeader");
            GlobalUpdateHeader = TranslateExtension.TranslateText("GlobalUpdateHeader");
            RefreshHeader = TranslateExtension.TranslateText("RefreshHeader");
            ConfirmedHeader = TranslateExtension.TranslateText("ConfirmedHeader");
            RecoveredHeader = TranslateExtension.TranslateText("RecoveredHeader");
            DeathsHeader = TranslateExtension.TranslateText("DeathsHeader");
            CountryISO2 = App.AppCultureInfo.Equals("pt") ? "BR" : "USA";
            LastUpdateHeader = "------------------------------";
            GlobalConfirmed = "-";
            GlobalRecovered = "-";
            GlobalDeaths = "-";
            LastUpdateSubtitleCountry = "------------------------------";
            CountryConfirmed = "-";
            CountryRecovered = "-";
            CountryDeaths = "-";
            LearnAboutCovid = TranslateExtension.TranslateText("LearnAboutCovid");
            ReadMoreCovid = TranslateExtension.TranslateText("ReadMoreCovid");
            HashTagCovid = TranslateExtension.TranslateText("HashTagCovid");
            SetDarkTheme(AppDarkTheme);
        }

        void GetAppVersion()
        {
            AppVersion = $"v{VersionTracking.CurrentVersion}";
        }

        private async Task ExecuteShowChangeLanguagePagePopUpCommand()
        {
            if (IsTouched)
                return;

            IsTouched = true;
            await Navigation.PushPopupAsync(new ChangeLanguagePage());
            IsTouched = false;
        }

        public void SubscribeChangeLanguage()
        {
            MessagingCenter.Subscribe<ChangeLanguagePageViewModel>(this, "changeLanguage", (s) =>
            {
                ChangeLanguageApp();
            });

            MessagingCenter.Subscribe<ChangeLanguagePageViewModel, bool>(this, "changeAppTheme", (s, param) =>
            {
                AppDarkTheme = param;
                SetDarkTheme(param);
            });

            MessagingCenter.Subscribe<SearchCountryViewModel, string>(this, "countrySelected", (s, param) =>
            {
                GetCountryByISO2(param);
            });
        }

        public void UnsubscribeEvents()
        {
            MessagingCenter.Unsubscribe<ChangeLanguagePageViewModel>(this, "changeLanguage");
            MessagingCenter.Unsubscribe<ChangeLanguagePageViewModel>(this, "changeAppTheme");
            MessagingCenter.Unsubscribe<SearchCountryViewModel>(this, "countrySelected");
        }

        private void ExecuteChangeThemeAppCommand()
        {
            SetDarkTheme(true);
        }

        void SetDarkTheme(bool darkTheme)
        {
            if (Device.RuntimePlatform == Device.iOS)
                DependencyService.Get<IStatusBarStyle>().ChangeTextColor(darkTheme);

            if (darkTheme)
            {
                IconArrow = "ic_chevron_down_white";
                Application.Current.Resources = new DarkTheme();
            }
            else
            {
                IconArrow = "ic_chevron_down";

                if (Application.Current.Resources.Source != null &&
                    Application.Current.Resources.Source.OriginalString.ToLower().Contains("light"))
                    return;

                Application.Current.Resources = new LightTheme();
            }
        }

        public override void OnDisappearing()
        {
            UnsubscribeEvents();
        }
    }
}
