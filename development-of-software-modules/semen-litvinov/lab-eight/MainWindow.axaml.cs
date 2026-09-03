using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PhysicsApp;

// Аналог: public partial class Form1 : Form
public partial class MainWindow : Window
{
    // Константы физики
    private const double RhoWater = 1000.0; // плотность воды, кг/м³
    private const double G = 9.81;           // ускорение свободного падения, м/с²

    // Аналог конструктора Form1() с InitializeComponent()
    public MainWindow()
    {
        InitializeComponent();
    }

    // =====================================================
    // ЗАДАЧА 1: Объём цилиндра
    // Формула: F_арх = P_воздух - P_вода
    //          V = F_арх / (ρ × g)
    //          Перевод в см³: × 1_000_000
    // =====================================================

    // Аналог: private void CalcVolume_B_Click(object sender, EventArgs e)
    private void CalcVolume_Click(object sender, RoutedEventArgs e)
    {
        // Очищаем предыдущие ошибки — аналог Error_L.Text = ""
        Error_L.Text = "";

        // Получение данных из TextBox — аналог Convert.ToDouble(AirWeight_TB.Text)
        if (!double.TryParse(AirWeight_TB.Text, out double airWeight) || airWeight <= 0)
        {
            Error_L.Text = "Ошибка: введите корректный вес в воздухе (больше 0)";
            return;
        }

        if (!double.TryParse(WaterWeight_TB.Text, out double waterWeight) || waterWeight < 0)
        {
            Error_L.Text = "Ошибка: введите корректный вес в воде (0 или больше)";
            return;
        }

        if (waterWeight >= airWeight)
        {
            Error_L.Text = "Ошибка: вес в воде должен быть меньше веса в воздухе";
            return;
        }

        // Расчёт архимедовой силы (Н)
        double archForce = airWeight - waterWeight;

        // Расчёт объёма в м³: V = F / (ρ × g)
        double volumeM3 = archForce / (RhoWater * G);

        // Перевод в см³ (1 м³ = 1_000_000 см³)
        double volumeCm3 = volumeM3 * 1_000_000;

        // Вывод результата — аналог: Res_TB.Text = res.ToString()
        Volume_TB.Text = $"{volumeCm3:F2}";
    }

    // =====================================================
    // ЗАДАЧА 3: Гидравлический пресс
    // Формула: P₁ = P₂  →  F₁/S₁ = F₂/S₂
    //          F₂ = F₁ × (S₂/S₁) = F₁ × k
    // =====================================================

    private void CalcPress_Click(object sender, RoutedEventArgs e)
    {
        Error_L.Text = "";

        // Получение F1
        if (!double.TryParse(F1_TB.Text, out double f1) || f1 <= 0)
        {
            Error_L.Text = "Ошибка: введите корректную силу F₁ (больше 0)";
            return;
        }

        // Получение коэффициента k
        if (!double.TryParse(K_TB.Text, out double k) || k <= 0)
        {
            Error_L.Text = "Ошибка: введите корректный коэффициент k (больше 0)";
            return;
        }

        // Расчёт F2 = F1 × k
        double f2 = f1 * k;

        // Вывод результата
        F2_TB.Text = $"{f2:F2}";
    }
}