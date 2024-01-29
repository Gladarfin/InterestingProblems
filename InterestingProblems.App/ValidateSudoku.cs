//Solution for "Validate Sudoku with size `NxN`" 4kyu kata from Codewars: https://www.codewars.com/kata/540afbe2dc9f615d5e000425

namespace InterestingProblems.App;

using System;
using System.Collections.Generic;
using System.Linq;

class Sudoku
{
    private int[][] NewSudoku { get; }

    public Sudoku(int[][] sudokuData)
    {
        NewSudoku = sudokuData;
    }

    public bool IsValid()
    {
        var cols = NewSudoku.Length;
        var rows = NewSudoku[0].Length;
        var cellSize = (int)Math.Sqrt(cols);

        if (rows != cols)
            return false;

        for (var i = 0; i < cols; i++)
        {
            if (NewSudoku[i].Length != cols)
                return false;
        }

        var correctRows = CheckByRowsAndColumns(cols, false);
        var correctColumns = CheckByRowsAndColumns(cols, true);
        if (!correctRows || !correctColumns)
            return false;

        var cellList = new List<List<int>>();

        for (var i = 0; i < rows; i++)
        {
            cellList.Add(Enumerable.Range(1, rows).ToList());
        }

        return CheckBySquares(rows, cols, cellSize, cellList);
    }

    private bool CheckByRowsAndColumns(int cols, bool isColumn)
    {
        for (var i = 0; i < cols; i++)
        {
            var cur = Enumerable.Range(1, cols).ToList();
            for (var j = 0; j < cols; j++)
            {
                switch (isColumn)
                {
                    case true when !cur.Contains(NewSudoku[j][i]):
                        return false;
                    case true:
                        cur.Remove(NewSudoku[j][i]);
                        break;
                    case false when !cur.Contains(NewSudoku[i][j]):
                        return false;
                    case false:
                        cur.Remove(NewSudoku[i][j]);
                        break;
                }
            }
        }

        return true;
    }

    private bool CheckBySquares(int rows, int cols, int cellSize, List<List<int>> cellList)
    {
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                var majorRow = i / cellSize;
                var majorCol = j / cellSize;
                var some = majorCol + majorRow * cellSize;
                if (!cellList[some].Contains(NewSudoku[i][j]))
                {
                    return false;
                }

                cellList[some].Remove(NewSudoku[i][j]);
            }
        }

        return true;
    }
}