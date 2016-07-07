using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petecat.Restful
{
    public interface IBizUnit
    {
        string CountryCodeKey
        {
            get;
        }

        string CompanyCodeKey
        {
            get;
        }

        string LanguageCodeKey
        {
            get;
        }

        string CurrencyExchangeRateKey
        {
            get;
        }

        string CurrencyCodeKey
        {
            get;
        }

        string LanguageIdKey
        {
            get;
        }

        string Name
        {
            get;
        }

        string CountryCode
        {
            get;
        }

        int CompanyCode
        {
            get;
        }

        string LanguageCode
        {
            get;
        }

        string TwoLetterLanguageCode
        {
            get;
        }

        string CurrencyExchangeRate
        {
            get;
        }

        string CurrencyCode
        {
            get;
        }

        string LanguageId
        {
            get;
        }
    }
}
