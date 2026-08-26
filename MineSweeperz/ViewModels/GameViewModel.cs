using System;
using System.Collections.Generic;
using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MineSweeper.Models;
using MineSweeperz.ViewModels;

namespace MineSweeper.ViewModels;

public class GameViewModel : ViewModelBase
{
    private const int Rows = 20;
    private const int Columns = 20;
    private Cell[,] _gameBoard = new Cell[Rows, Columns];
    private int _adjecentBombs;
    public Cell[,] Gameboard => _gameBoard;
    private Grid _gameGrid;
    public int bombCount { get; private set; }
    


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
        bombCount = (Rows * Columns) / 10;

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

            //Console.WriteLine(_gameBoard[row, column].ContainsBomb = true);
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

    public void FloodReveal(int startRow, int startColumn)
    {
        Queue<(int row, int column)> queue = new();

        queue.Enqueue((startRow, startColumn));

        while (queue.Count > 0)
        {
            var (row, column) = queue.Dequeue();

            //Checking if boundaries have been reached
            if (row < 0 || row >= Rows || column < 0 || column >= Columns)
            {
                continue;
            }

            Cell cell = _gameBoard[row, column];
            
            

            //Continues if already revealed or if the cell is a bomb
            if (cell.IsRevealed || cell.ContainsBomb)
            {
                continue;
            }
            
            cell.IsRevealed = true;
            
            //Stops spreading if adjacent to a bomb
            if (cell.AdjacentBomb > 0)
            {
                //Console.WriteLine(cell.AdjacentBomb);
                //AdjacentBombs = cell.AdjacentBomb;
                continue;
            }

            //Adding neighborus to queue
            for (int rowOffset = -1; rowOffset <= 1; rowOffset++)
            {
                for (int columnOffset = -1; columnOffset <= 1; columnOffset++)
                {
                    if (rowOffset == 0 && columnOffset == 0)
                    {
                        continue;
                    }

                    queue.Enqueue((
                        row + rowOffset, 
                        column + columnOffset
                    ));
                }
            }
        }
    }
    
    
}