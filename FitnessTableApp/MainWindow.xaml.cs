﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
                double wrist = double.Parse(wristTextBox.Text);
                double activityLevel = GetActivityLevel();

                // 計算BMI
                double heightInMeter = height / 100.0;
                double bmi = weight / (heightInMeter * heightInMeter);

                // 顯示BMI結果
                bmiResultTextBlock.Text = $"BMI: {bmi.ToString("0.00")}";

                // 根據BMI值設置顏色
                if (bmi >= 20 && bmi <= 24) // 正常範圍
                {
                    bmiResultTextBlock.Text = $"BMI:{bmi.ToString("0.0")}---正常";
                    bmiResultTextBlock.Foreground = Brushes.Green;
                }
                else if (bmi > 26) // 肥胖範圍
                {
                    bmiResultTextBlock.Text = $"BMI:{bmi.ToString("0.0")}---肥胖";
                    bmiResultTextBlock.Foreground = Brushes.Red;
                }
                else if (bmi < 20) // 太瘦範圍
                {
                    bmiResultTextBlock.Text = $"BMI:{bmi.ToString("0.0")}---太瘦";
                    bmiResultTextBlock.Foreground = Brushes.Red;
                }

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

                reeResultTextBlock.Text = $"\n您的靜態能量消耗值(REE)為: {ree:F0} 卡路里";

                // 計算骨架系數
                double skeletonIndex = CalculateSkeletonIndex(height, wrist);
                string skeletonType = GetSkeletonType(skeletonIndex, gender);
                skeletonIndexTextBlock.Text = $"骨架系數: {skeletonIndex:F2} - {skeletonType}";

                // 根據活動程度計算建議的每日熱量攝取量
                double bmr = ree;
                double dailyCalories = bmr * activityLevel;
                reeResultTextBlock.Text += $"\n根據活動程度建議每日熱量攝取量: {dailyCalories:F0} 卡路里";
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

        private double CalculateSkeletonIndex(double height, double wrist)
        {
            return height / wrist;
        }

        private string GetSkeletonType(double skeletonIndex, string gender)
        {
            if (gender == "男性")
            {
                if (skeletonIndex < 9.6)
                {
                    return "大骨架";
                }
                else if (skeletonIndex > 10.4)
                {
                    return "小骨架";
                }
                else
                {
                    return "中骨架";
                }
            }
            else // 女性
            {
                if (skeletonIndex < 9.9)
                {
                    return "大骨架";
                }
                else if (skeletonIndex > 10.9)
                {
                    return "小骨架";
                }
                else
                {
                    return "中骨架";
                }
            }
        }
    }
}
