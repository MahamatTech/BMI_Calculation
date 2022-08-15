using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace BMI_Calculator.Utilities
{
    internal static class Jeeves
    {
        internal async static void ShowMessage(string strTitle, string Msg)
        {
            ContentDialog diag = new ContentDialog()
            {
                Title = strTitle,
                Content = Msg,
                PrimaryButtonText = "Ok"
            };
            await diag.ShowAsync();
        }
    }
}
