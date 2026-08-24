using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MineSweeper.Models;
using MineSweeper.ViewModels;

namespace MineSweeper.Views;

public partial class GameView : UserControl
{
    private const int Rows = 20;
    private const int Columns = 20;
    
    public GameView()
    {
        InitializeComponent();

        for (int row = 0; row < Rows; row++)
        {
            GameGrid.RowDefinitions.Add(new RowDefinition());
            GameGrid.ColumnDefinitions.Add(new ColumnDefinition());
            
            for (int column = 0; column < Columns; column++)
            {
                Button button = new Button();
                button.Click += Button_OnClick;
                Grid.SetRow(button, row);
                Grid.SetColumn(button, column);
                
                GameGrid.Children.Add(button);
            }
        }
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Button button = sender as Button;
        Console.WriteLine("Clicked");

        int row = Grid.GetRow(button);
        int column = Grid.GetColumn(button);

        

    if (DataContext is GameViewModel viewModel)
        {
            viewModel.FloodReveal(row, column);

            Cell cell = viewModel.Gameboard[row, column];
            if (cell.IsRevealed) 
            {
                button.IsVisible = false;
            }
        }
    }
}