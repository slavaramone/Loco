using Contracts.Enums;
using Contracts.Requests;
using SharedLib.Filters;
using System;
using System.Collections.Generic;

namespace Management.Ui.Controllers
{
    /// <summary>
    /// Запрос получения списка видефайлов по фильтру
    /// </summary>
    public class VideoListRequest : CommonFilterContract
    {

        /// <summary>
        /// Id локомотивов
        /// </summary>
        public List<Guid> LocoIds { get; set; } = new List<Guid>();

        /// <summary>
        /// Список расположений камер
        /// </summary>
        public List<int> CameraPositions { get; set; } = new List<int>();

        /// <summary>
        /// Сортировка вида field-(desc|asc). Доступные поля: startdatetime, enddatetime. Пример startdatetime-desc
        /// </summary>
        [SortValidation(new string[] { "startdatetime", "enddatetime" }, new string[] { "asc", "desc" })]
        public string[] Sort { get; set; } = new string[0];
    }
}
