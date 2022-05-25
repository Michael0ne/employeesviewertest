using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MySql.Data.MySqlClient;

namespace EmployeesViewer
{
    /// <summary>
    /// Класс для работы с БД.
    /// После создания необходимо вызвать метод @Instantiate, передав необходимые для подключения параметры.
    /// При уничтожении, активное подключение будет закрыто автоматически.
    /// </summary>
    internal class DBManager
    {
        static private MySqlConnection  Connection;
        static private string           ServerAddress;
        static private UInt16           ServerPort;
        static private string           ServerDatabase;
        static private string           ServerUserName;
        static private string           ServerPassword;
        static private UInt32           ConnectionsThisSession = 0;

        static public bool              ConnectionEstabilished { get; private set; }

        static readonly UInt16 MySqlDefaultPort = 3306;

        /// <summary>
        /// Тип параметра, который передаётся в вызываемую процедуру.
        /// </summary>
        public enum eRequestParameterType
        {
            COLUMN_NAME,
            ARGUMENT_INPUT,
            ARGUMENT_OUTPUT,
            CONDITION
        };

        ~DBManager()
        {
            if (Connection != null)
                Connection.Dispose();
        }

        /// <summary>
        /// Класс для создания запроса к БД.
        /// Выполняет вызов STORED PROCEDURE с указанными параметрами (если есть) и возвращает значение (если есть).
        /// Если возвращаемое значение не указано, то возвращает коллекцию строк (если есть) и список столбцов (если есть).
        /// </summary>
        internal class Request
        {
            private struct RequestParameterKey
            {
                public eRequestParameterType   ParameterType;
                public string                  ParameterName;

                public RequestParameterKey(eRequestParameterType paramType, string paramName)
                {
                    ParameterType = paramType;
                    ParameterName = paramName;
                }
            }
            private struct RequestParameterValue
            {
                public MySqlDbType  ParameterType;
                public object       ParameterValue;

                public RequestParameterValue(MySqlDbType paramType, object paramValue)
                {
                    ParameterType = paramType;
                    ParameterValue = paramValue;
                }
            }

            private string FunctionName { get; set; }
            private Dictionary<RequestParameterKey, RequestParameterValue> RequestParameters { get; set; }
            private MySqlCommand RequestCommand;
            private List<string> RequestResultsColumns;
            private Dictionary<int, List<object>> RequestResultsRows;
            private string LastError;

            /// <summary>
            /// Возвращает экземпляр класса запроса готовый к выполнению.
            /// </summary>
            /// <param name="functionName">Название вызываемой процедуры.</param>
            public Request(string functionName)
            {
                RequestParameters = new Dictionary<RequestParameterKey, RequestParameterValue>();
                FunctionName = functionName;
            }

            ~Request()
            {
                if (RequestCommand != null)
                    RequestCommand.Dispose();
            }

            /// <summary>
            /// Добавить параметр к текущему запросу.
            /// </summary>
            /// <param name="parameterName">Имя параметра. Если данный параметр является возвращаемым значением, то это имя будет проигнорировано.</param>
            /// <param name="parameterType">Тип параметра - входной или возвращаемый аргумент процедуры.</param>
            /// <param name="parameterValue">Значение параметра.</param>
            /// <param name="parameterValueType">SQL тип параметра.</param>
            public void AddParameter(string parameterName, eRequestParameterType parameterType, object parameterValue, MySqlDbType parameterValueType)
            {
                RequestParameters.Add(new RequestParameterKey(parameterType, parameterName), new RequestParameterValue(parameterValueType, parameterValue));
            }

            /// <summary>
            /// Получить коллекцию строк, полученных в результате последнего запроса.
            /// </summary>
            /// <returns>Коллекция строк, где ключ - индекс строки, а значение - список значений колонок. Если последний запрос не вернул никаких строк, то возвращается null.</returns>
            public Dictionary<int, List<object>> GetResultsRows()
            {
                return RequestResultsRows ?? null;
            }

            /// <summary>
            /// Получить список названий колонок.
            /// </summary>
            /// <returns>Список названий колонок. Если последний запрос не вернул никаких колонок, то возвращается null.</returns>
            public List<string> GetResultsColumns()
            {
                return RequestResultsColumns ?? null;
            }

            /// <summary>
            /// В случае, если при выполнении запроса произошла ошибка, данный метод её вернёт.
            /// </summary>
            /// <returns>Строка с описанием ошибки.</returns>
            public string GetLastError()
            {
                return LastError;
            }

