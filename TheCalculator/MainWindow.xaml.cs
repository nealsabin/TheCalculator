using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TheCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private enum tempUnits { fahrenheit, celsius }

        tempUnits _tunits;

        private enum windUnits { mph, kph }

        windUnits _wunits;

        public MainWindow()
        {
            InitializeComponent();
            InitializeWindow();
        }

        private void InitializeWindow()
        {
            TextBox_Temperature.Focus();

            UpdateUnits();
        }

        //Exit Button
        private void Button_ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        //Help Button
        private void Button_HelpButton_Click(object sender, RoutedEventArgs e)
        {
            Help helpWindow = new Help();
            helpWindow.ShowDialog();
        }

        //Calculate Wind Chill and display result
        private void Button_Calculate_Click(object sender, RoutedEventArgs e)
        {
            UpdateUnits();

            //calculate wind chill
            if (ValidInputs(out string errorMessage))
            {
                double windChillFahrenheit;
                double windChillCelsius;
                double power = 0.16;

                double fahrenheitTemp = 0;
                double celsiusTemp = 0;
                double mphWind = 0;
                double kphWind = 0;

                double inputTemp = Convert.ToDouble(TextBox_Temperature.Text);
                double inputWind = Convert.ToDouble(TextBox_Wind.Text);

                // convert temperatures
                if (_tunits == tempUnits.fahrenheit)
                {
                    celsiusTemp = ((inputTemp - 32) / 1.8);
                    fahrenheitTemp = inputTemp;
                }
                else if (_tunits == tempUnits.celsius)
                {
                    fahrenheitTemp = ((inputTemp * 1.8) + 32);
                    celsiusTemp = inputTemp;
                }

                //convert wind velocities
                if (_wunits == windUnits.mph)
                {
                    kphWind = inputWind * 1.609344;
                    mphWind = inputWind;
                }
                else if (_wunits == windUnits.kph)
                {
                    mphWind = inputWind / 1.609344;
                    kphWind = inputWind;
                }

                windChillFahrenheit = (35.74 + (0.6215 * fahrenheitTemp)) - (35.75 * Math.Pow(mphWind, power)) + (0.4275 * fahrenheitTemp * Math.Pow(mphWind, power));
                TextBox_Calculate_Fahrenheit.Text = String.Format("{0:0.##}", windChillFahrenheit);

                windChillCelsius = (13.12 + (0.6215 * celsiusTemp)) - (11.37 * Math.Pow(kphWind, power)) + (0.3965 * celsiusTemp * Math.Pow(kphWind, power));
                TextBox_Calculate_Celsius.Text = String.Format("{0:0.##}", windChillCelsius);
            }
            else
            {
                MessageBox.Show(errorMessage);
            }
        }

        //Validate Inputs
        private bool ValidInputs(out string errorMessage)
        {
            UpdateUnits();

            bool validInputs = true;
            errorMessage = "";

            if (!int.TryParse(TextBox_Temperature.Text, out int temperatureValidF))
            {
                validInputs = false;
                errorMessage += "Please enter a valid number for temperature" + Environment.NewLine;
            }
            else if(int.TryParse(TextBox_Temperature.Text, out int temperatureValidC))
            {
                if (_tunits == tempUnits.fahrenheit && Convert.ToInt32(TextBox_Temperature.Text) >= 51)
                {
                    validInputs = false;
                    errorMessage += "Please enter a valid temperature that is less than 51°F." + Environment.NewLine;
                }
                if (_tunits == tempUnits.celsius && Convert.ToInt32(TextBox_Temperature.Text) > 10)
                {
                    validInputs = false;
                    errorMessage += "Please enter a valid temperature that is less than or equal to 10°C." + Environment.NewLine;
                }
            }

            if (!int.TryParse(TextBox_Wind.Text, out int wind))
            {
                validInputs = false;
                errorMessage += "Please enter a valid number for wind speed" + Environment.NewLine;
            }
            else if(int.TryParse(TextBox_Wind.Text, out int windValid))
            {
                if (_wunits == windUnits.mph && Convert.ToInt32(TextBox_Wind.Text) <= 3)
                {
                    validInputs = false;
                    errorMessage += "Please enter a valid wind speed that is at least 4 MPH." + Environment.NewLine;
                }
                if (_wunits == windUnits.kph && Convert.ToInt32(TextBox_Wind.Text) < 4.8)
                {
                    validInputs = false;
                    errorMessage += "Please enter a valid wind speed that is less than 4.8 KPH." + Environment.NewLine;
                }
            }

            return validInputs;


            //if (!int.TryParse(TextBox_Temperature.Text, out int temperatureCelsius) && _tunits == tempUnits.celsius)
            //{
            //    validInputs = false;
            //    errorMessage += "Please enter a valid temperature that is less than 11°C." + Environment.NewLine;
            //}


            //if (!int.TryParse(TextBox_Wind.Text, out int wind) || Convert.ToInt32(TextBox_Wind.Text) <= 3)
            //{
            //    validInputs = false;
            //    errorMessage += "Please enter a valid wind speed that is at least 4 MPH." + Environment.NewLine;
            //}


        }

        //Update Units
        private void UpdateUnits()
        {
            if ((bool)RadioButton_Fahrenheit.IsChecked)
            {
                _tunits = tempUnits.fahrenheit;
            }
            else if ((bool)RadioButton_Celsius.IsChecked)
            {
                _tunits = tempUnits.celsius;
            }

            if ((bool)RadioButton_Mph.IsChecked)
            {
                _wunits = windUnits.mph;
            }
            else if ((bool)RadioButton_Kph.IsChecked)
            {
                _wunits = windUnits.kph;
            }
        }
    }
}
