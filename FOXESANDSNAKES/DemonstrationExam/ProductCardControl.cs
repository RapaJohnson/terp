using System;
using System.Windows.Forms;

namespace DemonstrationExam
{
    public partial class ProductCardControl : UserControl
    {
        /// <summary>
        /// Самописный UserControl для вывода продукции в список согласно макету
        /// 
        /// Берет данные через публичный метод SetData, которые задаются при иниализации списка из базы данных
        /// </summary>

        public ProductCardControl()
        {
            InitializeComponent();
        }

        public void SetData(string productTypeAndName, string articul, double price, string materialType, double buildTime)
        {
            try
            {
                labelProductTypeName.Text = productTypeAndName;
                labelArticul.Text = $"Артикул: {articul}";
                labelPrice.Text = $"Минимальная стоимость для партнера: {price} руб.";
                labelMaterialType.Text = $"Основной материал: {materialType}";
                labelBuildTime.Text = $"Время изготовления: {buildTime} ч.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при передачи данных в элемент списка продуктов {productTypeAndName}: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
