using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
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
                Button button = new Button
                {
                    Width = 26,
                    Height = 26
                };

                button.Click += Button_OnClick;
                button.PointerPressed += Button_OnPointerPressed;

                Grid.SetRow(button, row);
                Grid.SetColumn(button, column);

                _buttons[row, column] = button;
                GameGrid.Children.Add(button);
            }
        }
    }


    private void Button_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is not Button button)
            return;

        if (e.GetCurrentPoint(button).Properties.PointerUpdateKind
            == PointerUpdateKind.RightButtonPressed)
            button.Content = "🚩";
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Button button = sender as Button;

        int row = Grid.GetRow(button);
        int column = Grid.GetColumn(button);

        if (BombCliked(row, column))
        {
            EndGame();
            return;
        }

        if (DataContext is GameViewModel gameViewModel)
        {
            gameViewModel.FloodReveal(row, column);
            RemoveButtons(gameViewModel);
        }

        UpdateBoard();
    }

    private void UpdateBoard()
    {
        foreach (var child in GameGrid.Children.ToList())
        {
            if (child is Label)
            {
                GameGrid.Children.Remove(child);
            }
        }

        if (DataContext is GameViewModel viewModel)
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    Cell cell = viewModel.Gameboard[row, column];
                    if (cell.IsRevealed)
                    {
                        if (cell.AdjacentBomb > 0)
                        {
                            Label adjecentBombLabel = new Label
                            {
                                Content = cell.AdjacentBomb,
                                FontSize = 8
                            };
                            Grid.SetColumn(adjecentBombLabel, column);
                            Grid.SetRow(adjecentBombLabel, row);

                            GameGrid.Children.Add(adjecentBombLabel);
                        }
                    }
                }
            }
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

    private bool BombCliked(int row, int column)
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
            Width = GameGrid.Bounds.Width,
            Height = GameGrid.Bounds.Height,
            Fill = Brushes.DarkGray,
            Opacity = 0.7,
        };
        CreateGameOverMenu();
        GameOverCanvas.Children.Add(endGame);
    }

    private void CreateGameOverMenu()
    {
        Label gameOverLabel = new Label
        {
            Content = "Game Over, you Lost!!!",
            FontSize = 14,
        };
        MainGrid.Children.Add(gameOverLabel);
    }
}