using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;

namespace Lab_3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            DataTable dt = new DataTable();

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=mybd.db;Version=3;"))
            {
                conn.Open();

                string sql = "SELECT * FROM processors";

                using (SQLiteCommand sqcmd = new SQLiteCommand(sql, conn))
                {
                    using (SQLiteDataReader reader = sqcmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }

                conn.Close();
            }

            DataGrid.ItemsSource = dt.DefaultView;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=mybd.db;Version=3;"))
            {
                conn.Open();

                string sql = "INSERT INTO processors(brand, model, price, releaseDate) VALUES(@brand, @model, @price, @releaseDate)";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@brand", brandTextBox.Text);
                    cmd.Parameters.AddWithValue("@model", modelTextBox.Text);
                    cmd.Parameters.AddWithValue("@price", decimal.Parse(priceTextBox.Text));
                    cmd.Parameters.AddWithValue("@releaseDate", releaseDateTextBox.Text);

                    cmd.ExecuteNonQuery();
                }

                conn.Close();
            }

            // Обновление отображаемых данных
            LoadData();
        }

        private void OutputButton_Click(object sender, RoutedEventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=mybd.db;Version=3;"))
            {
                conn.Open();
                string query = "SELECT * FROM processors WHERE id = @value";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    int idValue;
                    if (Int32.TryParse(id.Text, out idValue))
                    {
                        cmd.Parameters.AddWithValue("@value", idValue);
                    }
                    else
                    {
                        // Обработка ошибки
                        MessageBox.Show("Введите корректный ID.");
                        return;
                    }

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Заполнение текстовых полей данными процессора
                            brandTextBox.Text = reader.GetString(reader.GetOrdinal("brand"));
                            modelTextBox.Text = reader.GetString(reader.GetOrdinal("model"));
                            priceTextBox.Text = reader.GetDecimal(reader.GetOrdinal("price")).ToString();
                            releaseDateTextBox.Text = reader.GetString(reader.GetOrdinal("releaseDate"));
                        }
                        else
                        {
                            MessageBox.Show("Процессор не найден.");
                        }
                    }
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=mybd.db;Version=3;"))
            {
                conn.Open();
                string sql = "UPDATE processors SET brand = @brand, model = @model, price = @price, releaseDate = @releaseDate WHERE id = @id";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    int idValue;
                    if (Int32.TryParse(id.Text, out idValue))
                    {
                        cmd.Parameters.AddWithValue("@id", idValue);
                        cmd.Parameters.AddWithValue("@brand", brandTextBox.Text);
                        cmd.Parameters.AddWithValue("@model", modelTextBox.Text);
                        cmd.Parameters.AddWithValue("@price", decimal.Parse(priceTextBox.Text));
                        cmd.Parameters.AddWithValue("@releaseDate", releaseDateTextBox.Text);

                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        MessageBox.Show("Введите корректный ID.");
                        return;
                    }
                }

                conn.Close();
            }

            LoadData();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=mybd.db;Version=3;"))
            {
                conn.Open();
                string sql = "DELETE FROM processors WHERE id = @id";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    int idValue;
                    if (Int32.TryParse(id.Text, out idValue))
                    {
                        cmd.Parameters.AddWithValue("@id", idValue);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        MessageBox.Show("Введите корректный ID.");
                        return;
                    }
                }

                conn.Close();
            }

            LoadData();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string cs = "Data Source=mybd.db";

            using var con = new SQLiteConnection(cs);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            // Создание новой таблицы для процессоров
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS processors(
                            id INTEGER PRIMARY KEY,
                            brand TEXT,
                            model TEXT,
                            price REAL,
                            releaseDate TEXT)";
            cmd.ExecuteNonQuery();

            // Чтение данных из таблицы
            DataTable dt = new DataTable();

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=mybd.db;Version=3;"))
            {
                conn.Open();

                string sql = "SELECT * FROM processors";

                using (SQLiteCommand sqcmd = new SQLiteCommand(sql, conn))
                {
                    using (SQLiteDataReader reader = sqcmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }

                conn.Close();
            }

            DataGrid.ItemsSource = dt.DefaultView;
            LoadData();
        }
    }
}
