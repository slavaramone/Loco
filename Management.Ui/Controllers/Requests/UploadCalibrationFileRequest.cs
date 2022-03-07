using Microsoft.AspNetCore.Http;
using System;

namespace Management.Ui.Controllers
{
    /// <summary>
    /// Запрос калибровки датчиков на локомотиве
    /// </summary>
    public class UploadCalibrationFileRequest
    {
        /// <summary>
        /// Файл отправленный http request
        /// </summary>
        public IFormFile File { get; set; }

        /// <summary>
        /// Имя worksheet с данными калибровки
        /// </summary>
        public string WorksheetName { get; set; }

        /// <summary>
        /// Номер первой строки с калибровочными данными
        /// </summary>
        public int StartRow { get; set; }

        /// <summary>
        /// Номер первой колонки с калибровочными данными
        /// </summary>
        public int StartCol { get; set; }

        /// <summary>
        /// Id группы ДУТ расположенных слева
        /// </summary>
        public Guid LeftSensorId { get; set; }

        /// <summary>
        /// Id группы ДУТ расположенных справа
        /// </summary>
        public Guid RightSensorId { get; set; }
    }
}
