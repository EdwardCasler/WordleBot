using Spectre.Console;
public class Bot
{
    public List<string> possibleWords = File.ReadAllLines("words.txt").Select(w => w.Trim().ToLower()).ToList();
    private const char Green = '2';
    private const char Yellow = '1';
    private const char Gray = '0';
    private const char Empty = ' ';
    List<string> storedCandidates;
    public void GuessFromAnswer(string answer)
    {
        List<string> candidates = new List<string>(possibleWords);

        MakeGuess("salet", answer, candidates);

        float attempt = 2;
        while(attempt <= 6 && candidates.Count > 1)
        {
            MakeGuess(GetGuess(candidates), answer, candidates);
            attempt++;
        }
    }
    public void StartGuessingFromUser()
    {
        storedCandidates = new List<string>(possibleWords);
    }
    public string GuessFromUser(string outcome, string guess)
    {
        Filter(guess, outcome, storedCandidates);

        return GetGuess(storedCandidates);
    }
    private void MakeGuess(string guess, string answer, List<string> candidates)
    {
        string outcome = Solve(guess, answer);

        Filter(guess, outcome, candidates);

        Console.WriteLine(guess);
    }
    private string Solve(string guess, string answer)
    {
        char[] letters = answer.ToCharArray();
        char[] outcome = new char[5];
        char[] toCheck = guess.ToCharArray();

        for(var i = 0; i < answer.Length; i++)
        {
            if(toCheck[i] == letters[i])
            {
                outcome[i] = Green;
                letters[i] = Empty;
                toCheck[i] = Empty;
            }
        }

        for(var i = 0; i < toCheck.Length; i++)
        {
            if(toCheck[i] == Empty) continue;

            for(var j = 0; j < letters.Length; j++)
            {
                if(toCheck[i] == letters[j])
                {
                    outcome[i] = Yellow;
                    letters[j] = Empty;
                    toCheck[i] = Empty;
                    break;
                }
            }
        }

        for(var i = 0; i < outcome.Length; i++)
        {
            if(outcome[i] != Yellow && outcome[i] != Green)
            {
                outcome[i] = Gray;
            }
        }

        return new string(outcome);
    }
    private void Filter(string guess, string outcome, List<string> canidates)
    {
        canidates.RemoveAll(word => Solve(guess, word) != outcome);
    }
    private string GetGuess(List<string> candidates)
    {
        int[] table = GetFrequencyTable(candidates);

        float highestScore = -99999;
        string highestWord = "";

        foreach(string word in candidates)
        {
            float score = 0;
            foreach(char letter in word)
            {
                score += table[LetterToIndex(letter)];
            }

            if(score > highestScore)
            {
                highestScore = score;
                highestWord = word;
            }
        }

        return highestWord;
    }
    private int[] GetFrequencyTable(List<string> candidates)
    {
        int[] table = new int[26];
        foreach(string word in candidates)
        {
            foreach(char letter in word)
            {
                table[LetterToIndex(letter)]++;
            }
        }
        return table;
    }
    private int LetterToIndex(char letter)
    {
        return letter - 97;
    }
}