//Solution for 'Guess The Word' problem from LeetCode: https://leetcode.com/problems/guess-the-word/

namespace InterestingProblems.App;

class Solution {
    public void FindSecretWord(string[] words, Master master)
    {
        var possibleWords = words.ToList();
        var curWord = "";
        var curCorrectChars = 0;

        for (var i = 0; i < words.Length - 1; i++)
        {
            //We can just pick the first or last word here without sorting, like so:curWord = possibleWords[0] or curWord = possibleWords[^1],
            //but in LeetCode, picking words is slower than sorting. I don't know why.
            if (i % 2 == 0)
            {
                possibleWords.Sort((a, b) => String.Compare(a,b));
            }
            else
            {
                possibleWords.Sort((a, b) => String.Compare(b,a)); 
            }

            curWord = possibleWords[0];
            curCorrectChars = master.Guess(curWord);
            possibleWords.RemoveAt(0);
            if (curCorrectChars == 6)
                return; 

            if (curCorrectChars == 0)
            {
                possibleWords = RemoveImpossibleWords(possibleWords, curWord);
                continue;
            }

            possibleWords = SelectPossibleWords(possibleWords, curWord, curCorrectChars);
        }
    }

    private List<string> SelectPossibleWords(List<string> words, string curWord, int curCorrectChars)
    {
        var result = new List<string>();
        
        foreach (var w in words)
        {
            var tmpCorrect = 0;
            for (var i = 0; i < 6; i++)
            {
                if (w[i] == curWord[i])
                    tmpCorrect++;
            }

            if (tmpCorrect == curCorrectChars)
            {
                result.Add(w);
            }
        }

        return result;
    }

    private List<string> RemoveImpossibleWords(List<string> words, string curWord)
    {
        var result = new List<string>();

        foreach (var w in words)
        {
            var isSame = false;
            for (var i = 0; i < 6; i++)
            {
                if (w[i] == curWord[i])
                {
                    isSame = true;
                    break;
                }
            }
            if (!isSame)
                result.Add(w);
        }

        return result;
    }
}