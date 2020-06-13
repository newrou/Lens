namespace Lens
{
    partial class Form1
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

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.ExitButton = new System.Windows.Forms.Button();
            this.SolveButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.SolveButtonOneThread = new System.Windows.Forms.Button();
            this.buttonSaveScreen = new System.Windows.Forms.Button();
            this.buttonSaveGradient = new System.Windows.Forms.Button();
            this.comboBoxMaterial = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ExitButton
            // 
            this.ExitButton.Location = new System.Drawing.Point(1211, 365);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(129, 23);
            this.ExitButton.TabIndex = 0;
            this.ExitButton.Text = "Выход";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // SolveButton
            // 
            this.SolveButton.Location = new System.Drawing.Point(1384, 305);
            this.SolveButton.Name = "SolveButton";
            this.SolveButton.Size = new System.Drawing.Size(129, 23);
            this.SolveButton.TabIndex = 1;
            this.SolveButton.Text = "Решение \"в лоб\"";
            this.SolveButton.UseVisualStyleBackColor = true;
            this.SolveButton.Click += new System.EventHandler(this.SolveButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(1211, 191);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(129, 23);
            this.SaveButton.TabIndex = 2;
            this.SaveButton.Text = "Сохранить параметры";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // SolveButtonOneThread
            // 
            this.SolveButtonOneThread.Location = new System.Drawing.Point(1211, 245);
            this.SolveButtonOneThread.Name = "SolveButtonOneThread";
            this.SolveButtonOneThread.Size = new System.Drawing.Size(129, 23);
            this.SolveButtonOneThread.TabIndex = 3;
            this.SolveButtonOneThread.Text = "Элементарная ячейка";
            this.SolveButtonOneThread.UseVisualStyleBackColor = true;
            this.SolveButtonOneThread.Click += new System.EventHandler(this.SolveButtonOneThread_Click);
            // 
            // buttonSaveScreen
            // 
            this.buttonSaveScreen.Location = new System.Drawing.Point(1062, 245);
            this.buttonSaveScreen.Name = "buttonSaveScreen";
            this.buttonSaveScreen.Size = new System.Drawing.Size(129, 23);
            this.buttonSaveScreen.TabIndex = 4;
            this.buttonSaveScreen.Text = "Сохранить экран";
            this.buttonSaveScreen.UseVisualStyleBackColor = true;
            this.buttonSaveScreen.Click += new System.EventHandler(this.buttonSaveScreen_Click);
            // 
            // buttonSaveGradient
            // 
            this.buttonSaveGradient.Location = new System.Drawing.Point(914, 245);
            this.buttonSaveGradient.Name = "buttonSaveGradient";
            this.buttonSaveGradient.Size = new System.Drawing.Size(129, 23);
            this.buttonSaveGradient.TabIndex = 5;
            this.buttonSaveGradient.Text = "Сохранить градиент";
            this.buttonSaveGradient.UseVisualStyleBackColor = true;
            this.buttonSaveGradient.Click += new System.EventHandler(this.buttonSaveGradient_Click);
            // 
            // comboBoxMaterial
            // 
            this.comboBoxMaterial.FormattingEnabled = true;
            this.comboBoxMaterial.Items.AddRange(new object[] {
            "Ni (Nickel)",
            "Au (Gold)",
            "Al (Aluminium)"});
            this.comboBoxMaterial.Location = new System.Drawing.Point(90, 367);
            this.comboBoxMaterial.Name = "comboBoxMaterial";
            this.comboBoxMaterial.Size = new System.Drawing.Size(121, 21);
            this.comboBoxMaterial.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 370);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Материал";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1367, 419);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxMaterial);
            this.Controls.Add(this.buttonSaveGradient);
            this.Controls.Add(this.buttonSaveScreen);
            this.Controls.Add(this.SolveButtonOneThread);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.SolveButton);
            this.Controls.Add(this.ExitButton);
            this.Name = "Form1";
            this.Text = "XRay Lens";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Button SolveButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button SolveButtonOneThread;
        private System.Windows.Forms.Button buttonSaveScreen;
        private System.Windows.Forms.Button buttonSaveGradient;
        private System.Windows.Forms.ComboBox comboBoxMaterial;
        private System.Windows.Forms.Label label1;
    }
}

