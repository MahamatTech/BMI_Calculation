using BMI_Calculator.Utilities;
using BMICalculation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Mahamat Adoum

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BMI_Calculator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private SolidColorBrush errorBrush = new SolidColorBrush(Windows.UI.Colors.Red);
        private Brush correctBrush = null;
        private double h;
        private double w;
        private double inch;
        private bool metricUnits;
        private bool playSpeech;
        private SpeechUtil talker;

        public MainPage()
        {
            this.InitializeComponent();
            if (correctBrush == null)
                correctBrush = txtHeight.Foreground;
            metricUnits = true;
            playSpeech = false;
            speechIcon.Source = new BitmapImage(new Uri(this.BaseUri, "/Assets/speakerMuted.png"));
            talker = new SpeechUtil();
        }

        private async void btnCalc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (await isValid())
                {
                    string category;
                    double bmi;
                    if (!metricUnits)
                    {
                        bmi = BMICalc.BmiValue((h * 12 + inch), w, SysOfUnits.Imperial, out category);
                    }
                    else
                    {
                        bmi = BMICalc.BmiValue(h, w, SysOfUnits.Metric, out category);
                    }

                    if (bmi > 50)
                    {
                        RadialGauge.Maximum = (int)(bmi / 10) * 10 + 15;
                    }
                    else
                    {
                        RadialGauge.Maximum = 50;
                    }
                    moveGage.To = bmi;
                    myStoryboard.Begin();
                    lblBMIValue.Text = "BMI: " + bmi.ToString("n2");
                    lblBMIClassification.Text = category;
                    if (playSpeech)
                    {
                        await talker.SpeakTextAsync("Body Mass Index Category is: " + category, uiMediaElement);
                    }
                }
            }
            catch (ArgumentOutOfRangeException ax)
            {
                Jeeves.ShowMessage("Calculation Error", ax.ParamName);
            }
            catch (Exception ex)
            {
                Jeeves.ShowMessage("Calculation Error", ex.Message);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            clearForm();
        }

        private void clearForm()
        {
            lblBMIClassification.Text = "";
            lblBMIValue.Text = "";
            txtHeight.Text = "";
            txtWeight.Text = "";
            txtHeight.Foreground = correctBrush;
            txtWeight.Foreground = correctBrush;
            RadialGauge.Value = 0;
            RadialGauge.Maximum = 50;
            txtInches.Text = "0";
        }
        private async Task<bool> isValid()
        {
            bool validData = true;
            //Height
            string errMessage = "Please fix the following errors:" + Environment.NewLine;
            if (String.IsNullOrEmpty(txtHeight.Text))
            {
                txtHeight.Foreground = correctBrush;
                validData = false;
                errMessage += "-No height entered" + Environment.NewLine;
            }
            else if (!double.TryParse(txtHeight.Text, out h))
            {
                txtHeight.Foreground = errorBrush;
                validData = false;
                //errMessage += "-Invalid Height" + Environment.NewLine;
            }
            else if (h <= 0)
            {
                txtHeight.Foreground = errorBrush;
                validData = false;
                errMessage += "-Height must be greater then zero" + Environment.NewLine;
            }
            else
            {
                txtHeight.Foreground = correctBrush;
            }
            //Inches if Imperial Units
            if (!metricUnits)
            {
                if (String.IsNullOrEmpty(txtInches.Text))
                {
                    txtInches.Foreground = correctBrush;
                    validData = false;
                    errMessage += "-No inches entered" + Environment.NewLine;
                }
                else if (!double.TryParse(txtInches.Text, out inch))
                {
                    txtInches.Foreground = errorBrush;
                    validData = false;
                }
                else if (inch < 0)
                {
                    txtInches.Foreground = errorBrush;
                    validData = false;
                    errMessage += "-Inches cannot be negative" + Environment.NewLine;
                }
                else if (inch > 12)
                {
                    txtInches.Foreground = errorBrush;
                    validData = false;
                    errMessage += "-You cannot enter more than 12 inches" + Environment.NewLine;
                }
                else
                {
                    txtInches.Foreground = correctBrush;
                }
            }
            //Check Weight
            if (String.IsNullOrEmpty(txtWeight.Text))
            {
                txtHeight.Foreground = correctBrush;
                validData = false;
                errMessage += "-No weight entered" + Environment.NewLine;
            }
            else if (!double.TryParse(txtWeight.Text, out w))
            {
                txtWeight.Foreground = errorBrush;
                validData = false;
                errMessage += "-Invalid Weight" + Environment.NewLine;
            }
            else if (w <= 0)
            {
                txtWeight.Foreground = errorBrush;
                validData = false;
                errMessage += "-Weight must be greater then zero" + Environment.NewLine;
            }
            else
            {
                txtWeight.Foreground = correctBrush;
            }
            if (errMessage != "Please fix the following errors:" + Environment.NewLine)
            {
                Jeeves.ShowMessage("Invalid Data", errMessage);
                if (playSpeech)
                {
                    await talker.SpeakTextAsync(errMessage, uiMediaElement);
                }
            }

            return validData;
        }

        private void toggleButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            metricUnits = !metricUnits;
            if (!metricUnits)
            {
                lblUnits.Text = "Imperial (Feet,Lbs.)";
                lblHeight.Text = "Height (feet)";
                lblWeight.Text = "Weight (lbs.)";
                heightInches.Visibility = Visibility.Visible;
                txtInches.Text = "0";
            }
            else
            {
                lblUnits.Text = "Metric (Meters,Kg.)";
                lblHeight.Text = "Height (meters)";
                lblWeight.Text = "Weight (kg)";
                heightInches.Visibility = Visibility.Collapsed;
            }
            clearForm();
        }
        private void toggleSpeech_Tapped(object sender, TappedRoutedEventArgs e)
        {
            playSpeech = !playSpeech;
            if (playSpeech)
            {
                speechIcon.Source = new BitmapImage(new Uri(this.BaseUri, "/Assets/speaker.png"));
            }
            else
            {
                speechIcon.Source = new BitmapImage(new Uri(this.BaseUri, "/Assets/speakerMuted.png"));
            }
        }
        private void txtHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtHeight.Foreground = correctBrush;
        }

        private void txtInches_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtInches.Foreground = correctBrush;
        }

        private void txtWeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtWeight.Foreground = correctBrush;
        }
    }
}
