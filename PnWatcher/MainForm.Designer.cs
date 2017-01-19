namespace PnWatcher
{
    partial class MainForm
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.startBtn = new System.Windows.Forms.Button();
            this.pauseBtn = new System.Windows.Forms.Button();
            this.statusTxt = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // startBtn
            // 
            this.startBtn.Image = global::PnWatcher.Properties.Resources.play_icon__1_;
            this.startBtn.Location = new System.Drawing.Point(0, 10);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(95, 95);
            this.startBtn.TabIndex = 0;
            this.startBtn.UseVisualStyleBackColor = true;
            this.startBtn.Visible = false;
            // 
            // pauseBtn
            // 
            this.pauseBtn.Image = global::PnWatcher.Properties.Resources.pause_icon;
            this.pauseBtn.Location = new System.Drawing.Point(0, 10);
            this.pauseBtn.Name = "pauseBtn";
            this.pauseBtn.Size = new System.Drawing.Size(95, 95);
            this.pauseBtn.TabIndex = 1;
            this.pauseBtn.UseVisualStyleBackColor = true;
            this.pauseBtn.Visible = false;
            // 
            // statusTxt
            // 
            this.statusTxt.Location = new System.Drawing.Point(113, 10);
            this.statusTxt.Multiline = true;
            this.statusTxt.Name = "statusTxt";
            this.statusTxt.ReadOnly = true;
            this.statusTxt.Size = new System.Drawing.Size(594, 98);
            this.statusTxt.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(718, 128);
            this.Controls.Add(this.statusTxt);
            this.Controls.Add(this.pauseBtn);
            this.Controls.Add(this.startBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "PN File Watcher";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startBtn;
        private System.Windows.Forms.Button pauseBtn;
        private System.Windows.Forms.TextBox statusTxt;
    }
}

