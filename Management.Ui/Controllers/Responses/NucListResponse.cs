using System.Collections.Generic;

namespace Management.Ui.Controllers
{
    /// <summary>
    /// Список номеров NUC'ов с номерами камерами
    /// </summary>
    public class NucListResponse
    {
        /// <summary>
        /// Номер NUC'а
        /// </summary>
        public string NucNumber { get; set; }

        /// <summary>
        /// Номера камера NUC'а
        /// </summary>
        public List<string> CameraNumbers { get; set; }

    }
}
