﻿using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Input;

namespace CalculatorApp
{
    public partial class MainWindow : Window
    {
        private string expression = "";               // espressione visualizzata e valutata
        private readonly List<string> history = new(); // storico operazioni

        public MainWindow()
        {
            InitializeComponent();
        }

        // Gestisce numeri, operatori, parentesi, radice e potenza
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string content = (string)((Button)sender).Content;

            // Aggiorna l’espressione senza mai cancellare finché non si preme “=”
            expression += content switch
            {
              "√" => "√",     // token radice
                _ => content
            };

            Display.Text = expression;
        }

        // Valuta l’espressione corrente
        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(expression)) return;

            double result;
            bool ok = CalculatorLogic.Evaluate(expression, out result);

            if (ok)
            {
                Display.Text = result.ToString();
                history.Add($"{expression} = {result}");
                HistoryList.ItemsSource = null;
                HistoryList.ItemsSource = history;
                expression = result.ToString(); // consente di continuare a calcolare
            }
            else
            {
                Display.Text = "Errore";
                expression = "";
            }
        }

        // Pulisce display ed espressione
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            expression = "";
            Display.Text = "";
        }

        // Mostra / nasconde lo storico
        private void History_Click(object sender, RoutedEventArgs e)
        {
            HistoryPanel.Visibility = HistoryPanel.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;

            // scorri all’ultima operazione
            if (history.Count > 0 && HistoryPanel.Visibility == Visibility.Visible)
                HistoryList.ScrollIntoView(history[^1]);
        }

        private void DegToRadButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(expression)) return;

            double result;
            bool ok = CalculatorLogic.Evaluate(expression, out result);
            
            if (ok)
            {
                result = CalculatorLogic.DegToRad(result);

                Display.Text = result.ToString();
                history.Add($"DEG({expression}) = RAD({result})");
                HistoryList.ItemsSource = null;
                HistoryList.ItemsSource = history;
                expression = result.ToString(); // consente di continuare a calcolare
            }
            else
            {
                Display.Text = "Errore";
                expression = "";
            }
        }

        private void RadToDegButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(expression)) return;

            double result;
            bool ok = CalculatorLogic.Evaluate(expression, out result);

            if (ok)
            {
                result = CalculatorLogic.RadToDeg(result);

                Display.Text = result.ToString();
                history.Add($"RAD({expression}) = DEG({result})");
                HistoryList.ItemsSource = null;
                HistoryList.ItemsSource = history;
                expression = result.ToString(); // consente di continuare a calcolare
            }
            else
            {
                Display.Text = "Errore";
                expression = "";
            }
        }

        private void TrigonometryButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(expression)) return;

            double result;
            bool ok = CalculatorLogic.Evaluate(expression, out result);

            if (ok)
            {
                string content = (string)((Button)sender).Content;
                
                expression = $"{content}({expression})"; // aggiorna l'espressione con la funzione trigonometrica
                result = CalculatorLogic.TrigonometryCalculate(content, result);

                Display.Text = result.ToString();
                history.Add($"{expression} = {result}");
                HistoryList.ItemsSource = null;
                HistoryList.ItemsSource = history;
                expression = result.ToString(); // consente di continuare a calcolare
            }
            else
            {
                Display.Text = "Errore";
                expression = "";
            }
        }
    }
}