/***************************************************************************\
The MIT License (MIT)

Copyright (c) 2014 Jonas Schiegl (https://github.com/senritsu)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
\***************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Plugins.SQLite;

namespace Assets.Unitility.SQLite
{
    public class ExampleScoreModel
    {
        public string PlayerName { get; set; }
        public int Level { get; set; }
        public int Score { get; set; }

        public override string ToString()
        {
            return string.Format("<Score for Player '{0}' in Level '{1}': {2}>", PlayerName, Level, Score);
        }
    }

    public class ExampleSQLiteAccessor : BaseDbAccessor
    {
        public ExampleSQLiteAccessor()
        {
            SQL("CREATE TABLE IF NOT EXISTS " +
                "Scores(name TEXT, level INTEGER, score INTEGER)");
        }

        protected override string ConnectionString
        {
            get { return string.Format("URI=file:{0}", "Assets/example.sqlite"); }
        }

        public void AddScore(string playerName, int level, int score)
        {
            SQL(string.Format("INSERT INTO Scores VALUES ('{0}',{1},{2})", playerName, level, score));
        }

        public IEnumerable<ExampleScoreModel> GetHighscores(String playerName)
        {
            var result = SQL(string.Format("SELECT Scores.name, Scores.level, Scores.score " +
                                           "FROM Scores " +
                                           "WHERE Scores.name = '{0}'"
                , playerName));

            var scores = new List<ExampleScoreModel>();
            // every call to .Read() advances to the next row of the result set
            while (result.Read())
            {
                scores.Add(new ExampleScoreModel
                {
                    PlayerName = result.GetString(0),
                    Level = result.GetInt32(1),
                    Score = result.GetInt32(2)
                });
            }
            return scores
                .GroupBy(score => score.Level)
                .Select(group => group.OrderByDescending(score => score.Score).First());
        }

        public ExampleScoreModel GetHighscoreForLevel(String playerName, int level)
        {
            var result = SQL(string.Format("SELECT Scores.name, Scores.score " +
                                           "FROM Scores " +
                                           "WHERE Scores.name = '{0}' AND Scores.level = {1} " +
                                           "ORDER BY Scores.score DESC"
                , playerName, level));
            // .Read() will return true if there is at least one row, and advance the cursor to it
            // there could be more than one row, but we only care about the first here (highscore)
            if (result.Read())
            {
                return new ExampleScoreModel
                {
                    PlayerName = result.GetString(0),
                    Level = level,
                    Score = result.GetInt32(1)
                };
            }
            return null;
        }
    }
}