using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Runtime.CompilerServices;

namespace MineSweeper.Models;

/**
 * Cell class which populates the gamegrid. Each cell will contain a button and a boolean value
 */
public class Cell : INotifyPropertyChanged
{
    
    private bool _isFlag;
    private bool _isBomb;
    private bool _isRevealed;

    public Cell()
    {
    }

    public bool ContainsBomb { get; set; }

    public bool IsRevealed
    {
        get => _isRevealed;

        set
        {
            _isRevealed = value;

            OnPropertyChanged();

        }
    }

    public bool ContainsFlag{ get; set; } 
    
    public int AdjacentBomb { get; set; }


    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    
}