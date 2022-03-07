using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Tracker.Controllers
{
    /// <summary>
    /// Передача координат gps трекеров
    /// </summary>
    public class TrackRequest
    {
        /// <summary>
        /// Широта
        /// </summary>
        [Required(ErrorMessage = "Широта обязательна")]
        [Range(-90, 90, ErrorMessage = "Широта от -90 до 90")]
        public double? Latitude { get; set; }

        /// <summary>
        /// Долгота
        /// </summary>
        [Required(ErrorMessage = "Долгота обязательна")]
        [Range(0, 180, ErrorMessage = "Долгота от 0 до 180")]
        public double? Longitude { get; set; }

        /// <summary>
        /// Высота
        /// </summary>
        [Required(ErrorMessage = "Высота обязательна")]
        [Range(-200, 1000, ErrorMessage = "Долгота от -200 до 1000")]
        public double? Altitude { get; set; }

        /// <summary>
        /// Скорость
        /// </summary>
        [Range(0, 100, ErrorMessage = "Скорость не превышает 100")]
        public double? Speed { get; set; }

        /// <summary>
        /// Направление движения
        /// </summary>
        [Range(0, 360, ErrorMessage = "Направление движения от 0 до 360")]
        public double? Heading { get; set; }

        /// <summary>
        /// Время фиксации координат трекером
        /// </summary>
        public DateTimeOffset DateTime { get; set; }
    }
}
