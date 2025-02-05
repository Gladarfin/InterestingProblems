using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using var input = new StreamReader(Console.OpenStandardInput());
using var output = new StreamWriter(Console.OpenStandardOutput());

var t = int.Parse(input.ReadLine());
var finalJson = new List<Dictionary<string, object>>();
var options = new JsonSerializerOptions
{
    WriteIndented = true,
    MaxDepth = 1024
};
for (var ind = 0; ind < t; ind++)
{
    var nm = input.ReadLine().Split(' ').Select(int.Parse).ToList();
    var inputText = new List<string>();
    var isVisited = new bool[nm[0], nm[1]];

    for (var i = 0; i < nm[0]; i++)
    {
        inputText.Add(input.ReadLine());
    }

    var res = CreateJsonFromASCII(inputText, 0, 0, nm, isVisited);
    var d = new Dictionary<string, object>();

    foreach (var elem in res.InnerBoxes)
    {
        if (elem.Name != "")
        {
            d[elem.Name] = elem.ToTreeStructure();
        }
    }
    finalJson.Add(d);
}

output.WriteLine(JsonSerializer.Serialize(finalJson, options));

Box CreateJsonFromASCII(List<string> input, int curRow, int curCol, List<int> rowCol, bool[,] isVisited)
{
    var result = new Box();
    result.InnerBoxes = new List<Box>();
    result.Area = 0;
    var curDashes = 0;
    var curSeparators = 0;
    var rightBorder = rowCol[1];
    var curName = new StringBuilder();
    var isEmpty = true;

    for (var row = curRow; row < rowCol[0]; row++)
    {
        for (var col = curCol; col < rowCol[1]; col++)
        {
            if (col > rightBorder) break;
            if (isVisited[row, col]) continue;

            isVisited[row, col] = true;

            if (input[row][col] == '-')
            {
                curDashes++;
                continue;
            }

            if (input[row][col] == '|')
            {
                curSeparators++;
                continue;
            }

            if (col - curCol <= 3 && char.IsLetterOrDigit(input[row][col]))
            {
                curName.Append(input[row][col]);
                continue;
            }

            if (input[row][col] == '+')
            {
                if (InBounds(row + 1, col - 1, rowCol) && input[row + 1][col] == '|' && input[row][col - 1] == '-')
                {
                    rightBorder = col;
                    continue;
                }

                if (InBounds(row + 1, col + 1, rowCol) && input[row + 1][col] == '|' && input[row][col + 1] == '-')
                {
                    isEmpty = false;
                    result.InnerBoxes.Add(CreateJsonFromASCII(input, row, col, rowCol, isVisited));
                    continue;
                }

                if (InBounds(row - 1, col - 1, rowCol) && input[row - 1][col] == '|' && input[row][col - 1] == '-')
                {
                    if (isEmpty)
                    {
                        result.Area = curDashes / 2 * curSeparators / 2;
                    }
                    result.Name = curName.ToString();
                    return result;
                }
            }
        }
    }
    result.Name = curName.ToString();
    return result;
}

bool InBounds(int row, int col, List<int> rowCol)
{
    return row > 0 && row < rowCol[0] && col > 0 && col < rowCol[1];
}

public class Box
{
    public string Name { get; set; }
    public int Area { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<Box>? InnerBoxes { get; set; }

    public object ToTreeStructure()
    {
        if (InnerBoxes is null || InnerBoxes.Count == 0)
        {
            return Area;
        }

        var result = new Dictionary<string, object>();

        foreach (var innerBox in InnerBoxes)
        {
            result[innerBox.Name] = innerBox.ToTreeStructure();
        }

        return result;
    }
}