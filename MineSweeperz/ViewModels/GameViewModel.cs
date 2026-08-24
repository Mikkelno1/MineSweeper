using System;
using System.Collections.Generic;
using Avalonia.Controls;
using MineSweeper.Models;
using MineSweeperz.ViewModels;

namespace MineSweeper.ViewModels;

public class GameViewModel : ViewModelBase
{
    private const int Rows = 15;
    private const int Columns = 15;
    private Cell[,] _gameBoard = new Cell[Rows, Columns];
    private Grid _gameGrid;


    public GameViewModel()
    {
        GenerateGrid();
        GenerateBomb();
        CalculateAdjacentBombs();
    }

    private void GenerateGrid()
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                _gameBoard[row, column] = new Cell();
            }
        }
    }

    private void GenerateBomb()
    {
        int bombCount = (Rows * Columns) / 10;

        Random random = new();
        var placed = 0;

        while (placed < bombCount)
        {
            var row = random.Next(Rows);
            var column = random.Next(Columns);

            if (_gameBoard[row, column].ContainsBomb)
            {
                continue;
            }

            _gameBoard[row, column].ContainsBomb = true;
            placed++;

            Console.WriteLine(_gameBoard[row, column].ContainsBomb = true);
        }
    }

    private void FloodReveal(int row, int column)
    {
        //Checks for boundaries
        if (row < 0 || row >= Rows || column < 0 || column >= Columns)
        {
            return;
        }

        Cell cell = _gameBoard[row, column];
        
        cell.IsRevealed = true;

        //Checks if cell already is revealed or is a bomb, breaks out if true
        if (cell.IsRevealed || cell.ContainsBomb)
        {
            return;
        }
        
        //Breaks out of the recursion if number of adjacent bombs is larger than 0
        if (cell.AdjacentBomb > 0)
        {
            return;
        }
        
        //Checks immediate rows and columns in a -1, 0, +1 pattern, continues when reaching the middle (0,0) otherwise
        //
        for (int rowOffset = -1; rowOffset <= 1; rowOffset++)
        {
            for (int columnOffset = -1; columnOffset <= 1; columnOffset++)
            {
                if (rowOffset == 0 && columnOffset == 0)
                    continue;

                FloodReveal(
                    row,
                    column 
                );
            }
        }
    }
    
    private void CalculateAdjacentBombs()
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                int bombCount = 0;

                for (int rowOffset = -1; rowOffset <= 1; rowOffset++)
                {
                    for (int columnOffset = -1; columnOffset <= 1; columnOffset++)
                    {
                        if (rowOffset == 0 && columnOffset == 0)
                            continue;

                        int neighborRow = row + rowOffset;
                        int neighborColumn = column + columnOffset;

                        if (neighborRow < 0 || neighborRow >= Rows ||
                            neighborColumn < 0 || neighborColumn >= Columns)
                        {
                            continue;
                        } 

                        if (_gameBoard[neighborRow, neighborColumn].ContainsBomb)
                        {
                            bombCount++;
                        }
                    }
                }

                _gameBoard[row, column].AdjacentBomb = bombCount;
            }
        }
    }
    
    
    private void FloodReveals(int startRow, int startColumn)
    {
        Queue<(int row, int column)> queue = new();

        queue.Enqueue((startRow, startColumn));

        while (queue.Count > 0)
        {
            var (row, column) = queue.Dequeue();

            // Check boundaries
            if (row < 0 || row >= Rows || column < 0 || column >= Columns)
            {
                continue;
            }

            Cell cell = _gameBoard[row, column];
            
            cell.IsRevealed = true;

            // Already revealed or bomb
            if (cell.IsRevealed || cell.ContainsBomb)
            {
                continue;
            }

            // Stop spreading if next to a bomb
            if (cell.AdjacentBomb > 0)
            {
                continue;
            }

            // Add all 8 neighbors to the queue
            for (int rowOffset = -1; rowOffset <= 1; rowOffset++)
            {
                for (int columnOffset = -1; columnOffset <= 1; columnOffset++)
                {
                    if (rowOffset == 0 && columnOffset == 0)
                    {
                        continue;
                    }

                    queue.Enqueue((
                        row ,
                        column
                    ));
                }
            }
        }
    }
}