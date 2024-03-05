using System;
using System.Windows;
using System.Windows.Controls;

namespace FitnessTableApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CalculateBMI_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 讀取輸入值
                string gender = (genderComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                double height = double.Parse(heightTextBox.Text);
                double weight = double.Parse(weightTextBox.Text);
                int age = int.Parse(ageTextBox.Text);
                double activityLevel = GetActivityLevel();

                // 計算BMI
                double heightInMeter = height / 100.0;
                double bmi = weight / (heightInMeter * heightInMeter);

                // 顯示BMI結果
                bmiResultTextBlock.Text = $"您的BMI為: {bmi:F2}";

                // 計算REE
                double ree;
                if (gender == "男性")
                {
                    ree = (10 * weight) + (6.25 * height) - (5 * age) + 5;
                }
                else
                {
                    ree = (10 * weight) + (6.25 * 160) - (5 * age) - 161;
                }

                bmiResultTextBlock.Text += $"\n您的靜態能量消耗值(REE)為: {ree:F0} 卡路里";

                // 根據活動程度計算建議的每日熱量攝取量
                double bmr = ree;
                double dailyCalories = bmr * activityLevel;
                bmiResultTextBlock.Text += $"\n建議每日熱量攝取量: {dailyCalories:F0} 卡路里";
            }
            catch (FormatException)
            {
                MessageBox.Show("請輸入有效的數值。");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"發生錯誤: {ex.Message}");
            }
        }

        private double GetActivityLevel()
        {
            string activityText = (activityComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            switch (activityText)
            {
                case "臥床 (1.2)":
                    return 1.2;
                case "輕活動生活模式 (1.3)":
                    return 1.3;
                case "一般活動度 (1.55)":
                    return 1.55;
                case "一般活動度 (1.725)":
                    return 1.725;
                case "活動量大的生活模式 (2)":
                    return 2;
                default:
                    return 1.55; // 預設為一般活動度
            }
        }

        
    }
}
