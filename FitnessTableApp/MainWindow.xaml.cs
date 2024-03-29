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
                double height = double.Parse(heightTextBox.Text); // 身高（公分）
                double weight = double.Parse(weightTextBox.Text); // 體重（公斤）
                int age = int.Parse(ageTextBox.Text); // 年齡
                double wrist = double.Parse(wristTextBox.Text); // 手腕圍（公分）
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
                    ree = (10 * weight) + (6.25 * 160) - (5 * 25) - 161;
                }

                reeResultTextBlock.Text = $"\n您的靜態能量消耗值(REE)為: {ree:F0} 卡路里";

                // 計算骨架系數
                double skeletonIndex = CalculateSkeletonIndex(height, wrist);
                string skeletonType = GetSkeletonType(skeletonIndex, gender);
                skeletonIndexTextBlock.Text = $"骨架系數: {skeletonIndex:F2} - {skeletonType}";

                // 計算BMR
                double bmr;
                if (gender == "男性")
                {
                    bmr = 66 + (6.23 * weight * 2.20462) + (12.7 * (height * 0.393701)) - (6.76 * age);
                }
                else
                {
                    bmr = 655 + (4.35 * weight * 2.20462) + (4.7 * (height * 0.393701)) - (4.7 * age);
                }

                // 顯示BMR結果


                // 根據活動程度計算建議的每日熱量攝取量
                double bmrdailyCalories = bmr * activityLevel;
                double bmrcarb;
                double bmrprotein;
                double bmrfat;
                bmrcarb = (ree * 0.6) / 4;
                bmrprotein = (ree * 0.14) / 4;
                bmrfat = (ree * 0.26) / 9;

                bmrcarbResultTextBlock.Text = $"醣類: {bmrcarb:F0} 克/4卡路里";
                bmrproteinResultTextBlock.Text = $"蛋白質: {bmrprotein:F0} 克/4卡路里";
                bmrfatResultTextBlock.Text = $"脂肪: {bmrfat:F0} 克/9卡路里";

                bmrResultTextBlock.Text = $"\n您的基礎代謝率(BMR)為: {bmr:F0} 卡路里";
                // 計算TDEE
                double tdee = bmrdailyCalories;

                // 顯示TDEE結果

                // 判斷增重期或減脂期
                double carb;
                double protein;
                double fat;
                if (weightChangeComboBox.SelectedItem != null)
                {
                    string weightChange = (weightChangeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                    if (weightChange == "增重期")
                    {
                        // 醣類和蛋白質:((根據BMR活動程度建議每日熱量攝取量+750)*40%)/4
                        // 脂肪:((根據BMR活動程度建議每日熱量攝取量+750)*20%)/9
                        carb = ((tdee + 750) * 0.4) / 4;
                        protein = ((tdee + 750) * 0.4) / 4;
                        fat = ((tdee + 750) * 0.2) / 9;
                    }
                    else if (weightChange == "減脂期")
                    {
                        // 醣類:((根據BMR活動程度建議每日熱量攝取量+500)*30%)/4
                        // 蛋白質:((根據BMR活動程度建議每日熱量攝取量+500)*50%)/4
                        // 脂肪:((根據BMR活動程度建議每日熱量攝取量+500)*20%)/9
                        carb = ((tdee + 500) * 0.3) / 4; 
                        protein = ((tdee + 500) * 0.5) / 4;
                        fat = ((tdee + 500) * 0.2) / 9;
                    }
                    else
                    {
                        throw new ArgumentException("請選擇正確的體重變化期。");
                    }
                }
                else
                {
                    throw new ArgumentNullException("請選擇體重變化期。");
                }

                // 顯示結果
                tdeeResultTextBlock.Text = $"每日基礎代謝率 (TDEE)增重或減脂:  {tdee:F0} 卡路里";
                carbResultTextBlock.Text = $"醣類: {carb:F0} 克/4卡路里";
                proteinResultTextBlock.Text = $"蛋白質: {protein:F0} 克/4卡路里";
                fatResultTextBlock.Text = $"脂肪: {fat:F0} 克/9卡路里";
            
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
                case "輕活動生活模式-每周運動1-3日 (1.3)":
                    return 1.3;
                case "一般活動度-每周運動3-5日 (1.55)":
                    return 1.55;
                case "一般活動度-每周運動6-7日 (1.725)":
                    return 1.725;
                case "活動量大的生活模式-每日運動2回 (2)":
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
