using Contracts.Enums;
using System;

namespace Contracts.Responses
{
    /// <summary>
    /// Контракт камеры
    /// </summary>
    public class CameraContract
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Место установки камеры
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Номер NUC'as
        /// </summary>
        public string NucNumber { get; set; }

        /// <summary>
        /// Номер камеры
        /// </summary>
        public string Number { get; set; }
    }
}
