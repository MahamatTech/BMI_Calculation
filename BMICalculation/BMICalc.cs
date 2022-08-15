using System;

namespace BMICalculation
{
    public enum SysOfUnits
    {
        Metric, Imperial
    }

    public static class BMICalc
    {
        public static double BmiValue(double height, double weight, SysOfUnits u, out string category)
        {
            if (height < 0 || weight < 0)
            {
                throw new ArgumentOutOfRangeException("Neither height nor weight can be negative.");
            }
            else
            {
                double _bmi = weight / Math.Pow(height, 2);
                if (u == SysOfUnits.Imperial)
                    _bmi *= 703;
                //Set the category
                if (_bmi < 15)
                {
                    category = "Very severely underweight";
                }
                else if (_bmi <= 16)
                {
                    category = "Severely underweight";
                }
                else if (_bmi <= 18.5)
                {
                    category = "Underweight";
                }
                else if (_bmi <= 25)
                {
                    category = "Normal";
                }
                else if (_bmi <= 30)
                {
                    category = "Overweight";
                }
                else if (_bmi <= 35)
                {
                    category = "Obese Class I (Moderately obese)";
                }
                else if (_bmi <= 40)
                {
                    category = "Obese Class II (Severely obese)";
                }
                else
                {
                    category = "Obese Class III (Very Severely obese)";
                }
                return Math.Round(_bmi, 2);
            }
        }
    }
}
