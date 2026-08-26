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
    private Button[,] _buttons = new Button[Rows, Columns];
    
    public GameView()
    {
        InitializeComponent();
        CreateButtons();

        
    }

    private void CreateButtons()
    {
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

                _buttons[row, column] = button;
                        
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

        

    if (DataContext is GameViewModel gameViewModel)
        {
            gameViewModel.FloodReveal(row, column);
            RemoveButtons(gameViewModel);
        }
    }

    private void RemoveButtons(GameViewModel gameViewModel)
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                Cell cell = gameViewModel.Gameboard[row, column];
                Button button = _buttons[row, column];

                button.IsVisible = !cell.IsRevealed;
            }
            
        }
    }
}