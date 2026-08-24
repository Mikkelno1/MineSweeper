using System.Dynamic;

namespace MineSweeper.Models;

public class Cell
{
    
    private bool _isFlag;
    private bool _isBomb;

    public Cell()
    {
    }

    public bool ContainsBomb { get; set; } 
    
    public bool IsRevealed { get; set; }

    public bool ContainsFlag{ get; set; } 
    
    public int AdjacentBomb { get; set; }
    
    
}