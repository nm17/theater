using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;

using Dict = System.Collections.Generic.Dictionary<string, dynamic>;

namespace TheaterTickets
{
    

    public class PlaceDontExistException : Exception
    {
        public PlaceDontExistException()
        {

        }
    }

    public class PlaceAlreadyBookedException : Exception
    {
        public PlaceAlreadyBookedException()
        {

        }
    }

    

    /// <summary>
    /// API для общения с TheaterAPI написаном на питоне.
    /// Использует HTTP для передачи данных, JSON для её кодировки.
    /// 
    /// </summary>
    public class TheaterAPI
    {
        private RestClient client;
        private string jwt_token;

        

        /// <summary>
        /// Сам кеш в формате словаря.
        /// </summary>
        private Dictionary<string, Place> Cache = new Dictionary<string, Place>();

        public TheaterAPI()
        {
            client = new RestClient("http://192.168.1.72");
        }


        private bool Authenticated
        {
            get
            {
                return jwt_token.Length != 0;
            }
        }

        /// <summary>
        /// Вспомогательная функция для отправки POST запроса на сервер.
        /// </summary>
        /// <param name="url">Путь запроса к серверу. Отправляется в виде <code>"theaterapi/" + <paramref name="url"/></code></param>
        /// <param name="ReqJson">Словарь <see cref="Dict"/> JSON для отправки</param>
        /// <param name="UseAuth">Использовать </param>
        /// <returns></returns>
        private IRestResponse SendRequest(string url, Dict ReqJson, bool UseAuth=false)
        {
            if (UseAuth)
            {
                if (!Authenticated)
                {
                    throw new UnauthorizedAccessException();
                }
                ReqJson.Add("jwt", jwt_token);
            }
            var Request = new RestRequest("theaterapi/" + url, Method.POST);
            Request.AddJsonBody(ReqJson);
            Request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            return client.Execute(Request);
        }

        /// <summary>
        /// Свободно ли место на ряду <paramref name="Row"/>, место <paramref name="Place"/>
        /// Делает запрос на сервер вне зависимости от <see cref="Cache">кеша</see>.
        /// </summary>
        /// <remarks>Если нужно использовать кеш для уменьшения нагрузки на сервер используйте <see cref="GetInfo(int, int)"/></remarks>
        /// <exception cref="PlaceDontExistException">Если место не найдено (т.е отправлено 404)</exception>
        /// <param name="Row">Ряд</param>
        /// <param name="Place">Место</param>
        /// <returns><see cref="Place"/> найденого места</returns>
        private Place RequestInfo(int Row, int Place)
        {
            var response = SendRequest("placeinfo", new Dict
            {  // Добавляем к запросу ряд и место о котором нам нужно узнать
                {"row", Row},
                {"place", Place}
            });
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new PlaceDontExistException();  // Если получен 404, место является несуществующим местом
            }
            var json_result = JsonConvert.DeserializeObject<Dict>(response.Content);

            return new Place(json_result);
        }

        private string RowPlaceToString(int row, int place)
        {
            return row.ToString() + ":" + place.ToString();
        }

        /// <summary>
        /// Все тоже самое как в <see cref="RequestInfo(int, int)"/> только используется кеш.
        /// </summary>
        /// <param name="row">Ряд</param>
        /// <param name="place">Место</param>
        /// <returns><see cref="Place"/> найденого места</returns>
        public Place GetInfo(int row, int place)
        {
            Place info;
            if (Cache.ContainsKey(RowPlaceToString(row, place)))  // Если есть в кеше
            {
                info = Cache[RowPlaceToString(row, place)];
                return info;  // Возвращаем значение из кеша
            }
            else
            {
                try
                {
                    info = RequestInfo(row, place);
                    Cache[RowPlaceToString(row, place)] = info;
                    return info;
                }
                catch (PlaceDontExistException)  // Возвращаем и сохраняем в кеш что место пустое
                {
                    Cache[RowPlaceToString(row, place)] = new Place(row, place);
                    return new Place(row, place);
                }
            }
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="username">Имя пользователя</param>
        /// <param name="password">Пароль, в открытом тексте</param>
        public void Register(string username, string password)
        {
            var response = SendRequest("register", new Dict
            {
                {"username", username},
                {"password", password}
            });
            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                throw new Exception();
            }
        }

        public void Login(string username, string password)
        {
            var response = SendRequest("login", new Dict
            {
                {"username", username},
                {"password", password}
            });
            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                throw new Exception();
            }
            var json_result = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>(response.Content);
            jwt_token = json_result["jwt"];
        }

        /// <summary>
        /// Резервирует
        /// </summary>
        /// <exception cref="PlaceAlreadyBookedException">Если место уже забронировано</exception>
        /// <param name="row"></param>
        /// <param name="place"></param>
        public void Book(int row, int place)
        {
            var response = SendRequest("book", new Dict{
                {"row", row},
                {"place", place}
            }, true);

            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                throw new PlaceAlreadyBookedException();
            }
        }
    }
}
