using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    /// <summary>
    /// Форма "О программе" для отображения информации о приложении.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Форма содержит информацию о разработчике и ссылку на исходный код проекта.
    /// </para>
    /// <para>
    /// Предоставляет функционал для перехода на GitHub-страницу проекта.
    /// </para>
    /// </remarks>
    public partial class AboutForm : Form
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса AboutForm.
        /// </summary>
        public AboutForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик события нажатия кнопки закрытия формы.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Данные события.</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Обработчик события клика по ссылке на GitHub.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Данные события.</param>
        /// <remarks>
        /// <para>
        /// Открывает GitHub-страницу проекта в браузере по умолчанию.
        /// </para>
        /// <para>
        /// В случае ошибки отображает сообщение с описанием проблемы.
        /// </para>
        /// </remarks>
        private void lnkGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "https://github.com/markld-ui/DataBase",
                    UseShellExecute = true
                };

                System.Diagnostics.Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось открыть ссылку: {ex.Message}",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