            private bool PrepareRequest()
            {
                RequestCommand = new MySqlCommand();
                RequestCommand.Connection = Connection;
                RequestCommand.CommandType = System.Data.CommandType.StoredProcedure;
                RequestCommand.CommandText = FunctionName;

                foreach (KeyValuePair<RequestParameterKey, RequestParameterValue> parameter in RequestParameters)
                {
                    if (parameter.Key.ParameterType != eRequestParameterType.ARGUMENT_INPUT &&
                        parameter.Key.ParameterType != eRequestParameterType.ARGUMENT_OUTPUT)
                        continue;

                    MySqlParameter sqlParameter = new MySqlParameter();

                    if (parameter.Key.ParameterType == eRequestParameterType.ARGUMENT_INPUT)
                    {
                        if (String.IsNullOrEmpty(parameter.Value.ParameterValue.ToString()))
                            return false;

                        sqlParameter.ParameterName = "@" + parameter.Key.ParameterName;
                        sqlParameter.Direction = System.Data.ParameterDirection.Input;
                        RequestCommand.Parameters.AddWithValue(sqlParameter.ParameterName, parameter.Value.ParameterValue);
                    }
                    else
                    {
                        sqlParameter.ParameterName = "@Result";
                        sqlParameter.Direction = System.Data.ParameterDirection.Output;
                        RequestCommand.Parameters.Add(sqlParameter);
                    }
                }

                return true;
            }

            private bool ExecuteRequest()
            {
                using (MySqlDataReader reader = RequestCommand.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return false;

                    RequestResultsColumns = new List<string>();
                    RequestResultsRows = new Dictionary<int, List<object>>();
                    for (int i = 0; i < reader.FieldCount; ++i)
                        RequestResultsColumns.Add(reader.GetName(i));

                    int rowIndex = 0;
                    while (reader.Read())
                    {
                        List<object> rowValues = new List<object>();

                        for (int i = 0; i < reader.FieldCount; ++i)
                        {
                            int columnIndex = reader.GetOrdinal(RequestResultsColumns[i]);
                            if (columnIndex == -1)
                                return false;

                            rowValues.Add(reader.GetFieldValue<object>(columnIndex));
                        }

                        RequestResultsRows.Add(rowIndex++, rowValues);
                    }
                }

                return true;
            }

            private bool ExecuteRequest(out object outValue)
            {
                RequestCommand.ExecuteNonQuery();
                outValue = RequestCommand.Parameters["@Result"].Value;

                return true;
            }

            /// <summary>
            /// Выполнить запрос.
            /// В случае возникновения ошибки, метод вернёт false и запишет возникшую ошибку, которую можно будет посмотреть вызвав @GetLastError.
            /// </summary>
            /// <returns>Результат успешности выполнения запроса.</returns>
            public bool Execute()
            {
                if (PrepareRequest())
                {
                    try
                    {
                        return ExecuteRequest();
                    }catch(MySqlException e)
                    {
                        LastError = e.Message;
                        return false;
                    }
                }
                else
                    return false;
            }

            /// <summary>
            /// Выполнить запрос с указанным выходным аргументом.
            /// В случае возникновения ошибки, метод вернёт false и запишет возникшую ошибку, которую можно будет посмотреть вызвав @GetLastError.
            /// </summary>
            /// <param name="outValue">Переменная, в которую будет записан результат.</param>
            /// <returns>Результат успешности выполнения запроса.</returns>
            public bool Execute(out object outValue)
            {
                if (PrepareRequest())
                {
                    try
                    {
                        return ExecuteRequest(out outValue);
                    }catch (MySqlException e)
                    {
                        outValue = null;
                        LastError = e.Message;
                        return false;
                    }
                }
                else
                {
                    outValue = null;
                    return false;
                }
            }
        }

        /// <summary>
        /// Эта функция необходима для создания нового подключения к БД. Если подключение было создано ранее, оно будет закрыто и создано новое с указанными параметрами.
        /// </summary>
        /// <param name="address">Адрес сервера с БД.</param>
        /// <param name="port">Порт сервера с БД.</param>
        /// <param name="database">Название БД используемой на сервере.</param>
        /// <param name="username">Имя пользователя, который обладает необходимыми для доступа к БД правами.</param>
        /// <param name="password">Пароль для указанного имени пользователя.</param>
        /// <exception cref="ArgumentNullException">Данное исключение будет брошено в случае, если не указан какой-либо из необходимых для подключения параметров.</exception>
        public static void Instantiate(string address, UInt16? port, string database, string username, string password)
        {
            if (ConnectionEstabilished && Connection != null)
                Connection.Close();

            ConnectionEstabilished = false;

            if (address == null || database == null || username == null || password == null)
                throw new ArgumentNullException("Отсутствуют необходимые для подключения данные!");

            ServerAddress = address;
            ServerPort = port ?? MySqlDefaultPort;
            ServerDatabase = database;
            ServerUserName = username;
            ServerPassword = password;

            Connection = new MySqlConnection(
                "Server=" + ServerAddress +
                ";Port=" + ServerPort.ToString() +
                ";Database=" + ServerDatabase +
                ";User Id=" + ServerUserName +
                ";Password=" + password
            );
            Connection.Open();

            ConnectionEstabilished = true;
            ConnectionsThisSession++;
        }
    }
}