using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using MineSweeper.Models;
using MineSweeper.ViewModels;
using Avalonia.Controls.Shapes;


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
        
        Console.WriteLine($"Rows: {GameGrid.RowDefinitions.Count}");
        Console.WriteLine($"Columns: {GameGrid.ColumnDefinitions.Count}");
        Console.WriteLine($"Buttons: {GameGrid.Children.Count}");
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Button button = sender as Button;

        int row = Grid.GetRow(button);
        int column = Grid.GetColumn(button);

        if (bombCliked(row, column)) {EndGame();}

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

    private bool bombCliked(int row, int column)
    {
        if (DataContext is GameViewModel viewModel)
        {
            Cell cell = viewModel.Gameboard[row, column];
            return cell.ContainsBomb;
        }
        return false;
    }

    
    
    private void EndGame()
    {
        Rectangle endGame = new Rectangle
        {
            Width = 100,
            Height = 100,
        };
        
        Canvas.SetLeft(endGame, 0);
        Canvas.SetTop(endGame, 0);
        
        GameOverCanvas.Children.Add(endGame);
    }
}