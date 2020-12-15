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
            this.playerOneStoneCount = new System.Windows.Forms.PictureBox();
            this.playerTwoStoneCount = new System.Windows.Forms.PictureBox();
            this.playerTwoCountText = new System.Windows.Forms.Label();
            this.playerOneCountText = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.playerOneIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.playerTwoIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.playerOneStoneCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.playerTwoStoneCount)).BeginInit();
            this.SuspendLayout();
            // 
            // NewGameButton
            // 
            this.NewGameButton.Location = new System.Drawing.Point(126, 60);
            this.NewGameButton.Margin = new System.Windows.Forms.Padding(2);
            this.NewGameButton.Name = "NewGameButton";
            this.NewGameButton.Size = new System.Drawing.Size(92, 38);
            this.NewGameButton.TabIndex = 1;
            this.NewGameButton.Text = "New 1v1 game";
            this.NewGameButton.UseVisualStyleBackColor = true;
            this.NewGameButton.Click += new System.EventHandler(this.NewGameButton_Click);
            // 
            // BotButton
            // 
            this.BotButton.Location = new System.Drawing.Point(274, 60);
            this.BotButton.Margin = new System.Windows.Forms.Padding(2);
            this.BotButton.Name = "BotButton";
            this.BotButton.Size = new System.Drawing.Size(92, 38);
            this.BotButton.TabIndex = 2;
            this.BotButton.Text = "New 1vBot game";
            this.BotButton.UseVisualStyleBackColor = true;
            this.BotButton.Click += new System.EventHandler(this.BotButton_Click);
            // 
            // HelpButton
            // 
            this.HelpButton.Location = new System.Drawing.Point(440, 60);
            this.HelpButton.Margin = new System.Windows.Forms.Padding(2);
            this.HelpButton.Name = "HelpButton";
            this.HelpButton.Size = new System.Drawing.Size(92, 38);
            this.HelpButton.TabIndex = 3;
            this.HelpButton.Text = "Help";
            this.HelpButton.UseVisualStyleBackColor = true;
            this.HelpButton.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // playerOneIcon
            // 
            this.playerOneIcon.Location = new System.Drawing.Point(30, 122);
            this.playerOneIcon.Margin = new System.Windows.Forms.Padding(2);
            this.playerOneIcon.Name = "playerOneIcon";
            this.playerOneIcon.Size = new System.Drawing.Size(50, 52);
            this.playerOneIcon.TabIndex = 4;
            this.playerOneIcon.TabStop = false;
            // 
            // playerTwoIcon
            // 
            this.playerTwoIcon.Location = new System.Drawing.Point(30, 193);
            this.playerTwoIcon.Margin = new System.Windows.Forms.Padding(2);
            this.playerTwoIcon.Name = "playerTwoIcon";
            this.playerTwoIcon.Size = new System.Drawing.Size(50, 52);
            this.playerTwoIcon.TabIndex = 5;
            this.playerTwoIcon.TabStop = false;
            // 
            // playerOneIconName
            // 
            this.playerOneIconName.AutoSize = true;
            this.playerOneIconName.Location = new System.Drawing.Point(103, 145);
            this.playerOneIconName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.playerOneIconName.Name = "playerOneIconName";
            this.playerOneIconName.Size = new System.Drawing.Size(46, 13);
            this.playerOneIconName.TabIndex = 6;
            this.playerOneIconName.Text = "Speler 1";
            // 
            // playerTwoIconName
            // 
            this.playerTwoIconName.AutoSize = true;
            this.playerTwoIconName.Location = new System.Drawing.Point(103, 213);
            this.playerTwoIconName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.playerTwoIconName.Name = "playerTwoIconName";
            this.playerTwoIconName.Size = new System.Drawing.Size(46, 13);
            this.playerTwoIconName.TabIndex = 7;
            this.playerTwoIconName.Text = "Speler 2";
            // 
            // playerOneStoneCount
            // 
            this.playerOneStoneCount.Location = new System.Drawing.Point(30, 262);
            this.playerOneStoneCount.Margin = new System.Windows.Forms.Padding(2);
            this.playerOneStoneCount.Name = "playerOneStoneCount";
            this.playerOneStoneCount.Size = new System.Drawing.Size(50, 52);
            this.playerOneStoneCount.TabIndex = 8;
            this.playerOneStoneCount.TabStop = false;
            // 
            // playerTwoStoneCount
            // 
            this.playerTwoStoneCount.Location = new System.Drawing.Point(30, 330);
            this.playerTwoStoneCount.Margin = new System.Windows.Forms.Padding(2);
            this.playerTwoStoneCount.Name = "playerTwoStoneCount";
            this.playerTwoStoneCount.Size = new System.Drawing.Size(50, 52);
            this.playerTwoStoneCount.TabIndex = 9;
            this.playerTwoStoneCount.TabStop = false;
            // 
            // playerTwoCountText
            // 
            this.playerTwoCountText.AutoSize = true;
            this.playerTwoCountText.Location = new System.Drawing.Point(103, 345);
            this.playerTwoCountText.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.playerTwoCountText.Name = "playerTwoCountText";
            this.playerTwoCountText.Size = new System.Drawing.Size(23, 13);
            this.playerTwoCountText.TabIndex = 10;
            this.playerTwoCountText.Text = "X 0";
            // 
            // playerOneCountText
            // 
            this.playerOneCountText.AutoSize = true;
            this.playerOneCountText.Location = new System.Drawing.Point(103, 280);
            this.playerOneCountText.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.playerOneCountText.Name = "playerOneCountText";
            this.playerOneCountText.Size = new System.Drawing.Size(23, 13);
            this.playerOneCountText.TabIndex = 11;
            this.playerOneCountText.Text = "X 0";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 665);
            this.Controls.Add(this.playerOneCountText);
            this.Controls.Add(this.playerTwoCountText);
            this.Controls.Add(this.playerTwoStoneCount);
            this.Controls.Add(this.playerOneStoneCount);
            this.Controls.Add(this.playerTwoIconName);
            this.Controls.Add(this.playerOneIconName);
            this.Controls.Add(this.playerTwoIcon);
            this.Controls.Add(this.playerOneIcon);
            this.Controls.Add(this.HelpButton);
            this.Controls.Add(this.BotButton);
            this.Controls.Add(this.NewGameButton);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainWindow";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.playerOneIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.playerTwoIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.playerOneStoneCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.playerTwoStoneCount)).EndInit();
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
        private System.Windows.Forms.PictureBox playerOneStoneCount;
        private System.Windows.Forms.PictureBox playerTwoStoneCount;
        private System.Windows.Forms.Label playerTwoCountText;
        private System.Windows.Forms.Label playerOneCountText;
    }
}

