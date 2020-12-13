namespace Reversi
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.NewGameButton = new System.Windows.Forms.Button();
            this.BotButton = new System.Windows.Forms.Button();
            this.HelpButton = new System.Windows.Forms.Button();
            this.playerOneIcon = new System.Windows.Forms.PictureBox();
            this.playerTwoIcon = new System.Windows.Forms.PictureBox();
            this.playerOneIconName = new System.Windows.Forms.Label();
            this.playerTwoIconName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.playerOneIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.playerTwoIcon)).BeginInit();
            this.SuspendLayout();

            // 
            // NewGameButton
            // 
            this.NewGameButton.Location = new System.Drawing.Point(253, 116);
            this.NewGameButton.Name = "NewGameButton";
            this.NewGameButton.Size = new System.Drawing.Size(183, 74);
            this.NewGameButton.TabIndex = 1;
            this.NewGameButton.Text = "New 1v1 game";
            this.NewGameButton.UseVisualStyleBackColor = true;
            this.NewGameButton.Click += new System.EventHandler(this.NewGameButton_Click);
            // 
            // BotButton
            // 
            this.BotButton.Location = new System.Drawing.Point(548, 116);
            this.BotButton.Name = "BotButton";
            this.BotButton.Size = new System.Drawing.Size(183, 74);
            this.BotButton.TabIndex = 2;
            this.BotButton.Text = "New 1vBot game";
            this.BotButton.UseVisualStyleBackColor = true;
            this.BotButton.Click += new System.EventHandler(this.BotButton_Click);
            // 
            // HelpButton
            // 
            this.HelpButton.Location = new System.Drawing.Point(881, 116);
            this.HelpButton.Name = "HelpButton";
            this.HelpButton.Size = new System.Drawing.Size(183, 74);
            this.HelpButton.TabIndex = 3;
            this.HelpButton.Text = "Help";
            this.HelpButton.UseVisualStyleBackColor = true;
            this.HelpButton.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // playerOneIcon
            // 
            this.playerOneIcon.Location = new System.Drawing.Point(253, 240);
            this.playerOneIcon.Name = "playerOneIcon";
            this.playerOneIcon.Size = new System.Drawing.Size(100, 100);
            this.playerOneIcon.TabIndex = 4;
            this.playerOneIcon.TabStop = false;
            // 
            // playerTwoIcon
            // 
            this.playerTwoIcon.Location = new System.Drawing.Point(253, 377);
            this.playerTwoIcon.Name = "playerTwoIcon";
            this.playerTwoIcon.Size = new System.Drawing.Size(100, 100);
            this.playerTwoIcon.TabIndex = 5;
            this.playerTwoIcon.TabStop = false;
            // 
            // playerOneIconName
            // 
            this.playerOneIconName.AutoSize = true;
            this.playerOneIconName.Location = new System.Drawing.Point(372, 275);
            this.playerOneIconName.Name = "playerOneIconName";
            this.playerOneIconName.Size = new System.Drawing.Size(92, 25);
            this.playerOneIconName.TabIndex = 6;
            this.playerOneIconName.Text = "Speler 1";
            // 
            // playerTwoIconName
            // 
            this.playerTwoIconName.AutoSize = true;
            this.playerTwoIconName.Location = new System.Drawing.Point(372, 417);
            this.playerTwoIconName.Name = "playerTwoIconName";
            this.playerTwoIconName.Size = new System.Drawing.Size(92, 25);
            this.playerTwoIconName.TabIndex = 7;
            this.playerTwoIconName.Text = "Speler 2";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1360, 1336);
            this.Controls.Add(this.playerTwoIconName);
            this.Controls.Add(this.playerOneIconName);
            this.Controls.Add(this.playerTwoIcon);
            this.Controls.Add(this.playerOneIcon);
            this.Controls.Add(this.HelpButton);
            this.Controls.Add(this.BotButton);
            this.Controls.Add(this.NewGameButton);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainWindow";

            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.playerOneIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.playerTwoIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button NewGameButton;
        private System.Windows.Forms.Button BotButton;
        private System.Windows.Forms.Button HelpButton;
        private System.Windows.Forms.PictureBox playerOneIcon;
        private System.Windows.Forms.PictureBox playerTwoIcon;
        private System.Windows.Forms.Label playerOneIconName;
        private System.Windows.Forms.Label playerTwoIconName;

    }
}

