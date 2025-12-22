namespace Pacovallet.Core.Extensions
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// Assume uma data para o fuso horário passado, e retorna seu valor em UTC.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="currentOffset">Fuso horário a ser considerado ao valor.-12 a 12</param>
        /// <returns></returns>
        public static DateTime TransformToUtc(this DateTime dateTime, int currentOffset)
        {
            dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);

            dateTime = dateTime.AddHours(currentOffset * -1);

            dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);

            return dateTime;
        }
    }
}
