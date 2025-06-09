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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace DemonstrationExam
{
    ///
    /// РЕАЛИЗАЦИЯ В ОДНУ ФОРМУ
    /// 
    /// TO DO:
    ///  ПОМЕНЯТЬ НАЗВАНИЕ ОКНА
    ///  ПОМЕНЯТЬ ИКОНКУ У ОКНА
    ///  ДОБАВИТЬ ИКОНКУ ПО ЦЕНТРУ ГЛАВНОГО МЕНЮ (ТАМ ПИКЧЕР БОКС)
    ///  КОММЕНТАРИИ ПО КОДУ
    ///  СТИЛЬ НАИМЕНОВАНИЯ КАМЕЛ ИЛИ СНЕЙК
    ///  ОБРАБОТЧИКИ ОШИБОК
    ///  ЗАКИНУТЬ В ГИТ
    ///  СДЕЛАТЬ КОММИТЫ В ГИТ
    ///  СДЕЛАТЬ README В ГИТ
    ///  
    /// НЕ ЗАБЫТЬ УДАЛИТЬ ЭТО!!!!!!!!!!!!!!!!!
    ///  
    ///  ну было и было...
    ///

    public partial class FormAppHub : Form
    {
        #region Приватные поля

        private readonly DatabaseContext database;

        private int? productId;

        private readonly string appName;
        private readonly string mainMenuName = "Главное меню";
        private readonly string secondMenuName = "Продукты таблица";
        private readonly string thirdMenuName = "Продукты список";
        private readonly string fourthMenuName = "Цеха";

        #endregion

        #region Параметры списков

        // Параметры для сортируемого списка таблицы продуктов
        public class ProductDisplayItem
        {
            public int ProductID { get; set; }
            public string ProductTypeName { get; set; }
            public string ProductName { get; set; }
            public string Articul { get; set; }
            public decimal MinimumPrice { get; set; }
            public string MaterialTypeName { get; set; }
            public double BuildTime { get; set; }
        }

        // Параметры для сортируемого списка таблицы цехов
        public class WorkshopsDisplayItem
        {
            public int WorshopID { get; set; }
            public string WorkshopName { get; set; }
            public int WorkersAmount { get; set; }
            public double BuildTime { get; set; }
        }

        #endregion

        #region ПРОГРАММНАЯ ЛОГИКА

        // Первоначальная настройка формы
        public FormAppHub()
        {
            try
            {
                InitializeComponent();
                database = new DatabaseContext();
                appName = this.Text;

                this.Size = new Size(720, 480);

                PanelToggle(panelMainMenu);
                this.Text = $"{appName} | {mainMenuName}";

                TestDatabaseConnection();
            }
            catch (Exception ex)
            {
                ShowError($"Критическая ошибка при запуске главной формы: {ex.Message}");
            }
        }

        // Обработчик открытия панели, имя которой он определяет по имени нажатой кнопки
        // так же меняет название окна для удобного ориентирования
        private void ButtonOpenMenu_Click(object sender, EventArgs e)
        {
            try
            {
                if (!(sender is Button clickedButton)) return;

                Panel panelToToggle = null;

                switch (clickedButton.Name)
                {
                    case "buttonOpenMenuProducts":
                    case "buttonOpenMenuProducts1":
                    case "buttonOpenMenuProductsTable":
                        panelToToggle = panelMenuProductsTable;
                        this.Text = $"{appName} | {secondMenuName}";
                        break;
                    case "buttonOpenMenuProductsList1":
                    case "buttonOpenMenuProductsList":
                        panelToToggle = panelMenuProductsList;
                        this.Text = $"{appName} | {thirdMenuName}";
                        break;
                    case "btnEditProduct1":
                    case "btnEditProduct2":
                        if (IsProductsEdit() && productId.HasValue)
                        {
                            LoadProductData(productId.Value);
                            panelToToggle = panelMenuProductEdit;
                            this.Text = $"{appName} | {fourthMenuName}";
                        }
                        break;
                    case "btnAddProduct1":
                    case "btnAddProduct2":
                        ClearEditPanel();
                        panelToToggle = panelMenuProductEdit;
                        this.Text = $"{appName} | {fourthMenuName}";
                        break;
                    case "buttonOpenMenuWorkshops":
                        panelToToggle = panelWorkshops;
                        this.Text = $"{appName} | {fourthMenuName}";
                        break;
                    case "bntOpenPanelResult":
                        panelToToggle = panelWorkshopResult;
                        this.Text = $"{appName} | {fourthMenuName}";

                        ButtonCalculate_Click();
                        break;
                }

                if (panelToToggle != null)
                {
                    PanelToggle(panelToToggle);
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при нажатии на кнопку перехода {sender}: {ex.Message}");
            }
        }

        // Обработчик нажатия кнопки возвращения назад в главное меню
        private void ButtonBackToMain_Click(object sender, EventArgs e)
        {
            try
            {
                PanelToggle(panelMainMenu);
                this.Text = $"{appName} | {mainMenuName}";
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при переходу к главной панели: {ex.Message}");
            }
        }

        // Метод переключения активной панели приложения
        private void PanelToggle(Panel panelToShow)
        {
            try
            {
                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl is Panel panel)
                    {
                        panel.Location = new Point(15, 15);
                        panel.Dock = DockStyle.Fill;
                        bool isPanelToToggle = panel == panelToShow;
                        panel.Visible = isPanelToToggle;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при переходу к панели {panelToShow}: {ex.Message}");
            }
        }

        // Булевая переменная с проверкой на корректность ввода параметров в редакторе
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(textBoxArticul.Text))
            {
                ShowWarning("Артикул не может быть пустым");
                return false;
            }
            if (comboBoxProductType.SelectedIndex == -1)
            {
                ShowWarning("Не выбран тип продукта");
                return false;
            }
            if (string.IsNullOrWhiteSpace(textBoxProductName.Text))
            {
                ShowWarning("Название продукта не может быть пустым");
                return false;
            }
            if (numericUpDownPrice.Value < 0)
            {
                ShowWarning("Стоимость не может быть отрицательной");
                return false;
            }
            if (comboBoxMaterial.SelectedIndex == -1)
            {
                ShowWarning("Не выбран материал");
                return false;
            }
            return true;
        }

        // вывод ошибок
        private void ShowError(string message)
        {
            MessageBox.Show(message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // вывод предупреждения
        private void ShowWarning(string message)
        {
            MessageBox.Show(message, "Предупреждение!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        //Освобождение ресурсов при закрытии программы
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            database?.Dispose();
        }

        #endregion

        #region БАЗА ДАННЫХ

        // Метод проверки подключения к базе данных
        private void TestDatabaseConnection()
        {
            try
            {
                LoadProductsTable();
                LoadProductTypes();
                LoadMaterialTypes();
                LoadWorkshopsTable();
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка подключения к БД: {ex.Message}");
            }
        }

        // Загрузка таблицы продуктов из базы данных вместе с временем производства
        private void LoadProductsTable()
        {
            try
            {
                var products = database.Products
                .Include(p => p.ProductType)
                .Select(p => new
                {
                    p.ProductID,
                    p.ProductType.ProductTypeName,
                    p.ProductName,
                    p.Articul,
                    p.MinimumPrice,
                    p.MaterialType.MaterialTypeName,
                })
                .OrderBy(p => p.ProductID)
                .ToList();

                var productsWithBuildTime = products.Select(p => new ProductDisplayItem
                {
                    ProductID = p.ProductID,
                    ProductTypeName = p.ProductTypeName,
                    ProductName = p.ProductName,
                    Articul = p.Articul.ToString(),
                    MinimumPrice = p.MinimumPrice,
                    MaterialTypeName = p.MaterialTypeName,
                    BuildTime = LoadProductBuildTime(p.ProductID) ?? 0,
                })
                .ToList();

                var sortableList = new SortableBindingList<ProductDisplayItem>(productsWithBuildTime);

                dataGridViewProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dataGridViewProducts.DataSource = sortableList;

                SetColumnHeader(dataGridViewProducts, "ProductID", "ID продукта");
                SetColumnHeader(dataGridViewProducts, "ProductTypeName", "Тип");
                SetColumnHeader(dataGridViewProducts, "ProductName", "Наименование");
                SetColumnHeader(dataGridViewProducts, "Articul", "Артикул");
                SetColumnHeader(dataGridViewProducts, "MinimumPrice", "Минимальная цена (руб.)");
                SetColumnHeader(dataGridViewProducts, "MaterialTypeName", "Основной материа");
                SetColumnHeader(dataGridViewProducts, "BuildTime", "Время изготовления (ч.)");

                try
                {
                    flowLayoutPanelProducts.Controls.Clear();

                    foreach (var p in productsWithBuildTime)
                    {
                        var card = new ProductCardControl();
                        card.SetData(
                            $"{p.ProductTypeName} | {p.ProductName}",
                            p.Articul.ToString(),
                            (double)p.MinimumPrice,
                            p.MaterialTypeName,
                            p.BuildTime
                        );

                        card.AutoSize = false;
                        flowLayoutPanelProducts.Controls.Add(card);
                    }

                    flowLayoutPanelProducts.AutoSize = false;
                    flowLayoutPanelProducts.WrapContents = true;
                    flowLayoutPanelProducts.AutoScroll = true;
                }
                catch (Exception ex)
                {
                    ShowError($"Ошибка при загрузке списка продуктов: {ex.Message}");
                }

            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при загрузке таблицы Products: {ex.Message}");
            }
        }

        // Метод суммирования времени производства каждого продукта с каждого цеха
        private double? LoadProductBuildTime(int productId)
        {
            try
            {
                var buildTime = database.ProductWorkshops
                .Where(pw => pw.ProductID == productId)
                .Select(pw => pw.BuildTime)
                .ToList();

                if (buildTime.Count == 0) return null;

                return Math.Round(buildTime.Sum(), 4);
            }
            catch(Exception ex)
            {
                ShowError($"Ошибка при рассчетах времени производства: {ex.Message}");
                return null;
            }
        }

        // Загрузка таблицы цехов из базы данных вместе с временем производства на каждый тип продукта
        private void LoadWorkshopsTable()
        {
            try
            {
                var workshops = database.ProductWorkshops
                .Include(pw => pw.Workshops)
                .Select(pw => new
                {
                    pw.Workshops.WorshopID,
                    pw.Workshops.WorkshopName,
                    pw.Workshops.WorkersAmount,
                    pw.BuildTime
                })
                .ToList();

                var workshopsSortable = workshops.Select(products => new WorkshopsDisplayItem
                {
                    WorshopID = products.WorshopID,
                    WorkshopName = products.WorkshopName,
                    WorkersAmount = products.WorkersAmount,
                    BuildTime = products.BuildTime
                })
                .ToList();

                var sortableList = new SortableBindingList<WorkshopsDisplayItem>(workshopsSortable);

                dataGridViewWorkshops.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dataGridViewWorkshops.DataSource = sortableList;

                SetColumnHeader(dataGridViewWorkshops, "WorshopID", "ID цеха");
                SetColumnHeader(dataGridViewWorkshops, "WorkshopName", "Имя цеха");
                SetColumnHeader(dataGridViewWorkshops, "WorkersAmount", "Количество работников");
                SetColumnHeader(dataGridViewWorkshops, "BuildTime", "Время изготовления (ч.)");
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при загрузке таблицы Workshops: {ex.Message}");
            }
        }

        // Единый метод переименования столбцов на русский язык
        private void SetColumnHeader(DataGridView dataGridView, string columnName, string headerText)
        {
            if (dataGridView.Columns[columnName] != null)
            {
                dataGridView.Columns[columnName].HeaderText = headerText;
            }
            else
            {
                ShowError($"Ошибка при вставке значения {columnName}, проверьте ссылочную целостность");
            }
        }

        public int CalculateMaterialAmount(int productTypeId, int materialTypeId, int productQuantity, double param1, double param2)
        {
            // Проверка входных данных
            if (productQuantity <= 0 || param1 <= 0 || param2 <= 0)
                return -1;

            var productType = database.ProductTypes.Find(productTypeId);
            var materialType = database.MaterialTypes.Find(materialTypeId);

            if (productType == null || materialType == null)
                return -1;

            double productCoefficient = (double)productType.ProductTypeCoefficient; // вещественный коэффициент типа продукции
            double materialLossPercent = (double)materialType.MaterialTypeLossProcent; // процент потерь сырья

            // Расчет сырья на одну единицу продукции
            double materialPerUnit = param1 * param2 * productCoefficient;

            // Учет потерь сырья
            double totalMaterial = materialPerUnit * productQuantity * (1 + materialLossPercent / 100.0);

            // Округление до целого числа (в большую сторону)
            int result = (int)Math.Ceiling(totalMaterial);

            return result;
        }

        private void ButtonCalculate_Click()
        {
            if (dataGridViewProducts.CurrentRow != null)
            {
                // Получаем ID продукта из выбранной строки
                int productId = (int)dataGridViewProducts.CurrentRow.Cells["ProductID"].Value;

                // Получаем данные продукта (предполагается, что они есть в строке)
                // или загружаем из базы по ID
                var product = database.Products.Find(productId);

                if (product != null)
                {
                    int productTypeId = product.ProductTypeID;
                    int materialTypeId = product.MaterialTypeID;
                    int productQuantity = 10; // пример количества, можно брать из TextBox
                    double param1 = 2.5; // пример параметра 1, можно брать из TextBox
                    double param2 = 3.0; // пример параметра 2, можно брать из TextBox

                    // Вызываем метод расчета
                    int result = CalculateMaterialAmount(productTypeId, materialTypeId, productQuantity, param1, param2);

                    // Выводим результат в dataGridViewResult
                    // Создаем таблицу, если её нет
                    if (dataGridViewResult.Columns.Count == 0)
                    {
                        dataGridViewResult.Columns.Add("Parameter", "Параметр");
                        dataGridViewResult.Columns.Add("Value", "Значение");
                    }

                    // Очищаем предыдущие результаты
                    dataGridViewResult.Rows.Clear();

                    // Заполняем таблицу результатами
                    dataGridViewResult.Rows.Add("ID продукта", productId);
                    dataGridViewResult.Rows.Add("Тип продукта", productTypeId);
                    dataGridViewResult.Rows.Add("Материал", materialTypeId);
                    dataGridViewResult.Rows.Add("Количество", productQuantity);
                    dataGridViewResult.Rows.Add("Коэфф. типа продукции", param1);
                    dataGridViewResult.Rows.Add("Процент потерь сырья", param2);
                    dataGridViewResult.Rows.Add("Результат", result); // Добавляем результат

                }
                else
                {
                    MessageBox.Show("Продукт не найден", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите строку с продуктом", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadWorkshopsForProduct(int productId)
        {
            /*
            if (dataGridViewProducts.CurrentRow != null)
            {
                int productId = (int)dataGridViewProducts.CurrentRow.Cells["ProductID"].Value;
                LoadWorkshopsForProduct(1);
            }*/

            try
            {
                var workshops = database.ProductWorkshops
                    .Where(pw => pw.ProductID == productId)
                    .Include(pw => pw.Workshops)
                    .Select(pw => new
                    {
                        pw.Workshops.WorkshopName,
                        pw.Workshops.WorkersAmount,
                        pw.BuildTime // время изготовления в данном цехе для продукта
                    })
                    .ToList();

                dataGridViewWorkshops.DataSource = workshops;

                dataGridViewWorkshops.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                // Переименование заголовков столбцов
                if (dataGridViewWorkshops.Columns["WorkshopName"] != null)
                    dataGridViewWorkshops.Columns["WorkshopName"].HeaderText = "Название цеха";

                if (dataGridViewWorkshops.Columns["WorkerCount"] != null)
                    dataGridViewWorkshops.Columns["WorkerCount"].HeaderText = "Количество человек";

                if (dataGridViewWorkshops.Columns["BuildTime"] != null)
                    dataGridViewWorkshops.Columns["BuildTime"].HeaderText = "Время изготовления (ч.)";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке списка цехов: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Готова ли программа для редактирования продуктов
        private bool IsProductsEdit()
        {
            int selectedCount = dataGridViewProducts.SelectedRows.Count;

            if (selectedCount == 1)
            {
                var selectedRow = dataGridViewProducts.SelectedRows[0];
                int partnerId = Convert.ToInt32(selectedRow.Cells["ProductID"].Value);
                productId = partnerId;

                return true;
            }
            else if (selectedCount == 0)
            {
                ShowWarning("Пожалуйста, выберите строку для редактирования");
                return false;
            }
            else
            {
                ShowWarning("Пожалуйста, выберите только одну строку для редактирования");
                return false;
            }
        }

        // Метод очистки редактора от ранее введенных значений
        private void ClearEditPanel()
        {
            try
            {
                textBoxArticul.Clear();
                textBoxArticul.ForeColor = Color.Black;
                textBoxProductName.Clear();
                textBoxProductName.ForeColor = Color.Black;

                numericUpDownPrice.Value = numericUpDownPrice.Minimum;

                comboBoxProductType.SelectedIndex = -1;
                comboBoxMaterial.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при очистке редактора: {ex.Message}");
            }
        }

        // Метод загрузки данных из выбраной строки продуктов в редактор
        private void LoadProductData(int id)
        {
            try
            {
                var product = database.Products.Find(id);
                if (product != null)
                {
                    textBoxArticul.Text = product.Articul.ToString();
                    comboBoxProductType.SelectedValue = product.ProductTypeID;
                    textBoxProductName.Text = product.ProductName;
                    numericUpDownPrice.Value = product.MinimumPrice;
                    comboBoxMaterial.Text = product.MaterialType.MaterialTypeName;
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при загрузке данных продуктов в редактор: {ex.Message}");
            }
        }

        // Метод подгрузки в редактор типов продуктов
        private void LoadProductTypes()
        {
            try
            {
                var types = database.ProductTypes.ToList();
                comboBoxProductType.DataSource = types;
                comboBoxProductType.DisplayMember = "ProductTypeName";
                comboBoxProductType.ValueMember = "ProductTypeID";
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при загрузке типов продуктов в редактор: {ex.Message}");
            }
        }

        // Метод подгрузки в редактор типов материалов
        private void LoadMaterialTypes()
        {
            try
            {
                var materials = database.MaterialTypes.ToList();
                comboBoxMaterial.DataSource = materials;
                comboBoxMaterial.DisplayMember = "MaterialTypeName";
                comboBoxMaterial.ValueMember = "MaterialTypeID";
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при загрузке типов материала в редактор: {ex.Message}");
            }
        }

        // Обработчик нажатия кнопки сохранения введенных даных из редактора в базу данных
        private void ButtonSave_InputInfo(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInput())
                    return;

                string articulStr = textBoxArticul.Text.Trim();

                if (!int.TryParse(articulStr, out int articulInt))
                {
                    ShowError("Артикул должен быть числом");
                }

                int productTypeId = (int)comboBoxProductType.SelectedValue;
                string productName = textBoxProductName.Text.Trim();
                decimal minPrice = numericUpDownPrice.Value;
                int materialTypeId = (int)comboBoxMaterial.SelectedValue;

                if (productId.HasValue)
                {
                    var product = database.Products.Find(productId.Value);
                    if (product != null)
                    {
                        product.Articul = articulInt;
                        product.ProductTypeID = productTypeId;
                        product.ProductName = productName;
                        product.MinimumPrice = minPrice;
                        product.MaterialTypeID = materialTypeId;
                    }
                }
                else
                {
                    var product = new Products
                    {
                        Articul = articulInt,
                        ProductTypeID = productTypeId,
                        ProductName = productName,
                        MinimumPrice = minPrice,
                        MaterialTypeID = materialTypeId
                    };
                    database.Products.Add(product);
                }

                database.SaveChanges();
                TestDatabaseConnection();

                PanelToggle(panelMenuProductsTable);
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при сохранении: {ex.Message}");
            }
        }

        #endregion

        #region АВТОРИЗАЦИЯ

        // Обработчик нажатия Enter в поле логина
        private void TextLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                //txtPassword.Focus(); <- текстовый элемент строки пароля
                e.Handled = true;
            }
        }

        // Обработчик нажатия Enter в поле пароля
        private void TextPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                ButtonLogin_Click(sender, e);
                e.Handled = true;
            }
        }

        // Обработчик нажатия кнопки входа 
        private void ButtonLogin_Click(object sender, EventArgs e)
        {
            try
            {
                /* Поиск пользователя в БД по логину и паролю
                var user = database.Users.FirstOrDefault(u =>
                    u.Login == txtLogin.Text && u.Password == txtPassword.Text);

                if (user != null)
                {
                    // если пользователь найден переходим далее
                    PanelToggle(YOUR_MAIN_PANEL_NAME);
                    this.Text = $"{appName} | {mainMenuName}";
                }
                else
                {
                    // Если пользователь не найден, показываем ошибку
                    MessageBox.Show("Неверный логин или пароль!",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                */
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при входе: {ex.Message}\n\nПолное сообщение:\n{ex}");
            }
        }

        #endregion
    }
}
