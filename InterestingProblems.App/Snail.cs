//Solution for 'Snail' 4 kyu kata from Codewars: https://www.codewars.com/kata/521c2db8ddc89b9b7a0000c1
//pretty similar to the 'Spiral Matrix' problem from LeetCode: https://leetcode.com/problems/spiral-matrix/description/?tab=Description

namespace InterestingProblems.App;

using System.Collections.Generic;

public class SnailSolution
{
    public static int[] Snail(int[][] array)
    {
        int m = array.Length, n = array[0].Length;
        
        var directions = new List<(int, int)>(){ (0, 1), (1, 0), (0, -1), (-1, 0) };
        int directionIndex = 0;
        
        var seen = new HashSet<(int, int)>();
        int i = 0, j = 0;
        var res = new List<int>();
        
        for (int times = 0; times < m * n; times++)
        {
            res.Add(array[i][j]);
            seen.Add((i, j));
            
            var (di, dj) = directions[directionIndex];
            int ni = i + di, nj = j + dj;
            if (ni < 0 || ni >= m || nj < 0 || nj >= n || seen.Contains((ni, nj)))
            {
                directionIndex = (directionIndex + 1) % 4;
                (di, dj) = directions[directionIndex];
                ni = i + di;
                nj = j + dj;
            }
            i = ni;
            j = nj;
        }
        
        return res.ToArray();
    }
}