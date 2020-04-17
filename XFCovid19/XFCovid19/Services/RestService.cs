using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XFCovid19.Helpers;
using XFCovid19.Interfaces;
using XFCovid19.Models;

namespace XFCovid19.Services
{
    public class RestService : IRestService
    {
        public async Task<GlobalTotals> GetGlobalTotals()
        {
            await Task.Delay(2000);

            try
            {
                var response = await Constants.BASE_URL
                    .AppendPathSegment("all")
                    .WithTimeout(TimeSpan.FromSeconds(30))
                    .GetJsonAsync<GlobalTotals>();

                return response;
            }
            catch (FlurlHttpException ex)
            {
                var error = ex.Message;
                return null;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                return null;
            }
        }

        public async Task<Country> GetTotalsByCountry(string countryISO)
        {
            await Task.Delay(2000);

            try
            {
                var response = await Constants.BASE_URL
                    .AppendPathSegment($"countries/{countryISO}")
                    .WithTimeout(TimeSpan.FromSeconds(30))
                    .GetJsonAsync<Country>();

                return response;
            }
            catch (FlurlHttpException ex)
            {
                var error = ex.Message;
                return null;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                return null;
            }
        }

        public async Task<IEnumerable<Country>> GetAllCountries()
        {
            await Task.Delay(2000);

            try
            {
                var response = await Constants.BASE_URL
                    .AppendPathSegment("countries")
                    .WithTimeout(TimeSpan.FromSeconds(30))
                    .GetJsonAsync<IList<Country>>();

                return response;
            }
            catch (FlurlHttpException ex)
            {
                var error = ex.Message;
                return null;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                return null;
            }
        }
    }
}
