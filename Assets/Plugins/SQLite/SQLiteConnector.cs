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

/***************************************************************************\
 based on http://wiki.unity3d.com/index.php/SQLite
\***************************************************************************/

using System.Data;
using Mono.Data.SqliteClient;

// Required: 
// Mono.Data.SqliteClient.dll
// sqlite3.def
// sqlite3.dll
// System.Data.dll

namespace Assets.Plugins.SQLite
{
    public static class SQLiteConnector
    {
        public static IDataReader ExecuteSQL(string db, string sql)
        {
            var connection = new SqliteConnection(db);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = sql;
            var result = command.ExecuteReader();
            connection.Close();
            return result;
        }
    }

    public abstract class BaseDbAccessor
    {
        protected abstract string ConnectionString { get; }

        public IDataReader SQL(string sql)
        {
            return SQLiteConnector.ExecuteSQL(ConnectionString, sql);
        }
    }
}