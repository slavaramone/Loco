namespace Contracts.Responses
{
    /// <summary>
    /// Ответ на запрос калибровки групп ДУТ
    /// </summary>
    public class UploadCalibrationResponse
    {
        /// <summary>
        /// Удачно/неудачно
        /// </summary>
        public bool IsCompletedSuccessfuly { get; set; }
    }
}
