using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Input;
using System.Windows.Controls;
using System.Text.RegularExpressions;

namespace WpfApp1
{
   public partial class MainWindow : Window
   {
        private TextBox[,] textBoxs;
        private int size;
        public MainWindow()
        {
            InitializeComponent();          
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Files (*.txt)(*.csv)(*.xlsx)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                filePath.Text = openFileDialog.FileName;
                size = File.ReadAllLines(openFileDialog.FileName).Length;
                if (size > 10)
                {
                    MessageBox.Show("Wrong data in file");
                    return;
                }
                CreateGrid(size);
                ChangeControlsState();
                TextBox1.Text = size.ToString();
                using (StreamReader file = new StreamReader(openFileDialog.FileName))
                {
                    string line;
                    string[] linesSplitted;
                    int row = 0;
                    while((line = file.ReadLine()) != null)
                    {
                        int column = 0;
                        linesSplitted = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string lineSplitted in linesSplitted)
                        {
                            textBoxs[row, column].Text = lineSplitted;
                            column++;
                        }
                        row++;
                    }
                }
            }
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            Regex rg = new Regex("[0-9]");
            if (!rg.IsMatch(e.Key.ToString()))
            {
                e.Handled = true;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (TextBox1.Text == String.Empty)
            {
                MessageBox.Show("The value should be in range 1-10");
                return;
            }  
            int number = int.Parse(TextBox1.Text);
            if (number < 0 || number > 10)
            {
                TextBox1.Text = String.Empty;
                MessageBox.Show("The value should be in range 1-10");
                return;
            }
            size = number;
            ChangeControlsState();
            CreateGrid(number);
        }

        private void ChangeControlsState()
        {
            FillInArrayRandomNumbers.Visibility = Visibility.Visible;
            rowLabel.Visibility = Visibility.Visible;
            rowTextBox.Visibility = Visibility.Visible;
            columnLabel.Visibility = Visibility.Visible;
            columnTextBox.Visibility = Visibility.Visible;
            calculate.Visibility = Visibility.Visible;

            columnLabel.Content = $"Columns (1-{size})";
            rowLabel.Content = $"Rows (1-{size})";
            TextBox1.IsEnabled = false;
            generate.IsEnabled = false;
        }
        private string OrdinalNumber(int number)
        {
            switch (number)
            {
                case 1:
                    return $"{number.ToString()}st";
                case 2:
                    return $"{number.ToString()}nd";
                case 3:
                    return $"{number.ToString()}rd";
                case int n when number > 3:
                    return $"{number.ToString()}th";
                default:
                    return "";
            }
        }
        private void CreateGrid(int size)
        {
            Grid grid = new Grid
            {
                Width = 350,
                Height = 350,
                ShowGridLines = true
            };
            ColumnDefinition[] columns = new ColumnDefinition[size];
            RowDefinition[] rows = new RowDefinition[size];
            textBoxs = new TextBox[size, size];

            for(int i = 0; i < size; i++)
            {
                columns[i] = new ColumnDefinition();
                grid.ColumnDefinitions.Add(columns[i]);
            }

            for (int i = 0; i < size; i++)
            {
                rows[i] = new RowDefinition();
                grid.RowDefinitions.Add(rows[i]);
            }
            for(int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                {
                    textBoxs[i, j] = new TextBox();
                    Grid.SetRow(textBoxs[i, j], i);
                    Grid.SetColumn(textBoxs[i, j], j);
                    grid.Children.Add(textBoxs[i, j]);
                    textBoxs[i, j].FontSize = 14;
                    textBoxs[i, j].FontWeight = FontWeights.Bold;
                }
            }
            MainFrame.Content = grid;
        }

        private void FillInArrayRandomNumbers_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            for(int i=0; i<size; i++)
            {
                for(int j = 0; j<size; j++)
                {
                    textBoxs[i, j].Text = rnd.Next(0, 20).ToString();
                }
            }
        }

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            int[,] numbers = new int[size, size];
            for(int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                {
                    numbers[i, j] = int.Parse(textBoxs[i, j].Text);
                }
            }

            Array array = new Array(numbers, size);
            MessageBox.Show("Data has been saved to file array.txt");
            array.SaveToFile();
            try
            {
                int column = int.Parse(columnTextBox.Text);
                int row = int.Parse(rowTextBox.Text);
                if (row > size || column > size)
                    throw(new System.FormatException());
                result.Content = $"Sum {OrdinalNumber(row)} row equals {array.SumIndicatedRow(row)}\n";
                result.Content += $"Sum {OrdinalNumber(column)} column equals {array.SumIndicatedColumn(column)}\n";
                result.Content += $"Sum main diagonal equals {array.SumMainDiagonal()}";
            }
            catch(System.FormatException ex)
            {
                MessageBox.Show($"Wrong numbers in textbox Column or Row\nNumber should be in the range(1-{size})");
                rowTextBox.Text = String.Empty;
                columnTextBox.Text = String.Empty;
            }
        }

        private void TextBox_KeyDown_1(object sender, KeyEventArgs e)
        {
            Regex rg = new Regex("[0-9]");
            if (!rg.IsMatch(e.Key.ToString()))
            {
                e.Handled = true;
            }
        }

        private void TextBox_KeyDown_2(object sender, KeyEventArgs e)
        {
            Regex rg = new Regex("[0-9]");
            if (!rg.IsMatch(e.Key.ToString()))
            {
                e.Handled = true;
            }
        }
    }
}