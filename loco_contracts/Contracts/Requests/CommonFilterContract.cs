﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Requests
{
    /// <summary>
    /// Общий фильтр для запросов
    /// </summary>
    public class CommonFilterContract
    {
        /// <summary>
        /// Дата возникновения от. Пример 2020-09-18T11:52:45.876795
        /// </summary>
        public DateTimeOffset? DateTimeFrom { get; set; }

        /// <summary>
        /// Дата возникновения до. Пример 2020-09-18T11:52:45.876795
        /// </summary>
        public DateTimeOffset? DateTimeTo { get; set; }

        /// <summary>
        /// Кол-во на странице (для пагинации)
        /// </summary>
        public int? Take { get; set; }

        /// <summary>
        /// Кол-во пропускаемых записей (для пагинации)
        /// </summary>
        public int? Skip { get; set; }
    }
}
