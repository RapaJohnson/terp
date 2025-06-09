namespace DemonstrationExam
{
    partial class ProductCardControl
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelProductTypeName = new System.Windows.Forms.Label();
            this.labelArticul = new System.Windows.Forms.Label();
            this.labelPrice = new System.Windows.Forms.Label();
            this.labelMaterialType = new System.Windows.Forms.Label();
            this.labelBuildTime = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelProductTypeName
            // 
            this.labelProductTypeName.AutoEllipsis = true;
            this.labelProductTypeName.Font = new System.Drawing.Font("Candara", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelProductTypeName.ForeColor = System.Drawing.Color.Black;
            this.labelProductTypeName.Location = new System.Drawing.Point(3, 3);
            this.labelProductTypeName.Name = "labelProductTypeName";
            this.labelProductTypeName.Size = new System.Drawing.Size(277, 15);
            this.labelProductTypeName.TabIndex = 0;
            this.labelProductTypeName.Text = "Кровати | Кровать с подъемным механизмом с матрасом 1600х2000 Венге";
            // 
            // labelArticul
            // 
            this.labelArticul.AutoSize = true;
            this.labelArticul.Font = new System.Drawing.Font("Candara", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelArticul.ForeColor = System.Drawing.Color.Black;
            this.labelArticul.Location = new System.Drawing.Point(1, 18);
            this.labelArticul.Name = "labelArticul";
            this.labelArticul.Size = new System.Drawing.Size(47, 13);
            this.labelArticul.TabIndex = 1;
            this.labelArticul.Text = "Артикул";
            // 
            // labelPrice
            // 
            this.labelPrice.AutoSize = true;
            this.labelPrice.Font = new System.Drawing.Font("Candara", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelPrice.ForeColor = System.Drawing.Color.Black;
            this.labelPrice.Location = new System.Drawing.Point(1, 31);
            this.labelPrice.Name = "labelPrice";
            this.labelPrice.Size = new System.Drawing.Size(128, 13);
            this.labelPrice.TabIndex = 2;
            this.labelPrice.Text = "Минимальная стоимость";
            // 
            // labelMaterialType
            // 
            this.labelMaterialType.AutoSize = true;
            this.labelMaterialType.Font = new System.Drawing.Font("Candara", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelMaterialType.ForeColor = System.Drawing.Color.Black;
            this.labelMaterialType.Location = new System.Drawing.Point(1, 44);
            this.labelMaterialType.Name = "labelMaterialType";
            this.labelMaterialType.Size = new System.Drawing.Size(104, 13);
            this.labelMaterialType.TabIndex = 3;
            this.labelMaterialType.Text = "Основной материал";
            // 
            // labelBuildTime
            // 
            this.labelBuildTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelBuildTime.AutoSize = true;
            this.labelBuildTime.Font = new System.Drawing.Font("Candara", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelBuildTime.ForeColor = System.Drawing.Color.Black;
            this.labelBuildTime.Location = new System.Drawing.Point(288, 3);
            this.labelBuildTime.Name = "labelBuildTime";
            this.labelBuildTime.Size = new System.Drawing.Size(150, 15);
            this.labelBuildTime.TabIndex = 4;
            this.labelBuildTime.Text = "Время изготовления: 12ч";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.labelProductTypeName);
            this.panel1.Controls.Add(this.labelBuildTime);
            this.panel1.Controls.Add(this.labelArticul);
            this.panel1.Controls.Add(this.labelMaterialType);
            this.panel1.Controls.Add(this.labelPrice);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(445, 61);
            this.panel1.TabIndex = 5;
            // 
            // ProductCardControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(223)))), ((int)(((byte)(255)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panel1);
            this.Name = "ProductCardControl";
            this.Size = new System.Drawing.Size(445, 61);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelProductTypeName;
        private System.Windows.Forms.Label labelArticul;
        private System.Windows.Forms.Label labelPrice;
        private System.Windows.Forms.Label labelMaterialType;
        private System.Windows.Forms.Label labelBuildTime;
        private System.Windows.Forms.Panel panel1;
    }
}
