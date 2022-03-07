using System;

namespace Contracts.Responses
{
    /// <summary>
    /// Контракт элемента списка сцепщиков
    /// </summary>
    public class ShunterListItemContract
    {
        /// <summary>
        /// Id сцепшика
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Имя сцепщика
        /// </summary>
        public string Name { get; set; }
    }
}
