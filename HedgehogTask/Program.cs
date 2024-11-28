var counts = new int[3]; // counts[0]: Red, counts[1]: Green, counts[2]: Blue

Console.WriteLine("Enter the counts of Red, Green, and Blue hedgehogs (space-separated):");
var inputCounts = Console.ReadLine().Split();
counts[0] = int.Parse(inputCounts[0]);
counts[1] = int.Parse(inputCounts[1]);
counts[2] = int.Parse(inputCounts[2]);

Console.WriteLine("Enter the desired color (0 - Red, 1 - Green, 2 - Blue):");
var desiredColor = int.Parse(Console.ReadLine());

var result = MinimumMeetings(counts, desiredColor);
Console.WriteLine("Minimum number of meetings: " + result);

static int MinimumMeetings(int[] counts, int desiredColor)
{
    int totalHedgehogs = counts[0] + counts[1] + counts[2];

    // If all hedgehogs are already the desired color
    if (counts[desiredColor] == totalHedgehogs)
    {
        return -1;   
    }

    var currentCounts = new int[3];
    counts.CopyTo(currentCounts, 0);

    var color1 = (desiredColor + 1) % 3;
    var color2 = (desiredColor + 2) % 3;

    var meetings = 0;

    // Check the invariant: the difference modulo 3 must be zero
    if ((currentCounts[color1] - currentCounts[color2]) % 3 != 0)
    {
        return -1;
    }

    // If one non-desired color still has hedgehogs
    var remainingColor = currentCounts[color1] > 0 ? color1 : color2;
    var otherColor = (remainingColor == color1) ? color2 : color1;

    while (currentCounts[remainingColor] >= 3)
    {
        // Perform meetings to create pairs of the other non-desired color
        currentCounts[remainingColor] -= 3;
        currentCounts[otherColor] += 3;
        meetings += 3;

        // Now convert them to the desired color
        var meetCount = Math.Min(currentCounts[remainingColor], currentCounts[otherColor]);

        currentCounts[desiredColor] += 2 * meetCount;
        currentCounts[remainingColor] -= meetCount;
        currentCounts[otherColor] -= meetCount;

        meetings += meetCount;
    }

    // If any non-desired hedgehogs remain, we need to perform additional steps
    if (currentCounts[remainingColor] > 0)
    {
        // Introduce hedgehogs of the other non-desired color by sacrificing hedgehogs of the desired color
        while (currentCounts[remainingColor] > 0)
        {
            // Desired + Remaining Non-Desired → Other Non-Desired + Other Non-Desired
            currentCounts[desiredColor] -= 1;
            currentCounts[remainingColor] -= 1;
            currentCounts[otherColor] += 2;
            meetings += 1;

            // Now convert other non-desired hedgehogs to desired color
            var meetCount = Math.Min(currentCounts[remainingColor], currentCounts[otherColor]);
            currentCounts[desiredColor] += 2 * meetCount;
            currentCounts[remainingColor] -= meetCount;
            currentCounts[otherColor] -= meetCount;
            meetings += meetCount;
        }
    }

    return meetings;
}