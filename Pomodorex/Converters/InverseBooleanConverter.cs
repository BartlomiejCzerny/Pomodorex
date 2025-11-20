using System.Globalization;

namespace Pomodorex.Converters
{
    /// <summary>
    /// Konwerter wartości logicznej (bool) używany w XAML.
    /// Odwraca wartość boolean, np. true → false, false → true.
    /// Przydatny do powiązań (Binding), gdy chcemy np. odwrócić stan IsEnabled.
    /// </summary>
    public class InverseBooleanConverter : IValueConverter
    {
        /// <summary>
        /// Konwertuje wartość źródłową na wartość docelową.
        /// Jeśli wartość jest bool, zwraca przeciwną; w przeciwnym razie false.
        /// </summary>
        /// <param name="value">Wartość źródłowa (związana w XAML, np. isStarted)</param>
        /// <param name="targetType">Typ docelowy (XAML oczekuje np. bool)</param>
        /// <param name="parameter">Opcjonalny parametr konwertera (nieużywany tutaj)</param>
        /// <param name="culture">Informacje o kulturze (nieużywane tutaj)</param>
        /// <returns>Odwrócona wartość bool lub false jeśli input nie jest bool</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool boolValue ? !boolValue : false;
        }

        /// <summary>
        /// Konwertuje wartość docelową z powrotem na wartość źródłową.
        /// W tym konwerterze działa identycznie jak Convert (odwraca bool).
        /// </summary>
        /// <param name="value">Wartość docelowa (np. z kontrolki XAML)</param>
        /// <param name="targetType">Typ źródłowy (XAML oczekuje np. bool)</param>
        /// <param name="parameter">Opcjonalny parametr (nieużywany)</param>
        /// <param name="culture">Informacje o kulturze (nieużywane)</param>
        /// <returns>Odwrócona wartość bool lub false jeśli input nie jest bool</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool boolValue ? !boolValue : false;
        }
    }
}
